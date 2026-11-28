using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001756 RID: 5974
	public class RoleEffect_HuntingRevengeChanceFactor : RoleEffect
	{
		// Token: 0x060094BA RID: 38074 RVA: 0x00363565 File Offset: 0x00361765
		public RoleEffect_HuntingRevengeChanceFactor()
		{
			this.labelKey = "RoleEffectHuntingRevengeChance";
		}

		// Token: 0x060094BB RID: 38075 RVA: 0x00363578 File Offset: 0x00361778
		public override string Label(Pawn pawn, Precept_Role role)
		{
			return this.labelKey.Translate("x" + this.factor.ToStringPercent());
		}

		// Token: 0x040059A4 RID: 22948
		public float factor;
	}
}
