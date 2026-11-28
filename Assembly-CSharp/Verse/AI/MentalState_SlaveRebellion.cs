using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000884 RID: 2180
	public class MentalState_SlaveRebellion : MentalState
	{
		// Token: 0x060043F8 RID: 17400 RVA: 0x001AB010 File Offset: 0x001A9210
		public override void MentalStateTick(int delta)
		{
			base.MentalStateTick(delta);
			if (this.pawn.IsHashIntervalTick(500, delta) && this.pawn.CurJobDef != JobDefOf.InduceSlaveToRebel && SlaveRebellionUtility.FindSlaveForRebellion(this.pawn) == null)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x04002C30 RID: 11312
		private const int NoSlaveToRebelWithCheckInterval = 500;
	}
}
