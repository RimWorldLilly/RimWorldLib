using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AE1 RID: 6881
	public class Dialog_BillConfig : Window
	{
		// Token: 0x17001FAF RID: 8111
		// (get) Token: 0x0600AFB1 RID: 44977 RVA: 0x003DD2D8 File Offset: 0x003DB4D8
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(800f, 664f);
			}
		}

		// Token: 0x17001FB0 RID: 8112
		// (get) Token: 0x0600AFB2 RID: 44978 RVA: 0x003DD2EC File Offset: 0x003DB4EC
		private static IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters
		{
			get
			{
				if (Dialog_BillConfig.cachedHiddenSpecialThingFilters != null)
				{
					return Dialog_BillConfig.cachedHiddenSpecialThingFilters;
				}
				Dialog_BillConfig.cachedHiddenSpecialThingFilters = new List<SpecialThingFilterDef>();
				if (ModsConfig.IdeologyActive)
				{
					Dialog_BillConfig.cachedHiddenSpecialThingFilters.Add(SpecialThingFilterDefOf.AllowCarnivore);
					Dialog_BillConfig.cachedHiddenSpecialThingFilters.Add(SpecialThingFilterDefOf.AllowVegetarian);
					Dialog_BillConfig.cachedHiddenSpecialThingFilters.Add(SpecialThingFilterDefOf.AllowCannibal);
					Dialog_BillConfig.cachedHiddenSpecialThingFilters.Add(SpecialThingFilterDefOf.AllowInsectMeat);
				}
				return Dialog_BillConfig.cachedHiddenSpecialThingFilters;
			}
		}

		// Token: 0x0600AFB3 RID: 44979 RVA: 0x003DD358 File Offset: 0x003DB558
		public Dialog_BillConfig(Bill_Production bill, IntVec3 billGiverPos) : base(null)
		{
			this.billGiverPos = billGiverPos;
			this.bill = bill;
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.absorbInputAroundWindow = true;
			this.closeOnClickedOutside = true;
		}

		// Token: 0x0600AFB4 RID: 44980 RVA: 0x003DD3A8 File Offset: 0x003DB5A8
		public override void PreOpen()
		{
			base.PreOpen();
			this.thingFilterState.quickSearch.Reset();
		}

		// Token: 0x0600AFB5 RID: 44981 RVA: 0x003DD3C0 File Offset: 0x003DB5C0
		private void AdjustCount(int offset)
		{
			SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			this.bill.repeatCount += offset;
			if (this.bill.repeatCount < 1)
			{
				this.bill.repeatCount = 1;
			}
		}

		// Token: 0x0600AFB6 RID: 44982 RVA: 0x003DD3FA File Offset: 0x003DB5FA
		public override void WindowUpdate()
		{
			this.bill.TryDrawIngredientSearchRadiusOnMap(this.billGiverPos);
		}

		// Token: 0x0600AFB7 RID: 44983 RVA: 0x003DD410 File Offset: 0x003DB610
		protected override void LateWindowOnGUI(Rect inRect)
		{
			Rect rect = new Rect(inRect.x, inRect.y, 34f, 34f);
			ThingStyleDef thingStyleDef = null;
			if (ModsConfig.IdeologyActive && this.bill.recipe.ProducedThingDef != null)
			{
				ThingStyleDef thingStyleDef2;
				if (!this.bill.globalStyle)
				{
					thingStyleDef2 = this.bill.style;
				}
				else
				{
					StyleCategoryPair styleCategoryPair = Faction.OfPlayer.ideos.PrimaryIdeo.style.StyleForThingDef(this.bill.recipe.ProducedThingDef, null);
					thingStyleDef2 = ((styleCategoryPair != null) ? styleCategoryPair.styleDef : null);
				}
				thingStyleDef = thingStyleDef2;
			}
			Def recipe = this.bill.recipe;
			ThingDef stuffDef = null;
			float scale = 1f;
			ThingStyleDef thingStyleDef3 = thingStyleDef;
			bool drawPlaceholder = true;
			int? graphicIndexOverride = this.bill.graphicIndexOverride;
			Widgets.DefIcon(rect, recipe, stuffDef, scale, thingStyleDef3, drawPlaceholder, null, null, graphicIndexOverride, 1f);
		}

		// Token: 0x0600AFB8 RID: 44984 RVA: 0x003DD4DC File Offset: 0x003DB6DC
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(40f, 0f, 400f, 34f), this.bill.LabelCap);
			float width = (float)((int)((inRect.width - 34f) / 3f));
			Rect rect = new Rect(0f, 80f, width, inRect.height - 80f);
			Rect rect2 = new Rect(rect.xMax + 17f, 50f, width, inRect.height - 50f - Window.CloseButSize.y);
			Rect rect3 = new Rect(rect2.xMax + 17f, 50f, 0f, inRect.height - 50f - Window.CloseButSize.y);
			rect3.xMax = inRect.xMax;
			Text.Font = GameFont.Small;
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(rect2);
			Listing_Standard listing_Standard2 = listing_Standard.BeginSection((float)Dialog_BillConfig.RepeatModeSubdialogHeight, 4f, 4f);
			if (listing_Standard2.ButtonText(this.bill.repeatMode.LabelCap, null, 1f))
			{
				BillRepeatModeUtility.MakeConfigFloatMenu(this.bill);
			}
			listing_Standard2.Gap(12f);
			if (this.bill.repeatMode == BillRepeatModeDefOf.RepeatCount)
			{
				listing_Standard2.Label("RepeatCount".Translate(this.bill.repeatCount), -1f, null);
				listing_Standard2.IntEntry(ref this.bill.repeatCount, ref this.repeatCountEditBuffer, 1, 0);
			}
			else if (this.bill.repeatMode == BillRepeatModeDefOf.TargetCount)
			{
				string text = "CurrentlyHave".Translate() + ": ";
				text += this.bill.recipe.WorkerCounter.CountProducts(this.bill).ToString();
				text += " / ";
				text += ((this.bill.targetCount < 999999) ? this.bill.targetCount.ToString() : "Infinite".Translate().ToLower().ToString());
				string str = this.bill.recipe.WorkerCounter.ProductsDescription(this.bill);
				if (!str.NullOrEmpty())
				{
					text += "\n" + "CountingProducts".Translate() + ": " + str.CapitalizeFirst();
				}
				listing_Standard2.Label(text, -1f, null);
				int targetCount = this.bill.targetCount;
				listing_Standard2.IntEntry(ref this.bill.targetCount, ref this.targetCountEditBuffer, this.bill.recipe.targetCountAdjustment, 0);
				this.bill.unpauseWhenYouHave = Mathf.Max(0, this.bill.unpauseWhenYouHave + (this.bill.targetCount - targetCount));
				ThingDef producedThingDef = this.bill.recipe.ProducedThingDef;
				if (producedThingDef != null)
				{
					if (producedThingDef.IsWeapon || producedThingDef.IsApparel)
					{
						listing_Standard2.CheckboxLabeled("IncludeEquipped".Translate(), ref this.bill.includeEquipped, null, 0f, 1f);
					}
					if (producedThingDef.IsApparel && producedThingDef.apparel.careIfWornByCorpse)
					{
						listing_Standard2.CheckboxLabeled("IncludeTainted".Translate(), ref this.bill.includeTainted, null, 0f, 1f);
					}
					TaggedString taggedString = (this.bill.GetIncludeSlotGroup() == null) ? "IncludeFromAll".Translate() : "IncludeSpecific".Translate(SlotGroup.GetGroupLabel(this.bill.GetIncludeSlotGroup()));
					if (listing_Standard2.ButtonText(taggedString, null, 1f))
					{
						Text.Font = GameFont.Small;
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						list.Add(new FloatMenuOption("IncludeFromAll".Translate(), delegate()
						{
							this.bill.SetIncludeGroup(null);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						using (IEnumerator<BillStoreModeDef> enumerator = (from bsm in DefDatabase<BillStoreModeDef>.AllDefs
						orderby bsm.listOrder
						select bsm).GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current == BillStoreModeDefOf.SpecificStockpile)
								{
									this.FillOutputDropdownOptions(ref list, "IncludeSpecific".Translate(), delegate(ISlotGroup slot)
									{
										this.bill.SetIncludeGroup(slot);
									});
								}
							}
						}
						Find.WindowStack.Add(new FloatMenu(list));
					}
					if (this.bill.recipe.products.Any((ThingDefCountClass prod) => prod.thingDef.useHitPoints))
					{
						Widgets.FloatRange(listing_Standard2.GetRect(32f, 1f), 975643279, ref this.bill.hpRange, 0f, 1f, "HitPoints", ToStringStyle.PercentZero, 0f, GameFont.Small, null, 0f);
						this.bill.hpRange.min = Mathf.Round(this.bill.hpRange.min * 100f) / 100f;
						this.bill.hpRange.max = Mathf.Round(this.bill.hpRange.max * 100f) / 100f;
					}
					if (producedThingDef.HasComp(typeof(CompQuality)))
					{
						Widgets.QualityRange(listing_Standard2.GetRect(32f, 1f), 1098906561, ref this.bill.qualityRange);
					}
					if (producedThingDef.MadeFromStuff)
					{
						listing_Standard2.CheckboxLabeled("LimitToAllowedStuff".Translate(), ref this.bill.limitToAllowedStuff, null, 0f, 1f);
					}
				}
			}
			if (this.bill.repeatMode == BillRepeatModeDefOf.TargetCount)
			{
				listing_Standard2.CheckboxLabeled("PauseWhenSatisfied".Translate(), ref this.bill.pauseWhenSatisfied, null, 0f, 1f);
				if (this.bill.pauseWhenSatisfied)
				{
					listing_Standard2.Label("UnpauseWhenYouHave".Translate() + ": " + this.bill.unpauseWhenYouHave.ToString("F0"), -1f, null);
					listing_Standard2.IntEntry(ref this.bill.unpauseWhenYouHave, ref this.unpauseCountEditBuffer, this.bill.recipe.targetCountAdjustment, 0);
					if (this.bill.unpauseWhenYouHave >= this.bill.targetCount)
					{
						this.bill.unpauseWhenYouHave = this.bill.targetCount - 1;
						this.unpauseCountEditBuffer = this.bill.unpauseWhenYouHave.ToStringCached();
					}
				}
			}
			listing_Standard.EndSection(listing_Standard2);
			listing_Standard.Gap(12f);
			Listing_Standard listing_Standard3 = listing_Standard.BeginSection((float)Dialog_BillConfig.StoreModeSubdialogHeight, 4f, 4f);
			string text2 = string.Format(this.bill.GetStoreMode().LabelCap, (this.bill.GetSlotGroup() != null) ? SlotGroup.GetGroupLabel(this.bill.GetSlotGroup()) : "");
			if (this.bill.GetSlotGroup() != null && !this.bill.recipe.WorkerCounter.CanPossiblyStore(this.bill, this.bill.GetSlotGroup()))
			{
				text2 += string.Format(" ({0})", "IncompatibleLower".Translate());
				Text.Font = GameFont.Tiny;
			}
			if (listing_Standard3.ButtonText(text2, null, 1f))
			{
				Text.Font = GameFont.Small;
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				foreach (BillStoreModeDef billStoreModeDef in from bsm in DefDatabase<BillStoreModeDef>.AllDefs
				orderby bsm.listOrder
				select bsm)
				{
					if (billStoreModeDef == BillStoreModeDefOf.SpecificStockpile)
					{
						this.FillOutputDropdownOptions(ref list2, BillStoreModeDefOf.SpecificStockpile.LabelCap, delegate(ISlotGroup slot)
						{
							this.bill.SetStoreMode(BillStoreModeDefOf.SpecificStockpile, slot);
						});
					}
					else
					{
						BillStoreModeDef smLocal = billStoreModeDef;
						list2.Add(new FloatMenuOption(smLocal.LabelCap, delegate()
						{
							this.bill.SetStoreMode(smLocal, null);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			Text.Font = GameFont.Small;
			listing_Standard.EndSection(listing_Standard3);
			listing_Standard.Gap(12f);
			Listing_Standard listing_Standard4 = listing_Standard.BeginSection((float)Dialog_BillConfig.WorkerSelectionSubdialogHeight, 4f, 4f);
			string buttonLabel;
			if (this.bill.PawnRestriction != null)
			{
				buttonLabel = this.bill.PawnRestriction.LabelShortCap;
			}
			else if (ModsConfig.IdeologyActive && this.bill.SlavesOnly)
			{
				buttonLabel = "AnySlave".Translate();
			}
			else if (ModsConfig.BiotechActive && this.bill.recipe.mechanitorOnlyRecipe)
			{
				buttonLabel = "AnyMechanitor".Translate();
			}
			else if (ModsConfig.BiotechActive && this.bill.MechsOnly)
			{
				buttonLabel = "AnyMech".Translate();
			}
			else if (ModsConfig.BiotechActive && this.bill.NonMechsOnly)
			{
				buttonLabel = "AnyNonMech".Translate();
			}
			else
			{
				buttonLabel = "AnyWorker".Translate();
			}
			Widgets.Dropdown<Bill_Production, Pawn>(listing_Standard4.GetRect(30f, 1f), this.bill, (Bill_Production b) => b.PawnRestriction, (Bill_Production b) => this.GeneratePawnRestrictionOptions(), buttonLabel, null, null, null, null, false);
			if (this.bill.PawnRestriction == null && this.bill.recipe.workSkill != null && !this.bill.MechsOnly)
			{
				listing_Standard4.Label("AllowedSkillRange".Translate(this.bill.recipe.workSkill.label) + ":", -1f, null);
				listing_Standard4.IntRange(ref this.bill.allowedSkillRange, 0, 20);
			}
			listing_Standard.EndSection(listing_Standard4);
			listing_Standard.End();
			float y = rect3.y;
			this.DoIngredientConfigPane(rect3.x, ref y, rect3.width, rect3.height);
			Listing_Standard listing_Standard5 = new Listing_Standard();
			listing_Standard5.Begin(rect);
			if (this.bill.suspended)
			{
				if (listing_Standard5.ButtonText("Suspended".Translate(), null, 1f))
				{
					this.bill.suspended = false;
					SoundDefOf.Click.PlayOneShotOnCamera(null);
				}
			}
			else if (listing_Standard5.ButtonText("NotSuspended".Translate(), null, 1f))
			{
				this.bill.suspended = true;
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (this.bill.recipe.description != null)
			{
				stringBuilder.AppendLine(this.bill.recipe.description);
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine("WorkAmount".Translate() + ": " + this.bill.recipe.WorkAmountTotal(null).ToStringWorkAmount());
			if (ModsConfig.BiotechActive && this.bill.recipe.products.Count == 1)
			{
				ThingDef thingDef = this.bill.recipe.products[0].thingDef;
				if (thingDef.IsApparel)
				{
					stringBuilder.AppendLine("WearableBy".Translate() + ": " + thingDef.apparel.developmentalStageFilter.ToCommaList(false).CapitalizeFirst());
				}
			}
			Bill_Mech bill_Mech = this.bill as Bill_Mech;
			if (bill_Mech != null)
			{
				stringBuilder.AppendLine("GestationCycles".Translate() + ": " + this.bill.recipe.gestationCycles.ToString());
				Pawn gestatingMech = bill_Mech.Gestator.GestatingMech;
				ThingDef thingDef2 = ((gestatingMech != null) ? gestatingMech.def : null) ?? bill_Mech.recipe.ProducedThingDef;
				if (thingDef2 != null)
				{
					stringBuilder.AppendLine("Bandwidth".Translate() + ": " + thingDef2.GetStatValueAbstract(StatDefOf.BandwidthCost, null).ToString());
				}
				if (!this.bill.recipe.mechResurrection)
				{
					float num = (float)((int)this.bill.recipe.ProducedThingDef.GetStatValueAbstract(StatDefOf.WastepacksPerRecharge, null)) * this.bill.recipe.ProducedThingDef.GetStatValueAbstract(StatDefOf.BandwidthCost, null);
					stringBuilder.AppendLine(Find.ActiveLanguageWorker.Pluralize(ThingDefOf.Wastepack.LabelCap, -1) + " " + "ThingsProduced".Translate() + ": " + num.ToString());
				}
			}
			stringBuilder.AppendLine("BillRequires".Translate() + ": ");
			Dialog_BillConfig.tmpSortedIngredients.Clear();
			Dialog_BillConfig.tmpSortedIngredients.AddRange(this.bill.recipe.ingredients);
			Dialog_BillConfig.tmpSortedIngredients.Sort((IngredientCount a, IngredientCount b) => Mathf.RoundToInt(b.CountFor(this.bill.recipe) - a.CountFor(this.bill.recipe)));
			foreach (IngredientCount ingredientCount in Dialog_BillConfig.tmpSortedIngredients)
			{
				if (!ingredientCount.filter.Summary.NullOrEmpty())
				{
					stringBuilder.AppendLine(" - " + this.bill.recipe.IngredientValueGetter.BillRequirementsDescription(this.bill.recipe, ingredientCount));
				}
			}
			stringBuilder.AppendLine();
			string text3 = this.bill.recipe.IngredientValueGetter.ExtraDescriptionLine(this.bill.recipe);
			if (text3 != null)
			{
				stringBuilder.AppendLine(text3);
				stringBuilder.AppendLine();
			}
			if (!this.bill.recipe.skillRequirements.NullOrEmpty<SkillRequirement>())
			{
				stringBuilder.AppendLine("MinimumSkills".Translate());
				stringBuilder.AppendLine(this.bill.recipe.MinSkillString);
			}
			Text.Font = GameFont.Small;
			string text4 = stringBuilder.ToString();
			if (Text.CalcHeight(text4, rect.width) > rect.height)
			{
				Text.Font = GameFont.Tiny;
			}
			listing_Standard5.Label(text4, -1f, null);
			Text.Font = GameFont.Small;
			if (ModsConfig.IdeologyActive && Find.IdeoManager.classicMode && this.bill.recipe.ProducedThingDef != null)
			{
				listing_Standard5.Gap(rect.height - listing_Standard5.CurHeight - 90f);
				ThingDef producedThingDef2 = this.bill.recipe.ProducedThingDef;
				List<StyleCategoryDef> relevantStyleCategories = this.bill.recipe.ProducedThingDef.RelevantStyleCategories;
				if (relevantStyleCategories.Any<StyleCategoryDef>())
				{
					StyleCategoryPair global = Faction.OfPlayer.ideos.PrimaryIdeo.style.StyleForThingDef(producedThingDef2, null);
					string text5 = (global == null) ? "Basic".Translate().CapitalizeFirst() : global.category.LabelCap;
					string text6 = (this.bill.style != null) ? this.bill.style.Category.LabelCap : "Basic".Translate().CapitalizeFirst();
					string text7 = this.bill.globalStyle ? ("UseGlobalStyle".Translate().ToString() + " (" + text5 + ")") : text6;
					if (!this.bill.globalStyle && this.bill.style != null && this.bill.graphicIndexOverride != null)
					{
						text7 = text7 + " " + (this.bill.graphicIndexOverride.Value + 1).ToString();
					}
					if (listing_Standard5.ButtonText(text7, null, 1f))
					{
						GenStuff.DefaultStuffFor(producedThingDef2);
						List<FloatMenuOption> list3 = new List<FloatMenuOption>();
						List<FloatMenuOption> list4 = list3;
						string label = "UseGlobalStyle".Translate() + " (" + text5 + ")";
						Action action = delegate()
						{
							Bill bill = this.bill;
							StyleCategoryPair global2 = global;
							bill.style = ((global2 != null) ? global2.styleDef : null);
							this.bill.globalStyle = true;
							this.bill.graphicIndexOverride = null;
						};
						ThingDef uiiconThing = this.bill.recipe.UIIconThing;
						Texture2D uiicon = this.bill.recipe.UIIcon;
						StyleCategoryPair global3 = global;
						list4.Add(new FloatMenuOption(label, action, uiiconThing, uiicon, (global3 != null) ? global3.styleDef : null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null));
						list3.Add(new FloatMenuOption("Basic".Translate().CapitalizeFirst(), delegate()
						{
							this.bill.style = null;
							this.bill.globalStyle = false;
							this.bill.graphicIndexOverride = null;
						}, this.bill.recipe.UIIconThing, this.bill.recipe.UIIcon, null, true, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null));
						foreach (StyleCategoryDef styleCategoryDef in relevantStyleCategories)
						{
							using (List<ThingDefStyle>.Enumerator enumerator4 = styleCategoryDef.thingDefStyles.GetEnumerator())
							{
								while (enumerator4.MoveNext())
								{
									ThingDefStyle style = enumerator4.Current;
									if (producedThingDef2 == style.ThingDef)
									{
										Graphic_Random graphic_Random = style.StyleDef.Graphic as Graphic_Random;
										if (graphic_Random != null)
										{
											for (int i = 0; i < graphic_Random.SubGraphicsCount; i++)
											{
												int index = i;
												list3.Add(new FloatMenuOption(styleCategoryDef.LabelCap + " " + (index + 1).ToString(), delegate()
												{
													this.bill.style = style.StyleDef;
													this.bill.globalStyle = false;
													this.bill.graphicIndexOverride = new int?(index);
												}, this.bill.recipe.UIIconThing, this.bill.recipe.UIIcon, style.StyleDef, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, new int?(index)));
											}
										}
										else
										{
											list3.Add(new FloatMenuOption(styleCategoryDef.LabelCap, delegate()
											{
												this.bill.style = style.StyleDef;
												this.bill.globalStyle = false;
												this.bill.graphicIndexOverride = null;
											}, this.bill.recipe.UIIconThing, this.bill.recipe.UIIcon, style.StyleDef, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null));
										}
									}
								}
							}
						}
						Find.WindowStack.Add(new FloatMenu(list3));
					}
				}
				else
				{
					Rect rect4 = listing_Standard5.GetRect(30f, 1f);
					Widgets.DrawHighlight(rect4);
					Text.Anchor = TextAnchor.MiddleCenter;
					Widgets.Label(rect4, "NoStylesAvailable".Translate());
					Text.Anchor = TextAnchor.UpperLeft;
				}
			}
			listing_Standard5.End();
			float num2 = rect.x;
			if (this.bill.recipe.products.Count == 1)
			{
				ThingDef thingDef3 = this.bill.recipe.products[0].thingDef;
				Widgets.InfoCardButton(num2, rect3.y, thingDef3, GenStuff.DefaultStuffFor(thingDef3));
				num2 += 28f;
			}
			if (Widgets.ButtonImage(new Rect(num2, rect3.y, 30f, 30f), TexButton.Rename, true, null))
			{
				Find.WindowStack.Add(new Dialog_RenameBill(this.bill));
			}
		}

		// Token: 0x0600AFB9 RID: 44985 RVA: 0x003DEA94 File Offset: 0x003DCC94
		private void FillOutputDropdownOptions(ref List<FloatMenuOption> opts, string prefix, Action<ISlotGroup> selected)
		{
			List<SlotGroup> allGroupsListInPriorityOrder = this.bill.billStack.billGiver.Map.haulDestinationManager.AllGroupsListInPriorityOrder;
			Dialog_BillConfig.tmpGroups.ClearValueLists<string, ISlotGroup>();
			for (int i = 0; i < allGroupsListInPriorityOrder.Count; i++)
			{
				SlotGroup slotGroup = allGroupsListInPriorityOrder[i];
				if (slotGroup.StorageGroup != null)
				{
					StorageGroup storageGroup = slotGroup.StorageGroup;
					if (!Dialog_BillConfig.tmpGroups.ContainsKey(storageGroup.GroupingLabel))
					{
						Dialog_BillConfig.tmpGroups.Add(storageGroup.GroupingLabel, new List<ISlotGroup>());
					}
					if (!Dialog_BillConfig.tmpGroups[storageGroup.GroupingLabel].Contains(slotGroup.StorageGroup))
					{
						Dialog_BillConfig.tmpGroups[storageGroup.GroupingLabel].Add(slotGroup.StorageGroup);
					}
				}
				else
				{
					Building_Storage building_Storage = slotGroup.parent as Building_Storage;
					if (building_Storage == null || building_Storage is IRenameable)
					{
						if (!Dialog_BillConfig.tmpGroups.ContainsKey(slotGroup.GroupingLabel))
						{
							Dialog_BillConfig.tmpGroups.Add(slotGroup.GroupingLabel, new List<ISlotGroup>());
						}
						Dialog_BillConfig.tmpGroups[slotGroup.GroupingLabel].Add(slotGroup);
					}
				}
			}
			using (IEnumerator<KeyValuePair<string, List<ISlotGroup>>> enumerator = Dialog_BillConfig.tmpGroups.OrderBy(delegate(KeyValuePair<string, List<ISlotGroup>> kvp)
			{
				if (kvp.Value.Count <= 0)
				{
					return 0;
				}
				return kvp.Value[0].GroupingOrder;
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, List<ISlotGroup>> kvp = enumerator.Current;
					if (this.ShouldCollapseGroup(kvp.Key))
					{
						opts.Add(new FloatMenuOption(kvp.Key, delegate()
						{
							List<FloatMenuOption> list = new List<FloatMenuOption>();
							this.FillSlotGroupOptions(kvp.Value, list, prefix, selected);
							Find.WindowStack.Add(new FloatMenu(list));
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
			}
			foreach (KeyValuePair<string, List<ISlotGroup>> keyValuePair in Dialog_BillConfig.tmpGroups.OrderBy(delegate(KeyValuePair<string, List<ISlotGroup>> kvp)
			{
				if (kvp.Value.Count <= 0)
				{
					return 0;
				}
				return kvp.Value[0].GroupingOrder;
			}))
			{
				if (!this.ShouldCollapseGroup(keyValuePair.Key))
				{
					this.FillSlotGroupOptions(keyValuePair.Value, opts, prefix, selected);
				}
			}
		}

		// Token: 0x0600AFBA RID: 44986 RVA: 0x003DED14 File Offset: 0x003DCF14
		private bool ShouldCollapseGroup(string key)
		{
			bool flag = false;
			foreach (KeyValuePair<string, List<ISlotGroup>> keyValuePair in Dialog_BillConfig.tmpGroups)
			{
				if (keyValuePair.Key != key && keyValuePair.Value.Count > 0)
				{
					flag = true;
					break;
				}
			}
			return Dialog_BillConfig.tmpGroups[key].Count > 2 && flag;
		}

		// Token: 0x0600AFBB RID: 44987 RVA: 0x003DED98 File Offset: 0x003DCF98
		protected virtual void DoIngredientConfigPane(float x, ref float y, float width, float height)
		{
			bool flag = true;
			for (int i = 0; i < this.bill.recipe.ingredients.Count; i++)
			{
				if (!this.bill.recipe.ingredients[i].IsFixedIngredient)
				{
					flag = false;
					break;
				}
			}
			if (!flag)
			{
				Rect rect = new Rect(x, y, width, height - (float)Dialog_BillConfig.IngredientRadiusSubdialogHeight);
				bool flag2 = this.bill.GetSlotGroup() == null || this.bill.recipe.WorkerCounter.CanPossiblyStore(this.bill, this.bill.GetSlotGroup());
				ThingFilterUI.DoThingFilterConfigWindow(rect, this.thingFilterState, this.bill.ingredientFilter, this.bill.recipe.fixedIngredientFilter, 4, null, Dialog_BillConfig.HiddenSpecialThingFilters.ConcatIfNotNull(this.bill.recipe.forceHiddenSpecialFilters), false, false, false, this.bill.recipe.GetPremultipliedSmallIngredients(), this.bill.Map);
				y += rect.height;
				bool flag3 = this.bill.GetSlotGroup() == null || this.bill.recipe.WorkerCounter.CanPossiblyStore(this.bill, this.bill.GetSlotGroup());
				if (flag2 && !flag3)
				{
					Messages.Message("MessageBillValidationStoreZoneInsufficient".Translate(this.bill.LabelCap, this.bill.billStack.billGiver.LabelShort.CapitalizeFirst(), SlotGroup.GetGroupLabel(this.bill.GetSlotGroup())), this.bill.billStack.billGiver as Thing, MessageTypeDefOf.RejectInput, false);
				}
			}
			Rect rect2 = new Rect(x, y, width, (float)Dialog_BillConfig.IngredientRadiusSubdialogHeight);
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(rect2);
			string str = "IngredientSearchRadius".Translate().Truncate(rect2.width * 0.6f, null);
			string str2 = (this.bill.ingredientSearchRadius == 999f) ? "Unlimited".TranslateSimple().Truncate(rect2.width * 0.3f, null) : this.bill.ingredientSearchRadius.ToString("F0");
			listing_Standard.Label(str + ": " + str2, -1f, null);
			this.bill.ingredientSearchRadius = listing_Standard.Slider((this.bill.ingredientSearchRadius > 100f) ? 100f : this.bill.ingredientSearchRadius, 3f, 100f);
			if (this.bill.ingredientSearchRadius >= 100f)
			{
				this.bill.ingredientSearchRadius = 999f;
			}
			listing_Standard.End();
			y += (float)Dialog_BillConfig.IngredientRadiusSubdialogHeight;
		}

		// Token: 0x0600AFBC RID: 44988 RVA: 0x003DF07F File Offset: 0x003DD27F
		protected virtual IEnumerable<Widgets.DropdownMenuElement<Pawn>> GeneratePawnRestrictionOptions()
		{
			Dialog_BillConfig.<GeneratePawnRestrictionOptions>d__27 <GeneratePawnRestrictionOptions>d__ = new Dialog_BillConfig.<GeneratePawnRestrictionOptions>d__27(-2);
			<GeneratePawnRestrictionOptions>d__.<>4__this = this;
			return <GeneratePawnRestrictionOptions>d__;
		}

		// Token: 0x0600AFBD RID: 44989 RVA: 0x003DF090 File Offset: 0x003DD290
		private void FillSlotGroupOptions(List<ISlotGroup> groups, List<FloatMenuOption> opts, string prefix, Action<ISlotGroup> selected)
		{
			for (int i = 0; i < groups.Count; i++)
			{
				ISlotGroup group = groups[i];
				if (!this.bill.recipe.WorkerCounter.CanPossiblyStore(this.bill, group))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
					defaultInterpolatedStringHandler.AppendFormatted(string.Format(prefix, SlotGroup.GetGroupLabel(group)));
					defaultInterpolatedStringHandler.AppendLiteral(" (");
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("IncompatibleLower".Translate());
					defaultInterpolatedStringHandler.AppendLiteral(")");
					opts.Add(new FloatMenuOption(defaultInterpolatedStringHandler.ToStringAndClear(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				else
				{
					opts.Add(new FloatMenuOption(string.Format(prefix, SlotGroup.GetGroupLabel(group)), delegate()
					{
						selected(group);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
		}

		// Token: 0x04006562 RID: 25954
		private IntVec3 billGiverPos;

		// Token: 0x04006563 RID: 25955
		protected Bill_Production bill;

		// Token: 0x04006564 RID: 25956
		private ThingFilterUI.UIState thingFilterState = new ThingFilterUI.UIState();

		// Token: 0x04006565 RID: 25957
		private string repeatCountEditBuffer;

		// Token: 0x04006566 RID: 25958
		private string targetCountEditBuffer;

		// Token: 0x04006567 RID: 25959
		private string unpauseCountEditBuffer;

		// Token: 0x04006568 RID: 25960
		protected const float RecipeIconSize = 34f;

		// Token: 0x04006569 RID: 25961
		[TweakValue("Interface", 0f, 400f)]
		private static int RepeatModeSubdialogHeight = 354;

		// Token: 0x0400656A RID: 25962
		[TweakValue("Interface", 0f, 400f)]
		private static int StoreModeSubdialogHeight = 30;

		// Token: 0x0400656B RID: 25963
		[TweakValue("Interface", 0f, 400f)]
		private static int WorkerSelectionSubdialogHeight = 96;

		// Token: 0x0400656C RID: 25964
		[TweakValue("Interface", 0f, 400f)]
		private static int IngredientRadiusSubdialogHeight = 50;

		// Token: 0x0400656D RID: 25965
		private static List<IngredientCount> tmpSortedIngredients = new List<IngredientCount>();

		// Token: 0x0400656E RID: 25966
		private static List<SpecialThingFilterDef> cachedHiddenSpecialThingFilters;

		// Token: 0x0400656F RID: 25967
		private static readonly Dictionary<string, List<ISlotGroup>> tmpGroups = new Dictionary<string, List<ISlotGroup>>();
	}
}
