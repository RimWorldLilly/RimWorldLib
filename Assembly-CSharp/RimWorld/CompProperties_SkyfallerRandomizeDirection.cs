using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A00 RID: 6656
	public class CompProperties_SkyfallerRandomizeDirection : CompProperties
	{
		// Token: 0x0600A797 RID: 42903 RVA: 0x003BB102 File Offset: 0x003B9302
		public CompProperties_SkyfallerRandomizeDirection()
		{
			this.compClass = typeof(CompSkyfallerRandomizeDirection);
		}

		// Token: 0x0400629B RID: 25243
		public IntRange directionChangeInterval;

		// Token: 0x0400629C RID: 25244
		public float maxDeviationFromStartingAngle;
	}
}
