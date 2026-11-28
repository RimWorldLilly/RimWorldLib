using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE7 RID: 3815
	public class CompProperties_SnowExpand : CompProperties
	{
		// Token: 0x060063C3 RID: 25539 RVA: 0x0023C4BC File Offset: 0x0023A6BC
		public CompProperties_SnowExpand()
		{
			this.compClass = typeof(CompSnowExpand);
		}

		// Token: 0x04003820 RID: 14368
		public int expandInterval = 500;

		// Token: 0x04003821 RID: 14369
		public float addAmount = 0.12f;

		// Token: 0x04003822 RID: 14370
		public float maxRadius = 55f;
	}
}
