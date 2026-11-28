using System;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D80 RID: 3456
	public class ThinkNode_ConditionalNotFormingCaravan : ThinkNode_Conditional
	{
		// Token: 0x06005F87 RID: 24455 RVA: 0x0022C81E File Offset: 0x0022AA1E
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.IsFormingCaravan();
		}
	}
}
