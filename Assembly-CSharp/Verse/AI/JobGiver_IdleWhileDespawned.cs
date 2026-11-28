using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008EC RID: 2284
	public class JobGiver_IdleWhileDespawned : ThinkNode_JobGiver
	{
		// Token: 0x0600460B RID: 17931 RVA: 0x001B8316 File Offset: 0x001B6516
		protected override Job TryGiveJob(Pawn pawn)
		{
			return JobMaker.MakeJob(JobDefOf.IdleWhileDespawned);
		}
	}
}
