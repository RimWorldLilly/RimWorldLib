using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A71 RID: 2673
	public class JobDriver_RevenantEscape : JobDriver
	{
		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x060050A3 RID: 20643 RVA: 0x001E4B45 File Offset: 0x001E2D45
		private CompRevenant Comp
		{
			get
			{
				return this.pawn.TryGetComp<CompRevenant>();
			}
		}

		// Token: 0x060050A4 RID: 20644 RVA: 0x001E4B52 File Offset: 0x001E2D52
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.targetA.Cell);
			return true;
		}

		// Token: 0x060050A5 RID: 20645 RVA: 0x001E4B86 File Offset: 0x001E2D86
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RevenantEscape.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_RevenantEscape.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003155 RID: 12629
		private static int MinEscapeTime = 300;

		// Token: 0x04003156 RID: 12630
		private static int EscapedCheckInterval = 120;

		// Token: 0x04003157 RID: 12631
		private static int SmearMTBTicks = 60;

		// Token: 0x04003158 RID: 12632
		private static readonly int SmearLeavingDuration = Mathf.FloorToInt((float)JobDriver_RevenantEscape.MinEscapeTime * 1.4f);
	}
}
