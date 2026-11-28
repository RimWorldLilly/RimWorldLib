using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D1C RID: 3356
	public class RaidStrategyWorker_Siege : RaidStrategyWorker
	{
		// Token: 0x06005E29 RID: 24105 RVA: 0x0022733C File Offset: 0x0022553C
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			IntVec3 siegeSpot = RCellFinder.FindSiegePositionFrom(parms.spawnCenter.IsValid ? parms.spawnCenter : pawns[0].PositionHeld, map, false, true, null, true);
			float num = parms.points * Rand.Range(0.2f, 0.3f);
			if (num < 60f)
			{
				num = 60f;
			}
			return new LordJob_Siege(parms.faction, siegeSpot, num);
		}

		// Token: 0x06005E2A RID: 24106 RVA: 0x002273A7 File Offset: 0x002255A7
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction.def.canSiege;
		}

		// Token: 0x06005E2B RID: 24107 RVA: 0x002273C5 File Offset: 0x002255C5
		public override bool CanUsePawnGenOption(float pointsTotal, PawnGenOption g, List<PawnGenOptionWithXenotype> chosenGroups, Faction faction = null)
		{
			return !g.kind.RaceProps.Animal && base.CanUsePawnGenOption(pointsTotal, g, chosenGroups, faction);
		}
	}
}
