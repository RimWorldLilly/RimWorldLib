using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E5E RID: 3678
	public class ThoughtWorker_WearingColor_Ideo : ThoughtWorker_WearingColor
	{
		// Token: 0x060061D0 RID: 25040 RVA: 0x002340B4 File Offset: 0x002322B4
		protected override Color? Color(Pawn p)
		{
			Ideo ideo = p.Ideo;
			if (ideo == null)
			{
				return null;
			}
			return new Color?(ideo.ApparelColor);
		}
	}
}
