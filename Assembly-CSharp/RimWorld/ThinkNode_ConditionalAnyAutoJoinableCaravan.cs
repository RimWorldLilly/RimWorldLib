using System;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D6B RID: 3435
	public class ThinkNode_ConditionalAnyAutoJoinableCaravan : ThinkNode_Conditional
	{
		// Token: 0x06005F59 RID: 24409 RVA: 0x0022C0B6 File Offset: 0x0022A2B6
		protected override bool Satisfied(Pawn pawn)
		{
			return CaravanExitMapUtility.FindCaravanToJoinFor(pawn) != null;
		}
	}
}
