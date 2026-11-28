using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A7C RID: 2684
	public class JobDriver_TakeAndEnterPortal : JobDriver_EnterPortal
	{
		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x060050EE RID: 20718 RVA: 0x001E58A0 File Offset: 0x001E3AA0
		private Thing ThingToTake
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x060050EF RID: 20719 RVA: 0x001E58C1 File Offset: 0x001E3AC1
		private Pawn PawnToTake
		{
			get
			{
				return this.ThingToTake as Pawn;
			}
		}

		// Token: 0x060050F0 RID: 20720 RVA: 0x001E58CE File Offset: 0x001E3ACE
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.ThingToTake, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060050F1 RID: 20721 RVA: 0x001E58F1 File Offset: 0x001E3AF1
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_TakeAndEnterPortal.<MakeNewToils>d__6 <MakeNewToils>d__ = new JobDriver_TakeAndEnterPortal.<MakeNewToils>d__6(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400316C RID: 12652
		private const TargetIndex ThingInd = TargetIndex.B;
	}
}
