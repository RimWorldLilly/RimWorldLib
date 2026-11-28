using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021A7 RID: 8615
	public class QuestPart_AddShipJob_WaitSendable : QuestPart_AddShipJob_Wait
	{
		// Token: 0x0600D6D0 RID: 54992 RVA: 0x004FBF68 File Offset: 0x004FA168
		public override ShipJob GetShipJob()
		{
			ShipJob_WaitSendable shipJob_WaitSendable = (ShipJob_WaitSendable)ShipJobMaker.MakeShipJob(this.shipJobDef);
			shipJob_WaitSendable.destination = this.destination;
			shipJob_WaitSendable.leaveImmediatelyWhenSatisfied = this.leaveImmediatelyWhenSatisfied;
			shipJob_WaitSendable.showGizmos = this.showGizmos;
			shipJob_WaitSendable.sendAwayIfAllDespawned = this.sendAwayIfAllDespawned;
			shipJob_WaitSendable.sendAwayIfAnyDespawnedDownedOrDead = this.sendAwayIfAnyDespawnedDownedOrDead;
			shipJob_WaitSendable.targetPlayerSettlement = this.targetPlayerSettlement;
			return shipJob_WaitSendable;
		}

		// Token: 0x0600D6D1 RID: 54993 RVA: 0x004FBFCD File Offset: 0x004FA1CD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.destination, "destination", false);
			Scribe_Values.Look<bool>(ref this.targetPlayerSettlement, "targetPlayerSettlement", false, false);
		}

		// Token: 0x04008813 RID: 34835
		public MapParent destination;

		// Token: 0x04008814 RID: 34836
		public bool targetPlayerSettlement;
	}
}
