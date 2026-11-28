using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001036 RID: 4150
	public class HistoryAutoRecorderWorker_PopAdaptation : HistoryAutoRecorderWorker
	{
		// Token: 0x0600688D RID: 26765 RVA: 0x0024F567 File Offset: 0x0024D767
		public override float PullRecord()
		{
			return Find.StoryWatcher.watcherPopAdaptation.AdaptDays;
		}
	}
}
