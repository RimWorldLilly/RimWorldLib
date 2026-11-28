using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E7 RID: 5351
	public class PawnsArrivalModeWorker_EdgeWalkInGroups : PawnsArrivalModeWorker
	{
		// Token: 0x060082C8 RID: 33480 RVA: 0x002EF010 File Offset: 0x002ED210
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			List<Pair<List<Pawn>, IntVec3>> list = PawnsArrivalModeWorkerUtility.SplitIntoRandomGroupsNearMapEdge(pawns, map, false);
			PawnsArrivalModeWorkerUtility.SetPawnGroupsInfo(parms, list);
			for (int i = 0; i < list.Count; i++)
			{
				for (int j = 0; j < list[i].First.Count; j++)
				{
					IntVec3 loc = CellFinder.RandomClosewalkCellNear(list[i].Second, map, 8, null);
					GenSpawn.Spawn(list[i].First[j], loc, map, parms.spawnRotation, WipeMode.Vanish, false, false);
				}
			}
		}

		// Token: 0x060082C9 RID: 33481 RVA: 0x002EF0AC File Offset: 0x002ED2AC
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
