using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A39 RID: 2617
	public class JobDriver_FleeAndCowerShort : JobDriver_Flee
	{
		// Token: 0x06004EFD RID: 20221 RVA: 0x001DF980 File Offset: 0x001DDB80
		public override string GetReport()
		{
			if (this.pawn.CurJob == this.job && this.pawn.Position == this.job.GetTarget(TargetIndex.A).Cell)
			{
				return "ReportCowering".Translate();
			}
			return base.GetReport();
		}

		// Token: 0x06004EFE RID: 20222 RVA: 0x001DF9DC File Offset: 0x001DDBDC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_FleeAndCowerShort.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_FleeAndCowerShort.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040030DC RID: 12508
		private const int CowerTicks = 30;
	}
}
