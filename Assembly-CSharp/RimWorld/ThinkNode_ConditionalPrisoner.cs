using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D3F RID: 3391
	public class ThinkNode_ConditionalPrisoner : ThinkNode_Conditional
	{
		// Token: 0x06005EFB RID: 24315 RVA: 0x0022B76E File Offset: 0x0022996E
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsPrisoner;
		}
	}
}
