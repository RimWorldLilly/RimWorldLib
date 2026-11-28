using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002378 RID: 9080
	public class CaravanArrivalAction_VisitEscapeShip : CaravanArrivalAction
	{
		// Token: 0x170027AA RID: 10154
		// (get) Token: 0x0600E16B RID: 57707 RVA: 0x0053A81D File Offset: 0x00538A1D
		public override string Label
		{
			get
			{
				return "VisitEscapeShip".Translate(this.target.Label);
			}
		}

		// Token: 0x170027AB RID: 10155
		// (get) Token: 0x0600E16C RID: 57708 RVA: 0x0053A83E File Offset: 0x00538A3E
		public override string ReportString
		{
			get
			{
				return "CaravanVisiting".Translate(this.target.Label);
			}
		}

		// Token: 0x0600E16D RID: 57709 RVA: 0x0053A85F File Offset: 0x00538A5F
		public CaravanArrivalAction_VisitEscapeShip()
		{
		}

		// Token: 0x0600E16E RID: 57710 RVA: 0x0053A867 File Offset: 0x00538A67
		public CaravanArrivalAction_VisitEscapeShip(EscapeShipComp escapeShip)
		{
			this.target = (MapParent)escapeShip.parent;
		}

		// Token: 0x0600E16F RID: 57711 RVA: 0x0053A880 File Offset: 0x00538A80
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.target != null && this.target.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_VisitEscapeShip.CanVisit(caravan, this.target);
		}

		// Token: 0x0600E170 RID: 57712 RVA: 0x0053A8D0 File Offset: 0x00538AD0
		public override void Arrived(Caravan caravan)
		{
			if (!this.target.HasMap)
			{
				LongEventHandler.QueueLongEvent(delegate()
				{
					this.DoArrivalAction(caravan);
				}, "GeneratingMapForNewEncounter", false, null, true, false, null);
				return;
			}
			this.DoArrivalAction(caravan);
		}

		// Token: 0x0600E171 RID: 57713 RVA: 0x0053A926 File Offset: 0x00538B26
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.target, "target", false);
		}

		// Token: 0x0600E172 RID: 57714 RVA: 0x0053A940 File Offset: 0x00538B40
		private void DoArrivalAction(Caravan caravan)
		{
			bool flag = !this.target.HasMap;
			if (flag)
			{
				this.target.SetFaction(Faction.OfPlayer);
			}
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.target.Tile, null, null);
			LookTargets lookTargets = new LookTargets(caravan.PawnsListForReading);
			CaravanEnterMapUtility.Enter(caravan, orGenerateMap, CaravanEnterMode.Edge, CaravanDropInventoryMode.UnloadIndividually, false, null);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				Find.LetterStack.ReceiveLetter("EscapeShipFoundLabel".Translate(), (!Find.Storyteller.difficulty.allowBigThreats) ? "EscapeShipFoundPeaceful".Translate() : "EscapeShipFound".Translate(), LetterDefOf.PositiveEvent, new GlobalTargetInfo(this.target.Map.Center, this.target.Map, false), null, null, null, null, 0, true);
				return;
			}
			Find.LetterStack.ReceiveLetter("LetterLabelCaravanEnteredMap".Translate(this.target), "LetterCaravanEnteredMap".Translate(caravan.Label, this.target).CapitalizeFirst(), LetterDefOf.NeutralEvent, lookTargets, null, null, null, null, 0, true);
		}

		// Token: 0x0600E173 RID: 57715 RVA: 0x0053AA64 File Offset: 0x00538C64
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, MapParent escapeShip)
		{
			if (escapeShip == null || !escapeShip.Spawned || escapeShip.GetComponent<EscapeShipComp>() == null)
			{
				return false;
			}
			if (escapeShip.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(escapeShip.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true, false)));
			}
			return true;
		}

		// Token: 0x0600E174 RID: 57716 RVA: 0x0053AAC4 File Offset: 0x00538CC4
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, MapParent escapeShip)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitEscapeShip>(() => CaravanArrivalAction_VisitEscapeShip.CanVisit(caravan, escapeShip), () => new CaravanArrivalAction_VisitEscapeShip(escapeShip.GetComponent<EscapeShipComp>()), "VisitEscapeShip".Translate(escapeShip.Label), caravan, escapeShip.Tile, escapeShip, null);
		}

		// Token: 0x0400903D RID: 36925
		private MapParent target;
	}
}
