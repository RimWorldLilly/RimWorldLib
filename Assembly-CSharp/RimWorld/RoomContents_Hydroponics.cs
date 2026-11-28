using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001235 RID: 4661
	public class RoomContents_Hydroponics : RoomContentsWorker
	{
		// Token: 0x060073A9 RID: 29609 RVA: 0x00287FCC File Offset: 0x002861CC
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			ThingDef hydroponicsBasin = ThingDefOf.HydroponicsBasin;
			Rot4 rot = (room.Boundary.Width >= room.Boundary.Height) ? Rot4.East : Rot4.North;
			IntVec2 intVec = hydroponicsBasin.Size;
			if (rot == Rot4.East)
			{
				intVec = intVec.Rotated();
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			if (rot == Rot4.East)
			{
				num2 = 1;
				num3 = 1;
			}
			else
			{
				num = 1;
				num4 = 1;
			}
			foreach (CellRect cellRect in room.rects)
			{
				for (int i = cellRect.minZ + 2 + num4; i <= cellRect.maxZ - intVec.z - num3; i += intVec.z + num2)
				{
					for (int j = cellRect.minX + 2 + num3; j <= cellRect.maxX - intVec.x - num4; j += intVec.x + num)
					{
						GenSpawn.Spawn(hydroponicsBasin, new IntVec3(j, 0, i), map, rot, WipeMode.Vanish);
					}
				}
			}
			base.FillRoom(map, room, faction, threatPoints);
		}
	}
}
