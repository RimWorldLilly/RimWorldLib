using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DBE RID: 7614
	public class PawnColumnWorker_Xenotype : PawnColumnWorker_Icon
	{
		// Token: 0x0600C5E3 RID: 50659 RVA: 0x00489A66 File Offset: 0x00487C66
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			Pawn_GeneTracker genes = pawn.genes;
			if (genes == null)
			{
				return null;
			}
			return genes.XenotypeIcon;
		}

		// Token: 0x0600C5E4 RID: 50660 RVA: 0x00489A79 File Offset: 0x00487C79
		protected override Color GetIconColor(Pawn pawn)
		{
			return XenotypeDef.IconColor;
		}

		// Token: 0x0600C5E5 RID: 50661 RVA: 0x00489A80 File Offset: 0x00487C80
		protected override string GetIconTip(Pawn pawn)
		{
			if (pawn.genes != null)
			{
				return pawn.genes.XenotypeLabelCap;
			}
			return null;
		}
	}
}
