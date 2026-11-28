using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200118C RID: 4492
	public class IncidentWorker_OrbitalTraderArrival : IncidentWorker
	{
		// Token: 0x06007089 RID: 28809 RVA: 0x00276337 File Offset: 0x00274537
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && ((Map)parms.target).passingShipManager.passingShips.Count < 5;
		}

		// Token: 0x0600708A RID: 28810 RVA: 0x00276364 File Offset: 0x00274564
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (parms.traderKind == null)
			{
				if (!(from x in DefDatabase<TraderKindDef>.AllDefs
				where this.CanSpawn(map, x)
				select x).TryRandomElementByWeight((TraderKindDef traderDef) => traderDef.CalculatedCommonality, out parms.traderKind))
				{
					return false;
				}
			}
			TradeShip tradeShip = new TradeShip(parms.traderKind, this.GetFaction(parms.traderKind));
			tradeShip.WasAnnounced = false;
			if (map.listerBuildings.allBuildingsColonist.Any((Building b) => b.def.IsCommsConsole && (b.GetComp<CompPowerTrader>() == null || b.GetComp<CompPowerTrader>().PowerOn)))
			{
				base.SendStandardLetter(tradeShip.def.LabelCap, "TraderArrival".Translate(tradeShip.name, tradeShip.def.label, (tradeShip.Faction == null) ? "TraderArrivalNoFaction".Translate() : "TraderArrivalFromFaction".Translate(tradeShip.Faction.Named("FACTION"))), LetterDefOf.PositiveEvent, parms, LookTargets.Invalid, Array.Empty<NamedArgument>());
				tradeShip.WasAnnounced = true;
			}
			map.passingShipManager.AddShip(tradeShip);
			tradeShip.GenerateThings();
			return true;
		}

		// Token: 0x0600708B RID: 28811 RVA: 0x002764CC File Offset: 0x002746CC
		private Faction GetFaction(TraderKindDef trader)
		{
			if (trader.faction == null)
			{
				return null;
			}
			Faction result;
			if (!(from f in Find.FactionManager.AllFactions
			where f.def == trader.faction
			select f).TryRandomElement(out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x0600708C RID: 28812 RVA: 0x0027651C File Offset: 0x0027471C
		private bool CanSpawn(Map map, TraderKindDef trader)
		{
			if (!trader.orbital)
			{
				return false;
			}
			if (trader.faction == null)
			{
				return true;
			}
			Faction faction = this.GetFaction(trader);
			if (faction == null)
			{
				return false;
			}
			using (List<Pawn>.Enumerator enumerator = map.mapPawns.FreeColonists.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CanTradeWith(faction, trader).Accepted)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x040047B1 RID: 18353
		private const int MaxShips = 5;
	}
}
