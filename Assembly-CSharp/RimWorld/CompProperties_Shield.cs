using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200196F RID: 6511
	public class CompProperties_Shield : CompProperties
	{
		// Token: 0x0600A362 RID: 41826 RVA: 0x003A53A0 File Offset: 0x003A35A0
		public CompProperties_Shield()
		{
			this.compClass = typeof(CompShield);
		}

		// Token: 0x0400607C RID: 24700
		public int startingTicksToReset = 3200;

		// Token: 0x0400607D RID: 24701
		public float minDrawSize = 1.2f;

		// Token: 0x0400607E RID: 24702
		public float maxDrawSize = 1.55f;

		// Token: 0x0400607F RID: 24703
		public float energyLossPerDamage = 0.033f;

		// Token: 0x04006080 RID: 24704
		public float energyOnReset = 0.2f;

		// Token: 0x04006081 RID: 24705
		public bool blocksRangedWeapons = true;
	}
}
