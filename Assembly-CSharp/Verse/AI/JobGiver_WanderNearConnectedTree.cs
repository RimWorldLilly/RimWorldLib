using System;

namespace Verse.AI
{
	// Token: 0x020008F4 RID: 2292
	public class JobGiver_WanderNearConnectedTree : JobGiver_Wander
	{
		// Token: 0x06004635 RID: 17973 RVA: 0x001B8A0C File Offset: 0x001B6C0C
		public JobGiver_WanderNearConnectedTree()
		{
			this.wanderRadius = 12f;
			this.ticksBetweenWandersRange = new IntRange(130, 250);
		}

		// Token: 0x06004636 RID: 17974 RVA: 0x001B8A34 File Offset: 0x001B6C34
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.connections.ConnectedThings.FirstOrDefault((Thing x) => x.Spawned && x.Map == pawn.Map && pawn.CanReach(x, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn)) == null)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x06004637 RID: 17975 RVA: 0x001B8A7F File Offset: 0x001B6C7F
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.connections.ConnectedThings[0].Position;
		}
	}
}
