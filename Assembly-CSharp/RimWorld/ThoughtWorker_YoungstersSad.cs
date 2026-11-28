using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E7F RID: 3711
	public class ThoughtWorker_YoungstersSad : ThoughtWorker_YoungstersMoodBase
	{
		// Token: 0x06006239 RID: 25145 RVA: 0x0023531F File Offset: 0x0023351F
		protected override FloatRange MoodRange()
		{
			return ThoughtWorker_MyChildSad.SadMoodRange;
		}
	}
}
