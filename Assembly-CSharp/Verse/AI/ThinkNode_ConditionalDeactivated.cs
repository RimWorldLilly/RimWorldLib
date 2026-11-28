using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008CE RID: 2254
	public class ThinkNode_ConditionalDeactivated : ThinkNode_Conditional
	{
		// Token: 0x060045C2 RID: 17858 RVA: 0x001B75E1 File Offset: 0x001B57E1
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsDeactivated();
		}
	}
}
