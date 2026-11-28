using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D5D RID: 3421
	public class ThinkNode_ConditionalCannotReachMapEdge : ThinkNode_Conditional
	{
		// Token: 0x06005F3A RID: 24378 RVA: 0x0022BC8E File Offset: 0x00229E8E
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.CanReachMapEdge();
		}
	}
}
