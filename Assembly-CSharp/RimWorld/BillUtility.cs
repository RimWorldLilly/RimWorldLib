using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000999 RID: 2457
	public static class BillUtility
	{
		// Token: 0x06004A2F RID: 18991 RVA: 0x001CD0C1 File Offset: 0x001CB2C1
		public static void TryDrawIngredientSearchRadiusOnMap(this Bill bill, IntVec3 center)
		{
			if (bill.ingredientSearchRadius < GenRadial.MaxRadialPatternRadius)
			{
				GenDraw.DrawRadiusRing(center, bill.ingredientSearchRadius);
			}
		}

		// Token: 0x06004A30 RID: 18992 RVA: 0x001CD0DC File Offset: 0x001CB2DC
		public static Bill MakeNewBill(this RecipeDef recipe, Precept_ThingStyle precept = null)
		{
			if (recipe.UsesUnfinishedThing)
			{
				return new Bill_ProductionWithUft(recipe, precept);
			}
			if (recipe.mechResurrection)
			{
				return new Bill_ResurrectMech(recipe, precept);
			}
			if (recipe.gestationCycles > 0)
			{
				return new Bill_ProductionMech(recipe, precept);
			}
			if (recipe.formingTicks > 0)
			{
				return new Bill_Autonomous(recipe, precept);
			}
			return new Bill_Production(recipe, precept);
		}

		// Token: 0x06004A31 RID: 18993 RVA: 0x001CD132 File Offset: 0x001CB332
		public static IEnumerable<IBillGiver> GlobalBillGivers()
		{
			return new BillUtility.<GlobalBillGivers>d__8(-2);
		}

		// Token: 0x06004A32 RID: 18994 RVA: 0x001CD13B File Offset: 0x001CB33B
		public static IEnumerable<IBillGiver> MapBillGivers(Map map)
		{
			BillUtility.<MapBillGivers>d__9 <MapBillGivers>d__ = new BillUtility.<MapBillGivers>d__9(-2);
			<MapBillGivers>d__.<>3__map = map;
			return <MapBillGivers>d__;
		}

		// Token: 0x06004A33 RID: 18995 RVA: 0x001CD14B File Offset: 0x001CB34B
		public static IEnumerable<Bill> GlobalBills()
		{
			return new BillUtility.<GlobalBills>d__10(-2);
		}

		// Token: 0x06004A34 RID: 18996 RVA: 0x001CD154 File Offset: 0x001CB354
		public static IEnumerable<Bill> MapBills(Map map)
		{
			BillUtility.<MapBills>d__11 <MapBills>d__ = new BillUtility.<MapBills>d__11(-2);
			<MapBills>d__.<>3__map = map;
			return <MapBills>d__;
		}

		// Token: 0x06004A35 RID: 18997 RVA: 0x001CD164 File Offset: 0x001CB364
		public static void Notify_ISlotGroupRemoved(ISlotGroup group)
		{
			if (GravshipUtility.generatingGravship)
			{
				return;
			}
			foreach (Bill bill in BillUtility.GlobalBills())
			{
				bill.ValidateSettings();
			}
		}

		// Token: 0x06004A36 RID: 18998 RVA: 0x001CD1B8 File Offset: 0x001CB3B8
		public static void Notify_ColonistUnavailable(Pawn pawn)
		{
			try
			{
				foreach (Bill bill in BillUtility.GlobalBills())
				{
					bill.ValidateSettings();
				}
			}
			catch (Exception ex)
			{
				string str = "Could not notify bills: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x06004A37 RID: 18999 RVA: 0x001CD230 File Offset: 0x001CB430
		public static WorkGiverDef GetWorkgiver(this IBillGiver billGiver)
		{
			Thing thing = billGiver as Thing;
			if (thing == null)
			{
				Log.ErrorOnce(string.Format("Attempting to get the workgiver for a non-Thing IBillGiver {0}", billGiver.ToString()), 96810282);
				return null;
			}
			List<WorkGiverDef> allDefsListForReading = DefDatabase<WorkGiverDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkGiverDef workGiverDef = allDefsListForReading[i];
				WorkGiver_DoBill workGiver_DoBill = workGiverDef.Worker as WorkGiver_DoBill;
				if (workGiver_DoBill != null && workGiver_DoBill.ThingIsUsableBillGiver(thing))
				{
					return workGiverDef;
				}
			}
			Log.ErrorOnce(string.Format("Can't find a WorkGiver for a BillGiver {0}", thing.ToString()), 57348705);
			return null;
		}

		// Token: 0x06004A38 RID: 19000 RVA: 0x001CD2BC File Offset: 0x001CB4BC
		public static bool IsSurgeryViolationOnExtraFactionMember(this Bill_Medical bill, Pawn billDoer)
		{
			if (bill.recipe.IsSurgery && bill.recipe.Worker != null)
			{
				RecipeWorker worker = bill.recipe.Worker;
				Faction sharedExtraFaction = billDoer.GetSharedExtraFaction(bill.GiverPawn, ExtraFactionType.HomeFaction, null);
				if (sharedExtraFaction != null && worker.IsViolationOnPawn(bill.GiverPawn, bill.Part, sharedExtraFaction))
				{
					return true;
				}
				Faction sharedExtraFaction2 = billDoer.GetSharedExtraFaction(bill.GiverPawn, ExtraFactionType.MiniFaction, null);
				if (sharedExtraFaction2 != null && worker.IsViolationOnPawn(bill.GiverPawn, bill.Part, sharedExtraFaction2))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004A39 RID: 19001 RVA: 0x001CD344 File Offset: 0x001CB544
		public static void MakeIngredientsListInProcessingOrder(this Bill bill, List<IngredientCount> ingredientsOrdered)
		{
			ingredientsOrdered.Clear();
			if (bill.recipe.productHasIngredientStuff)
			{
				ingredientsOrdered.Add(bill.recipe.ingredients[0]);
			}
			for (int i = 0; i < bill.recipe.ingredients.Count; i++)
			{
				if (!bill.recipe.productHasIngredientStuff || i != 0)
				{
					IngredientCount ingredientCount = bill.recipe.ingredients[i];
					if (ingredientCount.IsFixedIngredient)
					{
						ingredientsOrdered.Add(ingredientCount);
					}
				}
			}
			for (int j = 0; j < bill.recipe.ingredients.Count; j++)
			{
				IngredientCount item = bill.recipe.ingredients[j];
				if (!ingredientsOrdered.Contains(item))
				{
					ingredientsOrdered.Add(item);
				}
			}
		}

		// Token: 0x06004A3A RID: 19002 RVA: 0x001CD404 File Offset: 0x001CB604
		public static bool BillGiverCanContainIngredients(Bill bill)
		{
			return BillUtility.GetBillGiverContainer(bill) != null;
		}

		// Token: 0x06004A3B RID: 19003 RVA: 0x001CD410 File Offset: 0x001CB610
		public static ThingOwner<Thing> GetBillGiverContainer(Bill bill)
		{
			Building_WorkTableAutonomous building_WorkTableAutonomous = bill.billStack.billGiver as Building_WorkTableAutonomous;
			if (building_WorkTableAutonomous != null)
			{
				return (ThingOwner<Thing>)building_WorkTableAutonomous.innerContainer;
			}
			return null;
		}

		// Token: 0x06004A3C RID: 19004 RVA: 0x001CD440 File Offset: 0x001CB640
		public static bool ContainedInBillGiver(Bill bill, Thing thing)
		{
			ThingOwner<Thing> billGiverContainer = BillUtility.GetBillGiverContainer(bill);
			return billGiverContainer != null && billGiverContainer.Contains(thing);
		}

		// Token: 0x06004A3D RID: 19005 RVA: 0x001CD460 File Offset: 0x001CB660
		public static void DoBillInfoWindow(int billIndex, string label, Rect rect, RecipeDef recipe, BodyPartRecord part = null, Pawn pawn = null)
		{
			BillUtility.LayoutTooltip(recipe, part, pawn, false);
			if (BillUtility.recipeTooltipLayout.Empty)
			{
				return;
			}
			Rect windowRect = Find.WindowStack.currentlyDrawnWindow.windowRect;
			Rect immRect = new Rect(windowRect.x + rect.xMax + 10f, windowRect.y + rect.y, BillUtility.recipeTooltipLayout.Size.x, BillUtility.recipeTooltipLayout.Size.y);
			immRect.x = Mathf.Min(immRect.x, (float)UI.screenWidth - immRect.width);
			immRect.y = Mathf.Min(immRect.y, (float)UI.screenHeight - immRect.height);
			Find.WindowStack.ImmediateWindow(123 * (billIndex + 1), immRect, WindowLayer.Super, delegate
			{
				GameFont font = Text.Font;
				Text.Font = GameFont.Small;
				GUI.BeginGroup(immRect.AtZero());
				BillUtility.LayoutTooltip(recipe, part, pawn, true);
				GUI.EndGroup();
				Text.Font = font;
			}, true, false, 1f, null, false);
		}

		// Token: 0x06004A3E RID: 19006 RVA: 0x001CD594 File Offset: 0x001CB794
		private static void LayoutTooltip(RecipeDef recipe, BodyPartRecord part, Pawn pawn, bool draw)
		{
			BillUtility.recipeTooltipLayout.Reset(6f);
			BillUtility.tmpSpecialProducts.Clear();
			BillUtility.tmpSortedIngredients.Clear();
			if (!recipe.skillRequirements.NullOrEmpty<SkillRequirement>())
			{
				BillUtility.recipeTooltipLayout.Label(("Requires".Translate() + ": ").AsTipTitle() + recipe.skillRequirements.Select(delegate(SkillRequirement x)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(x.skill.LabelCap);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(x.minLevel);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}).ToCommaList(false, false), draw, GameFont.Small);
			}
			if (!recipe.ingredients.NullOrEmpty<IngredientCount>())
			{
				bool flag = false;
				BillUtility.tmpSortedIngredients.AddRange(recipe.ingredients);
				BillUtility.tmpSortedIngredients.Sort((IngredientCount a, IngredientCount b) => Mathf.RoundToInt(b.CountFor(recipe) - a.CountFor(recipe)));
				using (List<IngredientCount>.Enumerator enumerator = BillUtility.tmpSortedIngredients.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IngredientCount ing = enumerator.Current;
						ThingFilter filter = ing.filter;
						if (((filter != null) ? filter.AllowedThingDefs : null) != null)
						{
							IEnumerable<ThingDef> enumerable = from x in ing.filter.AllowedThingDefs
							where Widgets.GetIconFor(x, null, null, null) != BaseContent.BadTex && ((ing.IsFixedIngredient && ing.filter.Allows(x)) || recipe.fixedIngredientFilter.AllowedDefCount == 0 || (recipe.fixedIngredientFilter.Allows(x) && !recipe.fixedIngredientFilter.IsAlwaysDisallowedDueToSpecialFilters(x)))
							select x;
							BillUtility.GetDistinctProductsFromIngredients(recipe, enumerable, BillUtility.tmpSpecialProducts);
							IEnumerable<TextureAndColor> enumerable2 = (from x in enumerable
							select x.ToTextureAndColor()).Distinct<TextureAndColor>();
							if (enumerable2.Any<TextureAndColor>())
							{
								if (!flag)
								{
									if (!BillUtility.recipeTooltipLayout.Empty)
									{
										BillUtility.recipeTooltipLayout.Newline(GameFont.Small);
									}
									BillUtility.recipeTooltipLayout.Label(("Ingredients".Translate() + ": ").AsTipTitle(), draw, GameFont.Small);
									flag = true;
								}
								BillUtility.recipeTooltipLayout.Newline(GameFont.Small);
								ThingDef singleIngredient = null;
								if (enumerable.EnumerableCount() == 1)
								{
									singleIngredient = enumerable.First<ThingDef>();
								}
								BillUtility.DisplayIngredientIconRow(enumerable2, draw, new float?(ing.CountFor(recipe)), singleIngredient);
							}
						}
					}
				}
			}
			if (!recipe.products.NullOrEmpty<ThingDefCountClass>() || BillUtility.tmpSpecialProducts.Count > 0)
			{
				if (!BillUtility.recipeTooltipLayout.Empty)
				{
					BillUtility.recipeTooltipLayout.Newline(GameFont.Small);
				}
				BillUtility.recipeTooltipLayout.Label(("Products".Translate() + ": ").AsTipTitle(), draw, GameFont.Small);
				BillUtility.recipeTooltipLayout.Newline(GameFont.Small);
				if (!recipe.products.NullOrEmpty<ThingDefCountClass>())
				{
					foreach (ThingDefCountClass thingDefCountClass in recipe.products)
					{
						BillUtility.DisplayIngredientIconRow(Gen.YieldSingle<TextureAndColor>(thingDefCountClass.thingDef.ToTextureAndColor()), draw, new float?((float)thingDefCountClass.count), null);
					}
					if (BillUtility.tmpSpecialProducts.Count > 0)
					{
						BillUtility.recipeTooltipLayout.Newline(GameFont.Small);
					}
				}
				if (BillUtility.tmpSpecialProducts.Count > 0)
				{
					BillUtility.DisplayIngredientIconRow((from x in BillUtility.tmpSpecialProducts
					select x.ToTextureAndColor()).Distinct<TextureAndColor>(), draw, null, null);
				}
			}
			if (recipe.addsHediff != null && part != null && pawn != null)
			{
				AddedBodyPartProps addedPartProps = recipe.addsHediff.addedPartProps;
				if (addedPartProps != null && addedPartProps.solid)
				{
					List<Hediff> list = new List<Hediff>();
					List<Hediff_MissingPart> list2 = new List<Hediff_MissingPart>();
					List<Hediff_MissingPart> missingPartsCommonAncestors = pawn.health.hediffSet.GetMissingPartsCommonAncestors();
					using (IEnumerator<BodyPartRecord> enumerator3 = part.GetPartAndAllChildParts().GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							BodyPartRecord childPart = enumerator3.Current;
							Hediff item;
							if (pawn.health.hediffSet.TryGetDirectlyAddedPartFor(childPart, out item))
							{
								list.Add(item);
							}
							else
							{
								Hediff hediff = pawn.health.hediffSet.hediffs.FirstOrDefault((Hediff x) => x.Part == childPart && x is Hediff_Implant);
								if (hediff != null)
								{
									list.Add(hediff);
								}
								else
								{
									Hediff_MissingPart hediff_MissingPart = missingPartsCommonAncestors.Find((Hediff_MissingPart h) => h.Part == childPart);
									if (hediff_MissingPart != null)
									{
										list2.Add(hediff_MissingPart);
									}
								}
							}
						}
					}
					if (list.Count > 0)
					{
						if (!BillUtility.recipeTooltipLayout.Empty)
						{
							BillUtility.recipeTooltipLayout.Newline(GameFont.Small);
						}
						BillUtility.recipeTooltipLayout.Label(("Replaces".Translate() + ": ").Colorize(ColoredText.WarningColor), draw, GameFont.Small);
						BillUtility.recipeTooltipLayout.Label((from p in list
						select p.Label).ToCommaList(false, false).CapitalizeFirst().Colorize(ColoredText.WarningColor), draw, GameFont.Small);
					}
					if (list2.Count > 0)
					{
						if (!BillUtility.recipeTooltipLayout.Empty)
						{
							BillUtility.recipeTooltipLayout.Newline(GameFont.Small);
						}
						BillUtility.recipeTooltipLayout.Label(("Replaces".Translate() + ": ").AsTipTitle(), draw, GameFont.Small);
						BillUtility.recipeTooltipLayout.Label((from p in list2
						select "MissingPartWithLabel".Translate(p.Part.Label).ToString()).ToCommaList(false, false).CapitalizeFirst(), draw, GameFont.Small);
					}
				}
			}
			if (!BillUtility.recipeTooltipLayout.Empty)
			{
				BillUtility.recipeTooltipLayout.Expand(6f, 6f);
			}
		}

		// Token: 0x06004A3F RID: 19007 RVA: 0x001CDBC0 File Offset: 0x001CBDC0
		private static void GetDistinctProductsFromIngredients(RecipeDef recipe, IEnumerable<ThingDef> ingredients, HashSet<ThingDef> outProducts)
		{
			if (recipe.specialProducts.NullOrEmpty<SpecialProductType>())
			{
				return;
			}
			foreach (SpecialProductType specialProductType in recipe.specialProducts)
			{
				foreach (ThingDef thingDef in ingredients)
				{
					if (thingDef.IsCorpse)
					{
						IngestibleProperties ingestible = thingDef.ingestible;
						if (((ingestible != null) ? ingestible.sourceDef : null) != null)
						{
							thingDef = thingDef.ingestible.sourceDef;
						}
					}
					switch (specialProductType)
					{
					case SpecialProductType.Butchery:
					case SpecialProductType.StoneBlocks:
					{
						if (!thingDef.butcherProducts.NullOrEmpty<ThingDefCountClass>())
						{
							foreach (ThingDefCountClass thingDefCountClass in thingDef.butcherProducts)
							{
								outProducts.Add(thingDefCountClass.thingDef);
							}
						}
						RaceProperties race = thingDef.race;
						if (((race != null) ? race.meatDef : null) != null)
						{
							outProducts.Add(thingDef.race.meatDef);
						}
						RaceProperties race2 = thingDef.race;
						if (((race2 != null) ? race2.leatherDef : null) != null)
						{
							outProducts.Add(thingDef.race.leatherDef);
						}
						break;
					}
					case SpecialProductType.Smelted:
						if (!thingDef.CostList.NullOrEmpty<ThingDefCountClass>())
						{
							foreach (ThingDefCountClass thingDefCountClass2 in thingDef.CostList)
							{
								if (!thingDefCountClass2.thingDef.intricate && Mathf.Round((float)thingDefCountClass2.count * 0.25f) > 0f)
								{
									outProducts.Add(thingDefCountClass2.thingDef);
								}
							}
						}
						if (!thingDef.smeltProducts.NullOrEmpty<ThingDefCountClass>())
						{
							foreach (ThingDefCountClass thingDefCountClass3 in thingDef.smeltProducts)
							{
								outProducts.Add(thingDefCountClass3.thingDef);
							}
						}
						break;
					}
				}
			}
		}

		// Token: 0x06004A40 RID: 19008 RVA: 0x001CDE58 File Offset: 0x001CC058
		private static void DisplayIngredientIconRow(IEnumerable<TextureAndColor> icons, bool draw, float? count = null, ThingDef singleIngredient = null)
		{
			int num = icons.EnumerableCount();
			if (num <= 0)
			{
				return;
			}
			int num2 = Mathf.Min(9, num);
			BillUtility.recipeTooltipLayout.Gap(8f, 0f);
			if (count != null)
			{
				BillUtility.recipeTooltipLayout.Label(count.Value.ToString() + "x ", draw, GameFont.Small);
			}
			for (int i = 0; i < num2; i++)
			{
				TextureAndColor textureAndColor = icons.ElementAt(i);
				BillUtility.recipeTooltipLayout.Icon(textureAndColor.Texture, textureAndColor.Color, Text.LineHeightOf(GameFont.Small), draw);
				BillUtility.recipeTooltipLayout.Gap(4f, 0f);
			}
			if (num > 9)
			{
				Text.Anchor = TextAnchor.MiddleLeft;
				BillUtility.recipeTooltipLayout.Label(" " + "Etc".Translate() + "...", draw, GameFont.Small);
				Text.Anchor = TextAnchor.UpperLeft;
				return;
			}
			if (singleIngredient != null)
			{
				BillUtility.recipeTooltipLayout.Label(singleIngredient.LabelCap, draw, GameFont.Small);
			}
		}

		// Token: 0x06004A41 RID: 19009 RVA: 0x001CDF60 File Offset: 0x001CC160
		private static TextureAndColor ToTextureAndColor(this ThingDef td)
		{
			return new TextureAndColor(Widgets.GetIconFor(td, null, null, null), td.uiIconColor);
		}

		// Token: 0x04002F7B RID: 12155
		public static Bill Clipboard = null;

		// Token: 0x04002F7C RID: 12156
		private static readonly RecipeTooltipLayout recipeTooltipLayout = new RecipeTooltipLayout();

		// Token: 0x04002F7D RID: 12157
		private const int MaxIngredientIcons = 9;

		// Token: 0x04002F7E RID: 12158
		private const float Indent = 8f;

		// Token: 0x04002F7F RID: 12159
		private const float ElementSpacing = 4f;

		// Token: 0x04002F80 RID: 12160
		private const float Margin = 6f;

		// Token: 0x04002F81 RID: 12161
		private static HashSet<ThingDef> tmpSpecialProducts = new HashSet<ThingDef>();

		// Token: 0x04002F82 RID: 12162
		private static List<IngredientCount> tmpSortedIngredients = new List<IngredientCount>();
	}
}
