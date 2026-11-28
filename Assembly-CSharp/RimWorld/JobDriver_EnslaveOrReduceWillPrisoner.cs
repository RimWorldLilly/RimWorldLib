using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A9A RID: 2714
	public class JobDriver_EnslaveOrReduceWillPrisoner : JobDriver
	{
		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x060051AF RID: 20911 RVA: 0x001E79CF File Offset: 0x001E5BCF
		protected Pawn Prisoner
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x060051B0 RID: 20912 RVA: 0x001E79E6 File Offset: 0x001E5BE6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060051B1 RID: 20913 RVA: 0x001E7A09 File Offset: 0x001E5C09
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_EnslaveOrReduceWillPrisoner.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_EnslaveOrReduceWillPrisoner.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
