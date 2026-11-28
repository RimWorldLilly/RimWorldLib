using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D56 RID: 3414
	public class ThinkNode_ConditionalExitTimedOut : ThinkNode_Conditional
	{
		// Token: 0x06005F2B RID: 24363 RVA: 0x0022BB21 File Offset: 0x00229D21
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.exitMapAfterTick >= 0 && Find.TickManager.TicksGame > pawn.mindState.exitMapAfterTick;
		}
	}
}
