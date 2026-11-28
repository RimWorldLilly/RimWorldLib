using System;

namespace Verse
{
	// Token: 0x0200046B RID: 1131
	public class HediffCompProperties_ReactOnDamage : HediffCompProperties
	{
		// Token: 0x06001FE4 RID: 8164 RVA: 0x000C0D7C File Offset: 0x000BEF7C
		public HediffCompProperties_ReactOnDamage()
		{
			this.compClass = typeof(HediffComp_ReactOnDamage);
		}

		// Token: 0x040018D3 RID: 6355
		public DamageDef damageDefIncoming;

		// Token: 0x040018D4 RID: 6356
		public BodyPartDef createHediffOn;

		// Token: 0x040018D5 RID: 6357
		public HediffDef createHediff;

		// Token: 0x040018D6 RID: 6358
		public bool vomit;
	}
}
