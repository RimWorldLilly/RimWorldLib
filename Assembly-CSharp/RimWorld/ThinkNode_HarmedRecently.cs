using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D74 RID: 3444
	public class ThinkNode_HarmedRecently : ThinkNode_Conditional
	{
		// Token: 0x06005F6C RID: 24428 RVA: 0x0022C480 File Offset: 0x0022A680
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.lastHarmTick > 0 && Find.TickManager.TicksGame < pawn.mindState.lastHarmTick + this.thresholdTicks;
		}

		// Token: 0x040035C2 RID: 13762
		public int thresholdTicks = 2500;
	}
}
