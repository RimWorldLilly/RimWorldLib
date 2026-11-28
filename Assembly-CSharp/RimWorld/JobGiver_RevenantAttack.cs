using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B60 RID: 2912
	public class JobGiver_RevenantAttack : ThinkNode_JobGiver
	{
		// Token: 0x060054F3 RID: 21747 RVA: 0x001F9CA7 File Offset: 0x001F7EA7
		protected override Job TryGiveJob(Pawn pawn)
		{
			return JobMaker.MakeJob(JobDefOf.RevenantAttack, pawn.mindState.enemyTarget);
		}
	}
}
