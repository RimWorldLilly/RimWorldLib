using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D47 RID: 3399
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerGuestOrPlayerPrisoner : ThinkNode_Conditional
	{
		// Token: 0x06005F0B RID: 24331 RVA: 0x0022B87F File Offset: 0x00229A7F
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || pawn.HostFaction == Faction.OfPlayer;
		}
	}
}
