using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EDB RID: 3803
	public class CompProperties_Mannable : CompProperties
	{
		// Token: 0x060063A2 RID: 25506 RVA: 0x0023BFF7 File Offset: 0x0023A1F7
		public CompProperties_Mannable()
		{
			this.compClass = typeof(CompMannable);
		}

		// Token: 0x040037FA RID: 14330
		public WorkTags manWorkType;

		// Token: 0x040037FB RID: 14331
		public List<PlanetLayerDef> planetLayerWhitelist;
	}
}
