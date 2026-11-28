using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x020008AC RID: 2220
	public class RegionCostCalculatorWrapper
	{
		// Token: 0x060044D8 RID: 17624 RVA: 0x001AFF85 File Offset: 0x001AE185
		public RegionCostCalculatorWrapper(Map map)
		{
			this.map = map;
			this.regionCostCalculator = new RegionCostCalculator(map);
		}

		// Token: 0x060044D9 RID: 17625 RVA: 0x001AFFAC File Offset: 0x001AE1AC
		public void Init(CellRect end, TraverseParms traverseParms, float moveTicksCardinal, float moveTicksDiagonal, ByteGrid avoidGrid, Area allowedArea, bool drafted, List<int> disallowedCorners)
		{
			this.moveTicksCardinal = moveTicksCardinal;
			this.moveTicksDiagonal = moveTicksDiagonal;
			this.endCell = end.CenterCell;
			this.cachedRegion = null;
			this.cachedBestLink = null;
			this.cachedSecondBestLink = null;
			this.cachedBestLinkCost = 0;
			this.cachedSecondBestLinkCost = 0;
			this.cachedRegionIsDestination = false;
			this.regionGrid = this.map.regionGrid.DirectGrid;
			this.destRegions.Clear();
			if (end.Width == 1 && end.Height == 1)
			{
				Region region = this.endCell.GetRegion(this.map, RegionType.Set_Passable);
				if (region != null)
				{
					this.destRegions.Add(region);
				}
			}
			else
			{
				foreach (IntVec3 intVec in end)
				{
					if (intVec.InBounds(this.map) && !disallowedCorners.Contains(this.map.cellIndices.CellToIndex(intVec)))
					{
						Region region2 = intVec.GetRegion(this.map, RegionType.Set_Passable);
						if (region2 != null && region2.Allows(traverseParms, true))
						{
							this.destRegions.Add(region2);
						}
					}
				}
			}
			if (this.destRegions.Count == 0)
			{
				Log.Error("Couldn't find any destination regions. This shouldn't ever happen because we've checked reachability.");
			}
			this.regionCostCalculator.Init(end, this.destRegions, traverseParms, moveTicksCardinal, moveTicksDiagonal, avoidGrid, allowedArea, drafted);
		}

		// Token: 0x060044DA RID: 17626 RVA: 0x001B011C File Offset: 0x001AE31C
		public int GetPathCostFromDestToRegion(int cellIndex)
		{
			Region region = this.regionGrid[cellIndex];
			IntVec3 cell = this.map.cellIndices.IndexToCell(cellIndex);
			if (region != this.cachedRegion)
			{
				this.cachedRegionIsDestination = this.destRegions.Contains(region);
				if (this.cachedRegionIsDestination)
				{
					return this.OctileDistanceToEnd(cell);
				}
				this.cachedBestLinkCost = this.regionCostCalculator.GetRegionBestDistances(region, out this.cachedBestLink, out this.cachedSecondBestLink, out this.cachedSecondBestLinkCost);
				this.cachedRegion = region;
			}
			else if (this.cachedRegionIsDestination)
			{
				return this.OctileDistanceToEnd(cell);
			}
			if (this.cachedBestLink != null)
			{
				int num = this.regionCostCalculator.RegionLinkDistance(cell, this.cachedBestLink, 1);
				int num3;
				if (this.cachedSecondBestLink != null)
				{
					int num2 = this.regionCostCalculator.RegionLinkDistance(cell, this.cachedSecondBestLink, 1);
					num3 = Mathf.Min(this.cachedSecondBestLinkCost + num2, this.cachedBestLinkCost + num);
				}
				else
				{
					num3 = this.cachedBestLinkCost + num;
				}
				return num3 + this.OctileDistanceToEndEps(cell);
			}
			return 10000;
		}

		// Token: 0x060044DB RID: 17627 RVA: 0x001B0218 File Offset: 0x001AE418
		private int OctileDistanceToEnd(IntVec3 cell)
		{
			int dx = Mathf.Abs(cell.x - this.endCell.x);
			int dz = Mathf.Abs(cell.z - this.endCell.z);
			return GenMath.OctileDistance(dx, dz, Mathf.RoundToInt(this.moveTicksCardinal), Mathf.RoundToInt(this.moveTicksDiagonal));
		}

		// Token: 0x060044DC RID: 17628 RVA: 0x001B0270 File Offset: 0x001AE470
		private int OctileDistanceToEndEps(IntVec3 cell)
		{
			int dx = Mathf.Abs(cell.x - this.endCell.x);
			int dz = Mathf.Abs(cell.z - this.endCell.z);
			return GenMath.OctileDistance(dx, dz, 2, 3);
		}

		// Token: 0x04002CA2 RID: 11426
		private Map map;

		// Token: 0x04002CA3 RID: 11427
		private IntVec3 endCell;

		// Token: 0x04002CA4 RID: 11428
		private HashSet<Region> destRegions = new HashSet<Region>();

		// Token: 0x04002CA5 RID: 11429
		private float moveTicksCardinal;

		// Token: 0x04002CA6 RID: 11430
		private float moveTicksDiagonal;

		// Token: 0x04002CA7 RID: 11431
		private RegionCostCalculator regionCostCalculator;

		// Token: 0x04002CA8 RID: 11432
		private Region cachedRegion;

		// Token: 0x04002CA9 RID: 11433
		private RegionLink cachedBestLink;

		// Token: 0x04002CAA RID: 11434
		private RegionLink cachedSecondBestLink;

		// Token: 0x04002CAB RID: 11435
		private int cachedBestLinkCost;

		// Token: 0x04002CAC RID: 11436
		private int cachedSecondBestLinkCost;

		// Token: 0x04002CAD RID: 11437
		private bool cachedRegionIsDestination;

		// Token: 0x04002CAE RID: 11438
		private Region[] regionGrid;
	}
}
