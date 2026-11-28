using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000ABF RID: 2751
	public class JobGiver_SelfDetonate : ThinkNode_JobGiver
	{
		// Token: 0x060052A4 RID: 21156 RVA: 0x001ECC9E File Offset: 0x001EAE9E
		protected override Job TryGiveJob(Pawn pawn)
		{
			return JobMaker.MakeJob(JobDefOf.SelfDetonate);
		}
	}
}
