using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011AD RID: 4525
	public class IncidentWorker_RaidFriendly : IncidentWorker_Raid
	{
		// Token: 0x06007110 RID: 28944 RVA: 0x00278E98 File Offset: 0x00277098
		public override bool FactionCanBeGroupSource(Faction f, IncidentParms parms, bool desperate = false)
		{
			List<Faction> list = (from x in ((Map)parms.target).attackTargetsCache.TargetsHostileToColony
			where GenHostility.IsActiveThreatToPlayer(x, false)
			select x into p
			select ((Thing)p).Faction).Distinct<Faction>().ToList<Faction>();
			Faction faction = parms.faction;
			parms.faction = f;
			if (!RaidStrategyDefOf.ImmediateAttackFriendly.Worker.CanUseWith(parms, null))
			{
				parms.faction = faction;
				return false;
			}
			parms.faction = faction;
			return base.FactionCanBeGroupSource(f, parms, desperate) && !f.Hidden && f.PlayerRelationKind == FactionRelationKind.Ally && (!list.Any<Faction>() || list.Any((Faction hf) => hf.HostileTo(f)));
		}

		// Token: 0x06007111 RID: 28945 RVA: 0x00278F9C File Offset: 0x0027719C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			return (from p in ((Map)parms.target).attackTargetsCache.TargetsHostileToColony
			where GenHostility.IsActiveThreatToPlayer(p, false)
			select p).Sum(delegate(IAttackTarget p)
			{
				Pawn pawn = p as Pawn;
				if (pawn != null)
				{
					return pawn.kindDef.combatPower;
				}
				return 0f;
			}) > 120f;
		}

		// Token: 0x06007112 RID: 28946 RVA: 0x00279018 File Offset: 0x00277218
		protected override bool TryResolveRaidFaction(IncidentParms parms)
		{
			if (parms.faction != null)
			{
				return true;
			}
			if (!base.CandidateFactions(parms, false).Any<Faction>())
			{
				return false;
			}
			parms.faction = base.CandidateFactions(parms, false).RandomElementByWeight((Faction fac) => (float)fac.PlayerGoodwill + 120.00001f);
			return true;
		}

		// Token: 0x06007113 RID: 28947 RVA: 0x00279073 File Offset: 0x00277273
		public override void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			if (parms.raidStrategy != null)
			{
				return;
			}
			parms.raidStrategy = RaidStrategyDefOf.ImmediateAttackFriendly;
		}

		// Token: 0x06007114 RID: 28948 RVA: 0x00279089 File Offset: 0x00277289
		protected override void ResolveRaidPoints(IncidentParms parms)
		{
			if (parms.points <= 0f)
			{
				parms.points = StorytellerUtility.DefaultThreatPointsNow(parms.target);
			}
		}

		// Token: 0x06007115 RID: 28949 RVA: 0x002790A9 File Offset: 0x002772A9
		protected override string GetLetterLabel(IncidentParms parms)
		{
			return parms.raidStrategy.letterLabelFriendly + ": " + parms.faction.Name;
		}

		// Token: 0x06007116 RID: 28950 RVA: 0x002790CC File Offset: 0x002772CC
		protected override string GetLetterText(IncidentParms parms, List<Pawn> pawns)
		{
			string text = string.Format(parms.raidArrivalMode.textFriendly, parms.faction.def.pawnsPlural, parms.faction.Name.ApplyTag(parms.faction));
			text += "\n\n";
			text += parms.raidStrategy.arrivalTextFriendly;
			Pawn pawn = pawns.Find((Pawn x) => x.Faction.leader == x);
			if (pawn != null)
			{
				text += "\n\n";
				text += "FriendlyRaidLeaderPresent".Translate(pawn.Faction.def.pawnsPlural, pawn.LabelShort, pawn.Named("LEADER"));
			}
			return text;
		}

		// Token: 0x06007117 RID: 28951 RVA: 0x002791AA File Offset: 0x002773AA
		protected override LetterDef GetLetterDef()
		{
			return LetterDefOf.PositiveEvent;
		}

		// Token: 0x06007118 RID: 28952 RVA: 0x002791B1 File Offset: 0x002773B1
		protected override string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsRaidFriendly".Translate(Faction.OfPlayer.def.pawnsPlural, parms.faction.def.pawnsPlural);
		}
	}
}
