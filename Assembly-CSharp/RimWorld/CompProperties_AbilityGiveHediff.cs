using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200140C RID: 5132
	public class CompProperties_AbilityGiveHediff : CompProperties_AbilityEffectWithDuration
	{
		// Token: 0x04004FA5 RID: 20389
		public HediffDef hediffDef;

		// Token: 0x04004FA6 RID: 20390
		public bool onlyBrain;

		// Token: 0x04004FA7 RID: 20391
		public bool applyToSelf;

		// Token: 0x04004FA8 RID: 20392
		public bool onlyApplyToSelf;

		// Token: 0x04004FA9 RID: 20393
		public bool applyToTarget = true;

		// Token: 0x04004FAA RID: 20394
		public bool replaceExisting;

		// Token: 0x04004FAB RID: 20395
		public float severity = -1f;

		// Token: 0x04004FAC RID: 20396
		public bool ignoreSelf;
	}
}
