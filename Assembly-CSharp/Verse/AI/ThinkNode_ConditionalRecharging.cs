using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008D3 RID: 2259
	public class ThinkNode_ConditionalRecharging : ThinkNode_Conditional
	{
		// Token: 0x060045CC RID: 17868 RVA: 0x001B7654 File Offset: 0x001B5854
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsCharging();
		}
	}
}
