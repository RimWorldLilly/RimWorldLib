using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008FD RID: 2301
	public class JobGiver_WanderMapEdge : JobGiver_Wander
	{
		// Token: 0x06004650 RID: 18000 RVA: 0x001B91E0 File Offset: 0x001B73E0
		public JobGiver_WanderMapEdge()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(50, 125);
		}

		// Token: 0x06004651 RID: 18001 RVA: 0x001B9204 File Offset: 0x001B7404
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			IntVec3 result;
			if (RCellFinder.TryFindBestExitSpot(pawn, out result, TraverseMode.ByPawn, true))
			{
				return result;
			}
			return pawn.Position;
		}
	}
}
