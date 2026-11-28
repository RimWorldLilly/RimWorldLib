using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DCE RID: 7630
	public class PawnColumnWorker_HostilityResponse : PawnColumnWorker
	{
		// Token: 0x0600C640 RID: 50752 RVA: 0x0048AAA9 File Offset: 0x00488CA9
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (!pawn.RaceProps.Humanlike)
			{
				return;
			}
			HostilityResponseModeUtility.DrawResponseButton(rect, pawn, true);
		}

		// Token: 0x0600C641 RID: 50753 RVA: 0x0048AAC1 File Offset: 0x00488CC1
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), Mathf.CeilToInt(24f) + 3);
		}

		// Token: 0x0600C642 RID: 50754 RVA: 0x0048AADB File Offset: 0x00488CDB
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 24);
		}

		// Token: 0x0600C643 RID: 50755 RVA: 0x0048AAEB File Offset: 0x00488CEB
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x0600C644 RID: 50756 RVA: 0x0048AB00 File Offset: 0x00488D00
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x0600C645 RID: 50757 RVA: 0x0048AB23 File Offset: 0x00488D23
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.playerSettings == null)
			{
				return int.MinValue;
			}
			return (int)pawn.playerSettings.hostilityResponse;
		}

		// Token: 0x040070F6 RID: 28918
		private const int TopPadding = 3;

		// Token: 0x040070F7 RID: 28919
		private const int Width = 24;
	}
}
