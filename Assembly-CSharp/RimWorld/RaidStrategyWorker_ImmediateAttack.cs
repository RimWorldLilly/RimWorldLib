using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D15 RID: 3349
	public class RaidStrategyWorker_ImmediateAttack : RaidStrategyWorker
	{
		// Token: 0x06005E16 RID: 24086 RVA: 0x00227054 File Offset: 0x00225254
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			IntVec3 originCell = parms.spawnCenter.IsValid ? parms.spawnCenter : pawns[0].PositionHeld;
			if (parms.attackTargets != null && parms.attackTargets.Count > 0)
			{
				return new LordJob_AssaultThings(parms.faction, parms.attackTargets, 1f, false);
			}
			if (parms.faction.HostileTo(Faction.OfPlayer))
			{
				Faction faction = parms.faction;
				bool canTimeoutOrFlee = parms.canTimeoutOrFlee;
				return new LordJob_AssaultColony(faction, parms.canKidnap, canTimeoutOrFlee, false, false, parms.canSteal, false, false);
			}
			IntVec3 fallbackLocation;
			RCellFinder.TryFindRandomSpotJustOutsideColony(originCell, map, out fallbackLocation);
			return new LordJob_AssistColony(parms.faction, fallbackLocation);
		}
	}
}
