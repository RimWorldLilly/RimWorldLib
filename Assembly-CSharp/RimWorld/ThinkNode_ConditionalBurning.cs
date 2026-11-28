using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D51 RID: 3409
	public class ThinkNode_ConditionalBurning : ThinkNode_Conditional
	{
		// Token: 0x06005F21 RID: 24353 RVA: 0x0022BA9F File Offset: 0x00229C9F
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.HasAttachment(ThingDefOf.Fire);
		}
	}
}
