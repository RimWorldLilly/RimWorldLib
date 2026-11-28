using System;

namespace RimWorld
{
	// Token: 0x02000D16 RID: 3350
	public class RaidStrategyWorker_ImmediateAttackFriendly : RaidStrategyWorker_ImmediateAttack
	{
		// Token: 0x06005E18 RID: 24088 RVA: 0x00227105 File Offset: 0x00225305
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction != null && !parms.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
