using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x0200081B RID: 2075
	public class JobDriver_Goto : JobDriver
	{
		// Token: 0x06004145 RID: 16709 RVA: 0x0019EA01 File Offset: 0x0019CC01
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.targetA.Cell);
			return true;
		}

		// Token: 0x06004146 RID: 16710 RVA: 0x0019EA35 File Offset: 0x0019CC35
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Goto.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_Goto.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004147 RID: 16711 RVA: 0x0019EA48 File Offset: 0x0019CC48
		private void TryExitMap()
		{
			if (this.job.failIfCantJoinOrCreateCaravan && !CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this.pawn))
			{
				return;
			}
			if (ModsConfig.BiotechActive)
			{
				MechanitorUtility.Notify_PawnGotoLeftMap(this.pawn, this.pawn.Map);
			}
			if (ModsConfig.AnomalyActive && !MetalhorrorUtility.TryPawnExitMap(this.pawn))
			{
				return;
			}
			this.pawn.ExitMap(true, CellRect.WholeMap(base.Map).GetClosestEdge(this.pawn.Position));
		}
	}
}
