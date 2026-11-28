using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001855 RID: 6229
	public class CompProperties_DamageOnInterval : CompProperties
	{
		// Token: 0x06009BEA RID: 39914 RVA: 0x00382C09 File Offset: 0x00380E09
		public CompProperties_DamageOnInterval()
		{
			this.compClass = typeof(CompDamageOnInterval);
		}

		// Token: 0x04005CB2 RID: 23730
		public DamageDef damageDef;

		// Token: 0x04005CB3 RID: 23731
		public int ticksBetweenDamage;

		// Token: 0x04005CB4 RID: 23732
		public float damage;

		// Token: 0x04005CB5 RID: 23733
		public List<DamageEffectStage> effectStages = new List<DamageEffectStage>();

		// Token: 0x04005CB6 RID: 23734
		public float startHitPointsPercent = -1f;
	}
}
