using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008E1 RID: 2273
	public class JobGiver_Carried : ThinkNode_JobGiver
	{
		// Token: 0x060045F3 RID: 17907 RVA: 0x001B7DAC File Offset: 0x001B5FAC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.Awake() && pawn.CurJob != null)
			{
				return pawn.CurJob;
			}
			return JobMaker.MakeJob(JobDefOf.Carried);
		}
	}
}
