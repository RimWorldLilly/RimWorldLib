using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB2 RID: 7602
	public class PawnColumnWorker_CopyPasteWorkPriorities : PawnColumnWorker_CopyPaste
	{
		// Token: 0x170024B5 RID: 9397
		// (get) Token: 0x0600C5A1 RID: 50593 RVA: 0x00489102 File Offset: 0x00487302
		protected override bool AnythingInClipboard
		{
			get
			{
				return PawnColumnWorker_CopyPasteWorkPriorities.clipboard != null;
			}
		}

		// Token: 0x0600C5A2 RID: 50594 RVA: 0x0048910C File Offset: 0x0048730C
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.Dead || pawn.workSettings == null || !pawn.workSettings.EverWork)
			{
				return;
			}
			base.DoCell(rect, pawn, table);
		}

		// Token: 0x0600C5A3 RID: 50595 RVA: 0x00489138 File Offset: 0x00487338
		protected override void CopyFrom(Pawn p)
		{
			if (PawnColumnWorker_CopyPasteWorkPriorities.clipboard == null)
			{
				PawnColumnWorker_CopyPasteWorkPriorities.clipboard = new DefMap<WorkTypeDef, int>();
			}
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				PawnColumnWorker_CopyPasteWorkPriorities.clipboard[workTypeDef] = ((!p.WorkTypeIsDisabled(workTypeDef)) ? p.workSettings.GetPriority(workTypeDef) : 0);
			}
		}

		// Token: 0x0600C5A4 RID: 50596 RVA: 0x00489198 File Offset: 0x00487398
		protected override void PasteTo(Pawn p)
		{
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				if (!p.WorkTypeIsDisabled(workTypeDef))
				{
					p.workSettings.SetPriority(workTypeDef, PawnColumnWorker_CopyPasteWorkPriorities.clipboard[workTypeDef]);
				}
			}
		}

		// Token: 0x040070DF RID: 28895
		private static DefMap<WorkTypeDef, int> clipboard;
	}
}
