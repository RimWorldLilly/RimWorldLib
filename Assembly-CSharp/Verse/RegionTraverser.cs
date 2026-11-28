using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000303 RID: 771
	public static class RegionTraverser
	{
		// Token: 0x0600177D RID: 6013 RVA: 0x0008D1FC File Offset: 0x0008B3FC
		public static District FloodAndSetDistricts(Region root, Map map, District existingRoom)
		{
			District floodingDistrict;
			if (existingRoom == null)
			{
				floodingDistrict = District.MakeNew(map);
			}
			else
			{
				floodingDistrict = existingRoom;
			}
			root.District = floodingDistrict;
			if (!root.type.AllowsMultipleRegionsPerDistrict())
			{
				return floodingDistrict;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.type == root.type && r.District != floodingDistrict;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				r.District = floodingDistrict;
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, regionProcessor, 999999, RegionType.Set_All);
			return floodingDistrict;
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x0008D28C File Offset: 0x0008B48C
		public static void FloodAndSetNewRegionIndex(Region root, int newRegionGroupIndex)
		{
			root.newRegionGroupIndex = newRegionGroupIndex;
			if (!root.type.AllowsMultipleRegionsPerDistrict())
			{
				return;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.type == root.type && r.newRegionGroupIndex < 0;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				r.newRegionGroupIndex = newRegionGroupIndex;
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, regionProcessor, 999999, RegionType.Set_All);
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x0008D300 File Offset: 0x0008B500
		public static bool WithinRegions(this IntVec3 A, IntVec3 B, Map map, int regionLookCount, TraverseParms traverseParams, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			Region region = A.GetRegion(map, traversableRegionTypes);
			if (region == null)
			{
				return false;
			}
			Region regB = B.GetRegion(map, traversableRegionTypes);
			if (regB == null)
			{
				return false;
			}
			if (region == regB)
			{
				return true;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParams, false);
			bool found = false;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				if (r == regB)
				{
					found = true;
					return true;
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, regionLookCount, traversableRegionTypes);
			return found;
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x0008D380 File Offset: 0x0008B580
		public static void MarkRegionsBFS(Region root, RegionEntryPredicate entryCondition, int maxRegions, int inRadiusMark, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, delegate(Region r)
			{
				r.mark = inRadiusMark;
				return false;
			}, maxRegions, traversableRegionTypes);
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x0008D3B0 File Offset: 0x0008B5B0
		public static bool ShouldCountRegion(Region r)
		{
			return !r.IsDoorway;
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x0008D3BB File Offset: 0x0008B5BB
		static RegionTraverser()
		{
			RegionTraverser.RecreateWorkers();
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x0008D3E8 File Offset: 0x0008B5E8
		public static void RecreateWorkers()
		{
			RegionTraverser.freeWorkers.Clear();
			for (int i = 0; i < RegionTraverser.NumWorkers; i++)
			{
				RegionTraverser.freeWorkers.Enqueue(new RegionTraverser.BFSWorker(i));
			}
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x0008D420 File Offset: 0x0008B620
		public static void BreadthFirstTraverse(IntVec3 start, Map map, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions = 999999, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			Region region = start.GetRegion(map, traversableRegionTypes);
			if (region == null)
			{
				return;
			}
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x0008D447 File Offset: 0x0008B647
		public static void BreadthFirstTraverse(Region root, RegionProcessorDelegateCache processor, int maxRegions = 999999, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			RegionTraverser.BreadthFirstTraverse(root, processor.RegionEntryPredicateDelegate, processor.RegionProcessorDelegate, maxRegions, traversableRegionTypes);
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x0008D460 File Offset: 0x0008B660
		public static void BreadthFirstTraverse(Region root, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions = 999999, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			if (RegionTraverser.freeWorkers.Count == 0)
			{
				Log.Error("No free workers for breadth-first traversal. Either BFS recurred deeper than " + RegionTraverser.NumWorkers.ToString() + ", or a bug has put this system in an inconsistent state. Resetting.");
				return;
			}
			if (root == null)
			{
				Log.Error("BreadthFirstTraverse with null root region.");
				return;
			}
			RegionTraverser.BFSWorker bfsworker = RegionTraverser.freeWorkers.Dequeue();
			try
			{
				bfsworker.BreadthFirstTraverseWork(root, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
			}
			catch (Exception ex)
			{
				Log.Error("Exception in BreadthFirstTraverse: " + ex.ToString());
			}
			finally
			{
				bfsworker.Clear();
				RegionTraverser.freeWorkers.Enqueue(bfsworker);
			}
		}

		// Token: 0x040013E8 RID: 5096
		private static Queue<RegionTraverser.BFSWorker> freeWorkers = new Queue<RegionTraverser.BFSWorker>();

		// Token: 0x040013E9 RID: 5097
		public static int NumWorkers = 8;

		// Token: 0x040013EA RID: 5098
		public static readonly RegionEntryPredicate PassAll = (Region from, Region to) => true;

		// Token: 0x02002635 RID: 9781
		private class BFSWorker
		{
			// Token: 0x0600F295 RID: 62101 RVA: 0x0058E45E File Offset: 0x0058C65E
			public BFSWorker(int closedArrayPos)
			{
				this.closedArrayPos = closedArrayPos;
			}

			// Token: 0x0600F296 RID: 62102 RVA: 0x0058E47F File Offset: 0x0058C67F
			public void Clear()
			{
				this.open.Clear();
			}

			// Token: 0x0600F297 RID: 62103 RVA: 0x0058E48C File Offset: 0x0058C68C
			private void QueueNewOpenRegion(Region region)
			{
				if (region.closedIndex[this.closedArrayPos] == this.closedIndex)
				{
					throw new InvalidOperationException("Region is already closed; you can't open it. Region: " + region.ToString());
				}
				this.open.Enqueue(region);
				region.closedIndex[this.closedArrayPos] = this.closedIndex;
			}

			// Token: 0x0600F298 RID: 62104 RVA: 0x0058E4E3 File Offset: 0x0058C6E3
			private void FinalizeSearch()
			{
			}

			// Token: 0x0600F299 RID: 62105 RVA: 0x0058E4E8 File Offset: 0x0058C6E8
			public void BreadthFirstTraverseWork(Region root, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions, RegionType traversableRegionTypes)
			{
				if ((root.type & traversableRegionTypes) == RegionType.None)
				{
					return;
				}
				this.closedIndex += 1U;
				this.open.Clear();
				this.numRegionsProcessed = 0;
				this.QueueNewOpenRegion(root);
				while (this.open.Count > 0)
				{
					Region region = this.open.Dequeue();
					if (DebugViewSettings.drawRegionTraversal)
					{
						region.Debug_Notify_Traversed();
					}
					if (regionProcessor != null && regionProcessor(region))
					{
						this.FinalizeSearch();
						return;
					}
					if (RegionTraverser.ShouldCountRegion(region))
					{
						this.numRegionsProcessed++;
					}
					if (this.numRegionsProcessed >= maxRegions)
					{
						this.FinalizeSearch();
						return;
					}
					for (int i = 0; i < region.links.Count; i++)
					{
						RegionLink regionLink = region.links[i];
						for (int j = 0; j < 2; j++)
						{
							Region region2 = regionLink.regions[j];
							if (region2 != null && region2.closedIndex[this.closedArrayPos] != this.closedIndex && (region2.type & traversableRegionTypes) != RegionType.None && (entryCondition == null || entryCondition(region, region2)))
							{
								this.QueueNewOpenRegion(region2);
							}
						}
					}
				}
				this.FinalizeSearch();
			}

			// Token: 0x04009C9B RID: 40091
			private Queue<Region> open = new Queue<Region>();

			// Token: 0x04009C9C RID: 40092
			private int numRegionsProcessed;

			// Token: 0x04009C9D RID: 40093
			private uint closedIndex = 1U;

			// Token: 0x04009C9E RID: 40094
			private int closedArrayPos;

			// Token: 0x04009C9F RID: 40095
			private const int skippableRegionSize = 4;
		}
	}
}
