using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001899 RID: 6297
	public class CompProperties_GasOnDamage : CompProperties
	{
		// Token: 0x06009DC6 RID: 40390 RVA: 0x0038AF22 File Offset: 0x00389122
		public CompProperties_GasOnDamage()
		{
			this.compClass = typeof(CompGasOnDamage);
		}

		// Token: 0x04005D64 RID: 23908
		public GasType type;

		// Token: 0x04005D65 RID: 23909
		public float damageFactor = 6f;

		// Token: 0x04005D66 RID: 23910
		public DamageDef damage;

		// Token: 0x04005D67 RID: 23911
		public bool useStackCountAsFactor;
	}
}
