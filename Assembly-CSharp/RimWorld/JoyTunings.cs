using System;

namespace RimWorld
{
	// Token: 0x02001575 RID: 5493
	public static class JoyTunings
	{
		// Token: 0x04005314 RID: 21268
		public const float BaseJoyGainPerHour = 0.36f;

		// Token: 0x04005315 RID: 21269
		public const float ThreshLow = 0.15f;

		// Token: 0x04005316 RID: 21270
		public const float ThreshSatisfied = 0.3f;

		// Token: 0x04005317 RID: 21271
		public const float ThreshHigh = 0.7f;

		// Token: 0x04005318 RID: 21272
		public const float ThreshVeryHigh = 0.85f;

		// Token: 0x04005319 RID: 21273
		public const float BaseFallPerInterval = 0.0015f;

		// Token: 0x0400531A RID: 21274
		public const float FallRateFactorWhenLow = 0.7f;

		// Token: 0x0400531B RID: 21275
		public const float FallRateFactorWhenVeryLow = 0.4f;

		// Token: 0x0400531C RID: 21276
		public const float FallRateFactorWhenFormingCaravan = 0.5f;

		// Token: 0x0400531D RID: 21277
		public const float ToleranceGainPerJoy = 0.65f;

		// Token: 0x0400531E RID: 21278
		public const float BoredStartToleranceThreshold = 0.5f;

		// Token: 0x0400531F RID: 21279
		public const float BoredEndToleranceThreshold = 0.3f;
	}
}
