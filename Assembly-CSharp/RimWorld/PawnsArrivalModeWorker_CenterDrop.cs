using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020014DF RID: 5343
	public class PawnsArrivalModeWorker_CenterDrop : PawnsArrivalModeWorker
	{
		// Token: 0x060082AA RID: 33450 RVA: 0x002EE7D6 File Offset: 0x002EC9D6
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			PawnsArrivalModeWorkerUtility.DropInDropPodsNearSpawnCenter(parms, pawns);
		}

		// Token: 0x060082AB RID: 33451 RVA: 0x002EE7E0 File Offset: 0x002EC9E0
		public override void TravellingTransportersArrived(List<ActiveTransporterInfo> transporters, Map map)
		{
			IntVec3 near;
			if (!DropCellFinder.TryFindRaidDropCenterClose(out near, map, true, true, true, -1))
			{
				near = DropCellFinder.FindRaidDropCenterDistant(map, false, true);
			}
			if (transporters.IsShuttle())
			{
				TransportersArrivalActionUtility.DropShuttle(transporters[0], map, near, null, null);
				return;
			}
			TransportersArrivalActionUtility.DropTravellingDropPods(transporters, near, map);
		}

		// Token: 0x060082AC RID: 33452 RVA: 0x002EE830 File Offset: 0x002ECA30
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!parms.raidArrivalModeForQuickMilitaryAid)
			{
				parms.podOpenDelay = 520;
			}
			parms.spawnRotation = Rot4.Random;
			if (!parms.spawnCenter.IsValid)
			{
				bool flag = parms.faction != null && parms.faction == Faction.OfMechanoids;
				bool flag2 = parms.faction != null && parms.faction.HostileTo(Faction.OfPlayer);
				if (Rand.Chance(0.4f) && !flag && map.listerBuildings.ColonistsHaveBuildingWithPowerOn(ThingDefOf.OrbitalTradeBeacon))
				{
					parms.spawnCenter = DropCellFinder.TradeDropSpot(map);
				}
				else if (!DropCellFinder.TryFindRaidDropCenterClose(out parms.spawnCenter, map, !flag && flag2, !flag, true, -1))
				{
					parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeDrop;
					return parms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(parms);
				}
			}
			return true;
		}

		// Token: 0x040050F4 RID: 20724
		public const int PodOpenDelay = 520;
	}
}
