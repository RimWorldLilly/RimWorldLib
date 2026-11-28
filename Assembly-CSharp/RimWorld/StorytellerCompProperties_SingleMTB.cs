using System;

namespace RimWorld
{
	// Token: 0x020011EC RID: 4588
	public class StorytellerCompProperties_SingleMTB : StorytellerCompProperties
	{
		// Token: 0x06007223 RID: 29219 RVA: 0x0027EE5C File Offset: 0x0027D05C
		public StorytellerCompProperties_SingleMTB()
		{
			this.compClass = typeof(StorytellerComp_SingleMTB);
		}

		// Token: 0x0400482B RID: 18475
		public IncidentDef incident;

		// Token: 0x0400482C RID: 18476
		public float mtbDays = 13f;
	}
}
