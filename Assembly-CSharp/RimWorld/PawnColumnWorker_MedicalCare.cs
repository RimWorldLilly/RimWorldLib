using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001DD1 RID: 7633
	public class PawnColumnWorker_MedicalCare : PawnColumnWorker
	{
		// Token: 0x0600C65B RID: 50779 RVA: 0x0048B01E File Offset: 0x0048921E
		public override void DoHeader(Rect rect, PawnTable table)
		{
			MouseoverSounds.DoRegion(rect);
			base.DoHeader(rect, table);
		}

		// Token: 0x0600C65C RID: 50780 RVA: 0x0048B02E File Offset: 0x0048922E
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 28);
		}

		// Token: 0x0600C65D RID: 50781 RVA: 0x0048B03E File Offset: 0x0048923E
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x0600C65E RID: 50782 RVA: 0x0048B053 File Offset: 0x00489253
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			MedicalCareUtility.MedicalCareSelectButton(rect, pawn);
		}

		// Token: 0x0600C65F RID: 50783 RVA: 0x0048B05C File Offset: 0x0048925C
		public override int Compare(Pawn a, Pawn b)
		{
			return a.playerSettings.medCare.CompareTo(b.playerSettings.medCare);
		}
	}
}
