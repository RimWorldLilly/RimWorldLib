using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DCA RID: 7626
	public class PawnColumnWorker_Carry : PawnColumnWorker
	{
		// Token: 0x0600C623 RID: 50723 RVA: 0x0048A47C File Offset: 0x0048867C
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.inventoryStock == null)
			{
				return;
			}
			float num = rect.width - 4f;
			int num2 = Mathf.FloorToInt(num * 0.33333334f);
			float x = rect.x;
			InventoryStockGroupDef group = InventoryStockGroupDefOf.Medicine;
			Widgets.Dropdown<Pawn, ThingDef>(new Rect(x, rect.y + 2f, (float)num2, rect.height - 4f), pawn, (Pawn p) => p.inventoryStock.GetDesiredThingForGroup(group), (Pawn p) => this.GenerateThingButtons(p, group), null, pawn.inventoryStock.GetDesiredThingForGroup(group).uiIcon, null, null, null, true);
			float x2 = x + (float)num2 + 4f;
			int num3 = Mathf.FloorToInt(num * 0.6666667f);
			Widgets.Dropdown<Pawn, int>(new Rect(x2, rect.y + 2f, (float)num3, rect.height - 4f), pawn, (Pawn p) => p.inventoryStock.GetDesiredCountForGroup(group), (Pawn p) => this.GenerateCountButtons(p, group), pawn.inventoryStock.GetDesiredCountForGroup(group).ToString(), null, null, null, null, true);
		}

		// Token: 0x0600C624 RID: 50724 RVA: 0x0048A59B File Offset: 0x0048879B
		private IEnumerable<Widgets.DropdownMenuElement<ThingDef>> GenerateThingButtons(Pawn pawn, InventoryStockGroupDef group)
		{
			PawnColumnWorker_Carry.<GenerateThingButtons>d__2 <GenerateThingButtons>d__ = new PawnColumnWorker_Carry.<GenerateThingButtons>d__2(-2);
			<GenerateThingButtons>d__.<>3__pawn = pawn;
			<GenerateThingButtons>d__.<>3__group = group;
			return <GenerateThingButtons>d__;
		}

		// Token: 0x0600C625 RID: 50725 RVA: 0x0048A5B2 File Offset: 0x004887B2
		private IEnumerable<Widgets.DropdownMenuElement<int>> GenerateCountButtons(Pawn pawn, InventoryStockGroupDef group)
		{
			PawnColumnWorker_Carry.<GenerateCountButtons>d__3 <GenerateCountButtons>d__ = new PawnColumnWorker_Carry.<GenerateCountButtons>d__3(-2);
			<GenerateCountButtons>d__.<>3__pawn = pawn;
			<GenerateCountButtons>d__.<>3__group = group;
			return <GenerateCountButtons>d__;
		}

		// Token: 0x0600C626 RID: 50726 RVA: 0x0048A5C9 File Offset: 0x004887C9
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(54f));
		}

		// Token: 0x0600C627 RID: 50727 RVA: 0x0048A5E1 File Offset: 0x004887E1
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(104f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x0600C628 RID: 50728 RVA: 0x0048A600 File Offset: 0x00488800
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x0600C629 RID: 50729 RVA: 0x0048A610 File Offset: 0x00488810
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x0600C62A RID: 50730 RVA: 0x0048A633 File Offset: 0x00488833
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.inventoryStock != null)
			{
				return pawn.inventoryStock.GetDesiredCountForGroup(InventoryStockGroupDefOf.Medicine);
			}
			return int.MinValue;
		}

		// Token: 0x040070F0 RID: 28912
		private const int TopAreaHeight = 65;
	}
}
