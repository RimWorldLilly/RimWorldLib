using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002091 RID: 8337
	public static class ThingRequiringRoyalPermissionUtility
	{
		// Token: 0x0600D229 RID: 53801 RVA: 0x004D33A0 File Offset: 0x004D15A0
		public static bool IsViolatingRulesOf(Def implantOrWeapon, Pawn pawn, Faction faction, int implantLevel = 0)
		{
			if (faction.def.royalImplantRules == null || faction.def.royalImplantRules.Count == 0)
			{
				return false;
			}
			RoyalTitleDef minTitleToUse = ThingRequiringRoyalPermissionUtility.GetMinTitleToUse(implantOrWeapon, faction, implantLevel);
			if (minTitleToUse == null)
			{
				return false;
			}
			RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(faction);
			if (currentTitle == null)
			{
				return true;
			}
			int num = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle);
			if (num < 0)
			{
				return false;
			}
			int num2 = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(minTitleToUse);
			return num < num2;
		}

		// Token: 0x0600D22A RID: 53802 RVA: 0x004D341C File Offset: 0x004D161C
		public static bool IsViolatingRulesOfAnyFaction(Def implantOrWeapon, Pawn pawn, int implantLevel = 0)
		{
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (ThingRequiringRoyalPermissionUtility.IsViolatingRulesOf(implantOrWeapon, pawn, faction, implantLevel))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D22B RID: 53803 RVA: 0x004D3478 File Offset: 0x004D1678
		public static RoyalTitleDef GetMinTitleToUse(Def implantOrWeapon, Faction faction, int implantLevel = 0)
		{
			HediffDef hediffDef = implantOrWeapon as HediffDef;
			if (hediffDef != null)
			{
				return faction.GetMinTitleForImplant(hediffDef, implantLevel);
			}
			return null;
		}

		// Token: 0x0600D22C RID: 53804 RVA: 0x004D3499 File Offset: 0x004D1699
		[Obsolete("Will be removed in the future")]
		public static TaggedString GetEquipWeaponConfirmationDialogText(Thing weapon, Pawn pawn)
		{
			return null;
		}
	}
}
