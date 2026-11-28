using System;

namespace Verse
{
	// Token: 0x0200015E RID: 350
	public class ExtraDamage
	{
		// Token: 0x06000A82 RID: 2690 RVA: 0x0003A4E0 File Offset: 0x000386E0
		public float AdjustedDamageAmount(Verb verb, Pawn caster)
		{
			return this.amount * verb.verbProps.GetDamageFactorFor(verb, caster);
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0003A4F6 File Offset: 0x000386F6
		public float AdjustedArmorPenetration(Verb verb, Pawn caster)
		{
			if (this.armorPenetration < 0f)
			{
				return this.AdjustedDamageAmount(verb, caster) * 0.015f;
			}
			return this.armorPenetration;
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x0003A51A File Offset: 0x0003871A
		public float AdjustedArmorPenetration()
		{
			if (this.armorPenetration < 0f)
			{
				return this.amount * 0.015f;
			}
			return this.armorPenetration;
		}

		// Token: 0x04000761 RID: 1889
		public DamageDef def;

		// Token: 0x04000762 RID: 1890
		public float amount;

		// Token: 0x04000763 RID: 1891
		public float armorPenetration = -1f;

		// Token: 0x04000764 RID: 1892
		public float chance = 1f;
	}
}
