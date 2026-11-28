using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022E9 RID: 8937
	public abstract class FeatureWorker_Protrusion : FeatureWorker
	{
		// Token: 0x17002657 RID: 9815
		// (get) Token: 0x0600DC78 RID: 56440 RVA: 0x0052212B File Offset: 0x0052032B
		protected virtual int MinSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		// Token: 0x17002658 RID: 9816
		// (get) Token: 0x0600DC79 RID: 56441 RVA: 0x00522138 File Offset: 0x00520338
		protected virtual int MaxSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		// Token: 0x17002659 RID: 9817
		// (get) Token: 0x0600DC7A RID: 56442 RVA: 0x00522145 File Offset: 0x00520345
		protected virtual int MaxPassageWidth
		{
			get
			{
				return this.def.maxPassageWidth;
			}
		}

		// Token: 0x1700265A RID: 9818
		// (get) Token: 0x0600DC7B RID: 56443 RVA: 0x00522152 File Offset: 0x00520352
		protected virtual float MaxPctOfWholeArea
		{
			get
			{
				return this.def.maxPctOfWholeArea;
			}
		}

		// Token: 0x0600DC7C RID: 56444
		protected abstract bool IsRoot(PlanetTile tile);

		// Token: 0x0600DC7D RID: 56445 RVA: 0x0052215F File Offset: 0x0052035F
		protected virtual bool IsMember(PlanetTile tile)
		{
			return Find.WorldGrid[tile].feature == null;
		}

		// Token: 0x0600DC7E RID: 56446 RVA: 0x00522174 File Offset: 0x00520374
		public override void GenerateWhereAppropriate(PlanetLayer layer)
		{
			this.CalculateRoots(layer);
			this.CalculateRootsWithoutSmallPassages(layer);
			this.CalculateContiguousGroups(layer);
		}

		// Token: 0x0600DC7F RID: 56447 RVA: 0x0052218C File Offset: 0x0052038C
		private void CalculateRoots(PlanetLayer layer)
		{
			this.roots.Clear();
			int tilesCount = layer.TilesCount;
			for (int i = 0; i < tilesCount; i++)
			{
				PlanetTile planetTile = new PlanetTile(i, layer);
				if (this.IsRoot(planetTile))
				{
					this.roots.Add(planetTile);
				}
			}
			this.rootsSet.Clear();
			this.rootsSet.AddRange(this.roots);
		}

		// Token: 0x0600DC80 RID: 56448 RVA: 0x005221F4 File Offset: 0x005203F4
		private void CalculateRootsWithoutSmallPassages(PlanetLayer layer)
		{
			this.rootsWithoutSmallPassages.Clear();
			this.rootsWithoutSmallPassages.AddRange(this.roots);
			GenPlanetMorphology.Open(layer, this.rootsWithoutSmallPassages, this.MaxPassageWidth);
			this.rootsWithoutSmallPassagesSet.Clear();
			this.rootsWithoutSmallPassagesSet.AddRange(this.rootsWithoutSmallPassages);
		}

		// Token: 0x0600DC81 RID: 56449 RVA: 0x0052224C File Offset: 0x0052044C
		private void CalculateContiguousGroups(PlanetLayer layer)
		{
			int minSize = this.MinSize;
			int maxSize = this.MaxSize;
			float maxPctOfWholeArea = this.MaxPctOfWholeArea;
			int maxPassageWidth = this.MaxPassageWidth;
			FeatureWorker.ClearVisited(layer);
			FeatureWorker.ClearGroupSizes(layer);
			Predicate<PlanetTile> <>9__0;
			for (int i = 0; i < this.roots.Count; i++)
			{
				PlanetTile planetTile = this.roots[i];
				if (!FeatureWorker.visited[planetTile.tileId])
				{
					FeatureWorker_Protrusion.tmpGroup.Clear();
					WorldFloodFiller filler = layer.Filler;
					PlanetTile rootTile = planetTile;
					Predicate<PlanetTile> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((PlanetTile x) => this.rootsSet.Contains(x)));
					}
					filler.FloodFill(rootTile, passCheck, delegate(PlanetTile x)
					{
						FeatureWorker.visited[x.tileId] = true;
						FeatureWorker_Protrusion.tmpGroup.Add(x.tileId);
					}, int.MaxValue, null);
					for (int j = 0; j < FeatureWorker_Protrusion.tmpGroup.Count; j++)
					{
						FeatureWorker.groupSize[FeatureWorker_Protrusion.tmpGroup[j]] = FeatureWorker_Protrusion.tmpGroup.Count;
					}
					FeatureWorker_Protrusion.tmpGroup.Clear();
				}
			}
			FeatureWorker.ClearVisited(layer);
			Predicate<PlanetTile> <>9__2;
			Action<PlanetTile> <>9__3;
			Predicate<PlanetTile> <>9__4;
			Predicate<PlanetTile> <>9__6;
			Predicate<PlanetTile> <>9__7;
			for (int k = 0; k < this.rootsWithoutSmallPassages.Count; k++)
			{
				PlanetTile planetTile2 = this.rootsWithoutSmallPassages[k];
				if (!FeatureWorker.visited[planetTile2.tileId])
				{
					this.currentGroup.Clear();
					WorldFloodFiller filler2 = layer.Filler;
					PlanetTile rootTile2 = planetTile2;
					Predicate<PlanetTile> passCheck2;
					if ((passCheck2 = <>9__2) == null)
					{
						passCheck2 = (<>9__2 = ((PlanetTile x) => this.rootsWithoutSmallPassagesSet.Contains(x)));
					}
					Action<PlanetTile> processor;
					if ((processor = <>9__3) == null)
					{
						processor = (<>9__3 = delegate(PlanetTile x)
						{
							FeatureWorker.visited[x.tileId] = true;
							this.currentGroup.Add(x);
						});
					}
					filler2.FloodFill(rootTile2, passCheck2, processor, int.MaxValue, null);
					if (this.currentGroup.Count >= minSize)
					{
						PlanetLayer layer2 = layer;
						List<PlanetTile> tiles = this.currentGroup;
						int count = maxPassageWidth * 2;
						Predicate<PlanetTile> extraPredicate;
						if ((extraPredicate = <>9__4) == null)
						{
							extraPredicate = (<>9__4 = ((PlanetTile x) => this.rootsSet.Contains(x)));
						}
						GenPlanetMorphology.Dilate(layer2, tiles, count, extraPredicate);
						if (this.currentGroup.Count <= maxSize && (float)this.currentGroup.Count / (float)FeatureWorker.groupSize[planetTile2.tileId] <= maxPctOfWholeArea)
						{
							if (!this.def.canTouchWorldEdge)
							{
								if (this.currentGroup.Any((PlanetTile x) => Find.WorldGrid.IsOnEdge(x)))
								{
									goto IL_34A;
								}
							}
							this.currentGroupMembers.Clear();
							for (int l = 0; l < this.currentGroup.Count; l++)
							{
								if (this.IsMember(this.currentGroup[l]))
								{
									this.currentGroupMembers.Add(this.currentGroup[l]);
								}
							}
							if (this.currentGroupMembers.Count >= minSize)
							{
								List<PlanetTile> list = this.currentGroup;
								Predicate<PlanetTile> predicate;
								if ((predicate = <>9__6) == null)
								{
									predicate = (<>9__6 = ((PlanetTile x) => layer[x].feature == null));
								}
								if (list.Any(predicate))
								{
									List<PlanetTile> list2 = this.currentGroup;
									Predicate<PlanetTile> match;
									if ((match = <>9__7) == null)
									{
										match = (<>9__7 = ((PlanetTile x) => layer[x].feature != null));
									}
									list2.RemoveAll(match);
								}
								base.AddFeature(layer, this.currentGroupMembers, this.currentGroup);
							}
						}
					}
				}
				IL_34A:;
			}
		}

		// Token: 0x04008D55 RID: 36181
		private readonly List<PlanetTile> roots = new List<PlanetTile>();

		// Token: 0x04008D56 RID: 36182
		private readonly HashSet<PlanetTile> rootsSet = new HashSet<PlanetTile>();

		// Token: 0x04008D57 RID: 36183
		private readonly List<PlanetTile> rootsWithoutSmallPassages = new List<PlanetTile>();

		// Token: 0x04008D58 RID: 36184
		private readonly HashSet<PlanetTile> rootsWithoutSmallPassagesSet = new HashSet<PlanetTile>();

		// Token: 0x04008D59 RID: 36185
		private readonly List<PlanetTile> currentGroup = new List<PlanetTile>();

		// Token: 0x04008D5A RID: 36186
		private readonly List<PlanetTile> currentGroupMembers = new List<PlanetTile>();

		// Token: 0x04008D5B RID: 36187
		private static readonly List<int> tmpGroup = new List<int>();
	}
}
