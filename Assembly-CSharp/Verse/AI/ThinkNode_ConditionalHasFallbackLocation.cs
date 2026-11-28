using System;

namespace Verse.AI
{
	// Token: 0x020008C7 RID: 2247
	public class ThinkNode_ConditionalHasFallbackLocation : ThinkNode_Conditional
	{
		// Token: 0x060045B2 RID: 17842 RVA: 0x001B7410 File Offset: 0x001B5610
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focusSecond.IsValid;
		}
	}
}
