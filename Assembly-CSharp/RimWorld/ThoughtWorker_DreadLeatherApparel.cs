using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E23 RID: 3619
	public class ThoughtWorker_DreadLeatherApparel : ThoughtWorker_ApparelThought
	{
		// Token: 0x06006135 RID: 24885 RVA: 0x00232144 File Offset: 0x00230344
		protected override bool ApparelCounts(Apparel apparel)
		{
			return apparel.Stuff == ThingDefOf.Leather_Dread;
		}

		// Token: 0x06006136 RID: 24886 RVA: 0x00232153 File Offset: 0x00230353
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			return base.CurrentStateInternal(p);
		}
	}
}
