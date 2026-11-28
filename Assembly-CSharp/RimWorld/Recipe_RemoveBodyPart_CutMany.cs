using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014AB RID: 5291
	public class Recipe_RemoveBodyPart_CutMany : Recipe_RemoveBodyPart_Cut
	{
		// Token: 0x0600819A RID: 33178 RVA: 0x002E3E1D File Offset: 0x002E201D
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			Recipe_RemoveBodyPart_CutMany.<GetPartsToApplyOn>d__0 <GetPartsToApplyOn>d__ = new Recipe_RemoveBodyPart_CutMany.<GetPartsToApplyOn>d__0(-2);
			<GetPartsToApplyOn>d__.<>4__this = this;
			<GetPartsToApplyOn>d__.<>3__pawn = pawn;
			<GetPartsToApplyOn>d__.<>3__recipe = recipe;
			return <GetPartsToApplyOn>d__;
		}

		// Token: 0x0600819B RID: 33179 RVA: 0x002E3E3C File Offset: 0x002E203C
		public IEnumerable<BodyPartRecord> FixedPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			return MedicalRecipesUtility.GetFixedPartsToApplyOn(recipe, pawn, (BodyPartRecord record) => !pawn.health.hediffSet.PartIsMissing(record));
		}

		// Token: 0x0600819C RID: 33180 RVA: 0x002E3E70 File Offset: 0x002E2070
		public override void DamagePart(Pawn pawn, BodyPartRecord part)
		{
			foreach (BodyPartRecord hitPart in this.FixedPartsToApplyOn(pawn, this.recipe))
			{
				pawn.TakeDamage(new DamageInfo(DamageDefOf.SurgicalCut, 99999f, 999f, -1f, null, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
		}

		// Token: 0x0600819D RID: 33181 RVA: 0x002E3EE8 File Offset: 0x002E20E8
		public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			return this.recipe.label;
		}
	}
}
