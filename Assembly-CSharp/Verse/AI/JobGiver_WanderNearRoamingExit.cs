using System;

namespace Verse.AI
{
	// Token: 0x020008FE RID: 2302
	public class JobGiver_WanderNearRoamingExit : JobGiver_Wander
	{
		// Token: 0x06004652 RID: 18002 RVA: 0x001B9225 File Offset: 0x001B7425
		public JobGiver_WanderNearRoamingExit()
		{
			this.wanderRadius = 12f;
		}

		// Token: 0x06004653 RID: 18003 RVA: 0x001B9238 File Offset: 0x001B7438
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_Roaming mentalState_Roaming = pawn.MentalState as MentalState_Roaming;
			if (mentalState_Roaming == null)
			{
				return null;
			}
			if (mentalState_Roaming.ShouldExitMapNow())
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x06004654 RID: 18004 RVA: 0x001B9267 File Offset: 0x001B7467
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			MentalState_Roaming mentalState_Roaming = pawn.MentalState as MentalState_Roaming;
			if (mentalState_Roaming == null)
			{
				return pawn.Position;
			}
			return mentalState_Roaming.exitDest;
		}
	}
}
