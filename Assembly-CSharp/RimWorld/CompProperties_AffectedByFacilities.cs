using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EBE RID: 3774
	public class CompProperties_AffectedByFacilities : CompProperties
	{
		// Token: 0x0600635A RID: 25434 RVA: 0x0023B2CA File Offset: 0x002394CA
		public CompProperties_AffectedByFacilities()
		{
			this.compClass = typeof(CompAffectedByFacilities);
		}

		// Token: 0x04003782 RID: 14210
		public List<ThingDef> linkableFacilities;
	}
}
