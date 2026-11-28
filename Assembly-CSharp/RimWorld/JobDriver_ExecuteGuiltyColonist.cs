using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A31 RID: 2609
	public class JobDriver_ExecuteGuiltyColonist : JobDriver
	{
		// Token: 0x17000E51 RID: 3665
		// (get) Token: 0x06004EC5 RID: 20165 RVA: 0x001DEC22 File Offset: 0x001DCE22
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06004EC6 RID: 20166 RVA: 0x001DEC39 File Offset: 0x001DCE39
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004EC7 RID: 20167 RVA: 0x001DEC5C File Offset: 0x001DCE5C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ExecuteGuiltyColonist.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_ExecuteGuiltyColonist.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
