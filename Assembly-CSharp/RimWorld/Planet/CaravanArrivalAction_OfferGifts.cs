using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002376 RID: 9078
	public class CaravanArrivalAction_OfferGifts : CaravanArrivalAction
	{
		// Token: 0x170027A6 RID: 10150
		// (get) Token: 0x0600E157 RID: 57687 RVA: 0x0053A3ED File Offset: 0x005385ED
		public override string Label
		{
			get
			{
				return "OfferGifts".Translate();
			}
		}

		// Token: 0x170027A7 RID: 10151
		// (get) Token: 0x0600E158 RID: 57688 RVA: 0x0053A3FE File Offset: 0x005385FE
		public override string ReportString
		{
			get
			{
				return "CaravanOfferingGifts".Translate(this.settlement.Label);
			}
		}

		// Token: 0x0600E159 RID: 57689 RVA: 0x0053A41F File Offset: 0x0053861F
		public CaravanArrivalAction_OfferGifts()
		{
		}

		// Token: 0x0600E15A RID: 57690 RVA: 0x0053A427 File Offset: 0x00538627
		public CaravanArrivalAction_OfferGifts(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x0600E15B RID: 57691 RVA: 0x0053A438 File Offset: 0x00538638
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
			return CaravanArrivalAction_OfferGifts.CanOfferGiftsTo(caravan, this.settlement);
		}

		// Token: 0x0600E15C RID: 57692 RVA: 0x0053A488 File Offset: 0x00538688
		public override void Arrived(Caravan caravan)
		{
			CameraJumper.TryJumpAndSelect(caravan, CameraJumper.MovementMode.Pan);
			Pawn playerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan, null, null);
			Find.WindowStack.Add(new Dialog_Trade(playerNegotiator, this.settlement, true));
		}

		// Token: 0x0600E15D RID: 57693 RVA: 0x0053A4C1 File Offset: 0x005386C1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x0600E15E RID: 57694 RVA: 0x0053A4DC File Offset: 0x005386DC
		public static FloatMenuAcceptanceReport CanOfferGiftsTo(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && !settlement.HasMap && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && settlement.Faction.HostileTo(Faction.OfPlayer) && settlement.CanTradeNow && CaravanArrivalAction_OfferGifts.HasNegotiator(caravan);
		}

		// Token: 0x0600E15F RID: 57695 RVA: 0x0053A54C File Offset: 0x0053874C
		private static bool HasNegotiator(Caravan caravan)
		{
			Pawn pawn = BestCaravanPawnUtility.FindBestNegotiator(caravan, null, null);
			return pawn != null && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled;
		}

		// Token: 0x0600E160 RID: 57696 RVA: 0x0053A580 File Offset: 0x00538780
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_OfferGifts>(() => CaravanArrivalAction_OfferGifts.CanOfferGiftsTo(caravan, settlement), () => new CaravanArrivalAction_OfferGifts(settlement), "OfferGifts".Translate(), caravan, settlement.Tile, settlement, null);
		}

		// Token: 0x0400903B RID: 36923
		private Settlement settlement;
	}
}
