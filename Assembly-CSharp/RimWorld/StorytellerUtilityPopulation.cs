using System;
using System.Collections.Generic;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011F6 RID: 4598
	public static class StorytellerUtilityPopulation
	{
		// Token: 0x1700135F RID: 4959
		// (get) Token: 0x06007244 RID: 29252 RVA: 0x002803FD File Offset: 0x0027E5FD
		private static StorytellerDef StorytellerDef
		{
			get
			{
				return Find.Storyteller.def;
			}
		}

		// Token: 0x17001360 RID: 4960
		// (get) Token: 0x06007245 RID: 29253 RVA: 0x00280409 File Offset: 0x0027E609
		public static float PopulationIntent
		{
			get
			{
				return StorytellerUtilityPopulation.CalculatePopulationIntent(StorytellerUtilityPopulation.StorytellerDef, StorytellerUtilityPopulation.AdjustedPopulation, Find.StoryWatcher.watcherPopAdaptation.AdaptDays);
			}
		}

		// Token: 0x17001361 RID: 4961
		// (get) Token: 0x06007246 RID: 29254 RVA: 0x00280429 File Offset: 0x0027E629
		public static float PopulationIntentForQuest
		{
			get
			{
				return StorytellerUtilityPopulation.CalculatePopulationIntent(StorytellerUtilityPopulation.StorytellerDef, StorytellerUtilityPopulation.AdjustedPopulationIncludingQuests, Find.StoryWatcher.watcherPopAdaptation.AdaptDays);
			}
		}

		// Token: 0x17001362 RID: 4962
		// (get) Token: 0x06007247 RID: 29255 RVA: 0x0028044C File Offset: 0x0027E64C
		public static float AdjustedPopulation
		{
			get
			{
				float num = 0f;
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive)
				{
					if (pawn.IsColonist || pawn.IsPrisonerOfColony || pawn.IsSlaveOfColony)
					{
						num += StorytellerUtilityPopulation.AdjustedPopulationValue(pawn);
					}
				}
				num += (float)QuestUtility.TotalBorrowedColonistCount();
				return num;
			}
		}

		// Token: 0x17001363 RID: 4963
		// (get) Token: 0x06007248 RID: 29256 RVA: 0x002804C8 File Offset: 0x0027E6C8
		public static float AdjustedPopulationIncludingQuests
		{
			get
			{
				float num = StorytellerUtilityPopulation.AdjustedPopulation;
				List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
				for (int i = 0; i < questsListForReading.Count; i++)
				{
					if (!questsListForReading[i].Historical && questsListForReading[i].IncreasesPopulation)
					{
						num += 1f;
					}
				}
				return num;
			}
		}

		// Token: 0x06007249 RID: 29257 RVA: 0x0028051C File Offset: 0x0027E71C
		private static float AdjustedPopulationValue(Pawn pawn)
		{
			if (pawn.DevelopmentalStage.Baby())
			{
				return 0f;
			}
			float num = pawn.DevelopmentalStage.Adult() ? 1f : Mathf.Lerp(StorytellerUtilityPopulation.PopulationValue_Child, 1f, (float)pawn.ageTracker.AgeBiologicalYears / pawn.ageTracker.AdultMinAge);
			if (pawn.IsPrisonerOfColony)
			{
				if (pawn.guest.Recruitable)
				{
					num *= StorytellerUtilityPopulation.PopulationValue_Prisoner;
				}
				else
				{
					num *= StorytellerUtilityPopulation.PopulationValue_PrisonerUnrecruitable;
				}
			}
			else if (pawn.IsSlaveOfColony)
			{
				num *= StorytellerUtilityPopulation.PopulationValue_Slave;
			}
			return num;
		}

		// Token: 0x0600724A RID: 29258 RVA: 0x002805B4 File Offset: 0x0027E7B4
		private static float CalculatePopulationIntent(StorytellerDef def, float curPop, float popAdaptation)
		{
			float num = def.populationIntentFactorFromPopCurve.Evaluate(curPop);
			if (num > 0f)
			{
				num *= def.populationIntentFactorFromPopAdaptDaysCurve.Evaluate(popAdaptation);
			}
			return num;
		}

		// Token: 0x0600724B RID: 29259 RVA: 0x002805E8 File Offset: 0x0027E7E8
		public static string DebugReadout()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Population intent: ".PadRight(40) + StorytellerUtilityPopulation.PopulationIntent.ToString("F2"));
			stringBuilder.AppendLine("Population intent for quest: ".PadRight(40) + StorytellerUtilityPopulation.PopulationIntentForQuest.ToString("F2"));
			stringBuilder.AppendLine("Chance random quest increases population: ".PadRight(40) + NaturalRandomQuestChooser.PopulationIncreasingQuestChance().ToStringPercent());
			stringBuilder.AppendLine("Adjusted population: ".PadRight(40) + StorytellerUtilityPopulation.AdjustedPopulation.ToString("F1"));
			stringBuilder.AppendLine("Adjusted population including quests: ".PadRight(40) + StorytellerUtilityPopulation.AdjustedPopulation.ToString("F1"));
			stringBuilder.AppendLine("Pop adaptation days: ".PadRight(40) + Find.StoryWatcher.watcherPopAdaptation.AdaptDays.ToString("F2"));
			return stringBuilder.ToString();
		}

		// Token: 0x0600724C RID: 29260 RVA: 0x00280700 File Offset: 0x0027E900
		[DebugOutput]
		public static void PopulationIntents()
		{
			List<float> list = new List<float>();
			for (int i = 0; i < 30; i++)
			{
				list.Add((float)i);
			}
			List<float> list2 = new List<float>();
			for (int j = 0; j < 40; j += 2)
			{
				list2.Add((float)j);
			}
			DebugTables.MakeTablesDialog<float, float>(list2, (float ds) => "d-" + ds.ToString("F0"), list, (float rv) => rv.ToString("F2"), (float ds, float p) => StorytellerUtilityPopulation.CalculatePopulationIntent(StorytellerUtilityPopulation.StorytellerDef, p, (float)((int)ds)).ToString("F2"), "pop");
		}

		// Token: 0x0400484E RID: 18510
		private static float PopulationValue_Prisoner = 0.5f;

		// Token: 0x0400484F RID: 18511
		private static float PopulationValue_PrisonerUnrecruitable = 0.25f;

		// Token: 0x04004850 RID: 18512
		private static float PopulationValue_Slave = 0.75f;

		// Token: 0x04004851 RID: 18513
		private static float PopulationValue_Child = 0.3f;
	}
}
