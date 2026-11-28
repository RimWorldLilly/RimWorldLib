using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E53 RID: 3667
	public class ThoughtWorker_WeaponTrait : ThoughtWorker
	{
		// Token: 0x060061AF RID: 25007 RVA: 0x00233B53 File Offset: 0x00231D53
		public override string PostProcessDescription(Pawn p, string description)
		{
			return base.PostProcessDescription(p, description.Formatted(this.WeaponName(p).Named("WEAPON")));
		}

		// Token: 0x060061B0 RID: 25008 RVA: 0x00233B78 File Offset: 0x00231D78
		public override string PostProcessLabel(Pawn p, string label)
		{
			return base.PostProcessLabel(p, label.Formatted(this.WeaponName(p).Named("WEAPON")));
		}

		// Token: 0x060061B1 RID: 25009 RVA: 0x00233BA0 File Offset: 0x00231DA0
		protected string WeaponName(Pawn pawn)
		{
			if (pawn.equipment.bondedWeapon == null)
			{
				return string.Empty;
			}
			if (pawn.equipment.bondedWeapon.StyleSourcePrecept != null)
			{
				return pawn.equipment.bondedWeapon.StyleSourcePrecept.Label;
			}
			CompGeneratedNames compGeneratedNames = pawn.equipment.bondedWeapon.TryGetComp<CompGeneratedNames>();
			if (compGeneratedNames != null)
			{
				return compGeneratedNames.Name;
			}
			return pawn.equipment.bondedWeapon.LabelNoCount;
		}

		// Token: 0x060061B2 RID: 25010 RVA: 0x00233C14 File Offset: 0x00231E14
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Pawn_EquipmentTracker equipment = p.equipment;
			if (((equipment != null) ? equipment.bondedWeapon : null) == null || p.equipment.bondedWeapon.Destroyed)
			{
				return ThoughtState.Inactive;
			}
			CompBladelinkWeapon compBladelinkWeapon = p.equipment.bondedWeapon.TryGetComp<CompBladelinkWeapon>();
			if (compBladelinkWeapon == null || compBladelinkWeapon.TraitsListForReading.NullOrEmpty<WeaponTraitDef>())
			{
				return ThoughtState.Inactive;
			}
			return true;
		}
	}
}
