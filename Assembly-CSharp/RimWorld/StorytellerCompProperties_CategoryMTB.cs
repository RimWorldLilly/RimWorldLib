using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011C9 RID: 4553
	public class StorytellerCompProperties_CategoryMTB : StorytellerCompProperties
	{
		// Token: 0x060071CA RID: 29130 RVA: 0x0027E384 File Offset: 0x0027C584
		public StorytellerCompProperties_CategoryMTB()
		{
			this.compClass = typeof(StorytellerComp_CategoryMTB);
		}

		// Token: 0x040047F2 RID: 18418
		public float mtbDays = -1f;

		// Token: 0x040047F3 RID: 18419
		public SimpleCurve mtbDaysFactorByDaysPassedCurve;

		// Token: 0x040047F4 RID: 18420
		public IncidentCategoryDef category;
	}
}
