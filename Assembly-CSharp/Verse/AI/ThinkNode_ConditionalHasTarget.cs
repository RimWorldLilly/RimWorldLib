using System;

namespace Verse.AI
{
	// Token: 0x020008C4 RID: 2244
	public class ThinkNode_ConditionalHasTarget : ThinkNode_Conditional
	{
		// Token: 0x060045AA RID: 17834 RVA: 0x001B7326 File Offset: 0x001B5526
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.enemyTarget != null;
		}
	}
}
