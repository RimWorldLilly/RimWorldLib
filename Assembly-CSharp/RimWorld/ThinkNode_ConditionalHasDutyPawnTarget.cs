using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D61 RID: 3425
	public class ThinkNode_ConditionalHasDutyPawnTarget : ThinkNode_Conditional
	{
		// Token: 0x06005F43 RID: 24387 RVA: 0x0022BD76 File Offset: 0x00229F76
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focus.Thing is Pawn;
		}
	}
}
