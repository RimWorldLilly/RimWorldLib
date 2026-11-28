using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E2 RID: 5346
	public class PawnsArrivalModeWorker_EdgeDropGroups : PawnsArrivalModeWorker
	{
		// Token: 0x060082B6 RID: 33462 RVA: 0x002EEA2C File Offset: 0x002ECC2C
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			bool canRoofPunch = parms.faction != null && parms.faction.HostileTo(Faction.OfPlayer);
			List<Pair<List<Pawn>, IntVec3>> list = PawnsArrivalModeWorkerUtility.SplitIntoRandomGroupsNearMapEdge(pawns, map, true);
			PawnsArrivalModeWorkerUtility.SetPawnGroupsInfo(parms, list);
			for (int i = 0; i < list.Count; i++)
			{
				DropPodUtility.DropThingsNear(list[i].Second, map, list[i].First.Cast<Thing>(), parms.podOpenDelay, false, true, canRoofPunch, true, true, parms.faction);
			}
		}

		// Token: 0x060082B7 RID: 33463 RVA: 0x002EEABE File Offset: 0x002ECCBE
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
