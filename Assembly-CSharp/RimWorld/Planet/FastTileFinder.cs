using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LudeonTK;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022DF RID: 8927
	public class FastTileFinder : IDisposable
	{
		// Token: 0x1700264B RID: 9803
		// (get) Token: 0x0600DC39 RID: 56377 RVA: 0x00520C20 File Offset: 0x0051EE20
		public PlanetLayer Layer { get; }

		// Token: 0x1700264C RID: 9804
		// (get) Token: 0x0600DC3A RID: 56378 RVA: 0x00520C28 File Offset: 0x0051EE28
		public int MaxResults
		{
			get
			{
				return math.min(this.Layer.TilesCount, 50);
			}
		}

		// Token: 0x0600DC3B RID: 56379 RVA: 0x00520C3C File Offset: 0x0051EE3C
		public FastTileFinder(PlanetLayer layer)
		{
			this.Layer = layer;
			this.tmpNormalResults = new NativeArray<PlanetTile>(this.MaxResults, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.tmpDesperateResults = new NativeArray<PlanetTile>(this.MaxResults, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.cachedTileData = new NativeArray<FastTileFinder.CachedTileData>(layer.TilesCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x0600DC3C RID: 56380 RVA: 0x00520CAC File Offset: 0x0051EEAC
		public void Dispose()
		{
			ref this.tmpNormalResults.EnsureDisposed<PlanetTile>();
			ref this.tmpDesperateResults.EnsureDisposed<PlanetTile>();
			ref this.cachedTileData.EnsureDisposed<FastTileFinder.CachedTileData>();
		}

		// Token: 0x0600DC3D RID: 56381 RVA: 0x00520CCF File Offset: 0x0051EECF
		public void DirtyCache()
		{
			this.cacheDirty = true;
		}

		// Token: 0x0600DC3E RID: 56382 RVA: 0x00520CD8 File Offset: 0x0051EED8
		public void DirtyTile(PlanetTile tile)
		{
			if (this.cacheDirty)
			{
				return;
			}
			this.tileChangedSinceQuery = true;
			this.cachedTileData[tile.tileId] = new FastTileFinder.CachedTileData(tile.Tile);
		}

		// Token: 0x0600DC3F RID: 56383 RVA: 0x00520D08 File Offset: 0x0051EF08
		public List<PlanetTile> Query(FastTileFinder.TileQueryParams query, List<BiomeDef> biomes = null, List<LandmarkDef> landmarks = null, FastTileFinder.TileQueryParams desperate = default(FastTileFinder.TileQueryParams))
		{
			if (this.cacheDirty)
			{
				this.RegenerateCache();
			}
			else if (!this.tileChangedSinceQuery && this.lastNormalQuery == query && this.lastDesperateQuery == desperate)
			{
				return this.tmpOutputResults;
			}
			NativeArray<ushort> nBiomes;
			NativeArray<ushort> nLandmarks;
			this.PrepareBuffers(biomes, landmarks, out nBiomes, out nLandmarks);
			using (ProfilerBlock.Scope("Compute Tile Query"))
			{
				this.ParameterizeJob(query, desperate, nBiomes, nLandmarks).Schedule(this.cachedTileData.Length, UnityData.GetIdealBatchCount(this.cachedTileData.Length), default(JobHandle)).Complete();
			}
			nBiomes.Dispose();
			nLandmarks.Dispose();
			this.lastNormalQuery = query;
			this.lastDesperateQuery = desperate;
			this.tileChangedSinceQuery = false;
			if (this.tmpNormalResults.Length > 0)
			{
				NativeArrayUtility.CopyArrayToList<PlanetTile>(this.tmpOutputResults, this.tmpNormalResults);
				FastTileFinder.RemoveInvalidTiles(this.tmpOutputResults);
			}
			if (this.tmpOutputResults.Count == 0 && this.tmpDesperateResults.Length > 0)
			{
				NativeArrayUtility.CopyArrayToList<PlanetTile>(this.tmpOutputResults, this.tmpDesperateResults);
				FastTileFinder.RemoveInvalidTiles(this.tmpOutputResults);
			}
			return this.tmpOutputResults;
		}

		// Token: 0x0600DC40 RID: 56384 RVA: 0x00520E50 File Offset: 0x0051F050
		public bool Closest(FastTileFinder.TileQueryParams query, out PlanetTile tile, List<BiomeDef> biomes = null, List<LandmarkDef> landmarks = null, FastTileFinder.TileQueryParams desperate = default(FastTileFinder.TileQueryParams))
		{
			if (this.cacheDirty)
			{
				this.RegenerateCache();
			}
			else if (!this.tileChangedSinceQuery && this.lastClosestNormalQuery == query && this.lastClosestDesperateQuery == desperate)
			{
				tile = this.tmpOutputClosest;
				return tile.Valid;
			}
			NativeArray<ushort> nBiomes;
			NativeArray<ushort> nLandmarks;
			this.PrepareBuffers(biomes, landmarks, out nBiomes, out nLandmarks);
			using (ProfilerBlock.Scope("Compute Tile Query"))
			{
				FastTileFinder.ComputeQueryJob jobData = this.ParameterizeJob(query, desperate, nBiomes, nLandmarks);
				jobData.closest = true;
				jobData.Schedule(this.cachedTileData.Length, UnityData.GetIdealBatchCount(this.cachedTileData.Length), default(JobHandle)).Complete();
			}
			nBiomes.Dispose();
			nLandmarks.Dispose();
			this.lastClosestNormalQuery = query;
			this.lastClosestDesperateQuery = desperate;
			this.tileChangedSinceQuery = false;
			if (this.TryGetClosest(out tile))
			{
				this.tmpOutputClosest = tile;
				return true;
			}
			this.tmpOutputClosest = (tile = PlanetTile.Invalid);
			return false;
		}

		// Token: 0x0600DC41 RID: 56385 RVA: 0x00520F78 File Offset: 0x0051F178
		private bool TryGetClosest(out PlanetTile closest)
		{
			int num = -1;
			float num2 = float.MaxValue;
			for (int i = 0; i < ThreadResultPool.closestSqrDists.Length; i++)
			{
				if (ThreadResultPool.closestSqrDists[i] < num2)
				{
					num = ThreadResultPool.closestResults[i];
					num2 = ThreadResultPool.closestSqrDists[i];
				}
			}
			if (num == -1)
			{
				closest = PlanetTile.Invalid;
				return false;
			}
			closest = new PlanetTile(num, this.Layer);
			return true;
		}

		// Token: 0x0600DC42 RID: 56386 RVA: 0x00520FEC File Offset: 0x0051F1EC
		private FastTileFinder.ComputeQueryJob ParameterizeJob(FastTileFinder.TileQueryParams query, FastTileFinder.TileQueryParams desperate, NativeArray<ushort> nBiomes, NativeArray<ushort> nLandmarks)
		{
			PlanetTile origin = query.origin;
			int region = (origin.Valid && origin.Layer == this.Layer) ? this.cachedTileData[origin.tileId].regionId : -1;
			return new FastTileFinder.ComputeQueryJob
			{
				tiles = this.cachedTileData,
				normalResult = this.tmpNormalResults,
				desperateResult = this.tmpDesperateResults,
				closestResults = ThreadResultPool.closestResults,
				closestSqrDists = ThreadResultPool.closestSqrDists,
				biomes = nBiomes,
				landmarks = nLandmarks,
				normal = query,
				desperate = desperate,
				layerId = this.Layer.LayerID,
				layerRadius = this.Layer.Radius,
				averageTileSize = this.Layer.AverageTileSize,
				maximum = this.MaxResults,
				countNormal = 0,
				countDesperate = 0,
				vOrigin = origin.Layer.GetTileCenter(origin),
				region = region
			};
		}

		// Token: 0x0600DC43 RID: 56387 RVA: 0x00521110 File Offset: 0x0051F310
		private void PrepareBuffers(List<BiomeDef> biomes, List<LandmarkDef> landmarks, out NativeArray<ushort> nBiomes, out NativeArray<ushort> nLandmarks)
		{
			this.ResetNativeCache();
			ThreadResultPool.EnsureReady();
			nBiomes = new NativeArray<ushort>(biomes.NullOrEmpty<BiomeDef>() ? 0 : biomes.Count, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			nLandmarks = new NativeArray<ushort>(landmarks.NullOrEmpty<LandmarkDef>() ? 0 : landmarks.Count, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			if (!biomes.NullOrEmpty<BiomeDef>())
			{
				for (int i = 0; i < biomes.Count; i++)
				{
					nBiomes[i] = biomes[i].shortHash;
				}
			}
			if (!landmarks.NullOrEmpty<LandmarkDef>())
			{
				for (int j = 0; j < landmarks.Count; j++)
				{
					nLandmarks[j] = landmarks[j].shortHash;
				}
			}
		}

		// Token: 0x0600DC44 RID: 56388 RVA: 0x005211C0 File Offset: 0x0051F3C0
		private static void RemoveInvalidTiles(List<PlanetTile> tiles)
		{
			for (int i = tiles.Count - 1; i >= 0; i--)
			{
				if (!tiles[i].Valid)
				{
					tiles.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600DC45 RID: 56389 RVA: 0x005211F8 File Offset: 0x0051F3F8
		private void ResetNativeCache()
		{
			for (int i = 0; i < this.MaxResults; i++)
			{
				this.tmpNormalResults[i] = PlanetTile.Invalid;
				this.tmpDesperateResults[i] = PlanetTile.Invalid;
			}
		}

		// Token: 0x0600DC46 RID: 56390 RVA: 0x00521238 File Offset: 0x0051F438
		public void RegenerateCache()
		{
			if (Find.WorldGrid == null)
			{
				return;
			}
			this.cacheDirty = false;
			this.tileChangedSinceQuery = true;
			Parallel.For(0, this.Layer.TilesCount, delegate(int i)
			{
				this.cachedTileData[i] = new FastTileFinder.CachedTileData(this.Layer.Tiles[i]);
			});
		}

		// Token: 0x04008D37 RID: 36151
		private NativeArray<PlanetTile> tmpNormalResults;

		// Token: 0x04008D38 RID: 36152
		private NativeArray<PlanetTile> tmpDesperateResults;

		// Token: 0x04008D39 RID: 36153
		private NativeArray<FastTileFinder.CachedTileData> cachedTileData;

		// Token: 0x04008D3A RID: 36154
		private ParallelLoopResult rebuildingTask;

		// Token: 0x04008D3B RID: 36155
		private bool cacheDirty = true;

		// Token: 0x04008D3C RID: 36156
		private bool tileChangedSinceQuery;

		// Token: 0x04008D3E RID: 36158
		private FastTileFinder.TileQueryParams lastNormalQuery;

		// Token: 0x04008D3F RID: 36159
		private FastTileFinder.TileQueryParams lastDesperateQuery;

		// Token: 0x04008D40 RID: 36160
		private readonly List<PlanetTile> tmpOutputResults = new List<PlanetTile>();

		// Token: 0x04008D41 RID: 36161
		private FastTileFinder.TileQueryParams lastClosestNormalQuery;

		// Token: 0x04008D42 RID: 36162
		private FastTileFinder.TileQueryParams lastClosestDesperateQuery;

		// Token: 0x04008D43 RID: 36163
		private PlanetTile tmpOutputClosest = PlanetTile.Invalid;

		// Token: 0x02003DD7 RID: 15831
		public enum LandmarkMode
		{
			// Token: 0x0400F3BB RID: 62395
			Any,
			// Token: 0x0400F3BC RID: 62396
			Required,
			// Token: 0x0400F3BD RID: 62397
			Forbidden
		}

		// Token: 0x02003DD8 RID: 15832
		public readonly struct TileQueryParams : IEquatable<FastTileFinder.TileQueryParams>
		{
			// Token: 0x17003733 RID: 14131
			// (get) Token: 0x06015EF1 RID: 89841 RVA: 0x006DA15F File Offset: 0x006D835F
			public bool Valid { get; }

			// Token: 0x06015EF2 RID: 89842 RVA: 0x006DA168 File Offset: 0x006D8368
			public TileQueryParams(PlanetTile origin, float minDistTiles = 0f, float maxDistTiles = 3.4028235E+38f, FastTileFinder.LandmarkMode landmarkMode = FastTileFinder.LandmarkMode.Any, bool reachable = true, Hilliness minHilliness = Hilliness.Undefined, Hilliness maxHilliness = Hilliness.Undefined, bool checkBiome = true, bool validSettlement = true, bool comboLandmarks = true)
			{
				this.origin = origin;
				this.minDistTiles = minDistTiles;
				this.maxDistTiles = maxDistTiles;
				this.landmarkMode = landmarkMode;
				this.reachable = reachable;
				this.minHilliness = minHilliness;
				this.maxHilliness = maxHilliness;
				this.checkBiome = checkBiome;
				this.validSettlement = validSettlement;
				this.comboLandmarks = comboLandmarks;
				this.Valid = 1;
			}

			// Token: 0x06015EF3 RID: 89843 RVA: 0x006DA1C9 File Offset: 0x006D83C9
			public static bool operator ==(FastTileFinder.TileQueryParams lhs, FastTileFinder.TileQueryParams rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06015EF4 RID: 89844 RVA: 0x006DA1D3 File Offset: 0x006D83D3
			public static bool operator !=(FastTileFinder.TileQueryParams lhs, FastTileFinder.TileQueryParams rhs)
			{
				return !lhs.Equals(rhs);
			}

			// Token: 0x06015EF5 RID: 89845 RVA: 0x006DA1E0 File Offset: 0x006D83E0
			public bool Equals(FastTileFinder.TileQueryParams other)
			{
				return this.origin.Equals(other.origin) && this.landmarkMode == other.landmarkMode && this.reachable == other.reachable && this.minHilliness == other.minHilliness && this.maxHilliness == other.maxHilliness && this.checkBiome == other.checkBiome && this.validSettlement == other.validSettlement && this.comboLandmarks == other.comboLandmarks && math.abs(this.minDistTiles - other.minDistTiles) < 0.01f && math.abs(this.maxDistTiles - other.maxDistTiles) < 0.01f;
			}

			// Token: 0x06015EF6 RID: 89846 RVA: 0x006DA29C File Offset: 0x006D849C
			public override bool Equals(object obj)
			{
				if (obj is FastTileFinder.TileQueryParams)
				{
					FastTileFinder.TileQueryParams other = (FastTileFinder.TileQueryParams)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x06015EF7 RID: 89847 RVA: 0x006DA2C4 File Offset: 0x006D84C4
			public override int GetHashCode()
			{
				int value = HashCode.Combine<PlanetTile, FastTileFinder.LandmarkMode, bool, Hilliness, Hilliness, bool>(this.origin, this.landmarkMode, this.reachable, this.minHilliness, this.maxHilliness, this.checkBiome);
				int value2 = HashCode.Combine<bool, float, float, bool>(this.validSettlement, this.minDistTiles, this.maxDistTiles, this.comboLandmarks);
				return HashCode.Combine<int, int>(value, value2);
			}

			// Token: 0x0400F3BE RID: 62398
			public readonly PlanetTile origin;

			// Token: 0x0400F3BF RID: 62399
			public readonly float minDistTiles;

			// Token: 0x0400F3C0 RID: 62400
			public readonly float maxDistTiles;

			// Token: 0x0400F3C1 RID: 62401
			public readonly FastTileFinder.LandmarkMode landmarkMode;

			// Token: 0x0400F3C2 RID: 62402
			public readonly bool reachable;

			// Token: 0x0400F3C3 RID: 62403
			public readonly bool checkBiome;

			// Token: 0x0400F3C4 RID: 62404
			public readonly bool comboLandmarks;

			// Token: 0x0400F3C5 RID: 62405
			public readonly bool validSettlement;

			// Token: 0x0400F3C6 RID: 62406
			public readonly Hilliness minHilliness;

			// Token: 0x0400F3C7 RID: 62407
			public readonly Hilliness maxHilliness;
		}

		// Token: 0x02003DD9 RID: 15833
		private readonly struct CachedTileData : IEquatable<FastTileFinder.CachedTileData>
		{
			// Token: 0x17003734 RID: 14132
			// (get) Token: 0x06015EF8 RID: 89848 RVA: 0x006DA31E File Offset: 0x006D851E
			public bool Valid { get; }

			// Token: 0x06015EF9 RID: 89849 RVA: 0x006DA328 File Offset: 0x006D8528
			public CachedTileData(Tile tile)
			{
				this.center = tile.Layer.GetTileCenter(tile.tile);
				this.regionId = Find.WorldReachability.GetLocalFieldId(tile.tile);
				this.passable = Find.WorldPathGrid.PassableFast(tile.tile);
				this.landmark = (tile.Landmark != null);
				this.biomeHash = tile.PrimaryBiome.shortHash;
				Landmark landmark = tile.Landmark;
				this.landmarkHash = ((landmark != null) ? landmark.def.shortHash : 0);
				this.hilliness = tile.hilliness;
				this.validForSettlement = TileFinder.IsValidTileForNewSettlement(tile.tile, null, false);
				Landmark landmark2 = tile.Landmark;
				this.comboLandmark = (landmark2 != null && landmark2.isComboLandmark);
				this.Valid = 1;
			}

			// Token: 0x06015EFA RID: 89850 RVA: 0x006DA3F8 File Offset: 0x006D85F8
			public bool Equals(FastTileFinder.CachedTileData other)
			{
				return this.biomeHash == other.biomeHash && this.landmarkHash == other.landmarkHash && this.regionId == other.regionId && this.landmark == other.landmark && this.validForSettlement == other.validForSettlement && this.center.Equals(other.center) && this.hilliness == other.hilliness && this.passable == other.passable;
			}

			// Token: 0x06015EFB RID: 89851 RVA: 0x006DA480 File Offset: 0x006D8680
			public override bool Equals(object obj)
			{
				if (obj is FastTileFinder.CachedTileData)
				{
					FastTileFinder.CachedTileData other = (FastTileFinder.CachedTileData)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x06015EFC RID: 89852 RVA: 0x006DA4A5 File Offset: 0x006D86A5
			public override int GetHashCode()
			{
				return HashCode.Combine<ushort, ushort, int, bool, bool, float3, int, bool>(this.biomeHash, this.landmarkHash, this.regionId, this.landmark, this.validForSettlement, this.center, (int)this.hilliness, this.passable);
			}

			// Token: 0x0400F3C9 RID: 62409
			public readonly ushort biomeHash;

			// Token: 0x0400F3CA RID: 62410
			public readonly ushort landmarkHash;

			// Token: 0x0400F3CB RID: 62411
			public readonly int regionId;

			// Token: 0x0400F3CC RID: 62412
			public readonly bool landmark;

			// Token: 0x0400F3CD RID: 62413
			public readonly bool validForSettlement;

			// Token: 0x0400F3CE RID: 62414
			public readonly bool comboLandmark;

			// Token: 0x0400F3CF RID: 62415
			public readonly float3 center;

			// Token: 0x0400F3D0 RID: 62416
			public readonly Hilliness hilliness;

			// Token: 0x0400F3D1 RID: 62417
			public readonly bool passable;
		}

		// Token: 0x02003DDA RID: 15834
		[BurstCompile]
		private struct ComputeQueryJob : IJobParallelFor
		{
			// Token: 0x06015EFD RID: 89853 RVA: 0x006DA4DC File Offset: 0x006D86DC
			[BurstCompile]
			public void Execute(int i)
			{
				FastTileFinder.CachedTileData cachedTileData = this.tiles[i];
				if (this.countNormal > this.maximum && !this.closestResults.IsCreated)
				{
					return;
				}
				if (this.IsValidQuery(this.normal, cachedTileData))
				{
					int num = Interlocked.Increment(ref this.countNormal);
					if (num <= this.maximum)
					{
						this.normalResult[num - 1] = new PlanetTile(i, this.layerId);
					}
					if (this.closest)
					{
						this.CheckClosest(i);
					}
				}
				if (this.countDesperate > this.maximum || !this.desperate.Valid)
				{
					return;
				}
				if (this.IsValidQuery(this.desperate, cachedTileData))
				{
					int num2 = Interlocked.Increment(ref this.countDesperate);
					if (num2 <= this.maximum)
					{
						this.desperateResult[num2 - 1] = new PlanetTile(i, this.layerId);
					}
					if (this.closest)
					{
						this.CheckClosest(i);
					}
				}
			}

			// Token: 0x06015EFE RID: 89854 RVA: 0x006DA5CC File Offset: 0x006D87CC
			private void CheckClosest(int index)
			{
				float num = math.distancesq(this.tiles[index].center, this.vOrigin);
				if (this.closestResults[this.threadIndex] == -1 || num < this.closestSqrDists[this.threadIndex])
				{
					this.closestResults[this.threadIndex] = index;
					this.closestSqrDists[this.threadIndex] = num;
				}
			}

			// Token: 0x06015EFF RID: 89855 RVA: 0x006DA644 File Offset: 0x006D8844
			private bool IsValidQuery(in FastTileFinder.TileQueryParams query, in FastTileFinder.CachedTileData tile)
			{
				return (!query.reachable || this.region < 0 || tile.regionId == this.region) && (query.minHilliness == Hilliness.Undefined || tile.hilliness >= query.minHilliness) && (query.maxHilliness == Hilliness.Undefined || tile.hilliness <= query.maxHilliness) && (!query.validSettlement || tile.validForSettlement) && this.IsValidLandmark(query, tile) && (!query.checkBiome || this.IsValidBiome(tile)) && this.IsValidDistance(query, tile);
			}

			// Token: 0x06015F00 RID: 89856 RVA: 0x006DA6E0 File Offset: 0x006D88E0
			private bool IsValidDistance(in FastTileFinder.TileQueryParams query, in FastTileFinder.CachedTileData tile)
			{
				float num = FastTileFinder.ComputeQueryJob.SphericalDistance(this.vOrigin, tile.center);
				num = num * this.layerRadius / this.averageTileSize;
				return num >= query.minDistTiles && num <= query.maxDistTiles;
			}

			// Token: 0x06015F01 RID: 89857 RVA: 0x006DA728 File Offset: 0x006D8928
			private bool IsValidLandmark(in FastTileFinder.TileQueryParams query, in FastTileFinder.CachedTileData tile)
			{
				if (query.landmarkMode == FastTileFinder.LandmarkMode.Any)
				{
					return true;
				}
				if (!query.comboLandmarks && tile.comboLandmark)
				{
					return false;
				}
				if (this.landmarks.Length > 0)
				{
					bool flag = false;
					for (int i = 0; i < this.landmarks.Length; i++)
					{
						if (this.landmarks[i] == tile.landmarkHash)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				return tile.landmark == (query.landmarkMode == FastTileFinder.LandmarkMode.Required);
			}

			// Token: 0x06015F02 RID: 89858 RVA: 0x006DA7A8 File Offset: 0x006D89A8
			private bool IsValidBiome(in FastTileFinder.CachedTileData tile)
			{
				if (this.biomes.Length <= 0)
				{
					return true;
				}
				bool result = false;
				for (int i = 0; i < this.biomes.Length; i++)
				{
					if (this.biomes[i] == tile.biomeHash)
					{
						result = true;
						break;
					}
				}
				return result;
			}

			// Token: 0x06015F03 RID: 89859 RVA: 0x006DA7F8 File Offset: 0x006D89F8
			[BurstCompile]
			private static float SphericalDistance(in float3 a, in float3 b)
			{
				if (math.lengthsq(a - b) < 0.0001f)
				{
					return 0f;
				}
				return math.acos(math.dot(math.normalize(a), math.normalize(b)));
			}

			// Token: 0x0400F3D3 RID: 62419
			[NativeSetThreadIndex]
			[ReadOnly]
			private int threadIndex;

			// Token: 0x0400F3D4 RID: 62420
			[ReadOnly]
			public NativeArray<FastTileFinder.CachedTileData> tiles;

			// Token: 0x0400F3D5 RID: 62421
			[ReadOnly]
			public NativeArray<ushort> biomes;

			// Token: 0x0400F3D6 RID: 62422
			[ReadOnly]
			public NativeArray<ushort> landmarks;

			// Token: 0x0400F3D7 RID: 62423
			[ReadOnly]
			public FastTileFinder.TileQueryParams normal;

			// Token: 0x0400F3D8 RID: 62424
			[ReadOnly]
			public FastTileFinder.TileQueryParams desperate;

			// Token: 0x0400F3D9 RID: 62425
			[ReadOnly]
			public int layerId;

			// Token: 0x0400F3DA RID: 62426
			[ReadOnly]
			public float averageTileSize;

			// Token: 0x0400F3DB RID: 62427
			[ReadOnly]
			public float layerRadius;

			// Token: 0x0400F3DC RID: 62428
			[ReadOnly]
			public float3 vOrigin;

			// Token: 0x0400F3DD RID: 62429
			[ReadOnly]
			public int region;

			// Token: 0x0400F3DE RID: 62430
			[ReadOnly]
			public bool closest;

			// Token: 0x0400F3DF RID: 62431
			public int countNormal;

			// Token: 0x0400F3E0 RID: 62432
			public int countDesperate;

			// Token: 0x0400F3E1 RID: 62433
			[ReadOnly]
			public int maximum;

			// Token: 0x0400F3E2 RID: 62434
			[NativeDisableParallelForRestriction]
			public NativeArray<PlanetTile> normalResult;

			// Token: 0x0400F3E3 RID: 62435
			[NativeDisableParallelForRestriction]
			public NativeArray<PlanetTile> desperateResult;

			// Token: 0x0400F3E4 RID: 62436
			[NativeDisableParallelForRestriction]
			public NativeArray<int> closestResults;

			// Token: 0x0400F3E5 RID: 62437
			[NativeDisableParallelForRestriction]
			public NativeArray<float> closestSqrDists;
		}
	}
}
