using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014EA RID: 5354
	public class PawnsArrivalModeWorker_RandomDrop : PawnsArrivalModeWorker
	{
		// Token: 0x060082D7 RID: 33495 RVA: 0x002EF4FC File Offset: 0x002ED6FC
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			bool defaultValue = parms.faction != null && parms.faction.HostileTo(Faction.OfPlayer);
			for (int i = 0; i < pawns.Count; i++)
			{
				DropPodUtility.DropThingsNear(DropCellFinder.RandomDropSpot(map, true), map, Gen.YieldSingle<Thing>(pawns[i]), parms.podOpenDelay, false, true, parms.canRoofPunch.GetValueOrDefault(defaultValue), true, true, parms.faction);
			}
		}

		// Token: 0x060082D8 RID: 33496 RVA: 0x002EF577 File Offset: 0x002ED777
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			if (!parms.raidArrivalModeForQuickMilitaryAid)
			{
				parms.podOpenDelay = 520;
			}
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
