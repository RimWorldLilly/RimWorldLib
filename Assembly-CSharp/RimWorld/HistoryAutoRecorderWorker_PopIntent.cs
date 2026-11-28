using System;

namespace RimWorld
{
	// Token: 0x02001037 RID: 4151
	public class HistoryAutoRecorderWorker_PopIntent : HistoryAutoRecorderWorker
	{
		// Token: 0x0600688F RID: 26767 RVA: 0x0024F580 File Offset: 0x0024D780
		public override float PullRecord()
		{
			return StorytellerUtilityPopulation.PopulationIntent * 10f;
		}
	}
}
