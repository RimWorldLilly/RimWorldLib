using System;

namespace RimWorld
{
	// Token: 0x020011F3 RID: 4595
	public class StorytellerCompProperties_WorkSite : StorytellerCompProperties
	{
		// Token: 0x06007232 RID: 29234 RVA: 0x0027F0C1 File Offset: 0x0027D2C1
		public StorytellerCompProperties_WorkSite()
		{
			this.compClass = typeof(StorytellerComp_WorkSite);
		}

		// Token: 0x04004835 RID: 18485
		public IncidentDef incident;

		// Token: 0x04004836 RID: 18486
		public float baseMtbDays = 5f;
	}
}
