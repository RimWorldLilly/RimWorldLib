using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001035 RID: 4149
	public class HistoryAutoRecorderWorker_Adaptation : HistoryAutoRecorderWorker
	{
		// Token: 0x0600688B RID: 26763 RVA: 0x0024F54E File Offset: 0x0024D74E
		public override float PullRecord()
		{
			return Find.StoryWatcher.watcherAdaptation.AdaptDays;
		}
	}
}
