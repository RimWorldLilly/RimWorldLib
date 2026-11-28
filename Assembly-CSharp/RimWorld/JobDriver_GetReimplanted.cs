using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A0F RID: 2575
	public class JobDriver_GetReimplanted : JobDriver
	{
		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x06004DCB RID: 19915 RVA: 0x001DBB78 File Offset: 0x001D9D78
		public Pawn Target
		{
			get
			{
				return this.job.targetA.Thing as Pawn;
			}
		}

		// Token: 0x06004DCC RID: 19916 RVA: 0x001DBB8F File Offset: 0x001D9D8F
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004DCD RID: 19917 RVA: 0x001DBBB2 File Offset: 0x001D9DB2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_GetReimplanted.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_GetReimplanted.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
