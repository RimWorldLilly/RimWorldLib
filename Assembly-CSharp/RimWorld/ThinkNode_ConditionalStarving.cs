using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D5B RID: 3419
	public class ThinkNode_ConditionalStarving : ThinkNode_Conditional
	{
		// Token: 0x06005F35 RID: 24373 RVA: 0x0022BBD7 File Offset: 0x00229DD7
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.needs.food != null && pawn.needs.food.CurCategory >= HungerCategory.Starving;
		}
	}
}
