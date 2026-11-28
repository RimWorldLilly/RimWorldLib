using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021DB RID: 8667
	public class QuestNode_GetRandomNegativeGameCondition : QuestNode
	{
		// Token: 0x0600D79B RID: 55195 RVA: 0x005001D0 File Offset: 0x004FE3D0
		public static void ResetStaticData()
		{
			QuestNode_GetRandomNegativeGameCondition.options = new List<QuestNode_GetRandomNegativeGameCondition.Option>
			{
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.VolcanicWinter, new FloatRange(10f, 20f), 0.4f, 1),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.WeatherController, new FloatRange(5f, 20f), 0.4f, 1),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.HeatWave, new FloatRange(4f, 8f), 1f, 1),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.ColdSnap, new FloatRange(4f, 8f), 1f, 1),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.ToxicFallout, new FloatRange(5f, 20f), 0.8f, 2),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.PsychicSuppression, new FloatRange(4f, 8f), 1.5f, 2),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.EMIField, new FloatRange(4f, 8f), 1.8f, 3),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.PsychicDrone, new FloatRange(4f, 8f), 2f, 3)
			};
		}

		// Token: 0x0600D79C RID: 55196 RVA: 0x0050030F File Offset: 0x004FE50F
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Get<Map>("map", null, false) != null && this.DoWork(slate);
		}

		// Token: 0x0600D79D RID: 55197 RVA: 0x00500329 File Offset: 0x004FE529
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600D79E RID: 55198 RVA: 0x00500338 File Offset: 0x004FE538
		private bool DoWork(Slate slate)
		{
			QuestNode_GetRandomNegativeGameCondition.Option option;
			if (QuestGen.Working)
			{
				if (!(from x in QuestNode_GetRandomNegativeGameCondition.options
				where x.challengeRating == QuestGen.quest.challengeRating && this.PossibleNow(x.gameCondition, slate)
				select x).TryRandomElement(out option) && !(from x in QuestNode_GetRandomNegativeGameCondition.options
				where x.challengeRating < QuestGen.quest.challengeRating && this.PossibleNow(x.gameCondition, slate)
				select x).TryRandomElement(out option) && !(from x in QuestNode_GetRandomNegativeGameCondition.options
				where this.PossibleNow(x.gameCondition, slate)
				select x).TryRandomElement(out option))
				{
					return false;
				}
			}
			else if (!(from x in QuestNode_GetRandomNegativeGameCondition.options
			where this.PossibleNow(x.gameCondition, slate)
			select x).TryRandomElement(out option))
			{
				return false;
			}
			int var = (int)(option.durationDaysRange.RandomInRange * 60000f);
			slate.Set<GameConditionDef>(this.storeGameConditionAs.GetValue(slate), option.gameCondition, false);
			slate.Set<int>(this.storeGameConditionDurationAs.GetValue(slate), var, false);
			slate.Set<float>(this.storeGameConditionDifficultyAs.GetValue(slate), option.difficulty, false);
			return true;
		}

		// Token: 0x0600D79F RID: 55199 RVA: 0x0050045C File Offset: 0x004FE65C
		private bool PossibleNow(GameConditionDef def, Slate slate)
		{
			if (def == null)
			{
				return false;
			}
			Map map = slate.Get<Map>("map", null, false);
			if (map.gameConditionManager.ConditionIsActive(def))
			{
				return false;
			}
			IncidentDef incidentDef = null;
			List<IncidentDef> allDefsListForReading = DefDatabase<IncidentDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].Worker is IncidentWorker_MakeGameCondition && allDefsListForReading[i].gameCondition == def)
				{
					incidentDef = allDefsListForReading[i];
					break;
				}
			}
			if (incidentDef != null)
			{
				if (!Find.Storyteller.difficulty.AllowedBy(incidentDef.disabledWhen))
				{
					return false;
				}
				if (GenDate.DaysPassedSinceSettle < incidentDef.earliestDay)
				{
					return false;
				}
				if (incidentDef.Worker.FiredTooRecently(map))
				{
					return false;
				}
			}
			return (def != GameConditionDefOf.ColdSnap || IncidentWorker_ColdSnap.IsTemperatureAppropriate(map)) && (def != GameConditionDefOf.HeatWave || IncidentWorker_HeatWave.IsTemperatureAppropriate(map));
		}

		// Token: 0x040088FC RID: 35068
		[NoTranslate]
		public SlateRef<string> storeGameConditionAs;

		// Token: 0x040088FD RID: 35069
		[NoTranslate]
		public SlateRef<string> storeGameConditionDurationAs;

		// Token: 0x040088FE RID: 35070
		[NoTranslate]
		public SlateRef<string> storeGameConditionDifficultyAs;

		// Token: 0x040088FF RID: 35071
		private static List<QuestNode_GetRandomNegativeGameCondition.Option> options;

		// Token: 0x02003D30 RID: 15664
		private struct Option
		{
			// Token: 0x06015C91 RID: 89233 RVA: 0x006D3B62 File Offset: 0x006D1D62
			public Option(GameConditionDef gameCondition, FloatRange durationDaysRange, float difficulty, int challengeRating)
			{
				this.gameCondition = gameCondition;
				this.durationDaysRange = durationDaysRange;
				this.difficulty = difficulty;
				this.challengeRating = challengeRating;
			}

			// Token: 0x0400F18D RID: 61837
			public GameConditionDef gameCondition;

			// Token: 0x0400F18E RID: 61838
			public FloatRange durationDaysRange;

			// Token: 0x0400F18F RID: 61839
			public float difficulty;

			// Token: 0x0400F190 RID: 61840
			public int challengeRating;
		}
	}
}
