using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E26 RID: 3622
	public class ThoughtWorker_WrongApparelGender : ThoughtWorker
	{
		// Token: 0x0600613D RID: 24893 RVA: 0x002322C4 File Offset: 0x002304C4
		public override string PostProcessLabel(Pawn p, string label)
		{
			return label.Formatted(p.gender.Opposite().GetLabel(false).ToLower(), p.Named("PAWN"));
		}

		// Token: 0x0600613E RID: 24894 RVA: 0x002322F8 File Offset: 0x002304F8
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.gender.Opposite().GetLabel(false).ToLower(), p.gender.GetLabel(false), p.Named("PAWN"));
		}

		// Token: 0x0600613F RID: 24895 RVA: 0x00232348 File Offset: 0x00230548
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (!wornApparel[i].def.apparel.CorrectGenderForWearing(p.gender))
				{
					return ThoughtState.ActiveDefault;
				}
			}
			return false;
		}
	}
}
