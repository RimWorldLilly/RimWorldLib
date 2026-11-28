using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E1 RID: 5345
	public class PawnsArrivalModeWorker_EdgeDrop : PawnsArrivalModeWorker
	{
		// Token: 0x060082B2 RID: 33458 RVA: 0x002EE993 File Offset: 0x002ECB93
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			PawnsArrivalModeWorkerUtility.DropInDropPodsNearSpawnCenter(parms, pawns);
		}

		// Token: 0x060082B3 RID: 33459 RVA: 0x002EE99C File Offset: 0x002ECB9C
		public override void TravellingTransportersArrived(List<ActiveTransporterInfo> transporters, Map map)
		{
			IntVec3 near = DropCellFinder.FindRaidDropCenterDistant(map, false, !transporters.IsShuttle());
			if (transporters.IsShuttle())
			{
				TransportersArrivalActionUtility.DropShuttle(transporters[0], map, near, null, null);
				return;
			}
			TransportersArrivalActionUtility.DropTravellingDropPods(transporters, near, map);
		}

		// Token: 0x060082B4 RID: 33460 RVA: 0x002EE9E4 File Offset: 0x002ECBE4
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!parms.spawnCenter.IsValid)
			{
				parms.spawnCenter = DropCellFinder.FindRaidDropCenterDistant(map, false, true);
			}
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
