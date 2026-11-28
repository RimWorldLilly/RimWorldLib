using System;

namespace RimWorld
{
	// Token: 0x020011D7 RID: 4567
	public class StorytellerCompProperties_ImportantQuest : StorytellerCompProperties
	{
		// Token: 0x060071ED RID: 29165 RVA: 0x0027E709 File Offset: 0x0027C909
		public StorytellerCompProperties_ImportantQuest()
		{
			this.compClass = typeof(StorytellerComp_ImportantQuest);
		}

		// Token: 0x04004805 RID: 18437
		public IncidentDef questIncident;

		// Token: 0x04004806 RID: 18438
		public QuestScriptDef questDef;

		// Token: 0x04004807 RID: 18439
		public int fireAfterDaysPassed;
	}
}
