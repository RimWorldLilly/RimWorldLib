using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200117B RID: 4475
	public class IncidentWorker_GhoulAttack : IncidentWorker
	{
		// Token: 0x0600704C RID: 28748 RVA: 0x00275098 File Offset: 0x00273298
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Hostile, false, null))
			{
				return false;
			}
			Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.Ghoul, Faction.OfEntities, null);
			if (pawn == null)
			{
				return false;
			}
			pawn.health.overrideDeathOnDownedChance = 0f;
			if (AnomalyIncidentUtility.IncidentShardChance(parms.points))
			{
				AnomalyIncidentUtility.PawnShardOnDeath(pawn);
			}
			Rot4 rot = Rot4.FromAngleFlat((map.Center - intVec).AngleFlat);
			GenSpawn.Spawn(pawn, intVec, map, rot, WipeMode.Vanish, false, false);
			IncidentWorker.SendIncidentLetter(this.def.letterLabel, this.def.letterText, LetterDefOf.ThreatSmall, parms, pawn, this.def, Array.Empty<NamedArgument>());
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			LordMaker.MakeNewLord(parms.faction, new LordJob_AssaultColony(Faction.OfEntities, false, false, false, false, false, false, false), map, Gen.YieldSingle<Pawn>(pawn));
			return true;
		}
	}
}
