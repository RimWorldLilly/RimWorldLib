using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DC0 RID: 7616
	public class PawnColumnWorker_RemainingSpace : PawnColumnWorker
	{
		// Token: 0x0600C5ED RID: 50669 RVA: 0x00489AE1 File Offset: 0x00487CE1
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		// Token: 0x0600C5EE RID: 50670 RVA: 0x00489AE3 File Offset: 0x00487CE3
		public override int GetMinWidth(PawnTable table)
		{
			return 0;
		}

		// Token: 0x0600C5EF RID: 50671 RVA: 0x00489AE6 File Offset: 0x00487CE6
		public override int GetMaxWidth(PawnTable table)
		{
			return 1000000;
		}

		// Token: 0x0600C5F0 RID: 50672 RVA: 0x00489AED File Offset: 0x00487CED
		public override int GetOptimalWidth(PawnTable table)
		{
			return this.GetMaxWidth(table);
		}

		// Token: 0x0600C5F1 RID: 50673 RVA: 0x00489AF6 File Offset: 0x00487CF6
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}
