using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022E5 RID: 8933
	public abstract class FeatureWorker_FloodFill : FeatureWorker
	{
		// Token: 0x17002653 RID: 9811
		// (get) Token: 0x0600DC65 RID: 56421 RVA: 0x00521AAA File Offset: 0x0051FCAA
		protected virtual int MinSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		// Token: 0x17002654 RID: 9812
		// (get) Token: 0x0600DC66 RID: 56422 RVA: 0x00521AB7 File Offset: 0x0051FCB7
		protected virtual int MaxSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		// Token: 0x17002655 RID: 9813
		// (get) Token: 0x0600DC67 RID: 56423 RVA: 0x00521AC4 File Offset: 0x0051FCC4
		protected virtual int MaxPossiblyAllowedSizeToTake
		{
			get
			{
				return this.def.maxPossiblyAllowedSizeToTake;
			}
		}

		// Token: 0x17002656 RID: 9814
		// (get) Token: 0x0600DC68 RID: 56424 RVA: 0x00521AD1 File Offset: 0x0051FCD1
		protected virtual float MaxPossiblyAllowedSizePctOfMeToTake
		{
			get
			{
				return this.def.maxPossiblyAllowedSizePctOfMeToTake;
			}
		}

		// Token: 0x0600DC69 RID: 56425
		protected abstract bool IsRoot(PlanetTile tile);

		// Token: 0x0600DC6A RID: 56426 RVA: 0x00521ADE File Offset: 0x0051FCDE
		protected virtual bool IsPossiblyAllowed(PlanetTile tile)
		{
			return false;
		}

		// Token: 0x0600DC6B RID: 56427 RVA: 0x00521AE1 File Offset: 0x0051FCE1
		protected virtual bool IsMember(PlanetTile tile)
		{
			return Find.WorldGrid[tile].feature == null;
		}

		// Token: 0x0600DC6C RID: 56428 RVA: 0x00521AF6 File Offset: 0x0051FCF6
		public override void GenerateWhereAppropriate(PlanetLayer layer)
		{
			this.CalculateRootsAndPossiblyAllowedTiles(layer);
			this.CalculateContiguousGroups(layer);
		}

		// Token: 0x0600DC6D RID: 56429 RVA: 0x00521B08 File Offset: 0x0051FD08
		private void CalculateRootsAndPossiblyAllowedTiles(PlanetLayer layer)
		{
			this.roots.Clear();
			this.possiblyAllowed.Clear();
			int tilesCount = layer.TilesCount;
			for (int i = 0; i < tilesCount; i++)
			{
				PlanetTile tile = layer[i].tile;
				if (this.IsRoot(tile))
				{
					this.roots.Add(tile);
				}
				if (this.IsPossiblyAllowed(tile))
				{
					this.possiblyAllowed.Add(tile);
				}
			}
			this.rootsSet.Clear();
			this.rootsSet.AddRange(this.roots);
			this.possiblyAllowedSet.Clear();
			this.possiblyAllowedSet.AddRange(this.possiblyAllowed);
		}

		// Token: 0x0600DC6E RID: 56430 RVA: 0x00521BB0 File Offset: 0x0051FDB0
		private void CalculateContiguousGroups(PlanetLayer layer)
		{
			WorldFloodFiller filler = layer.Filler;
			int minSize = this.MinSize;
			int maxSize = this.MaxSize;
			int maxPossiblyAllowedSizeToTake = this.MaxPossiblyAllowedSizeToTake;
			float maxPossiblyAllowedSizePctOfMeToTake = this.MaxPossiblyAllowedSizePctOfMeToTake;
			FeatureWorker.ClearVisited(layer);
			FeatureWorker.ClearGroupSizes(layer);
			Predicate<PlanetTile> <>9__0;
			for (int i = 0; i < this.possiblyAllowed.Count; i++)
			{
				PlanetTile planetTile = this.possiblyAllowed[i];
				if (!FeatureWorker.visited[planetTile.tileId] && !this.rootsSet.Contains(planetTile))
				{
					FeatureWorker_FloodFill.TmpGroup.Clear();
					WorldFloodFiller worldFloodFiller = filler;
					PlanetTile rootTile = planetTile;
					Predicate<PlanetTile> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((PlanetTile x) => this.possiblyAllowedSet.Contains(x) && !this.rootsSet.Contains(x)));
					}
					worldFloodFiller.FloodFill(rootTile, passCheck, delegate(PlanetTile x)
					{
						FeatureWorker.visited[x.tileId] = true;
						FeatureWorker_FloodFill.TmpGroup.Add(x);
					}, int.MaxValue, null);
					for (int j = 0; j < FeatureWorker_FloodFill.TmpGroup.Count; j++)
					{
						FeatureWorker.groupSize[FeatureWorker_FloodFill.TmpGroup[j].tileId] = FeatureWorker_FloodFill.TmpGroup.Count;
					}
				}
			}
			Predicate<PlanetTile> <>9__2;
			Predicate<PlanetTile> <>9__4;
			Predicate<PlanetTile> <>9__9;
			Predicate<PlanetTile> <>9__10;
			for (int k = 0; k < this.roots.Count; k++)
			{
				PlanetTile planetTile2 = this.roots[k];
				if (!FeatureWorker.visited[planetTile2.tileId])
				{
					int initialMembersCountClamped = 0;
					WorldFloodFiller worldFloodFiller2 = filler;
					PlanetTile rootTile2 = planetTile2;
					Predicate<PlanetTile> passCheck2;
					if ((passCheck2 = <>9__2) == null)
					{
						passCheck2 = (<>9__2 = ((PlanetTile x) => (this.rootsSet.Contains(x) || this.possiblyAllowedSet.Contains(x)) && this.IsMember(x)));
					}
					worldFloodFiller2.FloodFill(rootTile2, passCheck2, delegate(PlanetTile x)
					{
						FeatureWorker.visited[x.tileId] = true;
						int initialMembersCountClamped = initialMembersCountClamped;
						initialMembersCountClamped++;
						return initialMembersCountClamped >= minSize;
					}, int.MaxValue, null);
					if (initialMembersCountClamped >= minSize)
					{
						int initialRootsCount = 0;
						WorldFloodFiller worldFloodFiller3 = filler;
						PlanetTile rootTile3 = planetTile2;
						Predicate<PlanetTile> passCheck3;
						if ((passCheck3 = <>9__4) == null)
						{
							passCheck3 = (<>9__4 = ((PlanetTile x) => this.rootsSet.Contains(x)));
						}
						worldFloodFiller3.FloodFill(rootTile3, passCheck3, delegate(PlanetTile x)
						{
							FeatureWorker.visited[x.tileId] = true;
							int initialRootsCount = initialRootsCount;
							initialRootsCount++;
						}, int.MaxValue, null);
						if (initialRootsCount >= minSize && initialRootsCount <= maxSize)
						{
							int traversedRootsCount = 0;
							this.currentGroup.Clear();
							filler.FloodFill(planetTile2, (PlanetTile x) => this.rootsSet.Contains(x) || (this.possiblyAllowedSet.Contains(x) && FeatureWorker.groupSize[x.tileId] <= maxPossiblyAllowedSizeToTake && (float)FeatureWorker.groupSize[x.tileId] <= maxPossiblyAllowedSizePctOfMeToTake * (float)Mathf.Max(traversedRootsCount, initialRootsCount) && FeatureWorker.groupSize[x.tileId] < maxSize), delegate(PlanetTile x)
							{
								FeatureWorker.visited[x.tileId] = true;
								if (this.rootsSet.Contains(x))
								{
									int traversedRootsCount = traversedRootsCount;
									traversedRootsCount++;
								}
								this.currentGroup.Add(x);
							}, int.MaxValue, null);
							if (this.currentGroup.Count >= minSize && this.currentGroup.Count <= maxSize)
							{
								if (!this.def.canTouchWorldEdge)
								{
									if (this.currentGroup.Any((PlanetTile x) => Find.WorldGrid.IsOnEdge(x)))
									{
										goto IL_428;
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
									if ((predicate = <>9__9) == null)
									{
										predicate = (<>9__9 = ((PlanetTile x) => layer[x].feature == null));
									}
									if (list.Any(predicate))
									{
										List<PlanetTile> list2 = this.currentGroup;
										Predicate<PlanetTile> match;
										if ((match = <>9__10) == null)
										{
											match = (<>9__10 = ((PlanetTile x) => layer[x].feature != null));
										}
										list2.RemoveAll(match);
									}
									base.AddFeature(layer, this.currentGroupMembers, this.currentGroup);
								}
							}
						}
					}
				}
				IL_428:;
			}
		}

		// Token: 0x04008D4E RID: 36174
		private readonly List<PlanetTile> roots = new List<PlanetTile>();

		// Token: 0x04008D4F RID: 36175
		private readonly HashSet<PlanetTile> rootsSet = new HashSet<PlanetTile>();

		// Token: 0x04008D50 RID: 36176
		private readonly List<PlanetTile> possiblyAllowed = new List<PlanetTile>();

		// Token: 0x04008D51 RID: 36177
		private readonly HashSet<PlanetTile> possiblyAllowedSet = new HashSet<PlanetTile>();

		// Token: 0x04008D52 RID: 36178
		private readonly List<PlanetTile> currentGroup = new List<PlanetTile>();

		// Token: 0x04008D53 RID: 36179
		private readonly List<PlanetTile> currentGroupMembers = new List<PlanetTile>();

		// Token: 0x04008D54 RID: 36180
		private static readonly List<PlanetTile> TmpGroup = new List<PlanetTile>();
	}
}
