using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001120 RID: 4384
	public static class QuestTuning
	{
		// Token: 0x040045E5 RID: 17893
		public static readonly SimpleCurve IncreasesPopQuestChanceByPopIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.05f),
				true
			},
			{
				new CurvePoint(1f, 0.3f),
				true
			},
			{
				new CurvePoint(3f, 0.45f),
				true
			}
		};

		// Token: 0x040045E6 RID: 17894
		public static readonly float[] RecentStoryWeightFactors = new float[]
		{
			0.01f,
			0.3f,
			0.5f,
			0.7f,
			0.9f
		};

		// Token: 0x040045E7 RID: 17895
		public static readonly SimpleCurve NonFavorQuestSelectionWeightFactorByDaysSinceFavorQuestCurve = new SimpleCurve
		{
			{
				new CurvePoint(10f, 1f),
				true
			},
			{
				new CurvePoint(25f, 0.01f),
				true
			}
		};

		// Token: 0x040045E8 RID: 17896
		public static readonly SimpleCurve PointsToRewardMarketValueCurve = new SimpleCurve
		{
			{
				new CurvePoint(300f, 800f),
				true
			},
			{
				new CurvePoint(700f, 1500f),
				true
			},
			{
				new CurvePoint(5000f, 4000f),
				true
			}
		};

		// Token: 0x040045E9 RID: 17897
		public const int MinFavorAtOnce = 1;

		// Token: 0x040045EA RID: 17898
		public const int MaxFavorAtOnce = 12;

		// Token: 0x040045EB RID: 17899
		public const int MaxGoodwillToAllowGoodwillReward = 92;

		// Token: 0x040045EC RID: 17900
		public static readonly SimpleCurve PopIncreasingRewardWeightByPopIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.05f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(3f, 2f),
				true
			}
		};

		// Token: 0x040045ED RID: 17901
		public const float FutureResearchProjectTechprintSelectionWeightFactor = 0.02f;

		// Token: 0x040045EE RID: 17902
		public static readonly SimpleCurve DaysSincePsylinkAvailableToGuaranteedNeuroformerChance = new SimpleCurve
		{
			{
				new CurvePoint(45f, 0f),
				true
			},
			{
				new CurvePoint(60f, 1f),
				true
			}
		};

		// Token: 0x040045EF RID: 17903
		public const float MinDaysBetweenRaidSourceRaids = 1.5f;

		// Token: 0x040045F0 RID: 17904
		public const float RaidSourceRaidThreatPointsFactor = 0.6f;

		// Token: 0x040045F1 RID: 17905
		public static readonly SimpleCurve PointsToRaidSourceRaidsMTBDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(400f, 25f),
				true
			},
			{
				new CurvePoint(1500f, 10f),
				true
			},
			{
				new CurvePoint(5000f, 5f),
				true
			}
		};
	}
}
