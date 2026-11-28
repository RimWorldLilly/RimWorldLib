using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D81 RID: 3457
	public class ThinkNode_ConditionalSpawned : ThinkNode_Conditional
	{
		// Token: 0x06005F89 RID: 24457 RVA: 0x0022C831 File Offset: 0x0022AA31
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Spawned;
		}
	}
}
