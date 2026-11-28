using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A21 RID: 2593
	public class JobDriver_ClearSnowAndSand : JobDriver
	{
		// Token: 0x17000E3F RID: 3647
		// (get) Token: 0x06004E5B RID: 20059 RVA: 0x001DD6C0 File Offset: 0x001DB8C0
		private float TotalNeededWork
		{
			get
			{
				float num = 50f;
				float depth = base.Map.snowGrid.GetDepth(base.TargetLocA);
				SandGrid sandGrid = base.Map.sandGrid;
				return num * (depth + ((sandGrid != null) ? new float?(sandGrid.GetDepth(base.TargetLocA)) : null)).GetValueOrDefault();
			}
		}

		// Token: 0x06004E5C RID: 20060 RVA: 0x001DD73F File Offset: 0x001DB93F
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004E5D RID: 20061 RVA: 0x001DD762 File Offset: 0x001DB962
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ClearSnowAndSand.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_ClearSnowAndSand.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004E5E RID: 20062 RVA: 0x001DD774 File Offset: 0x001DB974
		private bool IsSand(IntVec3 cell)
		{
			Map map = base.Map;
			float? num;
			if (map == null)
			{
				num = null;
			}
			else
			{
				SandGrid sandGrid = map.sandGrid;
				num = ((sandGrid != null) ? new float?(sandGrid.GetDepth(cell)) : null);
			}
			float? num2 = num;
			if (num2 != null)
			{
				float valueOrDefault = num2.GetValueOrDefault();
				float num3 = valueOrDefault;
				Map map2 = base.Map;
				num2 = ((map2 != null) ? new float?(map2.snowGrid.GetDepth(cell)) : null);
				return num3 > num2.GetValueOrDefault() & num2 != null;
			}
			return false;
		}

		// Token: 0x06004E5F RID: 20063 RVA: 0x001DD804 File Offset: 0x001DBA04
		public override string GetReport()
		{
			string reportStringOverride = base.GetReportStringOverride();
			if (!reportStringOverride.NullOrEmpty())
			{
				return reportStringOverride;
			}
			LocalTargetInfo localTargetInfo = this.job.targetA.IsValid ? this.job.targetA : this.job.targetQueueA.FirstValid();
			if (localTargetInfo.Cell.IsValid && this.IsSand(localTargetInfo.Cell))
			{
				return "JobClearingSand".Translate();
			}
			return this.ReportStringProcessed(this.job.def.reportString);
		}

		// Token: 0x040030B1 RID: 12465
		private float workDone;

		// Token: 0x040030B2 RID: 12466
		private const float ClearWorkPerDepositedDepth = 50f;
	}
}
