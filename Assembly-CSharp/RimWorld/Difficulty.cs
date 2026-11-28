using System;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200112D RID: 4397
	public class Difficulty : IExposable
	{
		// Token: 0x06006E32 RID: 28210 RVA: 0x0026A150 File Offset: 0x00268350
		public Difficulty()
		{
		}

		// Token: 0x06006E33 RID: 28211 RVA: 0x0026A2F0 File Offset: 0x002684F0
		public Difficulty(DifficultyDef src)
		{
			this.CopyFrom(src);
		}

		// Token: 0x170012D9 RID: 4825
		// (get) Token: 0x06006E34 RID: 28212 RVA: 0x0026A496 File Offset: 0x00268696
		public float MinThreatPointsCeiling
		{
			get
			{
				return this.minThreatPointsRangeCeilingCurveCached.Evaluate(GenDate.DaysPassedSinceSettleFloat);
			}
		}

		// Token: 0x170012DA RID: 4826
		// (get) Token: 0x06006E35 RID: 28213 RVA: 0x0026A4A8 File Offset: 0x002686A8
		public float EffectiveQuestRewardValueFactor
		{
			get
			{
				return 1f / Mathf.Clamp(this.threatScale, 0.3f, 10f) * this.questRewardValueFactor;
			}
		}

		// Token: 0x170012DB RID: 4827
		// (get) Token: 0x06006E36 RID: 28214 RVA: 0x0026A4CC File Offset: 0x002686CC
		public float EffectiveRaidLootPointsFactor
		{
			get
			{
				return 1f / Mathf.Clamp(this.threatScale, 0.3f, 10f) * this.raidLootPointsFactor;
			}
		}

		// Token: 0x170012DC RID: 4828
		// (get) Token: 0x06006E37 RID: 28215 RVA: 0x0026A4F0 File Offset: 0x002686F0
		public bool ChildrenAllowed
		{
			get
			{
				return ModsConfig.BiotechActive && !this.noBabiesOrChildren;
			}
		}

		// Token: 0x170012DD RID: 4829
		// (get) Token: 0x06006E38 RID: 28216 RVA: 0x0026A504 File Offset: 0x00268704
		public bool ChildRaidersAllowed
		{
			get
			{
				return this.ChildrenAllowed && this.childRaidersAllowed;
			}
		}

		// Token: 0x170012DE RID: 4830
		// (get) Token: 0x06006E39 RID: 28217 RVA: 0x0026A516 File Offset: 0x00268716
		// (set) Token: 0x06006E3A RID: 28218 RVA: 0x0026A527 File Offset: 0x00268727
		public AnomalyPlaystyleDef AnomalyPlaystyleDef
		{
			get
			{
				return this.anomalyPlaystyleDef ?? AnomalyPlaystyleDefOf.Standard;
			}
			set
			{
				this.anomalyPlaystyleDef = value;
			}
		}

		// Token: 0x06006E3B RID: 28219 RVA: 0x0026A530 File Offset: 0x00268730
		public bool AllowedToBuild(BuildableDef def)
		{
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null)
			{
				if (!this.allowTraps && thingDef.building.isTrap)
				{
					return false;
				}
				if ((!this.allowMortars || !this.allowTurrets) && thingDef.building.IsTurret)
				{
					if (!thingDef.building.IsMortar)
					{
						return this.allowTurrets;
					}
					return this.allowMortars;
				}
			}
			return true;
		}

		// Token: 0x06006E3C RID: 28220 RVA: 0x0026A598 File Offset: 0x00268798
		public bool AllowedBy(DifficultyConditionConfig cfg)
		{
			return cfg == null || ((this.allowBigThreats || !cfg.bigThreatsDisabled) && (this.allowTraps || !cfg.trapsDisabled) && (this.allowTurrets || !cfg.turretsDisabled) && (this.allowMortars || !cfg.mortarsDisabled) && (this.allowExtremeWeatherIncidents || !cfg.extremeWeatherIncidentsDisabled));
		}

		// Token: 0x06006E3D RID: 28221 RVA: 0x0026A608 File Offset: 0x00268808
		public float CertaintyReductionFactor(Pawn initiator, Pawn recipient)
		{
			if (initiator != null && initiator.Faction == Faction.OfPlayer && initiator.Ideo != null && initiator.Ideo.initialPlayerIdeo)
			{
				int num = Difficulty.<CertaintyReductionFactor>g__ColonistsOfPlayerStartingIdeo|75_0();
				if (num > 2)
				{
					return 1f;
				}
				if (num == 2)
				{
					return this.lowPopConversionBoost / 2f;
				}
				return this.lowPopConversionBoost;
			}
			else
			{
				if (recipient == null || recipient.Faction != Faction.OfPlayer || recipient.Ideo == null || !recipient.Ideo.initialPlayerIdeo)
				{
					return 1f;
				}
				int num2 = Difficulty.<CertaintyReductionFactor>g__ColonistsOfPlayerStartingIdeo|75_0();
				if (num2 > 2)
				{
					return 1f;
				}
				if (num2 == 2)
				{
					return 1f / this.lowPopConversionBoost * 2f;
				}
				return 1f / this.lowPopConversionBoost;
			}
		}

		// Token: 0x06006E3E RID: 28222 RVA: 0x0026A6C0 File Offset: 0x002688C0
		public void CopyFrom(DifficultyDef src)
		{
			this.threatScale = src.threatScale;
			this.allowBigThreats = src.allowBigThreats;
			this.allowIntroThreats = src.allowIntroThreats;
			this.allowCaveHives = src.allowCaveHives;
			this.peacefulTemples = src.peacefulTemples;
			this.allowViolentQuests = src.allowViolentQuests;
			this.predatorsHuntHumanlikes = src.predatorsHuntHumanlikes;
			this.scariaRotChance = src.scariaRotChance;
			this.colonistMoodOffset = src.colonistMoodOffset;
			this.tradePriceFactorLoss = src.tradePriceFactorLoss;
			this.cropYieldFactor = src.cropYieldFactor;
			this.mineYieldFactor = src.mineYieldFactor;
			this.butcherYieldFactor = src.butcherYieldFactor;
			this.fishingYieldFactor = src.fishingYieldFactor;
			this.researchSpeedFactor = src.researchSpeedFactor;
			this.diseaseIntervalFactor = src.diseaseIntervalFactor;
			this.enemyReproductionRateFactor = src.enemyReproductionRateFactor;
			this.playerPawnInfectionChanceFactor = src.playerPawnInfectionChanceFactor;
			this.manhunterChanceOnDamageFactor = src.manhunterChanceOnDamageFactor;
			this.deepDrillInfestationChanceFactor = src.deepDrillInfestationChanceFactor;
			this.wastepackInfestationChanceFactor = src.wastepackInfestationChanceFactor;
			this.nomadicMineableResourcesFactor = src.nomadicMineableResourcesFactor;
			this.foodPoisonChanceFactor = src.foodPoisonChanceFactor;
			this.maintenanceCostFactor = src.maintenanceCostFactor;
			this.enemyDeathOnDownedChanceFactor = src.enemyDeathOnDownedChanceFactor;
			this.adaptationGrowthRateFactorOverZero = src.adaptationGrowthRateFactorOverZero;
			this.adaptationEffectFactor = src.adaptationEffectFactor;
			this.questRewardValueFactor = src.questRewardValueFactor;
			this.raidLootPointsFactor = src.raidLootPointsFactor;
			this.allowTraps = src.allowTraps;
			this.allowTurrets = src.allowTurrets;
			this.allowMortars = src.allowMortars;
			this.classicMortars = src.classicMortars;
			this.allowExtremeWeatherIncidents = src.allowExtremeWeatherIncidents;
			this.fixedWealthMode = src.fixedWealthMode;
			this.fixedWealthTimeFactor = 1f;
			this.friendlyFireChanceFactor = 0.4f;
			this.allowInstantKillChance = 1f;
			this.lowPopConversionBoost = src.lowPopConversionBoost;
			this.minThreatPointsRangeCeiling = src.minThreatPointsRangeCeiling;
			this.babiesAreHealthy = src.babiesAreHealthy;
			this.noBabiesOrChildren = src.noBabiesOrChildren;
			this.childAgingRate = src.childAgingRate;
			this.adultAgingRate = src.adultAgingRate;
			this.unwaveringPrisoners = src.unwaveringPrisoners;
			this.childRaidersAllowed = src.childRaidersAllowed;
			this.anomalyThreatsInactiveFraction = src.anomalyThreatsInactiveFraction;
			this.anomalyThreatsActiveFraction = src.anomalyThreatsActiveFraction;
			this.studyEfficiencyFactor = src.studyEfficiencyFactor;
			this.SetMinThreatPointsCurve();
		}

		// Token: 0x06006E3F RID: 28223 RVA: 0x0026A91C File Offset: 0x00268B1C
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.threatScale, "threatScale", 0f, false);
			Scribe_Values.Look<bool>(ref this.allowBigThreats, "allowBigThreats", false, false);
			Scribe_Values.Look<bool>(ref this.allowIntroThreats, "allowIntroThreats", false, false);
			Scribe_Values.Look<bool>(ref this.allowCaveHives, "allowCaveHives", false, false);
			Scribe_Values.Look<bool>(ref this.peacefulTemples, "peacefulTemples", false, false);
			Scribe_Values.Look<bool>(ref this.allowViolentQuests, "allowViolentQuests", false, false);
			Scribe_Values.Look<bool>(ref this.babiesAreHealthy, "babiesAreHealthy", false, false);
			Scribe_Values.Look<bool>(ref this.noBabiesOrChildren, "noBabiesOrChildren", false, false);
			Scribe_Values.Look<bool>(ref this.predatorsHuntHumanlikes, "predatorsHuntHumanlikes", false, false);
			Scribe_Values.Look<float>(ref this.scariaRotChance, "scariaRotChance", 0f, false);
			Scribe_Values.Look<float>(ref this.colonistMoodOffset, "colonistMoodOffset", 0f, false);
			Scribe_Values.Look<float>(ref this.tradePriceFactorLoss, "tradePriceFactorLoss", 0f, false);
			Scribe_Values.Look<float>(ref this.cropYieldFactor, "cropYieldFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.mineYieldFactor, "mineYieldFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.butcherYieldFactor, "butcherYieldFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.fishingYieldFactor, "fishingYieldFactor", 1f, false);
			Scribe_Values.Look<float>(ref this.researchSpeedFactor, "researchSpeedFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.diseaseIntervalFactor, "diseaseIntervalFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.enemyReproductionRateFactor, "enemyReproductionRateFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.playerPawnInfectionChanceFactor, "playerPawnInfectionChanceFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.manhunterChanceOnDamageFactor, "manhunterChanceOnDamageFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.deepDrillInfestationChanceFactor, "deepDrillInfestationChanceFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.wastepackInfestationChanceFactor, "wastepackInfestationChanceFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.nomadicMineableResourcesFactor, "nomadicMineableResourcesFactor", 1f, false);
			Scribe_Values.Look<float>(ref this.foodPoisonChanceFactor, "foodPoisonChanceFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.maintenanceCostFactor, "maintenanceCostFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.enemyDeathOnDownedChanceFactor, "enemyDeathOnDownedChanceFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.adaptationGrowthRateFactorOverZero, "adaptationGrowthRateFactorOverZero", 0f, false);
			Scribe_Values.Look<float>(ref this.adaptationEffectFactor, "adaptationEffectFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.questRewardValueFactor, "questRewardValueFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.raidLootPointsFactor, "raidLootPointsFactor", 1f, false);
			Scribe_Values.Look<bool>(ref this.allowTraps, "allowTraps", true, false);
			Scribe_Values.Look<bool>(ref this.allowTurrets, "allowTurrets", true, false);
			Scribe_Values.Look<bool>(ref this.allowMortars, "allowMortars", true, false);
			Scribe_Values.Look<bool>(ref this.classicMortars, "classicMortars", true, false);
			Scribe_Values.Look<bool>(ref this.allowExtremeWeatherIncidents, "allowExtremeWeatherIncidents", true, false);
			Scribe_Values.Look<bool>(ref this.fixedWealthMode, "fixedWealthMode", false, false);
			Scribe_Values.Look<float>(ref this.fixedWealthTimeFactor, "fixedWealthTimeFactor", 1f, false);
			Scribe_Values.Look<float>(ref this.friendlyFireChanceFactor, "friendlyFireChanceFactor", 0.4f, false);
			Scribe_Values.Look<float>(ref this.allowInstantKillChance, "allowInstantKillChance", 1f, false);
			Scribe_Values.Look<float>(ref this.lowPopConversionBoost, "lowPopConversionBoost", 3f, false);
			Scribe_Values.Look<float>(ref this.minThreatPointsRangeCeiling, "minThreatPointsRangeCeiling", 70f, false);
			Scribe_Values.Look<float>(ref this.adultAgingRate, "adultAgingRate", 1f, false);
			Scribe_Values.Look<float>(ref this.childAgingRate, "childAgingRate", 4f, false);
			Scribe_Values.Look<bool>(ref this.unwaveringPrisoners, "unwaveringPrisoners", false, false);
			Scribe_Values.Look<bool>(ref this.childRaidersAllowed, "childRaidersAllowed", true, false);
			Scribe_Values.Look<float>(ref this.anomalyThreatsInactiveFraction, "anomalyThreatsInactiveFraction", 0.08f, false);
			Scribe_Values.Look<float>(ref this.anomalyThreatsActiveFraction, "anomalyThreatsActiveFraction", 0.3f, false);
			Scribe_Values.Look<float?>(ref this.overrideAnomalyThreatsFraction, "overrideAnomalyThreatsFraction", null, false);
			Scribe_Values.Look<float>(ref this.studyEfficiencyFactor, "studyEfficiencyFactor", 1f, false);
			Scribe_Defs.Look<AnomalyPlaystyleDef>(ref this.anomalyPlaystyleDef, "anomalyPlaystyleDef");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.maintenanceCostFactor = Mathf.Max(0.01f, this.maintenanceCostFactor);
				this.SetMinThreatPointsCurve();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && ModsConfig.AnomalyActive && this.anomalyPlaystyleDef == null)
			{
				this.anomalyPlaystyleDef = AnomalyPlaystyleDefOf.Standard;
			}
		}

		// Token: 0x06006E40 RID: 28224 RVA: 0x0026AD90 File Offset: 0x00268F90
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("threatScale".PadRight(40)).Append(this.threatScale).AppendLine();
			stringBuilder.Append("allowBigThreats".PadRight(40)).Append(this.allowBigThreats).AppendLine();
			stringBuilder.Append("allowIntroThreats".PadRight(40)).Append(this.allowIntroThreats).AppendLine();
			stringBuilder.Append("allowCaveHives".PadRight(40)).Append(this.allowCaveHives).AppendLine();
			stringBuilder.Append("peacefulTemples".PadRight(40)).Append(this.peacefulTemples).AppendLine();
			stringBuilder.Append("allowViolentQuests".PadRight(40)).Append(this.allowViolentQuests).AppendLine();
			stringBuilder.Append("predatorsHuntHumanlikes".PadRight(40)).Append(this.predatorsHuntHumanlikes).AppendLine();
			stringBuilder.Append("scariaRotChance".PadRight(40)).Append(this.scariaRotChance).AppendLine();
			stringBuilder.Append("colonistMoodOffset".PadRight(40)).Append(this.colonistMoodOffset).AppendLine();
			stringBuilder.Append("tradePriceFactorLoss".PadRight(40)).Append(this.tradePriceFactorLoss).AppendLine();
			stringBuilder.Append("cropYieldFactor".PadRight(40)).Append(this.cropYieldFactor).AppendLine();
			stringBuilder.Append("mineYieldFactor".PadRight(40)).Append(this.mineYieldFactor).AppendLine();
			stringBuilder.Append("butcherYieldFactor".PadRight(40)).Append(this.butcherYieldFactor).AppendLine();
			stringBuilder.Append("fishingYieldFactor".PadRight(40)).Append(this.fishingYieldFactor).AppendLine();
			stringBuilder.Append("researchSpeedFactor".PadRight(40)).Append(this.researchSpeedFactor).AppendLine();
			stringBuilder.Append("diseaseIntervalFactor".PadRight(40)).Append(this.diseaseIntervalFactor).AppendLine();
			stringBuilder.Append("enemyReproductionRateFactor".PadRight(40)).Append(this.enemyReproductionRateFactor).AppendLine();
			stringBuilder.Append("playerPawnInfectionChanceFactor".PadRight(40)).Append(this.playerPawnInfectionChanceFactor).AppendLine();
			stringBuilder.Append("manhunterChanceOnDamageFactor".PadRight(40)).Append(this.manhunterChanceOnDamageFactor).AppendLine();
			stringBuilder.Append("deepDrillInfestationChanceFactor".PadRight(40)).Append(this.deepDrillInfestationChanceFactor).AppendLine();
			stringBuilder.Append("wastepackInfestationChanceFactor".PadRight(40)).Append(this.wastepackInfestationChanceFactor).AppendLine();
			stringBuilder.Append("nomadicMineableResourcesFactor".PadRight(40)).Append(this.nomadicMineableResourcesFactor).AppendLine();
			stringBuilder.Append("foodPoisonChanceFactor".PadRight(40)).Append(this.foodPoisonChanceFactor).AppendLine();
			stringBuilder.Append("maintenanceCostFactor".PadRight(40)).Append(this.maintenanceCostFactor).AppendLine();
			stringBuilder.Append("enemyDeathOnDownedChanceFactor".PadRight(40)).Append(this.enemyDeathOnDownedChanceFactor).AppendLine();
			stringBuilder.Append("adaptationGrowthRateFactorOverZero".PadRight(40)).Append(this.adaptationGrowthRateFactorOverZero).AppendLine();
			stringBuilder.Append("adaptationEffectFactor".PadRight(40)).Append(this.adaptationEffectFactor).AppendLine();
			stringBuilder.Append("questRewardValueFactor".PadRight(40)).Append(this.questRewardValueFactor).AppendLine();
			stringBuilder.Append("raidLootPointsFactor".PadRight(40)).Append(this.raidLootPointsFactor).AppendLine();
			stringBuilder.Append("allowTraps".PadRight(40)).Append(this.allowTraps).AppendLine();
			stringBuilder.Append("allowTurrets".PadRight(40)).Append(this.allowTurrets).AppendLine();
			stringBuilder.Append("allowMortars".PadRight(40)).Append(this.allowMortars).AppendLine();
			stringBuilder.Append("classicMortars".PadRight(40)).Append(this.classicMortars).AppendLine();
			stringBuilder.Append("allowExtremeWeatherIncidents".PadRight(40)).Append(this.allowExtremeWeatherIncidents).AppendLine();
			stringBuilder.Append("fixedWealthMode".PadRight(40)).Append(this.fixedWealthMode).AppendLine();
			stringBuilder.Append("fixedWealthTimeFactor".PadRight(40)).Append(this.fixedWealthTimeFactor).AppendLine();
			stringBuilder.Append("friendlyFireChanceFactor".PadRight(40)).Append(this.friendlyFireChanceFactor).AppendLine();
			stringBuilder.Append("allowInstantKillChance".PadRight(40)).Append(this.allowInstantKillChance).AppendLine();
			stringBuilder.Append("lowPopConversionBoost".PadRight(40)).Append(this.lowPopConversionBoost).AppendLine();
			stringBuilder.Append("minThreatPointsRangeCeiling".PadRight(40)).Append(this.minThreatPointsRangeCeiling).AppendLine();
			stringBuilder.Append("babiesAreHealthy".PadRight(40)).Append(this.babiesAreHealthy).AppendLine();
			stringBuilder.Append("noBabiesOrChildren".PadRight(40)).Append(this.noBabiesOrChildren).AppendLine();
			stringBuilder.Append("adultAgingRate".PadRight(40)).Append(this.adultAgingRate).AppendLine();
			stringBuilder.Append("childAgingRate".PadRight(40)).Append(this.childAgingRate).AppendLine();
			stringBuilder.Append("unwaveringPrisoners".PadRight(40)).Append(this.unwaveringPrisoners).AppendLine();
			stringBuilder.Append("childRaidersAllowed".PadRight(40)).Append(this.childRaidersAllowed).AppendLine();
			stringBuilder.Append("anomalyThreatsInactiveFraction".PadRight(40)).Append(this.anomalyThreatsInactiveFraction).AppendLine();
			stringBuilder.Append("anomalyThreatsActiveFraction".PadRight(40)).Append(this.anomalyThreatsActiveFraction).AppendLine();
			stringBuilder.Append("overrideAnomalyThreatsFraction".PadRight(40)).Append((this.overrideAnomalyThreatsFraction != null) ? this.overrideAnomalyThreatsFraction.ToString() : "null").AppendLine();
			stringBuilder.Append("studyEfficiencyFactor".PadRight(40)).Append(this.studyEfficiencyFactor).AppendLine();
			stringBuilder.Append("anomalyPlaystyleType".PadRight(40)).Append(this.anomalyPlaystyleDef.defName).AppendLine();
			return stringBuilder.ToString();
		}

		// Token: 0x06006E41 RID: 28225 RVA: 0x0026B4C4 File Offset: 0x002696C4
		[DebugOutput]
		public static void DifficultyDetails()
		{
			Log.Message(Find.Storyteller.difficulty.DebugString());
		}

		// Token: 0x06006E42 RID: 28226 RVA: 0x0026B4DC File Offset: 0x002696DC
		private void SetMinThreatPointsCurve()
		{
			if (this.minThreatPointsRangeCeiling < 35f)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Min threat points range ceiling is below ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(35f);
				defaultInterpolatedStringHandler.AppendLiteral(", resetting to ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(70f);
				Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				this.minThreatPointsRangeCeiling = 70f;
			}
			this.minThreatPointsRangeCeilingCurveCached = new SimpleCurve
			{
				{
					new CurvePoint(12f, 35f),
					true
				},
				{
					new CurvePoint(35f, this.minThreatPointsRangeCeiling),
					true
				}
			};
		}

		// Token: 0x06006E43 RID: 28227 RVA: 0x0026B580 File Offset: 0x00269780
		[CompilerGenerated]
		internal static int <CertaintyReductionFactor>g__ColonistsOfPlayerStartingIdeo|75_0()
		{
			int num = 0;
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoLodgers)
			{
				if (pawn.Ideo != null && pawn.Ideo.initialPlayerIdeo)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04004677 RID: 18039
		public const float DiseaseIntervalFactorCutoff = 100f;

		// Token: 0x04004678 RID: 18040
		public const float FriendlyFireDefault = 0.4f;

		// Token: 0x04004679 RID: 18041
		public const float AllowInstantKillChanceDefault = 1f;

		// Token: 0x0400467A RID: 18042
		public const float MaintenanceCostFactorMin = 0.01f;

		// Token: 0x0400467B RID: 18043
		public const float MinThreatPointsRangeCeilingDefault = 70f;

		// Token: 0x0400467C RID: 18044
		[Unsaved(false)]
		private SimpleCurve minThreatPointsRangeCeilingCurveCached;

		// Token: 0x0400467D RID: 18045
		public float threatScale = 1f;

		// Token: 0x0400467E RID: 18046
		public bool allowBigThreats = true;

		// Token: 0x0400467F RID: 18047
		public bool allowIntroThreats = true;

		// Token: 0x04004680 RID: 18048
		public bool allowCaveHives = true;

		// Token: 0x04004681 RID: 18049
		public bool peacefulTemples;

		// Token: 0x04004682 RID: 18050
		public bool allowViolentQuests = true;

		// Token: 0x04004683 RID: 18051
		public bool babiesAreHealthy;

		// Token: 0x04004684 RID: 18052
		public bool noBabiesOrChildren;

		// Token: 0x04004685 RID: 18053
		public bool childRaidersAllowed;

		// Token: 0x04004686 RID: 18054
		public bool childShamblersAllowed = true;

		// Token: 0x04004687 RID: 18055
		public bool predatorsHuntHumanlikes = true;

		// Token: 0x04004688 RID: 18056
		public float scariaRotChance;

		// Token: 0x04004689 RID: 18057
		public float colonistMoodOffset;

		// Token: 0x0400468A RID: 18058
		public float tradePriceFactorLoss;

		// Token: 0x0400468B RID: 18059
		public float cropYieldFactor = 1f;

		// Token: 0x0400468C RID: 18060
		public float mineYieldFactor = 1f;

		// Token: 0x0400468D RID: 18061
		public float butcherYieldFactor = 1f;

		// Token: 0x0400468E RID: 18062
		public float fishingYieldFactor = 1f;

		// Token: 0x0400468F RID: 18063
		public float researchSpeedFactor = 1f;

		// Token: 0x04004690 RID: 18064
		public float diseaseIntervalFactor = 1f;

		// Token: 0x04004691 RID: 18065
		public float enemyReproductionRateFactor = 1f;

		// Token: 0x04004692 RID: 18066
		public float playerPawnInfectionChanceFactor = 1f;

		// Token: 0x04004693 RID: 18067
		public float manhunterChanceOnDamageFactor = 1f;

		// Token: 0x04004694 RID: 18068
		public float deepDrillInfestationChanceFactor = 1f;

		// Token: 0x04004695 RID: 18069
		public float wastepackInfestationChanceFactor = 1f;

		// Token: 0x04004696 RID: 18070
		public float foodPoisonChanceFactor = 1f;

		// Token: 0x04004697 RID: 18071
		public float maintenanceCostFactor = 1f;

		// Token: 0x04004698 RID: 18072
		public float enemyDeathOnDownedChanceFactor = 1f;

		// Token: 0x04004699 RID: 18073
		public float adaptationGrowthRateFactorOverZero = 1f;

		// Token: 0x0400469A RID: 18074
		public float adaptationEffectFactor = 1f;

		// Token: 0x0400469B RID: 18075
		public float questRewardValueFactor = 1f;

		// Token: 0x0400469C RID: 18076
		public float raidLootPointsFactor = 1f;

		// Token: 0x0400469D RID: 18077
		public bool allowTraps = true;

		// Token: 0x0400469E RID: 18078
		public bool allowTurrets = true;

		// Token: 0x0400469F RID: 18079
		public bool allowMortars = true;

		// Token: 0x040046A0 RID: 18080
		public bool classicMortars;

		// Token: 0x040046A1 RID: 18081
		public bool allowExtremeWeatherIncidents = true;

		// Token: 0x040046A2 RID: 18082
		public bool fixedWealthMode;

		// Token: 0x040046A3 RID: 18083
		public float fixedWealthTimeFactor = 1f;

		// Token: 0x040046A4 RID: 18084
		public float friendlyFireChanceFactor = 0.4f;

		// Token: 0x040046A5 RID: 18085
		public float allowInstantKillChance = 1f;

		// Token: 0x040046A6 RID: 18086
		public float nomadicMineableResourcesFactor = 1f;

		// Token: 0x040046A7 RID: 18087
		public float lowPopConversionBoost = 3f;

		// Token: 0x040046A8 RID: 18088
		public float minThreatPointsRangeCeiling;

		// Token: 0x040046A9 RID: 18089
		public float childAgingRate = 4f;

		// Token: 0x040046AA RID: 18090
		public float adultAgingRate = 1f;

		// Token: 0x040046AB RID: 18091
		public bool unwaveringPrisoners = true;

		// Token: 0x040046AC RID: 18092
		public float anomalyThreatsInactiveFraction = 0.08f;

		// Token: 0x040046AD RID: 18093
		public float anomalyThreatsActiveFraction = 0.3f;

		// Token: 0x040046AE RID: 18094
		public float? overrideAnomalyThreatsFraction;

		// Token: 0x040046AF RID: 18095
		public float studyEfficiencyFactor = 1f;

		// Token: 0x040046B0 RID: 18096
		private AnomalyPlaystyleDef anomalyPlaystyleDef;
	}
}
