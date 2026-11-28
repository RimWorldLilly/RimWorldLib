using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001412 RID: 5138
	public class CompProperties_AbilityGiveMentalState : CompProperties_AbilityEffect
	{
		// Token: 0x04004FAD RID: 20397
		public MentalStateDef stateDef;

		// Token: 0x04004FAE RID: 20398
		public MentalStateDef stateDefForMechs;

		// Token: 0x04004FAF RID: 20399
		public StatDef durationMultiplier;

		// Token: 0x04004FB0 RID: 20400
		public bool applyToSelf;

		// Token: 0x04004FB1 RID: 20401
		public EffecterDef casterEffect;

		// Token: 0x04004FB2 RID: 20402
		public EffecterDef targetEffect;

		// Token: 0x04004FB3 RID: 20403
		public bool excludeNPCFactions;

		// Token: 0x04004FB4 RID: 20404
		public bool forced;
	}
}
