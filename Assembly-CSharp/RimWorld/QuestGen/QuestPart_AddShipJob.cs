using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021A1 RID: 8609
	public class QuestPart_AddShipJob : QuestPart
	{
		// Token: 0x0600D6BB RID: 54971 RVA: 0x004FBAE2 File Offset: 0x004F9CE2
		public virtual ShipJob GetShipJob()
		{
			return this.shipJob ?? ShipJobMaker.MakeShipJob(this.shipJobDef);
		}

		// Token: 0x0600D6BC RID: 54972 RVA: 0x004FBAF9 File Offset: 0x004F9CF9
		public override bool QuestPartReserves(TransportShip ship)
		{
			return ship == this.transportShip;
		}

		// Token: 0x0600D6BD RID: 54973 RVA: 0x004FBB04 File Offset: 0x004F9D04
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				switch (this.shipJobStartMode)
				{
				case ShipJobStartMode.Queue:
					this.transportShip.AddJob(this.GetShipJob());
					break;
				case ShipJobStartMode.Instant:
					this.transportShip.AddJob(this.GetShipJob());
					this.transportShip.TryGetNextJob();
					break;
				case ShipJobStartMode.Force:
					this.transportShip.ForceJob(this.GetShipJob());
					break;
				case ShipJobStartMode.Force_DelayCurrent:
					this.transportShip.ForceJob_DelayCurrent(this.GetShipJob());
					break;
				}
				this.shipJob = null;
			}
		}

		// Token: 0x0600D6BE RID: 54974 RVA: 0x004FBB9F File Offset: 0x004F9D9F
		public override void Cleanup()
		{
			base.Cleanup();
			this.transportShip = null;
		}

		// Token: 0x0600D6BF RID: 54975 RVA: 0x004FBBB0 File Offset: 0x004F9DB0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<ShipJobStartMode>(ref this.shipJobStartMode, "shipJobStartMode", ShipJobStartMode.Queue, false);
			Scribe_Deep.Look<ShipJob>(ref this.shipJob, "shipJob", Array.Empty<object>());
			Scribe_Defs.Look<ShipJobDef>(ref this.shipJobDef, "shipJobDef");
			Scribe_References.Look<TransportShip>(ref this.transportShip, "transportShip", false);
		}

		// Token: 0x04008804 RID: 34820
		public string inSignal;

		// Token: 0x04008805 RID: 34821
		public ShipJobDef shipJobDef;

		// Token: 0x04008806 RID: 34822
		public TransportShip transportShip;

		// Token: 0x04008807 RID: 34823
		public ShipJobStartMode shipJobStartMode;

		// Token: 0x04008808 RID: 34824
		public ShipJob shipJob;
	}
}
