using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001D96 RID: 7574
	public class MainTabWindow_Architect : MainTabWindow
	{
		// Token: 0x1700247D RID: 9341
		// (get) Token: 0x0600C478 RID: 50296 RVA: 0x0047E69F File Offset: 0x0047C89F
		public float WinHeight
		{
			get
			{
				return (float)Mathf.CeilToInt((float)this.desPanelsCached.Count / 2f) * 32f + 28f;
			}
		}

		// Token: 0x1700247E RID: 9342
		// (get) Token: 0x0600C479 RID: 50297 RVA: 0x0047E6C5 File Offset: 0x0047C8C5
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(200f, this.WinHeight);
			}
		}

		// Token: 0x1700247F RID: 9343
		// (get) Token: 0x0600C47A RID: 50298 RVA: 0x0047E6D7 File Offset: 0x0047C8D7
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17002480 RID: 9344
		// (get) Token: 0x0600C47B RID: 50299 RVA: 0x0047E6DE File Offset: 0x0047C8DE
		public bool QuickSearchWidgetFocused
		{
			get
			{
				return this.quickSearchWidget.CurrentlyFocused();
			}
		}

		// Token: 0x17002481 RID: 9345
		// (get) Token: 0x0600C47C RID: 50300 RVA: 0x0047E6EB File Offset: 0x0047C8EB
		public float PaneTopY
		{
			get
			{
				return (float)UI.screenHeight - this.WinHeight - 35f;
			}
		}

		// Token: 0x0600C47D RID: 50301 RVA: 0x0047E700 File Offset: 0x0047C900
		public MainTabWindow_Architect()
		{
			this.closeOnAccept = false;
			this.quickSearchWidget.inactiveTextColor = MainTabWindow_Architect.InactiveSearchColor;
			this.CacheDesPanels();
		}

		// Token: 0x0600C47E RID: 50302 RVA: 0x0047E73B File Offset: 0x0047C93B
		public override void PreOpen()
		{
			base.PreOpen();
			this.ResetAndUnfocusQuickSearch();
			this.forceActivatedCommand = null;
			this.didInitialUnfocus = false;
		}

		// Token: 0x0600C47F RID: 50303 RVA: 0x0047E758 File Offset: 0x0047C958
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			ArchitectCategoryTab architectCategoryTab = this.OpenTab();
			if (architectCategoryTab != null && architectCategoryTab.def.showPowerGrid)
			{
				OverlayDrawHandler.DrawPowerGridOverlayThisFrame();
			}
		}

		// Token: 0x0600C480 RID: 50304 RVA: 0x0047E787 File Offset: 0x0047C987
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			ArchitectCategoryTab architectCategoryTab = this.OpenTab();
			if (architectCategoryTab != null)
			{
				architectCategoryTab.DesignationTabOnGUI(this.forceActivatedCommand);
			}
			this.forceActivatedCommand = null;
		}

		// Token: 0x0600C481 RID: 50305 RVA: 0x0047E7B0 File Offset: 0x0047C9B0
		private ArchitectCategoryTab OpenTab()
		{
			if (!this.quickSearchWidget.filter.Active || this.userForcedSelectionDuringSearch)
			{
				return this.selectedDesPanel;
			}
			if (this.selectedDesPanel != null && this.selectedDesPanel.AnySearchMatches)
			{
				return this.selectedDesPanel;
			}
			foreach (ArchitectCategoryTab architectCategoryTab in this.desPanelsCached)
			{
				if (architectCategoryTab.AnySearchMatches)
				{
					return architectCategoryTab;
				}
			}
			return null;
		}

		// Token: 0x0600C482 RID: 50306 RVA: 0x0047E848 File Offset: 0x0047CA48
		private void DoCategoryButton(ArchitectCategoryTab panel, float butWidth, float curXInd, float curYInd, ArchitectCategoryTab openTab, bool enabled)
		{
			Rect rect = new Rect(curXInd * butWidth, curYInd * 32f, butWidth, 32f);
			float height = rect.height;
			rect.height = height + 1f;
			if (curXInd == 0f)
			{
				rect.width += 1f;
			}
			Color? labelColor = panel.AnySearchMatches ? null : new Color?(MainTabWindow_Architect.NoMatchColor);
			string label = panel.def.LabelCap;
			if (!enabled)
			{
				if (labelColor == null)
				{
					labelColor = new Color?(MainTabWindow_Architect.InactiveColor);
				}
				GUI.color = MainTabWindow_Architect.InactiveColor;
			}
			if (Widgets.ButtonTextSubtle(rect, label, 0f, 8f, SoundDefOf.Mouseover_Category, new Vector2(-1f, -1f), labelColor, this.quickSearchWidget.filter.Active && openTab == panel))
			{
				this.ClickedCategory(panel);
			}
			GUI.color = Color.white;
			if (this.selectedDesPanel != panel)
			{
				UIHighlighter.HighlightOpportunity(rect, panel.def.cachedHighlightClosedTag);
			}
		}

		// Token: 0x0600C483 RID: 50307 RVA: 0x0047E95D File Offset: 0x0047CB5D
		private void DoCategoryRow(ArchitectCategoryTab left, ArchitectCategoryTab right, float butWidth, float curYInd, ArchitectCategoryTab openTab)
		{
			if (left != null)
			{
				this.DoCategoryButton(left, butWidth, 0f, curYInd, openTab, left.Visible);
			}
			if (right != null)
			{
				this.DoCategoryButton(right, butWidth, 1f, curYInd, openTab, right.Visible);
			}
		}

		// Token: 0x0600C484 RID: 50308 RVA: 0x0047E994 File Offset: 0x0047CB94
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			float butWidth = inRect.width / 2f;
			float num = 0f;
			bool flag = ModsConfig.IdeologyActive && Find.IdeoManager.classicMode;
			ArchitectCategoryTab architectCategoryTab = this.OpenTab();
			if (KeyBindingDefOf.Accept.KeyDownEvent)
			{
				if (this.quickSearchWidget.filter.Active && ((architectCategoryTab != null) ? architectCategoryTab.UniqueSearchMatch : null) != null)
				{
					this.forceActivatedCommand = architectCategoryTab.UniqueSearchMatch;
					Event.current.Use();
				}
				else if (!SteamDeck.IsSteamDeck)
				{
					this.Close(true);
					Event.current.Use();
				}
			}
			int i = 0;
			while (i < this.desPanelsCached.Count)
			{
				ArchitectCategoryTab architectCategoryTab2 = this.desPanelsCached[i];
				ArchitectCategoryTab architectCategoryTab3 = (i + 1 < this.desPanelsCached.Count) ? this.desPanelsCached[i + 1] : null;
				if (architectCategoryTab2.PreferredColumn.GetValueOrDefault() == 1)
				{
					goto IL_106;
				}
				if (architectCategoryTab3 == null)
				{
					goto IL_155;
				}
				int? preferredColumn = architectCategoryTab3.PreferredColumn;
				int num2 = 0;
				if (preferredColumn.GetValueOrDefault() == num2 & preferredColumn != null)
				{
					goto IL_106;
				}
				goto IL_155;
				IL_162:
				num += 1f;
				i += 2;
				continue;
				IL_106:
				preferredColumn = architectCategoryTab2.PreferredColumn;
				num2 = 0;
				if (!(preferredColumn.GetValueOrDefault() == num2 & preferredColumn != null) && (architectCategoryTab3 == null || architectCategoryTab3.PreferredColumn.GetValueOrDefault() != 1))
				{
					this.DoCategoryRow(architectCategoryTab3, architectCategoryTab2, butWidth, num, architectCategoryTab);
					goto IL_162;
				}
				IL_155:
				this.DoCategoryRow(architectCategoryTab2, architectCategoryTab3, butWidth, num, architectCategoryTab);
				goto IL_162;
			}
			float num3 = inRect.width;
			if (flag)
			{
				num3 -= 32f;
			}
			Rect rect = new Rect(0f, num * 32f + 1f, num3, 24f);
			this.quickSearchWidget.OnGUI(rect, new Action(this.CacheSearchState), null);
			if (!this.didInitialUnfocus)
			{
				UI.UnfocusCurrentControl();
				this.didInitialUnfocus = true;
			}
			if (flag && Widgets.ButtonImage(new Rect(rect.xMax + 4f, rect.y, 24f, 24f).ContractedBy(2f), MainTabWindow_Architect.ChangeStyleIcon.Texture, true, null))
			{
				if (Find.WindowStack.IsOpen<Dialog_StyleSelection>())
				{
					Find.WindowStack.TryRemove(typeof(Dialog_StyleSelection), true);
					return;
				}
				Find.WindowStack.Add(new Dialog_StyleSelection());
			}
		}

		// Token: 0x0600C485 RID: 50309 RVA: 0x0047EBFC File Offset: 0x0047CDFC
		private void CacheDesPanels()
		{
			this.desPanelsCached.Clear();
			foreach (DesignationCategoryDef def in from dc in DefDatabase<DesignationCategoryDef>.AllDefs
			orderby dc.order descending
			select dc)
			{
				this.desPanelsCached.Add(new ArchitectCategoryTab(def, this.quickSearchWidget.filter));
			}
		}

		// Token: 0x0600C486 RID: 50310 RVA: 0x0047EC8C File Offset: 0x0047CE8C
		private void CacheSearchState()
		{
			bool flag = false;
			foreach (ArchitectCategoryTab architectCategoryTab in this.desPanelsCached)
			{
				architectCategoryTab.CacheSearchState();
				flag |= architectCategoryTab.AnySearchMatches;
			}
			this.quickSearchWidget.noResultsMatched = !flag;
			if (!this.quickSearchWidget.filter.Active)
			{
				this.userForcedSelectionDuringSearch = false;
			}
		}

		// Token: 0x0600C487 RID: 50311 RVA: 0x0047ED14 File Offset: 0x0047CF14
		protected void ClickedCategory(ArchitectCategoryTab Pan)
		{
			if (!Pan.Visible)
			{
				Messages.Message("NothingAvailableInCategory".Translate() + ": " + Pan.def.LabelCap, MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (this.selectedDesPanel == Pan && !this.quickSearchWidget.filter.Active)
			{
				this.selectedDesPanel = null;
			}
			else
			{
				this.selectedDesPanel = Pan;
			}
			if (this.quickSearchWidget.filter.Active)
			{
				this.userForcedSelectionDuringSearch = true;
			}
			SoundDefOf.ArchitectCategorySelect.PlayOneShotOnCamera(null);
		}

		// Token: 0x0600C488 RID: 50312 RVA: 0x0047EDAD File Offset: 0x0047CFAD
		public override void Notify_ClickOutsideWindow()
		{
			base.Notify_ClickOutsideWindow();
			this.quickSearchWidget.Unfocus();
		}

		// Token: 0x0600C489 RID: 50313 RVA: 0x0047EDC0 File Offset: 0x0047CFC0
		public override void OnCancelKeyPressed()
		{
			if (this.quickSearchWidget.CurrentlyFocused())
			{
				this.ResetAndUnfocusQuickSearch();
				return;
			}
			base.OnCancelKeyPressed();
		}

		// Token: 0x0600C48A RID: 50314 RVA: 0x0047EDDC File Offset: 0x0047CFDC
		private void ResetAndUnfocusQuickSearch()
		{
			this.quickSearchWidget.Reset();
			this.quickSearchWidget.Unfocus();
			this.CacheSearchState();
		}

		// Token: 0x04007034 RID: 28724
		private List<ArchitectCategoryTab> desPanelsCached = new List<ArchitectCategoryTab>();

		// Token: 0x04007035 RID: 28725
		public ArchitectCategoryTab selectedDesPanel;

		// Token: 0x04007036 RID: 28726
		private QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x04007037 RID: 28727
		private bool userForcedSelectionDuringSearch;

		// Token: 0x04007038 RID: 28728
		private Designator forceActivatedCommand;

		// Token: 0x04007039 RID: 28729
		private bool didInitialUnfocus;

		// Token: 0x0400703A RID: 28730
		public const float WinWidth = 200f;

		// Token: 0x0400703B RID: 28731
		private const float ButHeight = 32f;

		// Token: 0x0400703C RID: 28732
		private static readonly Color InactiveSearchColor = Color.white.ToTransparent(0.4f);

		// Token: 0x0400703D RID: 28733
		private static readonly Color NoMatchColor = Color.grey;

		// Token: 0x0400703E RID: 28734
		public const int ColumnCount = 2;

		// Token: 0x0400703F RID: 28735
		private static readonly Color InactiveColor = new Color(1f, 1f, 1f, 0.1f);

		// Token: 0x04007040 RID: 28736
		private static readonly CachedTexture ChangeStyleIcon = new CachedTexture("UI/Icons/ChangeSelectedStyles");
	}
}
