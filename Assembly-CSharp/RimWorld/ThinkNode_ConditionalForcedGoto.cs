using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D57 RID: 3415
	public class ThinkNode_ConditionalForcedGoto : ThinkNode_Conditional
	{
		// Token: 0x06005F2D RID: 24365 RVA: 0x0022BB52 File Offset: 0x00229D52
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.forcedGotoPosition.IsValid;
		}
	}
}
