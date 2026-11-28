using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB6 RID: 7606
	public class PawnColumnWorker_Guest : PawnColumnWorker_Icon
	{
		// Token: 0x0600C5B5 RID: 50613 RVA: 0x0048946B File Offset: 0x0048766B
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			if (pawn == null)
			{
				return null;
			}
			return pawn.guest.GetIcon();
		}

		// Token: 0x0600C5B6 RID: 50614 RVA: 0x00489480 File Offset: 0x00487680
		protected override string GetIconTip(Pawn pawn)
		{
			string str = (pawn != null) ? pawn.guest.GetLabel() : null;
			if (!str.NullOrEmpty())
			{
				return str.CapitalizeFirst();
			}
			return null;
		}
	}
}
