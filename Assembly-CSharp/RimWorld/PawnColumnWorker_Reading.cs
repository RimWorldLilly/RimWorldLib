using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001DD3 RID: 7635
	public class PawnColumnWorker_Reading : PawnColumnWorker
	{
		// Token: 0x0600C66A RID: 50794 RVA: 0x0048B368 File Offset: 0x00489568
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f);
			if (Widgets.ButtonText(rect2, "ManageReadingPolicies".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_ManageReadingPolicies(null));
			}
			UIHighlighter.HighlightOpportunity(rect2, "ManageReadingAssignments");
			UIHighlighter.HighlightOpportunity(rect2, "ReadingAssignPolicy");
		}

		// Token: 0x0600C66B RID: 50795 RVA: 0x0048B402 File Offset: 0x00489602
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.reading == null)
			{
				return;
			}
			ReadingColumnUIUtility.DoAssignReadingButtons(rect, pawn);
		}

		// Token: 0x0600C66C RID: 50796 RVA: 0x0048B414 File Offset: 0x00489614
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		// Token: 0x0600C66D RID: 50797 RVA: 0x0048B42C File Offset: 0x0048962C
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(251f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x0600C66E RID: 50798 RVA: 0x0048B44B File Offset: 0x0048964B
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x0600C66F RID: 50799 RVA: 0x0048B45C File Offset: 0x0048965C
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x0600C670 RID: 50800 RVA: 0x0048B47F File Offset: 0x0048967F
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.reading != null && pawn.reading.CurrentPolicy != null)
			{
				return pawn.reading.CurrentPolicy.id;
			}
			return int.MinValue;
		}

		// Token: 0x040070FE RID: 28926
		private const int TopAreaHeight = 65;

		// Token: 0x040070FF RID: 28927
		private const int ManageReadingButtonHeight = 32;
	}
}
