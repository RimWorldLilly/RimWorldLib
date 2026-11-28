using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A58 RID: 2648
	public class JobDriver_OfferHelp : JobDriver
	{
		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x06004FDF RID: 20447 RVA: 0x001E2604 File Offset: 0x001E0804
		public Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.pawn.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004FE0 RID: 20448 RVA: 0x001E262F File Offset: 0x001E082F
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004FE1 RID: 20449 RVA: 0x001E2632 File Offset: 0x001E0832
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_OfferHelp.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_OfferHelp.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003117 RID: 12567
		private const TargetIndex OtherPawnInd = TargetIndex.A;
	}
}
