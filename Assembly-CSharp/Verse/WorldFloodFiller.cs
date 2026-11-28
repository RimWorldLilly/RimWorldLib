using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Unity.Collections;

namespace Verse
{
	// Token: 0x020000CE RID: 206
	public class WorldFloodFiller
	{
		// Token: 0x060007BD RID: 1981 RVA: 0x0002CB1F File Offset: 0x0002AD1F
		public WorldFloodFiller(PlanetLayer layer)
		{
			this.layer = layer;
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0002CB50 File Offset: 0x0002AD50
		public void FloodFill(PlanetTile rootTile, Predicate<PlanetTile> passCheck, Action<PlanetTile> processor, int maxTilesToProcess = 2147483647, IEnumerable<PlanetTile> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, delegate(PlanetTile _, PlanetTile tile, int _)
			{
				processor(tile);
				return false;
			}, maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0002CB84 File Offset: 0x0002AD84
		public void FloodFill(PlanetTile rootTile, Predicate<PlanetTile> passCheck, Action<PlanetTile, int> processor, int maxTilesToProcess = 2147483647, IEnumerable<PlanetTile> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, delegate(PlanetTile _, PlanetTile tile, int dist)
			{
				processor(tile, dist);
				return false;
			}, maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0002CBB8 File Offset: 0x0002ADB8
		public void FloodFill(PlanetTile rootTile, Predicate<PlanetTile> passCheck, Action<PlanetTile, PlanetTile, int> processor, int maxTilesToProcess = 2147483647, IEnumerable<PlanetTile> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, delegate(PlanetTile from, PlanetTile tile, int dist)
			{
				processor(from, tile, dist);
				return false;
			}, maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0002CBEC File Offset: 0x0002ADEC
		public void FloodFill(PlanetTile rootTile, Predicate<PlanetTile> passCheck, Predicate<PlanetTile> processor, int maxTilesToProcess = 2147483647, IEnumerable<PlanetTile> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, (PlanetTile _, PlanetTile tile, int _) => processor(tile), maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0002CC20 File Offset: 0x0002AE20
		public void FloodFill(PlanetTile rootTile, Predicate<PlanetTile> passCheck, Predicate<PlanetTile, int> processor, int maxTilesToProcess = 2147483647, IEnumerable<PlanetTile> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, (PlanetTile _, PlanetTile tile, int dist) => processor(tile, dist), maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0002CC54 File Offset: 0x0002AE54
		public void FloodFill(PlanetTile rootTile, Predicate<PlanetTile> passCheck, WorldFloodFiller.WorldFillPredicate processor, int maxTilesToProcess = 2147483647, IEnumerable<PlanetTile> extraRootTiles = null)
		{
			if (this.working)
			{
				Log.Error("Nested FloodFill calls are not allowed. This will cause bugs.");
			}
			using (ProfilerBlock.Scope("WorldFloodFill"))
			{
				this.working = true;
				this.ClearVisited();
				if (rootTile.Valid && extraRootTiles == null && !passCheck(rootTile))
				{
					this.working = false;
				}
				else
				{
					int tilesCount = rootTile.Layer.TilesCount;
					int num = tilesCount;
					if (this.traversalDistance.Count < tilesCount)
					{
						this.traversalDistance.Capacity = tilesCount;
						for (int i = this.traversalDistance.Count; i < tilesCount; i++)
						{
							this.traversalDistance.Add(-1);
						}
					}
					NativeArray<int> unsafeTileIDToNeighbors_offsets = this.layer.UnsafeTileIDToNeighbors_offsets;
					NativeArray<PlanetTile> unsafeTileIDToNeighbors_values = this.layer.UnsafeTileIDToNeighbors_values;
					int num2 = 0;
					if (unsafeTileIDToNeighbors_offsets.IsCreated)
					{
						this.openSet.Clear();
						if (rootTile.Valid)
						{
							this.visited.Add(rootTile);
							this.traversalDistance[rootTile.tileId] = 0;
							this.openSet.Enqueue(new ValueTuple<PlanetTile, PlanetTile>(PlanetTile.Invalid, rootTile));
						}
						if (extraRootTiles == null)
						{
							goto IL_32D;
						}
						foreach (PlanetTile item in extraRootTiles)
						{
							this.visited.Add(item);
						}
						IList<PlanetTile> list = extraRootTiles as IList<PlanetTile>;
						if (list != null)
						{
							for (int j = 0; j < list.Count; j++)
							{
								PlanetTile planetTile = list[j];
								this.traversalDistance[planetTile.tileId] = 0;
								this.openSet.Enqueue(new ValueTuple<PlanetTile, PlanetTile>(PlanetTile.Invalid, planetTile));
							}
							goto IL_32D;
						}
						using (IEnumerator<PlanetTile> enumerator = extraRootTiles.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								PlanetTile planetTile2 = enumerator.Current;
								this.traversalDistance[planetTile2.tileId] = 0;
								this.openSet.Enqueue(new ValueTuple<PlanetTile, PlanetTile>(PlanetTile.Invalid, planetTile2));
							}
							goto IL_32D;
						}
						IL_201:
						ValueTuple<PlanetTile, PlanetTile> valueTuple = this.openSet.Dequeue();
						PlanetTile item2 = valueTuple.Item1;
						PlanetTile item3 = valueTuple.Item2;
						int num3 = this.traversalDistance[item3.tileId];
						if (processor(item2, item3, num3))
						{
							goto IL_33E;
						}
						num2++;
						if (num2 == maxTilesToProcess)
						{
							goto IL_33E;
						}
						ValueTuple<int, int> listIndexes = PackedListOfLists.GetListIndexes<PlanetTile>(unsafeTileIDToNeighbors_offsets, unsafeTileIDToNeighbors_values, item3);
						int item4 = listIndexes.Item1;
						int item5 = listIndexes.Item2;
						for (int k = item4; k < item5; k++)
						{
							PlanetTile planetTile3 = unsafeTileIDToNeighbors_values[k];
							if (this.traversalDistance[planetTile3.tileId] == -1 && passCheck(planetTile3))
							{
								this.visited.Add(planetTile3);
								this.openSet.Enqueue(new ValueTuple<PlanetTile, PlanetTile>(item3, planetTile3));
								this.traversalDistance[planetTile3.tileId] = num3 + 1;
							}
						}
						if (this.openSet.Count > num)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(106, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Overflow on world flood fill (>");
							defaultInterpolatedStringHandler.AppendFormatted<int>(num);
							defaultInterpolatedStringHandler.AppendLiteral(" cells). Make sure we're not flooding over the same area after we check it.");
							Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
							this.working = false;
							return;
						}
						IL_32D:
						if (this.openSet.Count > 0)
						{
							goto IL_201;
						}
						IL_33E:
						this.working = false;
					}
				}
			}
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0002D004 File Offset: 0x0002B204
		private void ClearVisited()
		{
			int i = 0;
			int count = this.visited.Count;
			while (i < count)
			{
				this.traversalDistance[this.visited[i].tileId] = -1;
				i++;
			}
			this.visited.Clear();
			this.openSet.Clear();
		}

		// Token: 0x04000375 RID: 885
		private bool working;

		// Token: 0x04000376 RID: 886
		private PlanetLayer layer;

		// Token: 0x04000377 RID: 887
		[TupleElementNames(new string[]
		{
			"from",
			"to"
		})]
		private readonly Queue<ValueTuple<PlanetTile, PlanetTile>> openSet = new Queue<ValueTuple<PlanetTile, PlanetTile>>();

		// Token: 0x04000378 RID: 888
		private readonly List<int> traversalDistance = new List<int>();

		// Token: 0x04000379 RID: 889
		private readonly List<PlanetTile> visited = new List<PlanetTile>();

		// Token: 0x020024F1 RID: 9457
		// (Invoke) Token: 0x0600EC2B RID: 60459
		public delegate bool WorldFillPredicate(PlanetTile from, PlanetTile to, int distance);
	}
}
