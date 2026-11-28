using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AC1 RID: 2753
	public abstract class JobGiver_Binge : ThinkNode_JobGiver
	{
		// Token: 0x060052A8 RID: 21160 RVA: 0x001ECD66 File Offset: 0x001EAF66
		protected bool IgnoreForbid(Pawn pawn)
		{
			return pawn.InMentalState;
		}

		// Token: 0x060052A9 RID: 21161
		protected abstract int IngestInterval(Pawn pawn);

		// Token: 0x060052AA RID: 21162 RVA: 0x001ECD70 File Offset: 0x001EAF70
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Find.TickManager.TicksGame - pawn.mindState.lastIngestTick > this.IngestInterval(pawn))
			{
				Job job = this.IngestJob(pawn);
				if (job != null)
				{
					return job;
				}
			}
			return null;
		}

		// Token: 0x060052AB RID: 21163 RVA: 0x001ECDAC File Offset: 0x001EAFAC
		private Job IngestJob(Pawn pawn)
		{
			Thing thing = this.BestIngestTarget(pawn);
			if (thing == null)
			{
				return null;
			}
			ThingDef finalIngestibleDef = FoodUtility.GetFinalIngestibleDef(thing, false);
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, thing);
			job.count = finalIngestibleDef.ingestible.defaultNumToIngestAtOnce;
			job.ignoreForbidden = this.IgnoreForbid(pawn);
			job.overeat = true;
			return job;
		}

		// Token: 0x060052AC RID: 21164
		protected abstract Thing BestIngestTarget(Pawn pawn);
	}
}
