using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000308 RID: 776
	public class AutoBuildRoofAreaSetter
	{
		// Token: 0x060017E2 RID: 6114 RVA: 0x0008F370 File Offset: 0x0008D570
		public AutoBuildRoofAreaSetter(Map map)
		{
			this.map = map;
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x0008F3AB File Offset: 0x0008D5AB
		public void TryGenerateAreaFor(Room room)
		{
			this.queuedGenerateRooms.Add(room);
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x0008F3B9 File Offset: 0x0008D5B9
		public void AutoBuildRoofAreaSetterTick_First()
		{
			this.ResolveQueuedGenerateRoofs();
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x0008F3C4 File Offset: 0x0008D5C4
		public void ResolveQueuedGenerateRoofs()
		{
			for (int i = 0; i < this.queuedGenerateRooms.Count; i++)
			{
				this.TryGenerateAreaNow(this.queuedGenerateRooms[i]);
			}
			this.queuedGenerateRooms.Clear();
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x0008F404 File Offset: 0x0008D604
		private void TryGenerateAreaNow(Room room)
		{
			this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
			if (room.Dereferenced || room.TouchesMapEdge)
			{
				return;
			}
			if (room.RegionCount > 26 || room.CellCount > 320)
			{
				return;
			}
			if (room.IsDoorway)
			{
				return;
			}
			bool flag = false;
			foreach (IntVec3 c in room.BorderCells)
			{
				Thing roofHolderOrImpassable = c.GetRoofHolderOrImpassable(this.map);
				if (roofHolderOrImpassable != null)
				{
					if (roofHolderOrImpassable.Faction != null && roofHolderOrImpassable.Faction != Faction.OfPlayer)
					{
						return;
					}
					if (roofHolderOrImpassable.def.building != null && !roofHolderOrImpassable.def.building.allowAutoroof)
					{
						return;
					}
					if (roofHolderOrImpassable.Faction == Faction.OfPlayer)
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return;
			}
			this.innerCells.Clear();
			foreach (IntVec3 intVec in room.Cells)
			{
				if (!this.innerCells.Contains(intVec))
				{
					this.innerCells.Add(intVec);
				}
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c2 = intVec + GenAdj.AdjacentCells[i];
					if (c2.InBounds(this.map))
					{
						Thing roofHolderOrImpassable2 = c2.GetRoofHolderOrImpassable(this.map);
						if (roofHolderOrImpassable2 != null && (roofHolderOrImpassable2.def.size.x > 1 || roofHolderOrImpassable2.def.size.z > 1))
						{
							CellRect cellRect = roofHolderOrImpassable2.OccupiedRect();
							cellRect.ClipInsideMap(this.map);
							for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
							{
								for (int k = cellRect.minX; k <= cellRect.maxX; k++)
								{
									IntVec3 item = new IntVec3(k, 0, j);
									if (!this.innerCells.Contains(item))
									{
										this.innerCells.Add(item);
									}
								}
							}
						}
					}
				}
			}
			this.cellsToRoof.Clear();
			foreach (IntVec3 a in this.innerCells)
			{
				for (int l = 0; l < 9; l++)
				{
					IntVec3 intVec2 = a + GenAdj.AdjacentCellsAndInside[l];
					if (intVec2.InBounds(this.map) && (l == 8 || intVec2.GetRoofHolderOrImpassable(this.map) != null) && !this.cellsToRoof.Contains(intVec2))
					{
						this.cellsToRoof.Add(intVec2);
					}
				}
			}
			this.justRoofedCells.Clear();
			foreach (IntVec3 intVec3 in this.cellsToRoof)
			{
				if (this.map.roofGrid.RoofAt(intVec3) == null && !this.justRoofedCells.Contains(intVec3) && !this.map.areaManager.NoRoof[intVec3] && RoofCollapseUtility.WithinRangeOfRoofHolder(intVec3, this.map, true))
				{
					this.map.areaManager.BuildRoof[intVec3] = true;
					this.justRoofedCells.Add(intVec3);
				}
			}
		}

		// Token: 0x04001421 RID: 5153
		private Map map;

		// Token: 0x04001422 RID: 5154
		private List<Room> queuedGenerateRooms = new List<Room>();

		// Token: 0x04001423 RID: 5155
		private HashSet<IntVec3> cellsToRoof = new HashSet<IntVec3>();

		// Token: 0x04001424 RID: 5156
		private HashSet<IntVec3> innerCells = new HashSet<IntVec3>();

		// Token: 0x04001425 RID: 5157
		private List<IntVec3> justRoofedCells = new List<IntVec3>();
	}
}
