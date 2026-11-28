using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000802 RID: 2050
	public class JobDriver_EmancipateSlave : JobDriver
	{
		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x060040A3 RID: 16547 RVA: 0x0019BA20 File Offset: 0x00199C20
		private Pawn Slave
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060040A4 RID: 16548 RVA: 0x0019BA46 File Offset: 0x00199C46
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Slave, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060040A5 RID: 16549 RVA: 0x0019BA69 File Offset: 0x00199C69
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_EmancipateSlave.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_EmancipateSlave.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04002AE8 RID: 10984
		private const TargetIndex SlaveInd = TargetIndex.A;

		// Token: 0x04002AE9 RID: 10985
		private const TargetIndex BedInd = TargetIndex.B;
	}
}
