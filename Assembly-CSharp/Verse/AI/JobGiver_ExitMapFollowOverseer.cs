using System;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x020008E7 RID: 2279
	public class JobGiver_ExitMapFollowOverseer : JobGiver_ExitMapBest
	{
		// Token: 0x06004600 RID: 17920 RVA: 0x001B8171 File Offset: 0x001B6371
		public JobGiver_ExitMapFollowOverseer()
		{
			this.failIfCantJoinOrCreateCaravan = true;
		}

		// Token: 0x06004601 RID: 17921 RVA: 0x001B8180 File Offset: 0x001B6380
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(pawn))
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}
	}
}
