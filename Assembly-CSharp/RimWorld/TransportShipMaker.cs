using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001762 RID: 5986
	public static class TransportShipMaker
	{
		// Token: 0x060094FE RID: 38142 RVA: 0x003644B8 File Offset: 0x003626B8
		public static TransportShip MakeTransportShip(TransportShipDef def, IEnumerable<Thing> contents, Thing shipThing = null)
		{
			TransportShip transportShip = new TransportShip(def);
			transportShip.shipThing = (shipThing ?? ThingMaker.MakeThing(def.shipThing, null));
			CompShuttle compShuttle = transportShip.shipThing.TryGetComp<CompShuttle>();
			if (compShuttle != null)
			{
				compShuttle.shipParent = transportShip;
			}
			if (contents != null)
			{
				transportShip.TransporterComp.innerContainer.TryAddRangeOrTransfer(contents, true, true);
			}
			return transportShip;
		}
	}
}
