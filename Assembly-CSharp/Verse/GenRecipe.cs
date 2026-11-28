using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020006EE RID: 1774
	public static class GenRecipe
	{
		// Token: 0x0600387D RID: 14461 RVA: 0x0016A008 File Offset: 0x00168208
		public static IEnumerable<Thing> MakeRecipeProducts(RecipeDef recipeDef, Pawn worker, List<Thing> ingredients, Thing dominantIngredient, IBillGiver billGiver, Precept_ThingStyle precept = null, ThingStyleDef style = null, int? overrideGraphicIndex = null)
		{
			GenRecipe.<MakeRecipeProducts>d__0 <MakeRecipeProducts>d__ = new GenRecipe.<MakeRecipeProducts>d__0(-2);
			<MakeRecipeProducts>d__.<>3__recipeDef = recipeDef;
			<MakeRecipeProducts>d__.<>3__worker = worker;
			<MakeRecipeProducts>d__.<>3__ingredients = ingredients;
			<MakeRecipeProducts>d__.<>3__dominantIngredient = dominantIngredient;
			<MakeRecipeProducts>d__.<>3__billGiver = billGiver;
			<MakeRecipeProducts>d__.<>3__precept = precept;
			<MakeRecipeProducts>d__.<>3__style = style;
			<MakeRecipeProducts>d__.<>3__overrideGraphicIndex = overrideGraphicIndex;
			return <MakeRecipeProducts>d__;
		}

		// Token: 0x0600387E RID: 14462 RVA: 0x0016A058 File Offset: 0x00168258
		public static IEnumerable<Thing> FinalizeGestatedPawns(Bill_Mech bill, Pawn worker, ThingStyleDef style = null, int? overrideGraphicIndex = null)
		{
			GenRecipe.<FinalizeGestatedPawns>d__1 <FinalizeGestatedPawns>d__ = new GenRecipe.<FinalizeGestatedPawns>d__1(-2);
			<FinalizeGestatedPawns>d__.<>3__bill = bill;
			<FinalizeGestatedPawns>d__.<>3__worker = worker;
			<FinalizeGestatedPawns>d__.<>3__style = style;
			<FinalizeGestatedPawns>d__.<>3__overrideGraphicIndex = overrideGraphicIndex;
			return <FinalizeGestatedPawns>d__;
		}

		// Token: 0x0600387F RID: 14463 RVA: 0x0016A080 File Offset: 0x00168280
		private static Thing PostProcessProduct(Thing product, RecipeDef recipeDef, Pawn worker, Precept_ThingStyle precept = null, ThingStyleDef style = null, int? overrideGraphicIndex = null)
		{
			CompQuality compQuality = product.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				if (recipeDef.workSkill == null)
				{
					Log.Error(((recipeDef != null) ? recipeDef.ToString() : null) + " needs workSkill because it creates a product with a quality.");
				}
				QualityCategory q = QualityUtility.GenerateQualityCreatedByPawn(worker, recipeDef.workSkill, true);
				compQuality.SetQuality(q, new ArtGenerationContext?(ArtGenerationContext.Colony));
			}
			CompArt compArt = product.TryGetComp<CompArt>();
			if (compArt != null)
			{
				compArt.JustCreatedBy(worker);
				if (compQuality != null && compQuality.Quality >= QualityCategory.Excellent)
				{
					TaleRecorder.RecordTale(TaleDefOf.CraftedArt, new object[]
					{
						worker,
						product
					});
				}
			}
			if (compQuality != null)
			{
				QualityUtility.SendCraftNotification(product, worker);
			}
			if (worker.Ideo != null)
			{
				product.StyleDef = worker.Ideo.GetStyleFor(product.def);
			}
			if (precept != null)
			{
				product.StyleSourcePrecept = precept;
			}
			else if (style != null)
			{
				product.StyleDef = style;
			}
			else if (!product.def.randomStyle.NullOrEmpty<ThingStyleChance>() && Rand.Chance(product.def.randomStyleChance))
			{
				product.SetStyleDef(product.def.randomStyle.RandomElementByWeight((ThingStyleChance x) => x.Chance).StyleDef);
			}
			product.overrideGraphicIndex = overrideGraphicIndex;
			if (product.def.Minifiable)
			{
				product = product.MakeMinified(DestroyMode.Vanish);
			}
			return product;
		}
	}
}
