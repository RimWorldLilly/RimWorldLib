using System;

namespace Verse.AI
{
	// Token: 0x020008FF RID: 2303
	public class JobGiver_WanderOwnRoom : JobGiver_Wander
	{
		// Token: 0x06004655 RID: 18005 RVA: 0x001B9284 File Offset: 0x001B7484
		public JobGiver_WanderOwnRoom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(300, 600);
			this.locomotionUrgency = LocomotionUrgency.Amble;
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => WanderRoomUtility.IsValidWanderDest(pawn, loc, root));
		}

		// Token: 0x06004656 RID: 18006 RVA: 0x001B92E4 File Offset: 0x001B74E4
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			MentalState_WanderOwnRoom mentalState_WanderOwnRoom = pawn.MentalState as MentalState_WanderOwnRoom;
			if (mentalState_WanderOwnRoom != null)
			{
				return mentalState_WanderOwnRoom.target;
			}
			return pawn.Position;
		}
	}
}
