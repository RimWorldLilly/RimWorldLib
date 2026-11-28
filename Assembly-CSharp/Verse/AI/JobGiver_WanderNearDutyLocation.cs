using System;

namespace Verse.AI
{
	// Token: 0x02000900 RID: 2304
	public class JobGiver_WanderNearDutyLocation : JobGiver_Wander
	{
		// Token: 0x06004657 RID: 18007 RVA: 0x001B930D File Offset: 0x001B750D
		public JobGiver_WanderNearDutyLocation()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06004658 RID: 18008 RVA: 0x001B9332 File Offset: 0x001B7532
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focus.Cell, pawn);
		}
	}
}
