using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008CF RID: 2255
	public class ThinkNode_ActivityModePassive : ThinkNode_Conditional
	{
		// Token: 0x060045C4 RID: 17860 RVA: 0x001B75F1 File Offset: 0x001B57F1
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetComp<CompActivity>() != null && pawn.GetComp<CompActivity>().IsDormant;
		}
	}
}
