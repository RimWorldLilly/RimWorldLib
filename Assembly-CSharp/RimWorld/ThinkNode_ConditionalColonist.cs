using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D43 RID: 3395
	public class ThinkNode_ConditionalColonist : ThinkNode_Conditional
	{
		// Token: 0x06005F03 RID: 24323 RVA: 0x0022B7EF File Offset: 0x002299EF
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonist;
		}
	}
}
