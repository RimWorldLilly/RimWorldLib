using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA3 RID: 3491
	public class ThoughtWorker_Precept_GroinUncovered : ThoughtWorker_Precept
	{
		// Token: 0x06005FF7 RID: 24567 RVA: 0x0022DEF1 File Offset: 0x0022C0F1
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_GroinUncovered.HasUncoveredGroin(p);
		}

		// Token: 0x06005FF8 RID: 24568 RVA: 0x0022DF00 File Offset: 0x0022C100
		public static bool HasUncoveredGroin(Pawn p)
		{
			if (p.apparel == null)
			{
				return false;
			}
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				Apparel apparel = wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Legs)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
