using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AE8 RID: 2792
	public class JobGiver_AIJumpToJobTarget : ThinkNode_JobGiver
	{
		// Token: 0x0600536B RID: 21355 RVA: 0x001F1058 File Offset: 0x001EF258
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
			if (!this.CanJumpToTarget(pawn, target))
			{
				return null;
			}
			IntVec3 cell = target.Cell;
			float num = pawn.Position.DistanceTo(cell);
			VerbProperties verbProps = ability.verb.verbProps;
			if (num < verbProps.minRange || num > ability.verb.EffectiveRange || !GenSight.LineOfSight(pawn.Position, cell, pawn.Map))
			{
				return null;
			}
			if (target.HasThing && !RCellFinder.TryFindGoodAdjacentSpotToTouch(pawn, target.Thing, out cell))
			{
				return null;
			}
			if (ability.verb.ValidateTarget(target, false))
			{
				Job job = ability.GetJob(cell, cell);
				pawn.jobs.StartJob(job, JobCondition.Ongoing, null, true, true, null, null, false, false, null, false, true, false);
				FleckMaker.Static(cell, pawn.Map, FleckDefOf.FeedbackGoto, 1f);
			}
			return null;
		}

		// Token: 0x0600536C RID: 21356 RVA: 0x001F119D File Offset: 0x001EF39D
		public virtual bool CanJumpToTarget(Pawn pawn, LocalTargetInfo target)
		{
			return true;
		}

		// Token: 0x04003220 RID: 12832
		public AbilityDef ability;

		// Token: 0x04003221 RID: 12833
		public TargetIndex targetIndex = TargetIndex.A;
	}
}
