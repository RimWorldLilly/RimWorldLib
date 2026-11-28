using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E5A RID: 3674
	public class ThoughtWorker_MasochistWearingCollar : ThoughtWorker
	{
		// Token: 0x060061C7 RID: 25031 RVA: 0x00233EF8 File Offset: 0x002320F8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (ModsConfig.IdeologyActive)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (wornApparel[i].def == ThingDefOf.Apparel_Collar)
					{
						return true;
					}
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
