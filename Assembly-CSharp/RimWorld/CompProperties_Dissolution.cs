using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019DD RID: 6621
	public class CompProperties_Dissolution : CompProperties
	{
		// Token: 0x0600A6B6 RID: 42678 RVA: 0x003B6C04 File Offset: 0x003B4E04
		public CompProperties_Dissolution()
		{
			this.compClass = typeof(CompDissolution);
		}

		// Token: 0x04006241 RID: 25153
		public int dissolutionAfterDays = 4;

		// Token: 0x04006242 RID: 25154
		public float dissolutinFactorIndoors = 0.5f;

		// Token: 0x04006243 RID: 25155
		public float dissolutionFactorRain = 2f;
	}
}
