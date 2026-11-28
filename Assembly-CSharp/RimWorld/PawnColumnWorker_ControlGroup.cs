using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DC2 RID: 7618
	public class PawnColumnWorker_ControlGroup : PawnColumnWorker
	{
		// Token: 0x0600C5F8 RID: 50680 RVA: 0x00489BE4 File Offset: 0x00487DE4
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.IsGestating())
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect, "Gestating".Translate().Colorize(ColoredText.SubtleGrayColor));
				Text.Anchor = TextAnchor.UpperLeft;
				return;
			}
			if (pawn.GetOverseer() != null)
			{
				Widgets.Dropdown<Pawn, MechanitorControlGroup>(rect, pawn, (Pawn p) => p.GetMechControlGroup(), new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<MechanitorControlGroup>>>(this.Button_GenerateMenu), pawn.GetMechControlGroup().Index.ToString(), null, null, null, null, this.def.paintable);
			}
		}

		// Token: 0x0600C5F9 RID: 50681 RVA: 0x00489C7C File Offset: 0x00487E7C
		private IEnumerable<Widgets.DropdownMenuElement<MechanitorControlGroup>> Button_GenerateMenu(Pawn pawn)
		{
			PawnColumnWorker_ControlGroup.<Button_GenerateMenu>d__3 <Button_GenerateMenu>d__ = new PawnColumnWorker_ControlGroup.<Button_GenerateMenu>d__3(-2);
			<Button_GenerateMenu>d__.<>3__pawn = pawn;
			return <Button_GenerateMenu>d__;
		}

		// Token: 0x0600C5FA RID: 50682 RVA: 0x00489C8C File Offset: 0x00487E8C
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 160);
		}

		// Token: 0x0600C5FB RID: 50683 RVA: 0x00489C9F File Offset: 0x00487E9F
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x040070E6 RID: 28902
		private const int Width = 160;

		// Token: 0x040070E7 RID: 28903
		private static List<MechanitorControlGroup> tmpControlGroups = new List<MechanitorControlGroup>();
	}
}
