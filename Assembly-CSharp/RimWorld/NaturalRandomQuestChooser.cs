using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001040 RID: 4160
	public static class NaturalRandomQuestChooser
	{
		// Token: 0x060068A1 RID: 26785 RVA: 0x0024F878 File Offset: 0x0024DA78
		public static float PopulationIncreasingQuestChance()
		{
			return QuestTuning.IncreasesPopQuestChanceByPopIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntentForQuest);
		}

		// Token: 0x060068A2 RID: 26786 RVA: 0x0024F88C File Offset: 0x0024DA8C
		public static QuestScriptDef ChooseNaturalRandomQuest(float points, IIncidentTarget target)
		{
			NaturalRandomQuestChooser.<>c__DisplayClass1_0 CS$<>8__locals1 = new NaturalRandomQuestChooser.<>c__DisplayClass1_0();
			CS$<>8__locals1.points = points;
			CS$<>8__locals1.target = target;
			bool flag = Rand.Chance(NaturalRandomQuestChooser.PopulationIncreasingQuestChance());
			QuestScriptDef result;
			if (CS$<>8__locals1.<ChooseNaturalRandomQuest>g__TryGetQuest|0(flag, out result))
			{
				return result;
			}
			QuestScriptDef result2;
			if (flag && CS$<>8__locals1.<ChooseNaturalRandomQuest>g__TryGetQuest|0(false, out result2))
			{
				return result2;
			}
			return null;
		}

		// Token: 0x060068A3 RID: 26787 RVA: 0x0024F8D8 File Offset: 0x0024DAD8
		public static float GetNaturalRandomSelectionWeight(QuestScriptDef quest, float points, StoryState storyState)
		{
			if (quest.rootSelectionWeight <= 0f || points < quest.rootMinPoints || GenDate.DaysPassedSinceSettle < quest.rootEarliestDay || StorytellerUtility.GetProgressScore(storyState.Target) < quest.rootMinProgressScore)
			{
				return 0f;
			}
			if (quest.minRefireDays > 0f)
			{
				List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
				for (int i = 0; i < questsListForReading.Count; i++)
				{
					if (questsListForReading[i].root == quest && (float)(Find.TickManager.TicksGame - questsListForReading[i].appearanceTick) < 60000f * quest.minRefireDays)
					{
						return 0f;
					}
				}
			}
			float num = quest.rootSelectionWeight;
			if (quest.rootSelectionWeightFactorFromPointsCurve != null)
			{
				num *= quest.rootSelectionWeightFactorFromPointsCurve.Evaluate(points);
			}
			for (int j = 0; j < storyState.RecentRandomQuests.Count; j++)
			{
				if (storyState.RecentRandomQuests[j] == quest)
				{
					num *= QuestTuning.RecentStoryWeightFactors[j];
				}
			}
			if (!quest.canGiveRoyalFavor && NaturalRandomQuestChooser.<GetNaturalRandomSelectionWeight>g__PlayerWantsRoyalFavorFromAnyFaction|2_0())
			{
				int num2 = (storyState.LastRoyalFavorQuestTick != -1) ? storyState.LastRoyalFavorQuestTick : 0;
				float x = (float)(Find.TickManager.TicksGame - num2) / 60000f;
				num *= QuestTuning.NonFavorQuestSelectionWeightFactorByDaysSinceFavorQuestCurve.Evaluate(x);
			}
			return num;
		}

		// Token: 0x060068A4 RID: 26788 RVA: 0x0024FA1C File Offset: 0x0024DC1C
		public static float GetNaturalDecreeSelectionWeight(QuestScriptDef quest, StoryState storyState)
		{
			if (quest.decreeSelectionWeight <= 0f)
			{
				return 0f;
			}
			float decreeSelectionWeight = quest.decreeSelectionWeight;
			for (int i = 0; i < storyState.RecentRandomDecrees.Count; i++)
			{
				if (storyState.RecentRandomDecrees[i] == quest)
				{
					return QuestTuning.RecentStoryWeightFactors[i];
				}
			}
			return decreeSelectionWeight;
		}

		// Token: 0x060068A5 RID: 26789 RVA: 0x0024FA74 File Offset: 0x0024DC74
		public static float DebugTotalNaturalRandomSelectionWeight(QuestScriptDef quest, float points, IIncidentTarget target)
		{
			if (!quest.IsRootRandomSelected)
			{
				return 0f;
			}
			if (!quest.CanRun(points, target))
			{
				return 0f;
			}
			float naturalRandomSelectionWeight = NaturalRandomQuestChooser.GetNaturalRandomSelectionWeight(quest, points, target.StoryState);
			float num = QuestTuning.IncreasesPopQuestChanceByPopIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntentForQuest);
			return num * (quest.rootIncreasesPopulation ? naturalRandomSelectionWeight : 0f) + (1f - num) * ((!quest.rootIncreasesPopulation) ? naturalRandomSelectionWeight : 0f);
		}

		// Token: 0x060068A6 RID: 26790 RVA: 0x0024FAE8 File Offset: 0x0024DCE8
		[CompilerGenerated]
		internal static bool <GetNaturalRandomSelectionWeight>g__PlayerWantsRoyalFavorFromAnyFaction|2_0()
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (allFactionsListForReading[i].allowRoyalFavorRewards && allFactionsListForReading[i] != Faction.OfPlayer && allFactionsListForReading[i].def.HasRoyalTitles && !allFactionsListForReading[i].temporary)
				{
					return true;
				}
			}
			return false;
		}
	}
}
