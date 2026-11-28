using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000AEF RID: 2799
	public abstract class JobGiver_PrepareCaravan_RopePawns : ThinkNode_JobGiver
	{
		// Token: 0x17000EFF RID: 3839
		// (get) Token: 0x06005381 RID: 21377
		protected abstract JobDef RopeJobDef { get; }

		// Token: 0x06005382 RID: 21378 RVA: 0x001F1908 File Offset: 0x001EFB08
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			Pawn pawn2;
			if (pawn.roping.IsRopingOthers)
			{
				pawn2 = pawn.roping.Ropees[0];
			}
			else
			{
				pawn2 = this.FindAnimalNeedingGathering(pawn);
			}
			if (pawn2 == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(this.RopeJobDef, pawn2, cell);
			job.lord = pawn.GetLord();
			this.DecorateJob(job);
			return job;
		}

		// Token: 0x06005383 RID: 21379 RVA: 0x001F1985 File Offset: 0x001EFB85
		protected virtual void DecorateJob(Job job)
		{
		}

		// Token: 0x06005384 RID: 21380 RVA: 0x001F1988 File Offset: 0x001EFB88
		private Pawn FindAnimalNeedingGathering(Pawn pawn)
		{
			foreach (Pawn pawn2 in pawn.GetLord().ownedPawns)
			{
				if (this.AnimalNeedsGathering(pawn, pawn2))
				{
					return pawn2;
				}
			}
			return null;
		}

		// Token: 0x06005385 RID: 21381
		protected abstract bool AnimalNeedsGathering(Pawn pawn, Pawn animal);
	}
}
