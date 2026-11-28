using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001974 RID: 6516
	public static class ShipLandingBeaconUtility
	{
		// Token: 0x0600A397 RID: 41879 RVA: 0x003A6694 File Offset: 0x003A4894
		public static List<ShipLandingArea> GetLandingZones(Map map)
		{
			ShipLandingBeaconUtility.tmpShipLandingAreas.Clear();
			if (!ModsConfig.RoyaltyActive)
			{
				return ShipLandingBeaconUtility.tmpShipLandingAreas;
			}
			foreach (Thing thing in map.listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon))
			{
				CompShipLandingBeacon compShipLandingBeacon = thing.TryGetComp<CompShipLandingBeacon>();
				if (compShipLandingBeacon != null && thing.Faction == Faction.OfPlayer)
				{
					foreach (ShipLandingArea shipLandingArea in compShipLandingBeacon.LandingAreas)
					{
						if (shipLandingArea.Active && !ShipLandingBeaconUtility.tmpShipLandingAreas.Contains(shipLandingArea))
						{
							shipLandingArea.RecalculateBlockingThing();
							ShipLandingBeaconUtility.tmpShipLandingAreas.Add(shipLandingArea);
						}
					}
				}
			}
			return ShipLandingBeaconUtility.tmpShipLandingAreas;
		}

		// Token: 0x0600A398 RID: 41880 RVA: 0x003A6788 File Offset: 0x003A4988
		public static void DrawLinesToNearbyBeacons(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map, Thing thing = null)
		{
			Vector3 a = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
			foreach (Thing thing2 in map.listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon))
			{
				if ((thing == null || thing != thing2) && thing2.Faction == Faction.OfPlayer)
				{
					CompShipLandingBeacon compShipLandingBeacon = thing2.TryGetComp<CompShipLandingBeacon>();
					if (compShipLandingBeacon != null && ShipLandingBeaconUtility.CanLinkTo(myPos, compShipLandingBeacon) && !GenThing.CloserThingBetween(myDef, myPos, thing2.Position, map, null))
					{
						GenDraw.DrawLineBetween(a, thing2.TrueCenter(), SimpleColor.White, 0.2f);
					}
				}
			}
			float minEdgeDistance = myDef.GetCompProperties<CompProperties_ShipLandingBeacon>().edgeLengthRange.min - 1f;
			float maxEdgeDistance = myDef.GetCompProperties<CompProperties_ShipLandingBeacon>().edgeLengthRange.max - 1f;
			foreach (Thing thing3 in map.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint))
			{
				if ((thing == null || thing != thing3) && thing3.def.entityDefToBuild == myDef && (myPos.x == thing3.Position.x || myPos.z == thing3.Position.z) && !ShipLandingBeaconUtility.AlignedDistanceTooShort(myPos, thing3.Position, minEdgeDistance) && !ShipLandingBeaconUtility.AlignedDistanceTooLong(myPos, thing3.Position, maxEdgeDistance) && !GenThing.CloserThingBetween(myDef, myPos, thing3.Position, map, null))
				{
					GenDraw.DrawLineBetween(a, thing3.TrueCenter(), SimpleColor.White, 0.2f);
				}
			}
		}

		// Token: 0x0600A399 RID: 41881 RVA: 0x003A6944 File Offset: 0x003A4B44
		public static bool AlignedDistanceTooShort(IntVec3 position, IntVec3 otherPos, float minEdgeDistance)
		{
			if (position.x == otherPos.x)
			{
				return (float)Mathf.Abs(position.z - otherPos.z) < minEdgeDistance;
			}
			return position.z == otherPos.z && (float)Mathf.Abs(position.x - otherPos.x) < minEdgeDistance;
		}

		// Token: 0x0600A39A RID: 41882 RVA: 0x003A699C File Offset: 0x003A4B9C
		private static bool AlignedDistanceTooLong(IntVec3 position, IntVec3 otherPos, float maxEdgeDistance)
		{
			if (position.x == otherPos.x)
			{
				return (float)Mathf.Abs(position.z - otherPos.z) >= maxEdgeDistance;
			}
			return position.z == otherPos.z && (float)Mathf.Abs(position.x - otherPos.x) >= maxEdgeDistance;
		}

		// Token: 0x0600A39B RID: 41883 RVA: 0x003A69FC File Offset: 0x003A4BFC
		public static bool CanLinkTo(IntVec3 position, CompShipLandingBeacon other)
		{
			if (position.x == other.parent.Position.x)
			{
				return other.parent.def.displayNumbersBetweenSameDefDistRange.Includes((float)(Mathf.Abs(position.z - other.parent.Position.z) + 1));
			}
			return position.z == other.parent.Position.z && other.parent.def.displayNumbersBetweenSameDefDistRange.Includes((float)(Mathf.Abs(position.x - other.parent.Position.x) + 1));
		}

		// Token: 0x04006096 RID: 24726
		public static List<ShipLandingArea> tmpShipLandingAreas = new List<ShipLandingArea>();
	}
}
