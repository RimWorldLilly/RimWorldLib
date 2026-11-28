using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008E5 RID: 2277
	public class JobGiver_ExitMapNearDutyTarget : JobGiver_ExitMap
	{
		// Token: 0x060045FD RID: 17917 RVA: 0x001B8114 File Offset: 0x001B6314
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, bool canBash, out IntVec3 spot)
		{
			TraverseMode mode = canDig ? TraverseMode.PassAllDestroyableThings : TraverseMode.ByPawn;
			IntVec3 near = pawn.DutyLocation();
			float num = pawn.mindState.duty.radius;
			if (num <= 0f)
			{
				num = 12f;
			}
			return RCellFinder.TryFindExitSpotNear(pawn, near, num, out spot, mode);
		}
	}
}
