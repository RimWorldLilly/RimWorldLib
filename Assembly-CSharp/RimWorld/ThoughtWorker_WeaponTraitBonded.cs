using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E54 RID: 3668
	public class ThoughtWorker_WeaponTraitBonded : ThoughtWorker_WeaponTrait
	{
		// Token: 0x060061B4 RID: 25012 RVA: 0x00233C84 File Offset: 0x00231E84
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!base.CurrentStateInternal(p).Active)
			{
				return ThoughtState.Inactive;
			}
			List<WeaponTraitDef> traitsListForReading = p.equipment.bondedWeapon.TryGetComp<CompBladelinkWeapon>().TraitsListForReading;
			for (int i = 0; i < traitsListForReading.Count; i++)
			{
				if (traitsListForReading[i].bondedThought == this.def)
				{
					return true;
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
