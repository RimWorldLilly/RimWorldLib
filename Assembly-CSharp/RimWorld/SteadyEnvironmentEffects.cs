using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001FD6 RID: 8150
	public class SteadyEnvironmentEffects
	{
		// Token: 0x0600CEC3 RID: 52931 RVA: 0x004C1CC5 File Offset: 0x004BFEC5
		public SteadyEnvironmentEffects(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600CEC4 RID: 52932 RVA: 0x004C1CD4 File Offset: 0x004BFED4
		public void SteadyEnvironmentEffectsTick()
		{
			if ((float)Find.TickManager.TicksGame % 97f == 0f && Rand.Chance(0.02f))
			{
				this.RollForRainFire();
			}
			this.outdoorMeltAmount = this.MeltAmountAt(this.map.mapTemperature.OutdoorTemp);
			this.snowRate = this.map.weatherManager.SnowRate;
			this.rainRate = this.map.weatherManager.RainRate;
			this.sandRate = this.map.weatherManager.SandRate;
			int num = Mathf.CeilToInt((float)this.map.Area * 0.0006f);
			int area = this.map.Area;
			for (int i = 0; i < num; i++)
			{
				if (this.cycleIndex >= area)
				{
					this.cycleIndex = 0;
				}
				IntVec3 c = this.map.cellsInRandomOrder.Get(this.cycleIndex);
				this.DoCellSteadyEffects(c);
				this.cycleIndex++;
			}
		}

		// Token: 0x0600CEC5 RID: 52933 RVA: 0x004C1DD8 File Offset: 0x004BFFD8
		private void DoCellSteadyEffects(IntVec3 c)
		{
			Room room = c.GetRoom(this.map);
			bool flag = this.map.roofGrid.Roofed(c);
			TerrainDef terrain = c.GetTerrain(this.map);
			bool flag2 = room != null && room.UsesOutdoorTemperature;
			if (room == null || flag2)
			{
				if (this.outdoorMeltAmount > 0f)
				{
					this.map.snowGrid.AddDepth(c, -this.outdoorMeltAmount);
				}
				if (!flag)
				{
					if (this.snowRate > 0.001f)
					{
						this.AddFallenSnowAt(c, 0.046f * this.map.weatherManager.SnowRate);
					}
					if (ModsConfig.OdysseyActive)
					{
						if (this.sandRate > 0.001f)
						{
							this.AddFallenSandAt(c, 0.046f * this.map.weatherManager.SandRate);
						}
						else
						{
							this.map.sandGrid.AddDepth(c, -0.0055555557f);
						}
					}
				}
				if (this.map.terrainGrid.TerrainAt(c).meltSnowRadius > 0f)
				{
					WeatherBuildupUtility.AddSnowRadial(c, this.map, this.map.terrainGrid.TerrainAt(c).meltSnowRadius, -0.06f);
				}
			}
			if (room != null)
			{
				List<Thing> thingList = c.GetThingList(this.map);
				for (int i = thingList.Count - 1; i >= 0; i--)
				{
					Thing thing = thingList[i];
					Filth filth = thing as Filth;
					if (filth != null)
					{
						if (!flag && thing.def.filth.rainWashes && Rand.Chance(this.rainRate))
						{
							filth.ThinFilth();
						}
						if (filth.DisappearAfterTicks != 0 && filth.TicksSinceThickened > filth.DisappearAfterTicks && !filth.Destroyed)
						{
							filth.Destroy(DestroyMode.Vanish);
						}
					}
					else
					{
						this.TryDoDeteriorate(thing, flag, flag2, terrain);
					}
				}
				if (!flag2)
				{
					float temperature = room.Temperature;
					if (temperature > 0f)
					{
						float num = this.MeltAmountAt(temperature);
						if (num > 0f)
						{
							this.map.snowGrid.AddDepth(c, -num);
						}
						if (temperature > SteadyEnvironmentEffects.AutoIgnitionTemperatureRange.min)
						{
							float value = Rand.Value;
							if (value < SteadyEnvironmentEffects.AutoIgnitionTemperatureRange.InverseLerpThroughRange(temperature) * 0.7f && Rand.Chance(FireUtility.ChanceToStartFireIn(c, this.map, null)))
							{
								FireUtility.TryStartFireIn(c, this.map, 0.1f, null, null);
							}
							if (value < 0.33f)
							{
								FleckMaker.ThrowHeatGlow(c, this.map, 2.3f);
							}
						}
					}
					if (terrain.heatPerTick > 0f)
					{
						GenTemperature.PushHeat(c, this.map, terrain.heatPerTick * 1666.6666f);
					}
					if (ModsConfig.OdysseyActive)
					{
						this.map.sandGrid.AddDepth(c, -0.0055555557f);
					}
				}
			}
			if (Rand.Chance(terrain.throwFleckChance))
			{
				FleckCreationData dataStatic = FleckMaker.GetDataStatic(c.ToVector3Shifted(), this.map, terrain.fleckData.fleck, terrain.fleckData.scaleRange.RandomInRange);
				dataStatic.velocitySpeed = terrain.fleckData.velocitySpeedRange.RandomInRange;
				dataStatic.velocityAngle = terrain.fleckData.velocityAngleRange.RandomInRange;
				dataStatic.rotationRate = terrain.fleckData.rotationSpeedRange.RandomInRange;
				if (!terrain.fleckData.solidTicksRange.IsZeros)
				{
					dataStatic.solidTimeOverride = new float?(terrain.fleckData.solidTicksRange.RandomInRange);
				}
				this.map.flecks.CreateFleck(dataStatic);
			}
			if (terrain.igniteRadius > 0f)
			{
				foreach (IntVec3 c2 in GenRadial.RadialCellsAround(c, terrain.igniteRadius, true))
				{
					if (c2.InBounds(this.map) && Rand.Chance(0.03f) && Rand.Chance(FireUtility.ChanceToStartFireIn(c2, this.map, null)))
					{
						FireUtility.TryStartFireIn(c2, this.map, 0.1f, null, null);
					}
				}
			}
			this.map.gameConditionManager.DoSteadyEffects(c, this.map);
			GasUtility.DoSteadyEffects(c, this.map);
		}

		// Token: 0x0600CEC6 RID: 52934 RVA: 0x004C2220 File Offset: 0x004C0420
		public static bool ProtectedByEdifice(IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.building != null && edifice.def.building.preventDeteriorationOnTop;
		}

		// Token: 0x0600CEC7 RID: 52935 RVA: 0x004C225A File Offset: 0x004C045A
		private float MeltAmountAt(float temperature)
		{
			if (temperature < 0f)
			{
				return 0f;
			}
			if (temperature < 10f)
			{
				return temperature * temperature * 0.0058f * 0.1f;
			}
			return temperature * 0.0058f;
		}

		// Token: 0x0600CEC8 RID: 52936 RVA: 0x004C228C File Offset: 0x004C048C
		public void AddFallenSnowAt(IntVec3 c, float baseAmount)
		{
			if (this.snowNoise == null)
			{
				this.snowNoise = new Perlin(0.03999999910593033, 2.0, 0.5, 5, Rand.Range(0, 651431), QualityMode.Medium);
			}
			float num = this.snowNoise.GetValue(c);
			num += 1f;
			num *= 0.5f;
			if (num < 0.5f)
			{
				num = 0.5f;
			}
			float depthToAdd = baseAmount * num;
			this.map.snowGrid.AddDepth(c, depthToAdd);
		}

		// Token: 0x0600CEC9 RID: 52937 RVA: 0x004C2318 File Offset: 0x004C0518
		public void AddFallenSandAt(IntVec3 c, float baseAmount)
		{
			if (this.snowNoise == null)
			{
				this.snowNoise = new Perlin(0.03999999910593033, 2.0, 0.5, 5, Rand.Range(0, 651431), QualityMode.Medium);
			}
			float num = this.snowNoise.GetValue(c);
			num += 1f;
			num *= 0.5f;
			if (num < 0.5f)
			{
				num = 0.5f;
			}
			float depthToAdd = baseAmount * num;
			this.map.sandGrid.AddDepth(c, depthToAdd);
		}

		// Token: 0x0600CECA RID: 52938 RVA: 0x004C23A4 File Offset: 0x004C05A4
		public static float FinalDeteriorationRate(Thing t, List<string> reasons = null)
		{
			if (t.Spawned)
			{
				Room room = t.GetRoom(RegionType.Set_All);
				return SteadyEnvironmentEffects.FinalDeteriorationRate(t, t.Position.Roofed(t.Map), room != null && room.UsesOutdoorTemperature, t.Position.GetTerrain(t.Map), reasons);
			}
			if (t.SpawnedOrAnyParentSpawned && !t.def.canDeteriorateUnspawned)
			{
				return 0f;
			}
			return SteadyEnvironmentEffects.FinalDeteriorationRate(t, false, false, null, reasons);
		}

		// Token: 0x0600CECB RID: 52939 RVA: 0x004C2420 File Offset: 0x004C0620
		public static float FinalDeteriorationRate(Thing t, bool roofed, bool roomUsesOutdoorTemperature, TerrainDef terrain, List<string> reasons = null)
		{
			if (!t.def.CanEverDeteriorate)
			{
				return 0f;
			}
			Map mapHeld = t.MapHeld;
			float num = t.GetStatValue(StatDefOf.DeteriorationRate, true, -1);
			Genepack genepack = t as Genepack;
			if (ModsConfig.BiotechActive && genepack != null && !genepack.Deteriorating)
			{
				num = 0f;
			}
			if (num <= 0f)
			{
				return 0f;
			}
			if (reasons != null)
			{
				if (t.def.deteriorateFromEnvironmentalEffects)
				{
					if (!roofed)
					{
						reasons.Add("DeterioratingUnroofed".Translate());
						if (mapHeld.weatherManager.RainRate > 0f)
						{
							reasons.Add("DeterioratingRaining".Translate());
						}
					}
					if (roomUsesOutdoorTemperature)
					{
						reasons.Add("DeterioratingOutdoors".Translate());
					}
					if (terrain != null && terrain.extraDeteriorationFactor != 0f)
					{
						reasons.Add(terrain.label);
					}
				}
				if (ModsConfig.BiotechActive && genepack != null && genepack.Deteriorating)
				{
					reasons.Add("NotInGeneBank".Translate());
				}
			}
			return num;
		}

		// Token: 0x0600CECC RID: 52940 RVA: 0x004C2538 File Offset: 0x004C0738
		private void TryDoDeteriorate(Thing t, bool roofed, bool roomUsesOutdoorTemperature, TerrainDef terrain)
		{
			Corpse corpse = t as Corpse;
			if (corpse != null && corpse.InnerPawn.apparel != null)
			{
				List<Apparel> wornApparel = corpse.InnerPawn.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					this.TryDoDeteriorate(wornApparel[i], roofed, roomUsesOutdoorTemperature, terrain);
				}
			}
			float num = SteadyEnvironmentEffects.FinalDeteriorationRate(t, roofed, roomUsesOutdoorTemperature, terrain, null);
			if (num < 0.001f)
			{
				return;
			}
			if (Rand.Chance(num / 36f))
			{
				IntVec3 position = t.Position;
				Map map = t.Map;
				bool sendMessage = t.IsInAnyStorage();
				SteadyEnvironmentEffects.DoDeteriorationDamage(t, position, map, sendMessage);
			}
		}

		// Token: 0x0600CECD RID: 52941 RVA: 0x004C25D8 File Offset: 0x004C07D8
		public static void DoDeteriorationDamage(Thing t, IntVec3 pos, Map map, bool sendMessage)
		{
			t.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 1f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			if (sendMessage && t.Destroyed && t.def.messageOnDeteriorateInStorage && (SteadyEnvironmentEffects.lastDeterioratedTick == 0 || GenTicks.TicksGame >= SteadyEnvironmentEffects.lastDeterioratedTick + 60000))
			{
				SteadyEnvironmentEffects.lastDeterioratedTick = GenTicks.TicksGame;
				Messages.Message("MessageDeterioratedAway".Translate(t.Label), new TargetInfo(pos, map, false), MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x0600CECE RID: 52942 RVA: 0x004C267C File Offset: 0x004C087C
		public static void Reset()
		{
			SteadyEnvironmentEffects.lastDeterioratedTick = 0;
		}

		// Token: 0x0600CECF RID: 52943 RVA: 0x004C2684 File Offset: 0x004C0884
		private void RollForRainFire()
		{
			if (!Rand.Chance(0.2f * (float)this.map.listerBuildings.allBuildingsColonistElecFire.Count * this.map.weatherManager.RainRate))
			{
				return;
			}
			Building building = this.map.listerBuildings.allBuildingsColonistElecFire.RandomElement<Building>();
			if (!this.map.roofGrid.Roofed(building.Position))
			{
				ShortCircuitUtility.TryShortCircuitInRain(building);
			}
		}

		// Token: 0x04008364 RID: 33636
		private Map map;

		// Token: 0x04008365 RID: 33637
		private ModuleBase snowNoise;

		// Token: 0x04008366 RID: 33638
		private int cycleIndex;

		// Token: 0x04008367 RID: 33639
		private float outdoorMeltAmount;

		// Token: 0x04008368 RID: 33640
		private float snowRate;

		// Token: 0x04008369 RID: 33641
		private float rainRate;

		// Token: 0x0400836A RID: 33642
		private float sandRate;

		// Token: 0x0400836B RID: 33643
		private const float MapFractionCheckPerTick = 0.0006f;

		// Token: 0x0400836C RID: 33644
		private const float RainFireCheckInterval = 97f;

		// Token: 0x0400836D RID: 33645
		private const float RainFireChanceOverall = 0.02f;

		// Token: 0x0400836E RID: 33646
		private const float RainFireChancePerBuilding = 0.2f;

		// Token: 0x0400836F RID: 33647
		private const float SnowFallRateFactor = 0.046f;

		// Token: 0x04008370 RID: 33648
		private const float SandFallRateFactor = 0.046f;

		// Token: 0x04008371 RID: 33649
		private const float SnowMeltRateFactor = 0.0058f;

		// Token: 0x04008372 RID: 33650
		private const float SandDissipateRate = 0.0055555557f;

		// Token: 0x04008373 RID: 33651
		private static readonly FloatRange AutoIgnitionTemperatureRange = new FloatRange(240f, 1000f);

		// Token: 0x04008374 RID: 33652
		private const float AutoIgnitionChanceFactor = 0.7f;

		// Token: 0x04008375 RID: 33653
		private const float FireGlowRate = 0.33f;

		// Token: 0x04008376 RID: 33654
		private const float IgniteChancePerCheck = 0.03f;

		// Token: 0x04008377 RID: 33655
		private static int lastDeterioratedTick;

		// Token: 0x04008378 RID: 33656
		private const int MessageCooldownTicks = 60000;
	}
}
