using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001750 RID: 5968
	public class RoleEffect_NoMeleeWeapons : RoleEffect
	{
		// Token: 0x1700196E RID: 6510
		// (get) Token: 0x060094AD RID: 38061 RVA: 0x00363464 File Offset: 0x00361664
		public override bool IsBad
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060094AE RID: 38062 RVA: 0x00363467 File Offset: 0x00361667
		public RoleEffect_NoMeleeWeapons()
		{
			this.labelKey = "RoleEffectWontUseMeleeWeapons";
		}

		// Token: 0x060094AF RID: 38063 RVA: 0x0036347A File Offset: 0x0036167A
		public override bool CanEquip(Pawn pawn, Thing thing)
		{
			return !thing.def.IsMeleeWeapon;
		}
	}
}
