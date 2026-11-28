using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A27 RID: 2599
	public class JobDriver_DyeHair : JobDriver
	{
		// Token: 0x06004E85 RID: 20101 RVA: 0x001DDEE8 File Offset: 0x001DC0E8
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			LocalTargetInfo target = this.job.GetTarget(TargetIndex.A);
			if (!this.pawn.Reserve(target, this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			Thing thing = target.Thing;
			if (thing != null && thing.def.hasInteractionCell && !this.pawn.ReserveSittableOrSpot(thing.InteractionCell, this.job, errorOnFailed))
			{
				return false;
			}
			Thing thing2 = this.job.GetTarget(TargetIndex.B).Thing;
			return this.pawn.Reserve(thing2, this.job, 1, 1, null, errorOnFailed, false);
		}

		// Token: 0x06004E86 RID: 20102 RVA: 0x001DDF87 File Offset: 0x001DC187
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_DyeHair.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_DyeHair.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040030B9 RID: 12473
		public const TargetIndex StylingStationInd = TargetIndex.A;

		// Token: 0x040030BA RID: 12474
		public const TargetIndex DyeInd = TargetIndex.B;

		// Token: 0x040030BB RID: 12475
		public const int WorkTimeTicks = 300;
	}
}
