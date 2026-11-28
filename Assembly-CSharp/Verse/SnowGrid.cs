using System;
using RimWorld;
using Unity.Collections;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000288 RID: 648
	public sealed class SnowGrid : IExposable, IDisposable
	{
		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06001356 RID: 4950 RVA: 0x0006D44C File Offset: 0x0006B64C
		internal NativeArray<float> DepthGrid_Unsafe
		{
			get
			{
				return this.depthGrid;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001357 RID: 4951 RVA: 0x0006D454 File Offset: 0x0006B654
		public float TotalDepth
		{
			get
			{
				return (float)this.totalDepth;
			}
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x0006D45D File Offset: 0x0006B65D
		public SnowGrid(Map map)
		{
			this.map = map;
			this.depthGrid = new NativeArray<float>(map.cellIndices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x0006D484 File Offset: 0x0006B684
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => SnowGrid.SnowFloatToShort(this.GetDepth(c)), delegate(IntVec3 c, ushort val)
			{
				this.depthGrid[this.map.cellIndices.CellToIndex(c)] = SnowGrid.SnowShortToFloat(val);
			}, "depthGrid");
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x0006D4AE File Offset: 0x0006B6AE
		private static ushort SnowFloatToShort(float depth)
		{
			depth = Mathf.Clamp(depth, 0f, 1f);
			depth *= 65535f;
			return (ushort)Mathf.RoundToInt(depth);
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x0006D4D2 File Offset: 0x0006B6D2
		private static float SnowShortToFloat(ushort depth)
		{
			return (float)depth / 65535f;
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x0006D4DC File Offset: 0x0006B6DC
		private bool CanHaveSnow(int ind)
		{
			Building building = this.map.edificeGrid[ind];
			if (building != null && !SnowGrid.CanCoexistWithSnow(building.def))
			{
				return false;
			}
			TerrainDef terrainDef = this.map.terrainGrid.TerrainAt(ind);
			return terrainDef == null || terrainDef.holdSnowOrSand;
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x0006D52D File Offset: 0x0006B72D
		public static bool CanCoexistWithSnow(ThingDef def)
		{
			return def.category != ThingCategory.Building || def.Fillage != FillCategory.Full;
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x0006D544 File Offset: 0x0006B744
		public void AddDepth(IntVec3 c, float depthToAdd)
		{
			if (!this.depthGrid.IsCreated)
			{
				return;
			}
			int num = this.map.cellIndices.CellToIndex(c);
			float num2 = this.depthGrid[num];
			if (num2 <= 0f && depthToAdd < 0f)
			{
				return;
			}
			if (num2 >= 0.999f && depthToAdd > 1f)
			{
				return;
			}
			if (!this.CanHaveSnow(num))
			{
				this.depthGrid[num] = 0f;
				return;
			}
			float num3 = num2 + depthToAdd;
			num3 = Mathf.Clamp(num3, 0f, 1f);
			float num4 = num3 - num2;
			this.totalDepth += (double)num4;
			if (Mathf.Abs(num4) > 0.0001f)
			{
				this.depthGrid[num] = num3;
				this.CheckVisualOrPathCostChange(c, num2, num3);
			}
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x0006D608 File Offset: 0x0006B808
		public void SetDepth(IntVec3 c, float newDepth)
		{
			if (!this.depthGrid.IsCreated)
			{
				return;
			}
			int num = this.map.cellIndices.CellToIndex(c);
			if (newDepth > 0f && !this.CanHaveSnow(num))
			{
				newDepth = 0f;
			}
			newDepth = Mathf.Clamp(newDepth, 0f, 1f);
			float num2 = this.depthGrid[num];
			this.depthGrid[num] = newDepth;
			float num3 = newDepth - num2;
			this.totalDepth += (double)num3;
			this.CheckVisualOrPathCostChange(c, num2, newDepth);
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x0006D698 File Offset: 0x0006B898
		private void CheckVisualOrPathCostChange(IntVec3 c, float oldDepth, float newDepth)
		{
			if (Mathf.Approximately(oldDepth, newDepth))
			{
				return;
			}
			if (Mathf.Abs(oldDepth - newDepth) > 0.15f || Rand.Value < 0.0125f)
			{
				this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Snow, true, false);
				this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Things, true, false);
			}
			else if (newDepth == 0f)
			{
				this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Snow, true, false);
			}
			float num = 0.4f;
			if (c.IsPolluted(this.map) && ((oldDepth > num && newDepth < num) || (oldDepth < num && newDepth > num)))
			{
				this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Terrain, true, false);
			}
			if (WeatherBuildupUtility.GetBuildupCategory(oldDepth) != WeatherBuildupUtility.GetBuildupCategory(newDepth))
			{
				this.map.pathing.RecalculatePerceivedPathCostAt(c);
			}
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x0006D78B File Offset: 0x0006B98B
		public void MakeMeshDirty(IntVec3 c)
		{
			this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Snow, true, false);
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x0006D7AA File Offset: 0x0006B9AA
		public float GetDepth(IntVec3 c)
		{
			if (!this.depthGrid.IsCreated || !c.InBounds(this.map))
			{
				return 0f;
			}
			return this.depthGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x0006D7E9 File Offset: 0x0006B9E9
		public WeatherBuildupCategory GetCategory(IntVec3 c)
		{
			return WeatherBuildupUtility.GetBuildupCategory(this.GetDepth(c));
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x0006D7F7 File Offset: 0x0006B9F7
		public void Dispose()
		{
			this.depthGrid.Dispose();
		}

		// Token: 0x040010F2 RID: 4338
		private Map map;

		// Token: 0x040010F3 RID: 4339
		private NativeArray<float> depthGrid;

		// Token: 0x040010F4 RID: 4340
		private double totalDepth;

		// Token: 0x040010F5 RID: 4341
		public const float MaxDepth = 1f;
	}
}
