using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002041 RID: 8257
	public class PlaceWorker_CoolerSimple : PlaceWorker
	{
		// Token: 0x0600D0DB RID: 53467 RVA: 0x004CCFD8 File Offset: 0x004CB1D8
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			Room room = center.GetRoom(currentMap);
			if (room != null && !room.UsesOutdoorTemperature)
			{
				GenDraw.DrawFieldEdges(room.Cells.ToList<IntVec3>(), GenTemperature.ColorRoomCold, null, null, 2900);
			}
		}
	}
}
