using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002049 RID: 8265
	public class PlaceWorker_ShowTradeBeaconRadius : PlaceWorker
	{
		// Token: 0x0600D0F0 RID: 53488 RVA: 0x004CD880 File Offset: 0x004CBA80
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			GenDraw.DrawFieldEdges(Building_OrbitalTradeBeacon.TradeableCellsAround(center, currentMap), 2900);
		}
	}
}
