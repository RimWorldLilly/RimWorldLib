using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008D0 RID: 2256
	public class ThinkNode_ActivityModeActive : ThinkNode_Conditional
	{
		// Token: 0x060045C6 RID: 17862 RVA: 0x001B7610 File Offset: 0x001B5810
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetComp<CompActivity>() != null && pawn.GetComp<CompActivity>().IsActive;
		}
	}
}
