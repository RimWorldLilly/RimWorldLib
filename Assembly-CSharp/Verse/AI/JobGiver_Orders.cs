using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008EE RID: 2286
	public class JobGiver_Orders : ThinkNode_JobGiver
	{
		// Token: 0x06004610 RID: 17936 RVA: 0x001B841D File Offset: 0x001B661D
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Drafted)
			{
				return JobMaker.MakeJob(JobDefOf.Wait_Combat, pawn.Position);
			}
			return null;
		}
	}
}
