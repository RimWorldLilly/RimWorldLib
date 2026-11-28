using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DCF RID: 7631
	public class PawnColumnWorker_Label : PawnColumnWorker
	{
		// Token: 0x170024BC RID: 9404
		// (get) Token: 0x0600C647 RID: 50759 RVA: 0x0048AB46 File Offset: 0x00488D46
		protected virtual TextAnchor LabelAlignment
		{
			get
			{
				return TextAnchor.MiddleLeft;
			}
		}

		// Token: 0x170024BD RID: 9405
		// (get) Token: 0x0600C648 RID: 50760 RVA: 0x0048AB49 File Offset: 0x00488D49
		protected override TextAnchor DefaultHeaderAlignment
		{
			get
			{
				return TextAnchor.LowerLeft;
			}
		}

		// Token: 0x0600C649 RID: 50761 RVA: 0x0048AB4C File Offset: 0x00488D4C
		protected override float GetHeaderOffsetX(Rect rect)
		{
			return 33f;
		}

		// Token: 0x0600C64A RID: 50762 RVA: 0x0048AB54 File Offset: 0x00488D54
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width, Mathf.Min(rect.height, this.def.groupable ? rect.height : ((float)this.GetMinCellHeight(pawn))));
			Rect rect3 = rect2;
			rect3.xMin += 3f;
			if (this.def.showIcon)
			{
				rect3.xMin += rect2.height;
				Rect rect4 = new Rect(rect2.x, rect2.y, rect2.height, rect2.height);
				if (Find.Selector.IsSelected(pawn))
				{
					SelectionDrawerUtility.DrawSelectionOverlayWholeGUI(rect4.ContractedBy(2f));
				}
				Widgets.ThingIcon(rect4, pawn, 1f, null, false, 1f, false);
			}
			if (pawn.health.summaryHealth.SummaryHealthPercent < 0.99f)
			{
				Rect rect5 = new Rect(rect3.x - 3f, rect3.y, rect3.width + 3f, rect3.height);
				rect5.yMin += 4f;
				rect5.yMax -= 6f;
				Widgets.FillableBar(rect5, pawn.health.summaryHealth.SummaryHealthPercent, GenMapUI.OverlayHealthTex, BaseContent.ClearTex, false);
			}
			if (Mouse.IsOver(rect2))
			{
				GUI.DrawTexture(rect2, TexUI.HighlightTex);
			}
			TaggedString taggedString = this.GetLabel(pawn);
			if (rect3.width != PawnColumnWorker_Label.labelCacheForWidth)
			{
				PawnColumnWorker_Label.labelCacheForWidth = rect3.width;
				PawnColumnWorker_Label.labelCache.Clear();
			}
			if (Text.CalcSize(taggedString).x > rect3.width)
			{
				taggedString = taggedString.Truncate(rect3.width, PawnColumnWorker_Label.labelCache);
			}
			if (pawn.IsSlave || pawn.IsColonyMech)
			{
				taggedString = taggedString.Colorize(PawnNameColorUtility.PawnNameColorOf(pawn));
			}
			Text.Font = GameFont.Small;
			Text.Anchor = this.LabelAlignment;
			Text.WordWrap = false;
			Widgets.Label(rect3, taggedString);
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
			if (Widgets.ButtonInvisible(rect2, true))
			{
				CameraJumper.TryJumpAndSelect(pawn, CameraJumper.MovementMode.Pan);
				if (Current.ProgramState == ProgramState.Playing && Event.current.button == 0)
				{
					Find.MainTabsRoot.EscapeCurrentTab(false);
				}
				return;
			}
			if (Mouse.IsOver(rect2))
			{
				TipSignal tooltip = pawn.GetTooltip();
				tooltip.text = "ClickToJumpTo".Translate() + "\n\n" + tooltip.text;
				TooltipHandler.TipRegion(rect2, tooltip);
			}
		}

		// Token: 0x0600C64B RID: 50763 RVA: 0x0048ADFC File Offset: 0x00488FFC
		private TaggedString GetLabel(Pawn pawn)
		{
			TaggedString result;
			if (!pawn.RaceProps.Humanlike && !pawn.RaceProps.Animal && pawn.Name != null && !pawn.Name.Numerical)
			{
				result = pawn.Name.ToStringShort.CapitalizeFirst() + ", " + pawn.KindLabel.Colorize(ColoredText.SubtleGrayColor);
			}
			else if (this.def.useLabelShort)
			{
				result = pawn.LabelShortCap;
			}
			else
			{
				result = pawn.LabelNoCount.CapitalizeFirst();
			}
			return result;
		}

		// Token: 0x0600C64C RID: 50764 RVA: 0x0048AE97 File Offset: 0x00489097
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 80);
		}

		// Token: 0x0600C64D RID: 50765 RVA: 0x0048AEA7 File Offset: 0x004890A7
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(165, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x0600C64E RID: 50766 RVA: 0x0048AEC1 File Offset: 0x004890C1
		public override int Compare(Pawn a, Pawn b)
		{
			return string.Compare(this.GetValueToCompare(a), this.GetValueToCompare(b), StringComparison.Ordinal);
		}

		// Token: 0x0600C64F RID: 50767 RVA: 0x0048AED7 File Offset: 0x004890D7
		private string GetValueToCompare(Pawn pawn)
		{
			return this.GetLabel(pawn);
		}

		// Token: 0x040070F8 RID: 28920
		private const int LeftMargin = 3;

		// Token: 0x040070F9 RID: 28921
		private const float PortraitCameraZoom = 1.2f;

		// Token: 0x040070FA RID: 28922
		private static Dictionary<string, TaggedString> labelCache = new Dictionary<string, TaggedString>();

		// Token: 0x040070FB RID: 28923
		private static float labelCacheForWidth = -1f;
	}
}
