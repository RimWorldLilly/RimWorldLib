using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D62 RID: 3426
	public class ThinkNode_ConditionalAtDutyLocation : ThinkNode_Conditional
	{
		// Token: 0x06005F45 RID: 24389 RVA: 0x0022BDAC File Offset: 0x00229FAC
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.Position == pawn.mindState.duty.focus.Cell;
		}
	}
}
