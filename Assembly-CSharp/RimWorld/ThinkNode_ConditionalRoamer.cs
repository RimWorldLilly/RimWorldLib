using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D76 RID: 3446
	public class ThinkNode_ConditionalRoamer : ThinkNode_Conditional
	{
		// Token: 0x06005F70 RID: 24432 RVA: 0x0022C4DF File Offset: 0x0022A6DF
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Roamer;
		}
	}
}
