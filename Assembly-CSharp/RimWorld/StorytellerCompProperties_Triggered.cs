using System;

namespace RimWorld
{
	// Token: 0x020011F1 RID: 4593
	public class StorytellerCompProperties_Triggered : StorytellerCompProperties
	{
		// Token: 0x0600722D RID: 29229 RVA: 0x0027EF19 File Offset: 0x0027D119
		public StorytellerCompProperties_Triggered()
		{
			this.compClass = typeof(StorytellerComp_Triggered);
		}

		// Token: 0x04004833 RID: 18483
		public IncidentDef incident;

		// Token: 0x04004834 RID: 18484
		public int delayTicks = 60;
	}
}
