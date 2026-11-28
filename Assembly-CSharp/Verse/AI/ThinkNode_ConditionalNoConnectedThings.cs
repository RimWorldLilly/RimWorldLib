using System;

namespace Verse.AI
{
	// Token: 0x020008CA RID: 2250
	public class ThinkNode_ConditionalNoConnectedThings : ThinkNode_Conditional
	{
		// Token: 0x060045B9 RID: 17849 RVA: 0x001B74A8 File Offset: 0x001B56A8
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.connections == null || !pawn.connections.ConnectedThings.Any<Thing>();
		}
	}
}
