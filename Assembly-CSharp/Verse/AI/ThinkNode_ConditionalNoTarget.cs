using System;

namespace Verse.AI
{
	// Token: 0x020008C3 RID: 2243
	public class ThinkNode_ConditionalNoTarget : ThinkNode_Conditional
	{
		// Token: 0x060045A8 RID: 17832 RVA: 0x001B730E File Offset: 0x001B550E
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.enemyTarget == null;
		}
	}
}
