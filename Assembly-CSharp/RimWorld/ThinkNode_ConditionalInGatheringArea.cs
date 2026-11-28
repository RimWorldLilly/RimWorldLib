using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D69 RID: 3433
	public class ThinkNode_ConditionalInGatheringArea : ThinkNode_Conditional
	{
		// Token: 0x06005F55 RID: 24405 RVA: 0x0022C048 File Offset: 0x0022A248
		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.mindState.duty == null)
			{
				return false;
			}
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			return GatheringsUtility.InGatheringArea(pawn.Position, cell, pawn.Map);
		}
	}
}
