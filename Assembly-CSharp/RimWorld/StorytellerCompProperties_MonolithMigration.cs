using System;

namespace RimWorld
{
	// Token: 0x020011DA RID: 4570
	public class StorytellerCompProperties_MonolithMigration : StorytellerCompProperties
	{
		// Token: 0x060071F3 RID: 29171 RVA: 0x0027E788 File Offset: 0x0027C988
		public StorytellerCompProperties_MonolithMigration()
		{
			this.compClass = typeof(StorytellerComp_MonolithMigration);
		}

		// Token: 0x0400480D RID: 18445
		public float mtbDays;

		// Token: 0x0400480E RID: 18446
		public IncidentDef incident;
	}
}
