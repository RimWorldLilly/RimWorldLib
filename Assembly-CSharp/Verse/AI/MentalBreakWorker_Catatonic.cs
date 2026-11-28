using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200085B RID: 2139
	public class MentalBreakWorker_Catatonic : MentalBreakWorker
	{
		// Token: 0x0600432A RID: 17194 RVA: 0x001A80C3 File Offset: 0x001A62C3
		public override bool BreakCanOccur(Pawn pawn)
		{
			return pawn.IsColonist && pawn.Spawned && base.BreakCanOccur(pawn);
		}

		// Token: 0x0600432B RID: 17195 RVA: 0x001A80E0 File Offset: 0x001A62E0
		public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			pawn.health.AddHediff(HediffDefOf.CatatonicBreakdown, null, null, null);
			base.TrySendLetter(pawn, "LetterCatatonicMentalBreak", reason);
			return true;
		}
	}
}
