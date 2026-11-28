using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000307 RID: 775
	public sealed class RoomTempTracker
	{
		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x060017CE RID: 6094 RVA: 0x0008E9EB File Offset: 0x0008CBEB
		private Map Map
		{
			get
			{
				return this.room.Map;
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x060017CF RID: 6095 RVA: 0x0008E9F8 File Offset: 0x0008CBF8
		private float ThinRoofCoverage
		{
			get
			{
				return 1f - (this.thickRoofCoverage + this.noRoofCoverage);
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x060017D0 RID: 6096 RVA: 0x0008EA0D File Offset: 0x0008CC0D
		public List<IntVec3> EqualizeCellsForReading
		{
			get
			{
				return this.equalizeCells;
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x060017D1 RID: 6097 RVA: 0x0008EA15 File Offset: 0x0008CC15
		// (set) Token: 0x060017D2 RID: 6098 RVA: 0x0008EA1D File Offset: 0x0008CC1D
		public float Temperature
		{
			get
			{
				return this.temperatureInt;
			}
			set
			{
				this.temperatureInt = Mathf.Clamp(value, -273.15f, 1000f);
			}
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x0008EA35 File Offset: 0x0008CC35
		public RoomTempTracker(Room room, Map map)
		{
			this.room = room;
			this.Temperature = map.mapTemperature.OutdoorTemp;
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x0008EA60 File Offset: 0x0008CC60
		public void RoofChanged()
		{
			this.RegenerateEqualizationData();
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x0008EA68 File Offset: 0x0008CC68
		public void RoomChanged()
		{
			if (this.Map != null && !GravshipPlacementUtility.placingGravship)
			{
				this.Map.autoBuildRoofAreaSetter.ResolveQueuedGenerateRoofs();
			}
			this.RegenerateEqualizationData();
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x0008EA90 File Offset: 0x0008CC90
		private void RegenerateEqualizationData()
		{
			this.thickRoofCoverage = 0f;
			this.noRoofCoverage = 0f;
			this.equalizeCells.Clear();
			if (this.room.DistrictCount == 0)
			{
				return;
			}
			Map map = this.Map;
			if (this.room.UsesOutdoorTemperature)
			{
				return;
			}
			this.CalculateRoofCovereage(map);
			this.RegenerateEqualizeCells(map);
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x0008EAF0 File Offset: 0x0008CCF0
		private void CalculateRoofCovereage(Map map)
		{
			int num = 0;
			foreach (IntVec3 c in this.room.Cells)
			{
				RoofDef roof = c.GetRoof(map);
				if (roof == null)
				{
					this.noRoofCoverage += 1f;
				}
				else if (roof.isThickRoof)
				{
					this.thickRoofCoverage += 1f;
				}
				num++;
			}
			this.thickRoofCoverage /= (float)num;
			this.noRoofCoverage /= (float)num;
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x0008EB98 File Offset: 0x0008CD98
		private void RegenerateEqualizeCells(Map map)
		{
			foreach (IntVec3 a in this.room.Cells)
			{
				int i = 0;
				while (i < 4)
				{
					IntVec3 intVec = a + GenAdj.CardinalDirections[i];
					IntVec3 intVec2 = a + GenAdj.CardinalDirections[i] * 2;
					if (!intVec.InBounds(map))
					{
						goto IL_100;
					}
					Region region = intVec.GetRegion(map, RegionType.Set_Passable);
					if (region == null)
					{
						goto IL_100;
					}
					if (region.type == RegionType.Portal)
					{
						bool flag = false;
						for (int j = 0; j < region.links.Count; j++)
						{
							Region regionA = region.links[j].RegionA;
							Region regionB = region.links[j].RegionB;
							if (regionA.Room != this.room && !regionA.IsDoorway)
							{
								flag = true;
								break;
							}
							if (regionB.Room != this.room && !regionB.IsDoorway)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							goto IL_100;
						}
					}
					IL_164:
					i++;
					continue;
					IL_100:
					if (!intVec2.InBounds(map) || intVec2.GetRoom(map) == this.room)
					{
						goto IL_164;
					}
					bool flag2 = false;
					for (int k = 0; k < 4; k++)
					{
						if ((intVec2 + GenAdj.CardinalDirections[k]).GetRoom(map) == this.room)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						this.equalizeCells.Add(intVec2);
						goto IL_164;
					}
					goto IL_164;
				}
			}
			this.equalizeCells.Shuffle<IntVec3>();
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x0008ED54 File Offset: 0x0008CF54
		public void EqualizeTemperature()
		{
			if (this.room.UsesOutdoorTemperature)
			{
				this.Temperature = this.Map.mapTemperature.OutdoorTemp;
				return;
			}
			if (this.room.IsDoorway)
			{
				bool flag = true;
				IntVec3 anyCell = this.room.Districts[0].Regions[0].AnyCell;
				for (int i = 0; i < 4; i++)
				{
					IntVec3 intVec = anyCell + GenAdj.CardinalDirections[i];
					if (intVec.InBounds(this.Map))
					{
						Room room = intVec.GetRoom(this.Map);
						if (room != null && !room.IsDoorway)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					this.room.Temperature += this.WallEqualizationTempChangePerInterval();
				}
				return;
			}
			float num = this.ThinRoofEqualizationTempChangePerInterval();
			float num2 = this.NoRoofEqualizationTempChangePerInterval();
			float num3 = this.WallEqualizationTempChangePerInterval();
			float num4 = this.DeepEqualizationTempChangePerInterval();
			this.Temperature += num + num2 + num3 + num4;
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x0008EE60 File Offset: 0x0008D060
		private float WallEqualizationTempChangePerInterval()
		{
			if (this.equalizeCells.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			int num2 = Mathf.CeilToInt((float)this.equalizeCells.Count * 0.2f);
			for (int i = 0; i < num2; i++)
			{
				this.cycleIndex++;
				int index = this.cycleIndex % this.equalizeCells.Count;
				float num3;
				if (GenTemperature.TryGetDirectAirTemperatureForCell(this.equalizeCells[index], this.Map, out num3))
				{
					if (this.Map.Biome.inVacuum)
					{
						float num4 = num3 - this.Temperature;
						num += num4 * 5E-05f;
					}
					else
					{
						num += num3 - this.Temperature;
					}
				}
				else
				{
					float outdoorTemp = this.Map.mapTemperature.OutdoorTemp;
					if (this.Map.Biome.inVacuum)
					{
						float num5 = outdoorTemp - this.Temperature;
						num += num5 * 5E-05f;
					}
					else
					{
						num += Mathf.Lerp(this.Temperature, outdoorTemp, 0.5f) - this.Temperature;
					}
				}
			}
			return num / (float)num2 * (float)this.equalizeCells.Count * 120f * 0.00017f / (float)this.room.CellCount;
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x0008EFA8 File Offset: 0x0008D1A8
		private float TempDiffFromOutdoorsAdjusted()
		{
			float num = this.Map.mapTemperature.OutdoorTemp - this.temperatureInt;
			if (Mathf.Abs(num) < 100f)
			{
				return num;
			}
			return Mathf.Sign(num) * 100f + 5f * (num - Mathf.Sign(num) * 100f);
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x0008F000 File Offset: 0x0008D200
		private float ThinRoofEqualizationTempChangePerInterval()
		{
			if (this.ThinRoofCoverage < 0.001f)
			{
				return 0f;
			}
			float num = this.TempDiffFromOutdoorsAdjusted();
			float num2 = this.Map.Biome.inVacuum ? 5E-05f : 5E-05f;
			return num * this.ThinRoofCoverage * num2 * 120f;
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x0008F054 File Offset: 0x0008D254
		private float NoRoofEqualizationTempChangePerInterval()
		{
			if (this.noRoofCoverage < 0.001f)
			{
				return 0f;
			}
			return this.TempDiffFromOutdoorsAdjusted() * this.noRoofCoverage * 0.0007f * 120f;
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x0008F084 File Offset: 0x0008D284
		private float DeepEqualizationTempChangePerInterval()
		{
			if (this.thickRoofCoverage < 0.001f)
			{
				return 0f;
			}
			float num = 15f - this.temperatureInt;
			if (num > 0f)
			{
				return 0f;
			}
			float num2 = (this.Map.generatorDef != null && this.Map.generatorDef.isUnderground) ? 0.002f : 5E-05f;
			return num * this.thickRoofCoverage * num2 * 120f;
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x0008F0FC File Offset: 0x0008D2FC
		public void DebugDraw()
		{
			foreach (IntVec3 c in this.equalizeCells)
			{
				CellRenderer.RenderCell(c, 0.5f);
			}
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x0008F154 File Offset: 0x0008D354
		internal string DebugString()
		{
			if (this.room.UsesOutdoorTemperature)
			{
				return "uses outdoor temperature";
			}
			if (Time.frameCount > RoomTempTracker.debugGetFrame + 120)
			{
				RoomTempTracker.debugWallEq = 0f;
				for (int i = 0; i < 40; i++)
				{
					RoomTempTracker.debugWallEq += this.WallEqualizationTempChangePerInterval();
				}
				RoomTempTracker.debugWallEq /= 40f;
				RoomTempTracker.debugGetFrame = Time.frameCount;
			}
			return string.Concat(new string[]
			{
				"  thick roof coverage: ",
				this.thickRoofCoverage.ToStringPercent("F0"),
				"\n  thin roof coverage: ",
				this.ThinRoofCoverage.ToStringPercent("F0"),
				"\n  no roof coverage: ",
				this.noRoofCoverage.ToStringPercent("F0"),
				"\n\n  wall equalization: ",
				RoomTempTracker.debugWallEq.ToStringTemperatureOffset("F3"),
				"\n  thin roof equalization: ",
				this.ThinRoofEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
				"\n  no roof equalization: ",
				this.NoRoofEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
				"\n  deep equalization: ",
				this.DeepEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
				"\n\n  temp diff from outdoors, adjusted: ",
				this.TempDiffFromOutdoorsAdjusted().ToStringTemperatureOffset("F3"),
				"\n  tempChange e=20 targ= 200C: ",
				GenTemperature.ControlTemperatureTempChange(this.room.Cells.First<IntVec3>(), this.room.Map, 20f, 200f).ToString(),
				"\n  tempChange e=20 targ=-200C: ",
				GenTemperature.ControlTemperatureTempChange(this.room.Cells.First<IntVec3>(), this.room.Map, 20f, -200f).ToString(),
				"\n  equalize interval ticks: ",
				120.ToString(),
				"\n  equalize cells count:",
				this.equalizeCells.Count.ToString()
			});
		}

		// Token: 0x04001411 RID: 5137
		private Room room;

		// Token: 0x04001412 RID: 5138
		private float temperatureInt;

		// Token: 0x04001413 RID: 5139
		private List<IntVec3> equalizeCells = new List<IntVec3>();

		// Token: 0x04001414 RID: 5140
		private float noRoofCoverage;

		// Token: 0x04001415 RID: 5141
		private float thickRoofCoverage;

		// Token: 0x04001416 RID: 5142
		public const float FractionWallEqualizeCells = 0.2f;

		// Token: 0x04001417 RID: 5143
		public const float WallEqualizeFactor = 0.00017f;

		// Token: 0x04001418 RID: 5144
		public const float EqualizationPowerOfFilledCells = 0.5f;

		// Token: 0x04001419 RID: 5145
		private int cycleIndex;

		// Token: 0x0400141A RID: 5146
		private const float ThinRoofEqualizeRate = 5E-05f;

		// Token: 0x0400141B RID: 5147
		private const float NoRoofEqualizeRate = 0.0007f;

		// Token: 0x0400141C RID: 5148
		private const float DeepEqualizeFractionPerTick = 5E-05f;

		// Token: 0x0400141D RID: 5149
		private const float UndergroundEqualizeFractionPerTick = 0.002f;

		// Token: 0x0400141E RID: 5150
		private const float VacuumEqualizeFractionPerTick = 5E-05f;

		// Token: 0x0400141F RID: 5151
		private static int debugGetFrame = -999;

		// Token: 0x04001420 RID: 5152
		private static float debugWallEq;
	}
}
