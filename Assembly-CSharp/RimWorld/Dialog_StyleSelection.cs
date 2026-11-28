using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CDB RID: 7387
	public class Dialog_StyleSelection : Window
	{
		// Token: 0x1700228E RID: 8846
		// (get) Token: 0x0600BDD9 RID: 48601 RVA: 0x0044319C File Offset: 0x0044139C
		public override Vector2 InitialSize
		{
			get
			{
				float x = Mathf.Max(Text.CalcSize("StylesInUse".Translate()).x, 84f) + 10f + this.Margin * 2f;
				float y = Text.LineHeightOf(GameFont.Small) + 20f + 24f + this.Margin * 2f;
				return new Vector2(x, y);
			}
		}

		// Token: 0x1700228F RID: 8847
		// (get) Token: 0x0600BDDA RID: 48602 RVA: 0x00443205 File Offset: 0x00441405
		protected override float Margin
		{
			get
			{
				return 4f;
			}
		}

		// Token: 0x0600BDDB RID: 48603 RVA: 0x0044320C File Offset: 0x0044140C
		public Dialog_StyleSelection() : base(null)
		{
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.doWindowBackground = false;
			this.drawShadow = false;
			this.closeOnClickedOutside = true;
		}

		// Token: 0x0600BDDC RID: 48604 RVA: 0x00443244 File Offset: 0x00441444
		public override void DoWindowContents(Rect inRect)
		{
			this.UpdateBaseColor();
			GUI.color = this.baseColor;
			Text.Font = GameFont.Small;
			Widgets.DrawWindowBackground(inRect, this.baseColor);
			GUI.BeginGroup(inRect.ContractedBy(this.Margin));
			Widgets.Label(new Rect(0f, 0f, inRect.width, Text.LineHeight), "StylesInUse".Translate());
			float num = 0f;
			for (int i = 0; i < Find.IdeoManager.selectedStyleCategories.Count; i++)
			{
				StyleCategoryDef cat = Find.IdeoManager.selectedStyleCategories[i];
				Rect rect = new Rect(num, Text.LineHeight + 10f, 24f, 24f);
				if (Widgets.ButtonImage(rect, cat.Icon, this.baseColor, true, null))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (StyleCategoryDef styleCategoryDef in DefDatabase<StyleCategoryDef>.AllDefs)
					{
						StyleCategoryDef category = styleCategoryDef;
						if (category != cat && !Find.IdeoManager.selectedStyleCategories.Contains(styleCategoryDef))
						{
							list.Add(new FloatMenuOption(category.LabelCap, delegate()
							{
								Find.IdeoManager.selectedStyleCategories.Replace(cat, category);
								this.ClearCaches();
							}, category.Icon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
						}
					}
					list.Add(new FloatMenuOption("Remove".Translate().CapitalizeFirst(), delegate()
					{
						Find.IdeoManager.selectedStyleCategories.Remove(cat);
						this.ClearCaches();
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					Find.WindowStack.Add(new FloatMenu(list));
				}
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
					TooltipHandler.TipRegion(rect, IdeoUIUtility.StyleTooltip(cat, IdeoEditMode.GameStart, null, Dialog_StyleSelection.tmpStyles, true));
				}
				num += 28f;
			}
			if (Find.IdeoManager.selectedStyleCategories.Count < 3)
			{
				Rect rect2 = new Rect(num, Text.LineHeight + 10f, 24f, 24f);
				if (Widgets.ButtonImage(rect2, TexButton.Plus, this.baseColor, true, null))
				{
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					foreach (StyleCategoryDef cat2 in DefDatabase<StyleCategoryDef>.AllDefs)
					{
						StyleCategoryDef cat = cat2;
						if (!Find.IdeoManager.selectedStyleCategories.Contains(cat))
						{
							list2.Add(new FloatMenuOption(cat.LabelCap, delegate()
							{
								Find.IdeoManager.selectedStyleCategories.Add(cat);
								this.ClearCaches();
							}, cat.Icon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
						}
					}
					if (list2.Any<FloatMenuOption>())
					{
						Find.WindowStack.Add(new FloatMenu(list2));
					}
				}
				if (Mouse.IsOver(rect2))
				{
					Widgets.DrawHighlight(rect2);
					TooltipHandler.TipRegion(rect2, string.Concat(new string[]
					{
						"AddStyleCategory".Translate().Colorize(ColoredText.TipSectionTitleColor),
						"\n\n",
						"StyleCategoryDescriptionAbstract".Translate().Colorize(ColoredText.SubtleGrayColor),
						"\n\n",
						"ClickToEdit".Translate().CapitalizeFirst().Colorize(ColorLibrary.Green)
					}));
				}
			}
			GUI.EndGroup();
			GUI.color = Color.white;
		}

		// Token: 0x0600BDDD RID: 48605 RVA: 0x00443644 File Offset: 0x00441844
		private void ClearCaches()
		{
			Faction.OfPlayer.ideos.PrimaryIdeo.style.ResetStylesForThingDef();
			Faction.OfPlayer.ideos.PrimaryIdeo.RecachePossibleBuildables();
			foreach (DesignationCategoryDef designationCategoryDef in DefDatabase<DesignationCategoryDef>.AllDefs)
			{
				designationCategoryDef.DirtyCache();
			}
		}

		// Token: 0x0600BDDE RID: 48606 RVA: 0x004436BC File Offset: 0x004418BC
		protected override void SetInitialSizeAndPosition()
		{
			Vector2 initialSize = this.InitialSize;
			Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
			this.windowRect = new Rect(mousePositionOnUIInverted.x, mousePositionOnUIInverted.y - initialSize.y, initialSize.x, initialSize.y).Rounded();
		}

		// Token: 0x0600BDDF RID: 48607 RVA: 0x00443708 File Offset: 0x00441908
		private void UpdateBaseColor()
		{
			if (Find.WindowStack.FloatMenu != null)
			{
				this.baseColor = Color.white;
				return;
			}
			Rect r = new Rect(0f, 0f, this.InitialSize.x, this.InitialSize.y).ExpandedBy(5f);
			if (!r.Contains(Event.current.mousePosition))
			{
				float num = GenUI.DistFromRect(r, Event.current.mousePosition);
				this.baseColor = new Color(1f, 1f, 1f, 1f - num / 145f);
				if (num > 145f)
				{
					this.Close(false);
				}
			}
		}

		// Token: 0x04006CB5 RID: 27829
		private Color baseColor = Color.white;

		// Token: 0x04006CB6 RID: 27830
		private static readonly List<ThingStyleCategoryWithPriority> tmpStyles = new List<ThingStyleCategoryWithPriority>();

		// Token: 0x04006CB7 RID: 27831
		private const float FadeStartMouseDist = 5f;

		// Token: 0x04006CB8 RID: 27832
		private const float FadeFinishMouseDist = 150f;

		// Token: 0x04006CB9 RID: 27833
		private const int MaxButtonCount = 3;
	}
}
