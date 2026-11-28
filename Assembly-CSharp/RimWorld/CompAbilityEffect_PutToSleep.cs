using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200142B RID: 5163
	public class CompAbilityEffect_PutToSleep : CompAbilityEffect
	{
		// Token: 0x06007F6D RID: 32621 RVA: 0x002D82FC File Offset: 0x002D64FC
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				pawn.needs.rest.CurLevel = 0f;
				Job job = JobMaker.MakeJob(JobDefOf.LayDown, pawn.Position);
				job.forceSleep = true;
				pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
			}
		}
	}
}
