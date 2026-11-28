using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A6F RID: 2671
	public class JobDriver_ResurrectMech : JobDriver
	{
		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x06005092 RID: 20626 RVA: 0x001E48C0 File Offset: 0x001E2AC0
		private Corpse Corpse
		{
			get
			{
				return (Corpse)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x06005093 RID: 20627 RVA: 0x001E48E8 File Offset: 0x001E2AE8
		private IntVec3 Destination
		{
			get
			{
				return base.TargetB.Cell;
			}
		}

		// Token: 0x06005094 RID: 20628 RVA: 0x001E4904 File Offset: 0x001E2B04
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Corpse, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.Destination, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005095 RID: 20629 RVA: 0x001E4957 File Offset: 0x001E2B57
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ResurrectMech.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_ResurrectMech.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400314D RID: 12621
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x0400314E RID: 12622
		private const TargetIndex CastCellInd = TargetIndex.B;
	}
}
