using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D4B RID: 3403
	public class ThinkNode_ConditionalNonPlayerNonHostileFaction : ThinkNode_Conditional
	{
		// Token: 0x06005F14 RID: 24340 RVA: 0x0022B904 File Offset: 0x00229B04
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction != null && pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer);
		}
	}
}
