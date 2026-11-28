using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x020008AB RID: 2219
	public class RegionCostCalculator
	{
		// Token: 0x060044C3 RID: 17603 RVA: 0x001AF2C4 File Offset: 0x001AD4C4
		public RegionCostCalculator(Map map)
		{
			this.map = map;
			this.preciseRegionLinkDistancesDistanceGetter = new Func<int, int, float>(this.PreciseRegionLinkDistancesDistanceGetter);
		}

		// Token: 0x060044C4 RID: 17604 RVA: 0x001AF338 File Offset: 0x001AD538
		public void Init(CellRect destination, HashSet<Region> destRegions, TraverseParms parms, float moveTicksCardinal, float moveTicksDiagonal, ByteGrid avoidGrid, Area allowedArea, bool drafted)
		{
			this.regionGrid = this.map.regionGrid.DirectGrid;
			this.traverseParms = parms;
			this.pathingContext = this.map.pathing.For(parms);
			this.destinationCell = destination.CenterCell;
			this.moveTicksCardinal = moveTicksCardinal;
			this.moveTicksDiagonal = moveTicksDiagonal;
			this.avoidGrid = avoidGrid;
			this.allowedArea = allowedArea;
			this.drafted = drafted;
			this.regionMinLink.Clear();
			this.distances.Clear();
			this.linkTargetCells.Clear();
			this.queue.Clear();
			this.minPathCosts.Clear();
			foreach (Region region in destRegions)
			{
				int minPathCost = this.RegionMedianPathCost(region);
				for (int i = 0; i < region.links.Count; i++)
				{
					RegionLink regionLink = region.links[i];
					if (regionLink.GetOtherRegion(region).Allows(this.traverseParms, false))
					{
						int num = this.RegionLinkDistance(this.destinationCell, regionLink, minPathCost);
						int num2;
						if (this.distances.TryGetValue(regionLink, out num2))
						{
							if (num < num2)
							{
								this.linkTargetCells[regionLink] = this.GetLinkTargetCell(this.destinationCell, regionLink);
							}
							num = Math.Min(num2, num);
						}
						else
						{
							this.linkTargetCells[regionLink] = this.GetLinkTargetCell(this.destinationCell, regionLink);
						}
						this.distances[regionLink] = num;
					}
				}
				this.GetPreciseRegionLinkDistances(region, destination, this.preciseRegionLinkDistances);
				for (int j = 0; j < this.preciseRegionLinkDistances.Count; j++)
				{
					Pair<RegionLink, int> pair = this.preciseRegionLinkDistances[j];
					RegionLink first = pair.First;
					int num3 = this.distances[first];
					int num4;
					if (pair.Second > num3)
					{
						this.distances[first] = pair.Second;
						num4 = pair.Second;
					}
					else
					{
						num4 = num3;
					}
					this.queue.Push(new RegionCostCalculator.RegionLinkQueueEntry(region, first, num4, num4));
				}
			}
		}

		// Token: 0x060044C5 RID: 17605 RVA: 0x001AF58C File Offset: 0x001AD78C
		public int GetRegionDistance(Region region, out RegionLink minLink)
		{
			if (this.regionMinLink.TryGetValue(region.id, out minLink))
			{
				return this.distances[minLink];
			}
			while (this.queue.Count != 0)
			{
				RegionCostCalculator.RegionLinkQueueEntry regionLinkQueueEntry = this.queue.Pop();
				int num = this.distances[regionLinkQueueEntry.Link];
				if (regionLinkQueueEntry.Cost == num)
				{
					Region otherRegion = regionLinkQueueEntry.Link.GetOtherRegion(regionLinkQueueEntry.From);
					if (otherRegion != null && otherRegion.valid)
					{
						int num2 = 0;
						if (otherRegion.door != null)
						{
							num2 = (int)PathUtility.GetBuildingCost(otherRegion.door, this.traverseParms, this.traverseParms.pawn, null);
							if (num2 == 65535)
							{
								continue;
							}
							num2 += this.OctileDistance(1, 0);
						}
						int minPathCost = this.RegionMedianPathCost(otherRegion);
						for (int i = 0; i < otherRegion.links.Count; i++)
						{
							RegionLink regionLink = otherRegion.links[i];
							if (regionLink != regionLinkQueueEntry.Link && regionLink.GetOtherRegion(otherRegion).Allows(this.traverseParms, false))
							{
								int num3 = (otherRegion.door != null) ? num2 : this.RegionLinkDistance(regionLinkQueueEntry.Link, regionLink, minPathCost);
								num3 = Math.Max(num3, 1);
								int num4 = num + num3;
								int estimatedPathCost = this.MinimumRegionLinkDistance(this.destinationCell, regionLink) + num4;
								int num5;
								if (this.distances.TryGetValue(regionLink, out num5))
								{
									if (num4 < num5)
									{
										this.distances[regionLink] = num4;
										this.queue.Push(new RegionCostCalculator.RegionLinkQueueEntry(otherRegion, regionLink, num4, estimatedPathCost));
									}
								}
								else
								{
									this.distances.Add(regionLink, num4);
									this.queue.Push(new RegionCostCalculator.RegionLinkQueueEntry(otherRegion, regionLink, num4, estimatedPathCost));
								}
							}
						}
						if (!this.regionMinLink.ContainsKey(otherRegion.id))
						{
							this.regionMinLink.Add(otherRegion.id, regionLinkQueueEntry.Link);
							if (otherRegion == region)
							{
								minLink = regionLinkQueueEntry.Link;
								return regionLinkQueueEntry.Cost;
							}
						}
					}
				}
			}
			return 10000;
		}

		// Token: 0x060044C6 RID: 17606 RVA: 0x001AF7B0 File Offset: 0x001AD9B0
		public int GetRegionBestDistances(Region region, out RegionLink bestLink, out RegionLink secondBestLink, out int secondBestCost)
		{
			int regionDistance = this.GetRegionDistance(region, out bestLink);
			secondBestLink = null;
			secondBestCost = int.MaxValue;
			for (int i = 0; i < region.links.Count; i++)
			{
				RegionLink regionLink = region.links[i];
				int num;
				if (regionLink != bestLink && regionLink.GetOtherRegion(region).Allows(this.traverseParms, false) && this.distances.TryGetValue(regionLink, out num) && num < secondBestCost)
				{
					secondBestCost = num;
					secondBestLink = regionLink;
				}
			}
			return regionDistance;
		}

		// Token: 0x060044C7 RID: 17607 RVA: 0x001AF82C File Offset: 0x001ADA2C
		public int RegionMedianPathCost(Region region)
		{
			int result;
			if (this.minPathCosts.TryGetValue(region, out result))
			{
				return result;
			}
			bool ignoreAllowedAreaCost = this.allowedArea != null && region.OverlapWith(this.allowedArea) > AreaOverlap.None;
			CellIndices cellIndices = this.map.cellIndices;
			Rand.PushState();
			Rand.Seed = cellIndices.CellToIndex(region.extentsClose.CenterCell) * (region.links.Count + 1);
			for (int i = 0; i < 11; i++)
			{
				RegionCostCalculator.pathCostSamples[i] = this.GetCellCostFast(cellIndices.CellToIndex(region.RandomCell), ignoreAllowedAreaCost);
			}
			Rand.PopState();
			Array.Sort<int>(RegionCostCalculator.pathCostSamples);
			return this.minPathCosts[region] = RegionCostCalculator.pathCostSamples[4];
		}

		// Token: 0x060044C8 RID: 17608 RVA: 0x001AF8EC File Offset: 0x001ADAEC
		private int GetCellCostFast(int index, bool ignoreAllowedAreaCost = false)
		{
			int num = this.pathingContext.pathGrid.Grid_Unsafe[index];
			if (this.avoidGrid != null)
			{
				num += (int)(this.avoidGrid[index] * 8);
			}
			if (this.allowedArea != null && !ignoreAllowedAreaCost && !this.allowedArea[index])
			{
				num += 600;
			}
			if (this.drafted)
			{
				num += this.map.terrainGrid.topGrid[index].extraDraftedPerceivedPathCost;
			}
			else
			{
				num += this.map.terrainGrid.topGrid[index].extraNonDraftedPerceivedPathCost;
			}
			return num;
		}

		// Token: 0x060044C9 RID: 17609 RVA: 0x001AF98C File Offset: 0x001ADB8C
		private int RegionLinkDistance(RegionLink a, RegionLink b, int minPathCost)
		{
			IntVec3 intVec;
			IntVec3 a2 = this.linkTargetCells.TryGetValue(a, out intVec) ? intVec : RegionCostCalculator.RegionLinkCenter(a);
			IntVec3 intVec2;
			IntVec3 b2 = this.linkTargetCells.TryGetValue(b, out intVec2) ? intVec2 : RegionCostCalculator.RegionLinkCenter(b);
			IntVec3 intVec3 = a2 - b2;
			int num = Math.Abs(intVec3.x);
			int num2 = Math.Abs(intVec3.z);
			return this.OctileDistance(num, num2) + minPathCost * Math.Max(num, num2) + minPathCost * Math.Min(num, num2);
		}

		// Token: 0x060044CA RID: 17610 RVA: 0x001AFA0C File Offset: 0x001ADC0C
		public int RegionLinkDistance(IntVec3 cell, RegionLink link, int minPathCost)
		{
			IntVec3 linkTargetCell = this.GetLinkTargetCell(cell, link);
			IntVec3 intVec = cell - linkTargetCell;
			int num = Math.Abs(intVec.x);
			int num2 = Math.Abs(intVec.z);
			return this.OctileDistance(num, num2) + minPathCost * Math.Max(num, num2) + minPathCost * Math.Min(num, num2);
		}

		// Token: 0x060044CB RID: 17611 RVA: 0x001AFA5C File Offset: 0x001ADC5C
		private static int SpanCenterX(EdgeSpan e)
		{
			return e.root.x + ((e.dir == SpanDirection.East) ? (e.length / 2) : 0);
		}

		// Token: 0x060044CC RID: 17612 RVA: 0x001AFA7E File Offset: 0x001ADC7E
		private static int SpanCenterZ(EdgeSpan e)
		{
			return e.root.z + ((e.dir == SpanDirection.North) ? (e.length / 2) : 0);
		}

		// Token: 0x060044CD RID: 17613 RVA: 0x001AFA9F File Offset: 0x001ADC9F
		private static IntVec3 RegionLinkCenter(RegionLink link)
		{
			return new IntVec3(RegionCostCalculator.SpanCenterX(link.span), 0, RegionCostCalculator.SpanCenterZ(link.span));
		}

		// Token: 0x060044CE RID: 17614 RVA: 0x001AFAC0 File Offset: 0x001ADCC0
		private int MinimumRegionLinkDistance(IntVec3 cell, RegionLink link)
		{
			IntVec3 intVec = cell - RegionCostCalculator.LinkClosestCell(cell, link);
			return this.OctileDistance(Math.Abs(intVec.x), Math.Abs(intVec.z));
		}

		// Token: 0x060044CF RID: 17615 RVA: 0x001AFAF7 File Offset: 0x001ADCF7
		private int OctileDistance(int dx, int dz)
		{
			return GenMath.OctileDistance(dx, dz, Mathf.RoundToInt(this.moveTicksCardinal), Mathf.RoundToInt(this.moveTicksDiagonal));
		}

		// Token: 0x060044D0 RID: 17616 RVA: 0x001AFB16 File Offset: 0x001ADD16
		private IntVec3 GetLinkTargetCell(IntVec3 cell, RegionLink link)
		{
			return RegionCostCalculator.LinkClosestCell(cell, link);
		}

		// Token: 0x060044D1 RID: 17617 RVA: 0x001AFB20 File Offset: 0x001ADD20
		private static IntVec3 LinkClosestCell(IntVec3 cell, RegionLink link)
		{
			EdgeSpan span = link.span;
			int num = 0;
			int num2 = 0;
			if (span.dir == SpanDirection.North)
			{
				num2 = span.length - 1;
			}
			else
			{
				num = span.length - 1;
			}
			IntVec3 root = span.root;
			return new IntVec3(Mathf.Clamp(cell.x, root.x, root.x + num), 0, Mathf.Clamp(cell.z, root.z, root.z + num2));
		}

		// Token: 0x060044D2 RID: 17618 RVA: 0x001AFB94 File Offset: 0x001ADD94
		private void GetPreciseRegionLinkDistances(Region region, CellRect destination, List<Pair<RegionLink, int>> outDistances)
		{
			outDistances.Clear();
			RegionCostCalculator.tmpCellIndices.Clear();
			if (destination.Width == 1 && destination.Height == 1)
			{
				RegionCostCalculator.tmpCellIndices.Add(this.map.cellIndices.CellToIndex(destination.CenterCell));
			}
			else
			{
				foreach (IntVec3 c in destination)
				{
					if (c.InBounds(this.map))
					{
						RegionCostCalculator.tmpCellIndices.Add(this.map.cellIndices.CellToIndex(c));
					}
				}
			}
			Dijkstra<int>.Run(RegionCostCalculator.tmpCellIndices, (int x) => this.PreciseRegionLinkDistancesNeighborsGetter(x, region), this.preciseRegionLinkDistancesDistanceGetter, RegionCostCalculator.tmpDistances, null);
			for (int i = 0; i < region.links.Count; i++)
			{
				RegionLink regionLink = region.links[i];
				if (regionLink.GetOtherRegion(region).Allows(this.traverseParms, false))
				{
					float num;
					if (!RegionCostCalculator.tmpDistances.TryGetValue(this.map.cellIndices.CellToIndex(this.linkTargetCells[regionLink]), out num))
					{
						Log.ErrorOnce("Dijkstra couldn't reach one of the cells even though they are in the same region. There is most likely something wrong with the neighbor nodes getter.", 1938471531);
						num = 100f;
					}
					outDistances.Add(new Pair<RegionLink, int>(regionLink, (int)num));
				}
			}
		}

		// Token: 0x060044D3 RID: 17619 RVA: 0x001AFD24 File Offset: 0x001ADF24
		private IEnumerable<int> PreciseRegionLinkDistancesNeighborsGetter(int node, Region region)
		{
			if (this.regionGrid[node] == null || this.regionGrid[node] != region)
			{
				return null;
			}
			return this.PathableNeighborIndices(node);
		}

		// Token: 0x060044D4 RID: 17620 RVA: 0x001AFD44 File Offset: 0x001ADF44
		private float PreciseRegionLinkDistancesDistanceGetter(int a, int b)
		{
			return (float)this.GetCellCostFast(b, false) + (this.AreCellsDiagonal(a, b) ? this.moveTicksDiagonal : this.moveTicksCardinal);
		}

		// Token: 0x060044D5 RID: 17621 RVA: 0x001AFD68 File Offset: 0x001ADF68
		private bool AreCellsDiagonal(int a, int b)
		{
			int x = this.map.Size.x;
			return a % x != b % x && a / x != b / x;
		}

		// Token: 0x060044D6 RID: 17622 RVA: 0x001AFD9C File Offset: 0x001ADF9C
		private List<int> PathableNeighborIndices(int index)
		{
			RegionCostCalculator.tmpPathableNeighborIndices.Clear();
			PathGrid pathGrid = this.pathingContext.pathGrid;
			int x = this.map.Size.x;
			bool flag = index % x > 0;
			bool flag2 = index % x < x - 1;
			bool flag3 = index >= x;
			bool flag4 = index / x < this.map.Size.z - 1;
			if (flag3 && pathGrid.WalkableFast(index - x))
			{
				RegionCostCalculator.tmpPathableNeighborIndices.Add(index - x);
			}
			if (flag2 && pathGrid.WalkableFast(index + 1))
			{
				RegionCostCalculator.tmpPathableNeighborIndices.Add(index + 1);
			}
			if (flag && pathGrid.WalkableFast(index - 1))
			{
				RegionCostCalculator.tmpPathableNeighborIndices.Add(index - 1);
			}
			if (flag4 && pathGrid.WalkableFast(index + x))
			{
				RegionCostCalculator.tmpPathableNeighborIndices.Add(index + x);
			}
			bool canBashFences = this.traverseParms.canBashFences;
			bool flag5 = !flag || PathUtility.BlocksDiagonalMovement(index - 1, this.pathingContext, canBashFences);
			bool flag6 = !flag2 || PathUtility.BlocksDiagonalMovement(index + 1, this.pathingContext, canBashFences);
			if (flag3 && !PathUtility.BlocksDiagonalMovement(index - x, this.pathingContext, canBashFences))
			{
				if (!flag6 && pathGrid.WalkableFast(index - x + 1))
				{
					RegionCostCalculator.tmpPathableNeighborIndices.Add(index - x + 1);
				}
				if (!flag5 && pathGrid.WalkableFast(index - x - 1))
				{
					RegionCostCalculator.tmpPathableNeighborIndices.Add(index - x - 1);
				}
			}
			if (flag4 && !PathUtility.BlocksDiagonalMovement(index + x, this.pathingContext, canBashFences))
			{
				if (!flag6 && pathGrid.WalkableFast(index + x + 1))
				{
					RegionCostCalculator.tmpPathableNeighborIndices.Add(index + x + 1);
				}
				if (!flag5 && pathGrid.WalkableFast(index + x - 1))
				{
					RegionCostCalculator.tmpPathableNeighborIndices.Add(index + x - 1);
				}
			}
			return RegionCostCalculator.tmpPathableNeighborIndices;
		}

		// Token: 0x04002C8C RID: 11404
		private readonly Map map;

		// Token: 0x04002C8D RID: 11405
		private Region[] regionGrid;

		// Token: 0x04002C8E RID: 11406
		private TraverseParms traverseParms;

		// Token: 0x04002C8F RID: 11407
		private PathingContext pathingContext;

		// Token: 0x04002C90 RID: 11408
		private IntVec3 destinationCell;

		// Token: 0x04002C91 RID: 11409
		private float moveTicksCardinal;

		// Token: 0x04002C92 RID: 11410
		private float moveTicksDiagonal;

		// Token: 0x04002C93 RID: 11411
		private ByteGrid avoidGrid;

		// Token: 0x04002C94 RID: 11412
		private Area allowedArea;

		// Token: 0x04002C95 RID: 11413
		private bool drafted;

		// Token: 0x04002C96 RID: 11414
		private readonly Func<int, int, float> preciseRegionLinkDistancesDistanceGetter;

		// Token: 0x04002C97 RID: 11415
		private readonly Dictionary<int, RegionLink> regionMinLink = new Dictionary<int, RegionLink>();

		// Token: 0x04002C98 RID: 11416
		private readonly Dictionary<RegionLink, int> distances = new Dictionary<RegionLink, int>();

		// Token: 0x04002C99 RID: 11417
		private readonly FastPriorityQueue<RegionCostCalculator.RegionLinkQueueEntry> queue = new FastPriorityQueue<RegionCostCalculator.RegionLinkQueueEntry>(new RegionCostCalculator.DistanceComparer());

		// Token: 0x04002C9A RID: 11418
		private readonly Dictionary<Region, int> minPathCosts = new Dictionary<Region, int>();

		// Token: 0x04002C9B RID: 11419
		private readonly List<Pair<RegionLink, int>> preciseRegionLinkDistances = new List<Pair<RegionLink, int>>();

		// Token: 0x04002C9C RID: 11420
		private readonly Dictionary<RegionLink, IntVec3> linkTargetCells = new Dictionary<RegionLink, IntVec3>();

		// Token: 0x04002C9D RID: 11421
		private const int SampleCount = 11;

		// Token: 0x04002C9E RID: 11422
		private static readonly int[] pathCostSamples = new int[11];

		// Token: 0x04002C9F RID: 11423
		private static readonly List<int> tmpCellIndices = new List<int>();

		// Token: 0x04002CA0 RID: 11424
		private static readonly Dictionary<int, float> tmpDistances = new Dictionary<int, float>();

		// Token: 0x04002CA1 RID: 11425
		private static readonly List<int> tmpPathableNeighborIndices = new List<int>();

		// Token: 0x02002B98 RID: 11160
		private struct RegionLinkQueueEntry
		{
			// Token: 0x17002C65 RID: 11365
			// (get) Token: 0x06010AB4 RID: 68276 RVA: 0x005D685B File Offset: 0x005D4A5B
			public Region From
			{
				get
				{
					return this.from;
				}
			}

			// Token: 0x17002C66 RID: 11366
			// (get) Token: 0x06010AB5 RID: 68277 RVA: 0x005D6863 File Offset: 0x005D4A63
			public RegionLink Link
			{
				get
				{
					return this.link;
				}
			}

			// Token: 0x17002C67 RID: 11367
			// (get) Token: 0x06010AB6 RID: 68278 RVA: 0x005D686B File Offset: 0x005D4A6B
			public int Cost
			{
				get
				{
					return this.cost;
				}
			}

			// Token: 0x17002C68 RID: 11368
			// (get) Token: 0x06010AB7 RID: 68279 RVA: 0x005D6873 File Offset: 0x005D4A73
			public int EstimatedPathCost
			{
				get
				{
					return this.estimatedPathCost;
				}
			}

			// Token: 0x06010AB8 RID: 68280 RVA: 0x005D687B File Offset: 0x005D4A7B
			public RegionLinkQueueEntry(Region from, RegionLink link, int cost, int estimatedPathCost)
			{
				this.from = from;
				this.link = link;
				this.cost = cost;
				this.estimatedPathCost = estimatedPathCost;
			}

			// Token: 0x0400B218 RID: 45592
			private readonly Region from;

			// Token: 0x0400B219 RID: 45593
			private readonly RegionLink link;

			// Token: 0x0400B21A RID: 45594
			private readonly int cost;

			// Token: 0x0400B21B RID: 45595
			private readonly int estimatedPathCost;
		}

		// Token: 0x02002B99 RID: 11161
		private class DistanceComparer : IComparer<RegionCostCalculator.RegionLinkQueueEntry>
		{
			// Token: 0x06010AB9 RID: 68281 RVA: 0x005D689C File Offset: 0x005D4A9C
			public int Compare(RegionCostCalculator.RegionLinkQueueEntry a, RegionCostCalculator.RegionLinkQueueEntry b)
			{
				return a.EstimatedPathCost.CompareTo(b.EstimatedPathCost);
			}
		}
	}
}
