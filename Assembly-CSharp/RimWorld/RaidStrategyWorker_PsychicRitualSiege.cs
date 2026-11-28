using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D1E RID: 3358
	public class RaidStrategyWorker_PsychicRitualSiege : RaidStrategyWorker_WithRequiredPawnKinds
	{
		// Token: 0x06005E33 RID: 24115 RVA: 0x00227484 File Offset: 0x00225684
		public override void MakeLords(IncidentParms parms, List<Pawn> pawns)
		{
			RaidStrategyWorker_PsychicRitualSiege.<>c__DisplayClass1_0 CS$<>8__locals1 = new RaidStrategyWorker_PsychicRitualSiege.<>c__DisplayClass1_0();
			CS$<>8__locals1.map = (Map)parms.target;
			Pawn pawn = this.BestInvoker(pawns);
			if (AnomalyIncidentUtility.IncidentShardChance(parms.points))
			{
				AnomalyIncidentUtility.PawnShardOnDeath(pawn);
			}
			parms.spawnCenter = RCellFinder.FindSiegePositionFrom(parms.spawnCenter.IsValid ? parms.spawnCenter : pawn.PositionHeld, CS$<>8__locals1.map, true, true, new Func<IntVec3, bool>(CS$<>8__locals1.<MakeLords>g__Validator|0), false);
			Lord lord = LordMaker.MakeNewLord(parms.faction, this.MakeLordJob(parms, CS$<>8__locals1.map, pawns, Rand.Int), CS$<>8__locals1.map, pawns);
			lord.inSignalLeave = parms.inSignalEnd;
			QuestUtility.AddQuestTag(lord, parms.questTag);
			pawn.health.AddHediff(HediffDefOf.PsychicTrance, null, null, null);
		}

		// Token: 0x06005E34 RID: 24116 RVA: 0x00227558 File Offset: 0x00225758
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			Pawn pawn = this.BestInvoker(pawns);
			PsychicRitualRoleAssignments psychicRitualRoleAssignments = parms.psychicRitualDef.BuildRoleAssignments(new TargetInfo(parms.spawnCenter, map, false));
			psychicRitualRoleAssignments.AddForcedRole(pawn, PsychicRitualRoleDefOf.Invoker, PsychicRitualRoleDef.Context.NonPlayerFaction);
			for (int i = 0; i < pawns.Count; i++)
			{
				if (pawns[i] != pawn)
				{
					psychicRitualRoleAssignments.AddForcedRole(pawns[i], PsychicRitualRoleDefOf.Defender, PsychicRitualRoleDef.Context.NonPlayerFaction);
				}
			}
			return new LordJob_PsychicRitualRepeating(parms.psychicRitualDef, psychicRitualRoleAssignments, RaidStrategyWorker_PsychicRitualSiege.PsychicRitualIterationsCountRange.RandomInRange, parms.spawnCenter, parms.points);
		}

		// Token: 0x06005E35 RID: 24117 RVA: 0x002275E8 File Offset: 0x002257E8
		private Pawn BestInvoker(List<Pawn> pawns)
		{
			Pawn pawn = pawns[0];
			for (int i = 1; i < pawns.Count; i++)
			{
				if (pawns[i].GetStatValue(StatDefOf.PsychicSensitivity, true, -1) > 0f && pawns[i].kindDef.isGoodPsychicRitualInvoker && pawns[i].kindDef.combatPower > pawn.kindDef.combatPower)
				{
					pawn = pawns[i];
				}
			}
			return pawn;
		}

		// Token: 0x06005E36 RID: 24118 RVA: 0x00227662 File Offset: 0x00225862
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && ModsConfig.AnomalyActive && parms.faction.def.canPsychicRitualSiege;
		}

		// Token: 0x06005E37 RID: 24119 RVA: 0x0022768E File Offset: 0x0022588E
		public override bool CanUsePawn(float pointsTotal, Pawn p, List<Pawn> otherPawns)
		{
			return otherPawns.Count != 0 || (p.kindDef.isGoodPsychicRitualInvoker && p.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) > 0f);
		}

		// Token: 0x06005E38 RID: 24120 RVA: 0x002276BD File Offset: 0x002258BD
		protected override bool MatchesRequiredPawnKind(PawnKindDef kind)
		{
			return kind.isGoodPsychicRitualInvoker;
		}

		// Token: 0x06005E39 RID: 24121 RVA: 0x002276C5 File Offset: 0x002258C5
		protected override int MinRequiredPawnsForPoints(float pointsTotal, Faction faction = null)
		{
			return 1;
		}

		// Token: 0x0400356A RID: 13674
		private static readonly IntRange PsychicRitualIterationsCountRange = new IntRange(3, 5);
	}
}
