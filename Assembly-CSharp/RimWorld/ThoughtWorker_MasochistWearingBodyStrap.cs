using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E5B RID: 3675
	public class ThoughtWorker_MasochistWearingBodyStrap : ThoughtWorker
	{
		// Token: 0x060061C9 RID: 25033 RVA: 0x00233F50 File Offset: 0x00232150
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (ModsConfig.IdeologyActive)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (wornApparel[i].def == ThingDefOf.Apparel_BodyStrap)
					{
						return true;
					}
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
