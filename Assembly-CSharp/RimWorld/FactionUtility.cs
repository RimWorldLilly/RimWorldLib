using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001607 RID: 5639
	public static class FactionUtility
	{
		// Token: 0x06008C88 RID: 35976 RVA: 0x003310F9 File Offset: 0x0032F2F9
		public static bool HostileTo(this Faction fac, Faction other)
		{
			return fac != null && other != null && other != fac && fac.RelationWith(other, false).kind == FactionRelationKind.Hostile;
		}

		// Token: 0x06008C89 RID: 35977 RVA: 0x00331117 File Offset: 0x0032F317
		public static bool AllyOrNeutralTo(this Faction fac, Faction other)
		{
			return !fac.HostileTo(other);
		}

		// Token: 0x06008C8A RID: 35978 RVA: 0x00331124 File Offset: 0x0032F324
		public static AcceptanceReport CanTradeWith(this Pawn p, Faction faction, TraderKindDef traderKind = null)
		{
			if (p.skills == null || p.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
			{
				return new AcceptanceReport("IncapableOfCapacity".Translate(SkillDefOf.Social.label));
			}
			if (faction != null)
			{
				if (faction.HostileTo(p.Faction))
				{
					return AcceptanceReport.WasRejected;
				}
				if (traderKind == null || traderKind.permitRequiredForTrading == null)
				{
					return AcceptanceReport.WasAccepted;
				}
				if (p.royalty == null || !p.royalty.HasPermit(traderKind.permitRequiredForTrading, faction))
				{
					return new AcceptanceReport("MessageNeedRoyalTitleToCallWithShip".Translate(traderKind.TitleRequiredToTrade));
				}
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06008C8B RID: 35979 RVA: 0x003311E0 File Offset: 0x0032F3E0
		public static Faction DefaultFactionFrom(FactionDef ft)
		{
			if (ft == null)
			{
				return null;
			}
			if (ft.isPlayer)
			{
				return Faction.OfPlayer;
			}
			Faction result;
			if (!(from x in Find.FactionManager.AllFactions
			where x.def == ft
			select x).TryRandomElement(out result) && !(from x in Find.FactionManager.AllFactions
			where x.def.replacesFaction != null && x.def.replacesFaction == ft
			select x).TryRandomElement(out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06008C8C RID: 35980 RVA: 0x00331262 File Offset: 0x0032F462
		public static bool IsPoliticallyProper(this Thing thing, Pawn pawn)
		{
			return thing.Faction == null || pawn.Faction == null || thing.Faction == pawn.Faction || thing.Faction == pawn.HostFaction;
		}

		// Token: 0x06008C8D RID: 35981 RVA: 0x00331299 File Offset: 0x0032F499
		public static bool IsPlayerSafe(this Faction faction)
		{
			return faction != null && faction.IsPlayer;
		}

		// Token: 0x06008C8E RID: 35982 RVA: 0x003312A8 File Offset: 0x0032F4A8
		public static void ResetAllFactionRelations()
		{
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				faction.RemoveAllRelations();
				foreach (Faction faction2 in Find.FactionManager.AllFactionsListForReading)
				{
					if (faction != faction2)
					{
						faction.TryMakeInitialRelationsWith(faction2);
					}
				}
			}
		}

		// Token: 0x06008C8F RID: 35983 RVA: 0x00331348 File Offset: 0x0032F548
		public static int GetSlavesInFactionCount(Faction faction)
		{
			if (faction == null)
			{
				return 0;
			}
			int num = 0;
			using (List<Pawn>.Enumerator enumerator = PawnsFinder.AllMaps_SpawnedPawnsInFaction(faction).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsSlave)
					{
						num++;
					}
				}
			}
			return num;
		}
	}
}
