using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E0 RID: 5344
	public class PawnsArrivalModeWorker_ClusterDrop : PawnsArrivalModeWorker
	{
		// Token: 0x060082AE RID: 33454 RVA: 0x002EE91B File Offset: 0x002ECB1B
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
		}

		// Token: 0x060082AF RID: 33455 RVA: 0x002EE920 File Offset: 0x002ECB20
		public override void TravellingTransportersArrived(List<ActiveTransporterInfo> transporters, Map map)
		{
			IntVec3 near = DropCellFinder.FindRaidDropCenterDistant(map, false, true);
			TransportersArrivalActionUtility.DropTravellingDropPods(transporters, near, map);
		}

		// Token: 0x060082B0 RID: 33456 RVA: 0x002EE940 File Offset: 0x002ECB40
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!parms.spawnCenter.IsValid)
			{
				parms.spawnCenter = MechClusterUtility.FindClusterPosition(map, parms.mechClusterSketch, 100, 0.5f);
			}
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
