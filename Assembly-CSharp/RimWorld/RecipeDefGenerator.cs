using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E9B RID: 3739
	public static class RecipeDefGenerator
	{
		// Token: 0x0600628A RID: 25226 RVA: 0x0023726C File Offset: 0x0023546C
		public static IEnumerable<RecipeDef> ImpliedRecipeDefs(bool hotReload = false)
		{
			RecipeDefGenerator.<ImpliedRecipeDefs>d__0 <ImpliedRecipeDefs>d__ = new RecipeDefGenerator.<ImpliedRecipeDefs>d__0(-2);
			<ImpliedRecipeDefs>d__.<>3__hotReload = hotReload;
			return <ImpliedRecipeDefs>d__;
		}

		// Token: 0x0600628B RID: 25227 RVA: 0x0023727C File Offset: 0x0023547C
		private static IEnumerable<RecipeDef> DefsFromRecipeMakers(bool hotReload = false)
		{
			RecipeDefGenerator.<DefsFromRecipeMakers>d__1 <DefsFromRecipeMakers>d__ = new RecipeDefGenerator.<DefsFromRecipeMakers>d__1(-2);
			<DefsFromRecipeMakers>d__.<>3__hotReload = hotReload;
			return <DefsFromRecipeMakers>d__;
		}

		// Token: 0x0600628C RID: 25228 RVA: 0x0023728C File Offset: 0x0023548C
		private static RecipeDef CreateRecipeDefFromMaker(ThingDef def, int adjustedCount = 1, bool hotReload = false)
		{
			RecipeMakerProperties recipeMaker = def.recipeMaker;
			string text = "Make_" + def.defName;
			if (adjustedCount != 1)
			{
				text += "Bulk";
			}
			RecipeDef recipeDef = hotReload ? (DefDatabase<RecipeDef>.GetNamed(text, false) ?? new RecipeDef()) : new RecipeDef();
			recipeDef.defName = text;
			string text2 = def.label;
			if (adjustedCount != 1)
			{
				text2 = text2 + " x" + adjustedCount.ToString();
			}
			if (string.IsNullOrEmpty(recipeMaker.label))
			{
				recipeDef.label = "RecipeMake".Translate(text2);
			}
			else
			{
				recipeDef.label = recipeMaker.label;
			}
			recipeDef.jobString = "RecipeMakeJobString".Translate(text2);
			recipeDef.modContentPack = def.modContentPack;
			recipeDef.displayPriority = recipeMaker.displayPriority + adjustedCount - 1;
			recipeDef.workAmount = (float)(recipeMaker.workAmount * adjustedCount);
			recipeDef.workSpeedStat = recipeMaker.workSpeedStat;
			recipeDef.efficiencyStat = recipeMaker.efficiencyStat;
			RecipeDefGenerator.SetIngredients(recipeDef, def, adjustedCount);
			recipeDef.useIngredientsForColor = recipeMaker.useIngredientsForColor;
			if (def.costListForDifficulty != null)
			{
				recipeDef.regenerateOnDifficultyChange = true;
			}
			recipeDef.defaultIngredientFilter = recipeMaker.defaultIngredientFilter;
			recipeDef.products.Add(new ThingDefCountClass(def, recipeMaker.productCount * adjustedCount));
			recipeDef.targetCountAdjustment = recipeMaker.targetCountAdjustment * adjustedCount;
			recipeDef.skillRequirements = recipeMaker.skillRequirements.ListFullCopyOrNull<SkillRequirement>();
			recipeDef.workSkill = recipeMaker.workSkill;
			recipeDef.workSkillLearnFactor = recipeMaker.workSkillLearnPerTick;
			recipeDef.requiredGiverWorkType = recipeMaker.requiredGiverWorkType;
			recipeDef.unfinishedThingDef = recipeMaker.unfinishedThingDef;
			recipeDef.recipeUsers = recipeMaker.recipeUsers.ListFullCopyOrNull<ThingDef>();
			recipeDef.mechanitorOnlyRecipe = recipeMaker.mechanitorOnlyRecipe;
			recipeDef.effectWorking = recipeMaker.effectWorking;
			recipeDef.soundWorking = recipeMaker.soundWorking;
			recipeDef.researchPrerequisite = recipeMaker.researchPrerequisite;
			recipeDef.memePrerequisitesAny = recipeMaker.memePrerequisitesAny;
			recipeDef.researchPrerequisites = recipeMaker.researchPrerequisites;
			recipeDef.factionPrerequisiteTags = recipeMaker.factionPrerequisiteTags;
			recipeDef.fromIdeoBuildingPreceptOnly = recipeMaker.fromIdeoBuildingPreceptOnly;
			string[] items = recipeDef.products.Select(delegate(ThingDefCountClass p)
			{
				if (p.count != 1)
				{
					return p.Label;
				}
				return Find.ActiveLanguageWorker.WithIndefiniteArticle(p.thingDef.label, false, false);
			}).ToArray<string>();
			recipeDef.description = "RecipeMakeDescription".Translate(items.ToCommaList(true, false));
			recipeDef.descriptionHyperlinks = (from p in recipeDef.products
			select new DefHyperlink(p.thingDef)).ToList<DefHyperlink>();
			if (adjustedCount != 1 && recipeDef.workAmount < 0f)
			{
				recipeDef.workAmount = recipeDef.WorkAmountTotal(null) * (float)adjustedCount;
			}
			return recipeDef;
		}

		// Token: 0x0600628D RID: 25229 RVA: 0x00237548 File Offset: 0x00235748
		public static void SetIngredients(RecipeDef r, ThingDef def, int adjustedCount = 1)
		{
			r.ingredients.Clear();
			r.adjustedCount = adjustedCount;
			if (def.MadeFromStuff)
			{
				IngredientCount ingredientCount = new IngredientCount();
				ingredientCount.SetBaseCount((float)(def.CostStuffCount * adjustedCount));
				ingredientCount.filter.SetAllowAllWhoCanMake(def);
				ThingFilter filter = ingredientCount.filter;
				string customSummary;
				if ((customSummary = def.stuffCategorySummary) == null)
				{
					customSummary = (from category in def.stuffCategories
					select category.noun).ToCommaListOr(false);
				}
				filter.customSummary = customSummary;
				r.ingredients.Add(ingredientCount);
				r.fixedIngredientFilter.SetAllowAllWhoCanMake(def);
				r.productHasIngredientStuff = true;
			}
			if (def.CostList != null)
			{
				foreach (ThingDefCountClass thingDefCountClass in def.CostList)
				{
					IngredientCount ingredientCount2 = new IngredientCount();
					ingredientCount2.SetBaseCount((float)(thingDefCountClass.count * adjustedCount));
					ingredientCount2.filter.SetAllow(thingDefCountClass.thingDef, true);
					r.ingredients.Add(ingredientCount2);
				}
			}
		}

		// Token: 0x0600628E RID: 25230 RVA: 0x00237674 File Offset: 0x00235874
		public static void ResetRecipeIngredientsForDifficulty()
		{
			foreach (RecipeDef recipeDef in from x in DefDatabase<RecipeDef>.AllDefs
			where x.regenerateOnDifficultyChange
			select x)
			{
				RecipeDefGenerator.SetIngredients(recipeDef, recipeDef.products[0].thingDef, recipeDef.adjustedCount);
			}
		}

		// Token: 0x0600628F RID: 25231 RVA: 0x002376FC File Offset: 0x002358FC
		private static IEnumerable<RecipeDef> DrugAdministerDefs(bool hotReload = false)
		{
			RecipeDefGenerator.<DrugAdministerDefs>d__5 <DrugAdministerDefs>d__ = new RecipeDefGenerator.<DrugAdministerDefs>d__5(-2);
			<DrugAdministerDefs>d__.<>3__hotReload = hotReload;
			return <DrugAdministerDefs>d__;
		}
	}
}
