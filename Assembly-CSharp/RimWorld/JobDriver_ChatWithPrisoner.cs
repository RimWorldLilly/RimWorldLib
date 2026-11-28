using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A98 RID: 2712
	public class JobDriver_ChatWithPrisoner : JobDriver
	{
		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x060051A2 RID: 20898 RVA: 0x001E787E File Offset: 0x001E5A7E
		protected Pawn Talkee
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x060051A3 RID: 20899 RVA: 0x001E7895 File Offset: 0x001E5A95
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060051A4 RID: 20900 RVA: 0x001E78B8 File Offset: 0x001E5AB8
		protected override string ReportStringProcessed(string str)
		{
			if (this.Talkee.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.ReduceResistance))
			{
				return "JobReport_ReduceResistance".Translate(this.Talkee);
			}
			return base.ReportStringProcessed(str);
		}

		// Token: 0x060051A5 RID: 20901 RVA: 0x001E78F3 File Offset: 0x001E5AF3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ChatWithPrisoner.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_ChatWithPrisoner.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
