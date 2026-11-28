using System;

namespace Verse
{
	// Token: 0x02000439 RID: 1081
	public class HediffCompProperties_GiveHediffLungRot : HediffCompProperties_GiveHediff
	{
		// Token: 0x06001F45 RID: 8005 RVA: 0x000BE62A File Offset: 0x000BC82A
		public HediffCompProperties_GiveHediffLungRot()
		{
			this.compClass = typeof(HediffComp_GiveHediffLungRot);
		}

		// Token: 0x0400186C RID: 6252
		public SimpleCurve mtbOverRotGasExposureCurve;

		// Token: 0x0400186D RID: 6253
		public float minSeverity = 0.5f;

		// Token: 0x0400186E RID: 6254
		public int mtbCheckDuration = 60;
	}
}
