using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AEA RID: 2794
	public class JobGiver_AISkipToJobTarget : ThinkNode_JobGiver
	{
		// Token: 0x06005370 RID: 21360 RVA: 0x001F1214 File Offset: 0x001EF414
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn_AbilityTracker abilities = pawn.abilities;
			Ability ability = (abilities != null) ? abilities.GetAbility(this.ability, false) : null;
			if (ability == null || !ability.CanCast)
			{
				return null;
			}
			Job curJob = pawn.CurJob;
			if (curJob == null || curJob.def == this.ability.jobDef)
			{
				return null;
			}
			LocalTargetInfo target = curJob.GetTarget(this.targetIndex);
			if (!this.CanSkipToTarget(pawn, target))
			{
				return null;
			}
			IntVec3 cell = target.Cell;
			if (target.HasThing && !RCellFinder.TryFindGoodAdjacentSpotToTouch(pawn, target.Thing, out cell))
			{
				return null;
			}
			if (ability.verb.ValidateTarget(target, false))
			{
				Job job = ability.GetJob(pawn, cell);
				pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, true, true, null, null, false, false, null, false, true, false);
			}
			return null;
		}

		// Token: 0x06005371 RID: 21361 RVA: 0x001F12F9 File Offset: 0x001EF4F9
		protected virtual bool CanSkipToTarget(Pawn pawn, LocalTargetInfo target)
		{
			return target.Cell.Fogged(pawn.Map);
		}

		// Token: 0x04003222 RID: 12834
		public AbilityDef ability;

		// Token: 0x04003223 RID: 12835
		public TargetIndex targetIndex = TargetIndex.A;
	}
}
