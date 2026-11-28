using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB9 RID: 7609
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Info : PawnColumnWorker
	{
		// Token: 0x0600C5CD RID: 50637 RVA: 0x00489748 File Offset: 0x00487948
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Widgets.InfoCardButtonCentered(rect, pawn);
		}

		// Token: 0x0600C5CE RID: 50638 RVA: 0x00489752 File Offset: 0x00487952
		public override int GetMinWidth(PawnTable table)
		{
			return 24;
		}

		// Token: 0x0600C5CF RID: 50639 RVA: 0x00489756 File Offset: 0x00487956
		public override int GetMaxWidth(PawnTable table)
		{
			return 24;
		}
	}
}
