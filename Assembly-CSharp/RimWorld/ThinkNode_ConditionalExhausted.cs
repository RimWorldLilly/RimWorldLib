using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D5F RID: 3423
	public class ThinkNode_ConditionalExhausted : ThinkNode_Conditional
	{
		// Token: 0x06005F3F RID: 24383 RVA: 0x0022BD19 File Offset: 0x00229F19
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.needs.rest != null && pawn.needs.rest.CurCategory >= RestCategory.Exhausted;
		}
	}
}
