using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002377 RID: 9079
	public class CaravanArrivalAction_Trade : CaravanArrivalAction
	{
		// Token: 0x170027A8 RID: 10152
		// (get) Token: 0x0600E161 RID: 57697 RVA: 0x0053A5E5 File Offset: 0x005387E5
		public override string Label
		{
			get
			{
				return "TradeWithSettlement".Translate(this.settlement.Label);
			}
		}

		// Token: 0x170027A9 RID: 10153
		// (get) Token: 0x0600E162 RID: 57698 RVA: 0x0053A606 File Offset: 0x00538806
		public override string ReportString
		{
			get
			{
				return "CaravanTrading".Translate(this.settlement.Label);
			}
		}

		// Token: 0x0600E163 RID: 57699 RVA: 0x0053A627 File Offset: 0x00538827
		public CaravanArrivalAction_Trade()
		{
		}

		// Token: 0x0600E164 RID: 57700 RVA: 0x0053A62F File Offset: 0x0053882F
		public CaravanArrivalAction_Trade(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x0600E165 RID: 57701 RVA: 0x0053A640 File Offset: 0x00538840
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.settlement != null && this.settlement.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_Trade.CanTradeWith(caravan, this.settlement);
		}

		// Token: 0x0600E166 RID: 57702 RVA: 0x0053A690 File Offset: 0x00538890
		public override void Arrived(Caravan caravan)
		{
			CameraJumper.TryJumpAndSelect(caravan, CameraJumper.MovementMode.Pan);
			Pawn playerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan, this.settlement.Faction, this.settlement.TraderKind);
			Find.WindowStack.Add(new Dialog_Trade(playerNegotiator, this.settlement, false));
		}

		// Token: 0x0600E167 RID: 57703 RVA: 0x0053A6DD File Offset: 0x005388DD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x0600E168 RID: 57704 RVA: 0x0053A6F8 File Offset: 0x005388F8
		public static FloatMenuAcceptanceReport CanTradeWith(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && !settlement.HasMap && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && !settlement.Faction.HostileTo(Faction.OfPlayer) && settlement.CanTradeNow && CaravanArrivalAction_Trade.HasNegotiator(caravan, settlement);
		}

		// Token: 0x0600E169 RID: 57705 RVA: 0x0053A768 File Offset: 0x00538968
		public static bool HasNegotiator(Caravan caravan, Settlement settlement)
		{
			Pawn pawn = BestCaravanPawnUtility.FindBestNegotiator(caravan, settlement.Faction, settlement.TraderKind);
			return pawn != null && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled;
		}

		// Token: 0x0600E16A RID: 57706 RVA: 0x0053A7A8 File Offset: 0x005389A8
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_Trade>(() => CaravanArrivalAction_Trade.CanTradeWith(caravan, settlement), () => new CaravanArrivalAction_Trade(settlement), "TradeWith".Translate(settlement.Label), caravan, settlement.Tile, settlement, null);
		}

		// Token: 0x0400903C RID: 36924
		private Settlement settlement;
	}
}
