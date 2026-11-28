using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200204D RID: 8269
	public class PlaceWorker_WatchArea : PlaceWorker
	{
		// Token: 0x0600D0F8 RID: 53496 RVA: 0x004CD940 File Offset: 0x004CBB40
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			GenDraw.DrawFieldEdges(WatchBuildingUtility.CalculateWatchCells(def, center, rot, currentMap).ToList<IntVec3>(), 2900);
		}
	}
}
