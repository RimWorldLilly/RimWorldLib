using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000402 RID: 1026
	public class HediffGiver_MeatHunger : HediffGiver
	{
		// Token: 0x06001DD1 RID: 7633 RVA: 0x000B7ED8 File Offset: 0x000B60D8
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			Need need;
			if (pawn.needs != null && pawn.needs.TryGetNeed(NeedDefOf.Food, out need) && need.CurLevel <= 0f)
			{
				HealthUtility.AdjustSeverity(pawn, this.hediff, 0.00090000004f);
				return;
			}
			HealthUtility.AdjustSeverity(pawn, this.hediff, -0.024f);
		}

		// Token: 0x040017D8 RID: 6104
		private const float BaseMeatHungerSeverityIncreasePerDay = 0.9f;

		// Token: 0x040017D9 RID: 6105
		private const float BaseMeatHungerSeverityDecreasePerDay = 24f;

		// Token: 0x040017DA RID: 6106
		private const float BaseMeatHungerSeverityIncreasePerInterval = 0.00090000004f;

		// Token: 0x040017DB RID: 6107
		private const float BaseMeatHungerSeverityDecreasePerInterval = 0.024f;
	}
}
