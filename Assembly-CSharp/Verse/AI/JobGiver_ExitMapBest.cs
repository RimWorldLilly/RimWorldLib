using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008E4 RID: 2276
	public class JobGiver_ExitMapBest : JobGiver_ExitMap
	{
		// Token: 0x060045FB RID: 17915 RVA: 0x001B80EC File Offset: 0x001B62EC
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, bool canBash, out IntVec3 spot)
		{
			TraverseMode mode = canDig ? TraverseMode.PassAllDestroyableThings : TraverseMode.ByPawn;
			return RCellFinder.TryFindBestExitSpot(pawn, out spot, mode, canBash);
		}
	}
}
