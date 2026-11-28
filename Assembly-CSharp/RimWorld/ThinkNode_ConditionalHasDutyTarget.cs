using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D60 RID: 3424
	public class ThinkNode_ConditionalHasDutyTarget : ThinkNode_Conditional
	{
		// Token: 0x06005F41 RID: 24385 RVA: 0x0022BD48 File Offset: 0x00229F48
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focus.IsValid;
		}
	}
}
