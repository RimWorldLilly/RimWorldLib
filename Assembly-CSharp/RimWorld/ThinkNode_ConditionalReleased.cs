using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D42 RID: 3394
	public class ThinkNode_ConditionalReleased : ThinkNode_Conditional
	{
		// Token: 0x06005F01 RID: 24321 RVA: 0x0022B7D0 File Offset: 0x002299D0
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.guest != null && pawn.guest.Released;
		}
	}
}
