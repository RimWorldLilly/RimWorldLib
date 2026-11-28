using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DC6 RID: 7622
	public class PawnColumnWorker_WorkMode : PawnColumnWorker
	{
		// Token: 0x0600C60C RID: 50700 RVA: 0x00489F44 File Offset: 0x00488144
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			MechanitorControlGroup mechControlGroup = pawn.GetMechControlGroup();
			if (mechControlGroup == null)
			{
				return;
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Rect rect2 = rect;
			rect2.xMin += 3f;
			Widgets.Label(rect2, mechControlGroup.WorkMode.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect))
			{
				AcceptanceReport canControlMechs = mechControlGroup.Tracker.CanControlMechs;
				TipSignal tooltip = pawn.GetTooltip();
				tooltip.text = "ClickToChangeWorkMode".Translate();
				if (!canControlMechs && !canControlMechs.Reason.NullOrEmpty())
				{
					tooltip.text = tooltip.text + "\n\n" + ("DisabledCommand".Translate() + ": " + canControlMechs.Reason).Colorize(ColorLibrary.RedReadable);
				}
				TooltipHandler.TipRegion(rect, tooltip);
				if (canControlMechs && Widgets.ButtonInvisible(rect, true))
				{
					PawnColumnWorker_WorkMode.tmpFloatMenuOptions.Clear();
					PawnColumnWorker_WorkMode.tmpFloatMenuOptions.AddRange(MechanitorControlGroupGizmo.GetWorkModeOptions(mechControlGroup));
					Find.WindowStack.Add(new FloatMenu(PawnColumnWorker_WorkMode.tmpFloatMenuOptions));
					PawnColumnWorker_WorkMode.tmpFloatMenuOptions.Clear();
				}
				Widgets.DrawHighlight(rect);
			}
		}

		// Token: 0x0600C60D RID: 50701 RVA: 0x0048A074 File Offset: 0x00488274
		public override bool CanGroupWith(Pawn pawn, Pawn other)
		{
			MechanitorControlGroup mechControlGroup = pawn.GetMechControlGroup();
			return mechControlGroup != null && other.GetMechControlGroup() == mechControlGroup;
		}

		// Token: 0x0600C60E RID: 50702 RVA: 0x0048A096 File Offset: 0x00488296
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 160);
		}

		// Token: 0x0600C60F RID: 50703 RVA: 0x0048A0A9 File Offset: 0x004882A9
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x040070EB RID: 28907
		private const int Width = 160;

		// Token: 0x040070EC RID: 28908
		private const int LeftMargin = 3;

		// Token: 0x040070ED RID: 28909
		private static List<FloatMenuOption> tmpFloatMenuOptions = new List<FloatMenuOption>();
	}
}
