using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021A9 RID: 8617
	public class QuestPart_SendTransportShipAwayOnCleanup : QuestPart
	{
		// Token: 0x0600D6D6 RID: 54998 RVA: 0x004FC0A6 File Offset: 0x004FA2A6
		public override bool QuestPartReserves(TransportShip ship)
		{
			return ship == this.transportShip;
		}

		// Token: 0x0600D6D7 RID: 54999 RVA: 0x004FC0B1 File Offset: 0x004FA2B1
		public override void Cleanup()
		{
			SendTransportShipAwayUtility.SendTransportShipAway(this.transportShip, this.unloadContents, this.unsatisfiedDropMode);
			this.transportShip = null;
		}

		// Token: 0x0600D6D8 RID: 55000 RVA: 0x004FC0D1 File Offset: 0x004FA2D1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.unloadContents, "unloadContents", false, false);
			Scribe_Values.Look<TransportShipDropMode>(ref this.unsatisfiedDropMode, "unsatisfiedDropMode", TransportShipDropMode.None, false);
			Scribe_References.Look<TransportShip>(ref this.transportShip, "transportShip", false);
		}

		// Token: 0x04008818 RID: 34840
		public TransportShip transportShip;

		// Token: 0x04008819 RID: 34841
		public TransportShipDropMode unsatisfiedDropMode;

		// Token: 0x0400881A RID: 34842
		public bool unloadContents;
	}
}
