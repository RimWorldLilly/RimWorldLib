using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D64 RID: 3428
	public class ThinkNode_ConditionalLyingDown : ThinkNode_Conditional
	{
		// Token: 0x06005F4A RID: 24394 RVA: 0x0022BE62 File Offset: 0x0022A062
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetPosture().Laying();
		}
	}
}
