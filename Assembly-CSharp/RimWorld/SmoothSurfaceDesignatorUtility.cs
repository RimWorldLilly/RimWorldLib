using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C63 RID: 7267
	public static class SmoothSurfaceDesignatorUtility
	{
		// Token: 0x0600B960 RID: 47456 RVA: 0x00415DD3 File Offset: 0x00413FD3
		public static bool CanSmoothFloorUnder(Building b)
		{
			return b.def.Fillage != FillCategory.Full || b.def.passability != Traversability.Impassable;
		}

		// Token: 0x0600B961 RID: 47457 RVA: 0x00415DF8 File Offset: 0x00413FF8
		public static void Notify_BuildingSpawned(Building b)
		{
			if (!SmoothSurfaceDesignatorUtility.CanSmoothFloorUnder(b))
			{
				foreach (IntVec3 c in b.OccupiedRect())
				{
					Designation designation = b.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothFloor);
					if (designation != null)
					{
						b.Map.designationManager.RemoveDesignation(designation);
					}
				}
			}
		}

		// Token: 0x0600B962 RID: 47458 RVA: 0x00415E7C File Offset: 0x0041407C
		public static void Notify_BuildingDespawned(Building b, Map map)
		{
			foreach (IntVec3 c in b.OccupiedRect())
			{
				Designation designation = map.designationManager.DesignationAt(c, DesignationDefOf.SmoothWall);
				if (designation != null)
				{
					map.designationManager.RemoveDesignation(designation);
				}
			}
		}

		// Token: 0x0600B963 RID: 47459 RVA: 0x00415EEC File Offset: 0x004140EC
		public static bool DesignateSmoothWall(Map map, IntVec3 c)
		{
			Building edifice = c.GetEdifice(map);
			if (edifice != null && edifice.def.IsSmoothable)
			{
				if (DebugSettings.godMode)
				{
					SmoothableWallUtility.Notify_SmoothedByPawn(SmoothableWallUtility.SmoothWall(edifice, null), null);
				}
				else
				{
					map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.SmoothWall, null));
					map.designationManager.TryRemoveDesignation(c, DesignationDefOf.Mine);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600B964 RID: 47460 RVA: 0x00415F58 File Offset: 0x00414158
		public static void DesignateSmoothFloor(Map map, IntVec3 c)
		{
			if (DebugSettings.godMode)
			{
				TerrainDef smoothedTerrain = c.GetTerrain(map).smoothedTerrain;
				map.terrainGrid.SetTerrain(c, smoothedTerrain);
				FilthMaker.RemoveAllFilth(c, map);
				return;
			}
			map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.SmoothFloor, null));
		}
	}
}
