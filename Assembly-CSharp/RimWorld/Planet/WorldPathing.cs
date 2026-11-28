using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LudeonTK;
using Unity.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022D9 RID: 8921
	public class WorldPathing : IDisposable
	{
		// Token: 0x17002648 RID: 9800
		// (get) Token: 0x0600DC0B RID: 56331 RVA: 0x0051F1E2 File Offset: 0x0051D3E2
		public PlanetLayer Layer { get; }

		// Token: 0x0600DC0C RID: 56332 RVA: 0x0051F1EC File Offset: 0x0051D3EC
		public WorldPathing(PlanetLayer layer)
		{
			this.Layer = layer;
			this.calcGrid = new NativeArray<WorldPathing.PathFinderNodeFast>(layer.TilesCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.frontier = new NativePriorityQueue<int, int, IntMinComparer>(Mathf.Min(layer.TilesCount, 500000), default(IntMinComparer), Allocator.Persistent);
		}

		// Token: 0x0600DC0D RID: 56333 RVA: 0x0051F240 File Offset: 0x0051D440
		public WorldPath FindPath(PlanetTile startTile, PlanetTile destTile, Caravan caravan, Func<float, bool> terminator = null)
		{
			if (!startTile.Valid)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to FindPath with invalid start tile ");
				defaultInterpolatedStringHandler.AppendFormatted<PlanetTile>(startTile);
				defaultInterpolatedStringHandler.AppendLiteral(", caravan = ");
				defaultInterpolatedStringHandler.AppendFormatted<Caravan>(caravan);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return WorldPath.NotFound;
			}
			if (!destTile.Valid)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(53, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("Tried to FindPath with invalid dest tile ");
				defaultInterpolatedStringHandler2.AppendFormatted<PlanetTile>(destTile);
				defaultInterpolatedStringHandler2.AppendLiteral(", caravan = ");
				defaultInterpolatedStringHandler2.AppendFormatted<Caravan>(caravan);
				Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
				return WorldPath.NotFound;
			}
			if (startTile.Layer != destTile.Layer)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(55, 3);
				defaultInterpolatedStringHandler3.AppendLiteral("Tried to FindPath to a different layer ");
				defaultInterpolatedStringHandler3.AppendFormatted<PlanetTile>(startTile);
				defaultInterpolatedStringHandler3.AppendLiteral(" -> ");
				defaultInterpolatedStringHandler3.AppendFormatted<PlanetTile>(destTile);
				defaultInterpolatedStringHandler3.AppendLiteral(", caravan = ");
				defaultInterpolatedStringHandler3.AppendFormatted<Caravan>(caravan);
				Log.Error(defaultInterpolatedStringHandler3.ToStringAndClear());
				return WorldPath.NotFound;
			}
			if (caravan != null)
			{
				if (!caravan.CanReach(destTile))
				{
					return WorldPath.NotFound;
				}
			}
			else if (!Find.WorldReachability.CanReach(startTile, destTile))
			{
				return WorldPath.NotFound;
			}
			PlanetTile planetTile = startTile;
			World world = Find.World;
			WorldGrid grid = world.grid;
			NativeArray<int> unsafeTileIDToNeighbors_offsets = startTile.Layer.UnsafeTileIDToNeighbors_offsets;
			NativeArray<PlanetTile> unsafeTileIDToNeighbors_values = startTile.Layer.UnsafeTileIDToNeighbors_values;
			Vector3 normalized = grid.GetTileCenter(destTile).normalized;
			float[] array = world.pathGrid.layerMovementDifficulty[startTile.Layer];
			int num = 0;
			int num2 = (caravan != null) ? caravan.TicksPerMove : 3300;
			int num3 = this.CalculateHeuristicStrength(startTile, destTile);
			this.frontier.Clear();
			this.calcGrid.Clear<WorldPathing.PathFinderNodeFast>();
			this.InitalizeSearch(planetTile);
			while (this.frontier.Count > 0)
			{
				int num4;
				int num5;
				this.frontier.Dequeue(out num4, out num5);
				WorldPathing.PathFinderNodeFast pathFinderNodeFast = this.calcGrid[num4];
				if (num5 == pathFinderNodeFast.costNodeCost)
				{
					planetTile = new PlanetTile(num4, this.Layer);
					if (pathFinderNodeFast.status != WorldPathing.PathFinderNodeFast.Status.Closed)
					{
						if (planetTile == destTile)
						{
							return this.FinalizedPath(planetTile);
						}
						if (num > 500000)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(46, 4);
							defaultInterpolatedStringHandler4.AppendFormatted<Caravan>(caravan);
							defaultInterpolatedStringHandler4.AppendLiteral(" pathing from ");
							defaultInterpolatedStringHandler4.AppendFormatted<PlanetTile>(startTile);
							defaultInterpolatedStringHandler4.AppendLiteral(" to ");
							defaultInterpolatedStringHandler4.AppendFormatted<PlanetTile>(destTile);
							defaultInterpolatedStringHandler4.AppendLiteral(" hit search limit of ");
							defaultInterpolatedStringHandler4.AppendFormatted<int>(500000);
							defaultInterpolatedStringHandler4.AppendLiteral(" tiles.");
							Log.Warning(defaultInterpolatedStringHandler4.ToStringAndClear());
							return WorldPath.NotFound;
						}
						ValueTuple<int, int> listIndexes = PackedListOfLists.GetListIndexes<PlanetTile>(unsafeTileIDToNeighbors_offsets, unsafeTileIDToNeighbors_values, planetTile.tileId);
						int item = listIndexes.Item1;
						int item2 = listIndexes.Item2;
						for (int i = item; i < item2; i++)
						{
							PlanetTile planetTile2 = unsafeTileIDToNeighbors_values[i];
							WorldPathing.PathFinderNodeFast pathFinderNodeFast2 = this.calcGrid[planetTile2.tileId];
							if (pathFinderNodeFast2.status != WorldPathing.PathFinderNodeFast.Status.Closed && !world.Impassable(planetTile2))
							{
								int num6 = (int)((float)num2 * array[planetTile2.tileId] * grid.GetRoadMovementDifficultyMultiplier(planetTile, planetTile2, null)) + pathFinderNodeFast.knownCost;
								WorldPathing.PathFinderNodeFast.Status status = this.calcGrid[planetTile2.tileId].status;
								if ((status != WorldPathing.PathFinderNodeFast.Status.Closed && status != WorldPathing.PathFinderNodeFast.Status.Open) || this.calcGrid[planetTile2.tileId].knownCost > num6)
								{
									Vector3 tileCenter = grid.GetTileCenter(planetTile2);
									if (status != WorldPathing.PathFinderNodeFast.Status.Closed && status != WorldPathing.PathFinderNodeFast.Status.Open)
									{
										float num7 = grid.ApproxDistanceInTiles(GenMath.SphericalDistance(tileCenter.normalized, normalized));
										pathFinderNodeFast2.heuristicCost = Mathf.RoundToInt((float)num2 * num7 * (float)num3 * 0.5f);
									}
									int num8 = num6 + this.calcGrid[planetTile2.tileId].heuristicCost;
									pathFinderNodeFast2.parentTile = planetTile;
									pathFinderNodeFast2.knownCost = num6;
									pathFinderNodeFast2.status = WorldPathing.PathFinderNodeFast.Status.Open;
									pathFinderNodeFast2.costNodeCost = num8;
									this.calcGrid[planetTile2.tileId] = pathFinderNodeFast2;
									this.frontier.Enqueue(planetTile2.tileId, num8);
								}
							}
						}
						num++;
						pathFinderNodeFast.status = WorldPathing.PathFinderNodeFast.Status.Closed;
						this.calcGrid[num4] = pathFinderNodeFast;
						if (terminator != null && terminator((float)pathFinderNodeFast.costNodeCost))
						{
							return WorldPath.NotFound;
						}
					}
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(47, 3);
			defaultInterpolatedStringHandler5.AppendFormatted<Caravan>(caravan);
			defaultInterpolatedStringHandler5.AppendLiteral(" pathing from ");
			defaultInterpolatedStringHandler5.AppendFormatted<PlanetTile>(startTile);
			defaultInterpolatedStringHandler5.AppendLiteral(" to ");
			defaultInterpolatedStringHandler5.AppendFormatted<PlanetTile>(destTile);
			defaultInterpolatedStringHandler5.AppendLiteral(" ran out of tiles to process.");
			Log.Warning(defaultInterpolatedStringHandler5.ToStringAndClear());
			return WorldPath.NotFound;
		}

		// Token: 0x0600DC0E RID: 56334 RVA: 0x0051F6FC File Offset: 0x0051D8FC
		private void InitalizeSearch(PlanetTile tile)
		{
			WorldPathing.PathFinderNodeFast value = this.calcGrid[tile.tileId];
			value.knownCost = 0;
			value.heuristicCost = 0;
			value.costNodeCost = 0;
			value.parentTile = tile;
			value.status = WorldPathing.PathFinderNodeFast.Status.Open;
			this.calcGrid[tile.tileId] = value;
			this.frontier.Clear();
			this.frontier.Enqueue(tile.tileId, 0);
		}

		// Token: 0x0600DC0F RID: 56335 RVA: 0x0051F774 File Offset: 0x0051D974
		public void FloodPathsWithCost(List<PlanetTile> startTiles, Func<PlanetTile, PlanetTile, int> costFunc, Func<PlanetTile, bool> impassable = null, Func<PlanetTile, float, bool> terminator = null)
		{
			if (startTiles.Count < 1 || startTiles.Contains(PlanetTile.Invalid))
			{
				Log.Error("Tried to FindPath with invalid start tiles");
				return;
			}
			for (int i = 0; i < startTiles.Count; i++)
			{
				if (startTiles[i].Layer != this.Layer)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(91, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Tried to FindPath with tiles on different layers, layer is ");
					defaultInterpolatedStringHandler.AppendFormatted<PlanetLayer>(this.Layer);
					defaultInterpolatedStringHandler.AppendLiteral(" but tile at index ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral(" is on layer ");
					defaultInterpolatedStringHandler.AppendFormatted<PlanetLayer>(startTiles[i].Layer);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					return;
				}
			}
			NativeArray<int> unsafeTileIDToNeighbors_offsets = this.Layer.UnsafeTileIDToNeighbors_offsets;
			NativeArray<PlanetTile> unsafeTileIDToNeighbors_values = this.Layer.UnsafeTileIDToNeighbors_values;
			if (impassable == null)
			{
				impassable = ((PlanetTile tid) => Find.World.Impassable(tid));
			}
			this.calcGrid.Clear<WorldPathing.PathFinderNodeFast>();
			this.frontier.Clear();
			using (List<PlanetTile>.Enumerator enumerator = startTiles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PlanetTile planetTile = enumerator.Current;
					WorldPathing.PathFinderNodeFast value = this.calcGrid[planetTile.tileId];
					value.knownCost = 0;
					value.costNodeCost = 0;
					value.parentTile = planetTile;
					value.status = WorldPathing.PathFinderNodeFast.Status.Open;
					this.calcGrid[planetTile.tileId] = value;
					this.frontier.Enqueue(planetTile.tileId, 0);
				}
				goto IL_35C;
			}
			IL_192:
			int num;
			int num2;
			this.frontier.Dequeue(out num, out num2);
			WorldPathing.PathFinderNodeFast pathFinderNodeFast = this.calcGrid[num];
			if (num2 == pathFinderNodeFast.costNodeCost)
			{
				PlanetTile planetTile2 = new PlanetTile(num, this.Layer);
				if (this.calcGrid[planetTile2.tileId].status != WorldPathing.PathFinderNodeFast.Status.Closed)
				{
					ValueTuple<int, int> listIndexes = PackedListOfLists.GetListIndexes<PlanetTile>(unsafeTileIDToNeighbors_offsets, unsafeTileIDToNeighbors_values, planetTile2.tileId);
					int item = listIndexes.Item1;
					int item2 = listIndexes.Item2;
					for (int j = item; j < item2; j++)
					{
						PlanetTile planetTile3 = unsafeTileIDToNeighbors_values[j];
						WorldPathing.PathFinderNodeFast value2 = this.calcGrid[planetTile3.tileId];
						if (this.calcGrid[planetTile3.tileId].status != WorldPathing.PathFinderNodeFast.Status.Closed && !impassable(planetTile3))
						{
							int num3 = costFunc(planetTile2, planetTile3) + this.calcGrid[planetTile2.tileId].knownCost;
							WorldPathing.PathFinderNodeFast.Status status = this.calcGrid[planetTile3.tileId].status;
							if ((status != WorldPathing.PathFinderNodeFast.Status.Closed && status != WorldPathing.PathFinderNodeFast.Status.Open) || this.calcGrid[planetTile3.tileId].knownCost > num3)
							{
								int num4 = num3;
								value2.parentTile = planetTile2;
								value2.knownCost = num3;
								value2.status = WorldPathing.PathFinderNodeFast.Status.Open;
								value2.costNodeCost = num4;
								this.calcGrid[planetTile3.tileId] = value2;
								this.frontier.Enqueue(planetTile3.tileId, num4);
							}
						}
					}
					pathFinderNodeFast.status = WorldPathing.PathFinderNodeFast.Status.Closed;
					this.calcGrid[planetTile2.tileId] = pathFinderNodeFast;
					if (terminator != null && terminator(planetTile2, (float)this.calcGrid[planetTile2.tileId].costNodeCost))
					{
						return;
					}
				}
			}
			IL_35C:
			if (this.frontier.Count > 0)
			{
				goto IL_192;
			}
		}

		// Token: 0x0600DC10 RID: 56336 RVA: 0x0051FB00 File Offset: 0x0051DD00
		public List<PlanetTile>[] FloodPathsWithCostForTree(List<PlanetTile> startTiles, Func<PlanetTile, PlanetTile, int> costFunc, Func<PlanetTile, bool> impassable = null, Func<PlanetTile, float, bool> terminator = null)
		{
			this.FloodPathsWithCost(startTiles, costFunc, impassable, terminator);
			PlanetLayer layer = startTiles[0].Layer;
			List<PlanetTile>[] array = new List<PlanetTile>[layer.TilesCount];
			for (int i = 0; i < layer.TilesCount; i++)
			{
				if (this.calcGrid[i].status == WorldPathing.PathFinderNodeFast.Status.Closed)
				{
					PlanetTile planetTile = new PlanetTile(i, layer);
					PlanetTile parentTile = this.calcGrid[i].parentTile;
					if (parentTile != planetTile)
					{
						if (array[parentTile.tileId] == null)
						{
							array[parentTile.tileId] = new List<PlanetTile>();
						}
						array[parentTile.tileId].Add(planetTile);
					}
				}
			}
			return array;
		}

		// Token: 0x0600DC11 RID: 56337 RVA: 0x0051FBAC File Offset: 0x0051DDAC
		private WorldPath FinalizedPath(PlanetTile lastTile)
		{
			WorldPath emptyWorldPath = Find.WorldPathPool.GetEmptyWorldPath();
			PlanetTile planetTile = lastTile;
			for (;;)
			{
				PlanetTile parentTile = this.calcGrid[planetTile.tileId].parentTile;
				PlanetTile planetTile2 = planetTile;
				emptyWorldPath.AddNodeAtStart(planetTile2);
				if (planetTile2 == parentTile)
				{
					break;
				}
				planetTile = parentTile;
			}
			emptyWorldPath.SetupFound((float)this.calcGrid[lastTile.tileId].knownCost, lastTile.Layer);
			return emptyWorldPath;
		}

		// Token: 0x0600DC12 RID: 56338 RVA: 0x0051FC18 File Offset: 0x0051DE18
		private int CalculateHeuristicStrength(PlanetTile startTile, PlanetTile destTile)
		{
			float x = Find.WorldGrid.ApproxDistanceInTiles(startTile, destTile);
			return Mathf.RoundToInt(WorldPathing.HeuristicStrength_DistanceCurve.Evaluate(x));
		}

		// Token: 0x0600DC13 RID: 56339 RVA: 0x0051FC42 File Offset: 0x0051DE42
		public void Dispose()
		{
			ref this.calcGrid.EnsureDisposed<WorldPathing.PathFinderNodeFast>();
			ref this.frontier.EnsureDisposed<int, int, IntMinComparer>();
		}

		// Token: 0x04008D29 RID: 36137
		private NativePriorityQueue<int, int, IntMinComparer> frontier;

		// Token: 0x04008D2A RID: 36138
		private NativeArray<WorldPathing.PathFinderNodeFast> calcGrid;

		// Token: 0x04008D2B RID: 36139
		private const int SearchLimit = 500000;

		// Token: 0x04008D2C RID: 36140
		private static readonly SimpleCurve HeuristicStrength_DistanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(30f, 1f),
				true
			},
			{
				new CurvePoint(40f, 1.3f),
				true
			},
			{
				new CurvePoint(130f, 2f),
				true
			}
		};

		// Token: 0x04008D2D RID: 36141
		private const float BestRoadDiscount = 0.5f;

		// Token: 0x02003DCF RID: 15823
		private struct PathFinderNodeFast
		{
			// Token: 0x0400F394 RID: 62356
			public int knownCost;

			// Token: 0x0400F395 RID: 62357
			public int heuristicCost;

			// Token: 0x0400F396 RID: 62358
			public PlanetTile parentTile;

			// Token: 0x0400F397 RID: 62359
			public int costNodeCost;

			// Token: 0x0400F398 RID: 62360
			public WorldPathing.PathFinderNodeFast.Status status;

			// Token: 0x02003F61 RID: 16225
			public enum Status : byte
			{
				// Token: 0x0400F9DD RID: 63965
				None,
				// Token: 0x0400F9DE RID: 63966
				Open,
				// Token: 0x0400F9DF RID: 63967
				Closed
			}
		}
	}
}
