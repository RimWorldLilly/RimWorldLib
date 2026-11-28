using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200206F RID: 8303
	public class PlaceWorker_ElectricInhibitor : PlaceWorker
	{
		// Token: 0x0600D15A RID: 53594 RVA: 0x004CF918 File Offset: 0x004CDB18
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			PlaceWorker_ElectricInhibitor.tmpCells.Clear();
			PlaceWorker_ElectricInhibitor.tmpCells.AddRange(ContainmentUtility.GetInhibitorAffectedCells(def, center, rot, Find.CurrentMap));
			GenDraw.DrawFieldEdges(PlaceWorker_ElectricInhibitor.tmpCells, Color.white, null, null, 2900);
			PlaceWorker_ElectricInhibitor.tmpCells.Clear();
		}

		// Token: 0x0600D15B RID: 53595 RVA: 0x004CF970 File Offset: 0x004CDB70
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			ThingDef thingDef = checkingDef as ThingDef;
			if (thingDef != null && ContainmentUtility.IsLinearBuildingBlocked(thingDef, loc, rot, map, thingToIgnore))
			{
				return new AcceptanceReport("InhibitorSpaceOccupied".Translate());
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x040084E7 RID: 34023
		private static readonly List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
