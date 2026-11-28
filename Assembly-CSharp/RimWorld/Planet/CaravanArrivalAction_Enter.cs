using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002375 RID: 9077
	public class CaravanArrivalAction_Enter : CaravanArrivalAction
	{
		// Token: 0x170027A4 RID: 10148
		// (get) Token: 0x0600E14E RID: 57678 RVA: 0x0053A182 File Offset: 0x00538382
		public override string Label
		{
			get
			{
				return "EnterMap".Translate(this.mapParent.Label);
			}
		}

		// Token: 0x170027A5 RID: 10149
		// (get) Token: 0x0600E14F RID: 57679 RVA: 0x0053A1A3 File Offset: 0x005383A3
		public override string ReportString
		{
			get
			{
				return "CaravanEntering".Translate(this.mapParent.Label);
			}
		}

		// Token: 0x0600E150 RID: 57680 RVA: 0x0053A1C4 File Offset: 0x005383C4
		public CaravanArrivalAction_Enter()
		{
		}

		// Token: 0x0600E151 RID: 57681 RVA: 0x0053A1CC File Offset: 0x005383CC
		public CaravanArrivalAction_Enter(MapParent mapParent)
		{
			this.mapParent = mapParent;
		}

		// Token: 0x0600E152 RID: 57682 RVA: 0x0053A1DC File Offset: 0x005383DC
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.mapParent != null && this.mapParent.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_Enter.CanEnter(caravan, this.mapParent);
		}

		// Token: 0x0600E153 RID: 57683 RVA: 0x0053A22C File Offset: 0x0053842C
		public override void Arrived(Caravan caravan)
		{
			Map map = this.mapParent.Map;
			if (map == null)
			{
				return;
			}
			CaravanDropInventoryMode dropInventoryMode = map.IsPlayerHome ? CaravanDropInventoryMode.UnloadIndividually : CaravanDropInventoryMode.DoNotDrop;
			bool draftColonists = this.mapParent.Faction != null && this.mapParent.Faction.HostileTo(Faction.OfPlayer);
			if (caravan.IsPlayerControlled || this.mapParent.Faction == Faction.OfPlayer)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelCaravanEnteredMap".Translate(this.mapParent), "LetterCaravanEnteredMap".Translate(caravan.Label, this.mapParent).CapitalizeFirst(), LetterDefOf.NeutralEvent, caravan.PawnsListForReading, null, null, null, null, 0, true);
			}
			CaravanEnterMapUtility.Enter(caravan, map, CaravanEnterMode.Edge, dropInventoryMode, draftColonists, null);
		}

		// Token: 0x0600E154 RID: 57684 RVA: 0x0053A2FE File Offset: 0x005384FE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x0600E155 RID: 57685 RVA: 0x0053A318 File Offset: 0x00538518
		public static FloatMenuAcceptanceReport CanEnter(Caravan caravan, MapParent mapParent)
		{
			if (mapParent == null || !mapParent.Spawned || !mapParent.HasMap)
			{
				return false;
			}
			if (mapParent.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(mapParent.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true, false)));
			}
			return true;
		}

		// Token: 0x0600E156 RID: 57686 RVA: 0x0053A378 File Offset: 0x00538578
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, MapParent mapParent)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_Enter>(() => CaravanArrivalAction_Enter.CanEnter(caravan, mapParent), () => new CaravanArrivalAction_Enter(mapParent), "EnterMap".Translate(mapParent.Label), caravan, mapParent.Tile, mapParent, null);
		}

		// Token: 0x0400903A RID: 36922
		private MapParent mapParent;
	}
}
