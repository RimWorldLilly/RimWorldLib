using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E7E RID: 3710
	public class ThoughtWorker_YoungstersHappy : ThoughtWorker_YoungstersMoodBase
	{
		// Token: 0x06006237 RID: 25143 RVA: 0x00235310 File Offset: 0x00233510
		protected override FloatRange MoodRange()
		{
			return ThoughtWorker_MyChildHappy.HappyMoodRange;
		}
	}
}
