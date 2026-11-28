using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using RimWorld;

namespace Verse
{
	// Token: 0x020005C6 RID: 1478
	public static class DebugOutputsEcology
	{
		// Token: 0x06002E2A RID: 11818 RVA: 0x0011BB9C File Offset: 0x00119D9C
		[DebugOutput]
		public static void Plants()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Plant
			orderby d.plant.fertilitySensitivity
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[12];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("grow days", (ThingDef d) => d.plant.growDays.ToString("F2"));
			array[2] = new TableDataGetter<ThingDef>("nutrition", (ThingDef d) => DebugOutputsEcology.<Plants>g__Nutrition|0_0(d).ToString("F2"));
			array[3] = new TableDataGetter<ThingDef>("nutrition\n/day", (ThingDef d) => (DebugOutputsEcology.<Plants>g__Nutrition|0_0(d) / d.plant.growDays).ToString("F4"));
			array[4] = new TableDataGetter<ThingDef>("fertility\nmin", (ThingDef d) => d.plant.fertilityMin.ToString("F2"));
			array[5] = new TableDataGetter<ThingDef>("fertility\nsensitivity", (ThingDef d) => d.plant.fertilitySensitivity.ToString("F2"));
			array[6] = new TableDataGetter<ThingDef>("harvest\nnutrition", (ThingDef d) => DebugOutputsEcology.<Plants>g__HarvestNutrition|0_1(d).ToString("F2"));
			array[7] = new TableDataGetter<ThingDef>("nutrition\n/ harvest nutrition", delegate(ThingDef d)
			{
				if (DebugOutputsEcology.<Plants>g__HarvestNutrition|0_1(d) > 0f)
				{
					return (DebugOutputsEcology.<Plants>g__Nutrition|0_0(d) / DebugOutputsEcology.<Plants>g__HarvestNutrition|0_1(d)).ToString("F2");
				}
				return "";
			});
			array[8] = new TableDataGetter<ThingDef>("tree", delegate(ThingDef d)
			{
				if (!d.plant.IsTree)
				{
					return "";
				}
				return "yes";
			});
			array[9] = new TableDataGetter<ThingDef>("farm animal edible", delegate(ThingDef d)
			{
				if (!MapPlantGrowthRateCalculator.IsEdibleByPastureAnimals(d))
				{
					return "";
				}
				return "yes";
			});
			array[10] = new TableDataGetter<ThingDef>("minifiable", (ThingDef d) => d.Minifiable);
			array[11] = new TableDataGetter<ThingDef>("treelovers\ncare", (ThingDef d) => d.plant.treeLoversCareIfChopped);
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x0011BE15 File Offset: 0x0011A015
		[DebugOutput(true)]
		public static void PlantCurrentProportions()
		{
			PlantUtility.LogPlantProportions();
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x0011BE1C File Offset: 0x0011A01C
		[DebugOutput]
		public static void Biomes()
		{
			IEnumerable<BiomeDef> dataSources = from d in DefDatabase<BiomeDef>.AllDefs
			orderby d.plantDensity descending
			select d;
			TableDataGetter<BiomeDef>[] array = new TableDataGetter<BiomeDef>[12];
			array[0] = new TableDataGetter<BiomeDef>("defName", (BiomeDef d) => d.defName);
			array[1] = new TableDataGetter<BiomeDef>("animalDensity", (BiomeDef d) => d.animalDensity.ToString("F2"));
			array[2] = new TableDataGetter<BiomeDef>("plantDensity", (BiomeDef d) => d.plantDensity.ToString("F2"));
			array[3] = new TableDataGetter<BiomeDef>("tree density", (BiomeDef d) => d.TreeDensity.ToStringPercent());
			array[4] = new TableDataGetter<BiomeDef>("tree sightings\nper hour", (BiomeDef d) => d.TreeSightingsPerHourFromCaravan);
			array[5] = new TableDataGetter<BiomeDef>("diseaseMtbDays", (BiomeDef d) => d.diseaseMtbDays.ToString("F0"));
			array[6] = new TableDataGetter<BiomeDef>("movementDifficulty", delegate(BiomeDef d)
			{
				if (!d.impassable)
				{
					return d.movementDifficulty.ToString("F1");
				}
				return "-";
			});
			array[7] = new TableDataGetter<BiomeDef>("forageability", (BiomeDef d) => d.forageability.ToStringPercent());
			array[8] = new TableDataGetter<BiomeDef>("forageFood", delegate(BiomeDef d)
			{
				if (d.foragedFood == null)
				{
					return "";
				}
				return d.foragedFood.label;
			});
			array[9] = new TableDataGetter<BiomeDef>("forageable plants", (BiomeDef d) => (from pd in d.AllWildPlants
			where pd.plant.harvestedThingDef != null && pd.plant.harvestedThingDef.IsNutritionGivingIngestible
			select pd.defName).ToCommaList(false, false));
			array[10] = new TableDataGetter<BiomeDef>("wildPlantRegrowDays", (BiomeDef d) => d.wildPlantRegrowDays.ToString("F0"));
			array[11] = new TableDataGetter<BiomeDef>("wildPlantsCareAboutLocalFertility", (BiomeDef d) => d.wildPlantsCareAboutLocalFertility.ToStringCheckBlank());
			DebugTables.MakeTablesDialog<BiomeDef>(dataSources, array);
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x0011C071 File Offset: 0x0011A271
		[DebugOutput]
		public static void BiomeAnimalsSpawnChances()
		{
			DebugOutputsEcology.BiomeAnimalsInternal(delegate(PawnKindDef k, BiomeDef b)
			{
				float num = b.CommonalityOfAnimal(k);
				if (num == 0f)
				{
					return "";
				}
				return (num / DefDatabase<PawnKindDef>.AllDefs.Sum((PawnKindDef ki) => b.CommonalityOfAnimal(ki))).ToStringPercent("F1");
			});
		}

		// Token: 0x06002E2E RID: 11822 RVA: 0x0011C097 File Offset: 0x0011A297
		[DebugOutput]
		public static void BiomeAnimalsTypicalCounts()
		{
			DebugOutputsEcology.BiomeAnimalsInternal((PawnKindDef k, BiomeDef b) => DebugOutputsEcology.ExpectedAnimalCount(k, b, null).ToStringEmptyZero("F2"));
		}

		// Token: 0x06002E2F RID: 11823 RVA: 0x0011C0C0 File Offset: 0x0011A2C0
		private static float ExpectedAnimalCount(PawnKindDef k, BiomeDef b, Map m)
		{
			float num = b.CommonalityOfAnimal(k);
			if (m != null && m.TileInfo.IsCoastal)
			{
				num += b.CommonalityOfCoastalAnimal(k);
			}
			if (num == 0f)
			{
				return 0f;
			}
			float num2 = DefDatabase<PawnKindDef>.AllDefs.Sum((PawnKindDef ki) => b.CommonalityOfAnimal(ki));
			float num3 = num / num2;
			float num4 = 10000f / b.animalDensity;
			float num5 = 62500f / num4;
			float totalCommonality = DefDatabase<PawnKindDef>.AllDefs.Sum((PawnKindDef ki) => b.CommonalityOfAnimal(ki));
			float num6 = DefDatabase<PawnKindDef>.AllDefs.Sum((PawnKindDef ki) => k.ecoSystemWeight * (b.CommonalityOfAnimal(ki) / totalCommonality));
			return num5 / num6 * num3;
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x0011C194 File Offset: 0x0011A394
		private static void BiomeAnimalsInternal(Func<PawnKindDef, BiomeDef, string> densityInBiomeOutputter)
		{
			List<TableDataGetter<PawnKindDef>> list = (from b in DefDatabase<BiomeDef>.AllDefs
			where b.implemented && b.canBuildBase
			orderby b.animalDensity
			select new TableDataGetter<PawnKindDef>(b.defName, (PawnKindDef k) => densityInBiomeOutputter(k, b))).ToList<TableDataGetter<PawnKindDef>>();
			list.Insert(0, new TableDataGetter<PawnKindDef>("animal", (PawnKindDef k) => k.defName + (k.race.race.predator ? " (P)" : "")));
			DebugTables.MakeTablesDialog<PawnKindDef>(from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Animal
			orderby d.defName
			select d, list.ToArray());
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x0011C29C File Offset: 0x0011A49C
		[DebugOutput]
		public static void BiomePlantsExpectedCount()
		{
			Func<ThingDef, BiomeDef, string> expectedCountInBiomeOutputter = (ThingDef p, BiomeDef b) => (b.CommonalityOfPlant(p) * b.plantDensity * 4000f).ToString("F0");
			List<TableDataGetter<ThingDef>> list = (from b in DefDatabase<BiomeDef>.AllDefs
			where b.implemented && b.canBuildBase
			orderby b.plantDensity
			select new TableDataGetter<ThingDef>(b.defName + " (" + b.plantDensity.ToString("F2") + ")", (ThingDef k) => expectedCountInBiomeOutputter(k, b))).ToList<TableDataGetter<ThingDef>>();
			list.Insert(0, new TableDataGetter<ThingDef>("plant", (ThingDef k) => k.defName));
			DebugTables.MakeTablesDialog<ThingDef>(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Plant
			orderby d.defName
			select d, list.ToArray());
		}

		// Token: 0x06002E32 RID: 11826 RVA: 0x0011C3C0 File Offset: 0x0011A5C0
		[DebugOutput]
		public static void AnimalWildCountsOnMap()
		{
			Map map = Find.CurrentMap;
			IEnumerable<PawnKindDef> dataSources = from k in DefDatabase<PawnKindDef>.AllDefs
			where k.race != null && k.RaceProps.Animal && DebugOutputsEcology.ExpectedAnimalCount(k, map.Biome, map) > 0f
			orderby DebugOutputsEcology.ExpectedAnimalCount(k, map.Biome, map) descending
			select k;
			TableDataGetter<PawnKindDef>[] array = new TableDataGetter<PawnKindDef>[3];
			array[0] = new TableDataGetter<PawnKindDef>("animal", (PawnKindDef k) => k.defName);
			array[1] = new TableDataGetter<PawnKindDef>("expected count on map (inaccurate)", (PawnKindDef k) => DebugOutputsEcology.ExpectedAnimalCount(k, map.Biome, map).ToString("F2"));
			array[2] = new TableDataGetter<PawnKindDef>("actual count on map", (PawnKindDef k) => map.mapPawns.AllPawnsSpawned.Count((Pawn p) => p.kindDef == k).ToString());
			DebugTables.MakeTablesDialog<PawnKindDef>(dataSources, array);
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x0011C470 File Offset: 0x0011A670
		[DebugOutput]
		public static void PlantCountsOnMap()
		{
			Map map = Find.CurrentMap;
			IEnumerable<ThingDef> dataSources = from p in DefDatabase<ThingDef>.AllDefs
			where p.category == ThingCategory.Plant && map.Biome.CommonalityOfPlant(p) > 0f
			orderby map.Biome.CommonalityOfPlant(p) descending
			select p;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
			array[0] = new TableDataGetter<ThingDef>("plant", (ThingDef p) => p.defName);
			array[1] = new TableDataGetter<ThingDef>("biome-defined commonality", (ThingDef p) => map.Biome.CommonalityOfPlant(p).ToString("F2"));
			array[2] = new TableDataGetter<ThingDef>("expected count (rough)", (ThingDef p) => (map.Biome.CommonalityOfPlant(p) * map.Biome.plantDensity * 4000f).ToString("F0"));
			array[3] = new TableDataGetter<ThingDef>("actual count on map", (ThingDef p) => map.AllCells.Count((IntVec3 c) => c.GetPlant(map) != null && c.GetPlant(map).def == p).ToString());
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x0011C537 File Offset: 0x0011A737
		[CompilerGenerated]
		internal static float <Plants>g__Nutrition|0_0(ThingDef d)
		{
			if (d.ingestible == null)
			{
				return 0f;
			}
			return d.GetStatValueAbstract(StatDefOf.Nutrition, null);
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x0011C553 File Offset: 0x0011A753
		[CompilerGenerated]
		internal static float <Plants>g__HarvestNutrition|0_1(ThingDef d)
		{
			if (d.plant.harvestedThingDef == null)
			{
				return 0f;
			}
			return d.plant.harvestYield * d.plant.harvestedThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null);
		}
	}
}
