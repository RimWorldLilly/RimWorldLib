using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011F9 RID: 4601
	public sealed class StoryWatcher : IExposable
	{
		// Token: 0x06007261 RID: 29281 RVA: 0x00280E02 File Offset: 0x0027F002
		public void StoryWatcherTick()
		{
			this.watcherAdaptation.AdaptationWatcherTick();
			this.watcherPopAdaptation.PopAdaptationWatcherTick();
		}

		// Token: 0x06007262 RID: 29282 RVA: 0x00280E1C File Offset: 0x0027F01C
		public void ExposeData()
		{
			Scribe_Deep.Look<StatsRecord>(ref this.statsRecord, "statsRecord", Array.Empty<object>());
			Scribe_Deep.Look<StoryWatcher_Adaptation>(ref this.watcherAdaptation, "watcherAdaptation", Array.Empty<object>());
			Scribe_Deep.Look<StoryWatcher_PopAdaptation>(ref this.watcherPopAdaptation, "watcherPopAdaptation", Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x04004861 RID: 18529
		public StatsRecord statsRecord = new StatsRecord();

		// Token: 0x04004862 RID: 18530
		public StoryWatcher_Adaptation watcherAdaptation = new StoryWatcher_Adaptation();

		// Token: 0x04004863 RID: 18531
		public StoryWatcher_PopAdaptation watcherPopAdaptation = new StoryWatcher_PopAdaptation();
	}
}
