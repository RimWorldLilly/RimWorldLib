using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020011D8 RID: 4568
	public class StorytellerCompProperties_MechanitorComplexQuest : StorytellerCompProperties
	{
		// Token: 0x060071EE RID: 29166 RVA: 0x0027E721 File Offset: 0x0027C921
		public StorytellerCompProperties_MechanitorComplexQuest()
		{
			this.compClass = typeof(StorytellerComp_MechanitorComplexQuest);
		}

		// Token: 0x04004808 RID: 18440
		public IncidentDef incident;

		// Token: 0x04004809 RID: 18441
		public int mtbDays = 60;

		// Token: 0x0400480A RID: 18442
		public float minSpacingDays;

		// Token: 0x0400480B RID: 18443
		public float existingMechanitorOrMechlinkMTBFactor = 1f;

		// Token: 0x0400480C RID: 18444
		public List<QuestScriptDef> blockedByQueuedOrActiveQuests;
	}
}
