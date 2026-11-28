using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A83 RID: 2691
	public class JobDriver_TalkCreepJoiner : JobDriver
	{
		// Token: 0x06005129 RID: 20777 RVA: 0x001E6405 File Offset: 0x001E4605
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(base.TargetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600512A RID: 20778 RVA: 0x001E6423 File Offset: 0x001E4623
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_TalkCreepJoiner.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_TalkCreepJoiner.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0600512B RID: 20779 RVA: 0x001E6433 File Offset: 0x001E4633
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.notified, "notified", false, false);
		}

		// Token: 0x0400317B RID: 12667
		private bool notified;
	}
}
