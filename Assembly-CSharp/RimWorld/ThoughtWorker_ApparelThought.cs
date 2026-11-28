using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E21 RID: 3617
	public abstract class ThoughtWorker_ApparelThought : ThoughtWorker
	{
		// Token: 0x06006130 RID: 24880
		protected abstract bool ApparelCounts(Apparel apparel);

		// Token: 0x06006131 RID: 24881 RVA: 0x002320B8 File Offset: 0x002302B8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			string text = null;
			int num = 0;
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (this.ApparelCounts(wornApparel[i]))
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
	}
}
