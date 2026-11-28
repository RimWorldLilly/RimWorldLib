using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001034 RID: 4148
	public class HistoryAutoRecorderWorker_ThreatPoints : HistoryAutoRecorderWorker
	{
		// Token: 0x06006889 RID: 26761 RVA: 0x0024F527 File Offset: 0x0024D727
		public override float PullRecord()
		{
			if (Find.AnyPlayerHomeMap == null)
			{
				return 0f;
			}
			return StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap) / 10f;
		}
	}
}
