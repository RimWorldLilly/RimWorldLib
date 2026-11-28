using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013C0 RID: 5056
	public class WildPlantSpawner : IExposable
	{
		// Token: 0x17001558 RID: 5464
		// (get) Token: 0x06007D37 RID: 32055 RVA: 0x002CBCA3 File Offset: 0x002C9EA3
		public float CurrentPlantDensityFactor
		{
			get
			{
				return this.map.TileInfo.PlantDensityFactor * this.map.gameConditionManager.AggregatePlantDensityFactor(this.map);
			}
		}

		// Token: 0x17001559 RID: 5465
		// (get) Token: 0x06007D38 RID: 32056 RVA: 0x002CBCCC File Offset: 0x002C9ECC
		public float CurrentWholeMapNumDesiredPlants
		{
			get
			{
				CellRect cellRect = CellRect.WholeMap(this.map);
				float currentPlantDensityFactor = this.CurrentPlantDensityFactor;
				float num = 0f;
				foreach (IntVec3 forCell in cellRect)
				{
					num += this.GetDesiredPlantsCountAt(forCell, currentPlantDensityFactor);
				}
				return num;
			}
		}

		// Token: 0x1700155A RID: 5466
		// (get) Token: 0x06007D39 RID: 32057 RVA: 0x002CBD3C File Offset: 0x002C9F3C
		public int CurrentWholeMapNumNonZeroFertilityCells
		{
			get
			{
				CellRect cellRect = CellRect.WholeMap(this.map);
				int num = 0;
				using (CellRect.Enumerator enumerator = cellRect.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.GetFertility(this.map) > 0f)
						{
							num++;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x1700155B RID: 5467
		// (get) Token: 0x06007D3A RID: 32058 RVA: 0x002CBDA8 File Offset: 0x002C9FA8
		public float CachedChanceFromDensity
		{
			get
			{
				this.CacheWholeMapNumDesiredPlants();
				return this.calculatedWholeMapNumDesiredPlants / (float)this.calculatedWholeMapNumNonZeroFertilityCells;
			}
		}

		// Token: 0x1700155C RID: 5468
		// (get) Token: 0x06007D3B RID: 32059 RVA: 0x002CBDC0 File Offset: 0x002C9FC0
		public SimpleCurve OverrideDensityForFertilityCurve
		{
			get
			{
				if (this.cachedOverrideDensityForFertilityCurve != null)
				{
					return this.cachedOverrideDensityForFertilityCurve;
				}
				this.cachedOverrideDensityForFertilityCurve = new SimpleCurve
				{
					{
						new CurvePoint(0f, -1f),
						true
					}
				};
				foreach (TileMutatorDef tileMutatorDef in this.map.TileInfo.Mutators)
				{
					if (tileMutatorDef.overrideDensityForFertilityCurve != null)
					{
						this.cachedOverrideDensityForFertilityCurve = tileMutatorDef.overrideDensityForFertilityCurve;
					}
				}
				return this.cachedOverrideDensityForFertilityCurve;
			}
		}

		// Token: 0x1700155D RID: 5469
		// (get) Token: 0x06007D3C RID: 32060 RVA: 0x002CBE5C File Offset: 0x002CA05C
		public List<ThingDef> AllWildPlants
		{
			get
			{
				if (this.cachedWildPlants != null)
				{
					return this.cachedWildPlants;
				}
				this.cachedWildPlants = new List<ThingDef>();
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
				{
					if (thingDef.category == ThingCategory.Plant && this.GetCommonalityOfPlant(thingDef) > 0f)
					{
						this.cachedWildPlants.Add(thingDef);
					}
				}
				return this.cachedWildPlants;
			}
		}

		// Token: 0x1700155E RID: 5470
		// (get) Token: 0x06007D3D RID: 32061 RVA: 0x002CBEEC File Offset: 0x002CA0EC
		public List<ThingDef> MutatorWildPlants
		{
			get
			{
				if (this.cachedMutatorWildPlants != null)
				{
					return this.cachedMutatorWildPlants;
				}
				this.cachedMutatorWildPlants = new List<ThingDef>();
				foreach (TileMutatorDef tileMutatorDef in this.map.TileInfo.Mutators)
				{
					TileMutatorWorker worker = tileMutatorDef.Worker;
					foreach (BiomePlantRecord biomePlantRecord in (((worker != null) ? worker.AdditionalWildPlants(this.map.Tile) : null) ?? Enumerable.Empty<BiomePlantRecord>()))
					{
						if (!this.cachedMutatorWildPlants.Contains(biomePlantRecord.plant))
						{
							this.cachedMutatorWildPlants.Add(biomePlantRecord.plant);
						}
					}
				}
				return this.cachedMutatorWildPlants;
			}
		}

		// Token: 0x1700155F RID: 5471
		// (get) Token: 0x06007D3E RID: 32062 RVA: 0x002CBFD4 File Offset: 0x002CA1D4
		private bool HaveAnyPlantsWhichIgnoreFertility
		{
			get
			{
				if (this.cachedHaveAnyPlantsWhichIgnoreFertility == null)
				{
					this.cachedHaveAnyPlantsWhichIgnoreFertility = new bool?(false);
					bool? flag = this.cachedHaveAnyPlantsWhichIgnoreFertility;
					this.cachedHaveAnyPlantsWhichIgnoreFertility = (flag | this.AllWildPlants.Any((ThingDef p) => p.plant.completelyIgnoreFertility));
				}
				return this.cachedHaveAnyPlantsWhichIgnoreFertility.Value;
			}
		}

		// Token: 0x17001560 RID: 5472
		// (get) Token: 0x06007D3F RID: 32063 RVA: 0x002CC048 File Offset: 0x002CA248
		public float PlantsCommonalitiesSum
		{
			get
			{
				if (this.cachedPlantsCommonalitiesSum == null)
				{
					this.cachedPlantsCommonalitiesSum = new float?(0f);
					for (int i = 0; i < this.AllWildPlants.Count; i++)
					{
						this.cachedPlantsCommonalitiesSum += this.GetCommonalityOfPlant(this.AllWildPlants[i]);
					}
				}
				return this.cachedPlantsCommonalitiesSum.Value;
			}
		}

		// Token: 0x17001561 RID: 5473
		// (get) Token: 0x06007D40 RID: 32064 RVA: 0x002CC0D8 File Offset: 0x002CA2D8
		public float CavePlantsCommonalitiesSum
		{
			get
			{
				if (this.cachedCavePlantsCommonalitiesSum == null)
				{
					this.cachedCavePlantsCommonalitiesSum = new float?(0f);
					for (int i = 0; i < WildPlantSpawner.allCavePlants.Count; i++)
					{
						this.cachedCavePlantsCommonalitiesSum += this.GetCommonalityOfPlant(WildPlantSpawner.allCavePlants[i]);
					}
				}
				return this.cachedCavePlantsCommonalitiesSum.Value;
			}
		}

		// Token: 0x06007D41 RID: 32065 RVA: 0x002CC163 File Offset: 0x002CA363
		public WildPlantSpawner(Map map)
		{
			this.map = map;
		}

		// Token: 0x06007D42 RID: 32066 RVA: 0x002CC172 File Offset: 0x002CA372
		public static void ResetStaticData()
		{
			WildPlantSpawner.allCavePlants.Clear();
			WildPlantSpawner.allCavePlants.AddRange(from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.category == ThingCategory.Plant && x.plant.cavePlant && x.plant.cavePlantWeight > 0f
			select x);
		}

		// Token: 0x06007D43 RID: 32067 RVA: 0x002CC1B4 File Offset: 0x002CA3B4
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.cycleIndex, "cycleIndex", 0, false);
			Scribe_Values.Look<float>(ref this.calculatedWholeMapNumDesiredPlants, "calculatedWholeMapNumDesiredPlants", 0f, false);
			Scribe_Values.Look<float>(ref this.calculatedWholeMapNumDesiredPlantsTmp, "calculatedWholeMapNumDesiredPlantsTmp", 0f, false);
			Scribe_Values.Look<bool>(ref this.hasWholeMapNumDesiredPlantsCalculated, "hasWholeMapNumDesiredPlantsCalculated", true, false);
			Scribe_Values.Look<int>(ref this.calculatedWholeMapNumNonZeroFertilityCells, "calculatedWholeMapNumNonZeroFertilityCells", 0, false);
			Scribe_Values.Look<int>(ref this.calculatedWholeMapNumNonZeroFertilityCellsTmp, "calculatedWholeMapNumNonZeroFertilityCellsTmp", 0, false);
		}

		// Token: 0x06007D44 RID: 32068 RVA: 0x002CC238 File Offset: 0x002CA438
		public void WildPlantSpawnerTick()
		{
			if (DebugSettings.fastEcology || DebugSettings.fastEcologyRegrowRateOnly)
			{
				for (int i = 0; i < 2000; i++)
				{
					this.WildPlantSpawnerTickInternal();
				}
				return;
			}
			this.WildPlantSpawnerTickInternal();
		}

		// Token: 0x06007D45 RID: 32069 RVA: 0x002CC270 File Offset: 0x002CA470
		private void WildPlantSpawnerTickInternal()
		{
			int area = this.map.Area;
			int num = Mathf.CeilToInt((float)area * 0.0001f);
			float currentPlantDensityFactor = this.CurrentPlantDensityFactor;
			this.CacheWholeMapNumDesiredPlants();
			int num2 = Mathf.CeilToInt(10000f);
			float cachedChanceFromDensity = this.CachedChanceFromDensity;
			for (int i = 0; i < num; i++)
			{
				if (this.cycleIndex >= area)
				{
					this.calculatedWholeMapNumDesiredPlants = this.calculatedWholeMapNumDesiredPlantsTmp;
					this.calculatedWholeMapNumDesiredPlantsTmp = 0f;
					this.calculatedWholeMapNumNonZeroFertilityCells = this.calculatedWholeMapNumNonZeroFertilityCellsTmp;
					this.calculatedWholeMapNumNonZeroFertilityCellsTmp = 0;
					this.cycleIndex = 0;
				}
				IntVec3 intVec = this.map.cellsInRandomOrder.Get(this.cycleIndex);
				this.calculatedWholeMapNumDesiredPlantsTmp += this.GetDesiredPlantsCountAt(intVec, currentPlantDensityFactor);
				if (this.map.fertilityGrid.FertilityAt(intVec) > 0f)
				{
					this.calculatedWholeMapNumNonZeroFertilityCellsTmp++;
				}
				float mtb = this.GoodRoofForCavePlant(intVec) ? 130f : this.map.BiomeAt(intVec).wildPlantRegrowDays;
				float num3 = this.OverrideDensityForFertilityCurve.Evaluate(this.map.fertilityGrid.FertilityAt(intVec));
				if (Rand.Chance((num3 > 0f) ? num3 : cachedChanceFromDensity) && Rand.MTBEventOccurs(mtb, 60000f, (float)num2) && this.CanRegrowAt(intVec))
				{
					this.CheckSpawnWildPlantAt(intVec, currentPlantDensityFactor, this.calculatedWholeMapNumDesiredPlants, false);
				}
				this.cycleIndex++;
			}
		}

		// Token: 0x06007D46 RID: 32070 RVA: 0x002CC3F0 File Offset: 0x002CA5F0
		private void CachePlantCommonalitiesIfShould()
		{
			if (this.cachedPlantCommonalities != null)
			{
				return;
			}
			this.cachedPlantCommonalities = new Dictionary<ThingDef, float>();
			foreach (BiomeDef biomeDef in this.map.Biomes)
			{
				foreach (BiomePlantRecord biomePlantRecord in biomeDef.wildPlants)
				{
					if (biomePlantRecord.plant != null)
					{
						if (this.cachedPlantCommonalities.ContainsKey(biomePlantRecord.plant))
						{
							this.cachedPlantCommonalities[biomePlantRecord.plant] = (this.cachedPlantCommonalities[biomePlantRecord.plant] + biomePlantRecord.commonality) / 2f;
						}
						else
						{
							this.cachedPlantCommonalities.Add(biomePlantRecord.plant, biomePlantRecord.commonality);
						}
					}
				}
			}
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.plant != null && thingDef.plant.wildBiomes != null)
				{
					for (int i = 0; i < thingDef.plant.wildBiomes.Count; i++)
					{
						if (this.map.Biomes.Contains(thingDef.plant.wildBiomes[i].biome))
						{
							if (this.cachedPlantCommonalities.ContainsKey(thingDef))
							{
								this.cachedPlantCommonalities[thingDef] = (this.cachedPlantCommonalities[thingDef] + thingDef.plant.wildBiomes[i].commonality) / 2f;
							}
							else
							{
								this.cachedPlantCommonalities.Add(thingDef, thingDef.plant.wildBiomes[i].commonality);
							}
						}
					}
				}
			}
			foreach (TileMutatorDef tileMutatorDef in this.map.TileInfo.Mutators)
			{
				TileMutatorWorker worker = tileMutatorDef.Worker;
				foreach (BiomePlantRecord biomePlantRecord2 in (((worker != null) ? worker.AdditionalWildPlants(this.map.Tile) : null) ?? Enumerable.Empty<BiomePlantRecord>()))
				{
					if (biomePlantRecord2.plant != null)
					{
						if (this.cachedPlantCommonalities.ContainsKey(biomePlantRecord2.plant))
						{
							this.cachedPlantCommonalities[biomePlantRecord2.plant] = Mathf.Max(this.cachedPlantCommonalities[biomePlantRecord2.plant], biomePlantRecord2.commonality);
						}
						else
						{
							this.cachedPlantCommonalities.Add(biomePlantRecord2.plant, biomePlantRecord2.commonality);
						}
					}
				}
			}
			foreach (ThingDef thingDef2 in this.cachedPlantCommonalities.Keys.ToList<ThingDef>())
			{
				float num = this.cachedPlantCommonalities[thingDef2];
				int num2 = this.map.Biomes.Count<BiomeDef>();
				foreach (BiomeDef biomeDef2 in this.map.Biomes)
				{
					if (thingDef2.plant.cavePlant && !biomeDef2.wildPlantsAreCavePlants)
					{
						num += thingDef2.plant.cavePlantWeight / (float)num2;
					}
					else
					{
						num += this.cachedPlantCommonalities.GetWithFallback(thingDef2, 0f) / (float)num2;
					}
					foreach (TileMutatorDef tileMutatorDef2 in this.map.TileInfo.Mutators)
					{
						if (tileMutatorDef2.Worker != null)
						{
							num *= tileMutatorDef2.Worker.PlantCommonalityFactorFor(thingDef2, this.map.Tile);
						}
					}
				}
				this.cachedPlantCommonalities[thingDef2] = num;
			}
		}

		// Token: 0x06007D47 RID: 32071 RVA: 0x002CC8E8 File Offset: 0x002CAAE8
		private void CacheWholeMapNumDesiredPlants()
		{
			if (!this.hasWholeMapNumDesiredPlantsCalculated)
			{
				this.calculatedWholeMapNumDesiredPlants = this.CurrentWholeMapNumDesiredPlants;
				this.calculatedWholeMapNumNonZeroFertilityCells = this.CurrentWholeMapNumNonZeroFertilityCells;
				this.hasWholeMapNumDesiredPlantsCalculated = true;
			}
		}

		// Token: 0x06007D48 RID: 32072 RVA: 0x002CC914 File Offset: 0x002CAB14
		public bool CheckSpawnWildPlantAt(IntVec3 c, float plantDensityFactor, float wholeMapNumDesiredPlants, bool setRandomGrowth = false)
		{
			if (plantDensityFactor <= 0f || c.GetPlant(this.map) != null || c.GetCover(this.map) != null || c.GetEdifice(this.map) != null || (!this.HaveAnyPlantsWhichIgnoreFertility && this.map.fertilityGrid.FertilityAt(c) <= 0f) || !PlantUtility.SnowAllowsPlanting(c, this.map) || !PlantUtility.SandAllowsPlanting(c, this.map))
			{
				return false;
			}
			bool cavePlants = this.GoodRoofForCavePlant(c);
			float num = this.OverrideDensityForFertilityCurve.Evaluate(this.map.fertilityGrid.FertilityAt(c));
			if (num > 0f)
			{
				if (!Rand.Chance(num))
				{
					return false;
				}
			}
			else if (this.SaturatedAt(c, plantDensityFactor, cavePlants, wholeMapNumDesiredPlants))
			{
				return false;
			}
			this.CalculatePlantsWhichCanGrowAt(c, WildPlantSpawner.tmpPossiblePlants, cavePlants, plantDensityFactor);
			if (!WildPlantSpawner.tmpPossiblePlants.Any<ThingDef>())
			{
				return false;
			}
			this.CalculateDistancesToNearbyClusters(c);
			WildPlantSpawner.tmpPossiblePlantsWithWeight.Clear();
			foreach (ThingDef thingDef in WildPlantSpawner.tmpPossiblePlants)
			{
				float value = this.PlantChoiceWeight(thingDef, c, WildPlantSpawner.distanceSqToNearbyClusters, wholeMapNumDesiredPlants, plantDensityFactor);
				WildPlantSpawner.tmpPossiblePlantsWithWeight.Add(new KeyValuePair<ThingDef, float>(thingDef, value));
			}
			KeyValuePair<ThingDef, float> keyValuePair;
			if (!WildPlantSpawner.tmpPossiblePlantsWithWeight.TryRandomElementByWeight((KeyValuePair<ThingDef, float> x) => x.Value, out keyValuePair))
			{
				return false;
			}
			if (keyValuePair.Key.plant.wildPlantUseDistanceToShore && !Rand.Chance(this.GetWaterPlantDistanceToShoreWeight(c)))
			{
				return false;
			}
			WildPlantSpawner.SpawnPlant(keyValuePair.Key, this.map, c, setRandomGrowth);
			return true;
		}

		// Token: 0x06007D49 RID: 32073 RVA: 0x002CCAD0 File Offset: 0x002CACD0
		public static Plant SpawnPlant(ThingDef plant, Map map, IntVec3 cell, bool setRandomGrowth)
		{
			Plant plant2 = (Plant)ThingMaker.MakeThing(plant, null);
			if (setRandomGrowth)
			{
				plant2.Growth = Mathf.Clamp01(WildPlantSpawner.InitialGrowthRandomRange.RandomInRange);
				if (plant2.def.plant.LimitedLifespan)
				{
					plant2.Age = Rand.Range(0, Mathf.Max(plant2.def.plant.LifespanTicks - 50, 0));
				}
			}
			GenSpawn.Spawn(plant2, cell, map, WipeMode.Vanish);
			return plant2;
		}

		// Token: 0x06007D4A RID: 32074 RVA: 0x002CCB48 File Offset: 0x002CAD48
		private float PlantChoiceWeight(ThingDef plantDef, IntVec3 c, Dictionary<ThingDef, float> distanceSqToNearbyClusters, float wholeMapNumDesiredPlants, float plantDensityFactor)
		{
			float commonalityOfPlant = this.GetCommonalityOfPlant(plantDef);
			float num = this.GetCommonalityPctOfPlant(plantDef);
			if (Current.ProgramState == ProgramState.Playing)
			{
				num *= plantDef.plant.plantRespawningCommonalityFactor;
			}
			float num2 = commonalityOfPlant;
			if (num2 <= 0f)
			{
				return num2;
			}
			float num3 = (float)this.map.listerThings.ThingsOfDef(plantDef).Count / (float)this.map.listerThings.ThingsInGroup(ThingRequestGroup.NonStumpPlant).Count / num;
			num2 *= WildPlantSpawner.GlobalPctSelectionWeightBias.Evaluate(num3);
			if (plantDef.plant.GrowsInClusters && num3 < 1.1f)
			{
				float num4 = (plantDef.plant.cavePlant && !this.map.BiomeAt(c).wildPlantsAreCavePlants) ? this.CavePlantsCommonalitiesSum : this.PlantsCommonalitiesSum;
				float x = commonalityOfPlant * plantDef.plant.wildClusterWeight / (num4 - commonalityOfPlant + commonalityOfPlant * plantDef.plant.wildClusterWeight);
				float num5 = 1f / (3.1415927f * (float)plantDef.plant.wildClusterRadius * (float)plantDef.plant.wildClusterRadius);
				num5 = GenMath.LerpDoubleClamped(num, 1f, 1f, num5, x);
				float f;
				if (distanceSqToNearbyClusters.TryGetValue(plantDef, out f))
				{
					float x2 = Mathf.Sqrt(f);
					num2 *= GenMath.LerpDoubleClamped((float)plantDef.plant.wildClusterRadius * 0.9f, (float)plantDef.plant.wildClusterRadius * 1.1f, plantDef.plant.wildClusterWeight, num5, x2);
				}
				else
				{
					num2 *= num5;
				}
			}
			if (plantDef.plant.wildEqualLocalDistribution)
			{
				float f2 = wholeMapNumDesiredPlants * num;
				float num6 = (float)Mathf.Max(this.map.Size.x, this.map.Size.z) / Mathf.Sqrt(f2) * 2f;
				if (plantDef.plant.GrowsInClusters)
				{
					num6 = Mathf.Max(num6, (float)plantDef.plant.wildClusterRadius * 1.6f);
				}
				num6 = Mathf.Max(num6, 7f);
				if (num6 <= 25f)
				{
					num2 *= this.LocalPlantProportionsWeightFactor(c, num, plantDensityFactor, num6, plantDef);
				}
			}
			return num2;
		}

		// Token: 0x06007D4B RID: 32075 RVA: 0x002CCD6C File Offset: 0x002CAF6C
		private float LocalPlantProportionsWeightFactor(IntVec3 c, float commonalityPct, float plantDensityFactor, float radiusToScan, ThingDef plantDef)
		{
			float numDesiredPlantsLocally = 0f;
			int numPlants = 0;
			int numPlantsThisDef = 0;
			RegionTraverser.BreadthFirstTraverse(c, this.map, (Region from, Region to) => c.InHorDistOf(to.extentsClose.ClosestCellTo(c), radiusToScan), delegate(Region reg)
			{
				numDesiredPlantsLocally += this.GetDesiredPlantsCountIn(reg, c, plantDensityFactor);
				numPlants += reg.ListerThings.ThingsInGroup(ThingRequestGroup.NonStumpPlant).Count;
				numPlantsThisDef += reg.ListerThings.ThingsOfDef(plantDef).Count;
				return false;
			}, 999999, RegionType.Set_Passable);
			if (numDesiredPlantsLocally * commonalityPct < 2f)
			{
				return 1f;
			}
			if ((float)numPlants <= numDesiredPlantsLocally * 0.5f)
			{
				return 1f;
			}
			float t = (float)numPlantsThisDef / (float)numPlants / commonalityPct;
			return Mathf.Lerp(7f, 1f, t);
		}

		// Token: 0x06007D4C RID: 32076 RVA: 0x002CCE40 File Offset: 0x002CB040
		private void CalculatePlantsWhichCanGrowAt(IntVec3 c, List<ThingDef> outPlants, bool cavePlants, float plantDensityFactor)
		{
			outPlants.Clear();
			if (cavePlants)
			{
				for (int i = 0; i < WildPlantSpawner.allCavePlants.Count; i++)
				{
					if (WildPlantSpawner.allCavePlants[i].CanEverPlantAt(c, this.map, false, true))
					{
						outPlants.Add(WildPlantSpawner.allCavePlants[i]);
					}
				}
				return;
			}
			WildPlantSpawner.tmpWildPlants.Clear();
			WildPlantSpawner.tmpWildPlants.AddRange(this.map.BiomeAt(c).AllWildPlants);
			WildPlantSpawner.tmpWildPlants.AddRange(this.MutatorWildPlants);
			foreach (ThingDef thingDef in WildPlantSpawner.tmpWildPlants)
			{
				if (!thingDef.IsDeadPlant && thingDef.CanEverPlantAt(c, this.map, false, true))
				{
					if (!Mathf.Approximately(thingDef.plant.wildOrder, this.map.BiomeAt(c).LowestWildAndCavePlantOrder))
					{
						float num = 7f;
						if (thingDef.plant.GrowsInClusters)
						{
							num = Math.Max(num, (float)thingDef.plant.wildClusterRadius * 1.5f);
						}
						if (!this.EnoughLowerOrderPlantsNearby(c, plantDensityFactor, num, thingDef))
						{
							continue;
						}
					}
					outPlants.Add(thingDef);
				}
			}
		}

		// Token: 0x06007D4D RID: 32077 RVA: 0x002CCF90 File Offset: 0x002CB190
		private bool EnoughLowerOrderPlantsNearby(IntVec3 c, float plantDensityFactor, float radiusToScan, ThingDef plantDef)
		{
			float num = 0f;
			WildPlantSpawner.tmpPlantDefsLowerOrder.Clear();
			List<BiomePlantRecord> wildPlants = this.map.BiomeAt(c).wildPlants;
			for (int i = 0; i < wildPlants.Count; i++)
			{
				if (wildPlants[i].plant.plant.wildOrder < plantDef.plant.wildOrder)
				{
					num += this.GetCommonalityPctOfPlant(wildPlants[i].plant);
					WildPlantSpawner.tmpPlantDefsLowerOrder.Add(wildPlants[i].plant);
				}
			}
			float numDesiredPlantsLocally = 0f;
			int numPlantsLowerOrder = 0;
			RegionTraverser.BreadthFirstTraverse(c, this.map, (Region from, Region to) => c.InHorDistOf(to.extentsClose.ClosestCellTo(c), radiusToScan), delegate(Region reg)
			{
				numDesiredPlantsLocally += this.GetDesiredPlantsCountIn(reg, c, plantDensityFactor);
				for (int j = 0; j < WildPlantSpawner.tmpPlantDefsLowerOrder.Count; j++)
				{
					numPlantsLowerOrder += reg.ListerThings.ThingsOfDef(WildPlantSpawner.tmpPlantDefsLowerOrder[j]).Count;
				}
				return false;
			}, 999999, RegionType.Set_Passable);
			float num2 = numDesiredPlantsLocally * num;
			return num2 < 4f || (float)numPlantsLowerOrder / num2 >= 0.57f;
		}

		// Token: 0x06007D4E RID: 32078 RVA: 0x002CD0B8 File Offset: 0x002CB2B8
		private bool SaturatedAt(IntVec3 c, float plantDensityFactor, bool cavePlants, float wholeMapNumDesiredPlants)
		{
			int num = GenRadial.NumCellsInRadius(20f);
			if (wholeMapNumDesiredPlants * ((float)num / (float)this.map.Area) <= 4f || (!MapGenUtility.IsMixedBiome(this.map) && !this.map.BiomeAt(c).wildPlantsCareAboutLocalFertility))
			{
				return (float)this.map.listerThings.ThingsInGroup(ThingRequestGroup.NonStumpPlant).Count >= wholeMapNumDesiredPlants;
			}
			float numDesiredPlantsLocally = 0f;
			int numPlants = 0;
			RegionTraverser.BreadthFirstTraverse(c, this.map, (Region from, Region to) => c.InHorDistOf(to.extentsClose.ClosestCellTo(c), 20f), delegate(Region reg)
			{
				numDesiredPlantsLocally += this.GetDesiredPlantsCountIn(reg, c, plantDensityFactor);
				numPlants += reg.ListerThings.ThingsInGroup(ThingRequestGroup.NonStumpPlant).Count;
				return false;
			}, 999999, RegionType.Set_Passable);
			return (float)numPlants >= numDesiredPlantsLocally;
		}

		// Token: 0x06007D4F RID: 32079 RVA: 0x002CD1A0 File Offset: 0x002CB3A0
		private void CalculateDistancesToNearbyClusters(IntVec3 c)
		{
			WildPlantSpawner.nearbyClusters.Clear();
			WildPlantSpawner.nearbyClustersList.Clear();
			int num = GenRadial.NumCellsInRadius((float)(this.map.BiomeAt(c).MaxWildAndCavePlantsClusterRadius * 2));
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = c + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.map))
				{
					List<Thing> list = this.map.thingGrid.ThingsListAtFast(intVec);
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing = list[j];
						if (thing.def.category == ThingCategory.Plant && thing.def.plant.GrowsInClusters)
						{
							float item = (float)intVec.DistanceToSquared(c);
							List<float> list2;
							if (!WildPlantSpawner.nearbyClusters.TryGetValue(thing.def, out list2))
							{
								list2 = SimplePool<List<float>>.Get();
								WildPlantSpawner.nearbyClusters.Add(thing.def, list2);
								WildPlantSpawner.nearbyClustersList.Add(new KeyValuePair<ThingDef, List<float>>(thing.def, list2));
							}
							list2.Add(item);
						}
					}
				}
			}
			WildPlantSpawner.distanceSqToNearbyClusters.Clear();
			for (int k = 0; k < WildPlantSpawner.nearbyClustersList.Count; k++)
			{
				List<float> value = WildPlantSpawner.nearbyClustersList[k].Value;
				value.Sort();
				WildPlantSpawner.distanceSqToNearbyClusters.Add(WildPlantSpawner.nearbyClustersList[k].Key, value[value.Count / 2]);
				value.Clear();
				SimplePool<List<float>>.Return(value);
			}
		}

		// Token: 0x06007D50 RID: 32080 RVA: 0x002CD341 File Offset: 0x002CB541
		private bool CanRegrowAt(IntVec3 c)
		{
			return !c.Roofed(this.map) || this.GoodRoofForCavePlant(c);
		}

		// Token: 0x06007D51 RID: 32081 RVA: 0x002CD35C File Offset: 0x002CB55C
		private bool GoodRoofForCavePlant(IntVec3 c)
		{
			RoofDef roof = c.GetRoof(this.map);
			return roof != null && roof.isNatural;
		}

		// Token: 0x06007D52 RID: 32082 RVA: 0x002CD384 File Offset: 0x002CB584
		private float GetCommonalityOfPlant(ThingDef plant)
		{
			this.CachePlantCommonalitiesIfShould();
			if (plant.plant.cavePlant && !this.map.Biome.wildPlantsAreCavePlants)
			{
				return plant.plant.cavePlantWeight;
			}
			return this.cachedPlantCommonalities.GetValueOrDefault(plant, 0f);
		}

		// Token: 0x06007D53 RID: 32083 RVA: 0x002CD3D3 File Offset: 0x002CB5D3
		public float GetCommonalityPctOfPlant(ThingDef plant)
		{
			if (!plant.plant.cavePlant || this.map.Biome.wildPlantsAreCavePlants)
			{
				return this.GetCommonalityOfPlant(plant) / this.PlantsCommonalitiesSum;
			}
			return this.GetCommonalityOfPlant(plant) / this.CavePlantsCommonalitiesSum;
		}

		// Token: 0x06007D54 RID: 32084 RVA: 0x002CD414 File Offset: 0x002CB614
		public float GetBaseDesiredPlantsCountAt(IntVec3 c)
		{
			float num = this.map.fertilityGrid.FertilityAt(c);
			if (this.GoodRoofForCavePlant(c))
			{
				num *= 0.5f;
			}
			if (this.HaveAnyPlantsWhichIgnoreFertility && c.GetTerrain(this.map).IsWater && num <= 0f)
			{
				num = 0.1f;
			}
			return num;
		}

		// Token: 0x06007D55 RID: 32085 RVA: 0x002CD470 File Offset: 0x002CB670
		public float GetDesiredPlantsCountAt(IntVec3 forCell, float plantDensityFactor)
		{
			float num = this.map.fertilityGrid.FertilityAt(forCell);
			if (num <= 0f && this.HaveAnyPlantsWhichIgnoreFertility)
			{
				num = 1f;
			}
			float num2 = this.map.BiomeAt(forCell).plantDensity * plantDensityFactor * num;
			return Mathf.Min(this.GetBaseDesiredPlantsCountAt(forCell) * num2, 1f);
		}

		// Token: 0x06007D56 RID: 32086 RVA: 0x002CD4D0 File Offset: 0x002CB6D0
		public float GetDesiredPlantsCountIn(Region reg, IntVec3 forCell, float plantDensityFactor)
		{
			float num = this.map.fertilityGrid.FertilityAt(forCell);
			if (num <= 0f && this.HaveAnyPlantsWhichIgnoreFertility)
			{
				num = 1f;
			}
			float num2 = this.map.BiomeAt(forCell).plantDensity * plantDensityFactor * num;
			return Mathf.Min(reg.GetBaseDesiredPlantsCount(true) * num2, (float)reg.CellCount);
		}

		// Token: 0x06007D57 RID: 32087 RVA: 0x002CD530 File Offset: 0x002CB730
		public float GetWaterPlantDistanceToShoreWeight(IntVec3 rootCell)
		{
			if (!rootCell.GetTerrain(this.map).IsWater)
			{
				return 1f;
			}
			float distance = float.MaxValue;
			this.map.floodFiller.FloodFill(rootCell, (IntVec3 c) => true, delegate(IntVec3 c)
			{
				if (this.map.terrainGrid.BaseTerrainAt(c).IsWater)
				{
					return false;
				}
				distance = c.DistanceTo(rootCell);
				return true;
			}, int.MaxValue, false, null);
			return Mathf.Clamp01(1f - distance / 4f);
		}

		// Token: 0x04004ED6 RID: 20182
		private readonly Map map;

		// Token: 0x04004ED7 RID: 20183
		private int cycleIndex;

		// Token: 0x04004ED8 RID: 20184
		private float calculatedWholeMapNumDesiredPlants;

		// Token: 0x04004ED9 RID: 20185
		private float calculatedWholeMapNumDesiredPlantsTmp;

		// Token: 0x04004EDA RID: 20186
		private int calculatedWholeMapNumNonZeroFertilityCells;

		// Token: 0x04004EDB RID: 20187
		private int calculatedWholeMapNumNonZeroFertilityCellsTmp;

		// Token: 0x04004EDC RID: 20188
		private bool hasWholeMapNumDesiredPlantsCalculated;

		// Token: 0x04004EDD RID: 20189
		[Unsaved(false)]
		private List<ThingDef> cachedWildPlants;

		// Token: 0x04004EDE RID: 20190
		[Unsaved(false)]
		private List<ThingDef> cachedMutatorWildPlants;

		// Token: 0x04004EDF RID: 20191
		[Unsaved(false)]
		private Dictionary<ThingDef, float> cachedPlantCommonalities;

		// Token: 0x04004EE0 RID: 20192
		[Unsaved(false)]
		private float? cachedPlantsCommonalitiesSum;

		// Token: 0x04004EE1 RID: 20193
		[Unsaved(false)]
		private float? cachedCavePlantsCommonalitiesSum;

		// Token: 0x04004EE2 RID: 20194
		[Unsaved(false)]
		private bool? cachedHaveAnyPlantsWhichIgnoreFertility;

		// Token: 0x04004EE3 RID: 20195
		[Unsaved(false)]
		private SimpleCurve cachedOverrideDensityForFertilityCurve;

		// Token: 0x04004EE4 RID: 20196
		private static readonly List<ThingDef> allCavePlants = new List<ThingDef>();

		// Token: 0x04004EE5 RID: 20197
		private static readonly List<ThingDef> tmpPossiblePlants = new List<ThingDef>();

		// Token: 0x04004EE6 RID: 20198
		private static readonly List<KeyValuePair<ThingDef, float>> tmpPossiblePlantsWithWeight = new List<KeyValuePair<ThingDef, float>>();

		// Token: 0x04004EE7 RID: 20199
		private static readonly Dictionary<ThingDef, float> distanceSqToNearbyClusters = new Dictionary<ThingDef, float>();

		// Token: 0x04004EE8 RID: 20200
		private static readonly Dictionary<ThingDef, List<float>> nearbyClusters = new Dictionary<ThingDef, List<float>>();

		// Token: 0x04004EE9 RID: 20201
		private static readonly List<KeyValuePair<ThingDef, List<float>>> nearbyClustersList = new List<KeyValuePair<ThingDef, List<float>>>();

		// Token: 0x04004EEA RID: 20202
		public static readonly FloatRange InitialGrowthRandomRange = new FloatRange(0.15f, 1.5f);

		// Token: 0x04004EEB RID: 20203
		private const float CavePlantsDensityFactor = 0.5f;

		// Token: 0x04004EEC RID: 20204
		private const float WaterPlantsDensityFactor = 0.1f;

		// Token: 0x04004EED RID: 20205
		private const int PlantSaturationScanRadius = 20;

		// Token: 0x04004EEE RID: 20206
		private const float MapFractionCheckPerTick = 0.0001f;

		// Token: 0x04004EEF RID: 20207
		private const float ChanceToRegrow = 0.012f;

		// Token: 0x04004EF0 RID: 20208
		private const float CavePlantChanceToRegrow = 0.0001f;

		// Token: 0x04004EF1 RID: 20209
		private const float BaseLowerOrderScanRadius = 7f;

		// Token: 0x04004EF2 RID: 20210
		private const float LowerOrderScanRadiusWildClusterRadiusFactor = 1.5f;

		// Token: 0x04004EF3 RID: 20211
		private const float MinDesiredLowerOrderPlantsToConsiderSkipping = 4f;

		// Token: 0x04004EF4 RID: 20212
		private const float MinLowerOrderPlantsPct = 0.57f;

		// Token: 0x04004EF5 RID: 20213
		private const float LocalPlantProportionsMaxScanRadius = 25f;

		// Token: 0x04004EF6 RID: 20214
		private const float MaxLocalProportionsBias = 7f;

		// Token: 0x04004EF7 RID: 20215
		private const float CavePlantRegrowDays = 130f;

		// Token: 0x04004EF8 RID: 20216
		private static readonly SimpleCurve GlobalPctSelectionWeightBias = new SimpleCurve
		{
			{
				new CurvePoint(0f, 3f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(1.5f, 0.25f),
				true
			},
			{
				new CurvePoint(3f, 0.02f),
				true
			}
		};

		// Token: 0x04004EF9 RID: 20217
		private static readonly List<ThingDef> tmpWildPlants = new List<ThingDef>();

		// Token: 0x04004EFA RID: 20218
		private static readonly List<ThingDef> tmpPlantDefsLowerOrder = new List<ThingDef>();
	}
}
