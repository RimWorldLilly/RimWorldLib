using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BA8 RID: 2984
	public abstract class BabyPlayGiver
	{
		// Token: 0x06005621 RID: 22049 RVA: 0x002006F9 File Offset: 0x001FE8F9
		public BabyPlayGiver()
		{
		}

		// Token: 0x06005622 RID: 22050
		public abstract bool CanDo(Pawn pawn, Pawn baby);

		// Token: 0x06005623 RID: 22051 RVA: 0x00200701 File Offset: 0x001FE901
		public virtual Job TryGiveJob(Pawn pawn, Pawn baby)
		{
			Job job = JobMaker.MakeJob(this.def.jobDef, baby);
			job.count = 1;
			return job;
		}

		// Token: 0x0400330D RID: 13069
		public BabyPlayDef def;
	}
}
