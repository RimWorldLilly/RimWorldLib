using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002037 RID: 8247
	public class PlaceWorker_WindTurbine : PlaceWorker
	{
		// Token: 0x0600D0C4 RID: 53444 RVA: 0x004CCB9D File Offset: 0x004CAD9D
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GenDraw.DrawFieldEdges(WindTurbineUtility.CalculateWindCells(center, rot, def.size).ToList<IntVec3>(), 2900);
		}
	}
}
