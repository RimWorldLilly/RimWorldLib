using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200204F RID: 8271
	public class PlaceWorker_NeedsFuelingPort : PlaceWorker
	{
		// Token: 0x0600D0FE RID: 53502 RVA: 0x004CDA07 File Offset: 0x004CBC07
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			if (FuelingPortUtility.FuelingPortGiverAtFuelingPortCell(center, map) == null)
			{
				return "MustPlaceNearFuelingPort".Translate();
			}
			return true;
		}

		// Token: 0x0600D0FF RID: 53503 RVA: 0x004CDA2C File Offset: 0x004CBC2C
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			List<Building> allBuildingsColonist = currentMap.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building building = allBuildingsColonist[i];
				if (building.def.building.hasFuelingPort && !Find.Selector.IsSelected(building) && FuelingPortUtility.GetFuelingPortCell(building).Standable(currentMap))
				{
					PlaceWorker_FuelingPort.DrawFuelingPortCell(building.Position, building.Rotation);
				}
			}
		}
	}
}
