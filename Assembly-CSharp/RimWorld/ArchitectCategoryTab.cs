using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D5A RID: 7514
	public class ArchitectCategoryTab
	{
		// Token: 0x170023F1 RID: 9201
		// (get) Token: 0x0600C286 RID: 49798 RVA: 0x0046EB26 File Offset: 0x0046CD26
		public bool AnySearchMatches
		{
			get
			{
				return this.anySearchMatches;
			}
		}

		// Token: 0x170023F2 RID: 9202
		// (get) Token: 0x0600C287 RID: 49799 RVA: 0x0046EB2E File Offset: 0x0046CD2E
		public Designator UniqueSearchMatch
		{
			get
			{
				return this.uniqueSearchMatch;
			}
		}

		// Token: 0x170023F3 RID: 9203
		// (get) Token: 0x0600C288 RID: 49800 RVA: 0x0046EB36 File Offset: 0x0046CD36
		public bool Visible
		{
			get
			{
				return this.def.Visible;
			}
		}

		// Token: 0x170023F4 RID: 9204
		// (get) Token: 0x0600C289 RID: 49801 RVA: 0x0046EB43 File Offset: 0x0046CD43
		public int? PreferredColumn
		{
			get
			{
				return this.def.preferredColumn;
			}
		}

		// Token: 0x170023F5 RID: 9205
		// (get) Token: 0x0600C28A RID: 49802 RVA: 0x0046EB50 File Offset: 0x0046CD50
		public static Rect InfoRect
		{
			get
			{
				return new Rect(0f, (float)(UI.screenHeight - 35) - ((MainTabWindow_Architect)MainButtonDefOf.Architect.TabWindow).WinHeight - 270f, 200f, 270f);
			}
		}

		// Token: 0x0600C28B RID: 49803 RVA: 0x0046EB8A File Offset: 0x0046CD8A
		public ArchitectCategoryTab(DesignationCategoryDef def, QuickSearchFilter quickSearchFilter)
		{
			this.def = def;
			this.quickSearchFilter = quickSearchFilter;
			this.shouldLowLightGizmoFunc = new Func<Gizmo, bool>(this.ShouldLowLightGizmo);
			this.shouldHighLightGizmoFunc = new Func<Gizmo, bool>(this.ShouldHighLightGizmo);
		}

		// Token: 0x0600C28C RID: 49804 RVA: 0x0046EBC4 File Offset: 0x0046CDC4
		public void DesignationTabOnGUI(Designator forceActivatedCommand)
		{
			if (Find.DesignatorManager.SelectedDesignator != null)
			{
				Find.DesignatorManager.SelectedDesignator.DoExtraGuiControls(0f, (float)(UI.screenHeight - 35) - ((MainTabWindow_Architect)MainButtonDefOf.Architect.TabWindow).WinHeight - 270f);
			}
			Func<Gizmo, bool> customActivatorFunc = (forceActivatedCommand == null) ? null : new Func<Gizmo, bool>((Gizmo cmd) => cmd == forceActivatedCommand);
			float startX = 210f;
			Gizmo selectedDesignator;
			GizmoGridDrawer.DrawGizmoGrid(this.def.ResolvedAllowedDesignators, startX, out selectedDesignator, customActivatorFunc, this.shouldHighLightGizmoFunc, this.shouldLowLightGizmoFunc, false);
			if (selectedDesignator == null && Find.DesignatorManager.SelectedDesignator != null)
			{
				selectedDesignator = Find.DesignatorManager.SelectedDesignator;
			}
			this.DoInfoBox(ArchitectCategoryTab.InfoRect, (Designator)selectedDesignator);
		}

		// Token: 0x0600C28D RID: 49805 RVA: 0x0046EC8C File Offset: 0x0046CE8C
		private bool ShouldLowLightGizmo(Gizmo gizmo)
		{
			Command command = gizmo as Command;
			return command != null && (this.quickSearchFilter.Active && !this.Matches(command));
		}

		// Token: 0x0600C28E RID: 49806 RVA: 0x0046ECC0 File Offset: 0x0046CEC0
		private bool ShouldHighLightGizmo(Gizmo gizmo)
		{
			Command command = gizmo as Command;
			return command != null && (this.quickSearchFilter.Active && this.Matches(command));
		}

		// Token: 0x0600C28F RID: 49807 RVA: 0x0046ECF2 File Offset: 0x0046CEF2
		private bool Matches(Command c)
		{
			return this.quickSearchFilter.Matches(c.Label);
		}

		// Token: 0x0600C290 RID: 49808 RVA: 0x0046ED08 File Offset: 0x0046CF08
		protected void DoInfoBox(Rect infoRect, Designator designator)
		{
			Find.WindowStack.ImmediateWindow(32520, infoRect, WindowLayer.GameUI, delegate
			{
				if (designator != null)
				{
					Rect rect = infoRect.AtZero().ContractedBy(7f);
					Widgets.BeginGroup(rect);
					Rect rect2 = new Rect(0f, 0f, rect.width - designator.PanelReadoutTitleExtraRightMargin, 999f);
					Text.Font = GameFont.Small;
					Widgets.Label(rect2, designator.LabelCap);
					float num = Mathf.Max(24f, Text.CalcHeight(designator.LabelCap, rect2.width));
					designator.DrawPanelReadout(ref num, rect.width);
					Rect rect3 = new Rect(0f, num, rect.width, rect.height - num);
					string text = designator.Desc;
					GenText.SetTextSizeToFit(text, rect3);
					text = text.TruncateHeight(rect3.width, rect3.height, null);
					Widgets.Label(rect3, text);
					Widgets.EndGroup();
				}
			}, true, false, 1f, null, false);
		}

		// Token: 0x0600C291 RID: 49809 RVA: 0x0046ED54 File Offset: 0x0046CF54
		public void CacheSearchState()
		{
			this.anySearchMatches = true;
			this.uniqueSearchMatch = null;
			if (!this.quickSearchFilter.Active)
			{
				return;
			}
			int num = 0;
			Designator designator = null;
			foreach (Designator designator2 in this.def.ResolvedAllowedDesignators)
			{
				if (this.Matches(designator2))
				{
					num++;
					designator = designator2;
					if (num > 1)
					{
						return;
					}
				}
			}
			if (num == 0)
			{
				this.anySearchMatches = false;
				return;
			}
			if (num == 1)
			{
				this.uniqueSearchMatch = designator;
			}
		}

		// Token: 0x04006F3F RID: 28479
		public readonly DesignationCategoryDef def;

		// Token: 0x04006F40 RID: 28480
		private readonly QuickSearchFilter quickSearchFilter;

		// Token: 0x04006F41 RID: 28481
		private bool anySearchMatches;

		// Token: 0x04006F42 RID: 28482
		private Designator uniqueSearchMatch;

		// Token: 0x04006F43 RID: 28483
		private readonly Func<Gizmo, bool> shouldLowLightGizmoFunc;

		// Token: 0x04006F44 RID: 28484
		private readonly Func<Gizmo, bool> shouldHighLightGizmoFunc;

		// Token: 0x04006F45 RID: 28485
		public const float InfoRectHeight = 270f;
	}
}
