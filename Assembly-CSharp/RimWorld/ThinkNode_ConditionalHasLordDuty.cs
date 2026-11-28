using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D4D RID: 3405
	public class ThinkNode_ConditionalHasLordDuty : ThinkNode_Conditional
	{
		// Token: 0x06005F18 RID: 24344 RVA: 0x0022B96E File Offset: 0x00229B6E
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetLord() != null && pawn.GetLord().CurLordToil.AssignsDuties;
		}
	}
}
