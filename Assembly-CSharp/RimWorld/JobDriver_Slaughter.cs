using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009B0 RID: 2480
	public class JobDriver_Slaughter : JobDriver
	{
		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x06004B21 RID: 19233 RVA: 0x001D2A51 File Offset: 0x001D0C51
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06004B22 RID: 19234 RVA: 0x001D2A68 File Offset: 0x001D0C68
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004B23 RID: 19235 RVA: 0x001D2A8B File Offset: 0x001D0C8B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Slaughter.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_Slaughter.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04002FB6 RID: 12214
		public const int SlaughterDuration = 180;
	}
}
