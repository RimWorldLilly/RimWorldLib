using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001141 RID: 4417
	public class GameCondition_Flashstorm : GameCondition
	{
		// Token: 0x17001302 RID: 4866
		// (get) Token: 0x06006EEB RID: 28395 RVA: 0x0026DCD7 File Offset: 0x0026BED7
		public int AreaRadius
		{
			get
			{
				return this.areaRadius;
			}
		}

		// Token: 0x06006EEC RID: 28396 RVA: 0x0026DCE0 File Offset: 0x0026BEE0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec2>(ref this.centerLocation, "centerLocation", default(IntVec2), false);
			Scribe_Values.Look<int>(ref this.areaRadius, "areaRadius", 0, false);
			Scribe_Values.Look<IntRange>(ref this.areaRadiusOverride, "areaRadiusOverride", default(IntRange), false);
			Scribe_Values.Look<int>(ref this.nextLightningTicks, "nextLightningTicks", 0, false);
			Scribe_Values.Look<IntRange>(ref this.initialStrikeDelay, "initialStrikeDelay", default(IntRange), false);
			Scribe_Values.Look<bool>(ref this.ambientSound, "ambientSound", false, false);
			Scribe_Values.Look<bool>(ref this.avoidConditionCauser, "avoidConditionCauser", false, false);
		}

		// Token: 0x06006EED RID: 28397 RVA: 0x0026DD8C File Offset: 0x0026BF8C
		public override void Init()
		{
			base.Init();
			this.areaRadius = ((this.areaRadiusOverride == IntRange.Zero) ? GameCondition_Flashstorm.AreaRadiusRange.RandomInRange : this.areaRadiusOverride.RandomInRange);
			this.nextLightningTicks = Find.TickManager.TicksGame + this.initialStrikeDelay.RandomInRange;
			if (this.centerLocation.IsInvalid)
			{
				this.FindGoodCenterLocation();
			}
		}

		// Token: 0x06006EEE RID: 28398 RVA: 0x0026DE00 File Offset: 0x0026C000
		public override void GameConditionTick()
		{
			if (Find.TickManager.TicksGame > this.nextLightningTicks)
			{
				Vector2 vector = Rand.UnitVector2 * Rand.Range(0f, (float)this.areaRadius);
				IntVec3 intVec = new IntVec3((int)Math.Round((double)vector.x) + this.centerLocation.x, 0, (int)Math.Round((double)vector.y) + this.centerLocation.z);
				if (this.IsGoodLocationForStrike(intVec))
				{
					base.SingleMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(base.SingleMap, intVec));
					this.nextLightningTicks = Find.TickManager.TicksGame + GameCondition_Flashstorm.TicksBetweenStrikes.RandomInRange;
				}
			}
			if (this.ambientSound)
			{
				if (this.soundSustainer == null || this.soundSustainer.Ended)
				{
					this.soundSustainer = SoundDefOf.FlashstormAmbience.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(this.centerLocation.ToIntVec3, base.SingleMap, false), MaintenanceType.PerTick));
					return;
				}
				this.soundSustainer.Maintain();
			}
		}

		// Token: 0x06006EEF RID: 28399 RVA: 0x0026DF16 File Offset: 0x0026C116
		public override void End()
		{
			base.SingleMap.weatherDecider.DisableRainFor(30000);
			base.End();
		}

		// Token: 0x06006EF0 RID: 28400 RVA: 0x0026DF34 File Offset: 0x0026C134
		private void FindGoodCenterLocation()
		{
			if (base.SingleMap.Size.x <= 16 || base.SingleMap.Size.z <= 16)
			{
				throw new Exception("Map too small for flashstorm.");
			}
			for (int i = 0; i < 10; i++)
			{
				this.centerLocation = new IntVec2(Rand.Range(8, base.SingleMap.Size.x - 8), Rand.Range(8, base.SingleMap.Size.z - 8));
				if (this.IsGoodCenterLocation(this.centerLocation))
				{
					break;
				}
			}
		}

		// Token: 0x06006EF1 RID: 28401 RVA: 0x0026DFCC File Offset: 0x0026C1CC
		private bool IsGoodLocationForStrike(IntVec3 loc)
		{
			return loc.InBounds(base.SingleMap) && !loc.Roofed(base.SingleMap) && loc.Standable(base.SingleMap) && (!this.avoidConditionCauser || this.conditionCauser == null || !this.conditionCauser.OccupiedRect().ExpandedBy(2).Contains(loc));
		}

		// Token: 0x06006EF2 RID: 28402 RVA: 0x0026E038 File Offset: 0x0026C238
		private bool IsGoodCenterLocation(IntVec2 loc)
		{
			int num = 0;
			int num2 = (int)(3.1415927f * (float)this.areaRadius * (float)this.areaRadius / 2f);
			foreach (IntVec3 loc2 in this.GetPotentiallyAffectedCells(loc))
			{
				if (this.IsGoodLocationForStrike(loc2))
				{
					num++;
				}
				if (num >= num2)
				{
					break;
				}
			}
			return num >= num2;
		}

		// Token: 0x06006EF3 RID: 28403 RVA: 0x0026E0B8 File Offset: 0x0026C2B8
		private IEnumerable<IntVec3> GetPotentiallyAffectedCells(IntVec2 center)
		{
			GameCondition_Flashstorm.<GetPotentiallyAffectedCells>d__21 <GetPotentiallyAffectedCells>d__ = new GameCondition_Flashstorm.<GetPotentiallyAffectedCells>d__21(-2);
			<GetPotentiallyAffectedCells>d__.<>4__this = this;
			<GetPotentiallyAffectedCells>d__.<>3__center = center;
			return <GetPotentiallyAffectedCells>d__;
		}

		// Token: 0x040046F0 RID: 18160
		private static readonly IntRange AreaRadiusRange = new IntRange(45, 60);

		// Token: 0x040046F1 RID: 18161
		private static readonly IntRange TicksBetweenStrikes = new IntRange(320, 800);

		// Token: 0x040046F2 RID: 18162
		private const int RainDisableTicksAfterConditionEnds = 30000;

		// Token: 0x040046F3 RID: 18163
		private const int AvoidConditionCauserExpandRect = 2;

		// Token: 0x040046F4 RID: 18164
		public IntVec2 centerLocation = IntVec2.Invalid;

		// Token: 0x040046F5 RID: 18165
		public IntRange areaRadiusOverride = IntRange.Zero;

		// Token: 0x040046F6 RID: 18166
		public IntRange initialStrikeDelay = IntRange.Zero;

		// Token: 0x040046F7 RID: 18167
		public bool ambientSound;

		// Token: 0x040046F8 RID: 18168
		private int areaRadius;

		// Token: 0x040046F9 RID: 18169
		private int nextLightningTicks;

		// Token: 0x040046FA RID: 18170
		private Sustainer soundSustainer;

		// Token: 0x040046FB RID: 18171
		public bool avoidConditionCauser;
	}
}
