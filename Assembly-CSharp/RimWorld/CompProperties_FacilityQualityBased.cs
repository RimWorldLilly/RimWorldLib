using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000ED7 RID: 3799
	public class CompProperties_FacilityQualityBased : CompProperties_Facility
	{
		// Token: 0x0600639A RID: 25498 RVA: 0x0023BE18 File Offset: 0x0023A018
		public CompProperties_FacilityQualityBased()
		{
			this.compClass = typeof(CompFacilityQualityBased);
		}

		// Token: 0x040037EA RID: 14314
		public Dictionary<StatDef, Dictionary<QualityCategory, float>> statOffsetsPerQuality;
	}
}
