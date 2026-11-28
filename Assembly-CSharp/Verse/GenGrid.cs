using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006EC RID: 1772
	public static class GenGrid
	{
		// Token: 0x0600385C RID: 14428 RVA: 0x001695D8 File Offset: 0x001677D8
		public static bool InNoBuildEdgeArea(this IntVec3 c, Map map)
		{
			return !map.IsPocketMap && !map.Tile.LayerDef.ignoreNoBuildArea && c.CloseToEdge(map, 10);
		}

		// Token: 0x0600385D RID: 14429 RVA: 0x0016960F File Offset: 0x0016780F
		public static bool InNoZoneEdgeArea(this IntVec3 c, Map map)
		{
			return c.CloseToEdge(map, 5);
		}

		// Token: 0x0600385E RID: 14430 RVA: 0x0016961C File Offset: 0x0016781C
		public static bool CloseToEdge(this IntVec3 c, Map map, int edgeDist)
		{
			IntVec3 size = map.Size;
			return c.x < edgeDist || c.z < edgeDist || c.x >= size.x - edgeDist || c.z >= size.z - edgeDist;
		}

		// Token: 0x0600385F RID: 14431 RVA: 0x00169668 File Offset: 0x00167868
		public static bool OnEdge(this IntVec3 c, Map map)
		{
			IntVec3 size = map.Size;
			return c.x == 0 || c.x == size.x - 1 || c.z == 0 || c.z == size.z - 1;
		}

		// Token: 0x06003860 RID: 14432 RVA: 0x001696B0 File Offset: 0x001678B0
		public static bool OnEdge(this IntVec3 c, Map map, Rot4 dir)
		{
			if (dir == Rot4.North)
			{
				return c.z == 0;
			}
			if (dir == Rot4.South)
			{
				return c.z == map.Size.z - 1;
			}
			if (dir == Rot4.West)
			{
				return c.x == 0;
			}
			if (dir == Rot4.East)
			{
				return c.x == map.Size.x - 1;
			}
			Log.ErrorOnce("Invalid edge direction", 55370769);
			return false;
		}

		// Token: 0x06003861 RID: 14433 RVA: 0x00169744 File Offset: 0x00167944
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool InBounds(this IntVec3 c, Map map)
		{
			IntVec3 size = map.info.Size;
			return (ulong)c.x < (ulong)((long)size.x) && (ulong)c.z < (ulong)((long)size.z);
		}

		// Token: 0x06003862 RID: 14434 RVA: 0x00169780 File Offset: 0x00167980
		public static bool InBounds(this IntVec3 c, Map map, int contractedBy)
		{
			IntVec3 size = map.Size;
			return c.x >= contractedBy && c.z >= contractedBy && c.x < size.x - contractedBy && c.z < size.z - contractedBy;
		}

		// Token: 0x06003863 RID: 14435 RVA: 0x001697C8 File Offset: 0x001679C8
		public static bool InBounds(this Vector3 v, Map map)
		{
			IntVec3 size = map.Size;
			return v.x >= 0f && v.z >= 0f && v.x < (float)size.x && v.z < (float)size.z;
		}

		// Token: 0x06003864 RID: 14436 RVA: 0x00169818 File Offset: 0x00167A18
		public static CellRect BoundsRect(this Map map, int contractedBy = 0)
		{
			IntVec3 size = map.Size;
			return new CellRect(contractedBy, contractedBy, size.x - contractedBy * 2, size.z - contractedBy * 2);
		}

		// Token: 0x06003865 RID: 14437 RVA: 0x00169847 File Offset: 0x00167A47
		public static bool WalkableByNormal(this IntVec3 c, Map map)
		{
			return map.pathing.Normal.pathGrid.Walkable(c);
		}

		// Token: 0x06003866 RID: 14438 RVA: 0x0016985F File Offset: 0x00167A5F
		public static bool WalkableByFenceBlocked(this IntVec3 c, Map map)
		{
			return map.pathing.FenceBlocked.pathGrid.Walkable(c);
		}

		// Token: 0x06003867 RID: 14439 RVA: 0x00169877 File Offset: 0x00167A77
		public static bool WalkableBy(this IntVec3 c, Map map, Pawn pawn)
		{
			return map.pathing.For(pawn).pathGrid.Walkable(c);
		}

		// Token: 0x06003868 RID: 14440 RVA: 0x00169890 File Offset: 0x00167A90
		public static bool WalkableByAny(this IntVec3 c, Map map)
		{
			return map.pathing.Normal.pathGrid.Walkable(c) || map.pathing.FenceBlocked.pathGrid.Walkable(c);
		}

		// Token: 0x06003869 RID: 14441 RVA: 0x001698C2 File Offset: 0x00167AC2
		public static bool Walkable(this IntVec3 c, Map map)
		{
			return map.pathing.Normal.pathGrid.Walkable(c) && map.pathing.FenceBlocked.pathGrid.Walkable(c);
		}

		// Token: 0x0600386A RID: 14442 RVA: 0x001698F4 File Offset: 0x00167AF4
		public static bool Standable(this IntVec3 c, Map map)
		{
			if (!c.Walkable(map))
			{
				return false;
			}
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.passability != Traversability.Standable)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600386B RID: 14443 RVA: 0x00169940 File Offset: 0x00167B40
		public static bool StandableBy(this IntVec3 c, Map map, Pawn pawn)
		{
			if (!c.WalkableBy(map, pawn))
			{
				return false;
			}
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.passability != Traversability.Standable)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x00169990 File Offset: 0x00167B90
		public static bool StandableByAny(this IntVec3 c, Map map)
		{
			if (!c.WalkableByAny(map))
			{
				return false;
			}
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.passability != Traversability.Standable)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600386D RID: 14445 RVA: 0x001699DC File Offset: 0x00167BDC
		public static bool Impassable(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAtFast(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.passability == Traversability.Impassable)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600386E RID: 14446 RVA: 0x00169A1E File Offset: 0x00167C1E
		public static bool SupportsStructureType(this IntVec3 c, Map map, TerrainAffordanceDef surfaceType)
		{
			return c.GetAffordances(map).Contains(surfaceType);
		}

		// Token: 0x0600386F RID: 14447 RVA: 0x00169A30 File Offset: 0x00167C30
		public static bool CanBeSeenOver(this IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return false;
			}
			Building edifice = c.GetEdifice(map);
			return edifice == null || edifice.CanBeSeenOver();
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x00169A60 File Offset: 0x00167C60
		public static bool CanBeSeenOverFast(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice == null || edifice.CanBeSeenOver();
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x00169A84 File Offset: 0x00167C84
		public static bool CanBeSeenOver(this Building b)
		{
			if (b.def.Fillage != FillCategory.Full)
			{
				return true;
			}
			Building_Door building_Door = b as Building_Door;
			return building_Door != null && building_Door.Open;
		}

		// Token: 0x06003872 RID: 14450 RVA: 0x00169AB8 File Offset: 0x00167CB8
		public static SurfaceType GetSurfaceType(this IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return SurfaceType.None;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.surfaceType != SurfaceType.None)
				{
					return thingList[i].def.surfaceType;
				}
			}
			return SurfaceType.None;
		}

		// Token: 0x06003873 RID: 14451 RVA: 0x00169B0F File Offset: 0x00167D0F
		public static bool HasEatSurface(this IntVec3 c, Map map)
		{
			return c.GetSurfaceType(map) == SurfaceType.Eat;
		}

		// Token: 0x06003874 RID: 14452 RVA: 0x00169B1C File Offset: 0x00167D1C
		public static void Fit(this Camera camera, CellRect bounds, float yPos)
		{
			camera.transform.position = bounds.CenterVector3 + Vector3.up * yPos;
			camera.aspect = (float)bounds.Width / (float)bounds.Height;
			camera.orthographicSize = (float)bounds.Height / 2f;
		}

		// Token: 0x040024CB RID: 9419
		public const int NoBuildEdgeWidth = 10;

		// Token: 0x040024CC RID: 9420
		public const int NoZoneEdgeWidth = 5;
	}
}
