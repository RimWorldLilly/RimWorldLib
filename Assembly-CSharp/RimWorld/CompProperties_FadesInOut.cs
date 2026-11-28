using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200187F RID: 6271
	public class CompProperties_FadesInOut : CompProperties
	{
		// Token: 0x06009D0F RID: 40207 RVA: 0x00387EA1 File Offset: 0x003860A1
		public CompProperties_FadesInOut()
		{
			this.compClass = typeof(CompFadesInOut);
		}

		// Token: 0x04005D17 RID: 23831
		public float fadeInSecs;

		// Token: 0x04005D18 RID: 23832
		public float fadeOutSecs;

		// Token: 0x04005D19 RID: 23833
		public float solidTimeSecs;
	}
}
