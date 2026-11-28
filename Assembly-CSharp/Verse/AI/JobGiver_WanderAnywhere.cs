using System;

namespace Verse.AI
{
	// Token: 0x020008F7 RID: 2295
	public class JobGiver_WanderAnywhere : JobGiver_Wander
	{
		// Token: 0x06004641 RID: 17985 RVA: 0x001B8E19 File Offset: 0x001B7019
		public JobGiver_WanderAnywhere()
		{
			this.wanderRadius = 7f;
			this.locomotionUrgency = LocomotionUrgency.Walk;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06004642 RID: 17986 RVA: 0x001B8E45 File Offset: 0x001B7045
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
