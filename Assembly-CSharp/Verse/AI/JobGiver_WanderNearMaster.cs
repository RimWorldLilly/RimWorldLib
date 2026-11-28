using System;

namespace Verse.AI
{
	// Token: 0x02000902 RID: 2306
	public class JobGiver_WanderNearMaster : JobGiver_Wander
	{
		// Token: 0x0600465B RID: 18011 RVA: 0x001B9391 File Offset: 0x001B7591
		public JobGiver_WanderNearMaster()
		{
			this.wanderRadius = 3f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.wanderDestValidator = ((Pawn p, IntVec3 c, IntVec3 root) => !this.MustUseRootRoom(p) || root.GetRoom(p.Map) == null || WanderRoomUtility.IsValidWanderDest(p, c, root));
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x001B93C8 File Offset: 0x001B75C8
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.playerSettings.Master.PositionHeld, pawn);
		}

		// Token: 0x0600465D RID: 18013 RVA: 0x001B93E0 File Offset: 0x001B75E0
		private bool MustUseRootRoom(Pawn pawn)
		{
			return !pawn.playerSettings.Master.playerSettings.animalsReleased;
		}
	}
}
