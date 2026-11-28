using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AAA RID: 2730
	public class JobDriver_ExecuteSlave : JobDriver
	{
		// Token: 0x17000EE9 RID: 3817
		// (get) Token: 0x06005240 RID: 21056 RVA: 0x001EAAAE File Offset: 0x001E8CAE
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06005241 RID: 21057 RVA: 0x001EAAC5 File Offset: 0x001E8CC5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005242 RID: 21058 RVA: 0x001EAAE8 File Offset: 0x001E8CE8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ExecuteSlave.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_ExecuteSlave.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
