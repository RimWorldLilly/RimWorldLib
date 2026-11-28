using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020011B4 RID: 4532
	public class IncidentWorker_PsychicRitualSiege : IncidentWorker_RaidEnemy
	{
		// Token: 0x06007130 RID: 28976 RVA: 0x00279DD0 File Offset: 0x00277FD0
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (parms.psychicRitualDef == null && !(from x in DefDatabase<PsychicRitualDef>.AllDefs
			where x.aiCastable && x.minThreatPoints <= parms.points
			select x).TryRandomElement(out parms.psychicRitualDef))
			{
				return false;
			}
			parms.pawnGroupKind = PawnGroupKindDefOf.PsychicRitualSiege;
			return base.TryExecuteWorker(parms);
		}

		// Token: 0x06007131 RID: 28977 RVA: 0x00279E46 File Offset: 0x00278046
		protected override bool TryResolveRaidFaction(IncidentParms parms)
		{
			if (Faction.OfHoraxCult != null)
			{
				parms.faction = Faction.OfHoraxCult;
				return true;
			}
			return false;
		}

		// Token: 0x06007132 RID: 28978 RVA: 0x00279E5D File Offset: 0x0027805D
		public override void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			parms.raidStrategy = RaidStrategyDefOf.PsychicRitualSiege;
		}

		// Token: 0x06007133 RID: 28979 RVA: 0x00279E6A File Offset: 0x0027806A
		protected override string GetLetterLabel(IncidentParms parms)
		{
			return parms.raidStrategy.letterLabelEnemy;
		}

		// Token: 0x06007134 RID: 28980 RVA: 0x00279E78 File Offset: 0x00278078
		protected override string GetLetterText(IncidentParms parms, List<Pawn> pawns)
		{
			TaggedString t = parms.raidArrivalMode.textEnemy.Formatted(parms.faction.def.pawnsPlural, parms.faction.NameColored).CapitalizeFirst();
			t += "\n\n";
			if (!parms.psychicRitualDef.letterAIArrivedText.NullOrEmpty())
			{
				t += parms.psychicRitualDef.letterAIArrivedText;
			}
			else
			{
				t += parms.raidStrategy.arrivalTextEnemy.Formatted(parms.psychicRitualDef.label.Named("RITUAL"));
			}
			t += "\n\n" + "AttackersMayCastRitualMultipleTimes".Translate();
			Pawn pawn = pawns.Find((Pawn x) => x.Faction.leader == x);
			if (pawn != null)
			{
				t += "\n\n";
				t += "EnemyRaidLeaderPresent".Translate(pawn.Faction.def.pawnsPlural, pawn.LabelShort, pawn.Named("LEADER")).Resolve();
			}
			if (parms.raidAgeRestriction != null && !parms.raidAgeRestriction.arrivalTextExtra.NullOrEmpty())
			{
				t += "\n\n";
				t += parms.raidAgeRestriction.arrivalTextExtra.Formatted(parms.faction.def.pawnsPlural.Named("PAWNSPLURAL")).Resolve();
			}
			return t.Resolve();
		}
	}
}
