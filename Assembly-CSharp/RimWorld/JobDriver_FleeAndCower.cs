using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A38 RID: 2616
	public class JobDriver_FleeAndCower : JobDriver_Flee
	{
		// Token: 0x06004EF8 RID: 20216 RVA: 0x001DF8DC File Offset: 0x001DDADC
		public override string GetReport()
		{
			if (this.pawn.CurJob == this.job && this.pawn.Position == this.job.GetTarget(TargetIndex.A).Cell)
			{
				return "ReportCowering".Translate();
			}
			return base.GetReport();
		}

		// Token: 0x06004EF9 RID: 20217 RVA: 0x001DF938 File Offset: 0x001DDB38
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_FleeAndCower.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_FleeAndCower.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040030DA RID: 12506
		private const int CowerTicks = 1200;

		// Token: 0x040030DB RID: 12507
		private const int CheckFleeAgainIntervalTicks = 35;
	}
}
