using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010F3 RID: 4339
	public static class SendShuttleAwayQuestPartUtility
	{
		// Token: 0x06006C52 RID: 27730 RVA: 0x00260100 File Offset: 0x0025E300
		public static void SendAway(Thing shuttle, bool dropEverything)
		{
			CompShuttle compShuttle = shuttle.TryGetComp<CompShuttle>();
			if (compShuttle.shipParent == null)
			{
				compShuttle.shipParent = TransportShipMaker.MakeTransportShip(TransportShipDefOf.Ship_Shuttle, null, shuttle);
			}
			if (dropEverything)
			{
				compShuttle.shipParent.ForceJob(ShipJobDefOf.Unload);
				compShuttle.shipParent.AddJob(ShipJobDefOf.FlyAway);
				return;
			}
			compShuttle.shipParent.ForceJob(ShipJobDefOf.FlyAway);
		}
	}
}
