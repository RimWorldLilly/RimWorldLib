using System;

namespace RimWorld
{
	// Token: 0x020011C7 RID: 4551
	public class StorytellerCompProperties_CategoryIndividualMTBByBiome : StorytellerCompProperties
	{
		// Token: 0x060071C5 RID: 29125 RVA: 0x0027E2F5 File Offset: 0x0027C4F5
		public StorytellerCompProperties_CategoryIndividualMTBByBiome()
		{
			this.compClass = typeof(StorytellerComp_CategoryIndividualMTBByBiome);
		}

		// Token: 0x040047F0 RID: 18416
		public IncidentCategoryDef category;

		// Token: 0x040047F1 RID: 18417
		public bool applyCaravanVisibility;
	}
}
