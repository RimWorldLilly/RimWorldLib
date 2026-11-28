using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA1 RID: 3489
	public class ThoughtWorker_Precept_FaceCovered : ThoughtWorker_Precept
	{
		// Token: 0x06005FF2 RID: 24562 RVA: 0x0022DE4B File Offset: 0x0022C04B
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_FaceCovered.HasCoveredFace(p);
		}

		// Token: 0x06005FF3 RID: 24563 RVA: 0x0022DE58 File Offset: 0x0022C058
		public static bool HasCoveredFace(Pawn p)
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
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.UpperHead)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
