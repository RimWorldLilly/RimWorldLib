using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D7A RID: 3450
	public class ThinkNode_ConditionalTargetPawnNotInBed : ThinkNode_Conditional
	{
		// Token: 0x06005F79 RID: 24441 RVA: 0x0022C5A4 File Offset: 0x0022A7A4
		protected override bool Satisfied(Pawn pawn)
		{
			Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
			Building_Bed building_Bed = pawn.mindState.duty.focusThird.Thing as Building_Bed;
			if (building_Bed != null)
			{
				return pawn2.CurrentBed() != building_Bed;
			}
			return !pawn2.InBed();
		}
	}
}
