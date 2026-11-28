using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F07 RID: 3847
	public class IngestionOutcomeDoer_Psilocap : IngestionOutcomeDoer
	{
		// Token: 0x060063FB RID: 25595 RVA: 0x0023CEB8 File Offset: 0x0023B0B8
		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
		{
			if (Rand.Value < this.chanceBreakdown)
			{
				pawn.mindState.mentalBreaker.TryDoMentalBreak("MentalBreakReason_Psilocap".Translate(), MentalBreakDefOf.Catatonic);
				return;
			}
			if (Rand.Value < this.chanceInspiration)
			{
				pawn.mindState.inspirationHandler.TryStartInspiration(InspirationDefOf.Inspired_Creativity, "LetterInspirationBeginPsilocap".Translate(), true);
			}
		}

		// Token: 0x040038CB RID: 14539
		public float chanceBreakdown = 0.01f;

		// Token: 0x040038CC RID: 14540
		public float chanceInspiration = 0.02f;
	}
}
