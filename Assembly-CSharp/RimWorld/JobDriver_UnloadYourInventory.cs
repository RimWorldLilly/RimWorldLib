using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A86 RID: 2694
	public class JobDriver_UnloadYourInventory : JobDriver
	{
		// Token: 0x06005138 RID: 20792 RVA: 0x001E6698 File Offset: 0x001E4898
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.countToDrop, "countToDrop", -1, false);
		}

		// Token: 0x06005139 RID: 20793 RVA: 0x001E66B2 File Offset: 0x001E48B2
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600513A RID: 20794 RVA: 0x001E66B5 File Offset: 0x001E48B5
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_UnloadYourInventory.<MakeNewToils>d__6 <MakeNewToils>d__ = new JobDriver_UnloadYourInventory.<MakeNewToils>d__6(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003180 RID: 12672
		private int countToDrop = -1;

		// Token: 0x04003181 RID: 12673
		private const TargetIndex ItemToHaulInd = TargetIndex.A;

		// Token: 0x04003182 RID: 12674
		private const TargetIndex StoreCellInd = TargetIndex.B;

		// Token: 0x04003183 RID: 12675
		private const int UnloadDuration = 10;
	}
}
