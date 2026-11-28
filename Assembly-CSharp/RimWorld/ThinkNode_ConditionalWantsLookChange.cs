using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D78 RID: 3448
	public class ThinkNode_ConditionalWantsLookChange : ThinkNode_Conditional
	{
		// Token: 0x06005F75 RID: 24437 RVA: 0x0022C56B File Offset: 0x0022A76B
		protected override bool Satisfied(Pawn pawn)
		{
			return ModsConfig.IdeologyActive && pawn.style != null && pawn.style.LookChangeDesired;
		}
	}
}
