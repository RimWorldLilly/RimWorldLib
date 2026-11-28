using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200164B RID: 5707
	public class Precept_Relic : Precept_ThingStyle
	{
		// Token: 0x170018BE RID: 6334
		// (get) Token: 0x06008F39 RID: 36665 RVA: 0x0034A5EC File Offset: 0x003487EC
		public override string TipLabel
		{
			get
			{
				if (this.tipLabelCached == null)
				{
					this.tipLabelCached = base.LabelCap + "\n" + base.ThingDef.LabelCap;
				}
				return this.tipLabelCached;
			}
		}

		// Token: 0x170018BF RID: 6335
		// (get) Token: 0x06008F3A RID: 36666 RVA: 0x0034A627 File Offset: 0x00348827
		public override bool CanRegenerate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170018C0 RID: 6336
		// (get) Token: 0x06008F3B RID: 36667 RVA: 0x0034A62A File Offset: 0x0034882A
		public bool CanGenerateRelic
		{
			get
			{
				return !this.relicGenerated;
			}
		}

		// Token: 0x170018C1 RID: 6337
		// (get) Token: 0x06008F3C RID: 36668 RVA: 0x0034A635 File Offset: 0x00348835
		public Thing GeneratedRelic
		{
			get
			{
				return this.generatedRelic;
			}
		}

		// Token: 0x170018C2 RID: 6338
		// (get) Token: 0x06008F3D RID: 36669 RVA: 0x0034A63D File Offset: 0x0034883D
		protected override string ThingLabelCap
		{
			get
			{
				return Find.ActiveLanguageWorker.PostProcessThingLabelForRelic(base.ThingLabelCap);
			}
		}

		// Token: 0x170018C3 RID: 6339
		// (get) Token: 0x06008F3E RID: 36670 RVA: 0x0034A64F File Offset: 0x0034884F
		public override string DescriptionForTip
		{
			get
			{
				ThingDef thingDef = base.ThingDef;
				return ((thingDef != null) ? thingDef.description : null) ?? base.Description;
			}
		}

		// Token: 0x170018C4 RID: 6340
		// (get) Token: 0x06008F3F RID: 36671 RVA: 0x0034A670 File Offset: 0x00348870
		public bool RelicInPlayerPossession
		{
			get
			{
				Thing thing = this.GeneratedRelic;
				return thing != null && !thing.Destroyed && thing.EverSeenByPlayer && (thing.MapHeld != null || ThingOwnerUtility.AnyParentIs<Caravan>(thing) || ThingOwnerUtility.AnyParentIs<TravellingTransporters>(thing));
			}
		}

		// Token: 0x170018C5 RID: 6341
		// (get) Token: 0x06008F40 RID: 36672 RVA: 0x0034A6B2 File Offset: 0x003488B2
		protected override string NameRootSymbol
		{
			get
			{
				return "r_ideoRelicName";
			}
		}

		// Token: 0x06008F41 RID: 36673 RVA: 0x0034A6BC File Offset: 0x003488BC
		public static ThingDef GenerateStuffFor(ThingDef thing, Ideo ideo = null)
		{
			IEnumerable<ThingDef> source = GenStuff.AllowedStuffsFor(thing, TechLevel.Undefined, false);
			if (ideo != null)
			{
				IEnumerable<ThingDef> alreadyUsedStuffs = (from p in ideo.PreceptsListForReading.Where(delegate(Precept p)
				{
					Precept_Relic precept_Relic = p as Precept_Relic;
					return precept_Relic != null && precept_Relic.ThingDef == thing;
				})
				select ((Precept_Relic)p).stuff into p
				where p != null
				select p).Distinct<ThingDef>();
				source = from stuff in source
				where !alreadyUsedStuffs.Contains(stuff)
				select stuff;
			}
			return source.RandomElementByWeight((ThingDef stuff) => stuff.BaseMarketValue);
		}

		// Token: 0x06008F42 RID: 36674 RVA: 0x0034A792 File Offset: 0x00348992
		public void SetRandomStuff()
		{
			if (base.ThingDef.MadeFromStuff)
			{
				this.stuff = Precept_Relic.GenerateStuffFor(base.ThingDef, this.ideo);
				return;
			}
			this.stuff = null;
		}

		// Token: 0x06008F43 RID: 36675 RVA: 0x0034A7C0 File Offset: 0x003489C0
		protected override void Notify_ThingDefSet()
		{
			base.Notify_ThingDefSet();
			this.SetRandomStuff();
		}

		// Token: 0x06008F44 RID: 36676 RVA: 0x0034A7D0 File Offset: 0x003489D0
		public override string GenerateNameRaw()
		{
			CompProperties_GeneratedName compProperties = base.ThingDef.GetCompProperties<CompProperties_GeneratedName>();
			if (compProperties != null)
			{
				return CompGeneratedNames.GenerateName(compProperties);
			}
			return base.GenerateNameRaw();
		}

		// Token: 0x06008F45 RID: 36677 RVA: 0x0034A7FC File Offset: 0x003489FC
		public Thing GenerateRelic()
		{
			if (this.stuff == null && base.ThingDef.MadeFromStuff)
			{
				Log.Warning("Tried generating relic with stuff, but precept had no stuff set? If this is an old savegame from testing this warning is to be expected.");
				this.stuff = Precept_Relic.GenerateStuffFor(base.ThingDef, this.ideo);
			}
			Thing thing;
			if (base.ThingDef.CompDefFor<CompQuality>() != null)
			{
				ThingStuffPairWithQuality thingStuffPairWithQuality = new ThingStuffPairWithQuality(base.ThingDef, this.stuff, QualityCategory.Legendary);
				thing = thingStuffPairWithQuality.MakeThing(true);
			}
			else
			{
				thing = ThingMaker.MakeThing(base.ThingDef, this.stuff);
			}
			thing.StyleSourcePrecept = this;
			this.relicGenerated = true;
			this.generatedRelic = thing;
			return thing;
		}

		// Token: 0x06008F46 RID: 36678 RVA: 0x0034A893 File Offset: 0x00348A93
		public override void ClearTipCache()
		{
			base.ClearTipCache();
			this.tipLabelCached = null;
		}

		// Token: 0x06008F47 RID: 36679 RVA: 0x0034A8A2 File Offset: 0x00348AA2
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			Precept_Relic.<EditFloatMenuOptions>d__27 <EditFloatMenuOptions>d__ = new Precept_Relic.<EditFloatMenuOptions>d__27(-2);
			<EditFloatMenuOptions>d__.<>4__this = this;
			return <EditFloatMenuOptions>d__;
		}

		// Token: 0x06008F48 RID: 36680 RVA: 0x0034A8B2 File Offset: 0x00348AB2
		public override string GetTip()
		{
			return base.GetTip() + "\n\n" + (this.ideo.classicMode ? "RelicTipClassic" : "RelicTip").Translate().Colorize(Color.grey);
		}

		// Token: 0x06008F49 RID: 36681 RVA: 0x0034A8EC File Offset: 0x00348AEC
		public override void DrawIcon(Rect rect)
		{
			Widgets.DefIcon(rect, base.ThingDef, this.stuff, 1f, this.ideo.GetStyleFor(base.ThingDef), false, null, null, null, 1f);
		}

		// Token: 0x06008F4A RID: 36682 RVA: 0x0034A93C File Offset: 0x00348B3C
		public override void Notify_ThingLost(Thing thing, bool destroyed = false)
		{
			if (thing.def == base.ThingDef && !QuestPart_NewColony.IsGeneratingNewColony)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(destroyed ? HistoryEventDefOf.RelicDestroyed : HistoryEventDefOf.RelicLost, thing.Named("SUBJECT")), true);
				List<Faction> list = new List<Faction>();
				HashSet<Pawn> hashSet = new HashSet<Pawn>();
				if (thing.EverSeenByPlayer)
				{
					list.Add(Faction.OfPlayer);
				}
				if (thing.MapHeld != null)
				{
					foreach (Pawn pawn in thing.MapHeld.mapPawns.AllPawnsSpawned)
					{
						if (!pawn.Dead && pawn.Faction != null && !list.Contains(pawn.Faction))
						{
							list.Add(pawn.Faction);
						}
					}
				}
				foreach (Pawn pawn2 in PawnsFinder.AllMapsAndWorld_Alive)
				{
					if (pawn2.needs != null && pawn2.needs.mood != null && pawn2.needs.mood.thoughts != null && pawn2.needs.mood.thoughts.memories != null && !hashSet.Contains(pawn2) && list.Contains(pawn2.Faction))
					{
						pawn2.needs.mood.thoughts.memories.TryGainMemory(destroyed ? ThoughtDefOf.RelicDestroyed : ThoughtDefOf.RelicLost, null, null);
						if (pawn2.Faction == Faction.OfPlayer)
						{
							hashSet.Add(pawn2);
						}
					}
				}
				if (list.Contains(Faction.OfPlayer) && (destroyed || thing.MapHeld == null || !thing.MapHeld.IsPlayerHome))
				{
					TaggedString label = (destroyed ? "LetterLabelRelicDestroyed" : "LetterLabelRelicLost").Translate() + ": " + base.LabelCap;
					TaggedString text = (destroyed ? "LetterTextRelicDestroyed" : "LetterTextRelicLost").Translate(base.LabelCap, (from p in hashSet
					select p.LabelNoCountColored.Resolve()).ToList<string>().ToLineList("- "));
					Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NegativeEvent, new LookTargets(from p in hashSet
					where p.Faction == Faction.OfPlayer
					select p), null, null, null, null, 0, true);
				}
			}
		}

		// Token: 0x06008F4B RID: 36683 RVA: 0x0034ABFC File Offset: 0x00348DFC
		public void Notify_NewColonyStarted()
		{
			if (this.relicGenerated && !this.RelicInPlayerPossession)
			{
				this.relicGenerated = false;
			}
		}

		// Token: 0x06008F4C RID: 36684 RVA: 0x0034AC15 File Offset: 0x00348E15
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(Thing thing)
		{
			Precept_Relic.<SpecialDisplayStats>d__32 <SpecialDisplayStats>d__ = new Precept_Relic.<SpecialDisplayStats>d__32(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x06008F4D RID: 36685 RVA: 0x0034AC25 File Offset: 0x00348E25
		public override string TransformThingLabel(string label)
		{
			return this.name + ", " + "Relic".Translate();
		}

		// Token: 0x06008F4E RID: 36686 RVA: 0x0034AC4C File Offset: 0x00348E4C
		public override string InspectStringExtra(Thing thing)
		{
			if (this.ideo.classicMode)
			{
				return "Relic".Translate().CapitalizeFirst();
			}
			return "RelicOf".Translate(this.ideo.Named("IDEO")).Resolve();
		}

		// Token: 0x06008F4F RID: 36687 RVA: 0x0034ACA0 File Offset: 0x00348EA0
		private IEnumerable<Quest> GetQuestsToDeactivateIfLost()
		{
			Precept_Relic.<GetQuestsToDeactivateIfLost>d__35 <GetQuestsToDeactivateIfLost>d__ = new Precept_Relic.<GetQuestsToDeactivateIfLost>d__35(-2);
			<GetQuestsToDeactivateIfLost>d__.<>4__this = this;
			return <GetQuestsToDeactivateIfLost>d__;
		}

		// Token: 0x06008F50 RID: 36688 RVA: 0x0034ACB0 File Offset: 0x00348EB0
		public override bool TryGetLostByReformingWarning(out string warning)
		{
			warning = null;
			IEnumerable<Quest> questsToDeactivateIfLost = this.GetQuestsToDeactivateIfLost();
			if (questsToDeactivateIfLost.Any<Quest>())
			{
				foreach (Quest quest in questsToDeactivateIfLost)
				{
					if (!warning.NullOrEmpty())
					{
						warning += "\n\n";
					}
					warning += "ReformIdeoRelicHuntQuestEnd".Translate(base.LabelCap, quest.name);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06008F51 RID: 36689 RVA: 0x0034AD4C File Offset: 0x00348F4C
		public override void Notify_RemovedByReforming()
		{
			foreach (Quest quest in this.GetQuestsToDeactivateIfLost())
			{
				quest.hidden = true;
				quest.End(QuestEndOutcome.Unknown, false, true);
			}
		}

		// Token: 0x06008F52 RID: 36690 RVA: 0x0034ADA0 File Offset: 0x00348FA0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			if (!GameDataSaveLoader.IsSavingOrLoadingExternalIdeo)
			{
				if (Scribe.mode == LoadSaveMode.Saving && this.relicGenerated && this.generatedRelic != null && (this.generatedRelic.Destroyed || this.generatedRelic.Discarded))
				{
					this.relicGenerated = false;
					this.generatedRelic = null;
				}
				Scribe_Values.Look<bool>(ref this.relicGenerated, "everGenerated", false, false);
				Scribe_References.Look<Thing>(ref this.generatedRelic, "generatedRelic", false);
			}
		}

		// Token: 0x06008F53 RID: 36691 RVA: 0x0034AE2D File Offset: 0x0034902D
		public override void CopyTo(Precept other)
		{
			base.CopyTo(other);
			Precept_Relic precept_Relic = (Precept_Relic)other;
			precept_Relic.stuff = this.stuff;
			precept_Relic.relicGenerated = this.relicGenerated;
			precept_Relic.generatedRelic = this.generatedRelic;
		}

		// Token: 0x06008F54 RID: 36692 RVA: 0x0034AE5F File Offset: 0x0034905F
		public override string ToString()
		{
			return "Relic - " + base.LabelCap;
		}

		// Token: 0x0400578F RID: 22415
		private static List<ThingDef> usedThingsTmp = new List<ThingDef>();

		// Token: 0x04005790 RID: 22416
		public ThingDef stuff;

		// Token: 0x04005791 RID: 22417
		private bool relicGenerated;

		// Token: 0x04005792 RID: 22418
		private Thing generatedRelic;

		// Token: 0x04005793 RID: 22419
		private string tipLabelCached;
	}
}
