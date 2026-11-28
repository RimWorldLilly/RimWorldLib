using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB5 RID: 7605
	public class PawnColumnWorker_Gender : PawnColumnWorker_Icon
	{
		// Token: 0x0600C5B2 RID: 50610 RVA: 0x00489449 File Offset: 0x00487649
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			return pawn.gender.GetIcon();
		}

		// Token: 0x0600C5B3 RID: 50611 RVA: 0x00489456 File Offset: 0x00487656
		protected override string GetIconTip(Pawn pawn)
		{
			return pawn.GetGenderLabel().CapitalizeFirst();
		}
	}
}
