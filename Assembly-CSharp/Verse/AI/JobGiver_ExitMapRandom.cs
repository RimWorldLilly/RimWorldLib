using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008E3 RID: 2275
	public class JobGiver_ExitMapRandom : JobGiver_ExitMap
	{
		// Token: 0x060045F9 RID: 17913 RVA: 0x001B80C4 File Offset: 0x001B62C4
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, bool canBash, out IntVec3 spot)
		{
			TraverseMode mode = canDig ? TraverseMode.PassAllDestroyableThings : TraverseMode.ByPawn;
			return RCellFinder.TryFindRandomExitSpot(pawn, out spot, mode);
		}
	}
}
