using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200023B RID: 571
	public class PenFoodCalculator
	{
		// Token: 0x17000332 RID: 818
		// (get) Token: 0x060010A6 RID: 4262 RVA: 0x0005FC71 File Offset: 0x0005DE71
		public float NutritionPerDayToday
		{
			get
			{
				return this.sumNutritionPerDayToday;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x060010A7 RID: 4263 RVA: 0x0005FC79 File Offset: 0x0005DE79
		public List<PenFoodCalculator.PenAnimalInfo> ActualAnimalInfos
		{
			get
			{
				return this.animals;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x060010A8 RID: 4264 RVA: 0x0005FC81 File Offset: 0x0005DE81
		public List<PenFoodCalculator.PenFoodItemInfo> AllStockpiledInfos
		{
			get
			{
				return this.stockpiled;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x060010A9 RID: 4265 RVA: 0x0005FC89 File Offset: 0x0005DE89
		public bool Unenclosed
		{
			get
			{
				return this.numCells == 0;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x060010AA RID: 4266 RVA: 0x0005FC94 File Offset: 0x0005DE94
		public float SumNutritionConsumptionPerDay
		{
			get
			{
				float num = 0f;
				foreach (PenFoodCalculator.PenAnimalInfo penAnimalInfo in this.animals)
				{
					num += penAnimalInfo.TotalNutritionConsumptionPerDay;
				}
				return num;
			}
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x0005FCF0 File Offset: 0x0005DEF0
		public PenFoodCalculator.PenAnimalInfo GetAnimalInfo(ThingDef animalDef)
		{
			foreach (PenFoodCalculator.PenAnimalInfo penAnimalInfo in this.animals)
			{
				if (penAnimalInfo.animalDef == animalDef)
				{
					return penAnimalInfo;
				}
			}
			PenFoodCalculator.PenAnimalInfo penAnimalInfo2 = new PenFoodCalculator.PenAnimalInfo(animalDef);
			this.animals.Add(penAnimalInfo2);
			return penAnimalInfo2;
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x0005FD60 File Offset: 0x0005DF60
		public PenFoodCalculator.PenFoodItemInfo GetStockpiledInfo(ThingDef itemDef)
		{
			foreach (PenFoodCalculator.PenFoodItemInfo penFoodItemInfo in this.stockpiled)
			{
				if (penFoodItemInfo.itemDef == itemDef)
				{
					return penFoodItemInfo;
				}
			}
			PenFoodCalculator.PenFoodItemInfo penFoodItemInfo2 = new PenFoodCalculator.PenFoodItemInfo(itemDef);
			this.stockpiled.Add(penFoodItemInfo2);
			return penFoodItemInfo2;
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x0005FDD0 File Offset: 0x0005DFD0
		public string NaturalGrowthRateTooltip()
		{
			if (this.cachedNaturalGrowthRateTooltip == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("PenFoodTab_NaturalNutritionGrowthRateDescription".Translate());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("PenFoodTab_NaturalNutritionGrowthRateSeasonal".Translate());
				stringBuilder.AppendLine();
				stringBuilder.Append("PenFoodTab_GrowthPerSeason".Translate()).AppendLine(":");
				Vector2 vector = Find.WorldGrid.LongLatOf(this.mapCalc.map.Tile);
				for (int i = 0; i < 4; i++)
				{
					Quadrum quadrum = (Quadrum)i;
					stringBuilder.Append("- ").Append(quadrum.Label()).Append(" (").Append(quadrum.GetSeason(vector.y).Label()).Append("): ");
					stringBuilder.AppendLine(PenFoodCalculator.NutritionPerDayToString(this.nutritionPerDayPerQuadrum.ForQuadrum(quadrum), false));
				}
				this.cachedNaturalGrowthRateTooltip = stringBuilder.ToString();
			}
			return this.cachedNaturalGrowthRateTooltip;
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x0005FEE0 File Offset: 0x0005E0E0
		public string TotalConsumedToolTop()
		{
			if (this.cachedTotalConsumedTooltip == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("PenFoodTab_NutritionConsumptionDescription".Translate());
				this.cachedTotalConsumedTooltip = stringBuilder.ToString();
			}
			return this.cachedTotalConsumedTooltip;
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x0005FF24 File Offset: 0x0005E124
		public string StockpileToolTip()
		{
			if (this.cachedStockpileTooltip == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("PenFoodTab_StockpileTotalDescription".Translate());
				stringBuilder.AppendLine();
				foreach (PenFoodCalculator.PenFoodItemInfo penFoodItemInfo in this.stockpiled)
				{
					stringBuilder.Append("- ").Append(penFoodItemInfo.itemDef.LabelCap).Append(" x").Append(penFoodItemInfo.totalCount).Append(": ");
					stringBuilder.AppendLine(PenFoodCalculator.NutritionToString(penFoodItemInfo.totalNutritionAvailable, true));
				}
				this.cachedStockpileTooltip = stringBuilder.ToString();
			}
			return this.cachedStockpileTooltip;
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x00060004 File Offset: 0x0005E204
		private void Reset(Map map)
		{
			this.mapCalc.Reset(map);
			this.animals.Clear();
			this.stockpiled.Clear();
			this.cachedNaturalGrowthRateTooltip = null;
			this.cachedTotalConsumedTooltip = null;
			this.cachedStockpileTooltip = null;
			this.nutritionPerDayPerQuadrum = default(MapPastureNutritionCalculator.NutritionPerDayPerQuadrum);
			this.sumNutritionPerDayToday = 0f;
			this.sumStockpiledNutritionAvailableNow = 0f;
			this.numCells = 0;
			this.numCellsSoil = 0;
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x00060078 File Offset: 0x0005E278
		public List<PenFoodCalculator.PenAnimalInfo> ComputeExampleAnimals(List<ThingDef> animalDefs)
		{
			this.tmpAddedExampleAnimals.Clear();
			foreach (ThingDef thingDef in animalDefs)
			{
				LifeStageAge lifeStageAge = thingDef.race.lifeStageAges.Last<LifeStageAge>();
				PenFoodCalculator.PenAnimalInfo penAnimalInfo = new PenFoodCalculator.PenAnimalInfo(thingDef);
				penAnimalInfo.example = true;
				penAnimalInfo.nutritionConsumptionPerDay = SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(thingDef, lifeStageAge.def);
				this.tmpAddedExampleAnimals.Add(penAnimalInfo);
			}
			PenFoodCalculator.SortAnimals(this.tmpAddedExampleAnimals);
			return this.tmpAddedExampleAnimals;
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x00060118 File Offset: 0x0005E318
		public void ResetAndProcessPen(CompAnimalPenMarker marker)
		{
			this.ResetAndProcessPen(marker.parent.Position, marker.parent.Map, false);
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x00060137 File Offset: 0x0005E337
		public void ResetAndProcessPen(IntVec3 position, Map map, bool considerBlueprints)
		{
			this.Reset(map);
			if (map == null)
			{
				return;
			}
			if (considerBlueprints)
			{
				this.ProcessBlueprintPen(position, map);
			}
			else
			{
				this.ProcessRealPen(position, map);
			}
			this.SortResults(map);
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x00060160 File Offset: 0x0005E360
		private void SortResults(Map map)
		{
			this.stockpiled.Sort(new Comparison<PenFoodCalculator.PenFoodItemInfo>(PenFoodCalculator.<SortResults>g__FoodSorter|36_0));
			PenFoodCalculator.SortAnimals(this.animals);
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x00060184 File Offset: 0x0005E384
		private static void SortAnimals(List<PenFoodCalculator.PenAnimalInfo> infos)
		{
			infos.Sort(new Comparison<PenFoodCalculator.PenAnimalInfo>(PenFoodCalculator.<SortAnimals>g__AnimalSorter|37_0));
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x00060198 File Offset: 0x0005E398
		private void ProcessBlueprintPen(IntVec3 markerPos, Map map)
		{
			this.blueprintEnclosureCalc.VisitPen(markerPos, map);
			if (!this.blueprintEnclosureCalc.isEnclosed)
			{
				return;
			}
			foreach (IntVec3 c in this.blueprintEnclosureCalc.cellsFound)
			{
				this.ProcessCell(c, map);
			}
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x0006020C File Offset: 0x0005E40C
		private void ProcessRealPen(IntVec3 markerPos, Map map)
		{
			using (ProfilerBlock.Scope("PenFoodCalculator.ProcessRealPen"))
			{
				foreach (District district in this.connectedDistrictsCalc.CalculateConnectedDistricts(markerPos, map))
				{
					foreach (Region region in district.Regions)
					{
						this.ProcessRegion(region);
					}
				}
				this.connectedDistrictsCalc.Reset();
			}
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x000602D4 File Offset: 0x0005E4D4
		private void ProcessRegion(Region region)
		{
			using (ProfilerBlock.Scope("PenFoodCalculator.ProcessRegion"))
			{
				foreach (IntVec3 c in region.Cells)
				{
					this.ProcessCell(c, region.Map);
				}
			}
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x0006034C File Offset: 0x0005E54C
		private void ProcessCell(IntVec3 c, Map map)
		{
			using (ProfilerBlock.Scope("ProcessTerrain"))
			{
				this.ProcessTerrain(c, map);
			}
			foreach (Thing thing in c.GetThingList(map))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.IsAnimal)
				{
					this.ProcessAnimal(pawn);
				}
				else if (thing.def.category == ThingCategory.Item && thing.IngestibleNow && MapPlantGrowthRateCalculator.IsEdibleByPastureAnimals(thing.def))
				{
					this.ProcessStockpiledFood(thing);
				}
			}
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x00060410 File Offset: 0x0005E610
		private void ProcessTerrain(IntVec3 c, Map map)
		{
			this.numCells++;
			if (c.GetEdifice(map) != null)
			{
				return;
			}
			TerrainDef terrain = c.GetTerrain(map);
			if (terrain.IsSoil)
			{
				this.numCellsSoil++;
			}
			MapPastureNutritionCalculator.NutritionPerDayPerQuadrum other = this.mapCalc.CalculateAverageNutritionPerDay(terrain);
			this.nutritionPerDayPerQuadrum.AddFrom(other);
			this.sumNutritionPerDayToday += this.mapCalc.GetAverageNutritionPerDayToday(terrain);
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x00060488 File Offset: 0x0005E688
		private void ProcessStockpiledFood(Thing thing)
		{
			PenFoodCalculator.PenFoodItemInfo stockpiledInfo = this.GetStockpiledInfo(thing.def);
			float num = thing.GetStatValue(StatDefOf.Nutrition, true, -1) * (float)thing.stackCount;
			stockpiledInfo.totalCount += thing.stackCount;
			stockpiledInfo.totalNutritionAvailable += num;
			this.sumStockpiledNutritionAvailableNow += num;
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x000604E8 File Offset: 0x0005E6E8
		private void ProcessAnimal(Pawn animal)
		{
			if (!MapPlantGrowthRateCalculator.IsPastureAnimal(animal.def) || !animal.Spawned)
			{
				return;
			}
			PenFoodCalculator.PenAnimalInfo animalInfo = this.GetAnimalInfo(animal.def);
			animalInfo.count++;
			animalInfo.nutritionConsumptionPerDay += SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(animal);
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x00060538 File Offset: 0x0005E738
		public static string NutritionToString(float value, bool withUnits = true)
		{
			string text = value.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute);
			if (withUnits)
			{
				return text + " " + "PenFoodTab_Nutrition_Unit".Translate();
			}
			return text;
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x00060574 File Offset: 0x0005E774
		public static string NutritionPerDayToString(float value, bool withUnits = true)
		{
			string text = value.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute);
			if (withUnits)
			{
				return text + " " + "PenFoodTab_NutritionPerDay_Unit".Translate();
			}
			return text;
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x000605AE File Offset: 0x0005E7AE
		public float CapacityOf(Quadrum q, ThingDef animal)
		{
			return this.nutritionPerDayPerQuadrum.ForQuadrum(q) / SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(animal);
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x000605C4 File Offset: 0x0005E7C4
		public Quadrum GetSummerOrBestQuadrum()
		{
			Vector2 location = Find.WorldGrid.LongLatOf(this.mapCalc.map.Tile);
			Quadrum? quadrum = null;
			float num = 0f;
			foreach (Quadrum quadrum2 in QuadrumUtility.Quadrums)
			{
				if (quadrum2.GetSeason(location) == Season.Summer)
				{
					return quadrum2;
				}
				float num2 = this.nutritionPerDayPerQuadrum.ForQuadrum(quadrum2);
				if (quadrum == null || num2 > num)
				{
					quadrum = new Quadrum?(quadrum2);
					num = num2;
				}
			}
			return quadrum.Value;
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x0006067C File Offset: 0x0005E87C
		public string PenSizeDescription()
		{
			string result;
			if (this.Unenclosed)
			{
				result = "PenSizeDesc_Unenclosed".Translate();
			}
			else if (this.numCellsSoil < 50)
			{
				result = "PenSizeDesc_VerySmall".Translate();
			}
			else if (this.numCellsSoil < 100)
			{
				result = "PenSizeDesc_Small".Translate();
			}
			else if (this.numCellsSoil < 400)
			{
				result = "PenSizeDesc_Medium".Translate();
			}
			else
			{
				result = "PenSizeDesc_Large".Translate();
			}
			return result;
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x00060761 File Offset: 0x0005E961
		[CompilerGenerated]
		internal static int <SortResults>g__FoodSorter|36_0(PenFoodCalculator.PenFoodItemInfo a, PenFoodCalculator.PenFoodItemInfo b)
		{
			return -1 * a.totalNutritionAvailable.CompareTo(b.totalNutritionAvailable);
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x00060778 File Offset: 0x0005E978
		[CompilerGenerated]
		internal static int <SortAnimals>g__AnimalSorter|37_0(PenFoodCalculator.PenAnimalInfo a, PenFoodCalculator.PenAnimalInfo b)
		{
			return -1 * a.TotalNutritionConsumptionPerDay.CompareTo(b.TotalNutritionConsumptionPerDay);
		}

		// Token: 0x04000F6D RID: 3949
		public const ToStringStyle NutritionStringStyle = ToStringStyle.FloatMaxTwo;

		// Token: 0x04000F6E RID: 3950
		private readonly AnimalPenConnectedDistrictsCalculator connectedDistrictsCalc = new AnimalPenConnectedDistrictsCalculator();

		// Token: 0x04000F6F RID: 3951
		private readonly AnimalPenBlueprintEnclosureCalculator blueprintEnclosureCalc = new AnimalPenBlueprintEnclosureCalculator();

		// Token: 0x04000F70 RID: 3952
		private readonly MapPastureNutritionCalculator mapCalc = new MapPastureNutritionCalculator();

		// Token: 0x04000F71 RID: 3953
		private readonly List<PenFoodCalculator.PenAnimalInfo> animals = new List<PenFoodCalculator.PenAnimalInfo>();

		// Token: 0x04000F72 RID: 3954
		private readonly List<PenFoodCalculator.PenFoodItemInfo> stockpiled = new List<PenFoodCalculator.PenFoodItemInfo>();

		// Token: 0x04000F73 RID: 3955
		private string cachedNaturalGrowthRateTooltip;

		// Token: 0x04000F74 RID: 3956
		private string cachedTotalConsumedTooltip;

		// Token: 0x04000F75 RID: 3957
		private string cachedStockpileTooltip;

		// Token: 0x04000F76 RID: 3958
		public MapPastureNutritionCalculator.NutritionPerDayPerQuadrum nutritionPerDayPerQuadrum;

		// Token: 0x04000F77 RID: 3959
		public float sumStockpiledNutritionAvailableNow;

		// Token: 0x04000F78 RID: 3960
		public int numCells;

		// Token: 0x04000F79 RID: 3961
		public int numCellsSoil;

		// Token: 0x04000F7A RID: 3962
		private float sumNutritionPerDayToday;

		// Token: 0x04000F7B RID: 3963
		private readonly List<PenFoodCalculator.PenAnimalInfo> tmpAddedExampleAnimals = new List<PenFoodCalculator.PenAnimalInfo>();

		// Token: 0x020025BA RID: 9658
		public class PenAnimalInfo
		{
			// Token: 0x17002A74 RID: 10868
			// (get) Token: 0x0600F0A1 RID: 61601 RVA: 0x00588816 File Offset: 0x00586A16
			public int TotalCount
			{
				get
				{
					return this.count;
				}
			}

			// Token: 0x17002A75 RID: 10869
			// (get) Token: 0x0600F0A2 RID: 61602 RVA: 0x0058881E File Offset: 0x00586A1E
			public float TotalNutritionConsumptionPerDay
			{
				get
				{
					return this.nutritionConsumptionPerDay;
				}
			}

			// Token: 0x0600F0A3 RID: 61603 RVA: 0x00588826 File Offset: 0x00586A26
			public PenAnimalInfo(ThingDef animalDef)
			{
				this.animalDef = animalDef;
			}

			// Token: 0x0600F0A4 RID: 61604 RVA: 0x00588838 File Offset: 0x00586A38
			public string ToolTip(PenFoodCalculator calc)
			{
				if (this.cachedToolTip == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					int value = Mathf.FloorToInt(calc.NutritionPerDayToday / SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(this.animalDef));
					stringBuilder.Append("PenFoodTab_AnimalTypeAnimalCapacity".Translate()).Append(": ").Append(value).AppendLine();
					stringBuilder.AppendLine();
					stringBuilder.Append("PenFoodTab_NutritionConsumedPerDay".Translate(this.animalDef.Named("ANIMALDEF"))).AppendLine(":");
					List<LifeStageAge> lifeStageAges = this.animalDef.race.lifeStageAges;
					for (int i = 0; i < lifeStageAges.Count; i++)
					{
						LifeStageDef def = lifeStageAges[i].def;
						float value2 = SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(this.animalDef, def);
						stringBuilder.Append("- ").Append(def.LabelCap).Append(": ").AppendLine(PenFoodCalculator.NutritionPerDayToString(value2, false));
					}
					this.cachedToolTip = stringBuilder.ToString();
				}
				return this.cachedToolTip;
			}

			// Token: 0x04009AB0 RID: 39600
			public ThingDef animalDef;

			// Token: 0x04009AB1 RID: 39601
			public bool example;

			// Token: 0x04009AB2 RID: 39602
			public int count;

			// Token: 0x04009AB3 RID: 39603
			public float nutritionConsumptionPerDay;

			// Token: 0x04009AB4 RID: 39604
			private string cachedToolTip;
		}

		// Token: 0x020025BB RID: 9659
		public class PenFoodItemInfo
		{
			// Token: 0x0600F0A5 RID: 61605 RVA: 0x00588955 File Offset: 0x00586B55
			public PenFoodItemInfo(ThingDef itemDef)
			{
				this.itemDef = itemDef;
			}

			// Token: 0x04009AB5 RID: 39605
			public ThingDef itemDef;

			// Token: 0x04009AB6 RID: 39606
			public int totalCount;

			// Token: 0x04009AB7 RID: 39607
			public float totalNutritionAvailable;
		}
	}
}
