using System;

namespace Verse.AI
{
	// Token: 0x02000904 RID: 2308
	public class JobGiver_RunRandom : JobGiver_Wander
	{
		// Token: 0x06004661 RID: 18017 RVA: 0x001B946B File Offset: 0x001B766B
		public JobGiver_RunRandom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(5, 10);
			this.locomotionUrgency = LocomotionUrgency.Sprint;
		}

		// Token: 0x06004662 RID: 18018 RVA: 0x001B9493 File Offset: 0x001B7693
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
