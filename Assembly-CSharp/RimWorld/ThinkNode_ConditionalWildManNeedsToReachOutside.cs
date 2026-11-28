using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D70 RID: 3440
	public class ThinkNode_ConditionalWildManNeedsToReachOutside : ThinkNode_Conditional
	{
		// Token: 0x06005F64 RID: 24420 RVA: 0x0022C202 File Offset: 0x0022A402
		protected override bool Satisfied(Pawn pawn)
		{
			return WildManUtility.WildManShouldReachOutsideNow(pawn);
		}
	}
}
