using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200237A RID: 9082
	public class CaravanArrivalAction_VisitSettlement : CaravanArrivalAction
	{
		// Token: 0x170027AE RID: 10158
		// (get) Token: 0x0600E17E RID: 57726 RVA: 0x0053AC91 File Offset: 0x00538E91
		public override string Label
		{
			get
			{
				return "VisitSettlement".Translate(this.settlement.Label);
			}
		}

		// Token: 0x170027AF RID: 10159
		// (get) Token: 0x0600E17F RID: 57727 RVA: 0x0053ACB2 File Offset: 0x00538EB2
		public override string ReportString
		{
			get
			{
				return "CaravanVisiting".Translate(this.settlement.Label);
			}
		}

		// Token: 0x0600E180 RID: 57728 RVA: 0x0053ACD3 File Offset: 0x00538ED3
		public CaravanArrivalAction_VisitSettlement()
		{
		}

		// Token: 0x0600E181 RID: 57729 RVA: 0x0053ACDB File Offset: 0x00538EDB
		public CaravanArrivalAction_VisitSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x0600E182 RID: 57730 RVA: 0x0053ACEC File Offset: 0x00538EEC
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
			return CaravanArrivalAction_VisitSettlement.CanVisit(caravan, this.settlement);
		}

		// Token: 0x0600E183 RID: 57731 RVA: 0x0053AD3C File Offset: 0x00538F3C
		public override void Arrived(Caravan caravan)
		{
			if (caravan.IsPlayerControlled)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelCaravanEnteredMap".Translate(this.settlement), "LetterCaravanEnteredMap".Translate(caravan.Label, this.settlement).CapitalizeFirst(), LetterDefOf.NeutralEvent, caravan, null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600E184 RID: 57732 RVA: 0x0053ADA9 File Offset: 0x00538FA9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x0600E185 RID: 57733 RVA: 0x0053ADC2 File Offset: 0x00538FC2
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && settlement.Visitable;
		}

		// Token: 0x0600E186 RID: 57734 RVA: 0x0053ADE0 File Offset: 0x00538FE0
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitSettlement>(() => CaravanArrivalAction_VisitSettlement.CanVisit(caravan, settlement), () => new CaravanArrivalAction_VisitSettlement(settlement), "VisitSettlement".Translate(settlement.Label), caravan, settlement.Tile, settlement, null);
		}

		// Token: 0x0400903F RID: 36927
		private Settlement settlement;
	}
}
