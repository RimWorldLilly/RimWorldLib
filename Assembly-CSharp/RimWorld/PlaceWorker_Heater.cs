using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002038 RID: 8248
	public class PlaceWorker_Heater : PlaceWorker
	{
		// Token: 0x0600D0C6 RID: 53446 RVA: 0x004CCBC4 File Offset: 0x004CADC4
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			Room room = center.GetRoom(currentMap);
			if (room != null && !room.UsesOutdoorTemperature)
			{
				GenDraw.DrawFieldEdges(room.Cells.ToList<IntVec3>(), GenTemperature.ColorRoomHot, null, null, 2900);
			}
		}
	}
}
