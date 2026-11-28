using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D45 RID: 3397
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerPrisoner : ThinkNode_Conditional
	{
		// Token: 0x06005F07 RID: 24327 RVA: 0x0022B816 File Offset: 0x00229A16
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || (pawn.HostFaction == Faction.OfPlayer && pawn.guest.IsPrisoner);
		}
	}
}
