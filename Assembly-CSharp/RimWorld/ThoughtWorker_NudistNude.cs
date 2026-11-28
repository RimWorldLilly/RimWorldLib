using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E2E RID: 3630
	public class ThoughtWorker_NudistNude : ThoughtWorker
	{
		// Token: 0x06006150 RID: 24912 RVA: 0x00232768 File Offset: 0x00230968
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				Apparel apparel = wornApparel[i];
				if (apparel.def.apparel.countsAsClothingForNudity)
				{
					for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
					{
						if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Torso)
						{
							return false;
						}
						if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Legs)
						{
							return false;
						}
					}
				}
			}
			return true;
		}
	}
}
