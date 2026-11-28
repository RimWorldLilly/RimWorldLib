using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

// Token: 0x02000017 RID: 23
public static class MechWorkUtility
{
	// Token: 0x060000C7 RID: 199 RVA: 0x00008055 File Offset: 0x00006255
	public static IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef, StatRequest req)
	{
		MechWorkUtility.<SpecialDisplayStats>d__0 <SpecialDisplayStats>d__ = new MechWorkUtility.<SpecialDisplayStats>d__0(-2);
		<SpecialDisplayStats>d__.<>3__parentDef = parentDef;
		return <SpecialDisplayStats>d__;
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00008068 File Offset: 0x00006268
	public static bool AnyWorkMechCouldDo(RecipeDef recipe)
	{
		IEnumerable<WorkTypeDef> mechEnabledWorkTypes = (from p in DefDatabase<PawnKindDef>.AllDefs
		where p.RaceProps.IsWorkMech
		select p).SelectMany((PawnKindDef p) => p.RaceProps.mechEnabledWorkTypes).Distinct<WorkTypeDef>();
		if (recipe.requiredGiverWorkType != null && !mechEnabledWorkTypes.Contains(recipe.requiredGiverWorkType))
		{
			return false;
		}
		IEnumerable<ThingDef> recipeUsers = recipe.AllRecipeUsers;
		return (from wg in DefDatabase<WorkGiverDef>.AllDefs
		where !wg.fixedBillGiverDefs.NullOrEmpty<ThingDef>() && wg.fixedBillGiverDefs.Intersect(recipeUsers).Any<ThingDef>()
		select wg).Any((WorkGiverDef wg) => mechEnabledWorkTypes.Contains(wg.workType));
	}
}
