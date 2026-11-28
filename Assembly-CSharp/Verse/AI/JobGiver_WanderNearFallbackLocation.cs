using System;

namespace Verse.AI
{
	// Token: 0x02000901 RID: 2305
	public class JobGiver_WanderNearFallbackLocation : JobGiver_Wander
	{
		// Token: 0x06004659 RID: 18009 RVA: 0x001B934F File Offset: 0x001B754F
		public JobGiver_WanderNearFallbackLocation()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x001B9374 File Offset: 0x001B7574
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focusSecond.Cell, pawn);
		}
	}
}
