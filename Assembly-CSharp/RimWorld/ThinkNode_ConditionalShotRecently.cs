using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D7D RID: 3453
	public class ThinkNode_ConditionalShotRecently : ThinkNode_Conditional
	{
		// Token: 0x06005F81 RID: 24449 RVA: 0x0022C6E9 File Offset: 0x0022A8E9
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.lastRangedHarmTick > 0 && Find.TickManager.TicksGame < pawn.mindState.lastRangedHarmTick + this.thresholdTicks;
		}

		// Token: 0x040035C9 RID: 13769
		public int thresholdTicks = 2500;
	}
}
