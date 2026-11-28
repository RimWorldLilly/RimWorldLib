using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D75 RID: 3445
	public class ThinkNode_ConditionalRoped : ThinkNode_Conditional
	{
		// Token: 0x06005F6E RID: 24430 RVA: 0x0022C4C4 File Offset: 0x0022A6C4
		protected override bool Satisfied(Pawn pawn)
		{
			Pawn_RopeTracker roping = pawn.roping;
			return roping != null && roping.IsRoped;
		}
	}
}
