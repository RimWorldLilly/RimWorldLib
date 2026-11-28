using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013C5 RID: 5061
	public class Zone_Growing : Zone, IPlantToGrowSettable
	{
		// Token: 0x17001572 RID: 5490
		// (get) Token: 0x06007D7E RID: 32126 RVA: 0x002CE058 File Offset: 0x002CC258
		public override bool IsMultiselectable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001573 RID: 5491
		// (get) Token: 0x06007D7F RID: 32127 RVA: 0x002CE05B File Offset: 0x002CC25B
		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextGrowingZoneColor();
			}
		}

		// Token: 0x17001574 RID: 5492
		// (get) Token: 0x06007D80 RID: 32128 RVA: 0x002CE062 File Offset: 0x002CC262
		IEnumerable<IntVec3> IPlantToGrowSettable.Cells
		{
			get
			{
				return base.Cells;
			}
		}

		// Token: 0x17001575 RID: 5493
		// (get) Token: 0x06007D81 RID: 32129 RVA: 0x002CE06A File Offset: 0x002CC26A
		public ThingDef PlantDefToGrow
		{
			get
			{
				if (this.plantDefToGrow != null)
				{
					return this.plantDefToGrow;
				}
				if (PollutionUtility.SettableEntirelyPolluted(this))
				{
					this.plantDefToGrow = ThingDefOf.Plant_Toxipotato;
				}
				else
				{
					this.plantDefToGrow = ThingDefOf.Plant_Potato;
				}
				return this.plantDefToGrow;
			}
		}

		// Token: 0x06007D82 RID: 32130 RVA: 0x002CE0A1 File Offset: 0x002CC2A1
		public Zone_Growing()
		{
		}

		// Token: 0x06007D83 RID: 32131 RVA: 0x002CE0B7 File Offset: 0x002CC2B7
		public Zone_Growing(ZoneManager zoneManager) : base("GrowingZone".Translate(), zoneManager)
		{
		}

		// Token: 0x06007D84 RID: 32132 RVA: 0x002CE0DD File Offset: 0x002CC2DD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToGrow, "plantDefToGrow");
			Scribe_Values.Look<bool>(ref this.allowSow, "allowSow", true, false);
			Scribe_Values.Look<bool>(ref this.allowCut, "allowCut", true, false);
		}

		// Token: 0x06007D85 RID: 32133 RVA: 0x002CE11C File Offset: 0x002CC31C
		public void ContentsStatistics(out int totalPlants, out float averagePlantAgeTicks, out int oldestPlantAgeTicks, out float averagePlantGrowth, out float maxPlantGrowth)
		{
			averagePlantAgeTicks = 0f;
			totalPlants = 0;
			oldestPlantAgeTicks = 0;
			averagePlantGrowth = 0f;
			maxPlantGrowth = 0f;
			foreach (IntVec3 c in base.Cells)
			{
				foreach (Thing thing in c.GetThingList(base.Map))
				{
					if (thing.def == this.plantDefToGrow)
					{
						Plant plant = thing as Plant;
						if (plant != null)
						{
							totalPlants++;
							averagePlantAgeTicks += (float)plant.Age;
							oldestPlantAgeTicks = Mathf.Max(oldestPlantAgeTicks, plant.Age);
							averagePlantGrowth += plant.Growth;
							maxPlantGrowth = Mathf.Max(maxPlantGrowth, plant.Growth);
						}
					}
				}
			}
			averagePlantGrowth /= (float)totalPlants;
			averagePlantAgeTicks /= (float)totalPlants;
		}

		// Token: 0x06007D86 RID: 32134 RVA: 0x002CE234 File Offset: 0x002CC434
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.GetInspectString());
			stringBuilder.AppendLine();
			if (!base.Cells.NullOrEmpty<IntVec3>())
			{
				int num;
				float f;
				int numTicks;
				float f2;
				float f3;
				this.ContentsStatistics(out num, out f, out numTicks, out f2, out f3);
				if (num > 0)
				{
					string value = Mathf.RoundToInt(f).ToStringTicksToPeriodVerbose(true, true);
					string value2 = numTicks.ToStringTicksToPeriodVerbose(true, true);
					StringBuilder stringBuilder2 = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 3);
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("Contains".Translate().CapitalizeFirst());
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendFormatted(Find.ActiveLanguageWorker.Pluralize(this.plantDefToGrow.label, num));
					stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
					StringBuilder stringBuilder3 = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(5, 3);
					defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("AveragePlantAge".Translate().CapitalizeFirst());
					defaultInterpolatedStringHandler2.AppendLiteral(": ");
					defaultInterpolatedStringHandler2.AppendFormatted(value);
					defaultInterpolatedStringHandler2.AppendLiteral(" (");
					defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("PercentGrowth".Translate(f2.ToStringPercent()));
					defaultInterpolatedStringHandler2.AppendLiteral(")");
					stringBuilder3.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
					StringBuilder stringBuilder4 = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(5, 3);
					defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("OldestPlantAge".Translate().CapitalizeFirst());
					defaultInterpolatedStringHandler3.AppendLiteral(": ");
					defaultInterpolatedStringHandler3.AppendFormatted(value2);
					defaultInterpolatedStringHandler3.AppendLiteral(" (");
					defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("PercentGrowth".Translate(f3.ToStringPercent()));
					defaultInterpolatedStringHandler3.AppendLiteral(")");
					stringBuilder4.AppendLine(defaultInterpolatedStringHandler3.ToStringAndClear());
				}
				IntVec3 c = base.Cells.First<IntVec3>();
				if (c.UsesOutdoorTemperature(base.Map))
				{
					StringBuilder stringBuilder5 = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler4.AppendFormatted<TaggedString>("OutdoorGrowingPeriod".Translate());
					defaultInterpolatedStringHandler4.AppendLiteral(": ");
					defaultInterpolatedStringHandler4.AppendFormatted(Zone_Growing.GrowingQuadrumsDescription(base.Map, this.plantDefToGrow));
					stringBuilder5.AppendLine(defaultInterpolatedStringHandler4.ToStringAndClear());
				}
				if (PlantUtility.GrowthSeasonNow(c, base.Map, this.plantDefToGrow))
				{
					stringBuilder.Append("GrowSeasonHereNow".Translate());
				}
				else
				{
					stringBuilder.Append("CannotGrowBadSeasonTemperature".Translate());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06007D87 RID: 32135 RVA: 0x002CE4A4 File Offset: 0x002CC6A4
		public static string GrowingQuadrumsDescription(PlanetTile tile, ThingDef plantDef = null)
		{
			if (!tile.Valid)
			{
				return "NoGrowingPeriod".Translate();
			}
			float minTemp = (plantDef != null) ? plantDef.plant.minOptimalGrowthTemperature : 6f;
			float maxTemp = (plantDef != null) ? plantDef.plant.maxOptimalGrowthTemperature : 42f;
			List<Twelfth> list = GenTemperature.TwelfthsInAverageTemperatureRange(tile, minTemp, maxTemp);
			if (list.NullOrEmpty<Twelfth>())
			{
				return "NoGrowingPeriod".Translate();
			}
			if (list.Count == 12)
			{
				return "GrowYearRound".Translate();
			}
			string key = "PeriodDays";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<int>(list.Count * 5);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(60);
			return key.Translate(defaultInterpolatedStringHandler.ToStringAndClear()) + (" (" + QuadrumUtility.QuadrumsRangeLabel(list) + ")");
		}

		// Token: 0x06007D88 RID: 32136 RVA: 0x002CE594 File Offset: 0x002CC794
		public static string GrowingQuadrumsDescription(Map map, ThingDef plantDef = null)
		{
			if (map == null)
			{
				return "NoGrowingPeriod".Translate();
			}
			if (!map.IsPocketMap)
			{
				return Zone_Growing.GrowingQuadrumsDescription(map.Tile, plantDef);
			}
			float num = (plantDef != null) ? plantDef.plant.minOptimalGrowthTemperature : 6f;
			float num2 = (plantDef != null) ? plantDef.plant.maxOptimalGrowthTemperature : 42f;
			float outdoorTemp = map.mapTemperature.OutdoorTemp;
			if (outdoorTemp >= num && outdoorTemp <= num2)
			{
				return "GrowYearRound".Translate();
			}
			return "NoGrowingPeriod".Translate();
		}

		// Token: 0x06007D89 RID: 32137 RVA: 0x002CE628 File Offset: 0x002CC828
		public override void AddCell(IntVec3 c)
		{
			base.AddCell(c);
			foreach (Thing thing in base.Map.thingGrid.ThingsListAt(c))
			{
				CompPlantPreventCutting compPlantPreventCutting;
				if (!thing.TryGetComp(out compPlantPreventCutting) || !compPlantPreventCutting.PreventCutting)
				{
					Designator_PlantsHarvestWood.PossiblyWarnPlayerImportantPlantDesignateCut(thing);
				}
			}
		}

		// Token: 0x06007D8A RID: 32138 RVA: 0x002CE6A0 File Offset: 0x002CC8A0
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Zone_Growing.<GetGizmos>d__19 <GetGizmos>d__ = new Zone_Growing.<GetGizmos>d__19(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06007D8B RID: 32139 RVA: 0x002CE6B0 File Offset: 0x002CC8B0
		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			return new Zone_Growing.<GetZoneAddGizmos>d__20(-2);
		}

		// Token: 0x06007D8C RID: 32140 RVA: 0x002CE6B9 File Offset: 0x002CC8B9
		public ThingDef GetPlantDefToGrow()
		{
			return this.PlantDefToGrow;
		}

		// Token: 0x06007D8D RID: 32141 RVA: 0x002CE6C1 File Offset: 0x002CC8C1
		public void SetPlantDefToGrow(ThingDef plantDef)
		{
			this.plantDefToGrow = plantDef;
		}

		// Token: 0x06007D8E RID: 32142 RVA: 0x002CE6CA File Offset: 0x002CC8CA
		public bool CanAcceptSowNow()
		{
			return true;
		}

		// Token: 0x04004F0E RID: 20238
		private ThingDef plantDefToGrow;

		// Token: 0x04004F0F RID: 20239
		public bool allowSow = true;

		// Token: 0x04004F10 RID: 20240
		public bool allowCut = true;
	}
}
