using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB1 RID: 7601
	public class PawnColumnWorker_CopyPasteTimetable : PawnColumnWorker_CopyPaste
	{
		// Token: 0x170024B4 RID: 9396
		// (get) Token: 0x0600C59C RID: 50588 RVA: 0x00489084 File Offset: 0x00487284
		protected override bool AnythingInClipboard
		{
			get
			{
				return PawnColumnWorker_CopyPasteTimetable.clipboard != null;
			}
		}

		// Token: 0x0600C59D RID: 50589 RVA: 0x0048908E File Offset: 0x0048728E
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.timetable == null)
			{
				return;
			}
			if (pawn.IsSubhuman)
			{
				return;
			}
			base.DoCell(rect, pawn, table);
		}

		// Token: 0x0600C59E RID: 50590 RVA: 0x004890AB File Offset: 0x004872AB
		protected override void CopyFrom(Pawn p)
		{
			PawnColumnWorker_CopyPasteTimetable.clipboard = p.timetable.times.ToList<TimeAssignmentDef>();
		}

		// Token: 0x0600C59F RID: 50591 RVA: 0x004890C4 File Offset: 0x004872C4
		protected override void PasteTo(Pawn p)
		{
			for (int i = 0; i < 24; i++)
			{
				p.timetable.times[i] = PawnColumnWorker_CopyPasteTimetable.clipboard[i];
			}
		}

		// Token: 0x040070DE RID: 28894
		private static List<TimeAssignmentDef> clipboard;
	}
}
