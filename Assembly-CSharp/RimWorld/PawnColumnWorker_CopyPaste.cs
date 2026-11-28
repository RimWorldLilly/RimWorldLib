using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB0 RID: 7600
	public abstract class PawnColumnWorker_CopyPaste : PawnColumnWorker
	{
		// Token: 0x170024B3 RID: 9395
		// (get) Token: 0x0600C595 RID: 50581
		protected abstract bool AnythingInClipboard { get; }

		// Token: 0x0600C596 RID: 50582 RVA: 0x00488FF0 File Offset: 0x004871F0
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Action pasteAction = null;
			if (this.AnythingInClipboard)
			{
				pasteAction = delegate()
				{
					this.PasteTo(pawn);
				};
			}
			CopyPasteUI.DoCopyPasteButtons(new Rect(rect.x, rect.y, 36f, 30f), delegate
			{
				this.CopyFrom(pawn);
			}, pasteAction);
		}

		// Token: 0x0600C597 RID: 50583 RVA: 0x00489057 File Offset: 0x00487257
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 36);
		}

		// Token: 0x0600C598 RID: 50584 RVA: 0x00489067 File Offset: 0x00487267
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x0600C599 RID: 50585
		protected abstract void CopyFrom(Pawn p);

		// Token: 0x0600C59A RID: 50586
		protected abstract void PasteTo(Pawn p);
	}
}
