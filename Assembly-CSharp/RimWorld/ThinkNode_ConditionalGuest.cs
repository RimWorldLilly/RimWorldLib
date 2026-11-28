using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D41 RID: 3393
	public class ThinkNode_ConditionalGuest : ThinkNode_Conditional
	{
		// Token: 0x06005EFF RID: 24319 RVA: 0x0022B7B3 File Offset: 0x002299B3
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.HostFaction != null && !pawn.IsPrisoner;
		}
	}
}
