using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013BF RID: 5055
	public class WildAnimalSpawner
	{
		// Token: 0x17001554 RID: 5460
		// (get) Token: 0x06007D2C RID: 32044 RVA: 0x002CB580 File Offset: 0x002C9780
		private float DesiredAnimalDensity
		{
			get
			{
				float num = this.map.TileInfo.AnimalDensity;
				float num2 = 0f;
				float num3 = 0f;
				foreach (BiomeDef biomeDef in this.map.Biomes)
				{
					foreach (PawnKindDef pawnKindDef in biomeDef.AllWildAnimals)
					{
						float num4 = biomeDef.CommonalityOfAnimal(pawnKindDef);
						if (this.map.TileInfo.IsCoastal)
						{
							num4 += biomeDef.CommonalityOfCoastalAnimal(pawnKindDef);
						}
						num3 += num4;
						if (this.map.mapTemperature.SeasonAcceptableFor(pawnKindDef.race, 0f))
						{
							num2 += num4;
						}
					}
				}
				num *= num2 / num3;
				num *= this.map.gameConditionManager.AggregateAnimalDensityFactor(this.map);
				if (ModsConfig.BiotechActive)
				{
					num *= WildAnimalSpawner.PollutionToAnimalDensityFactorCurve.Evaluate(this.map.TileInfo.pollution);
				}
				return num;
			}
		}

		// Token: 0x17001555 RID: 5461
		// (get) Token: 0x06007D2D RID: 32045 RVA: 0x002CB6C4 File Offset: 0x002C98C4
		private float DesiredTotalAnimalWeight
		{
			get
			{
				float desiredAnimalDensity = this.DesiredAnimalDensity;
				if (desiredAnimalDensity == 0f)
				{
					return 0f;
				}
				float num = 10000f / desiredAnimalDensity;
				return (float)this.map.Area / num;
			}
		}

		// Token: 0x17001556 RID: 5462
		// (get) Token: 0x06007D2E RID: 32046 RVA: 0x002CB6FC File Offset: 0x002C98FC
		private float CurrentTotalAnimalWeight
		{
			get
			{
				float num = 0f;
				IReadOnlyList<Pawn> allPawnsSpawned = this.map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].Faction == null)
					{
						num += allPawnsSpawned[i].kindDef.ecoSystemWeight;
					}
				}
				return num;
			}
		}

		// Token: 0x17001557 RID: 5463
		// (get) Token: 0x06007D2F RID: 32047 RVA: 0x002CB754 File Offset: 0x002C9954
		public bool AnimalEcosystemFull
		{
			get
			{
				return this.CurrentTotalAnimalWeight >= this.DesiredTotalAnimalWeight;
			}
		}

		// Token: 0x06007D30 RID: 32048 RVA: 0x002CB767 File Offset: 0x002C9967
		public WildAnimalSpawner(Map map)
		{
			this.map = map;
		}

		// Token: 0x06007D31 RID: 32049 RVA: 0x002CB778 File Offset: 0x002C9978
		public void WildAnimalSpawnerTick()
		{
			IntVec3 loc;
			if (Find.TickManager.TicksGame % 1213 == 0 && !this.AnimalEcosystemFull && Rand.Chance(0.026955556f * this.DesiredAnimalDensity) && RCellFinder.TryFindRandomPawnEntryCell(out loc, this.map, CellFinder.EdgeRoadChance_Animal, true, (IntVec3 cell) => this.map.reachability.CanReachMapEdge(cell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false).WithFenceblocked(true))))
			{
				this.SpawnRandomWildAnimalAt(loc, true, null);
			}
		}

		// Token: 0x06007D32 RID: 32050 RVA: 0x002CB7E0 File Offset: 0x002C99E0
		public bool SpawnRandomWildAnimalAt(IntVec3 loc, bool canFlyIn, PawnKindDef animalKind = null)
		{
			if (animalKind == null && !(from a in this.map.BiomeAt(loc).AllWildAnimals
			where this.map.mapTemperature.SeasonAcceptableFor(a.race, 0f)
			select a).TryRandomElementByWeight((PawnKindDef kind) => this.CommonalityOfAnimalNow(kind, loc), out animalKind))
			{
				return false;
			}
			bool flag = canFlyIn && animalKind.RaceProps.canFlyIntoMap && this.map != MapGenerator.mapBeingGenerated;
			if (flag)
			{
				bool anyWaterForSeekers = animalKind.RaceProps.waterSeeker && this.map.terrainGrid.AnyWaterCells;
				IntVec3 loc2;
				if (CellFinderLoose.TryGetRandomCellWith((IntVec3 c) => c.Walkable(this.map) && !c.Fogged(this.map) && !c.Roofed(this.map) && c.GetEdifice(this.map) == null && (!anyWaterForSeekers || this.map.terrainGrid.WaterAt(c)) && this.map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.ByPawn, Danger.Some, false, false, false, true, false)), this.map, 1000, out loc2))
				{
					loc = loc2;
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				flag = false;
			}
			if (this.map.TileInfo.IsCoastal && this.map.BiomeAt(loc).ShouldSpawnAnimalOnCoast(animalKind))
			{
				TerrainGrid terrainGrid = this.map.terrainGrid;
				IntVec3 loc3;
				if (RCellFinder.TryFindRandomPawnEntryCell(out loc3, this.map, CellFinder.EdgeRoadChance_Ignore, true, (IntVec3 cell) => terrainGrid.TerrainAt(cell) == TerrainDefOf.WaterOceanShallow && this.map.reachability.CanReachMapEdge(cell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false).WithFenceblocked(true))))
				{
					loc = loc3;
				}
			}
			int randomInRange = animalKind.wildGroupSize.RandomInRange;
			int radius = Mathf.CeilToInt(Mathf.Sqrt((float)animalKind.wildGroupSize.max));
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 intVec = CellFinder.RandomClosewalkCellNear(loc, this.map, radius, null);
				Pawn pawn = PawnGenerator.GeneratePawn(animalKind, null, null);
				if (Rand.Chance(this.map.BiomeAt(loc).wildAnimalScariaChance))
				{
					pawn.health.AddHediff(HediffDefOf.Scaria, null, null, null);
				}
				if (flag)
				{
					GenPlace.TryPlaceThing(SkyfallerMaker.MakeSkyfaller(ThingDefOf.FlyerArrival, pawn), intVec, this.map, ThingPlaceMode.Near, null, null, null, 1);
					pawn.Rotation = Rot4.East;
				}
				else
				{
					GenSpawn.Spawn(pawn, intVec, this.map, WipeMode.Vanish);
				}
			}
			return true;
		}

		// Token: 0x06007D33 RID: 32051 RVA: 0x002CBA3C File Offset: 0x002C9C3C
		private float CommonalityOfAnimalNow(PawnKindDef def, IntVec3 loc)
		{
			if (def.RaceProps.waterSeeker && !this.map.terrainGrid.AnyWaterCells)
			{
				return 0f;
			}
			float num;
			if (ModsConfig.BiotechActive && Rand.Value < WildAnimalSpawner.PollutionAnimalSpawnChanceFromPollutionCurve.Evaluate(Find.WorldGrid[this.map.Tile].pollution))
			{
				num = this.map.BiomeAt(loc).CommonalityOfPollutionAnimal(def);
			}
			else
			{
				num = this.map.BiomeAt(loc).CommonalityOfAnimal(def);
			}
			if (this.map.TileInfo.IsCoastal)
			{
				num += this.map.BiomeAt(loc).CommonalityOfCoastalAnimal(def);
			}
			foreach (TileMutatorDef tileMutatorDef in this.map.TileInfo.Mutators)
			{
				if (tileMutatorDef.Worker != null)
				{
					num *= tileMutatorDef.Worker.AnimalCommonalityFactorFor(def, this.map.Tile);
				}
			}
			return num / def.wildGroupSize.Average;
		}

		// Token: 0x06007D34 RID: 32052 RVA: 0x002CBB64 File Offset: 0x002C9D64
		public string DebugString()
		{
			return string.Concat(new string[]
			{
				"DesiredTotalAnimalWeight: ",
				this.DesiredTotalAnimalWeight.ToString(),
				"\nCurrentTotalAnimalWeight: ",
				this.CurrentTotalAnimalWeight.ToString(),
				"\nDesiredAnimalDensity: ",
				this.DesiredAnimalDensity.ToString()
			});
		}

		// Token: 0x04004ED1 RID: 20177
		private Map map;

		// Token: 0x04004ED2 RID: 20178
		private const int AnimalCheckInterval = 1213;

		// Token: 0x04004ED3 RID: 20179
		private const float BaseAnimalSpawnChancePerInterval = 0.026955556f;

		// Token: 0x04004ED4 RID: 20180
		private static readonly SimpleCurve PollutionToAnimalDensityFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.1f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.25f),
				true
			}
		};

		// Token: 0x04004ED5 RID: 20181
		public static readonly SimpleCurve PollutionAnimalSpawnChanceFromPollutionCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.25f, 0.1f),
				true
			},
			{
				new CurvePoint(0.75f, 0.9f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			}
		};
	}
}
