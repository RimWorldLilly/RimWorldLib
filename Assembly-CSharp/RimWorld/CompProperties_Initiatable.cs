using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A21 RID: 6689
	public class CompProperties_Initiatable : CompProperties
	{
		// Token: 0x0600A804 RID: 43012 RVA: 0x003BC102 File Offset: 0x003BA302
		public CompProperties_Initiatable()
		{
			this.compClass = typeof(CompInitiatable);
		}

		// Token: 0x040062C6 RID: 25286
		public int initiationDelayTicks;
	}
}
