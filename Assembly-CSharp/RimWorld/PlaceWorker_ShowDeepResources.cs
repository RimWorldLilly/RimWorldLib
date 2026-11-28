using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002039 RID: 8249
	public class PlaceWorker_ShowDeepResources : PlaceWorker
	{
		// Token: 0x0600D0C8 RID: 53448 RVA: 0x004CCC18 File Offset: 0x004CAE18
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap.deepResourceGrid.AnyActiveDeepScannersOnMap())
			{
				currentMap.deepResourceGrid.MarkForDraw();
			}
		}
	}
}
