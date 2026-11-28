using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023DC RID: 9180
	public class TransportersArrivalAction_Trade : TransportersArrivalAction_VisitSettlement
	{
		// Token: 0x0600E4D1 RID: 58577 RVA: 0x0054FDA5 File Offset: 0x0054DFA5
		public TransportersArrivalAction_Trade()
		{
		}

		// Token: 0x0600E4D2 RID: 58578 RVA: 0x0054FDAD File Offset: 0x0054DFAD
		public TransportersArrivalAction_Trade(Settlement settlement, string arrivalMessageKey) : base(settlement, arrivalMessageKey)
		{
		}

		// Token: 0x0600E4D3 RID: 58579 RVA: 0x0054FDB8 File Offset: 0x0054DFB8
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			return TransportersArrivalAction_Trade.CanTradeWith(pods, this.settlement);
		}

		// Token: 0x0600E4D4 RID: 58580 RVA: 0x0054FDE4 File Offset: 0x0054DFE4
		public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
		{
			Pawn pawn = null;
			int num = 0;
			while (num < transporters.Count && pawn == null)
			{
				foreach (Thing thing in ((IEnumerable<Thing>)transporters[num].GetDirectlyHeldThings()))
				{
					Pawn pawn2 = thing as Pawn;
					if (pawn2 != null)
					{
						pawn = pawn2;
						break;
					}
				}
				num++;
			}
			base.Arrived(transporters, tile);
			if (pawn != null)
			{
				Caravan caravan = pawn.GetCaravan();
				if (caravan != null && CaravanArrivalAction_Trade.HasNegotiator(caravan, this.settlement))
				{
					CameraJumper.TryJumpAndSelect(caravan, CameraJumper.MovementMode.Pan);
					Pawn playerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan, this.settlement.Faction, this.settlement.TraderKind);
					Find.WindowStack.Add(new Dialog_Trade(playerNegotiator, this.settlement, false));
				}
			}
		}

		// Token: 0x0600E4D5 RID: 58581 RVA: 0x0054FEBC File Offset: 0x0054E0BC
		public static FloatMenuAcceptanceReport CanTradeWith(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			if (!TransportersArrivalAction_VisitSettlement.CanVisit(pods, settlement))
			{
				return false;
			}
			if (settlement.Faction == null || settlement.Faction == Faction.OfPlayer)
			{
				return false;
			}
			bool flag = false;
			foreach (IThingHolder thingHolder in pods)
			{
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				CompTransporter compTransporter = thingHolder as CompTransporter;
				if (compTransporter != null && CaravanShuttleUtility.IsCaravanShuttle(compTransporter))
				{
					directlyHeldThings = compTransporter.parent.GetCaravan().GetDirectlyHeldThings();
				}
				foreach (Thing thing in ((IEnumerable<Thing>)directlyHeldThings))
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null && pawn.RaceProps.Humanlike && pawn.CanTradeWith(settlement.Faction, settlement.TraderKind).Accepted)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			return flag && !settlement.HasMap && !settlement.Faction.def.permanentEnemy && !settlement.Faction.HostileTo(Faction.OfPlayer) && settlement.CanTradeNow;
		}
	}
}
