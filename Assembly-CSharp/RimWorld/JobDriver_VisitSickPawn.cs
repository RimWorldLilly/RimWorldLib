using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A03 RID: 2563
	public class JobDriver_VisitSickPawn : JobDriver
	{
		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x06004D84 RID: 19844 RVA: 0x001DAA94 File Offset: 0x001D8C94
		private Pawn Patient
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x06004D85 RID: 19845 RVA: 0x001DAABC File Offset: 0x001D8CBC
		private Thing Chair
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06004D86 RID: 19846 RVA: 0x001DAAE0 File Offset: 0x001D8CE0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Patient, this.job, 1, -1, null, errorOnFailed, false) && (this.Chair == null || this.pawn.Reserve(this.Chair, this.job, 1, -1, null, errorOnFailed, false));
		}

		// Token: 0x06004D87 RID: 19847 RVA: 0x001DAB40 File Offset: 0x001D8D40
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_VisitSickPawn.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_VisitSickPawn.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400306B RID: 12395
		private const TargetIndex PatientInd = TargetIndex.A;

		// Token: 0x0400306C RID: 12396
		private const TargetIndex ChairInd = TargetIndex.B;
	}
}
