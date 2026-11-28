using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200146C RID: 5228
	public class CompProperties_AbilityEffectWithDuration : CompProperties_AbilityEffect
	{
		// Token: 0x0400501B RID: 20507
		public StatDef durationMultiplier;

		// Token: 0x0400501C RID: 20508
		public FloatRange durationSecondsOverride = FloatRange.Zero;
	}
}
