using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001472 RID: 5234
	public class CompProperties_AbilityBurner : CompProperties_AbilityEffect
	{
		// Token: 0x06008071 RID: 32881 RVA: 0x002DCD3C File Offset: 0x002DAF3C
		public CompProperties_AbilityBurner()
		{
			this.compClass = typeof(CompAbilityEffect_Burner);
		}

		// Token: 0x0400501F RID: 20511
		public ThingDef moteDef;

		// Token: 0x04005020 RID: 20512
		public int numStreams;

		// Token: 0x04005021 RID: 20513
		public float coneSizeDegrees;

		// Token: 0x04005022 RID: 20514
		public float range;

		// Token: 0x04005023 RID: 20515
		public float rangeNoise;

		// Token: 0x04005024 RID: 20516
		public float barrelOffsetDistance;

		// Token: 0x04005025 RID: 20517
		public int lifespanNoise;

		// Token: 0x04005026 RID: 20518
		public float sizeReductionDistanceThreshold;

		// Token: 0x04005027 RID: 20519
		public EffecterDef effecterDef;
	}
}
