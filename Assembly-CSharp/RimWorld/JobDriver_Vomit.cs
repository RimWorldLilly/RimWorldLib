using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A8C RID: 2700
	public class JobDriver_Vomit : JobDriver
	{
		// Token: 0x06005157 RID: 20823 RVA: 0x001E6CF8 File Offset: 0x001E4EF8
		public override void SetInitialPosture()
		{
		}

		// Token: 0x06005158 RID: 20824 RVA: 0x001E6CFA File Offset: 0x001E4EFA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x06005159 RID: 20825 RVA: 0x001E6D14 File Offset: 0x001E4F14
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x001E6D17 File Offset: 0x001E4F17
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Vomit.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_Vomit.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400318A RID: 12682
		private int ticksLeft;
	}
}
