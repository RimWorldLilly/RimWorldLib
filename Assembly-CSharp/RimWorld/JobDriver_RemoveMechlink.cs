using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A69 RID: 2665
	public class JobDriver_RemoveMechlink : JobDriver
	{
		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x06005067 RID: 20583 RVA: 0x001E4074 File Offset: 0x001E2274
		protected Corpse Corpse
		{
			get
			{
				return (Corpse)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06005068 RID: 20584 RVA: 0x001E409A File Offset: 0x001E229A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Corpse, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005069 RID: 20585 RVA: 0x001E40BD File Offset: 0x001E22BD
		public override string GetReport()
		{
			return "ReportRemovingMechlink".Translate(HediffDefOf.MechlinkImplant.label);
		}

		// Token: 0x0600506A RID: 20586 RVA: 0x001E40DD File Offset: 0x001E22DD
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RemoveMechlink.<MakeNewToils>d__6 <MakeNewToils>d__ = new JobDriver_RemoveMechlink.<MakeNewToils>d__6(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003143 RID: 12611
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x04003144 RID: 12612
		public const int RemoveTicks = 300;
	}
}
