using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D3B RID: 3387
	public class ThinkNode_ConditionalDowned : ThinkNode_Conditional
	{
		// Token: 0x06005EF3 RID: 24307 RVA: 0x0022B6D4 File Offset: 0x002298D4
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Downed;
		}
	}
}
