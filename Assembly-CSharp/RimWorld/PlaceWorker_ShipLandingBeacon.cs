using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002054 RID: 8276
	public class PlaceWorker_ShipLandingBeacon : PlaceWorker
	{
		// Token: 0x0600D10D RID: 53517 RVA: 0x004CE0F8 File Offset: 0x004CC2F8
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			if (def.HasComp(typeof(CompShipLandingBeacon)))
			{
				ShipLandingBeaconUtility.DrawLinesToNearbyBeacons(def, center, rot, currentMap, thing);
			}
		}
	}
}
