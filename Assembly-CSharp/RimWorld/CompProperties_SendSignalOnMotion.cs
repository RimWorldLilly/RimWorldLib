using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001968 RID: 6504
	public class CompProperties_SendSignalOnMotion : CompProperties
	{
		// Token: 0x0600A33D RID: 41789 RVA: 0x003A4904 File Offset: 0x003A2B04
		public CompProperties_SendSignalOnMotion()
		{
			this.compClass = typeof(CompSendSignalOnMotion);
		}

		// Token: 0x04006068 RID: 24680
		public bool triggerOnPawnInRoom;

		// Token: 0x04006069 RID: 24681
		public float radius;

		// Token: 0x0400606A RID: 24682
		public int enableAfterTicks;

		// Token: 0x0400606B RID: 24683
		public bool onlyHumanlike;

		// Token: 0x0400606C RID: 24684
		public bool triggeredBySkipPsycasts;

		// Token: 0x0400606D RID: 24685
		[NoTranslate]
		public string signalTag;
	}
}
