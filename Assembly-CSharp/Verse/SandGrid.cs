using System;
using RimWorld;
using Unity.Collections;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000287 RID: 647
	public sealed class SandGrid : IExposable, IDisposable
	{
		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06001345 RID: 4933 RVA: 0x0006D03F File Offset: 0x0006B23F
		internal NativeArray<float> DepthGrid_Unsafe
		{
			get
			{
				return this.depthGrid;
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06001346 RID: 4934 RVA: 0x0006D047 File Offset: 0x0006B247
		public float TotalDepth
		{
			get
			{
				return (float)this.totalDepth;
			}
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x0006D050 File Offset: 0x0006B250
		public SandGrid(Map map)
		{
			if (!ModLister.CheckOdyssey("sand"))
			{
				return;
			}
			this.map = map;
			this.depthGrid = new NativeArray<float>(map.cellIndices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x0006D084 File Offset: 0x0006B284
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => SandGrid.SandFloatToShort(this.GetDepth(c)), delegate(IntVec3 c, ushort val)
			{
				this.depthGrid[this.map.cellIndices.CellToIndex(c)] = SandGrid.SandShortToFloat(val);
			}, "depthGrid");
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x0006D0AE File Offset: 0x0006B2AE
		private static ushort SandFloatToShort(float depth)
		{
			depth = Mathf.Clamp(depth, 0f, 1f);
			depth *= 65535f;
			return (ushort)Mathf.RoundToInt(depth);
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x0006D0D2 File Offset: 0x0006B2D2
		private static float SandShortToFloat(ushort depth)
		{
			return (float)depth / 65535f;
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x0006D0DC File Offset: 0x0006B2DC
		private bool CanHaveSand(int ind)
		{
			Building building = this.map.edificeGrid[ind];
			if (building != null && !SandGrid.CanCoexistWithSand(building.def))
			{
				return false;
			}
			TerrainDef terrainDef = this.map.terrainGrid.TerrainAt(ind);
			return terrainDef == null || (terrainDef.holdSnowOrSand && terrainDef != TerrainDefOf.Sand && terrainDef != TerrainDefOf.SoftSand);
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x0006D13F File Offset: 0x0006B33F
		public static bool CanCoexistWithSand(ThingDef def)
		{
			return def.category != ThingCategory.Building || def.Fillage != FillCategory.Full;
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x0006D158 File Offset: 0x0006B358
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
			if (!this.CanHaveSand(num))
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

		// Token: 0x0600134E RID: 4942 RVA: 0x0006D21C File Offset: 0x0006B41C
		public void SetDepth(IntVec3 c, float newDepth)
		{
			if (!this.depthGrid.IsCreated)
			{
				return;
			}
			int num = this.map.cellIndices.CellToIndex(c);
			if (newDepth > 0f && !this.CanHaveSand(num))
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

		// Token: 0x0600134F RID: 4943 RVA: 0x0006D2AC File Offset: 0x0006B4AC
		private void CheckVisualOrPathCostChange(IntVec3 c, float oldDepth, float newDepth)
		{
			if (!Mathf.Approximately(oldDepth, newDepth))
			{
				if (Mathf.Abs(oldDepth - newDepth) > 0.15f || Rand.Value < 0.0125f)
				{
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Sand, true, false);
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Things, true, false);
				}
				else if (newDepth == 0f)
				{
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Sand, true, false);
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
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x0006D3A1 File Offset: 0x0006B5A1
		public void MakeMeshDirty(IntVec3 c)
		{
			this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Sand, true, false);
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x0006D3C0 File Offset: 0x0006B5C0
		public float GetDepth(IntVec3 c)
		{
			if (!this.depthGrid.IsCreated || !c.InBounds(this.map))
			{
				return 0f;
			}
			return this.depthGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x0006D3FF File Offset: 0x0006B5FF
		public WeatherBuildupCategory GetCategory(IntVec3 c)
		{
			return WeatherBuildupUtility.GetBuildupCategory(this.GetDepth(c));
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0006D40D File Offset: 0x0006B60D
		public void Dispose()
		{
			this.depthGrid.Dispose();
		}

		// Token: 0x040010EE RID: 4334
		private Map map;

		// Token: 0x040010EF RID: 4335
		private NativeArray<float> depthGrid;

		// Token: 0x040010F0 RID: 4336
		private double totalDepth;

		// Token: 0x040010F1 RID: 4337
		public const float MaxDepth = 1f;
	}
}
