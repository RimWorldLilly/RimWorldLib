using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001751 RID: 5969
	public class RoleEffect_NoRangedWeapons : RoleEffect
	{
		// Token: 0x1700196F RID: 6511
		// (get) Token: 0x060094B0 RID: 38064 RVA: 0x0036348A File Offset: 0x0036168A
		public override bool IsBad
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060094B1 RID: 38065 RVA: 0x0036348D File Offset: 0x0036168D
		public RoleEffect_NoRangedWeapons()
		{
			this.labelKey = "RoleEffectWontUseRangedWeapons";
		}

		// Token: 0x060094B2 RID: 38066 RVA: 0x003634A0 File Offset: 0x003616A0
		public override bool CanEquip(Pawn pawn, Thing thing)
		{
			return !thing.def.IsRangedWeapon;
		}
	}
}
