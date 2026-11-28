using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F3F RID: 3903
	public class BiomeDef : Def
	{
		// Token: 0x17001126 RID: 4390
		// (get) Token: 0x060064A5 RID: 25765 RVA: 0x0023EE38 File Offset: 0x0023D038
		public BiomeWorker Worker
		{
			get
			{
				BiomeWorker result;
				if ((result = this.worker) == null)
				{
					result = (this.worker = GenWorker<BiomeWorker>.Get(this.workerClass));
				}
				return result;
			}
		}

		// Token: 0x17001127 RID: 4391
		// (get) Token: 0x060064A6 RID: 25766 RVA: 0x0023EE64 File Offset: 0x0023D064
		public Material DrawMaterial
		{
			get
			{
				if (this.cachedMat == null)
				{
					if (this.texture.NullOrEmpty())
					{
						return null;
					}
					if (this == BiomeDefOf.Ocean || this == BiomeDefOf.Lake)
					{
						this.cachedMat = MaterialAllocator.Create(WorldMaterials.WorldOcean);
					}
					else if (!this.allowRoads && !this.allowRivers)
					{
						this.cachedMat = MaterialAllocator.Create(WorldMaterials.WorldIce);
					}
					else
					{
						this.cachedMat = MaterialAllocator.Create(WorldMaterials.WorldTerrain);
					}
					this.cachedMat.mainTexture = ContentFinder<Texture2D>.Get(this.texture, true);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x17001128 RID: 4392
		// (get) Token: 0x060064A7 RID: 25767 RVA: 0x0023EF00 File Offset: 0x0023D100
		public List<ThingDef> AllWildPlants
		{
			get
			{
				if (this.cachedWildPlants == null)
				{
					this.cachedWildPlants = new List<ThingDef>();
					foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
					{
						if (thingDef.category == ThingCategory.Plant && this.CommonalityOfPlant(thingDef) > 0f)
						{
							this.cachedWildPlants.Add(thingDef);
						}
					}
				}
				return this.cachedWildPlants;
			}
		}

		// Token: 0x17001129 RID: 4393
		// (get) Token: 0x060064A8 RID: 25768 RVA: 0x0023EF88 File Offset: 0x0023D188
		public int MaxWildAndCavePlantsClusterRadius
		{
			get
			{
				if (this.cachedMaxWildPlantsClusterRadius == null)
				{
					this.cachedMaxWildPlantsClusterRadius = new int?(0);
					List<ThingDef> allWildPlants = this.AllWildPlants;
					for (int i = 0; i < allWildPlants.Count; i++)
					{
						if (allWildPlants[i].plant.GrowsInClusters)
						{
							this.cachedMaxWildPlantsClusterRadius = new int?(Mathf.Max(this.cachedMaxWildPlantsClusterRadius.Value, allWildPlants[i].plant.wildClusterRadius));
						}
					}
					List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int j = 0; j < allDefsListForReading.Count; j++)
					{
						if (allDefsListForReading[j].category == ThingCategory.Plant && allDefsListForReading[j].plant.cavePlant)
						{
							this.cachedMaxWildPlantsClusterRadius = new int?(Mathf.Max(this.cachedMaxWildPlantsClusterRadius.Value, allDefsListForReading[j].plant.wildClusterRadius));
						}
					}
				}
				return this.cachedMaxWildPlantsClusterRadius.Value;
			}
		}

		// Token: 0x1700112A RID: 4394
		// (get) Token: 0x060064A9 RID: 25769 RVA: 0x0023F078 File Offset: 0x0023D278
		public float LowestWildAndCavePlantOrder
		{
			get
			{
				if (this.cachedLowestWildPlantOrder == null)
				{
					this.cachedLowestWildPlantOrder = new float?((float)int.MaxValue);
					List<ThingDef> allWildPlants = this.AllWildPlants;
					for (int i = 0; i < allWildPlants.Count; i++)
					{
						this.cachedLowestWildPlantOrder = new float?(Mathf.Min(this.cachedLowestWildPlantOrder.Value, allWildPlants[i].plant.wildOrder));
					}
					List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int j = 0; j < allDefsListForReading.Count; j++)
					{
						if (allDefsListForReading[j].category == ThingCategory.Plant && allDefsListForReading[j].plant.cavePlant)
						{
							this.cachedLowestWildPlantOrder = new float?(Mathf.Min(this.cachedLowestWildPlantOrder.Value, allDefsListForReading[j].plant.wildOrder));
						}
					}
				}
				return this.cachedLowestWildPlantOrder.Value;
			}
		}

		// Token: 0x1700112B RID: 4395
		// (get) Token: 0x060064AA RID: 25770 RVA: 0x0023F15A File Offset: 0x0023D35A
		public IEnumerable<PawnKindDef> AllWildAnimals
		{
			get
			{
				BiomeDef.<get_AllWildAnimals>d__94 <get_AllWildAnimals>d__ = new BiomeDef.<get_AllWildAnimals>d__94(-2);
				<get_AllWildAnimals>d__.<>4__this = this;
				return <get_AllWildAnimals>d__;
			}
		}

		// Token: 0x1700112C RID: 4396
		// (get) Token: 0x060064AB RID: 25771 RVA: 0x0023F16C File Offset: 0x0023D36C
		public float TreeDensity
		{
			get
			{
				float num = 0f;
				float num2 = 0f;
				this.CachePlantCommonalitiesIfShould();
				foreach (KeyValuePair<ThingDef, float> keyValuePair in this.cachedPlantCommonalities)
				{
					num += keyValuePair.Value;
					if (keyValuePair.Key.plant.IsTree)
					{
						num2 += keyValuePair.Value;
					}
				}
				if (num == 0f)
				{
					return 0f;
				}
				return num2 / num * this.plantDensity;
			}
		}

		// Token: 0x1700112D RID: 4397
		// (get) Token: 0x060064AC RID: 25772 RVA: 0x0023F20C File Offset: 0x0023D40C
		public int TreeSightingsPerHourFromCaravan
		{
			get
			{
				return Mathf.FloorToInt(this.TreeDensity * 25f);
			}
		}

		// Token: 0x060064AD RID: 25773 RVA: 0x0023F220 File Offset: 0x0023D420
		public TerrainDef TerrainForAffordance(TerrainAffordanceDef affordance)
		{
			foreach (TerrainThreshold terrainThreshold in this.terrainsByFertility)
			{
				if (terrainThreshold.terrain.affordances.Contains(affordance))
				{
					return terrainThreshold.terrain;
				}
			}
			return TerrainDefOf.Soil;
		}

		// Token: 0x060064AE RID: 25774 RVA: 0x0023F290 File Offset: 0x0023D490
		public float CommonalityOfAnimal(PawnKindDef animalDef)
		{
			if (this.cachedAnimalCommonalities == null)
			{
				this.cachedAnimalCommonalities = new Dictionary<PawnKindDef, float>();
				for (int i = 0; i < this.wildAnimals.Count; i++)
				{
					this.cachedAnimalCommonalities.Add(this.wildAnimals[i].animal, this.wildAnimals[i].commonality);
				}
				foreach (PawnKindDef pawnKindDef in DefDatabase<PawnKindDef>.AllDefs)
				{
					if (pawnKindDef.RaceProps.wildBiomes != null)
					{
						for (int j = 0; j < pawnKindDef.RaceProps.wildBiomes.Count; j++)
						{
							if (pawnKindDef.RaceProps.wildBiomes[j].biome == this)
							{
								this.cachedAnimalCommonalities.Add(pawnKindDef, pawnKindDef.RaceProps.wildBiomes[j].commonality);
							}
						}
					}
				}
			}
			float result;
			if (this.cachedAnimalCommonalities.TryGetValue(animalDef, out result))
			{
				return result;
			}
			return 0f;
		}

		// Token: 0x060064AF RID: 25775 RVA: 0x0023F3B0 File Offset: 0x0023D5B0
		public float CommonalityOfPollutionAnimal(PawnKindDef animalDef)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			if (this.cachedPollutionAnimalCommonalities == null)
			{
				this.cachedPollutionAnimalCommonalities = new Dictionary<PawnKindDef, float>();
				for (int i = 0; i < this.pollutionWildAnimals.Count; i++)
				{
					this.cachedPollutionAnimalCommonalities.Add(this.pollutionWildAnimals[i].animal, this.pollutionWildAnimals[i].commonality);
				}
			}
			float result;
			if (this.cachedPollutionAnimalCommonalities.TryGetValue(animalDef, out result))
			{
				return result;
			}
			return 0f;
		}

		// Token: 0x060064B0 RID: 25776 RVA: 0x0023F438 File Offset: 0x0023D638
		public float CommonalityOfCoastalAnimal(PawnKindDef animalDef)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return 0f;
			}
			if (this.cachedCoastalAnimalCommonalities == null)
			{
				this.cachedCoastalAnimalCommonalities = new Dictionary<PawnKindDef, float>();
				for (int i = 0; i < this.coastalWildAnimals.Count; i++)
				{
					this.cachedCoastalAnimalCommonalities.Add(this.coastalWildAnimals[i].animal, this.coastalWildAnimals[i].commonality);
				}
			}
			float result;
			if (this.cachedCoastalAnimalCommonalities.TryGetValue(animalDef, out result))
			{
				return result;
			}
			return 0f;
		}

		// Token: 0x060064B1 RID: 25777 RVA: 0x0023F4BF File Offset: 0x0023D6BF
		public bool ShouldSpawnAnimalOnCoast(PawnKindDef animalDef)
		{
			return this.CommonalityOfCoastalAnimal(animalDef) > this.CommonalityOfAnimal(animalDef);
		}

		// Token: 0x060064B2 RID: 25778 RVA: 0x0023F4D4 File Offset: 0x0023D6D4
		public float CommonalityOfPlant(ThingDef plantDef)
		{
			this.CachePlantCommonalitiesIfShould();
			float result;
			if (this.cachedPlantCommonalities.TryGetValue(plantDef, out result))
			{
				return result;
			}
			return 0f;
		}

		// Token: 0x060064B3 RID: 25779 RVA: 0x0023F500 File Offset: 0x0023D700
		public float CommonalityOfDisease(IncidentDef diseaseInc)
		{
			if (this.cachedDiseaseCommonalities == null)
			{
				this.cachedDiseaseCommonalities = new Dictionary<IncidentDef, float>();
				for (int i = 0; i < this.diseases.Count; i++)
				{
					this.cachedDiseaseCommonalities.Add(this.diseases[i].diseaseInc, this.diseases[i].commonality);
				}
				foreach (IncidentDef incidentDef in DefDatabase<IncidentDef>.AllDefs)
				{
					if (incidentDef.diseaseBiomeRecords != null)
					{
						for (int j = 0; j < incidentDef.diseaseBiomeRecords.Count; j++)
						{
							if (incidentDef.diseaseBiomeRecords[j].biome == this)
							{
								this.cachedDiseaseCommonalities.Add(incidentDef.diseaseBiomeRecords[j].diseaseInc, incidentDef.diseaseBiomeRecords[j].commonality);
							}
						}
					}
				}
			}
			float result;
			if (this.cachedDiseaseCommonalities.TryGetValue(diseaseInc, out result))
			{
				return result;
			}
			return 0f;
		}

		// Token: 0x060064B4 RID: 25780 RVA: 0x0023F61C File Offset: 0x0023D81C
		private void CachePlantCommonalitiesIfShould()
		{
			if (this.cachedPlantCommonalities == null)
			{
				this.cachedPlantCommonalities = new Dictionary<ThingDef, float>();
				for (int i = 0; i < this.wildPlants.Count; i++)
				{
					if (this.wildPlants[i].plant != null)
					{
						this.cachedPlantCommonalities.Add(this.wildPlants[i].plant, this.wildPlants[i].commonality);
					}
				}
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
				{
					if (thingDef.plant != null && thingDef.plant.wildBiomes != null)
					{
						for (int j = 0; j < thingDef.plant.wildBiomes.Count; j++)
						{
							if (thingDef.plant.wildBiomes[j].biome == this)
							{
								if (this.cachedPlantCommonalities.ContainsKey(thingDef))
								{
									this.cachedPlantCommonalities[thingDef] = (this.cachedPlantCommonalities[thingDef] + thingDef.plant.wildBiomes[j].commonality) / 2f;
								}
								else
								{
									this.cachedPlantCommonalities.Add(thingDef, thingDef.plant.wildBiomes[j].commonality);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060064B5 RID: 25781 RVA: 0x0023F790 File Offset: 0x0023D990
		public bool IsPackAnimalAllowed(ThingDef pawn)
		{
			return this.allowedPackAnimals.Contains(pawn);
		}

		// Token: 0x060064B6 RID: 25782 RVA: 0x0023F79E File Offset: 0x0023D99E
		public static BiomeDef Named(string defName)
		{
			return DefDatabase<BiomeDef>.GetNamed(defName, true);
		}

		// Token: 0x060064B7 RID: 25783 RVA: 0x0023F7A7 File Offset: 0x0023D9A7
		public override IEnumerable<string> ConfigErrors()
		{
			BiomeDef.<ConfigErrors>d__109 <ConfigErrors>d__ = new BiomeDef.<ConfigErrors>d__109(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040039F0 RID: 14832
		public Type workerClass;

		// Token: 0x040039F1 RID: 14833
		public bool implemented = true;

		// Token: 0x040039F2 RID: 14834
		public bool generatesNaturally = true;

		// Token: 0x040039F3 RID: 14835
		public bool canBuildBase = true;

		// Token: 0x040039F4 RID: 14836
		public bool canAutoChoose = true;

		// Token: 0x040039F5 RID: 14837
		public bool allowRoads = true;

		// Token: 0x040039F6 RID: 14838
		public bool allowRivers = true;

		// Token: 0x040039F7 RID: 14839
		public bool allowFarmingCamps = true;

		// Token: 0x040039F8 RID: 14840
		public float animalDensity;

		// Token: 0x040039F9 RID: 14841
		public float plantDensity;

		// Token: 0x040039FA RID: 14842
		public float diseaseMtbDays = 60f;

		// Token: 0x040039FB RID: 14843
		public float settlementSelectionWeight = 1f;

		// Token: 0x040039FC RID: 14844
		public float campSelectionWeight = 1f;

		// Token: 0x040039FD RID: 14845
		public float pollutionOffset;

		// Token: 0x040039FE RID: 14846
		public bool impassable;

		// Token: 0x040039FF RID: 14847
		public bool hasVirtualPlants = true;

		// Token: 0x04003A00 RID: 14848
		public float forageability;

		// Token: 0x04003A01 RID: 14849
		public ThingDef foragedFood;

		// Token: 0x04003A02 RID: 14850
		public bool wildPlantsCareAboutLocalFertility = true;

		// Token: 0x04003A03 RID: 14851
		public bool wildPlantsAreCavePlants;

		// Token: 0x04003A04 RID: 14852
		public float wildPlantRegrowDays = 25f;

		// Token: 0x04003A05 RID: 14853
		public float movementDifficulty = 1f;

		// Token: 0x04003A06 RID: 14854
		public List<WeatherCommonalityRecord> baseWeatherCommonalities = new List<WeatherCommonalityRecord>();

		// Token: 0x04003A07 RID: 14855
		public List<TerrainThreshold> terrainsByFertility = new List<TerrainThreshold>();

		// Token: 0x04003A08 RID: 14856
		public List<SoundDef> soundsAmbient = new List<SoundDef>();

		// Token: 0x04003A09 RID: 14857
		public List<TerrainPatchMaker> terrainPatchMakers = new List<TerrainPatchMaker>();

		// Token: 0x04003A0A RID: 14858
		public List<BiomePlantRecord> wildPlants = new List<BiomePlantRecord>();

		// Token: 0x04003A0B RID: 14859
		private List<BiomeAnimalRecord> wildAnimals = new List<BiomeAnimalRecord>();

		// Token: 0x04003A0C RID: 14860
		private List<BiomeAnimalRecord> pollutionWildAnimals = new List<BiomeAnimalRecord>();

		// Token: 0x04003A0D RID: 14861
		private List<BiomeAnimalRecord> coastalWildAnimals = new List<BiomeAnimalRecord>();

		// Token: 0x04003A0E RID: 14862
		private List<BiomeDiseaseRecord> diseases = new List<BiomeDiseaseRecord>();

		// Token: 0x04003A0F RID: 14863
		private List<ThingDef> allowedPackAnimals = new List<ThingDef>();

		// Token: 0x04003A10 RID: 14864
		public bool hasBedrock = true;

		// Token: 0x04003A11 RID: 14865
		public bool isExtremeBiome;

		// Token: 0x04003A12 RID: 14866
		public bool isWaterBiome;

		// Token: 0x04003A13 RID: 14867
		public bool allowPollution = true;

		// Token: 0x04003A14 RID: 14868
		public bool wildAnimalsCanWanderInto = true;

		// Token: 0x04003A15 RID: 14869
		public bool noAmbientWind;

		// Token: 0x04003A16 RID: 14870
		public bool inVacuum;

		// Token: 0x04003A17 RID: 14871
		public bool disableSkyLighting;

		// Token: 0x04003A18 RID: 14872
		public bool disableShadows;

		// Token: 0x04003A19 RID: 14873
		public bool noGravel;

		// Token: 0x04003A1A RID: 14874
		public bool canExitMap = true;

		// Token: 0x04003A1B RID: 14875
		public bool onlyAllowWhitelistedArrivalModes;

		// Token: 0x04003A1C RID: 14876
		public bool isBackgroundBiome;

		// Token: 0x04003A1D RID: 14877
		public float wildAnimalScariaChance;

		// Token: 0x04003A1E RID: 14878
		public float geyserCountFactor = 1f;

		// Token: 0x04003A1F RID: 14879
		public float? constantOutdoorTemperature;

		// Token: 0x04003A20 RID: 14880
		public TerrainDef coastalBeachTerrain;

		// Token: 0x04003A21 RID: 14881
		public TerrainDef lakeBeachTerrain;

		// Token: 0x04003A22 RID: 14882
		public TerrainDef riverbankTerrain;

		// Token: 0x04003A23 RID: 14883
		public TerrainDef mudTerrain;

		// Token: 0x04003A24 RID: 14884
		public TerrainDef gravelTerrain;

		// Token: 0x04003A25 RID: 14885
		public TerrainDef waterShallowTerrain;

		// Token: 0x04003A26 RID: 14886
		public TerrainDef waterDeepTerrain;

		// Token: 0x04003A27 RID: 14887
		public TerrainDef oceanShallowTerrain;

		// Token: 0x04003A28 RID: 14888
		public TerrainDef oceanDeepTerrain;

		// Token: 0x04003A29 RID: 14889
		public TerrainDef waterMovingShallowTerrain;

		// Token: 0x04003A2A RID: 14890
		public TerrainDef waterMovingChestDeepTerrain;

		// Token: 0x04003A2B RID: 14891
		public IntRange riverbankSizeRange;

		// Token: 0x04003A2C RID: 14892
		public List<GameConditionDef> biomeMapConditions = new List<GameConditionDef>();

		// Token: 0x04003A2D RID: 14893
		public List<GenStepDef> extraGenSteps = new List<GenStepDef>();

		// Token: 0x04003A2E RID: 14894
		public List<GenStepDef> preventGenSteps = new List<GenStepDef>();

		// Token: 0x04003A2F RID: 14895
		public List<ThingDef> extraRockTypes;

		// Token: 0x04003A30 RID: 14896
		public List<ThingDef> forceRockTypes;

		// Token: 0x04003A31 RID: 14897
		public float maxFishPopulation;

		// Token: 0x04003A32 RID: 14898
		public BiomeFishTypes fishTypes;

		// Token: 0x04003A33 RID: 14899
		public List<PlanetLayerDef> layerWhitelist;

		// Token: 0x04003A34 RID: 14900
		public List<PlanetLayerDef> layerBlacklist;

		// Token: 0x04003A35 RID: 14901
		[MustTranslate]
		public string settleWarning;

		// Token: 0x04003A36 RID: 14902
		public Color? fogOfWarColor;

		// Token: 0x04003A37 RID: 14903
		public OrbitalDebrisDef orbitalDebris;

		// Token: 0x04003A38 RID: 14904
		[NoTranslate]
		public string texture;

		// Token: 0x04003A39 RID: 14905
		[Unsaved(false)]
		private Dictionary<PawnKindDef, float> cachedAnimalCommonalities;

		// Token: 0x04003A3A RID: 14906
		[Unsaved(false)]
		private Dictionary<PawnKindDef, float> cachedPollutionAnimalCommonalities;

		// Token: 0x04003A3B RID: 14907
		[Unsaved(false)]
		private Dictionary<PawnKindDef, float> cachedCoastalAnimalCommonalities;

		// Token: 0x04003A3C RID: 14908
		[Unsaved(false)]
		private Dictionary<ThingDef, float> cachedPlantCommonalities;

		// Token: 0x04003A3D RID: 14909
		[Unsaved(false)]
		private Dictionary<IncidentDef, float> cachedDiseaseCommonalities;

		// Token: 0x04003A3E RID: 14910
		[Unsaved(false)]
		private Material cachedMat;

		// Token: 0x04003A3F RID: 14911
		[Unsaved(false)]
		private List<ThingDef> cachedWildPlants;

		// Token: 0x04003A40 RID: 14912
		[Unsaved(false)]
		private float? cachedLowestWildPlantOrder;

		// Token: 0x04003A41 RID: 14913
		[Unsaved(false)]
		private int? cachedMaxWildPlantsClusterRadius;

		// Token: 0x04003A42 RID: 14914
		private BiomeWorker worker;
	}
}
