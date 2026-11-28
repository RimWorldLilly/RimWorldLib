using System;

namespace Verse.AI
{
	// Token: 0x020008D1 RID: 2257
	public class ThinkNode_ConditionalWorkMech : ThinkNode_Conditional
	{
		// Token: 0x060045C8 RID: 17864 RVA: 0x001B762F File Offset: 0x001B582F
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.IsWorkMech;
		}
	}
}
