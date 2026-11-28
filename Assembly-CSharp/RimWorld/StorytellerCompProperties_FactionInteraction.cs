using System;

namespace RimWorld
{
	// Token: 0x020011D2 RID: 4562
	public class StorytellerCompProperties_FactionInteraction : StorytellerCompProperties
	{
		// Token: 0x060071DE RID: 29150 RVA: 0x0027E5E3 File Offset: 0x0027C7E3
		public StorytellerCompProperties_FactionInteraction()
		{
			this.compClass = typeof(StorytellerComp_FactionInteraction);
		}

		// Token: 0x040047FC RID: 18428
		public IncidentDef incident;

		// Token: 0x040047FD RID: 18429
		public float baseIncidentsPerYear;

		// Token: 0x040047FE RID: 18430
		public float minSpacingDays;

		// Token: 0x040047FF RID: 18431
		public StoryDanger minDanger;

		// Token: 0x04004800 RID: 18432
		public bool fullAlliesOnly;

		// Token: 0x04004801 RID: 18433
		public float minWealth;
	}
}
