using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A59 RID: 2649
	public class JobDriver_Open : JobDriver
	{
		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x06004FE5 RID: 20453 RVA: 0x001E2677 File Offset: 0x001E0877
		private IOpenable Openable
		{
			get
			{
				return (IOpenable)this.job.targetA.Thing;
			}
		}

		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x06004FE6 RID: 20454 RVA: 0x001E268E File Offset: 0x001E088E
		private Thing Target
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		// Token: 0x06004FE7 RID: 20455 RVA: 0x001E26A0 File Offset: 0x001E08A0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004FE8 RID: 20456 RVA: 0x001E26C3 File Offset: 0x001E08C3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Open.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_Open.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
