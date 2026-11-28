using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CBF RID: 3263
	public static class GatherAnimalsAndSlavesForCaravanUtility
	{
		// Token: 0x06005C99 RID: 23705 RVA: 0x00220168 File Offset: 0x0021E368
		public static void CheckArrived(Lord lord, List<Pawn> pawns, IntVec3 meetingPoint, string memo, Predicate<Pawn> shouldCheckIfArrived, Predicate<Pawn> extraValidator = null)
		{
			bool flag = true;
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (shouldCheckIfArrived(pawn) && (!pawn.Spawned || !pawn.Position.InHorDistOf(meetingPoint, 10f) || !pawn.CanReach(meetingPoint, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn) || (extraValidator != null && !extraValidator(pawn))))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				lord.ReceiveMemo(memo);
			}
		}

		// Token: 0x06005C9A RID: 23706 RVA: 0x002201E8 File Offset: 0x0021E3E8
		public static bool IsRopedByCaravanPawn(Pawn animal)
		{
			Lord lord = animal.GetLord();
			if (lord != null && animal.roping.IsRopedByPawn)
			{
				Pawn ropedByPawn = animal.roping.RopedByPawn;
				return ((ropedByPawn != null) ? ropedByPawn.GetLord() : null) == lord;
			}
			return false;
		}

		// Token: 0x06005C9B RID: 23707 RVA: 0x00220228 File Offset: 0x0021E428
		public static bool CanRoperTakeAnimalToDest(Pawn pawn, Pawn animal, IntVec3 destSpot)
		{
			return pawn.CanReach(animal, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && animal.Map.reachability.CanReach(animal.Position, destSpot, PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true).WithFenceblockedOf(animal));
		}
	}
}
