using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023DB RID: 9179
	public class TransportersArrivalAction_VisitSettlement : TransportersArrivalAction_FormCaravan
	{
		// Token: 0x0600E4CB RID: 58571 RVA: 0x0054FC7E File Offset: 0x0054DE7E
		public TransportersArrivalAction_VisitSettlement()
		{
		}

		// Token: 0x0600E4CC RID: 58572 RVA: 0x0054FC86 File Offset: 0x0054DE86
		public TransportersArrivalAction_VisitSettlement(Settlement settlement, string arrivalMessageKey) : base(arrivalMessageKey)
		{
			this.settlement = settlement;
		}

		// Token: 0x0600E4CD RID: 58573 RVA: 0x0054FC96 File Offset: 0x0054DE96
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x0600E4CE RID: 58574 RVA: 0x0054FCB0 File Offset: 0x0054DEB0
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.settlement != null && this.settlement.Tile != destinationTile)
			{
				return false;
			}
			return TransportersArrivalAction_VisitSettlement.CanVisit(pods, this.settlement);
		}

		// Token: 0x0600E4CF RID: 58575 RVA: 0x0054FCFE File Offset: 0x0054DEFE
		public static FloatMenuAcceptanceReport CanVisit(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			if (settlement == null || !settlement.Spawned || !settlement.Visitable)
			{
				return false;
			}
			if (!TransportersArrivalActionUtility.AnyPotentialCaravanOwner(pods, Faction.OfPlayer))
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600E4D0 RID: 58576 RVA: 0x0054FD34 File Offset: 0x0054DF34
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Action<PlanetTile, TransportersArrivalAction> launchAction, IEnumerable<IThingHolder> pods, Settlement settlement, bool isShuttle)
		{
			return TransportersArrivalActionUtility.GetFloatMenuOptions<TransportersArrivalAction_VisitSettlement>(() => TransportersArrivalAction_VisitSettlement.CanVisit(pods, settlement), () => new TransportersArrivalAction_VisitSettlement(settlement, isShuttle ? "MessageShuttleArrived" : "MessageTransportPodsArrived"), "VisitSettlement".Translate(settlement.Label), launchAction, settlement.Tile, null);
		}

		// Token: 0x04009171 RID: 37233
		protected Settlement settlement;
	}
}
