using System;

namespace Verse.AI
{
	// Token: 0x020008F8 RID: 2296
	public class JobGiver_WanderCurrentRoom : JobGiver_Wander
	{
		// Token: 0x06004643 RID: 17987 RVA: 0x001B8E50 File Offset: 0x001B7050
		public JobGiver_WanderCurrentRoom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.locomotionUrgency = LocomotionUrgency.Amble;
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => WanderRoomUtility.IsValidWanderDest(pawn, loc, root));
		}

		// Token: 0x06004644 RID: 17988 RVA: 0x001B8EAC File Offset: 0x001B70AC
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
