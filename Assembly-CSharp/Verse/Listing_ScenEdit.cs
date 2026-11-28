using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000626 RID: 1574
	public class Listing_ScenEdit : Listing_Standard
	{
		// Token: 0x0600313B RID: 12603 RVA: 0x0013AADF File Offset: 0x00138CDF
		public Listing_ScenEdit(Scenario scen)
		{
			this.scen = scen;
		}

		// Token: 0x0600313C RID: 12604 RVA: 0x0013AAF0 File Offset: 0x00138CF0
		public Rect GetScenPartRect(ScenPart part, float height)
		{
			string label = part.Label;
			Rect rect = base.GetRect(height, 1f);
			Widgets.DrawBoxSolid(rect, new Color(1f, 1f, 1f, 0.08f));
			WidgetRow widgetRow = new WidgetRow(rect.x, rect.y, UIDirection.RightThenDown, 72f, 0f);
			if (part.def.PlayerAddRemovable && widgetRow.ButtonIcon(TexButton.Delete, null, new Color?(GenUI.SubtleMouseoverColor), null, null, true, -1f))
			{
				this.scen.RemovePart(part);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			if (this.scen.CanReorder(part, ReorderDirection.Up) && widgetRow.ButtonIcon(TexButton.ReorderUp, null, null, null, null, true, -1f))
			{
				this.scen.Reorder(part, ReorderDirection.Up);
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
			if (this.scen.CanReorder(part, ReorderDirection.Down) && widgetRow.ButtonIcon(TexButton.ReorderDown, null, null, null, null, true, -1f))
			{
				this.scen.Reorder(part, ReorderDirection.Down);
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
			}
			Text.Anchor = TextAnchor.UpperRight;
			Rect rect2 = rect.LeftPart(0.5f).Rounded();
			rect2.xMax -= 4f;
			Widgets.Label(rect2, label);
			Text.Anchor = TextAnchor.UpperLeft;
			base.Gap(4f);
			return rect.RightPart(0.5f).Rounded();
		}

		// Token: 0x04001F9A RID: 8090
		private Scenario scen;
	}
}
