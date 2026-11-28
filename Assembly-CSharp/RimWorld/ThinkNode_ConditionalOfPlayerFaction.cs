using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D44 RID: 3396
	public class ThinkNode_ConditionalOfPlayerFaction : ThinkNode_Conditional
	{
		// Token: 0x06005F05 RID: 24325 RVA: 0x0022B7FF File Offset: 0x002299FF
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer;
		}
	}
}
