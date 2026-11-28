using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200085A RID: 2138
	public class MentalBreakWorker_HumanityBreak : MentalBreakWorker
	{
		// Token: 0x06004327 RID: 17191 RVA: 0x001A805C File Offset: 0x001A625C
		public override bool BreakCanOccur(Pawn pawn)
		{
			return ModsConfig.AnomalyActive && base.BreakCanOccur(pawn) && !pawn.Inhumanized();
		}

		// Token: 0x06004328 RID: 17192 RVA: 0x001A807C File Offset: 0x001A627C
		public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			if (base.TryStart(pawn, reason, causedByMood))
			{
				if (!pawn.Inhumanized())
				{
					pawn.health.AddHediff(HediffDefOf.Inhumanized, null, null, null);
				}
				return true;
			}
			return false;
		}
	}
}
