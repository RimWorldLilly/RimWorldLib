using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A9B RID: 2715
	public class JobDriver_Execute : JobDriver
	{
		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x060051B5 RID: 20917 RVA: 0x001E7A5A File Offset: 0x001E5C5A
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x060051B6 RID: 20918 RVA: 0x001E7A71 File Offset: 0x001E5C71
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060051B7 RID: 20919 RVA: 0x001E7A94 File Offset: 0x001E5C94
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Execute.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_Execute.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
