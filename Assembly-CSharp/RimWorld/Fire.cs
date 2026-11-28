using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B18 RID: 6936
	public class Fire : AttachableThing, ISizeReporter
	{
		// Token: 0x17001FF5 RID: 8181
		// (get) Token: 0x0600B15C RID: 45404 RVA: 0x003E7CDE File Offset: 0x003E5EDE
		public int TicksSinceSpawn
		{
			get
			{
				return this.ticksSinceSpawn;
			}
		}

		// Token: 0x17001FF6 RID: 8182
		// (get) Token: 0x0600B15D RID: 45405 RVA: 0x003E7CE6 File Offset: 0x003E5EE6
		public override string Label
		{
			get
			{
				if (this.parent != null)
				{
					return "FireOn".Translate(this.parent.LabelCap, this.parent);
				}
				return this.def.label;
			}
		}

		// Token: 0x17001FF7 RID: 8183
		// (get) Token: 0x0600B15E RID: 45406 RVA: 0x003E7D28 File Offset: 0x003E5F28
		public override string InspectStringAddon
		{
			get
			{
				return "Burning".Translate() + " (" + "FireSizeLower".Translate((this.fireSize * 100f).ToString("F0")) + ")";
			}
		}

		// Token: 0x17001FF8 RID: 8184
		// (get) Token: 0x0600B15F RID: 45407 RVA: 0x003E7D88 File Offset: 0x003E5F88
		private float SpreadInterval
		{
			get
			{
				float num = 150f - (this.fireSize - 1f) * 40f;
				if (num < 75f)
				{
					num = 75f;
				}
				return num;
			}
		}

		// Token: 0x17001FF9 RID: 8185
		// (get) Token: 0x0600B160 RID: 45408 RVA: 0x003E7DBD File Offset: 0x003E5FBD
		public override int UpdateRateTicks
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x0600B161 RID: 45409 RVA: 0x003E7DC1 File Offset: 0x003E5FC1
		public override bool ShouldBeLitNow()
		{
			return true;
		}

		// Token: 0x0600B162 RID: 45410 RVA: 0x003E7DC4 File Offset: 0x003E5FC4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceSpawn, "ticksSinceSpawn", 0, false);
			Scribe_Values.Look<float>(ref this.fireSize, "fireSize", 0f, false);
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
		}

		// Token: 0x0600B163 RID: 45411 RVA: 0x003E7E10 File Offset: 0x003E6010
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.RecalcPathsOnAndAroundMe(map);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.HomeArea, this, OpportunityType.Important);
			this.ticksSinceSpread = (int)(this.SpreadInterval * Rand.Value);
		}

		// Token: 0x0600B164 RID: 45412 RVA: 0x003E7E40 File Offset: 0x003E6040
		public float CurrentSize()
		{
			return this.fireSize;
		}

		// Token: 0x0600B165 RID: 45413 RVA: 0x003E7E48 File Offset: 0x003E6048
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.sustainer != null)
			{
				if (this.sustainer.externalParams.sizeAggregator == null)
				{
					this.sustainer.externalParams.sizeAggregator = new SoundSizeAggregator();
				}
				this.sustainer.externalParams.sizeAggregator.RemoveReporter(this);
			}
			Map map = base.Map;
			base.DeSpawn(mode);
			this.RecalcPathsOnAndAroundMe(map);
		}

		// Token: 0x0600B166 RID: 45414 RVA: 0x003E7EB0 File Offset: 0x003E60B0
		private void RecalcPathsOnAndAroundMe(Map map)
		{
			IntVec3[] adjacentCellsAndInside = GenAdj.AdjacentCellsAndInside;
			for (int i = 0; i < adjacentCellsAndInside.Length; i++)
			{
				IntVec3 c = base.Position + adjacentCellsAndInside[i];
				if (c.InBounds(map))
				{
					map.pathing.RecalculatePerceivedPathCostAt(c);
				}
			}
		}

		// Token: 0x0600B167 RID: 45415 RVA: 0x003E7EFC File Offset: 0x003E60FC
		public override void AttachTo(Thing newParent)
		{
			base.AttachTo(newParent);
			Pawn pawn = newParent as Pawn;
			if (pawn != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.WasOnFire, new object[]
				{
					pawn
				});
			}
		}

		// Token: 0x0600B168 RID: 45416 RVA: 0x003E7F2F File Offset: 0x003E612F
		protected override void Tick()
		{
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.Maintain();
			}
		}

		// Token: 0x0600B169 RID: 45417 RVA: 0x003E7F54 File Offset: 0x003E6154
		protected override void TickInterval(int delta)
		{
			this.ticksSinceSpawn += delta;
			if (Fire.lastFireCountUpdateTick != Find.TickManager.TicksGame)
			{
				Fire.fireCount = base.Map.listerThings.ThingsOfDef(this.def).Count;
				Fire.lastFireCountUpdateTick = Find.TickManager.TicksGame;
			}
			if ((this.sustainer == null || this.sustainer.Ended) && !base.Position.Fogged(base.Map))
			{
				SoundInfo info = SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.PerTick);
				this.sustainer = SustainerAggregatorUtility.AggregateOrSpawnSustainerFor(this, SoundDefOf.FireBurning, info);
			}
			using (ProfilerBlock.Scope("Spawn particles"))
			{
				this.ticksUntilSmoke -= delta;
				if (this.ticksUntilSmoke <= 0)
				{
					this.SpawnSmokeParticles();
				}
				if (Fire.fireCount < 15 && this.fireSize > 0.7f && Rand.Value < this.fireSize * 0.01f)
				{
					FleckMaker.ThrowMicroSparks(this.DrawPos, base.Map);
				}
			}
			using (ProfilerBlock.Scope("Spread"))
			{
				if (this.fireSize > 1f)
				{
					this.ticksSinceSpread += delta;
					if ((float)this.ticksSinceSpread >= this.SpreadInterval)
					{
						this.TrySpread();
						this.ticksSinceSpread = 0;
					}
				}
			}
			if (this.IsHashIntervalTick(150, delta))
			{
				this.DoComplexCalcs();
			}
			if (this.ticksSinceSpawn >= 7500)
			{
				this.TryBurnFloor();
			}
		}

		// Token: 0x0600B16A RID: 45418 RVA: 0x003E810C File Offset: 0x003E630C
		private void SpawnSmokeParticles()
		{
			if (Fire.fireCount < 15)
			{
				FleckMaker.ThrowSmoke(this.DrawPos, base.Map, this.fireSize);
			}
			if (this.fireSize > 0.5f && this.parent == null)
			{
				FleckMaker.ThrowFireGlow(base.Position.ToVector3Shifted(), base.Map, this.fireSize);
			}
			float num = this.fireSize / 2f;
			if (num > 1f)
			{
				num = 1f;
			}
			num = 1f - num;
			this.ticksUntilSmoke = Fire.SmokeIntervalRange.Lerped(num) + (int)(10f * Rand.Value);
		}

		// Token: 0x0600B16B RID: 45419 RVA: 0x003E81B0 File Offset: 0x003E63B0
		private void DoComplexCalcs()
		{
			bool flag = false;
			Fire.flammableList.Clear();
			this.flammabilityMax = 0f;
			if (!base.Position.GetTerrain(base.Map).extinguishesFire)
			{
				if (this.parent == null)
				{
					if (base.Position.TerrainFlammableNow(base.Map))
					{
						this.flammabilityMax = base.Position.TerrainFlammability(base.Map);
					}
					List<Thing> list = base.Map.thingGrid.ThingsListAt(base.Position);
					for (int i = 0; i < list.Count; i++)
					{
						Thing thing = list[i];
						if (thing is Building_Door)
						{
							flag = true;
						}
						float statValue = thing.GetStatValue(StatDefOf.Flammability, true, -1);
						if (statValue >= 0.01f)
						{
							Fire.flammableList.Add(list[i]);
							if (statValue > this.flammabilityMax)
							{
								this.flammabilityMax = statValue;
							}
							if (this.parent == null && this.fireSize > 0.4f && list[i].def.category == ThingCategory.Pawn && Rand.Chance(FireUtility.ChanceToAttachFireCumulative(list[i], 150f)))
							{
								list[i].TryAttachFire(this.fireSize * 0.2f, this.instigator);
							}
						}
					}
				}
				else
				{
					Fire.flammableList.Add(this.parent);
					this.flammabilityMax = this.parent.GetStatValue(StatDefOf.Flammability, true, -1);
				}
			}
			if (this.flammabilityMax < 0.01f)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			Thing thing2;
			if (this.parent != null)
			{
				thing2 = this.parent;
			}
			else if (Fire.flammableList.Count > 0)
			{
				thing2 = Fire.flammableList.RandomElement<Thing>();
			}
			else
			{
				thing2 = null;
			}
			if (thing2 != null && (this.fireSize >= 0.4f || thing2 == this.parent || thing2.def.category != ThingCategory.Pawn))
			{
				this.DoFireDamage(thing2);
			}
			if (base.Spawned)
			{
				float num = this.fireSize * 160f;
				if (flag)
				{
					num *= 0.15f;
				}
				GenTemperature.PushHeat(base.Position, base.Map, num);
				if (Rand.Value < 0.4f)
				{
					float radius = this.fireSize * 3f;
					WeatherBuildupUtility.AddSnowRadial(base.Position, base.Map, radius, -(this.fireSize * 0.1f));
				}
				float effectiveVacuumForFire = FireUtility.GetEffectiveVacuumForFire(base.Position, base.Map);
				this.fireSize += 0.00055f * this.flammabilityMax * 150f * (1f - effectiveVacuumForFire);
				if (this.fireSize > 1.75f)
				{
					this.fireSize = 1.75f;
				}
				if (base.Map.weatherManager.RainRate > 0.01f && this.VulnerableToRain() && Rand.Value < 6f)
				{
					base.TakeDamage(new DamageInfo(DamageDefOf.Extinguish, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
				}
				if (effectiveVacuumForFire > 0f)
				{
					base.TakeDamage(new DamageInfo(DamageDefOf.Extinguish, 20f * effectiveVacuumForFire, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
				}
			}
		}

		// Token: 0x0600B16C RID: 45420 RVA: 0x003E84EE File Offset: 0x003E66EE
		private void TryBurnFloor()
		{
			if (this.parent != null || !base.Spawned)
			{
				return;
			}
			if (base.Position.TerrainFlammableNow(base.Map))
			{
				base.Map.terrainGrid.Notify_TerrainBurned(base.Position);
			}
		}

		// Token: 0x0600B16D RID: 45421 RVA: 0x003E852C File Offset: 0x003E672C
		private bool VulnerableToRain()
		{
			if (!base.Spawned)
			{
				return false;
			}
			RoofDef roofDef = base.Map.roofGrid.RoofAt(base.Position);
			if (roofDef == null)
			{
				return true;
			}
			if (roofDef.isThickRoof)
			{
				return false;
			}
			Thing edifice = base.Position.GetEdifice(base.Map);
			return edifice != null && edifice.def.holdsRoof;
		}

		// Token: 0x0600B16E RID: 45422 RVA: 0x003E858C File Offset: 0x003E678C
		private void DoFireDamage(Thing targ)
		{
			int num = GenMath.RoundRandom(Mathf.Clamp(0.0125f + 0.0036f * this.fireSize, 0.0125f, 0.05f) * 150f);
			if (num < 1)
			{
				num = 1;
			}
			Pawn pawn = targ as Pawn;
			if (pawn != null)
			{
				BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(pawn, RulePackDefOf.DamageEvent_Fire, null);
				Find.BattleLog.Add(battleLogEntry_DamageTaken);
				DamageInfo dinfo = new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this.instigator ?? this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
				dinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
				targ.TakeDamage(dinfo).AssociateWithLog(battleLogEntry_DamageTaken);
				Apparel apparel;
				if (pawn.apparel != null && pawn.apparel.WornApparel.TryRandomElement(out apparel))
				{
					apparel.TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this.instigator ?? this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
					return;
				}
			}
			else
			{
				targ.TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this.instigator ?? this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
		}

		// Token: 0x0600B16F RID: 45423 RVA: 0x003E86B4 File Offset: 0x003E68B4
		protected void TrySpread()
		{
			IntVec3 intVec;
			bool flag;
			if (Rand.Chance(0.8f))
			{
				intVec = base.Position + GenRadial.ManualRadialPattern[Rand.RangeInclusive(1, 8)];
				flag = true;
			}
			else
			{
				intVec = base.Position + GenRadial.ManualRadialPattern[Rand.RangeInclusive(10, 20)];
				flag = false;
			}
			if (!intVec.InBounds(base.Map))
			{
				return;
			}
			if (Rand.Chance(FireUtility.ChanceToStartFireIn(intVec, base.Map, null)))
			{
				if (!flag)
				{
					CellRect startRect = CellRect.SingleCell(base.Position);
					CellRect endRect = CellRect.SingleCell(intVec);
					if (!GenSight.LineOfSight(base.Position, intVec, base.Map, startRect, endRect, null, false))
					{
						return;
					}
					((Spark)GenSpawn.Spawn(ThingDefOf.Spark, base.Position, base.Map, WipeMode.Vanish)).Launch(this, intVec, intVec, ProjectileHitFlags.All, false, null);
					return;
				}
				else
				{
					FireUtility.TryStartFireIn(intVec, base.Map, 0.1f, this.instigator, null);
				}
			}
		}

		// Token: 0x04006671 RID: 26225
		private int ticksSinceSpawn;

		// Token: 0x04006672 RID: 26226
		public float fireSize = 0.1f;

		// Token: 0x04006673 RID: 26227
		private int ticksSinceSpread;

		// Token: 0x04006674 RID: 26228
		private float flammabilityMax = 0.5f;

		// Token: 0x04006675 RID: 26229
		public Thing instigator;

		// Token: 0x04006676 RID: 26230
		private int ticksUntilSmoke;

		// Token: 0x04006677 RID: 26231
		private Sustainer sustainer;

		// Token: 0x04006678 RID: 26232
		private static List<Thing> flammableList = new List<Thing>();

		// Token: 0x04006679 RID: 26233
		private static int fireCount;

		// Token: 0x0400667A RID: 26234
		private static int lastFireCountUpdateTick;

		// Token: 0x0400667B RID: 26235
		public const float MinFireSize = 0.1f;

		// Token: 0x0400667C RID: 26236
		private const float MinSizeForSpark = 1f;

		// Token: 0x0400667D RID: 26237
		private const float TicksBetweenSparksBase = 150f;

		// Token: 0x0400667E RID: 26238
		private const float TicksBetweenSparksReductionPerFireSize = 40f;

		// Token: 0x0400667F RID: 26239
		private const float MinTicksBetweenSparks = 75f;

		// Token: 0x04006680 RID: 26240
		public const float MaxFireSize = 1.75f;

		// Token: 0x04006681 RID: 26241
		private const int TicksToBurnFloor = 7500;

		// Token: 0x04006682 RID: 26242
		private const int ComplexCalcsInterval = 150;

		// Token: 0x04006683 RID: 26243
		private const float MinSizeForIgniteMovables = 0.4f;

		// Token: 0x04006684 RID: 26244
		private const float FireBaseGrowthPerTick = 0.00055f;

		// Token: 0x04006685 RID: 26245
		private static IntRange SmokeIntervalRange = new IntRange(130, 200);

		// Token: 0x04006686 RID: 26246
		private const int SmokeIntervalRandomAddon = 10;

		// Token: 0x04006687 RID: 26247
		private const float BaseSkyExtinguishChance = 0.04f;

		// Token: 0x04006688 RID: 26248
		private const int BaseSkyExtinguishDamage = 10;

		// Token: 0x04006689 RID: 26249
		private const int VacuumExtinguishDamage = 20;

		// Token: 0x0400668A RID: 26250
		private const float HeatPerFireSizePerInterval = 160f;

		// Token: 0x0400668B RID: 26251
		private const float HeatFactorWhenDoorPresent = 0.15f;

		// Token: 0x0400668C RID: 26252
		private const float SnowClearRadiusPerFireSize = 3f;

		// Token: 0x0400668D RID: 26253
		private const float SnowClearDepthFactor = 0.1f;

		// Token: 0x0400668E RID: 26254
		private const int FireCountParticlesOff = 15;
	}
}
