using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009E8 RID: 2536
	public class JobDriver_Sacrifice : JobDriver
	{
		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06004CDA RID: 19674 RVA: 0x001D83B8 File Offset: 0x001D65B8
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004CDB RID: 19675 RVA: 0x001D83DE File Offset: 0x001D65DE
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004CDC RID: 19676 RVA: 0x001D8401 File Offset: 0x001D6601
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Sacrifice.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_Sacrifice.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003032 RID: 12338
		private const TargetIndex VictimIndex = TargetIndex.A;

		// Token: 0x04003033 RID: 12339
		private const TargetIndex StandingIndex = TargetIndex.B;
	}
}
