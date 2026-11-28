using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DC7 RID: 7623
	public class PawnColumnWorker_Age : PawnColumnWorker_Text
	{
		// Token: 0x170024BB RID: 9403
		// (get) Token: 0x0600C612 RID: 50706 RVA: 0x0048A0D2 File Offset: 0x004882D2
		protected override TextAnchor Anchor
		{
			get
			{
				return TextAnchor.MiddleCenter;
			}
		}

		// Token: 0x0600C613 RID: 50707 RVA: 0x0048A0D5 File Offset: 0x004882D5
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 50);
		}

		// Token: 0x0600C614 RID: 50708 RVA: 0x0048A0E8 File Offset: 0x004882E8
		public override int Compare(Pawn a, Pawn b)
		{
			return a.ageTracker.AgeBiologicalYears.CompareTo(b.ageTracker.AgeBiologicalYears);
		}

		// Token: 0x0600C615 RID: 50709 RVA: 0x0048A114 File Offset: 0x00488314
		protected override string GetTextFor(Pawn pawn)
		{
			return pawn.ageTracker.AgeBiologicalYears.ToString();
		}
	}
}
