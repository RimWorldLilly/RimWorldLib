using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001DCB RID: 7627
	public class PawnColumnWorker_DrugPolicy : PawnColumnWorker
	{
		// Token: 0x0600C62C RID: 50732 RVA: 0x0048A65C File Offset: 0x0048885C
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f);
			if (Widgets.ButtonText(rect2, "ManageDrugPolicies".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_ManageDrugPolicies(null));
			}
			UIHighlighter.HighlightOpportunity(rect2, "ManageDrugPolicies");
			UIHighlighter.HighlightOpportunity(rect2, "ButtonAssignDrugs");
		}

		// Token: 0x0600C62D RID: 50733 RVA: 0x0048A6F6 File Offset: 0x004888F6
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.drugs == null)
			{
				return;
			}
			DrugPolicyUIUtility.DoAssignDrugPolicyButtons(rect, pawn);
		}

		// Token: 0x0600C62E RID: 50734 RVA: 0x0048A708 File Offset: 0x00488908
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		// Token: 0x0600C62F RID: 50735 RVA: 0x0048A720 File Offset: 0x00488920
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(251f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x0600C630 RID: 50736 RVA: 0x0048A73F File Offset: 0x0048893F
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x0600C631 RID: 50737 RVA: 0x0048A750 File Offset: 0x00488950
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x0600C632 RID: 50738 RVA: 0x0048A773 File Offset: 0x00488973
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.drugs != null && pawn.drugs.CurrentPolicy != null)
			{
				return pawn.drugs.CurrentPolicy.id;
			}
			return int.MinValue;
		}

		// Token: 0x040070F1 RID: 28913
		private const int TopAreaHeight = 65;

		// Token: 0x040070F2 RID: 28914
		public const int ManageDrugPoliciesButtonHeight = 32;
	}
}
