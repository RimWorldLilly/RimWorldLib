using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011D0 RID: 4560
	public class StorytellerCompProperties_DissolutionTriggered : StorytellerCompProperties
	{
		// Token: 0x060071DA RID: 29146 RVA: 0x0027E4F8 File Offset: 0x0027C6F8
		public StorytellerCompProperties_DissolutionTriggered()
		{
			this.compClass = typeof(StorytellerComp_DissolutionTriggered);
		}

		// Token: 0x040047F9 RID: 18425
		public IncidentDef incident;

		// Token: 0x040047FA RID: 18426
		public ThingDef thing;

		// Token: 0x040047FB RID: 18427
		public int delayTicks;
	}
}
