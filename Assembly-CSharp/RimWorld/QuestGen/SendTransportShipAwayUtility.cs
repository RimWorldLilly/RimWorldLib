using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020021AB RID: 8619
	public static class SendTransportShipAwayUtility
	{
		// Token: 0x0600D6DD RID: 55005 RVA: 0x004FC218 File Offset: 0x004FA418
		public static void SendTransportShipAway(TransportShip transportShip, bool unloadContents, TransportShipDropMode unsatisfiedDropMode = TransportShipDropMode.NonRequired)
		{
			if (transportShip == null || transportShip.Disposed)
			{
				return;
			}
			if (!transportShip.started)
			{
				transportShip.Dispose();
				return;
			}
			if (!transportShip.ShipExistsAndIsSpawned || transportShip.LeavingSoonAutomatically)
			{
				return;
			}
			ShipJob_FlyAway shipJob_FlyAway = (ShipJob_FlyAway)ShipJobMaker.MakeShipJob(ShipJobDefOf.FlyAway);
			shipJob_FlyAway.dropMode = unsatisfiedDropMode;
			if (unloadContents)
			{
				transportShip.ForceJob(ShipJobDefOf.Unload);
				transportShip.AddJob(shipJob_FlyAway);
				return;
			}
			transportShip.ForceJob(shipJob_FlyAway);
		}
	}
}
