using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D4F RID: 3407
	public class ThinkNode_ConditionalDrafted : ThinkNode_Conditional
	{
		// Token: 0x06005F1C RID: 24348 RVA: 0x0022B9C3 File Offset: 0x00229BC3
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Drafted;
		}
	}
}
