using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E8 RID: 5352
	public class PawnsArrivalModeWorker_EdgeWalkInHateChanters : PawnsArrivalModeWorker
	{
		// Token: 0x060082CB RID: 33483 RVA: 0x002EF0C4 File Offset: 0x002ED2C4
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			PawnsArrivalModeWorker_EdgeWalkInHateChanters.<>c__DisplayClass1_0 CS$<>8__locals1 = new PawnsArrivalModeWorker_EdgeWalkInHateChanters.<>c__DisplayClass1_0();
			CS$<>8__locals1.pawns = pawns;
			PawnsArrivalModeWorker_EdgeWalkInHateChanters.EntryCells.Clear();
			Map map = (Map)parms.target;
			int i;
			for (i = 0; i < CS$<>8__locals1.pawns.Count; i++)
			{
				bool flag = false;
				for (int j = 0; j < 100; j++)
				{
					IntVec3 intVec;
					if (RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Ignore, false, new Predicate<IntVec3>(CS$<>8__locals1.<Arrive>g__SmallGroupValidator|0)))
					{
						CS$<>8__locals1.pawns[i].Position = intVec;
						PawnsArrivalModeWorker_EdgeWalkInHateChanters.EntryCells.Add(intVec);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
			}
			CS$<>8__locals1.pawns.RemoveRange(i, CS$<>8__locals1.pawns.Count - i);
		}

		// Token: 0x060082CC RID: 33484 RVA: 0x002EF179 File Offset: 0x002ED379
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			parms.spawnRotation = Rot4.Random;
			return true;
		}

		// Token: 0x040050F7 RID: 20727
		private static readonly HashSet<IntVec3> EntryCells = new HashSet<IntVec3>();
	}
}
