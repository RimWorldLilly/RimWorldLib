using System;

namespace Verse
{
	// Token: 0x02000442 RID: 1090
	public class HediffComp_GiveNeurocharge : HediffComp
	{
		// Token: 0x06001F56 RID: 8022 RVA: 0x000BEBD7 File Offset: 0x000BCDD7
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.parent.pawn.health.lastReceivedNeuralSuperchargeTick = Find.TickManager.TicksGame;
		}
	}
}
