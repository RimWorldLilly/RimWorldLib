using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000820 RID: 2080
	public class JobDriver_WaitDowned : JobDriver_Wait
	{
		// Token: 0x06004164 RID: 16740 RVA: 0x0019F164 File Offset: 0x0019D364
		public override string GetReport()
		{
			if (this.pawn.Deathresting)
			{
				return this.ReportStringProcessed(SanguophageUtility.DeathrestJobReport(this.pawn));
			}
			if (!this.pawn.Awake())
			{
				return "DownedUnconscious".Translate();
			}
			if (!this.pawn.health.CanCrawl)
			{
				return "DownedCannotCrawl".Translate();
			}
			return base.GetReport();
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x0019F1D5 File Offset: 0x0019D3D5
		public override void DecorateWaitToil(Toil wait)
		{
			base.DecorateWaitToil(wait);
			wait.AddFailCondition(() => !this.pawn.Downed);
		}
	}
}
