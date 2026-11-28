using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200208A RID: 8330
	public class HediffCompProperties_ExplodeOnDeath : HediffCompProperties
	{
		// Token: 0x0600D212 RID: 53778 RVA: 0x004D2D2E File Offset: 0x004D0F2E
		public HediffCompProperties_ExplodeOnDeath()
		{
			this.compClass = typeof(HediffComp_ExplodeOnDeath);
		}

		// Token: 0x0400851E RID: 34078
		public bool destroyGear;

		// Token: 0x0400851F RID: 34079
		public bool destroyBody;

		// Token: 0x04008520 RID: 34080
		public float explosionRadius;

		// Token: 0x04008521 RID: 34081
		public DamageDef damageDef;

		// Token: 0x04008522 RID: 34082
		public int damageAmount = -1;
	}
}
