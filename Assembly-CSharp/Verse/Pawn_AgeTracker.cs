using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004DE RID: 1246
	public class Pawn_AgeTracker : IExposable
	{
		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x0600245A RID: 9306 RVA: 0x000DB9A3 File Offset: 0x000D9BA3
		// (set) Token: 0x0600245B RID: 9307 RVA: 0x000DB9AB File Offset: 0x000D9BAB
		public long BirthAbsTicks
		{
			get
			{
				return this.birthAbsTicksInt;
			}
			set
			{
				this.birthAbsTicksInt = value;
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x0600245C RID: 9308 RVA: 0x000DB9B4 File Offset: 0x000D9BB4
		public int AgeBiologicalYears
		{
			get
			{
				return (int)(this.ageBiologicalTicksInt / 3600000L);
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x0600245D RID: 9309 RVA: 0x000DB9C4 File Offset: 0x000D9BC4
		public float AgeBiologicalYearsFloat
		{
			get
			{
				return (float)this.ageBiologicalTicksInt / 3600000f;
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x0600245E RID: 9310 RVA: 0x000DB9D3 File Offset: 0x000D9BD3
		// (set) Token: 0x0600245F RID: 9311 RVA: 0x000DB9DB File Offset: 0x000D9BDB
		public long AgeBiologicalTicks
		{
			get
			{
				return this.ageBiologicalTicksInt;
			}
			set
			{
				this.ageBiologicalTicksInt = value;
				this.CalculateInitialGrowth();
				this.RecalculateLifeStageIndex();
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06002460 RID: 9312 RVA: 0x000DB9F0 File Offset: 0x000D9BF0
		// (set) Token: 0x06002461 RID: 9313 RVA: 0x000DB9FF File Offset: 0x000D9BFF
		public long AgeChronologicalTicks
		{
			get
			{
				return (long)GenTicks.TicksAbs - this.birthAbsTicksInt;
			}
			set
			{
				this.BirthAbsTicks = (long)GenTicks.TicksAbs - value;
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06002462 RID: 9314 RVA: 0x000DBA0F File Offset: 0x000D9C0F
		public int AgeChronologicalYears
		{
			get
			{
				return (int)(this.AgeChronologicalTicks / 3600000L);
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06002463 RID: 9315 RVA: 0x000DBA1F File Offset: 0x000D9C1F
		public float AgeChronologicalYearsFloat
		{
			get
			{
				return (float)this.AgeChronologicalTicks / 3600000f;
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06002464 RID: 9316 RVA: 0x000DBA2E File Offset: 0x000D9C2E
		public int BirthYear
		{
			get
			{
				return GenDate.Year(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06002465 RID: 9317 RVA: 0x000DBA40 File Offset: 0x000D9C40
		public int BirthDayOfSeasonZeroBased
		{
			get
			{
				return GenDate.DayOfSeason(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06002466 RID: 9318 RVA: 0x000DBA52 File Offset: 0x000D9C52
		public int BirthDayOfYear
		{
			get
			{
				return GenDate.DayOfYear(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06002467 RID: 9319 RVA: 0x000DBA64 File Offset: 0x000D9C64
		public Quadrum BirthQuadrum
		{
			get
			{
				return GenDate.Quadrum(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06002468 RID: 9320 RVA: 0x000DBA78 File Offset: 0x000D9C78
		public string AgeNumberString
		{
			get
			{
				string text = this.AgeBiologicalYearsFloat.ToStringApproxAge();
				if (this.AgeChronologicalYears != this.AgeBiologicalYears)
				{
					text = text + " (" + this.AgeChronologicalYears.ToString() + ")";
				}
				return text;
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06002469 RID: 9321 RVA: 0x000DBAC0 File Offset: 0x000D9CC0
		public string AgeTooltipString
		{
			get
			{
				int value;
				int value2;
				int value3;
				float biologicalTicksPerTick;
				this.ageBiologicalTicksInt.TicksToPeriod(out value, out value2, out value3, out biologicalTicksPerTick);
				int value4;
				int value5;
				int value6;
				((long)GenTicks.TicksAbs - this.birthAbsTicksInt).TicksToPeriod(out value4, out value5, out value6, out biologicalTicksPerTick);
				string value7 = "FullDate".Translate(Find.ActiveLanguageWorker.OrdinalNumber(this.BirthDayOfSeasonZeroBased + 1, Gender.None), this.BirthQuadrum.Label(), this.BirthYear);
				string text = "Born".Translate(value7) + "\n" + "AgeChronological".Translate(value4, value5, value6) + "\n" + "AgeBiological".Translate(value, value2, value3);
				if (Prefs.DevMode)
				{
					text += "\n\nDev mode info:";
					text = text + "\nageBiologicalTicksInt: " + this.ageBiologicalTicksInt.ToString();
					text = text + "\nbirthAbsTicksInt: " + this.birthAbsTicksInt.ToString();
					string str = text;
					string str2 = "\nBiologicalTicksPerTick: ";
					biologicalTicksPerTick = this.BiologicalTicksPerTick;
					text = str + str2 + biologicalTicksPerTick.ToString();
					text = text + "\ngrowth: " + this.growth.ToString();
					text = string.Concat(new string[]
					{
						text,
						"\nage reversal demand deadline: ",
						((int)Math.Abs(this.AgeReversalDemandedDeadlineTicks)).ToStringTicksToPeriod(true, false, true, true, false),
						(this.AgeReversalDemandedDeadlineTicks < 0L) ? " past deadline" : " in future",
						"(",
						this.AgeReversalDemandedDeadlineTicks.ToString(),
						")"
					});
					string str3 = text;
					string str4 = "\nlife stage: ";
					LifeStageDef curLifeStage = this.CurLifeStage;
					text = str3 + str4 + ((curLifeStage != null) ? curLifeStage.ToString() : null);
					text = text + "\nsterile: " + this.pawn.Sterile().ToString();
				}
				return text;
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x0600246A RID: 9322 RVA: 0x000DBCE0 File Offset: 0x000D9EE0
		public int CurLifeStageIndex
		{
			get
			{
				if (this.cachedLifeStageIndex < 0)
				{
					this.RecalculateLifeStageIndex();
				}
				return this.cachedLifeStageIndex;
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x0600246B RID: 9323 RVA: 0x000DBCF7 File Offset: 0x000D9EF7
		public LifeStageDef CurLifeStage
		{
			get
			{
				return this.CurLifeStageRace.def;
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x0600246C RID: 9324 RVA: 0x000DBD04 File Offset: 0x000D9F04
		public LifeStageAge CurLifeStageRace
		{
			get
			{
				return this.GetLifeStageAge(this.CurLifeStageIndex);
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x0600246D RID: 9325 RVA: 0x000DBD14 File Offset: 0x000D9F14
		public PawnKindLifeStage CurKindLifeStage
		{
			get
			{
				if (this.pawn.RaceProps.Humanlike)
				{
					string str = "Tried to get CurKindLifeStage from humanlike pawn ";
					Pawn pawn = this.pawn;
					Log.ErrorOnce(str + ((pawn != null) ? pawn.ToString() : null), 8888811);
					return null;
				}
				return this.pawn.kindDef.lifeStages[this.CurLifeStageIndex];
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x0600246E RID: 9326 RVA: 0x000DBD76 File Offset: 0x000D9F76
		public int MaxRaceLifeStageIndex
		{
			get
			{
				return this.pawn.RaceProps.lifeStageAges.Count - 1;
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x0600246F RID: 9327 RVA: 0x000DBD8F File Offset: 0x000D9F8F
		public float Growth
		{
			get
			{
				return this.growth;
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06002470 RID: 9328 RVA: 0x000DBD98 File Offset: 0x000D9F98
		public float AdultMinAge
		{
			get
			{
				if (this.pawn.RaceProps.Humanlike)
				{
					LifeStageAge lifeStageAge2 = this.pawn.RaceProps.lifeStageAges.FirstOrDefault((LifeStageAge lifeStageAge) => lifeStageAge.def.developmentalStage.Adult());
					if (lifeStageAge2 == null)
					{
						return 0f;
					}
					return lifeStageAge2.minAge;
				}
				else
				{
					if (this.pawn.RaceProps.lifeStageAges.Count <= 0)
					{
						return 0f;
					}
					return this.pawn.RaceProps.lifeStageAges.Last<LifeStageAge>().minAge;
				}
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06002471 RID: 9329 RVA: 0x000DBE33 File Offset: 0x000DA033
		public long AdultMinAgeTicks
		{
			get
			{
				return (long)Mathf.FloorToInt(this.AdultMinAge * 3600000f);
			}
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06002472 RID: 9330 RVA: 0x000DBE47 File Offset: 0x000DA047
		private long TicksToAdulthood
		{
			get
			{
				return this.AdultMinAgeTicks - this.AgeBiologicalTicks;
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06002473 RID: 9331 RVA: 0x000DBE56 File Offset: 0x000DA056
		public bool Adult
		{
			get
			{
				return this.TicksToAdulthood <= 0L;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06002474 RID: 9332 RVA: 0x000DBE65 File Offset: 0x000DA065
		public long AgeReversalDemandedDeadlineTicks
		{
			get
			{
				return this.ageReversalDemandedAtAgeTicks - this.AgeBiologicalTicks;
			}
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06002475 RID: 9333 RVA: 0x000DBE74 File Offset: 0x000DA074
		public Pawn_AgeTracker.AgeReversalReason LastAgeReversalReason
		{
			get
			{
				return this.lastAgeReversalReason;
			}
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06002476 RID: 9334 RVA: 0x000DBE7C File Offset: 0x000DA07C
		public float BiologicalTicksPerTick
		{
			get
			{
				if (!this.pawn.RaceProps.Humanlike)
				{
					return 1f;
				}
				if (ModsConfig.BiotechActive && this.pawn.ParentHolder != null && this.pawn.ParentHolder is Building_GrowthVat)
				{
					return 1f;
				}
				float num = 1f;
				float ageBiologicalYearsFloat = this.AgeBiologicalYearsFloat;
				if (ageBiologicalYearsFloat <= 11f)
				{
					num *= this.ChildAgingMultiplier;
				}
				else if (ageBiologicalYearsFloat >= 20f)
				{
					num *= this.AdultAgingMultiplier;
				}
				else
				{
					float t = Mathf.InverseLerp(11f, 20f, ageBiologicalYearsFloat);
					num *= Mathf.Lerp(this.ChildAgingMultiplier, this.AdultAgingMultiplier, t);
				}
				if (this.pawn.genes != null)
				{
					num *= this.pawn.genes.BiologicalAgeTickFactor;
				}
				return num;
			}
		}

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06002477 RID: 9335 RVA: 0x000DBF47 File Offset: 0x000DA147
		public float AdultAgingMultiplier
		{
			get
			{
				return Find.Storyteller.difficulty.adultAgingRate;
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06002478 RID: 9336 RVA: 0x000DBF58 File Offset: 0x000DA158
		public float ChildAgingMultiplier
		{
			get
			{
				return Find.Storyteller.difficulty.childAgingRate;
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06002479 RID: 9337 RVA: 0x000DBF69 File Offset: 0x000DA169
		public bool AtMaxGrowthTier
		{
			get
			{
				return this.GrowthTier >= GrowthUtility.GrowthTiers.Length - 1;
			}
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x0600247A RID: 9338 RVA: 0x000DBF80 File Offset: 0x000DA180
		public float PercentToNextGrowthTier
		{
			get
			{
				if (this.growthPoints <= 0f)
				{
					return 0f;
				}
				if (this.AtMaxGrowthTier)
				{
					return 1f;
				}
				int growthTier = this.GrowthTier;
				return Mathf.InverseLerp(GrowthUtility.GrowthTiers[growthTier].pointsRequirement, GrowthUtility.GrowthTiers[growthTier + 1].pointsRequirement, this.growthPoints);
			}
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x0600247B RID: 9339 RVA: 0x000DBFE4 File Offset: 0x000DA1E4
		public int GrowthTier
		{
			get
			{
				for (int i = GrowthUtility.GrowthTiers.Length - 1; i >= 0; i--)
				{
					if (this.growthPoints >= GrowthUtility.GrowthTiers[i].pointsRequirement)
					{
						return i;
					}
				}
				return 0;
			}
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x0600247C RID: 9340 RVA: 0x000DC020 File Offset: 0x000DA220
		public float GrowthPointsPerDay
		{
			get
			{
				if (!ModsConfig.BiotechActive)
				{
					return 0f;
				}
				float result;
				if (this.pawn.ParentHolder is Building_GrowthVat)
				{
					result = 2f;
				}
				else if (this.pawn.Suspended)
				{
					result = 0f;
				}
				else
				{
					Pawn_NeedsTracker needs = this.pawn.needs;
					Need_Learning need_Learning = (needs != null) ? needs.learning : null;
					if (need_Learning != null && !need_Learning.Suspended)
					{
						result = this.GrowthPointsPerDayAtLearningLevel(need_Learning.CurLevel);
					}
					else
					{
						result = 0f;
					}
				}
				return result;
			}
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x0600247D RID: 9341 RVA: 0x000DC0A2 File Offset: 0x000DA2A2
		private float GrowthPointsFactor
		{
			get
			{
				if ((float)this.AgeBiologicalYears < 7f)
				{
					return 0.75f;
				}
				return 1f;
			}
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x000DC0C0 File Offset: 0x000DA2C0
		public Pawn_AgeTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x000DC13C File Offset: 0x000DA33C
		public float LifeStageMinAge(LifeStageDef lifeStage)
		{
			foreach (LifeStageAge lifeStageAge in this.pawn.RaceProps.lifeStageAges)
			{
				if (lifeStageAge.def == lifeStage)
				{
					return lifeStageAge.minAge;
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Life stage def ");
			defaultInterpolatedStringHandler.AppendFormatted<LifeStageDef>(lifeStage);
			defaultInterpolatedStringHandler.AppendLiteral(" not found while searching for min age of ");
			defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
			Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			return 0f;
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x000DC1F0 File Offset: 0x000DA3F0
		private float GrowthPointsPerDayAtLearningLevel(float level)
		{
			return level * this.GrowthPointsFactor * this.pawn.ageTracker.ChildAgingMultiplier;
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x000DC20C File Offset: 0x000DA40C
		public void TrySimulateGrowthPoints()
		{
			if (!ModsConfig.BiotechActive || !this.pawn.RaceProps.Humanlike || this.AgeBiologicalYears >= 13)
			{
				return;
			}
			if (Pawn_AgeTracker.growthMomentAges == null)
			{
				Pawn_AgeTracker.growthMomentAges = new List<int>();
				Pawn_AgeTracker.growthMomentAges.Add(3);
				Pawn_AgeTracker.growthMomentAges.AddRange(GrowthUtility.GrowthMomentAges);
			}
			this.growthPoints = 0f;
			int ageBiologicalYears = this.AgeBiologicalYears;
			for (int i = Pawn_AgeTracker.growthMomentAges.Count - 1; i >= 0; i--)
			{
				if (ageBiologicalYears >= Pawn_AgeTracker.growthMomentAges[i])
				{
					float num = this.GrowthPointsPerDayAtLearningLevel(Rand.Range(0.2f, 0.5f)) * (((float)Pawn_AgeTracker.growthMomentAges[i] < 7f) ? 0.75f : 1f);
					int num2 = Pawn_AgeTracker.growthMomentAges[i] * 3600000;
					long num3 = this.AgeBiologicalTicks;
					while (num3 > (long)num2)
					{
						num3 -= 60000L;
						this.growthPoints += num;
					}
					break;
				}
			}
			this.growthPoints *= 0.25f;
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x000DC32C File Offset: 0x000DA52C
		public void ExposeData()
		{
			Scribe_Values.Look<long>(ref this.ageBiologicalTicksInt, "ageBiologicalTicks", 0L, false);
			Scribe_Values.Look<long>(ref this.birthAbsTicksInt, "birthAbsTicks", 0L, false);
			Scribe_Values.Look<float>(ref this.growth, "growth", -1f, false);
			Scribe_Values.Look<float>(ref this.progressToNextBiologicalTick, "progressToNextBiologicalTick", 0f, false);
			Scribe_Values.Look<long>(ref this.nextGrowthCheckTick, "nextGrowthCheckTick", -1L, false);
			Scribe_Values.Look<long>(ref this.vatGrowTicks, "vatGrowTicks", 0L, false);
			Scribe_Values.Look<long>(ref this.ageReversalDemandedAtAgeTicks, "ageReversalDemandedAtAgeTicks", long.MaxValue, false);
			Scribe_Values.Look<Pawn_AgeTracker.AgeReversalReason>(ref this.lastAgeReversalReason, "lastAgeReversalReason", Pawn_AgeTracker.AgeReversalReason.Initial, false);
			Scribe_Values.Look<bool>(ref this.initializedAgeReversalDemand, "initializedAgeReversalDemand", false, false);
			Scribe_Values.Look<bool>(ref this.lifeStageChange, "lifeStageChange", false, false);
			Scribe_Values.Look<float>(ref this.growthPoints, "growthPoints", -1f, false);
			Scribe_Values.Look<bool>(ref this.canGainGrowthPoints, "canGainGrowthPoints", true, false);
			Scribe_Values.Look<int>(ref this.lockedLifeStageIndex, "lockedLifeStageIndex", -1, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.cachedLifeStageIndex = -1;
				if (this.growthPoints < 0f)
				{
					this.TrySimulateGrowthPoints();
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.AgeBiologicalYearsFloat > 25f && (float)this.AgeReversalDemandedDeadlineTicks > 5400000f)
			{
				this.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Initial, false);
			}
			if (this.ageReversalDemandedAtAgeTicks == 9223372036854775807L)
			{
				this.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Initial, false);
			}
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x000DC4A4 File Offset: 0x000DA6A4
		private void TickBiologicalAge(int interval)
		{
			this.progressToNextBiologicalTick += this.BiologicalTicksPerTick * (float)interval;
			int num = Mathf.FloorToInt(this.progressToNextBiologicalTick);
			if (num > 0)
			{
				this.progressToNextBiologicalTick -= (float)num;
				this.ageBiologicalTicksInt += (long)num;
			}
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x000DC4F8 File Offset: 0x000DA6F8
		public void AgeTickInterval(int delta)
		{
			if (this.lifeStageChange)
			{
				this.PostResolveLifeStageChange();
			}
			int ageBiologicalYears = this.AgeBiologicalYears;
			this.TickBiologicalAge(delta);
			if (this.lockedLifeStageIndex >= 0)
			{
				return;
			}
			if (this.pawn.IsMutant && this.pawn.mutant.Def.disableAging)
			{
				return;
			}
			if ((long)Find.TickManager.TicksGame >= this.nextGrowthCheckTick)
			{
				this.CalculateGrowth(240);
			}
			if (this.pawn.IsHashIntervalTick(60000, delta))
			{
				this.CheckAgeReversalDemand();
			}
			if (ageBiologicalYears < this.AgeBiologicalYears)
			{
				this.BirthdayBiological(this.AgeBiologicalYears);
			}
			if (this.initializedAgeReversalDemand && this.pawn.MapHeld != null && this.pawn.IsHashIntervalTick(2500, delta) && ExpectationsUtility.CurrentExpectationFor(this.pawn.MapHeld).order < ThoughtDefOf.AgeReversalDemanded.minExpectation.order)
			{
				this.ageReversalDemandedAtAgeTicks += 2500L;
			}
			if (this.pawn.IsFreeColonist && this.pawn.IsHashIntervalTick(1000, delta))
			{
				if (this.CurLifeStage.developmentalStage == DevelopmentalStage.Baby)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.Babies, OpportunityType.Important);
					return;
				}
				if (this.CurLifeStage.developmentalStage == DevelopmentalStage.Child)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.Children, OpportunityType.Important);
				}
			}
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x000DC650 File Offset: 0x000DA850
		public void AgeTickMothballed(int interval)
		{
			long num = this.ageBiologicalTicksInt;
			this.TickBiologicalAge(interval);
			this.CalculateGrowth(interval);
			this.CheckAgeReversalDemand();
			for (int i = (int)(num / 3600000L) + 1; i <= this.AgeBiologicalYears; i++)
			{
				this.BirthdayBiological(i);
			}
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x000DC698 File Offset: 0x000DA898
		public void Notify_TickedInGrowthVat(int ticks)
		{
			this.vatGrowTicks += 1L;
			this.AgeTickMothballed(ticks);
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x000DC6B0 File Offset: 0x000DA8B0
		public void LockCurrentLifeStageIndex(int index)
		{
			this.lockedLifeStageIndex = index;
			this.RecalculateLifeStageIndex();
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x000DC6C0 File Offset: 0x000DA8C0
		private void CheckAgeReversalDemand()
		{
			if (!ModsConfig.IdeologyActive || this.initializedAgeReversalDemand || this.pawn.Faction != Faction.OfPlayer || this.pawn.MapHeld == null)
			{
				return;
			}
			this.ResetAgeReversalDemand(this.lastAgeReversalReason, false);
			this.initializedAgeReversalDemand = true;
		}

		// Token: 0x06002489 RID: 9353 RVA: 0x000DC710 File Offset: 0x000DA910
		private void CalculateInitialGrowth()
		{
			this.growth = Mathf.Clamp01(this.AgeBiologicalYearsFloat / this.pawn.RaceProps.lifeStageAges[this.pawn.RaceProps.lifeStageAges.Count - 1].minAge);
			this.nextGrowthCheckTick = (long)(Find.TickManager.TicksGame + 240);
		}

		// Token: 0x0600248A RID: 9354 RVA: 0x000DC778 File Offset: 0x000DA978
		private void CalculateGrowth(int interval)
		{
			if (this.growth >= 1f)
			{
				this.nextGrowthCheckTick = long.MaxValue;
				return;
			}
			this.growth += PawnUtility.BodyResourceGrowthSpeed(this.pawn) * (float)interval / Mathf.Max((float)this.AdultMinAgeTicks, 1f);
			this.growth = Mathf.Min(this.growth, 1f);
			this.nextGrowthCheckTick = (long)(Find.TickManager.TicksGame + 240);
			this.RecalculateLifeStageIndex();
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x000DC804 File Offset: 0x000DAA04
		private void RecalculateLifeStageIndex()
		{
			int num = -1;
			if (this.lockedLifeStageIndex >= 0)
			{
				num = this.lockedLifeStageIndex;
			}
			else if (this.growth < 0f)
			{
				this.CalculateInitialGrowth();
			}
			float num2;
			if (this.pawn.RaceProps.Humanlike)
			{
				num2 = (float)this.AgeBiologicalYears;
			}
			else
			{
				num2 = Mathf.Lerp(0f, this.pawn.RaceProps.lifeStageAges[this.pawn.RaceProps.lifeStageAges.Count - 1].minAge, this.growth);
			}
			if (num < 0)
			{
				List<LifeStageAge> lifeStageAges = this.pawn.RaceProps.lifeStageAges;
				for (int i = lifeStageAges.Count - 1; i >= 0; i--)
				{
					if (lifeStageAges[i].minAge <= num2 + 1E-06f)
					{
						num = i;
						break;
					}
				}
			}
			if (num == -1)
			{
				num = 0;
			}
			int index = this.cachedLifeStageIndex;
			bool flag = this.cachedLifeStageIndex != num;
			this.cachedLifeStageIndex = num;
			if (flag)
			{
				this.lifeStageChange = true;
				if (!this.pawn.RaceProps.Humanlike || ModsConfig.BiotechActive)
				{
					this.pawn.Drawer.renderer.SetAllGraphicsDirty();
					this.CheckChangePawnKindName();
				}
				LifeStageWorker worker = this.CurLifeStage.Worker;
				Pawn pawn = this.pawn;
				LifeStageAge lifeStageAge = this.GetLifeStageAge(index);
				worker.Notify_LifeStageStarted(pawn, (lifeStageAge != null) ? lifeStageAge.def : null);
				if (this.pawn.SpawnedOrAnyParentSpawned)
				{
					PawnComponentsUtility.AddAndRemoveDynamicComponents(this.pawn, false);
				}
			}
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x000DC97C File Offset: 0x000DAB7C
		private void BirthdayBiological(int birthdayAge)
		{
			Pawn_AgeTracker.tmpHediffsGained.Clear();
			Pawn_AgeTracker.tmpEnabledWorkTypes.Clear();
			bool flag = (float)birthdayAge == this.AdultMinAge;
			bool flag2 = this.pawn.DevelopmentalStage.Child() && (float)birthdayAge == this.CurLifeStageRace.minAge;
			if (flag || flag2)
			{
				this.RecalculateLifeStageIndex();
			}
			float age = (float)birthdayAge / this.pawn.GetStatValue(StatDefOf.LifespanFactor, true, -1);
			foreach (HediffGiver_Birthday hediffGiver_Birthday in AgeInjuryUtility.RandomHediffsToGainOnBirthday(this.pawn, age))
			{
				if (hediffGiver_Birthday.TryApply(this.pawn, null))
				{
					Pawn_AgeTracker.tmpHediffsGained.Add(hediffGiver_Birthday.hediff);
				}
			}
			Building_Bed ownedBed = this.pawn.ownership.OwnedBed;
			if (ownedBed != null && this.pawn.ageTracker.CurLifeStage.bodySizeFactor > ownedBed.def.building.bed_maxBodySize)
			{
				this.pawn.ownership.UnclaimBed();
			}
			if (this.pawn.RaceProps.Humanlike)
			{
				List<LifeStageWorkSettings> lifeStageWorkSettings = this.pawn.RaceProps.lifeStageWorkSettings;
				for (int i = 0; i < lifeStageWorkSettings.Count; i++)
				{
					if (lifeStageWorkSettings[i].minAge == birthdayAge)
					{
						Pawn_AgeTracker.tmpEnabledWorkTypes.Add(lifeStageWorkSettings[i].workType);
					}
				}
				if (Pawn_AgeTracker.tmpEnabledWorkTypes.Count > 0)
				{
					this.pawn.Notify_DisabledWorkTypesChanged();
				}
				int passionChoiceCount;
				int num;
				int num2;
				this.TryChildGrowthMoment(birthdayAge, out passionChoiceCount, out num, out num2);
				bool flag3 = !flag2 && (!Pawn_AgeTracker.tmpEnabledWorkTypes.NullOrEmpty<WorkTypeDef>() || num2 > 0 || num > 0);
				if (!PawnUtility.ShouldSendNotificationAbout(this.pawn) || this.pawn.Faction != Faction.OfPlayer || this.pawn.IsQuestLodger())
				{
					if (num2 > 0)
					{
						SkillDef skillDef = ChoiceLetter_GrowthMoment.PassionOptions(this.pawn, num2, true).FirstOrFallback(null);
						if (skillDef != null)
						{
							SkillRecord skill = this.pawn.skills.GetSkill(skillDef);
							if (skill != null)
							{
								skill.passion = skill.passion.IncrementPassion();
							}
						}
					}
					if (num > 0)
					{
						Trait trait = PawnGenerator.GenerateTraitsFor(this.pawn, 1, null, true).FirstOrFallback(null);
						if (trait != null)
						{
							this.pawn.story.traits.GainTrait(trait, false);
						}
					}
				}
				else if (Pawn_AgeTracker.tmpHediffsGained.Count > 0 || flag3)
				{
					TaggedString taggedString = "LetterBirthdayBiological".Translate(this.pawn, birthdayAge);
					if (Pawn_AgeTracker.tmpHediffsGained.Count > 0)
					{
						taggedString += "\n\n" + "BirthdayBiologicalAgeInjuries".Translate(this.pawn);
						taggedString += ":\n\n" + (from h in Pawn_AgeTracker.tmpHediffsGained
						select h.LabelCap.Resolve()).ToLineList("  - ", false);
					}
					if (ModsConfig.BiotechActive && flag3 && this.pawn.Spawned && (this.pawn.DevelopmentalStage.Juvenile() || flag))
					{
						EffecterDefOf.Birthday.SpawnAttached(this.pawn, this.pawn.Map, 1f);
					}
					if (Pawn_AgeTracker.tmpHediffsGained.Count > 0)
					{
						LetterDef letterDef = LetterDefOf.NegativeEvent;
						Find.LetterStack.ReceiveLetter("LetterLabelBirthday".Translate(), taggedString, letterDef, this.pawn, null, null, null, null, 0, true);
					}
					if (ModsConfig.BiotechActive && flag3)
					{
						Name name = this.pawn.Name;
						NameTriple nameTriple = this.pawn.Name as NameTriple;
						if (nameTriple != null && !nameTriple.NickSet)
						{
							Rand.PushState(Gen.HashCombine<int>(this.pawn.thingIDNumber, birthdayAge));
							try
							{
								if (Rand.Chance(0.5f))
								{
									string name2 = PawnNameDatabaseShuffled.BankOf(PawnNameCategory.HumanStandard).GetName(PawnNameSlot.Nick, this.pawn.gender, true);
									this.pawn.Name = new NameTriple(nameTriple.First, name2, nameTriple.Last);
								}
							}
							finally
							{
								Rand.PopState();
							}
						}
						LetterDef letterDef = flag ? LetterDefOf.ChildToAdult : LetterDefOf.ChildBirthday;
						ChoiceLetter_GrowthMoment choiceLetter_GrowthMoment = (ChoiceLetter_GrowthMoment)LetterMaker.MakeLetter(letterDef);
						List<string> enabledWorkTypes = (from w in Pawn_AgeTracker.tmpEnabledWorkTypes
						select w.labelShort.CapitalizeFirst()).ToList<string>();
						choiceLetter_GrowthMoment.ConfigureGrowthLetter(this.pawn, passionChoiceCount, num, num2, enabledWorkTypes, name);
						choiceLetter_GrowthMoment.Label = (flag ? "LetterLabelBecameAdult".Translate(this.pawn) : "BirthdayGrowthMoment".Translate(this.pawn, name.ToStringShort.Named("PAWNNAME")));
						choiceLetter_GrowthMoment.StartTimeout(120000);
						this.canGainGrowthPoints = false;
						Find.LetterStack.ReceiveLetter(choiceLetter_GrowthMoment, null, 0, true);
					}
					Pawn_AgeTracker.tmpEnabledWorkTypes.Clear();
				}
			}
			Pawn_AgeTracker.tmpHediffsGained.Clear();
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x000DCEE0 File Offset: 0x000DB0E0
		public void TryChildGrowthMoment(int birthdayAge, out int newPassionOptions, out int newTraitOptions, out int passionGainsCount)
		{
			newPassionOptions = 0;
			newTraitOptions = 0;
			passionGainsCount = 0;
			if (ModsConfig.BiotechActive && GrowthUtility.IsGrowthBirthday(birthdayAge))
			{
				int growthTier = this.GrowthTier;
				newPassionOptions = (int)GrowthUtility.GrowthTiers[growthTier].passionChoices;
				newTraitOptions = (int)GrowthUtility.GrowthTiers[growthTier].traitChoices;
				passionGainsCount = Mathf.Min(this.pawn.skills.skills.Count((SkillRecord s) => s.passion < Passion.Major), GrowthUtility.GrowthTiers[growthTier].PassionGainFor(this.pawn));
			}
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x000DCF88 File Offset: 0x000DB188
		public void ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason reason, bool cancelInitialization = false)
		{
			int num;
			if (reason == Pawn_AgeTracker.AgeReversalReason.Recruited)
			{
				num = this.RecruitedPawnAgeReversalDemandInDays.RandomInRange;
			}
			else if (reason == Pawn_AgeTracker.AgeReversalReason.ViaTreatment)
			{
				num = 60;
			}
			else
			{
				num = this.NewPawnAgeReversalDemandInDays.RandomInRange;
			}
			long num2 = (long)(num * 60000);
			long num3 = Math.Max(this.AgeBiologicalTicks, 90000000L) + num2;
			if (reason == Pawn_AgeTracker.AgeReversalReason.Recruited && num3 < this.ageReversalDemandedAtAgeTicks)
			{
				return;
			}
			this.ageReversalDemandedAtAgeTicks = num3;
			this.lastAgeReversalReason = reason;
			if (cancelInitialization)
			{
				this.initializedAgeReversalDemand = false;
			}
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x000DD008 File Offset: 0x000DB208
		public void PostResolveLifeStageChange()
		{
			this.pawn.health.CheckForStateChange(null, null);
			this.lifeStageChange = false;
			MeditationFocusTypeAvailabilityCache.ClearFor(this.pawn);
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x000DD041 File Offset: 0x000DB241
		public void DebugForceAgeReversalDemandNow()
		{
			this.ageReversalDemandedAtAgeTicks = this.AgeBiologicalTicks;
		}

		// Token: 0x06002491 RID: 9361 RVA: 0x000DD04F File Offset: 0x000DB24F
		public void DebugResetAgeReversalDemand()
		{
			this.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Initial, false);
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x000DD059 File Offset: 0x000DB259
		public void Notify_IdeoChanged()
		{
			Ideo ideo = this.pawn.Ideo;
			if (ideo != null && ideo.HasPrecept(PreceptDefOf.AgeReversal_Demanded))
			{
				this.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Recruited, false);
			}
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x000DD081 File Offset: 0x000DB281
		public void DebugForceBirthdayBiological()
		{
			this.BirthdayBiological(this.AgeBiologicalYears);
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x000DD090 File Offset: 0x000DB290
		public void DebugSetAge(long finalAgeTicks)
		{
			long num = finalAgeTicks - this.AgeBiologicalTicks;
			while (num > 0L)
			{
				int ageBiologicalYears = this.AgeBiologicalYears;
				long num2 = Pawn_AgeTracker.<DebugSetAge>g__NextBirthdayTick|126_0(this.AgeBiologicalTicks) - this.AgeBiologicalTicks;
				long num3 = (num2 > num) ? num : num2;
				this.AgeBiologicalTicks += num3;
				num -= num3;
				if (this.AgeBiologicalYears > ageBiologicalYears)
				{
					this.BirthdayBiological(this.AgeBiologicalYears);
				}
			}
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x000DD0F7 File Offset: 0x000DB2F7
		public void DebugSetGrowth(float val)
		{
			this.growth = val;
			this.RecalculateLifeStageIndex();
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x000DD108 File Offset: 0x000DB308
		public void CheckChangePawnKindName()
		{
			NameSingle nameSingle = this.pawn.Name as NameSingle;
			if (nameSingle == null || !nameSingle.Numerical)
			{
				return;
			}
			string b = this.pawn.KindLabel.CapitalizeFirst();
			if (nameSingle.NameWithoutNumber == b)
			{
				return;
			}
			int number = nameSingle.Number;
			string text = this.pawn.KindLabel.CapitalizeFirst() + " " + number.ToString();
			if (!NameUseChecker.NameSingleIsUsed(text))
			{
				this.pawn.Name = new NameSingle(text, true);
				return;
			}
			this.pawn.Name = PawnBioAndNameGenerator.GeneratePawnName(this.pawn, NameStyle.Numeric, null, false, null);
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x000DD1B0 File Offset: 0x000DB3B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private LifeStageAge GetLifeStageAge(int index)
		{
			List<LifeStageAge> lifeStageAges = this.pawn.def.race.lifeStageAges;
			if (index < 0 || index >= lifeStageAges.Count)
			{
				return null;
			}
			return lifeStageAges[index];
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x000DD205 File Offset: 0x000DB405
		[CompilerGenerated]
		internal static long <DebugSetAge>g__NextBirthdayTick|126_0(long ageTicks)
		{
			return (ageTicks / 3600000L + 1L) * 3600000L;
		}

		// Token: 0x04001A92 RID: 6802
		private Pawn pawn;

		// Token: 0x04001A93 RID: 6803
		private long ageBiologicalTicksInt = -1L;

		// Token: 0x04001A94 RID: 6804
		private long birthAbsTicksInt = -1L;

		// Token: 0x04001A95 RID: 6805
		private long nextGrowthCheckTick = -1L;

		// Token: 0x04001A96 RID: 6806
		private float growth = -1f;

		// Token: 0x04001A97 RID: 6807
		private float progressToNextBiologicalTick;

		// Token: 0x04001A98 RID: 6808
		private long ageReversalDemandedAtAgeTicks;

		// Token: 0x04001A99 RID: 6809
		public long vatGrowTicks;

		// Token: 0x04001A9A RID: 6810
		private Pawn_AgeTracker.AgeReversalReason lastAgeReversalReason;

		// Token: 0x04001A9B RID: 6811
		private bool initializedAgeReversalDemand;

		// Token: 0x04001A9C RID: 6812
		private bool lifeStageChange;

		// Token: 0x04001A9D RID: 6813
		public float growthPoints = -1f;

		// Token: 0x04001A9E RID: 6814
		public bool canGainGrowthPoints = true;

		// Token: 0x04001A9F RID: 6815
		public int lockedLifeStageIndex = -1;

		// Token: 0x04001AA0 RID: 6816
		private int cachedLifeStageIndex = -1;

		// Token: 0x04001AA1 RID: 6817
		private const float BornAtLongitude = 0f;

		// Token: 0x04001AA2 RID: 6818
		private const int GrowthInterval = 240;

		// Token: 0x04001AA3 RID: 6819
		public const int AgeReversalDemandMinAgeYears = 25;

		// Token: 0x04001AA4 RID: 6820
		private const int TreatedPawnAgeReversalDemandInDays = 60;

		// Token: 0x04001AA5 RID: 6821
		public const int MinGrowthBirthday = 3;

		// Token: 0x04001AA6 RID: 6822
		public const int MaxGrowthBirthday = 13;

		// Token: 0x04001AA7 RID: 6823
		private const float NicknameGainChance = 0.5f;

		// Token: 0x04001AA8 RID: 6824
		private readonly IntRange NewPawnAgeReversalDemandInDays = new IntRange(20, 40);

		// Token: 0x04001AA9 RID: 6825
		private readonly IntRange RecruitedPawnAgeReversalDemandInDays = new IntRange(15, 20);

		// Token: 0x04001AAA RID: 6826
		private const int ColonistDevelopmentStageLessonInterval = 1000;

		// Token: 0x04001AAB RID: 6827
		private const float GrowthPointsSuspended = 2f;

		// Token: 0x04001AAC RID: 6828
		private const float YoungAgeCutoff = 7f;

		// Token: 0x04001AAD RID: 6829
		private const float GrowthPointsFactor_Young = 0.75f;

		// Token: 0x04001AAE RID: 6830
		private const float GeneratedGrowthPointsFactor = 0.25f;

		// Token: 0x04001AAF RID: 6831
		private const float MaxAgeChildFactorAge = 11f;

		// Token: 0x04001AB0 RID: 6832
		private const float MinAgeAdultFactorAge = 20f;

		// Token: 0x04001AB1 RID: 6833
		private static List<int> growthMomentAges = null;

		// Token: 0x04001AB2 RID: 6834
		private static List<WorkTypeDef> tmpEnabledWorkTypes = new List<WorkTypeDef>();

		// Token: 0x04001AB3 RID: 6835
		private static List<HediffDef> tmpHediffsGained = new List<HediffDef>();

		// Token: 0x0200272A RID: 10026
		public enum AgeReversalReason
		{
			// Token: 0x0400A058 RID: 41048
			Initial,
			// Token: 0x0400A059 RID: 41049
			Recruited,
			// Token: 0x0400A05A RID: 41050
			ViaTreatment
		}
	}
}
