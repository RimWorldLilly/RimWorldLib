using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001CAA RID: 7338
	[StaticConstructorOnStartup]
	public class Dialog_EditIdeoStyleItems : Window
	{
		// Token: 0x17002228 RID: 8744
		// (get) Token: 0x0600BBE8 RID: 48104 RVA: 0x0042F27F File Offset: 0x0042D47F
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(Mathf.Min(1200f, (float)UI.screenWidth), 750f);
			}
		}

		// Token: 0x17002229 RID: 8745
		// (get) Token: 0x0600BBE9 RID: 48105 RVA: 0x0042F29B File Offset: 0x0042D49B
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x0600BBEA RID: 48106 RVA: 0x0042F2A4 File Offset: 0x0042D4A4
		public Dialog_EditIdeoStyleItems(Ideo ideo, StyleItemTab category, IdeoEditMode editMode) : base(null)
		{
			this.ideo = ideo;
			this.curTab = category;
			this.editMode = editMode;
			this.absorbInputAroundWindow = true;
			this.allFrequencies = (StyleItemFrequency[])Enum.GetValues(typeof(StyleItemFrequency));
			this.expandedInfos = new List<Dialog_EditIdeoStyleItems.ExpandedInfo>();
			foreach (Dialog_EditIdeoStyleItems.ItemType itemType in (Dialog_EditIdeoStyleItems.ItemType[])Enum.GetValues(typeof(Dialog_EditIdeoStyleItems.ItemType)))
			{
				foreach (StyleItemCategoryDef styleItemCategoryDef in DefDatabase<StyleItemCategoryDef>.AllDefs)
				{
					this.expandedInfos.Add(new Dialog_EditIdeoStyleItems.ExpandedInfo(styleItemCategoryDef, itemType, this.CanList(styleItemCategoryDef, itemType)));
				}
			}
			this.Reset();
		}

		// Token: 0x0600BBEB RID: 48107 RVA: 0x0042F394 File Offset: 0x0042D594
		private void Reset()
		{
			this.hairFrequencies = new DefMap<HairDef, StyleItemSpawningProperties>();
			foreach (KeyValuePair<HairDef, StyleItemSpawningProperties> keyValuePair in this.hairFrequencies)
			{
				this.hairFrequencies[keyValuePair.Key].frequency = this.ideo.style.GetFrequency(keyValuePair.Key);
				this.hairFrequencies[keyValuePair.Key].gender = this.ideo.style.GetGender(keyValuePair.Key);
			}
			this.beardFrequencies = new DefMap<BeardDef, StyleItemSpawningProperties>();
			foreach (KeyValuePair<BeardDef, StyleItemSpawningProperties> keyValuePair2 in this.beardFrequencies)
			{
				this.beardFrequencies[keyValuePair2.Key].frequency = this.ideo.style.GetFrequency(keyValuePair2.Key);
				this.beardFrequencies[keyValuePair2.Key].gender = this.ideo.style.GetGender(keyValuePair2.Key);
			}
			this.tattooFrequencies = new DefMap<TattooDef, StyleItemSpawningProperties>();
			foreach (KeyValuePair<TattooDef, StyleItemSpawningProperties> keyValuePair3 in this.tattooFrequencies)
			{
				this.tattooFrequencies[keyValuePair3.Key].frequency = this.ideo.style.GetFrequency(keyValuePair3.Key);
				this.tattooFrequencies[keyValuePair3.Key].gender = this.ideo.style.GetGender(keyValuePair3.Key);
			}
		}

		// Token: 0x0600BBEC RID: 48108 RVA: 0x0042F580 File Offset: 0x0042D780
		public override void PostOpen()
		{
			base.PostOpen();
			if (!ModLister.CheckIdeology("Appearance editing"))
			{
				this.Close(true);
			}
		}

		// Token: 0x0600BBED RID: 48109 RVA: 0x0042F59C File Offset: 0x0042D79C
		public override void DoWindowContents(Rect rect)
		{
			this.hover = null;
			Rect rect2 = new Rect(rect);
			rect2.xMin += 18f;
			rect2.yMin += 10f;
			rect2.height = 35f;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect2, "EditAppearanceItems".Translate());
			Text.Font = GameFont.Small;
			Rect rect3 = rect;
			rect3.yMin = rect2.yMax + 35f;
			rect3.yMax -= Dialog_EditIdeoStyleItems.ButSize.y + 10f;
			this.FillDialog(rect3);
			Rect rect4 = new Rect(rect.xMax - Dialog_EditIdeoStyleItems.ButSize.x - 10f, rect.y + 10f, Dialog_EditIdeoStyleItems.ButSize.x, 30f);
			if (Widgets.ButtonText(rect4, "ExpandAllCategories".Translate().CapitalizeFirst(), true, true, true, null))
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				foreach (Dialog_EditIdeoStyleItems.ExpandedInfo expandedInfo in this.expandedInfos)
				{
					expandedInfo.expanded = true;
				}
			}
			if (Widgets.ButtonText(new Rect(rect4.x, rect4.yMax + 4f, Dialog_EditIdeoStyleItems.ButSize.x, 30f), "CollapseAllCategories".Translate().CapitalizeFirst(), true, true, true, null))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				foreach (Dialog_EditIdeoStyleItems.ExpandedInfo expandedInfo2 in this.expandedInfos)
				{
					expandedInfo2.expanded = false;
				}
			}
			rect = rect.ContractedBy(18f);
			float y = rect.height - Dialog_EditIdeoStyleItems.ButSize.y + 18f;
			Rect rect5 = new Rect(rect.x + rect.width / 2f - Dialog_EditIdeoStyleItems.ButSize.x / 2f, y, Dialog_EditIdeoStyleItems.ButSize.x, Dialog_EditIdeoStyleItems.ButSize.y);
			if (this.editMode == IdeoEditMode.None)
			{
				if (Widgets.ButtonText(rect5, "Back".Translate(), true, true, true, null))
				{
					this.Close(true);
					return;
				}
			}
			else
			{
				if (Widgets.ButtonText(new Rect(rect.x, y, Dialog_EditIdeoStyleItems.ButSize.x, Dialog_EditIdeoStyleItems.ButSize.y), "Cancel".Translate(), true, true, true, null))
				{
					this.Close(true);
				}
				TooltipHandler.TipRegion(rect5, "ResetButtonDesc".Translate());
				if (Widgets.ButtonText(rect5, "Reset".Translate(), true, true, true, null))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					this.Reset();
				}
				if (Widgets.ButtonText(new Rect(rect.width - Dialog_EditIdeoStyleItems.ButSize.x + 18f, y, Dialog_EditIdeoStyleItems.ButSize.x, Dialog_EditIdeoStyleItems.ButSize.y), "DoneButton".Translate(), true, true, true, null))
				{
					this.Done();
				}
			}
		}

		// Token: 0x0600BBEE RID: 48110 RVA: 0x0042F92C File Offset: 0x0042DB2C
		public override void OnAcceptKeyPressed()
		{
			this.Done();
			Event.current.Use();
		}

		// Token: 0x0600BBEF RID: 48111 RVA: 0x0042F940 File Offset: 0x0042DB40
		private void FillDialog(Rect rect)
		{
			this.DrawTabs(rect);
			rect = rect.ContractedBy(18f);
			Rect rect2 = new Rect(rect.x, rect.y, rect.width / 2f - 5f, rect.height);
			this.DrawSection(rect2, ref this.scrollPositionLeft, ref this.scrollViewHeightLeft, (this.curTab == StyleItemTab.HairAndBeard) ? Dialog_EditIdeoStyleItems.ItemType.Hair : Dialog_EditIdeoStyleItems.ItemType.FaceTattoo);
			Rect rect3 = new Rect(rect2.xMax + 10f, rect.y, rect.width / 2f - 10f, rect.height);
			this.DrawSection(rect3, ref this.scrollPositionRight, ref this.scrollViewHeightRight, (this.curTab == StyleItemTab.HairAndBeard) ? Dialog_EditIdeoStyleItems.ItemType.Beard : Dialog_EditIdeoStyleItems.ItemType.BodyTattoo);
			if (!Input.GetMouseButton(0))
			{
				this.painting = false;
			}
		}

		// Token: 0x0600BBF0 RID: 48112 RVA: 0x0042FA14 File Offset: 0x0042DC14
		private void DrawTabs(Rect rect)
		{
			this.tabs.Clear();
			this.tabs.Add(new TabRecord("HairAndBeards".Translate(), delegate()
			{
				this.curTab = StyleItemTab.HairAndBeard;
				this.scrollPositionLeft = (this.scrollPositionRight = Vector2.zero);
			}, this.curTab == StyleItemTab.HairAndBeard));
			this.tabs.Add(new TabRecord("Tattoos".Translate(), delegate()
			{
				this.curTab = StyleItemTab.Tattoo;
				this.scrollPositionLeft = (this.scrollPositionRight = Vector2.zero);
			}, this.curTab == StyleItemTab.Tattoo));
			TabDrawer.DrawTabs<TabRecord>(rect, this.tabs, 200f);
		}

		// Token: 0x0600BBF1 RID: 48113 RVA: 0x0042FAA8 File Offset: 0x0042DCA8
		private void DrawSection(Rect rect, ref Vector2 scrollPosition, ref float scrollViewHeight, Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(new Rect(rect.x + 10f, rect.y, rect.width, 30f), this.GetSectionLabel(itemType).CapitalizeFirst());
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			rect.yMin += 30f;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			for (int i = 0; i < this.allFrequencies.Length; i++)
			{
				Rect rect2 = this.FrequencyPosition(new Rect(rect.x, rect.y, rect.width, Text.LineHeight), i);
				string label = this.allFrequencies[i].GetLabel();
				string text = label.Truncate(rect2.width, null);
				Widgets.Label(rect2, text);
				if (label != text)
				{
					TooltipHandler.TipRegion(rect2, label);
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			rect.yMin += Text.LineHeight;
			Widgets.BeginGroup(rect);
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, scrollViewHeight);
			float num = 0f;
			float num2 = 28f;
			Widgets.BeginScrollView(rect.AtZero(), ref scrollPosition, viewRect, true);
			using (IEnumerator<StyleItemCategoryDef> enumerator = DefDatabase<StyleItemCategoryDef>.AllDefs.GetEnumerator())
			{
				Func<StyleItemDef, bool> <>9__1;
				while (enumerator.MoveNext())
				{
					StyleItemCategoryDef c = enumerator.Current;
					Dialog_EditIdeoStyleItems.ExpandedInfo expandedInfo = this.expandedInfos.FirstOrDefault((Dialog_EditIdeoStyleItems.ExpandedInfo x) => x.categoryDef == c && itemType == x.itemType);
					if (expandedInfo != null && expandedInfo.any)
					{
						this.ListStyleItemCategory(c, ref num, viewRect, expandedInfo, itemType);
						float num3 = num2;
						float num4 = 28f;
						float num5 = 28f;
						IEnumerable<StyleItemDef> itemsInCategory = c.ItemsInCategory;
						Func<StyleItemDef, bool> predicate;
						if ((predicate = <>9__1) == null)
						{
							predicate = (<>9__1 = ((StyleItemDef x) => this.CanList(x, itemType)));
						}
						num2 = num3 + (num4 + num5 * (float)itemsInCategory.Where(predicate).Count<StyleItemDef>());
					}
				}
			}
			if (Event.current.type == EventType.layout)
			{
				scrollViewHeight = num2;
			}
			Widgets.EndScrollView();
			Widgets.EndGroup();
		}

		// Token: 0x0600BBF2 RID: 48114 RVA: 0x0042FD2C File Offset: 0x0042DF2C
		private void ListStyleItemCategory(StyleItemCategoryDef category, ref float curY, Rect viewRect, Dialog_EditIdeoStyleItems.ExpandedInfo expandedInfo, Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			Rect rect = new Rect(viewRect.x, viewRect.y + curY, viewRect.width, 28f);
			Widgets.DrawHighlightSelected(rect);
			Rect rect2 = new Rect(viewRect.x, curY, 28f, 28f);
			GUI.DrawTexture(rect2.ContractedBy(4f), expandedInfo.expanded ? Dialog_EditIdeoStyleItems.Minus : Dialog_EditIdeoStyleItems.Plus);
			Widgets.DrawHighlightIfMouseover(rect);
			Rect rect3 = new Rect(rect2.xMax + 4f, curY, 110f, 28f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect3, category.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			if (Widgets.ButtonInvisible(new Rect(rect2.x, rect2.y, rect2.width + rect3.width + 4f, 28f), true))
			{
				expandedInfo.expanded = !expandedInfo.expanded;
				if (expandedInfo.expanded)
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
			}
			Gender? gender = null;
			if (itemType != Dialog_EditIdeoStyleItems.ItemType.Beard)
			{
				gender = new Gender?(this.TryGetGender(category, itemType));
			}
			this.DrawConfigInfo(viewRect, curY, delegate(Gender g)
			{
				foreach (StyleItemDef styleItemDef2 in category.ItemsInCategory)
				{
					if (this.CanList(styleItemDef2, itemType))
					{
						this.ChangeGender(styleItemDef2, g);
					}
				}
			}, delegate(StyleItemFrequency f)
			{
				foreach (StyleItemDef styleItemDef2 in category.ItemsInCategory)
				{
					if (this.CanList(styleItemDef2, itemType))
					{
						this.ChangeFrequency(styleItemDef2, f);
					}
				}
			}, gender, this.TryGetFrequency(category, itemType), (StyleItemFrequency f) => category.ItemsInCategory.Any((StyleItemDef x) => this.CanList(x, itemType) && this.GetFrequency(x) == f));
			curY += rect.height;
			if (expandedInfo.expanded)
			{
				int num = 0;
				foreach (StyleItemDef styleItemDef in category.ItemsInCategory)
				{
					if (this.CanList(styleItemDef, itemType))
					{
						this.ListStyleItem(styleItemDef, ref curY, num, viewRect);
						num++;
					}
				}
			}
			curY += 4f;
		}

		// Token: 0x0600BBF3 RID: 48115 RVA: 0x0042FF60 File Offset: 0x0042E160
		private void ListStyleItem(StyleItemDef styleItem, ref float curY, int index, Rect viewRect)
		{
			Rect rect = new Rect(viewRect.x + 17f, viewRect.y + curY, viewRect.width - 17f, 28f);
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(new Rect(rect.x + 28f, rect.y, rect.width - 28f, rect.height));
			}
			if (Mouse.IsOver(rect))
			{
				this.hover = styleItem;
				Rect r = new Rect(UI.MousePositionOnUI.x + 10f, UI.MousePositionOnUIInverted.y, 100f, 100f + Text.LineHeight);
				Find.WindowStack.ImmediateWindow(12918217, r, WindowLayer.Super, delegate
				{
					Rect rect5 = r.AtZero();
					rect5.height -= Text.LineHeight;
					Widgets.DrawHighlight(rect5);
					if (this.hover != null)
					{
						Text.Anchor = TextAnchor.UpperCenter;
						Widgets.LabelFit(new Rect(0f, rect5.yMax, rect5.width, Text.LineHeight), this.hover.LabelCap);
						Text.Anchor = TextAnchor.UpperLeft;
						float scale = 1.1f;
						if (this.hover is HairDef)
						{
							GUI.color = Dialog_EditIdeoStyleItems.HairColor;
							rect5.y += 10f;
						}
						else if (this.hover is BeardDef)
						{
							GUI.color = Dialog_EditIdeoStyleItems.HairColor;
							rect5.y -= 10f;
						}
						else if (this.hover is TattooDef)
						{
							Widgets.DrawRectFast(rect5, Dialog_EditIdeoStyleItems.SemiSelectedColor, null);
							scale = 1.25f;
						}
						Widgets.DefIcon(rect5, this.hover, null, scale, null, false, null, null, null, 1f);
						GUI.color = Color.white;
					}
				}, true, false, 1f, null, false);
			}
			Widgets.DrawHighlightIfMouseover(rect);
			Rect rect2 = new Rect(rect.x, curY, 28f, 28f);
			Rect rect3 = rect2.ContractedBy(2f);
			Widgets.DrawHighlight(rect3);
			if (styleItem is HairDef || styleItem is BeardDef)
			{
				GUI.color = Dialog_EditIdeoStyleItems.HairColor;
			}
			else if (styleItem is TattooDef)
			{
				Widgets.DrawHighlight(rect3);
			}
			Widgets.DefIcon(rect2, styleItem, null, 1.25f, null, false, null, null, null, 1f);
			GUI.color = Color.white;
			Rect rect4 = new Rect(rect2.xMax + 4f, curY, 110f, 28f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect4, styleItem.LabelCap.Truncate(rect4.width, this.labelCache));
			Text.Anchor = TextAnchor.UpperLeft;
			this.DrawConfigInfo(viewRect, curY, delegate(Gender g)
			{
				this.ChangeGender(styleItem, g);
			}, delegate(StyleItemFrequency f)
			{
				this.ChangeFrequency(styleItem, f);
			}, this.GetGender(styleItem), new StyleItemFrequency?(this.GetFrequency(styleItem)), null);
			curY += 28f;
		}

		// Token: 0x0600BBF4 RID: 48116 RVA: 0x004301BC File Offset: 0x0042E3BC
		private void DrawConfigInfo(Rect viewRect, float curY, Action<Gender> changeGenderAction, Action<StyleItemFrequency> changeFrequencyAction, Gender? gender = null, StyleItemFrequency? curFrequency = null, Func<StyleItemFrequency, bool> semiSelectedValidator = null)
		{
			Rect inRect = new Rect(viewRect.x, curY, viewRect.width + 16f, 28f);
			Rect rect = this.FrequencyPosition(inRect, 0);
			if (gender != null)
			{
				Rect rect2 = new Rect(rect.xMin - rect.width / 2f, curY, 28f, 28f);
				if (Mouse.IsOver(rect2))
				{
					Widgets.DrawHighlight(rect2);
					if (gender != null)
					{
						TooltipHandler.TipRegion(rect2, Dialog_EditIdeoStyleItems.<DrawConfigInfo>g__GenderLabel|42_0(gender.Value).CapitalizeFirst());
					}
				}
				GUI.DrawTexture(rect2.ContractedBy(4f), gender.Value.GetIcon());
				if (Widgets.ButtonInvisible(rect2, true))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					using (IEnumerator enumerator = Enum.GetValues(typeof(Gender)).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Gender g = (Gender)enumerator.Current;
							list.Add(new FloatMenuOption(Dialog_EditIdeoStyleItems.<DrawConfigInfo>g__GenderLabel|42_0(g).CapitalizeFirst(), delegate()
							{
								changeGenderAction(g);
							}, g.GetIcon(), Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
						}
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
			}
			for (int i = 0; i < this.allFrequencies.Length; i++)
			{
				StyleItemFrequency styleItemFrequency = this.allFrequencies[i];
				StyleItemFrequency? styleItemFrequency2 = curFrequency;
				StyleItemFrequency styleItemFrequency3 = styleItemFrequency;
				bool flag = styleItemFrequency2.GetValueOrDefault() == styleItemFrequency3 & styleItemFrequency2 != null;
				Rect rect3 = this.FrequencyPosition(inRect, i);
				Widgets.DrawHighlightIfMouseover(rect3);
				Vector2 vector = rect3.center - new Vector2(12f, 12f);
				if (this.editMode != IdeoEditMode.None)
				{
					Widgets.DraggableResult draggableResult = Widgets.ButtonInvisibleDraggable(rect3, false);
					if (draggableResult == Widgets.DraggableResult.Dragged)
					{
						this.painting = true;
					}
					Widgets.RadioButton(vector, flag, false);
					if (!flag && semiSelectedValidator != null && semiSelectedValidator(styleItemFrequency))
					{
						GUI.DrawTexture(new Rect(vector.x, vector.y, 24f, 24f), Dialog_EditIdeoStyleItems.RadioButSemiOn);
					}
					if (!flag && ((this.painting && Mouse.IsOver(rect3)) || draggableResult.AnyPressed()))
					{
						SoundDefOf.Designate_DragStandard_Changed_NoCam.PlayOneShotOnCamera(null);
						changeFrequencyAction(styleItemFrequency);
					}
				}
				else if (flag)
				{
					Widgets.RadioButton(vector, flag, false);
				}
			}
		}

		// Token: 0x0600BBF5 RID: 48117 RVA: 0x00430478 File Offset: 0x0042E678
		private StyleItemFrequency GetFrequency(StyleItemDef def)
		{
			HairDef hairDef = def as HairDef;
			if (hairDef != null)
			{
				return this.hairFrequencies[hairDef].frequency;
			}
			BeardDef beardDef = def as BeardDef;
			if (beardDef != null)
			{
				return this.beardFrequencies[beardDef].frequency;
			}
			TattooDef tattooDef = def as TattooDef;
			if (tattooDef != null)
			{
				return this.tattooFrequencies[tattooDef].frequency;
			}
			return StyleItemFrequency.Never;
		}

		// Token: 0x0600BBF6 RID: 48118 RVA: 0x004304DC File Offset: 0x0042E6DC
		private Gender? GetGender(StyleItemDef def)
		{
			StyleGender styleGender = StyleGender.Any;
			HairDef hairDef = def as HairDef;
			if (hairDef != null)
			{
				styleGender = this.hairFrequencies[hairDef].gender;
			}
			else
			{
				if (def is BeardDef)
				{
					return null;
				}
				TattooDef tattooDef = def as TattooDef;
				if (tattooDef != null)
				{
					styleGender = this.tattooFrequencies[tattooDef].gender;
				}
			}
			switch (styleGender)
			{
			case StyleGender.Male:
			case StyleGender.MaleUsually:
				return new Gender?(Gender.Male);
			case StyleGender.FemaleUsually:
			case StyleGender.Female:
				return new Gender?(Gender.Female);
			}
			return new Gender?(Gender.None);
		}

		// Token: 0x0600BBF7 RID: 48119 RVA: 0x00430568 File Offset: 0x0042E768
		private void ChangeFrequency(StyleItemDef def, StyleItemFrequency freq)
		{
			HairDef hairDef = def as HairDef;
			if (hairDef != null)
			{
				this.hairFrequencies[hairDef].frequency = freq;
				return;
			}
			BeardDef beardDef = def as BeardDef;
			if (beardDef != null)
			{
				this.beardFrequencies[beardDef].frequency = freq;
				return;
			}
			TattooDef tattooDef = def as TattooDef;
			if (tattooDef != null)
			{
				this.tattooFrequencies[tattooDef].frequency = freq;
			}
		}

		// Token: 0x0600BBF8 RID: 48120 RVA: 0x004305CC File Offset: 0x0042E7CC
		private void ChangeGender(StyleItemDef def, StyleGender gender)
		{
			HairDef hairDef = def as HairDef;
			if (hairDef != null)
			{
				this.hairFrequencies[hairDef].gender = gender;
				return;
			}
			BeardDef beardDef = def as BeardDef;
			if (beardDef != null)
			{
				this.beardFrequencies[beardDef].gender = gender;
				return;
			}
			TattooDef tattooDef = def as TattooDef;
			if (tattooDef != null)
			{
				this.tattooFrequencies[tattooDef].gender = gender;
			}
		}

		// Token: 0x0600BBF9 RID: 48121 RVA: 0x00430630 File Offset: 0x0042E830
		private void ChangeGender(StyleItemDef def, Gender gender)
		{
			StyleGender gender2 = StyleGender.Any;
			if (gender == Gender.Male)
			{
				gender2 = StyleGender.Male;
			}
			else if (gender == Gender.Female)
			{
				gender2 = StyleGender.Female;
			}
			this.ChangeGender(def, gender2);
		}

		// Token: 0x0600BBFA RID: 48122 RVA: 0x00430658 File Offset: 0x0042E858
		private bool CanList(StyleItemDef s, Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			switch (itemType)
			{
			case Dialog_EditIdeoStyleItems.ItemType.Hair:
				return s is HairDef;
			case Dialog_EditIdeoStyleItems.ItemType.Beard:
				return s is BeardDef;
			case Dialog_EditIdeoStyleItems.ItemType.FaceTattoo:
			{
				TattooDef tattooDef = s as TattooDef;
				return tattooDef != null && tattooDef.tattooType == TattooType.Face;
			}
			case Dialog_EditIdeoStyleItems.ItemType.BodyTattoo:
			{
				TattooDef tattooDef2 = s as TattooDef;
				return tattooDef2 != null && tattooDef2.tattooType == TattooType.Body;
			}
			default:
				return false;
			}
		}

		// Token: 0x0600BBFB RID: 48123 RVA: 0x004306C0 File Offset: 0x0042E8C0
		private Dialog_EditIdeoStyleItems.ItemType GetItemType(StyleItemDef s)
		{
			if (s is HairDef)
			{
				return Dialog_EditIdeoStyleItems.ItemType.Hair;
			}
			if (s is BeardDef)
			{
				return Dialog_EditIdeoStyleItems.ItemType.Beard;
			}
			TattooDef tattooDef = s as TattooDef;
			if (tattooDef == null)
			{
				return Dialog_EditIdeoStyleItems.ItemType.Hair;
			}
			if (tattooDef.tattooType == TattooType.Body)
			{
				return Dialog_EditIdeoStyleItems.ItemType.BodyTattoo;
			}
			return Dialog_EditIdeoStyleItems.ItemType.FaceTattoo;
		}

		// Token: 0x0600BBFC RID: 48124 RVA: 0x004306FC File Offset: 0x0042E8FC
		private bool CanList(StyleItemCategoryDef category, Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			foreach (StyleItemDef s in category.ItemsInCategory)
			{
				if (this.GetItemType(s) == itemType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600BBFD RID: 48125 RVA: 0x0043075C File Offset: 0x0042E95C
		private string GetSectionLabel(Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			switch (itemType)
			{
			case Dialog_EditIdeoStyleItems.ItemType.Hair:
				return "Hair".Translate();
			case Dialog_EditIdeoStyleItems.ItemType.Beard:
				return "Beard".Translate();
			case Dialog_EditIdeoStyleItems.ItemType.FaceTattoo:
				return "TattooFace".Translate();
			case Dialog_EditIdeoStyleItems.ItemType.BodyTattoo:
				return "TattooBody".Translate();
			default:
				return string.Empty;
			}
		}

		// Token: 0x0600BBFE RID: 48126 RVA: 0x004307C8 File Offset: 0x0042E9C8
		private Gender TryGetGender(StyleItemCategoryDef def, Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			Gender? gender = null;
			foreach (StyleItemDef styleItemDef in def.ItemsInCategory)
			{
				if (this.CanList(styleItemDef, itemType))
				{
					if (gender == null)
					{
						gender = this.GetGender(styleItemDef);
					}
					else
					{
						Gender? gender2 = this.GetGender(styleItemDef);
						Gender? gender3 = gender;
						if (!(gender2.GetValueOrDefault() == gender3.GetValueOrDefault() & gender2 != null == (gender3 != null)))
						{
							return Gender.None;
						}
					}
				}
			}
			return gender.GetValueOrDefault();
		}

		// Token: 0x0600BBFF RID: 48127 RVA: 0x00430878 File Offset: 0x0042EA78
		private StyleItemFrequency? TryGetFrequency(StyleItemCategoryDef def, Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			StyleItemFrequency? styleItemFrequency = null;
			foreach (StyleItemDef styleItemDef in def.ItemsInCategory)
			{
				if (this.CanList(styleItemDef, itemType))
				{
					if (styleItemFrequency == null)
					{
						styleItemFrequency = new StyleItemFrequency?(this.GetFrequency(styleItemDef));
					}
					else
					{
						StyleItemFrequency frequency = this.GetFrequency(styleItemDef);
						StyleItemFrequency? result = styleItemFrequency;
						if (!(frequency == result.GetValueOrDefault() & result != null))
						{
							result = null;
							return result;
						}
					}
				}
			}
			return styleItemFrequency;
		}

		// Token: 0x0600BC00 RID: 48128 RVA: 0x0043091C File Offset: 0x0042EB1C
		private Rect FrequencyPosition(Rect inRect, int index)
		{
			Rect result = inRect;
			result.width = (inRect.width - 178f) / (float)this.allFrequencies.Length - 4f;
			result.x = inRect.x + 178f + result.width * (float)index;
			return result;
		}

		// Token: 0x0600BC01 RID: 48129 RVA: 0x00430970 File Offset: 0x0042EB70
		private void Done()
		{
			if (this.editMode != IdeoEditMode.None)
			{
				foreach (KeyValuePair<HairDef, StyleItemSpawningProperties> keyValuePair in this.hairFrequencies)
				{
					this.ideo.style.SetFrequency(keyValuePair.Key, keyValuePair.Value.frequency);
					this.ideo.style.SetGender(keyValuePair.Key, keyValuePair.Value.gender);
				}
				foreach (KeyValuePair<BeardDef, StyleItemSpawningProperties> keyValuePair2 in this.beardFrequencies)
				{
					this.ideo.style.SetFrequency(keyValuePair2.Key, keyValuePair2.Value.frequency);
					this.ideo.style.SetGender(keyValuePair2.Key, keyValuePair2.Value.gender);
				}
				foreach (KeyValuePair<TattooDef, StyleItemSpawningProperties> keyValuePair3 in this.tattooFrequencies)
				{
					this.ideo.style.SetFrequency(keyValuePair3.Key, keyValuePair3.Value.frequency);
					this.ideo.style.SetGender(keyValuePair3.Key, keyValuePair3.Value.gender);
				}
				this.ideo.style.EnsureAtLeastOneStyleItemAvailable();
			}
			this.Close(true);
		}

		// Token: 0x0600BC05 RID: 48133 RVA: 0x00430BE8 File Offset: 0x0042EDE8
		[CompilerGenerated]
		internal static string <DrawConfigInfo>g__GenderLabel|42_0(Gender g)
		{
			if (g == Gender.None)
			{
				return "MaleAndFemale".Translate();
			}
			return g.GetLabel(false);
		}

		// Token: 0x04006B3E RID: 27454
		private Ideo ideo;

		// Token: 0x04006B3F RID: 27455
		private StyleItemDef hover;

		// Token: 0x04006B40 RID: 27456
		private StyleItemTab curTab;

		// Token: 0x04006B41 RID: 27457
		private DefMap<HairDef, StyleItemSpawningProperties> hairFrequencies;

		// Token: 0x04006B42 RID: 27458
		private DefMap<BeardDef, StyleItemSpawningProperties> beardFrequencies;

		// Token: 0x04006B43 RID: 27459
		private DefMap<TattooDef, StyleItemSpawningProperties> tattooFrequencies;

		// Token: 0x04006B44 RID: 27460
		private List<Dialog_EditIdeoStyleItems.ExpandedInfo> expandedInfos;

		// Token: 0x04006B45 RID: 27461
		private List<TabRecord> tabs = new List<TabRecord>();

		// Token: 0x04006B46 RID: 27462
		private Vector2 scrollPositionLeft;

		// Token: 0x04006B47 RID: 27463
		private Vector2 scrollPositionRight;

		// Token: 0x04006B48 RID: 27464
		private float scrollViewHeightLeft;

		// Token: 0x04006B49 RID: 27465
		private float scrollViewHeightRight;

		// Token: 0x04006B4A RID: 27466
		private bool painting;

		// Token: 0x04006B4B RID: 27467
		private StyleItemFrequency[] allFrequencies;

		// Token: 0x04006B4C RID: 27468
		private IdeoEditMode editMode;

		// Token: 0x04006B4D RID: 27469
		private static readonly Vector2 ButSize = new Vector2(150f, 38f);

		// Token: 0x04006B4E RID: 27470
		private static readonly Color HairColor = PawnHairColors.ReddishBrown;

		// Token: 0x04006B4F RID: 27471
		private static readonly Color SemiSelectedColor = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x04006B50 RID: 27472
		private const float HeaderHeight = 35f;

		// Token: 0x04006B51 RID: 27473
		private const float TabsSpacing = 45f;

		// Token: 0x04006B52 RID: 27474
		private const float ItemHeight = 28f;

		// Token: 0x04006B53 RID: 27475
		private const float ItemLabelWidth = 110f;

		// Token: 0x04006B54 RID: 27476
		private static readonly Texture2D Plus = ContentFinder<Texture2D>.Get("UI/Buttons/Plus", true);

		// Token: 0x04006B55 RID: 27477
		private static readonly Texture2D Minus = ContentFinder<Texture2D>.Get("UI/Buttons/Minus", true);

		// Token: 0x04006B56 RID: 27478
		private static readonly Texture2D RadioButSemiOn = ContentFinder<Texture2D>.Get("UI/Widgets/RadioButSemiOn", true);

		// Token: 0x04006B57 RID: 27479
		private Dictionary<string, string> labelCache = new Dictionary<string, string>();

		// Token: 0x02003945 RID: 14661
		public enum ItemType
		{
			// Token: 0x0400E46C RID: 58476
			Hair,
			// Token: 0x0400E46D RID: 58477
			Beard,
			// Token: 0x0400E46E RID: 58478
			FaceTattoo,
			// Token: 0x0400E46F RID: 58479
			BodyTattoo
		}

		// Token: 0x02003946 RID: 14662
		public class ExpandedInfo
		{
			// Token: 0x06014D50 RID: 85328 RVA: 0x006A5997 File Offset: 0x006A3B97
			public ExpandedInfo(StyleItemCategoryDef categoryDef, Dialog_EditIdeoStyleItems.ItemType itemType, bool any)
			{
				this.categoryDef = categoryDef;
				this.itemType = itemType;
				this.any = any;
				this.expanded = false;
			}

			// Token: 0x0400E470 RID: 58480
			public StyleItemCategoryDef categoryDef;

			// Token: 0x0400E471 RID: 58481
			public Dialog_EditIdeoStyleItems.ItemType itemType;

			// Token: 0x0400E472 RID: 58482
			public bool any;

			// Token: 0x0400E473 RID: 58483
			public bool expanded;
		}
	}
}
