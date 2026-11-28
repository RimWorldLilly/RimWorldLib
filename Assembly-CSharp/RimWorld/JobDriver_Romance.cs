using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A74 RID: 2676
	public class JobDriver_Romance : JobDriver
	{
		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x060050B6 RID: 20662 RVA: 0x001E4F30 File Offset: 0x001E3130
		private Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060050B7 RID: 20663 RVA: 0x001E4F56 File Offset: 0x001E3156
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060050B8 RID: 20664 RVA: 0x001E4F59 File Offset: 0x001E3159
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Romance.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_Romance.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400315B RID: 12635
		private const TargetIndex OtherPawnInd = TargetIndex.A;
	}
}
