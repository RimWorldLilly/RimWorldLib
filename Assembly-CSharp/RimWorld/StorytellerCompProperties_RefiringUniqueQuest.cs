using System;

namespace RimWorld
{
	// Token: 0x020011E8 RID: 4584
	public class StorytellerCompProperties_RefiringUniqueQuest : StorytellerCompProperties
	{
		// Token: 0x06007219 RID: 29209 RVA: 0x0027ED07 File Offset: 0x0027CF07
		public StorytellerCompProperties_RefiringUniqueQuest()
		{
			this.compClass = typeof(StorytellerComp_RefiringUniqueQuest);
		}

		// Token: 0x04004827 RID: 18471
		public IncidentDef incident;

		// Token: 0x04004828 RID: 18472
		public float refireEveryDays = -1f;

		// Token: 0x04004829 RID: 18473
		public int minColonyWealth = -1;
	}
}
