using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D86 RID: 3462
	public class ThinkNodeConditional_EscapingHoldingPlatform : ThinkNode_Conditional
	{
		// Token: 0x06005F95 RID: 24469 RVA: 0x0022C91C File Offset: 0x0022AB1C
		protected override bool Satisfied(Pawn pawn)
		{
			CompHoldingPlatformTarget compHoldingPlatformTarget = pawn.TryGetComp<CompHoldingPlatformTarget>();
			return compHoldingPlatformTarget != null && compHoldingPlatformTarget.isEscaping;
		}
	}
}
