using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A0D RID: 2573
	public class JobDriver_Workwatching : JobDriver
	{
		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x06004DC0 RID: 19904 RVA: 0x001DB7CA File Offset: 0x001D99CA
		private Pawn AdultToFollow
		{
			get
			{
				return (Pawn)base.TargetThingA;
			}
		}

		// Token: 0x06004DC1 RID: 19905 RVA: 0x001DB7D7 File Offset: 0x001D99D7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x001DB7DA File Offset: 0x001D99DA
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Workwatching.<MakeNewToils>d__8 <MakeNewToils>d__ = new JobDriver_Workwatching.<MakeNewToils>d__8(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004DC3 RID: 19907 RVA: 0x001DB7EA File Offset: 0x001D99EA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.consecutiveTicksNoSkillJob, "consecutiveTicksNoSkillJob", 0, false);
		}

		// Token: 0x0400307B RID: 12411
		private const TargetIndex AdultInd = TargetIndex.A;

		// Token: 0x0400307C RID: 12412
		private const int FollowDistance = 3;

		// Token: 0x0400307D RID: 12413
		private const int StopWatchingNoSkillJobTicks = 5;

		// Token: 0x0400307E RID: 12414
		private const float LearningObservationDistance = 12f;

		// Token: 0x0400307F RID: 12415
		private int consecutiveTicksNoSkillJob;
	}
}
