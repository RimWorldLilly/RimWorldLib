using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015FD RID: 5629
	public static class DiplomacyTuning
	{
		// Token: 0x040055BB RID: 21947
		public const int MaxGoodwill = 100;

		// Token: 0x040055BC RID: 21948
		public const int MinGoodwill = -100;

		// Token: 0x040055BD RID: 21949
		public const int NaturalEnemyNaturalGoodwill = -130;

		// Token: 0x040055BE RID: 21950
		public const int NaturalEnemyInitialGoodwill = -80;

		// Token: 0x040055BF RID: 21951
		public const int NaturalGoodwillRange = 50;

		// Token: 0x040055C0 RID: 21952
		public const float NaturalGoodwillDailyChange = 0.2f;

		// Token: 0x040055C1 RID: 21953
		public const float GoodwillChangeTowardsNaturalGoodwillFactor = 1.25f;

		// Token: 0x040055C2 RID: 21954
		public const int BecomeHostileThreshold = -75;

		// Token: 0x040055C3 RID: 21955
		public const int BecomeNeutralThreshold = 0;

		// Token: 0x040055C4 RID: 21956
		public const int BecomeAllyThreshold = 75;

		// Token: 0x040055C5 RID: 21957
		public const int InitialHostileThreshold = -10;

		// Token: 0x040055C6 RID: 21958
		public const int InitialAllyThreshold = 75;

		// Token: 0x040055C7 RID: 21959
		public static readonly IntRange ForcedStartingEnemyGoodwillRange = new IntRange(-100, -40);

		// Token: 0x040055C8 RID: 21960
		public const int MinGoodwillToRequestAICoreQuest = 40;

		// Token: 0x040055C9 RID: 21961
		public const int RequestAICoreQuestSilverCost = 1500;

		// Token: 0x040055CA RID: 21962
		public static readonly FloatRange RansomFeeMarketValueFactorRange = new FloatRange(1.2f, 2.2f);

		// Token: 0x040055CB RID: 21963
		public const int Goodwill_NaturalChangeStep = 10;

		// Token: 0x040055CC RID: 21964
		public const float Goodwill_PerDirectDamageToPawn = -1.3f;

		// Token: 0x040055CD RID: 21965
		public const float Goodwill_PerDirectDamageToBuilding = -1f;

		// Token: 0x040055CE RID: 21966
		public const int Goodwill_MemberCrushed_Humanlike = -25;

		// Token: 0x040055CF RID: 21967
		public const int Goodwill_MemberCrushed_Animal = -15;

		// Token: 0x040055D0 RID: 21968
		public const int Goodwill_MemberNeutrallyDied_Humanlike = -5;

		// Token: 0x040055D1 RID: 21969
		public const int Goodwill_MemberNeutrallyDied_Animal = -3;

		// Token: 0x040055D2 RID: 21970
		public const int Goodwill_BodyPartRemovalViolation = -70;

		// Token: 0x040055D3 RID: 21971
		public const int Goodwill_HemogenPackExtracted = -1;

		// Token: 0x040055D4 RID: 21972
		public const int Goodwill_SurgicalInspection = -1;

		// Token: 0x040055D5 RID: 21973
		public const int Goodwill_MemberEuthanized = -100;

		// Token: 0x040055D6 RID: 21974
		public const int Goodwill_MemberKilledInstantly = -100;

		// Token: 0x040055D7 RID: 21975
		public const int Goodwill_AttackedSettlement = -50;

		// Token: 0x040055D8 RID: 21976
		public const int Goodwill_MilitaryAidRequested = -25;

		// Token: 0x040055D9 RID: 21977
		public const int Goodwill_TraderRequested = -15;

		// Token: 0x040055DA RID: 21978
		public const int Goodwill_OrbitalTraderRequested = -30;

		// Token: 0x040055DB RID: 21979
		public const int Goodwill_MemberStripped = -40;

		// Token: 0x040055DC RID: 21980
		public const int Goodwill_MemberKidnappedOnGravship = -10;

		// Token: 0x040055DD RID: 21981
		public const int Goodwill_MemberXenogermAbsorbed = -50;

		// Token: 0x040055DE RID: 21982
		public const int Goodwill_MemberMutated = -50;

		// Token: 0x040055DF RID: 21983
		public static readonly SimpleCurve Goodwill_PerQuadrumFromSettlementProximity = new SimpleCurve
		{
			{
				new CurvePoint(2f, -30f),
				true
			},
			{
				new CurvePoint(3f, -20f),
				true
			},
			{
				new CurvePoint(4f, -10f),
				true
			},
			{
				new CurvePoint(5f, 0f),
				true
			}
		};

		// Token: 0x040055E0 RID: 21984
		public const float Goodwill_BaseGiftSilverForOneGoodwill = 40f;

		// Token: 0x040055E1 RID: 21985
		public static readonly SimpleCurve GiftGoodwillFactorRelationsCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(75f, 0.25f),
				true
			}
		};

		// Token: 0x040055E2 RID: 21986
		public const float Goodwill_GiftPrisonerOfTheirFactionValueFactor = 2f;

		// Token: 0x040055E3 RID: 21987
		public const float Goodwill_TradedMarketValueforOneGoodwill = 600f;

		// Token: 0x040055E4 RID: 21988
		public const int Goodwill_DestroyedMutualEnemyBase = 20;

		// Token: 0x040055E5 RID: 21989
		public const int Goodwill_MemberExitedMapHealthy = 12;

		// Token: 0x040055E6 RID: 21990
		public const int Goodwill_MemberExitedMapHealthy_LeaderBonus = 40;

		// Token: 0x040055E7 RID: 21991
		public const float Goodwill_PerTend = 1f;

		// Token: 0x040055E8 RID: 21992
		public const int Goodwill_MaxTimesTendedTo = 10;

		// Token: 0x040055E9 RID: 21993
		public const int Goodwill_QuestTradeRequestCompleted = 12;

		// Token: 0x040055EA RID: 21994
		public static readonly IntRange Goodwill_PeaceTalksDisasterRange = new IntRange(-50, -40);

		// Token: 0x040055EB RID: 21995
		public static readonly IntRange Goodwill_PeaceTalksBackfireRange = new IntRange(-20, -10);

		// Token: 0x040055EC RID: 21996
		public static readonly IntRange Goodwill_PeaceTalksSuccessRange = new IntRange(60, 70);

		// Token: 0x040055ED RID: 21997
		public static readonly IntRange Goodwill_PeaceTalksTriumphRange = new IntRange(100, 110);

		// Token: 0x040055EE RID: 21998
		public static readonly IntRange RoyalFavor_PeaceTalksSuccessRange = new IntRange(1, 4);

		// Token: 0x040055EF RID: 21999
		public const float VisitorGiftChanceBase = 0.25f;

		// Token: 0x040055F0 RID: 22000
		public static readonly SimpleCurve VisitorGiftChanceFactorFromPlayerWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(30000f, 1f),
				true
			},
			{
				new CurvePoint(80000f, 0.1f),
				true
			},
			{
				new CurvePoint(300000f, 0f),
				true
			}
		};

		// Token: 0x040055F1 RID: 22001
		public static readonly SimpleCurve VisitorGiftChanceFactorFromGoodwillCurve = new SimpleCurve
		{
			{
				new CurvePoint(-30f, 0f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			}
		};

		// Token: 0x040055F2 RID: 22002
		public static readonly FloatRange VisitorGiftTotalMarketValueRangeBase = new FloatRange(100f, 500f);

		// Token: 0x040055F3 RID: 22003
		public static readonly SimpleCurve VisitorGiftTotalMarketValueFactorFromPlayerWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(10000f, 0.25f),
				true
			},
			{
				new CurvePoint(100000f, 1f),
				true
			}
		};

		// Token: 0x040055F4 RID: 22004
		public static readonly FloatRange RequestedMilitaryAidPointsRange = new FloatRange(800f, 1000f);
	}
}
