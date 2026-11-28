using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200203C RID: 8252
	public class PlaceWorker_ShowInstrumentAoE : PlaceWorker
	{
		// Token: 0x0600D0CE RID: 53454 RVA: 0x004CCD00 File Offset: 0x004CAF00
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			ThingDef thingDef = (ThingDef)checkingDef;
			PlaceWorker_ShowInstrumentAoE.tmpCells.Clear();
			int num = GenRadial.NumCellsInRadius(thingDef.building.instrumentRange);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = loc + GenRadial.RadialPattern[i];
				if (Building_MusicalInstrument.IsAffectedByInstrument(thingDef, loc, intVec, map))
				{
					PlaceWorker_ShowInstrumentAoE.tmpCells.Add(intVec);
				}
			}
			GenDraw.DrawFieldEdges(PlaceWorker_ShowInstrumentAoE.tmpCells, 2900);
			return true;
		}

		// Token: 0x040084D5 RID: 34005
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
