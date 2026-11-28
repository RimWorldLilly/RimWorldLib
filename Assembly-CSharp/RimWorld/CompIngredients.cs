using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C4 RID: 6340
	public class CompIngredients : ThingComp
	{
		// Token: 0x17001B46 RID: 6982
		// (get) Token: 0x06009EF2 RID: 40690 RVA: 0x003907E6 File Offset: 0x0038E9E6
		public CompProperties_Ingredients Props
		{
			get
			{
				return (CompProperties_Ingredients)this.props;
			}
		}

		// Token: 0x17001B47 RID: 6983
		// (get) Token: 0x06009EF3 RID: 40691 RVA: 0x003907F3 File Offset: 0x0038E9F3
		public List<string> MergeCompatibilityTags
		{
			get
			{
				if (this.cachedMergeCompatibilityTags == null)
				{
					this.cachedMergeCompatibilityTags = new List<string>();
					if (this.Props.performMergeCompatibilityChecks)
					{
						CompIngredients.ComputeTags(this.cachedMergeCompatibilityTags, this.ingredients);
					}
				}
				return this.cachedMergeCompatibilityTags;
			}
		}

		// Token: 0x06009EF4 RID: 40692 RVA: 0x0039082C File Offset: 0x0038EA2C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<ThingDef>(ref this.ingredients, "ingredients", LookMode.Def, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.ingredients == null)
				{
					this.ingredients = new List<ThingDef>();
				}
				if (this.ingredients.RemoveAll((ThingDef i) => i == null) != 0)
				{
					Log.Error("Some ingredients were null after loading.");
				}
			}
		}

		// Token: 0x06009EF5 RID: 40693 RVA: 0x003908A6 File Offset: 0x0038EAA6
		public void RegisterIngredient(ThingDef def)
		{
			if (!this.ingredients.Contains(def))
			{
				this.ingredients.Add(def);
				this.cachedMergeCompatibilityTags = null;
			}
		}

		// Token: 0x06009EF6 RID: 40694 RVA: 0x003908CC File Offset: 0x0038EACC
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			if (piece != this.parent)
			{
				CompIngredients compIngredients = piece.TryGetComp<CompIngredients>();
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					compIngredients.ingredients.Add(this.ingredients[i]);
				}
			}
		}

		// Token: 0x06009EF7 RID: 40695 RVA: 0x00390920 File Offset: 0x0038EB20
		public override bool AllowStackWith(Thing otherStack)
		{
			CompIngredients compIngredients;
			if (!otherStack.TryGetComp(out compIngredients))
			{
				return false;
			}
			if (!this.Props.performMergeCompatibilityChecks || !compIngredients.Props.performMergeCompatibilityChecks)
			{
				return true;
			}
			if (ModsConfig.BiotechActive && FoodUtility.GetFoodKind(this.parent) != FoodUtility.GetFoodKind(otherStack))
			{
				return false;
			}
			int count = this.MergeCompatibilityTags.Count;
			int count2 = compIngredients.MergeCompatibilityTags.Count;
			bool result;
			if (count == 0 && count2 == 0)
			{
				result = true;
			}
			else if (count != count2 && (count == 0 || count2 == 0))
			{
				result = false;
			}
			else
			{
				CompIngredients.tmpMergedIngredients.Clear();
				CompIngredients.tmpMergedIngredientTags.Clear();
				CompIngredients.tmpMergedIngredients.AddRange(this.ingredients);
				bool flag;
				CompIngredients.MergeIngredients(CompIngredients.tmpMergedIngredients, compIngredients.ingredients, out flag, this.parent.def);
				if (flag)
				{
					result = false;
				}
				else
				{
					CompIngredients.ComputeTags(CompIngredients.tmpMergedIngredientTags, CompIngredients.tmpMergedIngredients);
					result = (CompIngredients.tmpMergedIngredientTags.SetsEqual(this.MergeCompatibilityTags) && CompIngredients.tmpMergedIngredientTags.SetsEqual(compIngredients.MergeCompatibilityTags));
				}
			}
			return result;
		}

		// Token: 0x06009EF8 RID: 40696 RVA: 0x00390A24 File Offset: 0x0038EC24
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			base.PreAbsorbStack(otherStack, count);
			CompIngredients compIngredients = otherStack.TryGetComp<CompIngredients>();
			bool flag;
			CompIngredients.MergeIngredients(this.ingredients, compIngredients.ingredients, out flag, null);
			this.cachedMergeCompatibilityTags = null;
		}

		// Token: 0x06009EF9 RID: 40697 RVA: 0x00390A5C File Offset: 0x0038EC5C
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.ingredients.Count > 0)
			{
				stringBuilder.Append("Ingredients".Translate() + ": ");
				bool flag;
				stringBuilder.Append(this.GetIngredientsString(true, out flag));
				if (flag)
				{
					stringBuilder.Append(" (* " + "OnlyStacksWithCompatibleMeals".Translate().Resolve() + ")");
				}
			}
			if (ModsConfig.IdeologyActive)
			{
				stringBuilder.AppendLineIfNotEmpty().Append(this.GetFoodKindInspectString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06009EFA RID: 40698 RVA: 0x00390AF8 File Offset: 0x0038ECF8
		public string GetIngredientsString(bool includeMergeCompatibility, out bool hasMergeCompatibilityIngredients)
		{
			StringBuilder stringBuilder = new StringBuilder();
			hasMergeCompatibilityIngredients = false;
			for (int i = 0; i < this.ingredients.Count; i++)
			{
				ThingDef thingDef = this.ingredients[i];
				stringBuilder.Append((i == 0) ? thingDef.LabelCap.Resolve() : thingDef.label);
				if (includeMergeCompatibility && this.Props.performMergeCompatibilityChecks)
				{
					IngredientProperties ingredient = thingDef.ingredient;
					if (ingredient != null && ingredient.mergeCompatibilityTags.Count > 0)
					{
						stringBuilder.Append("*");
						hasMergeCompatibilityIngredients = true;
					}
				}
				if (i < this.ingredients.Count - 1)
				{
					stringBuilder.Append(", ");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06009EFB RID: 40699 RVA: 0x00390BB4 File Offset: 0x0038EDB4
		private string GetFoodKindInspectString()
		{
			if (FoodUtility.GetFoodKind(this.parent) == FoodKind.NonMeat)
			{
				return "MealKindVegetarian".Translate().Colorize(Color.green);
			}
			if (FoodUtility.GetFoodKind(this.parent) == FoodKind.Meat)
			{
				return "MealKindMeat".Translate().Colorize(ColorLibrary.RedReadable);
			}
			return "MealKindAny".Translate().Colorize(Color.white);
		}

		// Token: 0x06009EFC RID: 40700 RVA: 0x00390C1A File Offset: 0x0038EE1A
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompIngredients.<SpecialDisplayStats>d__17 <SpecialDisplayStats>d__ = new CompIngredients.<SpecialDisplayStats>d__17(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x06009EFD RID: 40701 RVA: 0x00390C2C File Offset: 0x0038EE2C
		private static void ComputeTags(List<string> result, List<ThingDef> ingredients)
		{
			foreach (ThingDef thingDef in ingredients)
			{
				if (thingDef.ingredient != null)
				{
					foreach (string obj in thingDef.ingredient.mergeCompatibilityTags)
					{
						result.AddUnique(obj);
					}
				}
			}
		}

		// Token: 0x06009EFE RID: 40702 RVA: 0x00390CC4 File Offset: 0x0038EEC4
		private static void MergeIngredients(List<ThingDef> destIngredients, List<ThingDef> srcIngredients, out bool lostImportantIngredients, ThingDef defToMake = null)
		{
			lostImportantIngredients = false;
			foreach (ThingDef obj in srcIngredients)
			{
				destIngredients.AddUnique(obj);
			}
			if (destIngredients.Count > 3)
			{
				destIngredients.SortStable(CompIngredients.MostTagsCmp);
				while (destIngredients.Count > 3)
				{
					IngredientProperties ingredient = destIngredients.Last<ThingDef>().ingredient;
					List<string> list = (ingredient != null) ? ingredient.mergeCompatibilityTags : null;
					if (list != null && list.Any<string>())
					{
						lostImportantIngredients = true;
					}
					destIngredients.RemoveLast<ThingDef>();
				}
			}
			if (defToMake != null && !lostImportantIngredients)
			{
				RecipeDef recipeDef = DefDatabase<RecipeDef>.AllDefs.FirstOrDefault((RecipeDef x) => x.ProducedThingDef == defToMake);
				if (recipeDef == null)
				{
					return;
				}
				CompIngredients.missingIngredients.AddRange(recipeDef.ingredients);
				using (List<ThingDef>.Enumerator enumerator = destIngredients.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ThingDef ing = enumerator.Current;
						IngredientCount ingredientCount = CompIngredients.missingIngredients.FirstOrDefault((IngredientCount x) => x.filter.Allows(ing));
						if (ingredientCount != null)
						{
							CompIngredients.missingIngredients.Remove(ingredientCount);
							if (!CompIngredients.missingIngredients.Any<IngredientCount>())
							{
								break;
							}
						}
					}
				}
				if (CompIngredients.missingIngredients.Any<IngredientCount>())
				{
					lostImportantIngredients = true;
				}
			}
			CompIngredients.missingIngredients.Clear();
		}

		// Token: 0x04005E11 RID: 24081
		public List<ThingDef> ingredients = new List<ThingDef>();

		// Token: 0x04005E12 RID: 24082
		private List<string> cachedMergeCompatibilityTags;

		// Token: 0x04005E13 RID: 24083
		public const int MaxNumIngredients = 3;

		// Token: 0x04005E14 RID: 24084
		private static readonly List<ThingDef> tmpMergedIngredients = new List<ThingDef>();

		// Token: 0x04005E15 RID: 24085
		private static readonly List<string> tmpMergedIngredientTags = new List<string>();

		// Token: 0x04005E16 RID: 24086
		private static readonly List<IngredientCount> missingIngredients = new List<IngredientCount>();

		// Token: 0x04005E17 RID: 24087
		private static readonly Func<ThingDef, ThingDef, int> MostTagsCmp = new Func<ThingDef, ThingDef, int>(GenCollection.CompareBy<ThingDef, int>(new Func<ThingDef, int>(CompIngredients.<>c.<>9, ldftn(<.cctor>b__23_0))).Descending<ThingDef>().Compare);
	}
}
