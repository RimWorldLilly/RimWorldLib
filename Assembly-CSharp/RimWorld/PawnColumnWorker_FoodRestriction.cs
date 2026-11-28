using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001DCD RID: 7629
	public class PawnColumnWorker_FoodRestriction : PawnColumnWorker
	{
		// Token: 0x0600C636 RID: 50742 RVA: 0x0048A8E0 File Offset: 0x00488AE0
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
			if (Widgets.ButtonText(new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f), "ManageFoodPolicies".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_ManageFoodPolicies(null));
			}
		}

		// Token: 0x0600C637 RID: 50743 RVA: 0x0048A964 File Offset: 0x00488B64
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.foodRestriction == null)
			{
				return;
			}
			this.DoAssignFoodRestrictionButtons(rect, pawn);
		}

		// Token: 0x0600C638 RID: 50744 RVA: 0x0048A977 File Offset: 0x00488B77
		private IEnumerable<Widgets.DropdownMenuElement<FoodPolicy>> Button_GenerateMenu(Pawn pawn)
		{
			PawnColumnWorker_FoodRestriction.<Button_GenerateMenu>d__4 <Button_GenerateMenu>d__ = new PawnColumnWorker_FoodRestriction.<Button_GenerateMenu>d__4(-2);
			<Button_GenerateMenu>d__.<>3__pawn = pawn;
			return <Button_GenerateMenu>d__;
		}

		// Token: 0x0600C639 RID: 50745 RVA: 0x0048A987 File Offset: 0x00488B87
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		// Token: 0x0600C63A RID: 50746 RVA: 0x0048A99F File Offset: 0x00488B9F
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(251f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x0600C63B RID: 50747 RVA: 0x0048A9BE File Offset: 0x00488BBE
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x0600C63C RID: 50748 RVA: 0x0048A9D0 File Offset: 0x00488BD0
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x0600C63D RID: 50749 RVA: 0x0048A9F3 File Offset: 0x00488BF3
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.foodRestriction != null && pawn.foodRestriction.CurrentFoodPolicy != null)
			{
				return pawn.foodRestriction.CurrentFoodPolicy.id;
			}
			return int.MinValue;
		}

		// Token: 0x0600C63E RID: 50750 RVA: 0x0048AA20 File Offset: 0x00488C20
		private void DoAssignFoodRestrictionButtons(Rect rect, Pawn pawn)
		{
			Rect rect2 = rect.ContractedBy(0f, 2f);
			Widgets.Dropdown<Pawn, FoodPolicy>(rect2, pawn, (Pawn p) => p.foodRestriction.CurrentFoodPolicy, new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<FoodPolicy>>>(this.Button_GenerateMenu), pawn.foodRestriction.CurrentFoodPolicy.label.Truncate(rect2.width, null), null, pawn.foodRestriction.CurrentFoodPolicy.label, null, null, true);
		}

		// Token: 0x040070F4 RID: 28916
		private const int TopAreaHeight = 65;

		// Token: 0x040070F5 RID: 28917
		public const int ManageFoodRestrictionsButtonHeight = 32;
	}
}
