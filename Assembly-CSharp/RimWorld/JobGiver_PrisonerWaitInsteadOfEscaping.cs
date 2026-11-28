using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B5C RID: 2908
	public class JobGiver_PrisonerWaitInsteadOfEscaping : JobGiver_Wander
	{
		// Token: 0x060054E6 RID: 21734 RVA: 0x001F98D8 File Offset: 0x001F7AD8
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.guest == null || !pawn.guest.ShouldWaitInsteadOfEscaping)
			{
				return null;
			}
			Room room = pawn.GetRoom(RegionType.Set_All);
			if (room != null && room.IsPrisonCell)
			{
				return null;
			}
			IntVec3 spotToWaitInsteadOfEscaping = pawn.guest.spotToWaitInsteadOfEscaping;
			if (!spotToWaitInsteadOfEscaping.IsValid || !pawn.CanReach(spotToWaitInsteadOfEscaping, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				if (!RCellFinder.TryFindRandomSpotJustOutsideColony(pawn, out spotToWaitInsteadOfEscaping))
				{
					return null;
				}
				pawn.guest.spotToWaitInsteadOfEscaping = spotToWaitInsteadOfEscaping;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x060054E7 RID: 21735 RVA: 0x001F9959 File Offset: 0x001F7B59
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.guest.spotToWaitInsteadOfEscaping;
		}
	}
}
