using System;

namespace Verse
{
	// Token: 0x020003FD RID: 1021
	public class HediffGiver_Bleeding : HediffGiver
	{
		// Token: 0x06001DC6 RID: 7622 RVA: 0x000B78E4 File Offset: 0x000B5AE4
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			HediffSet hediffSet = pawn.health.hediffSet;
			if (hediffSet.BleedRateTotal >= 0.1f)
			{
				HealthUtility.AdjustSeverity(pawn, this.hediff, hediffSet.BleedRateTotal * 0.001f);
				return;
			}
			HealthUtility.AdjustSeverity(pawn, this.hediff, -0.00033333333f);
		}
	}
}
