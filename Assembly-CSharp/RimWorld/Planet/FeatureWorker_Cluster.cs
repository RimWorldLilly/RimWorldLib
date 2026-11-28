using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022E1 RID: 8929
	public abstract class FeatureWorker_Cluster : FeatureWorker
	{
		// Token: 0x1700264D RID: 9805
		// (get) Token: 0x0600DC4C RID: 56396 RVA: 0x005212F1 File Offset: 0x0051F4F1
		protected virtual int MinRootGroupsInCluster
		{
			get
			{
				return this.def.minRootGroupsInCluster;
			}
		}

		// Token: 0x1700264E RID: 9806
		// (get) Token: 0x0600DC4D RID: 56397 RVA: 0x005212FE File Offset: 0x0051F4FE
		protected virtual int MinRootGroupSize
		{
			get
			{
				return this.def.minRootGroupSize;
			}
		}

		// Token: 0x1700264F RID: 9807
		// (get) Token: 0x0600DC4E RID: 56398 RVA: 0x0052130B File Offset: 0x0051F50B
		protected virtual int MaxRootGroupSize
		{
			get
			{
				return this.def.maxRootGroupSize;
			}
		}

		// Token: 0x17002650 RID: 9808
		// (get) Token: 0x0600DC4F RID: 56399 RVA: 0x00521318 File Offset: 0x0051F518
		protected virtual int MinOverallSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		// Token: 0x17002651 RID: 9809
		// (get) Token: 0x0600DC50 RID: 56400 RVA: 0x00521325 File Offset: 0x0051F525
		protected virtual int MaxOverallSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		// Token: 0x17002652 RID: 9810
		// (get) Token: 0x0600DC51 RID: 56401 RVA: 0x00521332 File Offset: 0x0051F532
		protected virtual int MaxSpaceBetweenRootGroups
		{
			get
			{
				return this.def.maxSpaceBetweenRootGroups;
			}
		}

		// Token: 0x0600DC52 RID: 56402
		protected abstract bool IsRoot(PlanetTile tile);

		// Token: 0x0600DC53 RID: 56403 RVA: 0x0052133F File Offset: 0x0051F53F
		protected virtual bool CanTraverse(PlanetTile tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return true;
		}

		// Token: 0x0600DC54 RID: 56404 RVA: 0x00521345 File Offset: 0x0051F545
		protected virtual bool IsMember(PlanetTile tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return Find.WorldGrid[tile].feature == null;
		}

		// Token: 0x0600DC55 RID: 56405 RVA: 0x0052135D File Offset: 0x0051F55D
		public override void GenerateWhereAppropriate(PlanetLayer layer)
		{
			this.CalculateRootTiles(layer);
			this.CalculateRootsWithAreaInBetween(layer);
			this.CalculateContiguousGroups(layer);
		}

		// Token: 0x0600DC56 RID: 56406 RVA: 0x00521374 File Offset: 0x0051F574
		private void CalculateRootTiles(PlanetLayer layer)
		{
			this.roots.Clear();
			int tilesCount = layer.TilesCount;
			for (int i = 0; i < layer.TilesCount; i++)
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

		// Token: 0x0600DC57 RID: 56407 RVA: 0x005213E0 File Offset: 0x0051F5E0
		private void CalculateRootsWithAreaInBetween(PlanetLayer layer)
		{
			this.rootsWithAreaInBetween.Clear();
			this.rootsWithAreaInBetween.AddRange(this.roots);
			GenPlanetMorphology.Close(layer, this.rootsWithAreaInBetween, this.MaxSpaceBetweenRootGroups);
			this.rootsWithAreaInBetweenSet.Clear();
			this.rootsWithAreaInBetweenSet.AddRange(this.rootsWithAreaInBetween);
		}

		// Token: 0x0600DC58 RID: 56408 RVA: 0x00521438 File Offset: 0x0051F638
		private void CalculateContiguousGroups(PlanetLayer layer)
		{
			WorldFloodFiller filler = layer.Filler;
			WorldGrid worldGrid = Find.WorldGrid;
			int minRootGroupSize = this.MinRootGroupSize;
			int maxRootGroupSize = this.MaxRootGroupSize;
			int minOverallSize = this.MinOverallSize;
			int maxOverallSize = this.MaxOverallSize;
			int minRootGroupsInCluster = this.MinRootGroupsInCluster;
			FeatureWorker.ClearVisited(layer);
			FeatureWorker.ClearGroupSizes(layer);
			FeatureWorker.ClearGroupIDs(layer);
			Predicate<PlanetTile> <>9__0;
			for (int i = 0; i < this.roots.Count; i++)
			{
				PlanetTile planetTile = this.roots[i];
				if (!FeatureWorker.visited[planetTile.tileId])
				{
					bool anyMember = false;
					FeatureWorker_Cluster.tmpGroup.Clear();
					WorldFloodFiller worldFloodFiller = filler;
					PlanetTile rootTile = planetTile;
					Predicate<PlanetTile> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((PlanetTile x) => this.rootsSet.Contains(x)));
					}
					worldFloodFiller.FloodFill(rootTile, passCheck, delegate(PlanetTile x)
					{
						FeatureWorker.visited[x.tileId] = true;
						FeatureWorker_Cluster.tmpGroup.Add(x.tileId);
						bool flag2;
						if (!anyMember && this.IsMember(x, out flag2))
						{
							anyMember = true;
						}
					}, int.MaxValue, null);
					for (int j = 0; j < FeatureWorker_Cluster.tmpGroup.Count; j++)
					{
						FeatureWorker.groupSize[FeatureWorker_Cluster.tmpGroup[j]] = FeatureWorker_Cluster.tmpGroup.Count;
						if (anyMember)
						{
							FeatureWorker.groupID[FeatureWorker_Cluster.tmpGroup[j]] = i + 1;
						}
					}
				}
			}
			FeatureWorker.ClearVisited(layer);
			Predicate<PlanetTile> <>9__2;
			Action<PlanetTile> <>9__3;
			Predicate<PlanetTile> <>9__4;
			Predicate<PlanetTile> <>9__5;
			Predicate<PlanetTile> <>9__6;
			for (int k = 0; k < this.roots.Count; k++)
			{
				PlanetTile planetTile2 = this.roots[k];
				if (!FeatureWorker.visited[planetTile2.tileId] && FeatureWorker.groupSize[planetTile2.tileId] >= minRootGroupSize && FeatureWorker.groupSize[planetTile2.tileId] <= maxRootGroupSize && FeatureWorker.groupSize[planetTile2.tileId] <= maxOverallSize)
				{
					this.currentGroup.Clear();
					this.visitedValidGroupIDs.Clear();
					WorldFloodFiller worldFloodFiller2 = filler;
					PlanetTile rootTile2 = planetTile2;
					Predicate<PlanetTile> passCheck2;
					if ((passCheck2 = <>9__2) == null)
					{
						passCheck2 = (<>9__2 = delegate(PlanetTile x)
						{
							bool flag2;
							return this.rootsWithAreaInBetweenSet.Contains(x) && this.CanTraverse(x, out flag2) && (!flag2 || !this.rootsSet.Contains(x) || (FeatureWorker.groupSize[x.tileId] >= minRootGroupSize && FeatureWorker.groupSize[x.tileId] <= maxRootGroupSize));
						});
					}
					Action<PlanetTile> processor;
					if ((processor = <>9__3) == null)
					{
						processor = (<>9__3 = delegate(PlanetTile x)
						{
							FeatureWorker.visited[x.tileId] = true;
							this.currentGroup.Add(x);
							if (FeatureWorker.groupID[x.tileId] != 0 && FeatureWorker.groupSize[x.tileId] >= minRootGroupSize && FeatureWorker.groupSize[x.tileId] <= maxRootGroupSize)
							{
								this.visitedValidGroupIDs.Add(FeatureWorker.groupID[x.tileId]);
							}
						});
					}
					worldFloodFiller2.FloodFill(rootTile2, passCheck2, processor, int.MaxValue, null);
					if (this.currentGroup.Count >= minOverallSize && this.currentGroup.Count <= maxOverallSize && this.visitedValidGroupIDs.Count >= minRootGroupsInCluster)
					{
						if (!this.def.canTouchWorldEdge)
						{
							List<PlanetTile> list = this.currentGroup;
							Predicate<PlanetTile> predicate;
							if ((predicate = <>9__4) == null)
							{
								predicate = (<>9__4 = ((PlanetTile x) => worldGrid.IsOnEdge(x)));
							}
							if (list.Any(predicate))
							{
								goto IL_3C1;
							}
						}
						this.currentGroupMembers.Clear();
						for (int l = 0; l < this.currentGroup.Count; l++)
						{
							PlanetTile planetTile3 = this.currentGroup[l];
							bool flag;
							if (this.IsMember(planetTile3, out flag) && (!flag || !this.rootsSet.Contains(planetTile3) || (FeatureWorker.groupSize[planetTile3.tileId] >= minRootGroupSize && FeatureWorker.groupSize[planetTile3.tileId] <= maxRootGroupSize)))
							{
								this.currentGroupMembers.Add(this.currentGroup[l]);
							}
						}
						if (this.currentGroupMembers.Count >= minOverallSize)
						{
							List<PlanetTile> list2 = this.currentGroup;
							Predicate<PlanetTile> predicate2;
							if ((predicate2 = <>9__5) == null)
							{
								predicate2 = (<>9__5 = ((PlanetTile x) => worldGrid[x].feature == null));
							}
							if (list2.Any(predicate2))
							{
								List<PlanetTile> list3 = this.currentGroup;
								Predicate<PlanetTile> match;
								if ((match = <>9__6) == null)
								{
									match = (<>9__6 = ((PlanetTile x) => worldGrid[x].feature != null));
								}
								list3.RemoveAll(match);
							}
							base.AddFeature(layer, this.currentGroupMembers, this.currentGroup);
						}
					}
				}
				IL_3C1:;
			}
		}

		// Token: 0x04008D44 RID: 36164
		private readonly List<PlanetTile> roots = new List<PlanetTile>();

		// Token: 0x04008D45 RID: 36165
		private readonly HashSet<PlanetTile> rootsSet = new HashSet<PlanetTile>();

		// Token: 0x04008D46 RID: 36166
		private readonly List<PlanetTile> rootsWithAreaInBetween = new List<PlanetTile>();

		// Token: 0x04008D47 RID: 36167
		private readonly HashSet<PlanetTile> rootsWithAreaInBetweenSet = new HashSet<PlanetTile>();

		// Token: 0x04008D48 RID: 36168
		private readonly List<PlanetTile> currentGroup = new List<PlanetTile>();

		// Token: 0x04008D49 RID: 36169
		private readonly List<PlanetTile> currentGroupMembers = new List<PlanetTile>();

		// Token: 0x04008D4A RID: 36170
		private readonly HashSet<int> visitedValidGroupIDs = new HashSet<int>();

		// Token: 0x04008D4B RID: 36171
		private static readonly List<int> tmpGroup = new List<int>();
	}
}
