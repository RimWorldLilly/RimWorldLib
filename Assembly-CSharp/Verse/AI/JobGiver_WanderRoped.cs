using System;

namespace Verse.AI
{
	// Token: 0x02000905 RID: 2309
	public class JobGiver_WanderRoped : JobGiver_Wander
	{
		// Token: 0x06004663 RID: 18019 RVA: 0x001B949B File Offset: 0x001B769B
		public JobGiver_WanderRoped()
		{
			this.wanderRadius = 6f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06004664 RID: 18020 RVA: 0x001B94C0 File Offset: 0x001B76C0
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.roping.RopedTo.Cell;
		}

		// Token: 0x06004665 RID: 18021 RVA: 0x001B94E0 File Offset: 0x001B76E0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.roping.IsRoped || pawn.roping.IsRopedByPawn)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}
	}
}
