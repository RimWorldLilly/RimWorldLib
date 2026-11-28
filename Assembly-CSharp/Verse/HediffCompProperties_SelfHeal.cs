using System;

namespace Verse
{
	// Token: 0x02000475 RID: 1141
	public class HediffCompProperties_SelfHeal : HediffCompProperties
	{
		// Token: 0x06001FFE RID: 8190 RVA: 0x000C13E4 File Offset: 0x000BF5E4
		public HediffCompProperties_SelfHeal()
		{
			this.compClass = typeof(HediffComp_SelfHeal);
		}

		// Token: 0x040018E6 RID: 6374
		public int healIntervalTicksStanding = 50;

		// Token: 0x040018E7 RID: 6375
		public float healAmount = 1f;
	}
}
