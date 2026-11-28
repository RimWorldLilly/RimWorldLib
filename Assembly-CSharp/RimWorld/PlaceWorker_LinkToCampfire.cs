using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002063 RID: 8291
	public class PlaceWorker_LinkToCampfire : PlaceWorker
	{
		// Token: 0x0600D13D RID: 53565 RVA: 0x004CEFEC File Offset: 0x004CD1EC
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GenDraw.DrawRadiusRing(center, this.range);
			List<Thing> forCell = Find.CurrentMap.listerBuldingOfDefInProximity.GetForCell(center, this.range, ThingDefOf.Campfire, null);
			for (int i = 0; i < forCell.Count; i++)
			{
				GenDraw.DrawLineBetween(GenThing.TrueCenter(center, Rot4.North, def.size, def.Altitude), forCell[i].TrueCenter(), SimpleColor.Green, 0.2f);
			}
		}

		// Token: 0x040084E4 RID: 34020
		public float range = 11.9f;
	}
}
