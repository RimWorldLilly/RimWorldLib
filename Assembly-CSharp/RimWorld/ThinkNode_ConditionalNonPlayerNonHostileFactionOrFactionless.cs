using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D4C RID: 3404
	public class ThinkNode_ConditionalNonPlayerNonHostileFactionOrFactionless : ThinkNode_Conditional
	{
		// Token: 0x06005F16 RID: 24342 RVA: 0x0022B938 File Offset: 0x00229B38
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == null || (pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer));
		}
	}
}
