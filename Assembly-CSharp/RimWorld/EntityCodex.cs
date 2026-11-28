using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001025 RID: 4133
	public sealed class EntityCodex : IExposable
	{
		// Token: 0x06006817 RID: 26647 RVA: 0x0024C918 File Offset: 0x0024AB18
		public EntityCodex()
		{
			this.hiddenCategories = new Dictionary<EntityCategoryDef, bool>();
			this.hiddenEntries = new Dictionary<EntityCodexEntryDef, bool>();
			this.discoveredEntities = new HashSet<ThingDef>();
			foreach (EntityCategoryDef key in DefDatabase<EntityCategoryDef>.AllDefs)
			{
				this.hiddenCategories.Add(key, true);
			}
		}

		// Token: 0x06006818 RID: 26648 RVA: 0x0024C994 File Offset: 0x0024AB94
		public static int EntryCountInCategory(EntityCategoryDef def)
		{
			int num = 0;
			using (IEnumerator<EntityCodexEntryDef> enumerator = DefDatabase<EntityCodexEntryDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.category == def)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06006819 RID: 26649 RVA: 0x0024C9E8 File Offset: 0x0024ABE8
		public int DiscoveredCount(EntityCategoryDef def)
		{
			int num = 0;
			foreach (EntityCodexEntryDef entityCodexEntryDef in DefDatabase<EntityCodexEntryDef>.AllDefs)
			{
				if (entityCodexEntryDef.category == def && Find.EntityCodex.Discovered(entityCodexEntryDef))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600681A RID: 26650 RVA: 0x0024CA4C File Offset: 0x0024AC4C
		public bool Hidden(ResearchProjectDef def)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (this.debug_UnhideAllResearch)
			{
				return false;
			}
			if (def.knowledgeCategory != null && Find.Anomaly.HighestLevelReached < 1 && Find.Anomaly.GenerateMonolith)
			{
				return true;
			}
			foreach (EntityCodexEntryDef entityCodexEntryDef in DefDatabase<EntityCodexEntryDef>.AllDefs)
			{
				if (entityCodexEntryDef.discoveredResearchProjects.NotNullAndContains(def))
				{
					return !this.Discovered(entityCodexEntryDef);
				}
			}
			return false;
		}

		// Token: 0x0600681B RID: 26651 RVA: 0x0024CAE8 File Offset: 0x0024ACE8
		public bool Discovered(EntityCategoryDef def)
		{
			bool flag;
			return ModLister.CheckAnomaly("Entity codex") && this.hiddenCategories.TryGetValue(def, out flag) && !flag;
		}

		// Token: 0x0600681C RID: 26652 RVA: 0x0024CB1C File Offset: 0x0024AD1C
		public bool Discovered(EntityCodexEntryDef entry)
		{
			bool flag;
			return ModLister.CheckAnomaly("Entity codex") && this.Discovered(entry.category) && this.hiddenEntries.TryGetValue(entry, out flag) && !flag;
		}

		// Token: 0x0600681D RID: 26653 RVA: 0x0024CB60 File Offset: 0x0024AD60
		public void SetDiscovered(List<EntityCodexEntryDef> entries, ThingDef discoveredDef = null, Thing discoveredThing = null)
		{
			foreach (EntityCodexEntryDef entry in entries)
			{
				this.SetDiscovered(entry, discoveredDef, discoveredThing);
			}
			Find.HiddenItemsManager.SetDiscovered(discoveredDef);
		}

		// Token: 0x0600681E RID: 26654 RVA: 0x0024CBBC File Offset: 0x0024ADBC
		public void SetDiscovered(EntityCodexEntryDef entry, ThingDef discoveredDef = null, Thing discoveredThing = null)
		{
			if (!ModLister.CheckAnomaly("Entity codex"))
			{
				return;
			}
			if (discoveredDef != null)
			{
				this.discoveredEntities.Add(discoveredDef);
			}
			if (this.Discovered(entry))
			{
				return;
			}
			this.hiddenEntries[entry] = false;
			this.hiddenCategories[entry.category] = false;
			Find.SignalManager.SendSignal(new Signal("EntityDiscovered", true));
			if (Current.ProgramState == ProgramState.Playing && Find.Anomaly.AnomalyStudyEnabled)
			{
				if (discoveredThing != null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("MessageEntityDiscovered".Translate());
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(entry.LabelCap);
					Messages.Message(defaultInterpolatedStringHandler.ToStringAndClear(), discoveredThing, MessageTypeDefOf.PositiveEvent, true);
				}
				if (!entry.discoveredResearchProjects.NullOrEmpty<ResearchProjectDef>())
				{
					string text = string.Empty;
					if (!entry.discoveredResearchProjects.NullOrEmpty<ResearchProjectDef>())
					{
						string text2;
						if ((text2 = ((discoveredThing != null) ? discoveredThing.LabelShort : null)) == null)
						{
							text2 = (((discoveredDef != null) ? discoveredDef.label : null) ?? entry.label);
						}
						string arg = text2;
						Pawn pawn = discoveredThing as Pawn;
						if (pawn != null && pawn.IsMutant)
						{
							arg = pawn.mutant.Def.label;
						}
						text += "LetterTextEntityDiscoveryResearch".Translate(arg.Named("ENTITY")) + ":\n" + (from r in entry.discoveredResearchProjects
						select r.LabelCap).ToLineList("  - ", false);
					}
					if (!text.NullOrEmpty())
					{
						ChoiceLetter_EntityDiscovered choiceLetter_EntityDiscovered = (ChoiceLetter_EntityDiscovered)LetterMaker.MakeLetter("LetterLabelEntityDiscovery".Translate(), text, LetterDefOf.EntityDiscovered, null, null);
						choiceLetter_EntityDiscovered.codexEntry = entry;
						if (discoveredThing != null)
						{
							choiceLetter_EntityDiscovered.lookTargets = discoveredThing;
						}
						Find.LetterStack.ReceiveLetter(choiceLetter_EntityDiscovered, null, 600, true);
					}
				}
				if (Find.Anomaly.GenerateMonolith && Find.Anomaly.monolith != null)
				{
					MonolithLevelDef levelDef = Find.Anomaly.LevelDef;
					string text3;
					string text4;
					if (levelDef.level > Find.Anomaly.lastLevelActivationLetterSent && Find.Anomaly.monolith.CanActivate(out text3, out text4) && !levelDef.activatableLetterLabel.NullOrEmpty() && !levelDef.activatableLetterText.NullOrEmpty())
					{
						Find.Anomaly.lastLevelActivationLetterSent = levelDef.level;
						Find.LetterStack.ReceiveLetter(levelDef.activatableLetterLabel, levelDef.activatableLetterText, LetterDefOf.PositiveEvent, Find.Anomaly.monolith, null, null, null, null, 900, true);
					}
				}
			}
		}

		// Token: 0x0600681F RID: 26655 RVA: 0x0024CE79 File Offset: 0x0024B079
		public bool Discovered(ThingDef def)
		{
			return ModLister.CheckAnomaly("Entity codex") && this.discoveredEntities.Contains(def);
		}

		// Token: 0x06006820 RID: 26656 RVA: 0x0024CE98 File Offset: 0x0024B098
		public void Debug_DiscoverAll()
		{
			foreach (EntityCodexEntryDef entry in DefDatabase<EntityCodexEntryDef>.AllDefs)
			{
				this.SetDiscovered(entry, null, null);
			}
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.entityCodexEntry != null)
				{
					this.discoveredEntities.Add(thingDef);
				}
			}
			foreach (EntityCategoryDef key in DefDatabase<EntityCategoryDef>.AllDefs)
			{
				this.hiddenCategories[key] = false;
			}
		}

		// Token: 0x06006821 RID: 26657 RVA: 0x0024CF78 File Offset: 0x0024B178
		public void Reset()
		{
			this.hiddenCategories.Clear();
			this.hiddenEntries.Clear();
			this.discoveredEntities.Clear();
			foreach (EntityCategoryDef key in DefDatabase<EntityCategoryDef>.AllDefs)
			{
				this.hiddenCategories.Add(key, true);
			}
			foreach (EntityCodexEntryDef entityCodexEntryDef in DefDatabase<EntityCodexEntryDef>.AllDefs)
			{
				this.hiddenEntries.Add(entityCodexEntryDef, !entityCodexEntryDef.startDiscovered);
			}
		}

		// Token: 0x06006822 RID: 26658 RVA: 0x0024D034 File Offset: 0x0024B234
		public void ExposeData()
		{
			Scribe_Collections.Look<EntityCategoryDef, bool>(ref this.hiddenCategories, "hiddenCategories", LookMode.Def, LookMode.Value);
			Scribe_Collections.Look<EntityCodexEntryDef, bool>(ref this.hiddenEntries, "hiddenEntries", LookMode.Def, LookMode.Value);
			Scribe_Collections.Look<ThingDef>(ref this.discoveredEntities, "discoveredEntities", LookMode.Def);
			Scribe_Values.Look<bool>(ref this.debug_UnhideAllResearch, "debug_UnhideAllResearch", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.discoveredEntities == null)
				{
					this.discoveredEntities = new HashSet<ThingDef>();
				}
				Dictionary<EntityCategoryDef, bool> dictionary = new Dictionary<EntityCategoryDef, bool>();
				foreach (EntityCategoryDef key in DefDatabase<EntityCategoryDef>.AllDefs)
				{
					dictionary.Add(key, true);
				}
				foreach (KeyValuePair<EntityCategoryDef, bool> keyValuePair in this.hiddenCategories)
				{
					if (dictionary.ContainsKey(keyValuePair.Key))
					{
						dictionary[keyValuePair.Key] = keyValuePair.Value;
					}
				}
				this.hiddenCategories = dictionary;
				Dictionary<EntityCodexEntryDef, bool> dictionary2 = new Dictionary<EntityCodexEntryDef, bool>();
				foreach (EntityCodexEntryDef entityCodexEntryDef in DefDatabase<EntityCodexEntryDef>.AllDefs)
				{
					dictionary2.Add(entityCodexEntryDef, !entityCodexEntryDef.startDiscovered);
				}
				foreach (KeyValuePair<EntityCodexEntryDef, bool> keyValuePair2 in this.hiddenEntries)
				{
					if (dictionary2.ContainsKey(keyValuePair2.Key))
					{
						dictionary2[keyValuePair2.Key] = keyValuePair2.Value;
					}
				}
				this.hiddenEntries = dictionary2;
				foreach (ThingDef discovered in this.discoveredEntities)
				{
					Find.HiddenItemsManager.SetDiscovered(discovered);
				}
			}
		}

		// Token: 0x0400424F RID: 16975
		private Dictionary<EntityCategoryDef, bool> hiddenCategories;

		// Token: 0x04004250 RID: 16976
		private Dictionary<EntityCodexEntryDef, bool> hiddenEntries;

		// Token: 0x04004251 RID: 16977
		private HashSet<ThingDef> discoveredEntities;

		// Token: 0x04004252 RID: 16978
		public bool debug_UnhideAllResearch;

		// Token: 0x04004253 RID: 16979
		public const string EntityDiscoveredSignal = "EntityDiscovered";

		// Token: 0x04004254 RID: 16980
		private const int EntityDiscoveredLetterDelayTicks = 600;

		// Token: 0x04004255 RID: 16981
		private const int MonolithInteractableLetterDelayTicks = 900;
	}
}
