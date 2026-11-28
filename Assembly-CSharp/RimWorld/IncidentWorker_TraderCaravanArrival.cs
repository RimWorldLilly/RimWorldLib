using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020011BC RID: 4540
	public class IncidentWorker_TraderCaravanArrival : IncidentWorker_NeutralGroup
	{
		// Token: 0x1700133B RID: 4923
		// (get) Token: 0x0600716C RID: 29036 RVA: 0x0027BA7D File Offset: 0x00279C7D
		protected override PawnGroupKindDef PawnGroupKindDef
		{
			get
			{
				return PawnGroupKindDefOf.Trader;
			}
		}

		// Token: 0x0600716D RID: 29037 RVA: 0x0027BA84 File Offset: 0x00279C84
		public override bool FactionCanBeGroupSource(Faction f, IncidentParms parms, bool desperate = false)
		{
			if (!base.FactionCanBeGroupSource(f, parms, desperate) || f.def.caravanTraderKinds.Count == 0)
			{
				return false;
			}
			Map map = (Map)parms.target;
			return f.def.caravanTraderKinds.Any((TraderKindDef t) => this.TraderKindCommonality(t, map, f) > 0f);
		}

		// Token: 0x0600716E RID: 29038 RVA: 0x0027BB00 File Offset: 0x00279D00
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			return parms.faction == null || !NeutralGroupIncidentUtility.AnyBlockingHostileLord(map, parms.faction);
		}

		// Token: 0x0600716F RID: 29039 RVA: 0x0027BB40 File Offset: 0x00279D40
		protected override bool TryResolveParmsGeneral(IncidentParms parms)
		{
			if (!base.TryResolveParmsGeneral(parms))
			{
				return false;
			}
			if (parms.traderKind == null)
			{
				Map map = (Map)parms.target;
				if (!parms.faction.def.caravanTraderKinds.TryRandomElementByWeight((TraderKindDef traderDef) => this.TraderKindCommonality(traderDef, map, parms.faction), out parms.traderKind))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007170 RID: 29040 RVA: 0x0027BBCC File Offset: 0x00279DCC
		protected virtual float TraderKindCommonality(TraderKindDef traderKind, Map map, Faction faction)
		{
			if (traderKind.faction != null && faction.def != traderKind.faction)
			{
				return 0f;
			}
			if (ModsConfig.IdeologyActive && faction.ideos != null && traderKind.category == "Slaver")
			{
				using (IEnumerator<Ideo> enumerator = faction.ideos.AllIdeos.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.IdeoApprovesOfSlavery())
						{
							return 0f;
						}
					}
				}
			}
			if (traderKind.permitRequiredForTrading != null && !map.mapPawns.FreeColonists.Any((Pawn p) => p.royalty != null && p.royalty.HasPermit(traderKind.permitRequiredForTrading, faction)))
			{
				return 0f;
			}
			return traderKind.CalculatedCommonality;
		}

		// Token: 0x06007171 RID: 29041 RVA: 0x0027BCD4 File Offset: 0x00279ED4
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!base.TryResolveParms(parms))
			{
				return false;
			}
			if (parms.faction.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			List<Pawn> pawns = base.SpawnPawns(parms);
			if (pawns.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < pawns.Count; i++)
			{
				if (pawns[i].needs != null && pawns[i].needs.food != null)
				{
					pawns[i].needs.food.CurLevel = pawns[i].needs.food.MaxLevel;
				}
			}
			TraderKindDef traderKind = null;
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn = pawns[j];
				if (pawn.TraderKind != null)
				{
					traderKind = pawn.TraderKind;
					break;
				}
			}
			this.SendLetter(parms, pawns, traderKind);
			IntVec3 chillSpot;
			if (!RCellFinder.TryFindRandomSpotJustOutsideColony(pawns[0].Position, pawns[0].MapHeld, pawns[0], out chillSpot, delegate(IntVec3 c)
			{
				for (int k = 0; k < pawns.Count; k++)
				{
					if (!pawns[k].CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
					{
						return false;
					}
				}
				return true;
			}))
			{
				return false;
			}
			LordJob_TradeWithColony lordJob = new LordJob_TradeWithColony(parms.faction, chillSpot);
			LordMaker.MakeNewLord(parms.faction, lordJob, map, pawns);
			return true;
		}

		// Token: 0x06007172 RID: 29042 RVA: 0x0027BE64 File Offset: 0x0027A064
		protected virtual void SendLetter(IncidentParms parms, List<Pawn> pawns, TraderKindDef traderKind)
		{
			TaggedString baseLetterLabel = "LetterLabelTraderCaravanArrival".Translate(parms.faction.Name, traderKind.label).CapitalizeFirst();
			TaggedString taggedString = "LetterTraderCaravanArrival".Translate(parms.faction.NameColored, traderKind.label).CapitalizeFirst();
			taggedString += "\n\n" + "LetterCaravanArrivalCommonWarning".Translate();
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(pawns, ref baseLetterLabel, ref taggedString, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
			base.SendStandardLetter(baseLetterLabel, taggedString, LetterDefOf.PositiveEvent, parms, pawns[0], Array.Empty<NamedArgument>());
		}

		// Token: 0x06007173 RID: 29043 RVA: 0x0027BF36 File Offset: 0x0027A136
		protected override void ResolveParmsPoints(IncidentParms parms)
		{
			parms.points = TraderCaravanUtility.GenerateGuardPoints();
		}

		// Token: 0x040047CD RID: 18381
		public const string SlaverTraderKindCategory = "Slaver";
	}
}
