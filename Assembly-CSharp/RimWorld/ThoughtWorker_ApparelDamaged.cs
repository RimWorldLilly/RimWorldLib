using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E25 RID: 3621
	public class ThoughtWorker_ApparelDamaged : ThoughtWorker
	{
		// Token: 0x0600613B RID: 24891 RVA: 0x00232208 File Offset: 0x00230408
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			float num = 999f;
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (wornApparel[i].def.useHitPoints && !p.apparel.IsLocked(wornApparel[i]) && wornApparel[i].def.apparel.careIfDamaged)
				{
					float num2 = (float)wornApparel[i].HitPoints / (float)wornApparel[i].MaxHitPoints;
					if (num2 < num)
					{
						num = num2;
					}
					if (num < 0.2f)
					{
						return ThoughtState.ActiveAtStage(1);
					}
				}
			}
			if (num < 0.5f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x04003600 RID: 13824
		public const float MinForFrayed = 0.5f;

		// Token: 0x04003601 RID: 13825
		public const float MinForTattered = 0.2f;
	}
}
