using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CC8 RID: 3272
	public static class SappersUtility
	{
		// Token: 0x06005CCD RID: 23757 RVA: 0x002210FA File Offset: 0x0021F2FA
		public static bool IsGoodSapper(Pawn p)
		{
			return p.kindDef.canBeSapper && SappersUtility.HasBuildingDestroyerWeapon(p) && SappersUtility.CanMineReasonablyFast(p);
		}

		// Token: 0x06005CCE RID: 23758 RVA: 0x00221119 File Offset: 0x0021F319
		public static bool IsGoodBackupSapper(Pawn p)
		{
			return p.kindDef.canBeSapper && SappersUtility.CanMineReasonablyFast(p);
		}

		// Token: 0x06005CCF RID: 23759 RVA: 0x00221130 File Offset: 0x0021F330
		private static bool CanMineReasonablyFast(Pawn p)
		{
			return p.RaceProps.Humanlike && !p.skills.GetSkill(SkillDefOf.Mining).TotallyDisabled && !StatDefOf.MiningSpeed.Worker.IsDisabledFor(p) && p.skills.GetSkill(SkillDefOf.Mining).Level >= 4;
		}

		// Token: 0x06005CD0 RID: 23760 RVA: 0x00221190 File Offset: 0x0021F390
		public static bool HasBuildingDestroyerWeapon(Pawn p)
		{
			if (p.equipment == null || p.equipment.Primary == null)
			{
				return false;
			}
			List<Verb> allVerbs = p.equipment.Primary.GetComp<CompEquippable>().AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				if (allVerbs[i].verbProps.ai_IsBuildingDestroyer)
				{
					return true;
				}
			}
			return false;
		}
	}
}
