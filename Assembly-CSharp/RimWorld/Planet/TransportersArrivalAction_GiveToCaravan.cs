using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023D9 RID: 9177
	public class TransportersArrivalAction_GiveToCaravan : TransportersArrivalAction
	{
		// Token: 0x17002841 RID: 10305
		// (get) Token: 0x0600E4BA RID: 58554 RVA: 0x0054F88D File Offset: 0x0054DA8D
		public override bool GeneratesMap
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600E4BB RID: 58555 RVA: 0x0054F890 File Offset: 0x0054DA90
		public TransportersArrivalAction_GiveToCaravan()
		{
		}

		// Token: 0x0600E4BC RID: 58556 RVA: 0x0054F898 File Offset: 0x0054DA98
		public TransportersArrivalAction_GiveToCaravan(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x0600E4BD RID: 58557 RVA: 0x0054F8A7 File Offset: 0x0054DAA7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Caravan>(ref this.caravan, "caravan", false);
		}

		// Token: 0x0600E4BE RID: 58558 RVA: 0x0054F8C0 File Offset: 0x0054DAC0
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.caravan != null && !Find.WorldGrid.IsNeighborOrSame(this.caravan.Tile, destinationTile))
			{
				return false;
			}
			return TransportersArrivalAction_GiveToCaravan.CanGiveTo(pods, this.caravan);
		}

		// Token: 0x0600E4BF RID: 58559 RVA: 0x0054F914 File Offset: 0x0054DB14
		public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
		{
			for (int i = 0; i < transporters.Count; i++)
			{
				TransportersArrivalAction_GiveToCaravan.tmpContainedThings.Clear();
				TransportersArrivalAction_GiveToCaravan.tmpContainedThings.AddRange(transporters[i].innerContainer);
				for (int j = 0; j < TransportersArrivalAction_GiveToCaravan.tmpContainedThings.Count; j++)
				{
					transporters[i].innerContainer.Remove(TransportersArrivalAction_GiveToCaravan.tmpContainedThings[j]);
					this.caravan.AddPawnOrItem(TransportersArrivalAction_GiveToCaravan.tmpContainedThings[j], true);
				}
			}
			TransportersArrivalAction_GiveToCaravan.tmpContainedThings.Clear();
			Messages.Message("MessageTransportPodsArrivedAndAddedToCaravan".Translate(this.caravan.Name), this.caravan, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x0600E4C0 RID: 58560 RVA: 0x0054F9DA File Offset: 0x0054DBDA
		public static FloatMenuAcceptanceReport CanGiveTo(IEnumerable<IThingHolder> pods, Caravan caravan)
		{
			return caravan != null && caravan.Spawned && caravan.IsPlayerControlled;
		}

		// Token: 0x0600E4C1 RID: 58561 RVA: 0x0054F9F8 File Offset: 0x0054DBF8
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Action<PlanetTile, TransportersArrivalAction> launchAction, IEnumerable<IThingHolder> pods, Caravan caravan)
		{
			return TransportersArrivalActionUtility.GetFloatMenuOptions<TransportersArrivalAction_GiveToCaravan>(() => TransportersArrivalAction_GiveToCaravan.CanGiveTo(pods, caravan), () => new TransportersArrivalAction_GiveToCaravan(caravan), "GiveToCaravan".Translate(caravan.Label), launchAction, caravan.Tile, null);
		}

		// Token: 0x0400916B RID: 37227
		private Caravan caravan;

		// Token: 0x0400916C RID: 37228
		private static readonly List<Thing> tmpContainedThings = new List<Thing>();
	}
}
