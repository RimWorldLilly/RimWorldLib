using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021A8 RID: 8616
	public class QuestPart_AddShipJob_FlyAway : QuestPart_AddShipJob
	{
		// Token: 0x0600D6D3 RID: 54995 RVA: 0x004FC000 File Offset: 0x004FA200
		public override ShipJob GetShipJob()
		{
			ShipJob_FlyAway shipJob_FlyAway = (ShipJob_FlyAway)ShipJobMaker.MakeShipJob(this.shipJobDef);
			shipJob_FlyAway.destinationTile = this.destinationTile;
			shipJob_FlyAway.arrivalAction = this.arrivalAction;
			shipJob_FlyAway.dropMode = this.dropMode;
			return shipJob_FlyAway;
		}

		// Token: 0x0600D6D4 RID: 54996 RVA: 0x004FC038 File Offset: 0x004FA238
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<PlanetTile>(ref this.destinationTile, "destinationTile", default(PlanetTile), false);
			Scribe_Deep.Look<TransportersArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
			Scribe_Values.Look<TransportShipDropMode>(ref this.dropMode, "dropMode", TransportShipDropMode.None, false);
		}

		// Token: 0x04008815 RID: 34837
		public PlanetTile destinationTile = PlanetTile.Invalid;

		// Token: 0x04008816 RID: 34838
		public TransportersArrivalAction arrivalAction;

		// Token: 0x04008817 RID: 34839
		public TransportShipDropMode dropMode = TransportShipDropMode.All;
	}
}
