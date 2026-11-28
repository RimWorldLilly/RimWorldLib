using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002379 RID: 9081
	public class CaravanArrivalAction_VisitPeaceTalks : CaravanArrivalAction
	{
		// Token: 0x170027AC RID: 10156
		// (get) Token: 0x0600E175 RID: 57717 RVA: 0x0053AB39 File Offset: 0x00538D39
		public override string Label
		{
			get
			{
				return "VisitPeaceTalks".Translate(this.peaceTalks.Label);
			}
		}

		// Token: 0x170027AD RID: 10157
		// (get) Token: 0x0600E176 RID: 57718 RVA: 0x0053AB5A File Offset: 0x00538D5A
		public override string ReportString
		{
			get
			{
				return "CaravanVisiting".Translate(this.peaceTalks.Label);
			}
		}

		// Token: 0x0600E177 RID: 57719 RVA: 0x0053AB7B File Offset: 0x00538D7B
		public CaravanArrivalAction_VisitPeaceTalks()
		{
		}

		// Token: 0x0600E178 RID: 57720 RVA: 0x0053AB83 File Offset: 0x00538D83
		public CaravanArrivalAction_VisitPeaceTalks(PeaceTalks peaceTalks)
		{
			this.peaceTalks = peaceTalks;
		}

		// Token: 0x0600E179 RID: 57721 RVA: 0x0053AB94 File Offset: 0x00538D94
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.peaceTalks != null && this.peaceTalks.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_VisitPeaceTalks.CanVisit(caravan, this.peaceTalks);
		}

		// Token: 0x0600E17A RID: 57722 RVA: 0x0053ABE2 File Offset: 0x00538DE2
		public override void Arrived(Caravan caravan)
		{
			this.peaceTalks.Notify_CaravanArrived(caravan);
		}

		// Token: 0x0600E17B RID: 57723 RVA: 0x0053ABF0 File Offset: 0x00538DF0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<PeaceTalks>(ref this.peaceTalks, "peaceTalks", false);
		}

		// Token: 0x0600E17C RID: 57724 RVA: 0x0053AC09 File Offset: 0x00538E09
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, PeaceTalks peaceTalks)
		{
			return peaceTalks != null && peaceTalks.Spawned;
		}

		// Token: 0x0600E17D RID: 57725 RVA: 0x0053AC1C File Offset: 0x00538E1C
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, PeaceTalks peaceTalks)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitPeaceTalks>(() => CaravanArrivalAction_VisitPeaceTalks.CanVisit(caravan, peaceTalks), () => new CaravanArrivalAction_VisitPeaceTalks(peaceTalks), "VisitPeaceTalks".Translate(peaceTalks.Label), caravan, peaceTalks.Tile, peaceTalks, null);
		}

		// Token: 0x0400903E RID: 36926
		private PeaceTalks peaceTalks;
	}
}
