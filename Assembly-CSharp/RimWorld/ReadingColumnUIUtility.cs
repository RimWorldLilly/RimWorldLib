using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DD4 RID: 7636
	public static class ReadingColumnUIUtility
	{
		// Token: 0x0600C672 RID: 50802 RVA: 0x0048B4B4 File Offset: 0x004896B4
		public static void DoAssignReadingButtons(Rect rect, Pawn pawn)
		{
			Rect rect2 = rect.ContractedBy(0f, 2f);
			string label = pawn.reading.CurrentPolicy.label;
			Widgets.Dropdown<Pawn, ReadingPolicy>(rect2, pawn, (Pawn p) => p.reading.CurrentPolicy, new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<ReadingPolicy>>>(ReadingColumnUIUtility.Button_GenerateMenu), label.Truncate(rect2.width, null), null, pawn.reading.CurrentPolicy.label, null, null, true);
			UIHighlighter.HighlightOpportunity(rect2, "ReadingAssignPolicy");
		}

		// Token: 0x0600C673 RID: 50803 RVA: 0x0048B542 File Offset: 0x00489742
		private static IEnumerable<Widgets.DropdownMenuElement<ReadingPolicy>> Button_GenerateMenu(Pawn pawn)
		{
			ReadingColumnUIUtility.<Button_GenerateMenu>d__2 <Button_GenerateMenu>d__ = new ReadingColumnUIUtility.<Button_GenerateMenu>d__2(-2);
			<Button_GenerateMenu>d__.<>3__pawn = pawn;
			return <Button_GenerateMenu>d__;
		}

		// Token: 0x04007100 RID: 28928
		public const string ReadingAssigningTutorHighlightTag = "ReadingAssignPolicy";
	}
}
