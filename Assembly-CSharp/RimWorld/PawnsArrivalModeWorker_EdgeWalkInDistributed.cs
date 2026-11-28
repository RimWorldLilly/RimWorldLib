using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E5 RID: 5349
	public class PawnsArrivalModeWorker_EdgeWalkInDistributed : PawnsArrivalModeWorker
	{
		// Token: 0x060082BF RID: 33471 RVA: 0x002EED48 File Offset: 0x002ECF48
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			PawnsArrivalModeWorker_EdgeWalkInDistributed.EntryCells.Clear();
			Map map = (Map)parms.target;
			int i;
			for (i = 0; i < pawns.Count; i++)
			{
				bool flag = false;
				for (int j = 0; j < 100; j++)
				{
					IntVec3 intVec;
					if (RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Ignore, false, null))
					{
						pawns[i].Position = intVec;
						PawnsArrivalModeWorker_EdgeWalkInDistributed.EntryCells.Add(intVec);
						flag = true;
						GenSpawn.Spawn(pawns[i], intVec, map, parms.spawnRotation, WipeMode.Vanish, false, false);
						break;
					}
				}
				if (!flag)
				{
					break;
				}
			}
			pawns.RemoveRange(i, pawns.Count - i);
		}

		// Token: 0x060082C0 RID: 33472 RVA: 0x002EEDE6 File Offset: 0x002ECFE6
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			parms.spawnRotation = Rot4.Random;
			return true;
		}

		// Token: 0x040050F5 RID: 20725
		private static readonly HashSet<IntVec3> EntryCells = new HashSet<IntVec3>();
	}
}
