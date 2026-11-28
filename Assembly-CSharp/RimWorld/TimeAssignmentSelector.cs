using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001DF0 RID: 7664
	public static class TimeAssignmentSelector
	{
		// Token: 0x0600C74F RID: 51023 RVA: 0x00492340 File Offset: 0x00490540
		public static void DrawTimeAssignmentSelectorGrid(Rect rect)
		{
			rect.yMax -= 2f;
			Rect rect2 = rect;
			rect2.xMax = rect2.center.x;
			rect2.yMax = rect2.center.y;
			TimeAssignmentSelector.DrawTimeAssignmentSelectorFor(rect2, TimeAssignmentDefOf.Anything);
			rect2.x += rect2.width;
			TimeAssignmentSelector.DrawTimeAssignmentSelectorFor(rect2, TimeAssignmentDefOf.Work);
			rect2.x += rect2.width;
			TimeAssignmentSelector.DrawTimeAssignmentSelectorFor(rect2, TimeAssignmentDefOf.Joy);
			rect2.x += rect2.width;
			TimeAssignmentSelector.DrawTimeAssignmentSelectorFor(rect2, TimeAssignmentDefOf.Sleep);
			if (ModsConfig.RoyaltyActive)
			{
				rect2.x += rect2.width;
				TimeAssignmentSelector.DrawTimeAssignmentSelectorFor(rect2, TimeAssignmentDefOf.Meditate);
			}
		}

		// Token: 0x0600C750 RID: 51024 RVA: 0x0049241C File Offset: 0x0049061C
		private static void DrawTimeAssignmentSelectorFor(Rect rect, TimeAssignmentDef ta)
		{
			rect = rect.ContractedBy(2f);
			GUI.DrawTexture(rect, ta.ColorTexture);
			if (Widgets.ButtonInvisible(rect, true))
			{
				TimeAssignmentSelector.selectedAssignment = ta;
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			using (new TextBlock(TextAnchor.MiddleCenter))
			{
				Widgets.Label(rect, ta.LabelCap);
			}
			if (TimeAssignmentSelector.selectedAssignment == ta)
			{
				Widgets.DrawBox(rect, 2, null);
				return;
			}
			UIHighlighter.HighlightOpportunity(rect, ta.cachedHighlightNotSelectedTag);
		}

		// Token: 0x04007185 RID: 29061
		public static TimeAssignmentDef selectedAssignment = TimeAssignmentDefOf.Work;
	}
}
