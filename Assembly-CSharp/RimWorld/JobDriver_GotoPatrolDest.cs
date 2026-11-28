using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009D7 RID: 2519
	public class JobDriver_GotoPatrolDest : JobDriver
	{
		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x06004C70 RID: 19568 RVA: 0x001D7086 File Offset: 0x001D5286
		private LocalTargetInfo Destination
		{
			get
			{
				return this.job.targetA;
			}
		}

		// Token: 0x06004C71 RID: 19569 RVA: 0x001D7094 File Offset: 0x001D5294
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.Destination.Cell);
			return true;
		}

		// Token: 0x06004C72 RID: 19570 RVA: 0x001D70D1 File Offset: 0x001D52D1
		protected override IEnumerable<Toil> MakeNewToils()
		{
			return new JobDriver_GotoPatrolDest.<MakeNewToils>d__5(-2);
		}

		// Token: 0x0400300B RID: 12299
		private const float WaitAtDestChance = 0.35f;

		// Token: 0x0400300C RID: 12300
		private static readonly IntRange WaitAtDestTicks = new IntRange(120, 9600);
	}
}
