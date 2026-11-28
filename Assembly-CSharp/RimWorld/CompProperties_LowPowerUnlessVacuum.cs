using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001358 RID: 4952
	public class CompProperties_LowPowerUnlessVacuum : CompProperties
	{
		// Token: 0x06007AA1 RID: 31393 RVA: 0x002BAC5C File Offset: 0x002B8E5C
		public CompProperties_LowPowerUnlessVacuum()
		{
			this.compClass = typeof(CompLowPowerInSpace);
		}

		// Token: 0x04004D72 RID: 19826
		public float lowPowerConsumptionFactor = 0.1f;

		// Token: 0x04004D73 RID: 19827
		public bool checkRoomVacuum = true;
	}
}
