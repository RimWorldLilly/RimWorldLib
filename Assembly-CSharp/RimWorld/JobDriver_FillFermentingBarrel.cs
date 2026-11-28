using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A34 RID: 2612
	public class JobDriver_FillFermentingBarrel : JobDriver
	{
		// Token: 0x17000E55 RID: 3669
		// (get) Token: 0x06004EDB RID: 20187 RVA: 0x001DF0CC File Offset: 0x001DD2CC
		protected Building_FermentingBarrel Barrel
		{
			get
			{
				return (Building_FermentingBarrel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000E56 RID: 3670
		// (get) Token: 0x06004EDC RID: 20188 RVA: 0x001DF0F4 File Offset: 0x001DD2F4
		protected Thing Wort
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06004EDD RID: 20189 RVA: 0x001DF118 File Offset: 0x001DD318
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Barrel, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.Wort, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004EDE RID: 20190 RVA: 0x001DF16B File Offset: 0x001DD36B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_FillFermentingBarrel.<MakeNewToils>d__8 <MakeNewToils>d__ = new JobDriver_FillFermentingBarrel.<MakeNewToils>d__8(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040030D1 RID: 12497
		private const TargetIndex BarrelInd = TargetIndex.A;

		// Token: 0x040030D2 RID: 12498
		private const TargetIndex WortInd = TargetIndex.B;

		// Token: 0x040030D3 RID: 12499
		private const int Duration = 200;
	}
}
