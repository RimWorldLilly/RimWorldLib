using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A37 RID: 2615
	public class JobDriver_Flee : JobDriver
	{
		// Token: 0x06004EF0 RID: 20208 RVA: 0x001DF76C File Offset: 0x001DD96C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.GetTarget(TargetIndex.A).Cell);
			return true;
		}

		// Token: 0x06004EF1 RID: 20209 RVA: 0x001DF7AF File Offset: 0x001DD9AF
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Flee.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_Flee.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004EF2 RID: 20210 RVA: 0x001DF7C0 File Offset: 0x001DD9C0
		private void ExitMap()
		{
			this.pawn.ExitMap(true, CellRect.WholeMap(base.Map).GetClosestEdge(this.pawn.Position));
		}

		// Token: 0x040030D8 RID: 12504
		protected const TargetIndex DestInd = TargetIndex.A;

		// Token: 0x040030D9 RID: 12505
		protected const TargetIndex DangerInd = TargetIndex.B;
	}
}
