using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000471 RID: 1137
	public class HediffCompProperties_RemoveIfOtherHediff : HediffCompProperties_MessageBase
	{
		// Token: 0x06001FF3 RID: 8179 RVA: 0x000C0FA4 File Offset: 0x000BF1A4
		public HediffCompProperties_RemoveIfOtherHediff()
		{
			this.compClass = typeof(HediffComp_RemoveIfOtherHediff);
		}

		// Token: 0x040018D9 RID: 6361
		public List<HediffDef> hediffs = new List<HediffDef>();

		// Token: 0x040018DA RID: 6362
		public IntRange? stages;

		// Token: 0x040018DB RID: 6363
		public int mtbHours;
	}
}
