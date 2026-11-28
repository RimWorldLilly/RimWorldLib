using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001881 RID: 6273
	public class CompProperties_FireBurst : CompProperties
	{
		// Token: 0x06009D15 RID: 40213 RVA: 0x00387FD9 File Offset: 0x003861D9
		public CompProperties_FireBurst()
		{
			this.compClass = typeof(CompFireBurst);
		}

		// Token: 0x04005D1B RID: 23835
		public float radius = 6f;

		// Token: 0x04005D1C RID: 23836
		public int ticksAwayFromDetonate = 17;
	}
}
