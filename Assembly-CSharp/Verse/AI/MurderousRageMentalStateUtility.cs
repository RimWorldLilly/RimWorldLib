using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200089E RID: 2206
	public static class MurderousRageMentalStateUtility
	{
		// Token: 0x06004448 RID: 17480 RVA: 0x001ABCC4 File Offset: 0x001A9EC4
		public static Pawn FindPawnToKill(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return null;
			}
			MurderousRageMentalStateUtility.tmpTargets.Clear();
			IReadOnlyList<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn2 = allPawnsSpawned[i];
				if ((pawn2.Faction == pawn.Faction || (pawn2.IsPrisoner && pawn2.HostFaction == pawn.Faction)) && pawn2.RaceProps.Humanlike && pawn2 != pawn && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && (pawn2.CurJob == null || !pawn2.CurJob.exitMapOnArrival))
				{
					MurderousRageMentalStateUtility.tmpTargets.Add(pawn2);
				}
			}
			if (!MurderousRageMentalStateUtility.tmpTargets.Any<Pawn>())
			{
				return null;
			}
			Pawn result = MurderousRageMentalStateUtility.tmpTargets.RandomElement<Pawn>();
			MurderousRageMentalStateUtility.tmpTargets.Clear();
			return result;
		}

		// Token: 0x04002C43 RID: 11331
		private static List<Pawn> tmpTargets = new List<Pawn>();
	}
}
