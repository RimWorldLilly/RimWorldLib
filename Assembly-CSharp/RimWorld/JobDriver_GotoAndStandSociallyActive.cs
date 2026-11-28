using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009E4 RID: 2532
	public class JobDriver_GotoAndStandSociallyActive : JobDriver
	{
		// Token: 0x06004CC1 RID: 19649 RVA: 0x001D8058 File Offset: 0x001D6258
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA.Cell, this.job, 1, -1, null, true, false);
		}

		// Token: 0x06004CC2 RID: 19650 RVA: 0x001D8085 File Offset: 0x001D6285
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_GotoAndStandSociallyActive.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_GotoAndStandSociallyActive.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x06004CC3 RID: 19651 RVA: 0x001D8095 File Offset: 0x001D6295
		public virtual Toil StandToil
		{
			get
			{
				Toil toil = ToilMaker.MakeToil("StandToil");
				toil.tickIntervalAction = delegate(int delta)
				{
					if (!this.job.forceMaintainFacing)
					{
						Pawn pawn = JobDriver_StandAndBeSociallyActive.FindClosePawn(this.pawn);
						if (pawn != null)
						{
							this.pawn.rotationTracker.FaceCell(pawn.Position);
						}
					}
					this.pawn.GainComfortFromCellIfPossible(delta, false);
				};
				toil.socialMode = RandomSocialMode.SuperActive;
				toil.defaultCompleteMode = ToilCompleteMode.Never;
				toil.handlingFacing = true;
				return toil;
			}
		}
	}
}
