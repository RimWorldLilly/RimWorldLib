using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A7B RID: 2683
	public class JobDriver_StudyItem : JobDriver
	{
		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x060050E7 RID: 20711 RVA: 0x001E57C6 File Offset: 0x001E39C6
		protected Thing ThingToStudy
		{
			get
			{
				return base.TargetThingA;
			}
		}

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x060050E8 RID: 20712 RVA: 0x001E57CE File Offset: 0x001E39CE
		protected bool CanStudyInPlace
		{
			get
			{
				CompAnalyzable compAnalyzable = this.ThingToStudy.TryGetComp<CompAnalyzable>();
				return compAnalyzable != null && compAnalyzable.Props.canStudyInPlace;
			}
		}

		// Token: 0x060050E9 RID: 20713 RVA: 0x001E57EC File Offset: 0x001E39EC
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.ThingToStudy, this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			if (!this.CanStudyInPlace)
			{
				if (!this.pawn.Reserve(base.TargetB, this.job, 1, -1, null, errorOnFailed, false))
				{
					return false;
				}
				if (!this.pawn.Reserve(base.TargetC, this.job, 1, -1, null, errorOnFailed, false))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060050EA RID: 20714 RVA: 0x001E5867 File Offset: 0x001E3A67
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_StudyItem.<MakeNewToils>d__9 <MakeNewToils>d__ = new JobDriver_StudyItem.<MakeNewToils>d__9(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x060050EB RID: 20715 RVA: 0x001E5877 File Offset: 0x001E3A77
		protected virtual IEnumerable<Toil> GetStudyToils()
		{
			JobDriver_StudyItem.<GetStudyToils>d__10 <GetStudyToils>d__ = new JobDriver_StudyItem.<GetStudyToils>d__10(-2);
			<GetStudyToils>d__.<>4__this = this;
			return <GetStudyToils>d__;
		}

		// Token: 0x04003168 RID: 12648
		private const int JobEndInterval = 4000;

		// Token: 0x04003169 RID: 12649
		protected const TargetIndex ThingToStudyIndex = TargetIndex.A;

		// Token: 0x0400316A RID: 12650
		protected const TargetIndex ResearchBenchInd = TargetIndex.B;

		// Token: 0x0400316B RID: 12651
		protected const TargetIndex HaulCell = TargetIndex.C;
	}
}
