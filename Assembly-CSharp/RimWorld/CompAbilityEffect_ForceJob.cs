using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200140B RID: 5131
	public class CompAbilityEffect_ForceJob : CompAbilityEffect_WithDest
	{
		// Token: 0x170015BD RID: 5565
		// (get) Token: 0x06007F10 RID: 32528 RVA: 0x002D69DC File Offset: 0x002D4BDC
		public new CompProperties_AbilityForceJob Props
		{
			get
			{
				return (CompProperties_AbilityForceJob)this.props;
			}
		}

		// Token: 0x06007F11 RID: 32529 RVA: 0x002D69EC File Offset: 0x002D4BEC
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Thing as Pawn;
			if (pawn != null)
			{
				Job job = JobMaker.MakeJob(this.Props.jobDef, new LocalTargetInfo(base.GetDestination(target).Cell));
				float num = 1f;
				if (this.Props.durationMultiplier != null)
				{
					num = pawn.GetStatValue(this.Props.durationMultiplier, true, -1);
				}
				job.expiryInterval = (this.parent.def.GetStatValueAbstract(StatDefOf.Ability_Duration, this.parent.pawn) * num).SecondsToTicks();
				job.mote = MoteMaker.MakeThoughtBubble(pawn, this.parent.def.iconPath, true);
				RestUtility.WakeUp(pawn, true);
				pawn.jobs.StopAll(false, true);
				pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
			}
		}
	}
}
