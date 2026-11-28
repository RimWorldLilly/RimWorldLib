using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D1B RID: 3355
	public class RaidStrategyWorker_StageThenAttack : RaidStrategyWorker
	{
		// Token: 0x06005E25 RID: 24101 RVA: 0x0022728C File Offset: 0x0022548C
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			IntVec3 stageLoc = RCellFinder.FindSiegePositionFrom(parms.spawnCenter.IsValid ? parms.spawnCenter : pawns[0].PositionHeld, map, true, false, null, true);
			if (!stageLoc.IsValid)
			{
				stageLoc = pawns[0].PositionHeld;
			}
			return new LordJob_StageThenAttack(parms.faction, stageLoc, raidSeed, true, true, true, null);
		}

		// Token: 0x06005E26 RID: 24102 RVA: 0x002272F5 File Offset: 0x002254F5
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction.def.canStageAttacks;
		}

		// Token: 0x06005E27 RID: 24103 RVA: 0x00227313 File Offset: 0x00225513
		public override bool CanUsePawnGenOption(float pointsTotal, PawnGenOption g, List<PawnGenOptionWithXenotype> chosenGroups, Faction faction = null)
		{
			return !g.kind.RaceProps.Animal && base.CanUsePawnGenOption(pointsTotal, g, chosenGroups, faction);
		}
	}
}
