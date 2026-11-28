using System;

namespace Verse.AI
{
	// Token: 0x02000829 RID: 2089
	public class JobDriver_CastJump : JobDriver_CastVerbOnceStatic
	{
		// Token: 0x0600418B RID: 16779 RVA: 0x0019F963 File Offset: 0x0019DB63
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.targetA.Cell);
			return true;
		}
	}
}
