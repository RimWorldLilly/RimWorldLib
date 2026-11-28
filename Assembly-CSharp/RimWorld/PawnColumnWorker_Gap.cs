using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DBF RID: 7615
	public class PawnColumnWorker_Gap : PawnColumnWorker
	{
		// Token: 0x170024B9 RID: 9401
		// (get) Token: 0x0600C5E7 RID: 50663 RVA: 0x00489A9F File Offset: 0x00487C9F
		protected virtual int Width
		{
			get
			{
				return this.def.gap;
			}
		}

		// Token: 0x0600C5E8 RID: 50664 RVA: 0x00489AAC File Offset: 0x00487CAC
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		// Token: 0x0600C5E9 RID: 50665 RVA: 0x00489AAE File Offset: 0x00487CAE
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x0600C5EA RID: 50666 RVA: 0x00489AC2 File Offset: 0x00487CC2
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.Width);
		}

		// Token: 0x0600C5EB RID: 50667 RVA: 0x00489AD6 File Offset: 0x00487CD6
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}
