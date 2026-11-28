using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002043 RID: 8259
	public class PlaceWorker_Vent : PlaceWorker
	{
		// Token: 0x0600D0E0 RID: 53472 RVA: 0x004CD2AC File Offset: 0x004CB4AC
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			IntVec3 intVec = center + IntVec3.South.RotatedBy(rot);
			IntVec3 intVec2 = center + IntVec3.North.RotatedBy(rot);
			GenDraw.DrawFieldEdges(new List<IntVec3>
			{
				intVec
			}, Color.white, null, null, 2900);
			GenDraw.DrawFieldEdges(new List<IntVec3>
			{
				intVec2
			}, Color.white, null, null, 2900);
			Room room = intVec2.GetRoom(currentMap);
			Room room2 = intVec.GetRoom(currentMap);
			if (room != null && room2 != null)
			{
				if (room == room2 && !room.UsesOutdoorTemperature)
				{
					GenDraw.DrawFieldEdges(room.Cells.ToList<IntVec3>(), Color.white, null, null, 2900);
					return;
				}
				if (!room.UsesOutdoorTemperature)
				{
					GenDraw.DrawFieldEdges(room.Cells.ToList<IntVec3>(), Color.white, null, null, 2900);
				}
				if (!room2.UsesOutdoorTemperature)
				{
					GenDraw.DrawFieldEdges(room2.Cells.ToList<IntVec3>(), Color.white, null, null, 2900);
				}
			}
		}

		// Token: 0x0600D0E1 RID: 53473 RVA: 0x004CD3DC File Offset: 0x004CB5DC
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			IntVec3 c = center + IntVec3.South.RotatedBy(rot);
			IntVec3 c2 = center + IntVec3.North.RotatedBy(rot);
			if (c.Impassable(map) || c2.Impassable(map))
			{
				return "MustPlaceVentWithFreeSpaces".Translate();
			}
			return true;
		}
	}
}
