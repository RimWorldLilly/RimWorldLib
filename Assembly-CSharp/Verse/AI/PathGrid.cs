using System;
using System.Collections.Generic;
using System.Text;
using LudeonTK;
using RimWorld;
using Unity.Collections;

namespace Verse.AI
{
	// Token: 0x020008A3 RID: 2211
	public class PathGrid : IDisposable
	{
		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x06004456 RID: 17494 RVA: 0x001AC396 File Offset: 0x001AA596
		public NativeArray<int> Grid_Unsafe
		{
			get
			{
				return this.pathGrid;
			}
		}

		// Token: 0x06004457 RID: 17495 RVA: 0x001AC39E File Offset: 0x001AA59E
		public PathGrid(Map map, PathGridDef def)
		{
			this.map = map;
			this.def = def;
			this.pathGrid = new NativeArray<int>(map.cellIndices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06004458 RID: 17496 RVA: 0x001AC3CC File Offset: 0x001AA5CC
		public bool Walkable(IntVec3 loc)
		{
			return loc.InBounds(this.map) && this.Cost(loc) < 10000;
		}

		// Token: 0x06004459 RID: 17497 RVA: 0x001AC3EC File Offset: 0x001AA5EC
		public bool WalkableFast(IntVec3 loc)
		{
			return this.Cost(loc) < 10000;
		}

		// Token: 0x0600445A RID: 17498 RVA: 0x001AC3FC File Offset: 0x001AA5FC
		public bool WalkableFast(int x, int z)
		{
			return this.pathGrid[this.map.cellIndices.CellToIndex(x, z)] < 10000;
		}

		// Token: 0x0600445B RID: 17499 RVA: 0x001AC422 File Offset: 0x001AA622
		public bool WalkableFast(int index)
		{
			return this.pathGrid[index] < 10000;
		}

		// Token: 0x0600445C RID: 17500 RVA: 0x001AC437 File Offset: 0x001AA637
		public int Cost(IntVec3 loc)
		{
			return this.pathGrid[this.map.cellIndices.CellToIndex(loc)];
		}

		// Token: 0x0600445D RID: 17501 RVA: 0x001AC458 File Offset: 0x001AA658
		private void RecalculatePerceivedPathCostAt(IntVec3 c)
		{
			bool flag = false;
			this.RecalculatePerceivedPathCostAt(c, ref flag);
		}

		// Token: 0x0600445E RID: 17502 RVA: 0x001AC470 File Offset: 0x001AA670
		public void RecalculatePerceivedPathCostAt(IntVec3 c, ref bool haveNotified)
		{
			if (!c.InBounds(this.map))
			{
				return;
			}
			if (this.dirty.IsCreated)
			{
				this.dirty.Set(this.map.cellIndices.CellToIndex(c), true);
				return;
			}
			if (!haveNotified)
			{
				if (this.RecalcuateCellDirect(c))
				{
					this.NotifyCellDirtied(c);
					haveNotified = true;
					return;
				}
			}
			else
			{
				this.pathGrid[this.map.cellIndices.CellToIndex(c)] = this.CalculatedCostAt(c, true, IntVec3.Invalid, null);
			}
		}

		// Token: 0x0600445F RID: 17503 RVA: 0x001AC504 File Offset: 0x001AA704
		private bool RecalcuateCellDirect(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				return false;
			}
			int index = this.map.cellIndices[c];
			bool flag = this.WalkableFast(index);
			this.pathGrid[index] = this.CalculatedCostAt(c, true, IntVec3.Invalid, null);
			return flag != this.WalkableFast(index);
		}

		// Token: 0x06004460 RID: 17504 RVA: 0x001AC568 File Offset: 0x001AA768
		private void NotifyCellDirtied(IntVec3 cell)
		{
			this.map.reachability.ClearCache();
			this.map.regionDirtyer.Notify_WalkabilityChanged(cell, this.WalkableFast(cell));
		}

		// Token: 0x06004461 RID: 17505 RVA: 0x001AC594 File Offset: 0x001AA794
		public void RecalculateAllPerceivedPathCosts()
		{
			foreach (IntVec3 c in this.map.AllCells)
			{
				this.RecalculatePerceivedPathCostAt(c);
			}
			if (this.dirty.IsCreated)
			{
				this.dirty.Clear();
			}
		}

		// Token: 0x06004462 RID: 17506 RVA: 0x001AC600 File Offset: 0x001AA800
		public virtual int CalculatedCostAt(IntVec3 c, bool perceivedStatic, IntVec3 prevCell, int? baseCostOverride = null)
		{
			int num = 0;
			bool flag = false;
			TerrainDef terrainDef = this.map.terrainGrid.TerrainAt(c);
			if (terrainDef == null || (terrainDef.passability == Traversability.Impassable && (!this.def.flying || !terrainDef.forcePassableByFlyingPawns)))
			{
				return 10000;
			}
			if (baseCostOverride != null)
			{
				num = baseCostOverride.Value;
			}
			else if (!this.def.flying)
			{
				num = terrainDef.pathCost;
			}
			List<Thing> list = this.map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def.passability == Traversability.Impassable && (!this.def.flying || !thing.def.forcePassableByFlyingPawns))
				{
					return 10000;
				}
				if (!this.def.fencePassable && thing.def.building != null && thing.def.building.isFence)
				{
					return 10000;
				}
				if (!PathGrid.IsPathCostIgnoreRepeater(thing.def) || !prevCell.IsValid || !this.ContainsPathCostIgnoreRepeater(prevCell))
				{
					int pathCost = thing.def.pathCost;
					if (pathCost > num)
					{
						num = pathCost;
					}
				}
				Building_Door building_Door = thing as Building_Door;
				if (building_Door != null && prevCell.IsValid)
				{
					Building_Door building_Door2 = prevCell.GetEdifice(this.map) as Building_Door;
					if (building_Door2 != null && !building_Door.FreePassage && !building_Door2.FreePassage)
					{
						flag = true;
					}
				}
			}
			int num2 = WeatherBuildupUtility.MovementTicksAddOn(this.map.snowGrid.GetCategory(c));
			if (num2 > num)
			{
				num = num2;
			}
			if (ModsConfig.OdysseyActive)
			{
				int num3 = WeatherBuildupUtility.MovementTicksAddOn(this.map.sandGrid.GetCategory(c));
				if (num3 > num)
				{
					num = num3;
				}
			}
			if (flag)
			{
				num += 45;
			}
			if (perceivedStatic)
			{
				for (int j = 0; j < 9; j++)
				{
					IntVec3 intVec = GenAdj.AdjacentCellsAndInside[j];
					IntVec3 c2 = c + intVec;
					if (c2.InBounds(this.map))
					{
						Fire fire = null;
						list = this.map.thingGrid.ThingsListAtFast(c2);
						for (int k = 0; k < list.Count; k++)
						{
							fire = (list[k] as Fire);
							if (fire != null)
							{
								break;
							}
						}
						if (fire != null && fire.parent == null)
						{
							if (intVec.x == 0 && intVec.z == 0)
							{
								num += 1000;
							}
							else
							{
								num += 150;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06004463 RID: 17507 RVA: 0x001AC888 File Offset: 0x001AAA88
		private bool ContainsPathCostIgnoreRepeater(IntVec3 c)
		{
			List<Thing> list = this.map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (PathGrid.IsPathCostIgnoreRepeater(list[i].def))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004464 RID: 17508 RVA: 0x001AC8CE File Offset: 0x001AAACE
		private static bool IsPathCostIgnoreRepeater(ThingDef def)
		{
			return def.pathCost >= 25 && def.pathCostIgnoreRepeat;
		}

		// Token: 0x06004465 RID: 17509 RVA: 0x001AC8E2 File Offset: 0x001AAAE2
		public void DisableIncrementalDirtying()
		{
			if (!this.dirty.IsCreated)
			{
				this.dirty = new NativeBitArray(this.map.cellIndices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}
		}

		// Token: 0x06004466 RID: 17510 RVA: 0x001AC914 File Offset: 0x001AAB14
		public void ReEnableIncrementalDirtying()
		{
			if (!this.dirty.IsCreated)
			{
				return;
			}
			for (int i = 0; i < this.dirty.Length; i++)
			{
				if (this.dirty.IsSet(i))
				{
					IntVec3 intVec = this.map.cellIndices[i];
					this.RecalcuateCellDirect(intVec);
					this.NotifyCellDirtied(intVec);
				}
			}
			this.dirty.Dispose();
		}

		// Token: 0x06004467 RID: 17511 RVA: 0x001AC97F File Offset: 0x001AAB7F
		public void Dispose()
		{
			ref this.pathGrid.EnsureDisposed<int>();
			ref this.dirty.EnsureDisposed();
		}

		// Token: 0x06004468 RID: 17512 RVA: 0x001AC998 File Offset: 0x001AAB98
		[DebugOutput]
		public static void ThingPathCostsIgnoreRepeaters()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("===============PATH COST IGNORE REPEATERS==============");
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (PathGrid.IsPathCostIgnoreRepeater(thingDef) && thingDef.passability != Traversability.Impassable)
				{
					stringBuilder.AppendLine(thingDef.defName + " " + thingDef.pathCost.ToString());
				}
			}
			stringBuilder.AppendLine("===============NON-PATH COST IGNORE REPEATERS that are buildings with >0 pathCost ==============");
			foreach (ThingDef thingDef2 in DefDatabase<ThingDef>.AllDefs)
			{
				if (!PathGrid.IsPathCostIgnoreRepeater(thingDef2) && thingDef2.passability != Traversability.Impassable && thingDef2.category == ThingCategory.Building && thingDef2.pathCost > 0)
				{
					stringBuilder.AppendLine(thingDef2.defName + " " + thingDef2.pathCost.ToString());
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04002C48 RID: 11336
		public readonly Map map;

		// Token: 0x04002C49 RID: 11337
		public readonly PathGridDef def;

		// Token: 0x04002C4A RID: 11338
		private NativeArray<int> pathGrid;

		// Token: 0x04002C4B RID: 11339
		private NativeBitArray dirty;

		// Token: 0x04002C4C RID: 11340
		public const int ImpassableCost = 10000;
	}
}
