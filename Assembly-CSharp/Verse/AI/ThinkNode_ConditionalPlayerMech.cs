using System;

namespace Verse.AI
{
	// Token: 0x020008D2 RID: 2258
	public class ThinkNode_ConditionalPlayerMech : ThinkNode_Conditional
	{
		// Token: 0x060045CA RID: 17866 RVA: 0x001B7644 File Offset: 0x001B5844
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonyMech;
		}
	}
}
