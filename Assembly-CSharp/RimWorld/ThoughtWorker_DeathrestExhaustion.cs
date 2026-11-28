using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E6E RID: 3694
	public class ThoughtWorker_DeathrestExhaustion : ThoughtWorker
	{
		// Token: 0x060061FD RID: 25085 RVA: 0x00234860 File Offset: 0x00232A60
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			Need_Deathrest need_Deathrest;
			if (p.needs == null || !p.needs.TryGetNeed<Need_Deathrest>(out need_Deathrest))
			{
				return ThoughtState.Inactive;
			}
			return need_Deathrest.CurLevel == 0f;
		}
	}
}
