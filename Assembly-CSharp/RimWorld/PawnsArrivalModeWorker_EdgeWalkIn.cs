using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E3 RID: 5347
	public class PawnsArrivalModeWorker_EdgeWalkIn : PawnsArrivalModeWorker
	{
		// Token: 0x060082B9 RID: 33465 RVA: 0x002EEAD4 File Offset: 0x002ECCD4
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			for (int i = 0; i < pawns.Count; i++)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(parms.spawnCenter, map, 8, null);
				GenSpawn.Spawn(pawns[i], loc, map, parms.spawnRotation, WipeMode.Vanish, false, false);
			}
		}

		// Token: 0x060082BA RID: 33466 RVA: 0x002EEB28 File Offset: 0x002ECD28
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			PawnsArrivalModeWorker_EdgeWalkIn.<>c__DisplayClass1_0 CS$<>8__locals1 = new PawnsArrivalModeWorker_EdgeWalkIn.<>c__DisplayClass1_0();
			CS$<>8__locals1.map = (Map)parms.target;
			if (parms.attackTargets != null && parms.attackTargets.Count > 0 && !RCellFinder.TryFindEdgeCellFromThingAvoidingColony(parms.attackTargets[0], CS$<>8__locals1.map, new Func<IntVec3, IntVec3, bool>(CS$<>8__locals1.<TryResolveRaidSpawnCenter>g__predicate|0), out parms.spawnCenter))
			{
				CellFinder.TryFindRandomEdgeCellWith((IntVec3 p) => (CS$<>8__locals1.map.TileInfo.AllowRoofedEdgeWalkIn || !CS$<>8__locals1.map.roofGrid.Roofed(p)) && p.Walkable(CS$<>8__locals1.map), CS$<>8__locals1.map, CellFinder.EdgeRoadChance_Hostile, out parms.spawnCenter);
			}
			if (!parms.spawnCenter.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out parms.spawnCenter, CS$<>8__locals1.map, CellFinder.EdgeRoadChance_Hostile, false, null))
			{
				return false;
			}
			parms.spawnRotation = Rot4.FromAngleFlat((CS$<>8__locals1.map.Center - parms.spawnCenter).AngleFlat);
			return true;
		}
	}
}
