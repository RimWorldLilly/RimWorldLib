using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E55 RID: 3669
	public class ThoughtWorker_WeaponTraitKillNeed : ThoughtWorker_WeaponTrait
	{
		// Token: 0x060061B6 RID: 25014 RVA: 0x00233CF8 File Offset: 0x00231EF8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!base.CurrentStateInternal(p).Active)
			{
				return ThoughtState.Inactive;
			}
			CompBladelinkWeapon compBladelinkWeapon = p.equipment.bondedWeapon.TryGetComp<CompBladelinkWeapon>();
			List<WeaponTraitDef> traitsListForReading = compBladelinkWeapon.TraitsListForReading;
			for (int i = 0; i < traitsListForReading.Count; i++)
			{
				if (traitsListForReading[i] == WeaponTraitDefOf.NeedKill)
				{
					return compBladelinkWeapon.TicksSinceLastKill > 1200000;
				}
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x04003606 RID: 13830
		public const int TicksToGiveMemory = 1200000;
	}
}
