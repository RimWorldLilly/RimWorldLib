using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D38 RID: 3384
	public class ShipJob_WaitSendable : ShipJob_Wait
	{
		// Token: 0x17001092 RID: 4242
		// (get) Token: 0x06005EDD RID: 24285 RVA: 0x0022AE4C File Offset: 0x0022904C
		protected override bool ShouldEnd
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001093 RID: 4243
		// (get) Token: 0x06005EDE RID: 24286 RVA: 0x0022AE4F File Offset: 0x0022904F
		public override bool HasDestination
		{
			get
			{
				return this.destination != null;
			}
		}

		// Token: 0x06005EDF RID: 24287 RVA: 0x0022AE5A File Offset: 0x0022905A
		public override IEnumerable<Gizmo> GetJobGizmos()
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x06005EE0 RID: 24288 RVA: 0x0022AE64 File Offset: 0x00229064
		protected override void SendAway()
		{
			if (this.targetPlayerSettlement && (this.destination == null || !this.destination.HasMap))
			{
				MapParent mapParent = null;
				foreach (Settlement settlement in Find.World.worldObjects.Settlements)
				{
					if (settlement.HasMap && settlement.Faction == Faction.OfPlayer && (mapParent == null || settlement.Map.mapPawns.ColonistCount > mapParent.Map.mapPawns.ColonistCount))
					{
						mapParent = settlement;
					}
				}
				if (mapParent == null)
				{
					mapParent = Find.AnyPlayerHomeMap.Parent;
				}
				if (mapParent == null)
				{
					if (!this.sentMessage)
					{
						Messages.Message("ShipNoSettlementToReturnTo".Translate(), this.transportShip.shipThing, MessageTypeDefOf.CautionInput, true);
						this.sentMessage = true;
					}
					return;
				}
				this.destination = mapParent;
			}
			ShipJob_FlyAway shipJob_FlyAway = (ShipJob_FlyAway)ShipJobMaker.MakeShipJob(ShipJobDefOf.FlyAway);
			shipJob_FlyAway.destinationTile = this.destination.Tile;
			shipJob_FlyAway.arrivalAction = new TransportersArrivalAction_TransportShip(this.destination, this.transportShip);
			shipJob_FlyAway.dropMode = TransportShipDropMode.None;
			this.transportShip.SetNextJob(shipJob_FlyAway);
			this.transportShip.TryGetNextJob();
		}

		// Token: 0x06005EE1 RID: 24289 RVA: 0x0022AFC4 File Offset: 0x002291C4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.destination, "destination", false);
			Scribe_Values.Look<bool>(ref this.sentMessage, "sentMessage", false, false);
		}

		// Token: 0x040035B6 RID: 13750
		public MapParent destination;

		// Token: 0x040035B7 RID: 13751
		private bool sentMessage;
	}
}
