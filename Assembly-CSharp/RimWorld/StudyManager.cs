using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011FE RID: 4606
	public sealed class StudyManager : IExposable
	{
		// Token: 0x06007276 RID: 29302 RVA: 0x00281234 File Offset: 0x0027F434
		public void UpdateStudiableCache(Thing thing, Map map)
		{
			if (map == null)
			{
				return;
			}
			if (!this.studiableThingsCache.ContainsKey(map))
			{
				this.studiableThingsCache.Add(map, new HashSet<Thing>());
			}
			if (thing.def.IsStudiable)
			{
				CompStudiable compStudiable = thing.TryGetComp<CompStudiable>();
				if (compStudiable != null && compStudiable.EverStudiableCached())
				{
					goto IL_5D;
				}
			}
			if (ModsConfig.AnomalyActive)
			{
				Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
				if (building_HoldingPlatform != null && building_HoldingPlatform.HeldPawn != null)
				{
					goto IL_5D;
				}
			}
			if (!this.studiableThingsCache[map].Contains(thing))
			{
				return;
			}
			this.studiableThingsCache[map].Remove(thing);
			if (this.studiableThingsCache[map].NullOrEmpty<Thing>())
			{
				this.studiableThingsCache.Remove(map);
				return;
			}
			return;
			IL_5D:
			if (!this.studiableThingsCache[map].Contains(thing))
			{
				this.studiableThingsCache[map].Add(thing);
				return;
			}
		}

		// Token: 0x06007277 RID: 29303 RVA: 0x0028130D File Offset: 0x0027F50D
		public HashSet<Thing> GetStudiableThingsAndPlatforms(Map map)
		{
			if (!this.studiableThingsCache.ContainsKey(map))
			{
				return new HashSet<Thing>();
			}
			return this.studiableThingsCache[map];
		}

		// Token: 0x06007278 RID: 29304 RVA: 0x00281330 File Offset: 0x0027F530
		public void Study(Thing studiedThing, Pawn studier, float studyAmount)
		{
			if (!studiedThing.def.IsStudiable)
			{
				Log.Error("Tried to study " + studiedThing.def.label + " which is not studiable.");
				return;
			}
			CompStudiable compStudiable = studiedThing.TryGetComp<CompStudiable>();
			compStudiable.studyPoints += studyAmount;
			if (compStudiable.Props.studyAmountToComplete > 0f && compStudiable.studyPoints >= compStudiable.Props.studyAmountToComplete)
			{
				compStudiable.studyPoints = compStudiable.Props.studyAmountToComplete;
			}
			studiedThing.Notify_Studied(studier, studyAmount, null);
			Find.SignalManager.SendSignal(new Signal("ThingStudied", true));
		}

		// Token: 0x06007279 RID: 29305 RVA: 0x002813D4 File Offset: 0x0027F5D4
		public void StudyAnomaly(Thing studiedThing, Pawn studier, float knowledgeAmount, KnowledgeCategoryDef knowledgeCategory)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			if (knowledgeAmount <= 0f)
			{
				return;
			}
			Thing thing = studiedThing;
			if (thing.Map == null)
			{
				thing = (thing.ParentHolder as Thing);
			}
			if (thing != null)
			{
				Vector3 drawPos = thing.DrawPos;
				Map map = thing.Map;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(knowledgeCategory.LabelCap);
				defaultInterpolatedStringHandler.AppendLiteral(" +");
				defaultInterpolatedStringHandler.AppendFormatted<float>(knowledgeAmount, "0.00");
				MoteMaker.ThrowText(drawPos, map, defaultInterpolatedStringHandler.ToStringAndClear(), 3f);
			}
			Find.ResearchManager.ApplyKnowledge(knowledgeCategory, knowledgeAmount);
			studiedThing.Notify_Studied(studier, knowledgeAmount, knowledgeCategory);
			Find.SignalManager.SendSignal(new Signal("ThingStudied", true));
		}

		// Token: 0x0600727A RID: 29306 RVA: 0x00281484 File Offset: 0x0027F684
		public float GetKnowledgeAmount(KnowledgeCategoryDef knowledgeCategory)
		{
			float num = 0f;
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
			{
				if (researchProjectDef.knowledgeCategory == knowledgeCategory)
				{
					num += Find.ResearchManager.GetKnowledge(researchProjectDef);
				}
			}
			return num;
		}

		// Token: 0x0600727B RID: 29307 RVA: 0x002814F0 File Offset: 0x0027F6F0
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Dictionary<ThingDef, float> dictionary = new Dictionary<ThingDef, float>();
				Scribe_Collections.Look<ThingDef, float>(ref dictionary, "studyProgress", LookMode.Def, LookMode.Value);
				if (dictionary != null)
				{
					foreach (ThingDef thingDef in dictionary.Keys)
					{
						if (thingDef.GetCompProperties<CompProperties_Studiable>() != null)
						{
							this.backCompatStudyProgress[thingDef] = dictionary[thingDef];
						}
						CompProperties_CompAnalyzableUnlockResearch compProperties = thingDef.GetCompProperties<CompProperties_CompAnalyzableUnlockResearch>();
						if (compProperties != null && dictionary[thingDef] >= 1f)
						{
							AnalysisManager analysisManager = Find.AnalysisManager;
							if (analysisManager != null)
							{
								analysisManager.ForceCompleteAnalysisProgress(compProperties.analysisID);
							}
						}
					}
				}
			}
		}

		// Token: 0x04004870 RID: 18544
		public const float DefaultStudyPerTick = 0.08f;

		// Token: 0x04004871 RID: 18545
		public const float DefaultStudyPerInteraction = 0.87f;

		// Token: 0x04004872 RID: 18546
		public const int DefaultStudyInteractions = 5;

		// Token: 0x04004873 RID: 18547
		public const string ThingStudiedSignal = "ThingStudied";

		// Token: 0x04004874 RID: 18548
		private Dictionary<Map, HashSet<Thing>> studiableThingsCache = new Dictionary<Map, HashSet<Thing>>();

		// Token: 0x04004875 RID: 18549
		public Dictionary<ThingDef, float> backCompatStudyProgress = new Dictionary<ThingDef, float>();
	}
}
