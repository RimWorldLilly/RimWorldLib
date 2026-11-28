using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A27 RID: 6695
	public abstract class CompPlaysMusic : ThingComp
	{
		// Token: 0x17001D7E RID: 7550
		// (get) Token: 0x0600A823 RID: 43043
		public abstract bool Playing { get; }

		// Token: 0x17001D7F RID: 7551
		// (get) Token: 0x0600A824 RID: 43044
		public abstract FloatRange SoundRange { get; }
	}
}
