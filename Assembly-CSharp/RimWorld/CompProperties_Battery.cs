using System;

namespace RimWorld
{
	// Token: 0x02000EC2 RID: 3778
	public class CompProperties_Battery : CompProperties_Power
	{
		// Token: 0x06006360 RID: 25440 RVA: 0x0023B357 File Offset: 0x00239557
		public CompProperties_Battery()
		{
			this.compClass = typeof(CompPowerBattery);
		}

		// Token: 0x04003792 RID: 14226
		public float storedEnergyMax = 1000f;

		// Token: 0x04003793 RID: 14227
		public float efficiency = 0.5f;
	}
}
