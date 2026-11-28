using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A4A RID: 2634
	public class JobDriver_Kidnap : JobDriver_TakeAndExitMap
	{
		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x06004F85 RID: 20357 RVA: 0x001E1367 File Offset: 0x001DF567
		protected Pawn Takee
		{
			get
			{
				return (Pawn)base.Item;
			}
		}

		// Token: 0x06004F86 RID: 20358 RVA: 0x001E1374 File Offset: 0x001DF574
		public override string GetReport()
		{
			if (this.Takee == null || this.pawn.HostileTo(this.Takee))
			{
				return base.GetReport();
			}
			return JobUtility.GetResolvedJobReport(JobDefOf.Rescue.reportString, this.Takee);
		}

		// Token: 0x06004F87 RID: 20359 RVA: 0x001E13B2 File Offset: 0x001DF5B2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Kidnap.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_Kidnap.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
