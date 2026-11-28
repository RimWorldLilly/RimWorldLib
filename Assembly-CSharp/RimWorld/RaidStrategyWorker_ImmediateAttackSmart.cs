using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D17 RID: 3351
	public class RaidStrategyWorker_ImmediateAttackSmart : RaidStrategyWorker
	{
		// Token: 0x06005E1A RID: 24090 RVA: 0x00227138 File Offset: 0x00225338
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			Faction faction = parms.faction;
			bool canTimeoutOrFlee = parms.canTimeoutOrFlee;
			return new LordJob_AssaultColony(faction, parms.canKidnap, canTimeoutOrFlee, false, true, parms.canSteal, false, false);
		}

		// Token: 0x06005E1B RID: 24091 RVA: 0x00227168 File Offset: 0x00225368
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction.def.canUseAvoidGrid;
		}
	}
}
