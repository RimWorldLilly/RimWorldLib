using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D72 RID: 3442
	public class ThinkNode_ConditionalAnyEnemyInHostileMap : ThinkNode_Conditional
	{
		// Token: 0x06005F68 RID: 24424 RVA: 0x0022C228 File Offset: 0x0022A428
		protected override bool Satisfied(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return false;
			}
			Map map = pawn.Map;
			return !map.IsPlayerHome && map.ParentFaction != null && map.ParentFaction.HostileTo(Faction.OfPlayer) && GenHostility.AnyHostileActiveThreatToPlayer(map, true, false);
		}
	}
}
