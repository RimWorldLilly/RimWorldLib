using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D46 RID: 3398
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerGuest : ThinkNode_Conditional
	{
		// Token: 0x06005F09 RID: 24329 RVA: 0x0022B849 File Offset: 0x00229A49
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || (pawn.HostFaction == Faction.OfPlayer && !pawn.guest.IsPrisoner);
		}
	}
}
