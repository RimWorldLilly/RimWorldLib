using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A0E RID: 2574
	public class JobDriver_AbsorbXenogerm : JobDriver
	{
		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x06004DC6 RID: 19910 RVA: 0x001DBA88 File Offset: 0x001D9C88
		public Pawn Target
		{
			get
			{
				return this.job.targetA.Thing as Pawn;
			}
		}

		// Token: 0x06004DC7 RID: 19911 RVA: 0x001DBA9F File Offset: 0x001D9C9F
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004DC8 RID: 19912 RVA: 0x001DBAC2 File Offset: 0x001D9CC2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_AbsorbXenogerm.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_AbsorbXenogerm.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003080 RID: 12416
		public const int TicksToAbsorb = 60;
	}
}
