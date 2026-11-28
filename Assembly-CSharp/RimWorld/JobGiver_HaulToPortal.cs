using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B19 RID: 2841
	public class JobGiver_HaulToPortal : ThinkNode_JobGiver
	{
		// Token: 0x06005404 RID: 21508 RVA: 0x001F43D4 File Offset: 0x001F25D4
		protected override Job TryGiveJob(Pawn pawn)
		{
			MapPortal portal = pawn.mindState.duty.focus.Thing as MapPortal;
			if (EnterPortalUtility.HasJobOnPortal(pawn, portal))
			{
				return EnterPortalUtility.JobOnPortal(pawn, portal);
			}
			return null;
		}
	}
}
