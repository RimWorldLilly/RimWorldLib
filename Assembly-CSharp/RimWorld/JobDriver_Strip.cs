using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A79 RID: 2681
	public class JobDriver_Strip : JobDriver
	{
		// Token: 0x060050CF RID: 20687 RVA: 0x001E52DD File Offset: 0x001E34DD
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060050D0 RID: 20688 RVA: 0x001E5300 File Offset: 0x001E3500
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Strip.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_Strip.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x060050D1 RID: 20689 RVA: 0x001E5310 File Offset: 0x001E3510
		public override object[] TaleParameters()
		{
			Corpse corpse = base.TargetA.Thing as Corpse;
			return new object[]
			{
				this.pawn,
				(corpse != null) ? corpse.InnerPawn : base.TargetA.Thing
			};
		}

		// Token: 0x04003161 RID: 12641
		private const int StripTicks = 60;
	}
}
