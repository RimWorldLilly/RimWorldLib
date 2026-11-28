using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001971 RID: 6513
	public class CompProperties_ShipLandingBeacon : CompProperties
	{
		// Token: 0x0600A380 RID: 41856 RVA: 0x003A5B62 File Offset: 0x003A3D62
		public CompProperties_ShipLandingBeacon()
		{
			this.compClass = typeof(CompShipLandingBeacon);
		}

		// Token: 0x0400608C RID: 24716
		public FloatRange edgeLengthRange;

		// Token: 0x0400608D RID: 24717
		public TerrainAffordanceDef landingAreaTerrainSupport;
	}
}
