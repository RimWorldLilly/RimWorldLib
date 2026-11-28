using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D6A RID: 3434
	public class ThinkNode_ConditionalInNonPlayerHomeMap : ThinkNode_Conditional
	{
		// Token: 0x06005F57 RID: 24407 RVA: 0x0022C094 File Offset: 0x0022A294
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.MapHeld != null && !pawn.MapHeld.IsPlayerHome;
		}
	}
}
