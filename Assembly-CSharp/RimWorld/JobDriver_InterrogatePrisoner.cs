using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A9C RID: 2716
	public class JobDriver_InterrogatePrisoner : JobDriver
	{
		// Token: 0x17000ED9 RID: 3801
		// (get) Token: 0x060051B9 RID: 20921 RVA: 0x001E7AAC File Offset: 0x001E5CAC
		protected Pawn Prisoner
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x060051BA RID: 20922 RVA: 0x001E7AC3 File Offset: 0x001E5CC3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060051BB RID: 20923 RVA: 0x001E7AE6 File Offset: 0x001E5CE6
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_InterrogatePrisoner.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_InterrogatePrisoner.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400319C RID: 12700
		private const int NumTalks = 3;
	}
}
