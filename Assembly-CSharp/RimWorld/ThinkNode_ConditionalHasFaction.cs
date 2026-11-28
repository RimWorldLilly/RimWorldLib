using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D53 RID: 3411
	public class ThinkNode_ConditionalHasFaction : ThinkNode_Conditional
	{
		// Token: 0x06005F25 RID: 24357 RVA: 0x0022BAE6 File Offset: 0x00229CE6
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction != null;
		}
	}
}
