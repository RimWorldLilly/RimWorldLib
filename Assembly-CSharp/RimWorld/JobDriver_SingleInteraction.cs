using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A78 RID: 2680
	public class JobDriver_SingleInteraction : JobDriver
	{
		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x060050CB RID: 20683 RVA: 0x001E529C File Offset: 0x001E349C
		private Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060050CC RID: 20684 RVA: 0x001E52C2 File Offset: 0x001E34C2
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060050CD RID: 20685 RVA: 0x001E52C5 File Offset: 0x001E34C5
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_SingleInteraction.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_SingleInteraction.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003160 RID: 12640
		private const TargetIndex OtherPawnInd = TargetIndex.A;
	}
}
