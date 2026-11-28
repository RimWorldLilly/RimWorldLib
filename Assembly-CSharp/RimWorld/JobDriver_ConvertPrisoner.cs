using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A99 RID: 2713
	public class JobDriver_ConvertPrisoner : JobDriver
	{
		// Token: 0x17000ED6 RID: 3798
		// (get) Token: 0x060051A9 RID: 20905 RVA: 0x001E7944 File Offset: 0x001E5B44
		protected Pawn Prisoner
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x060051AA RID: 20906 RVA: 0x001E795B File Offset: 0x001E5B5B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060051AB RID: 20907 RVA: 0x001E797E File Offset: 0x001E5B7E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ConvertPrisoner.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_ConvertPrisoner.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400319B RID: 12699
		private const int NumTalks = 3;
	}
}
