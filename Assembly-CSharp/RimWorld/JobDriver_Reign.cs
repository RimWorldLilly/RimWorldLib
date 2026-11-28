using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AA5 RID: 2725
	public class JobDriver_Reign : JobDriver_Meditate
	{
		// Token: 0x17000EE8 RID: 3816
		// (get) Token: 0x06005218 RID: 21016 RVA: 0x001E9520 File Offset: 0x001E7720
		private Building_Throne Throne
		{
			get
			{
				return base.TargetThingA as Building_Throne;
			}
		}

		// Token: 0x06005219 RID: 21017 RVA: 0x001E952D File Offset: 0x001E772D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Throne, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600521A RID: 21018 RVA: 0x001E9550 File Offset: 0x001E7750
		public override string GetReport()
		{
			return string.Concat(new string[]
			{
				this.ReportStringProcessed(this.job.def.reportString),
				": ",
				this.Throne.LabelShort.CapitalizeFirst(),
				".",
				base.PsyfocusPerDayReport()
			});
		}

		// Token: 0x0600521B RID: 21019 RVA: 0x001E95AD File Offset: 0x001E77AD
		public override bool CanBeginNowWhileLyingDown()
		{
			return false;
		}

		// Token: 0x0600521C RID: 21020 RVA: 0x001E95B0 File Offset: 0x001E77B0
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Reign.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_Reign.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040031BB RID: 12731
		protected const TargetIndex FacingInd = TargetIndex.B;

		// Token: 0x040031BC RID: 12732
		protected const int ApplyThoughtInitialTicks = 10000;
	}
}
