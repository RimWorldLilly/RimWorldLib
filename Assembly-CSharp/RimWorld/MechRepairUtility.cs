using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A6C RID: 2668
	public static class MechRepairUtility
	{
		// Token: 0x06005080 RID: 20608 RVA: 0x001E43E4 File Offset: 0x001E25E4
		private static Hediff GetHediffToHeal(Pawn mech)
		{
			Hediff hediff = null;
			float num = float.PositiveInfinity;
			foreach (Hediff hediff2 in mech.health.hediffSet.hediffs)
			{
				if (hediff2 is Hediff_Injury && hediff2.Severity < num)
				{
					num = hediff2.Severity;
					hediff = hediff2;
				}
			}
			if (hediff != null)
			{
				return hediff;
			}
			foreach (Hediff hediff3 in mech.health.hediffSet.hediffs)
			{
				if (hediff3 is Hediff_MissingPart)
				{
					return hediff3;
				}
			}
			return null;
		}

		// Token: 0x06005081 RID: 20609 RVA: 0x001E44BC File Offset: 0x001E26BC
		public static bool CanRepair(Pawn mech)
		{
			return mech.TryGetComp<CompMechRepairable>() != null && (MechRepairUtility.GetHediffToHeal(mech) != null || MechRepairUtility.IsMissingWeapon(mech));
		}

		// Token: 0x06005082 RID: 20610 RVA: 0x001E44D8 File Offset: 0x001E26D8
		public static void RepairTick(Pawn mech)
		{
			Hediff hediffToHeal = MechRepairUtility.GetHediffToHeal(mech);
			if (hediffToHeal == null)
			{
				MechRepairUtility.GenerateWeapon(mech);
				return;
			}
			Hediff_MissingPart hediff_MissingPart = hediffToHeal as Hediff_MissingPart;
			if (hediff_MissingPart != null)
			{
				mech.health.RemoveHediff(hediff_MissingPart);
				return;
			}
			hediffToHeal.Heal(1f);
		}

		// Token: 0x06005083 RID: 20611 RVA: 0x001E4518 File Offset: 0x001E2718
		[Obsolete]
		public static void RepairTick(Pawn mech, int delta)
		{
			Hediff hediffToHeal = MechRepairUtility.GetHediffToHeal(mech);
			if (hediffToHeal == null)
			{
				MechRepairUtility.GenerateWeapon(mech);
				return;
			}
			Hediff_MissingPart hediff_MissingPart = hediffToHeal as Hediff_MissingPart;
			if (hediff_MissingPart != null)
			{
				mech.health.RemoveHediff(hediff_MissingPart);
				return;
			}
			hediffToHeal.Heal((float)delta);
		}

		// Token: 0x06005084 RID: 20612 RVA: 0x001E4558 File Offset: 0x001E2758
		public static bool IsMissingWeapon(Pawn mech)
		{
			List<string> weaponTags = mech.kindDef.weaponTags;
			if (weaponTags.NullOrEmpty<string>())
			{
				return false;
			}
			List<ThingWithComps> allEquipmentListForReading = mech.equipment.AllEquipmentListForReading;
			Predicate<string> <>9__0;
			for (int i = 0; i < allEquipmentListForReading.Count; i++)
			{
				List<string> weaponTags2 = allEquipmentListForReading[i].def.weaponTags;
				Predicate<string> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((string t) => weaponTags.Contains(t)));
				}
				if (weaponTags2.Any(predicate))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005085 RID: 20613 RVA: 0x001E45E4 File Offset: 0x001E27E4
		public static void GenerateWeapon(Pawn mech)
		{
			if (!MechRepairUtility.IsMissingWeapon(mech))
			{
				return;
			}
			PawnWeaponGenerator.TryGenerateWeaponFor(mech, default(PawnGenerationRequest));
		}
	}
}
