using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E24 RID: 3620
	public class ThoughtWorker_HumanLeatherApparel : ThoughtWorker
	{
		// Token: 0x06006138 RID: 24888 RVA: 0x00232174 File Offset: 0x00230374
		public static ThoughtState CurrentThoughtState(Pawn p)
		{
			string text = null;
			int num = 0;
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (wornApparel[i].Stuff == ThingDefOf.Human.race.leatherDef)
				{
					if (text == null)
					{
						text = wornApparel[i].def.label;
					}
					num++;
				}
			}
			if (num == 0)
			{
				return ThoughtState.Inactive;
			}
			if (num >= 5)
			{
				return ThoughtState.ActiveAtStage(4, text);
			}
			return ThoughtState.ActiveAtStage(num - 1, text);
		}

		// Token: 0x06006139 RID: 24889 RVA: 0x002321F6 File Offset: 0x002303F6
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return ThoughtWorker_HumanLeatherApparel.CurrentThoughtState(p);
		}
	}
}
