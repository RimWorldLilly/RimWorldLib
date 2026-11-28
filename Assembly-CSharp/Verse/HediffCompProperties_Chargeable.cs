using System;

namespace Verse
{
	// Token: 0x02000417 RID: 1047
	public class HediffCompProperties_Chargeable : HediffCompProperties
	{
		// Token: 0x06001EC9 RID: 7881 RVA: 0x000BCB57 File Offset: 0x000BAD57
		public HediffCompProperties_Chargeable()
		{
			this.compClass = typeof(HediffComp_Chargeable);
		}

		// Token: 0x0400182B RID: 6187
		public int ticksToFullCharge = -1;

		// Token: 0x0400182C RID: 6188
		public float initialCharge;

		// Token: 0x0400182D RID: 6189
		public float fullChargeAmount = 1f;

		// Token: 0x0400182E RID: 6190
		public float minChargeToActivate;

		// Token: 0x0400182F RID: 6191
		public string labelInBrackets;
	}
}
