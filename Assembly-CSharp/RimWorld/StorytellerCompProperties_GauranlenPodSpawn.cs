using System;

namespace RimWorld
{
	// Token: 0x020011D4 RID: 4564
	public class StorytellerCompProperties_GauranlenPodSpawn : StorytellerCompProperties
	{
		// Token: 0x060071E3 RID: 29155 RVA: 0x0027E64E File Offset: 0x0027C84E
		public StorytellerCompProperties_GauranlenPodSpawn()
		{
			this.compClass = typeof(StorytellerComp_GauranlenPodSpawn);
		}

		// Token: 0x04004802 RID: 18434
		public float daysBetweenPodSpawns;

		// Token: 0x04004803 RID: 18435
		public int countdownFactorAnyConnectors = 1;

		// Token: 0x04004804 RID: 18436
		public IncidentDef incident;
	}
}
