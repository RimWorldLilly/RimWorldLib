using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D49 RID: 3401
	public class ThinkNode_ConditionalPlayerControlledMech : ThinkNode_Conditional
	{
		// Token: 0x06005F0F RID: 24335 RVA: 0x0022B8B5 File Offset: 0x00229AB5
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonyMechPlayerControlled;
		}
	}
}
