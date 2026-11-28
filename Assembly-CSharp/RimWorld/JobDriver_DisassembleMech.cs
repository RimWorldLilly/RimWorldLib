using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A25 RID: 2597
	public class JobDriver_DisassembleMech : JobDriver
	{
		// Token: 0x17000E45 RID: 3653
		// (get) Token: 0x06004E79 RID: 20089 RVA: 0x001DDD24 File Offset: 0x001DBF24
		private Pawn Mech
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004E7A RID: 20090 RVA: 0x001DDD4A File Offset: 0x001DBF4A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Mech, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004E7B RID: 20091 RVA: 0x001DDD6D File Offset: 0x001DBF6D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_DisassembleMech.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_DisassembleMech.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040030B7 RID: 12471
		private const TargetIndex MechInd = TargetIndex.A;

		// Token: 0x040030B8 RID: 12472
		private const int DisassemblyTime = 300;
	}
}
