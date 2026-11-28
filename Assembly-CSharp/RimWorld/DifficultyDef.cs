using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F4D RID: 3917
	public sealed class DifficultyDef : Def
	{
		// Token: 0x04003A87 RID: 14983
		[Obsolete]
		public Color drawColor = Color.white;

		// Token: 0x04003A88 RID: 14984
		[Obsolete]
		public bool isExtreme;

		// Token: 0x04003A89 RID: 14985
		public bool isCustom;

		// Token: 0x04003A8A RID: 14986
		[Obsolete]
		public int difficulty = -1;

		// Token: 0x04003A8B RID: 14987
		public float threatScale = 1f;

		// Token: 0x04003A8C RID: 14988
		public bool allowBigThreats = true;

		// Token: 0x04003A8D RID: 14989
		public bool allowIntroThreats = true;

		// Token: 0x04003A8E RID: 14990
		public bool allowCaveHives = true;

		// Token: 0x04003A8F RID: 14991
		public bool peacefulTemples;

		// Token: 0x04003A90 RID: 14992
		public bool allowViolentQuests = true;

		// Token: 0x04003A91 RID: 14993
		public bool predatorsHuntHumanlikes = true;

		// Token: 0x04003A92 RID: 14994
		public bool babiesAreHealthy;

		// Token: 0x04003A93 RID: 14995
		public bool noBabiesOrChildren;

		// Token: 0x04003A94 RID: 14996
		public float scariaRotChance;

		// Token: 0x04003A95 RID: 14997
		public float colonistMoodOffset;

		// Token: 0x04003A96 RID: 14998
		public float tradePriceFactorLoss;

		// Token: 0x04003A97 RID: 14999
		public float cropYieldFactor = 1f;

		// Token: 0x04003A98 RID: 15000
		public float mineYieldFactor = 1f;

		// Token: 0x04003A99 RID: 15001
		public float butcherYieldFactor = 1f;

		// Token: 0x04003A9A RID: 15002
		public float fishingYieldFactor = 1f;

		// Token: 0x04003A9B RID: 15003
		public float researchSpeedFactor = 1f;

		// Token: 0x04003A9C RID: 15004
		public float diseaseIntervalFactor = 1f;

		// Token: 0x04003A9D RID: 15005
		public float enemyReproductionRateFactor = 1f;

		// Token: 0x04003A9E RID: 15006
		public float playerPawnInfectionChanceFactor = 1f;

		// Token: 0x04003A9F RID: 15007
		public float manhunterChanceOnDamageFactor = 1f;

		// Token: 0x04003AA0 RID: 15008
		public float deepDrillInfestationChanceFactor = 1f;

		// Token: 0x04003AA1 RID: 15009
		public float wastepackInfestationChanceFactor = 1f;

		// Token: 0x04003AA2 RID: 15010
		public float nomadicMineableResourcesFactor = 1f;

		// Token: 0x04003AA3 RID: 15011
		public float foodPoisonChanceFactor = 1f;

		// Token: 0x04003AA4 RID: 15012
		[Obsolete]
		public float threatsGeneratorThreatCountFactor = 1f;

		// Token: 0x04003AA5 RID: 15013
		public float maintenanceCostFactor = 1f;

		// Token: 0x04003AA6 RID: 15014
		public float enemyDeathOnDownedChanceFactor = 1f;

		// Token: 0x04003AA7 RID: 15015
		public float adaptationGrowthRateFactorOverZero = 1f;

		// Token: 0x04003AA8 RID: 15016
		public float adaptationEffectFactor = 1f;

		// Token: 0x04003AA9 RID: 15017
		public float questRewardValueFactor = 1f;

		// Token: 0x04003AAA RID: 15018
		public float raidLootPointsFactor = 1f;

		// Token: 0x04003AAB RID: 15019
		public bool allowTraps = true;

		// Token: 0x04003AAC RID: 15020
		public bool allowTurrets = true;

		// Token: 0x04003AAD RID: 15021
		public bool allowMortars = true;

		// Token: 0x04003AAE RID: 15022
		public bool classicMortars;

		// Token: 0x04003AAF RID: 15023
		public bool allowExtremeWeatherIncidents = true;

		// Token: 0x04003AB0 RID: 15024
		public bool fixedWealthMode;

		// Token: 0x04003AB1 RID: 15025
		public float lowPopConversionBoost = 3f;

		// Token: 0x04003AB2 RID: 15026
		public float minThreatPointsRangeCeiling = 70f;

		// Token: 0x04003AB3 RID: 15027
		public float childAgingRate = 4f;

		// Token: 0x04003AB4 RID: 15028
		public float adultAgingRate = 1f;

		// Token: 0x04003AB5 RID: 15029
		public bool unwaveringPrisoners = true;

		// Token: 0x04003AB6 RID: 15030
		public bool childRaidersAllowed;

		// Token: 0x04003AB7 RID: 15031
		public float anomalyThreatsInactiveFraction = 0.08f;

		// Token: 0x04003AB8 RID: 15032
		public float anomalyThreatsActiveFraction = 0.3f;

		// Token: 0x04003AB9 RID: 15033
		public float studyEfficiencyFactor = 1f;
	}
}
