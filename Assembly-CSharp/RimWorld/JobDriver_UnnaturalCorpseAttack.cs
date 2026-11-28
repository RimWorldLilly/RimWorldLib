using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A87 RID: 2695
	public class JobDriver_UnnaturalCorpseAttack : JobDriver
	{
		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x0600513E RID: 20798 RVA: 0x001E6881 File Offset: 0x001E4A81
		private Pawn Victim
		{
			get
			{
				return base.TargetPawnA;
			}
		}

		// Token: 0x0600513F RID: 20799 RVA: 0x001E6889 File Offset: 0x001E4A89
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06005140 RID: 20800 RVA: 0x001E688C File Offset: 0x001E4A8C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_UnnaturalCorpseAttack.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_UnnaturalCorpseAttack.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06005141 RID: 20801 RVA: 0x001E689C File Offset: 0x001E4A9C
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x04003184 RID: 12676
		private const TargetIndex VictimIndex = TargetIndex.A;
	}
}
