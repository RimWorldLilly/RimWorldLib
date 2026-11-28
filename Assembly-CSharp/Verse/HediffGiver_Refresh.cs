using System;

namespace Verse
{
	// Token: 0x02000406 RID: 1030
	public class HediffGiver_Refresh : HediffGiver
	{
		// Token: 0x06001DDA RID: 7642 RVA: 0x000B80F0 File Offset: 0x000B62F0
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(this.hediff, false);
			if (firstHediffOfDef != null)
			{
				firstHediffOfDef.ageTicks = 0;
				return;
			}
			if (base.TryApply(pawn, null))
			{
				base.SendLetter(pawn, cause);
			}
		}
	}
}
