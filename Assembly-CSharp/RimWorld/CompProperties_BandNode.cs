using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001827 RID: 6183
	public class CompProperties_BandNode : CompProperties
	{
		// Token: 0x06009AA3 RID: 39587 RVA: 0x0037C619 File Offset: 0x0037A819
		public CompProperties_BandNode()
		{
			this.compClass = typeof(CompBandNode);
		}

		// Token: 0x04005BF1 RID: 23537
		public HediffDef hediff;

		// Token: 0x04005BF2 RID: 23538
		public float retuneDays = 3f;

		// Token: 0x04005BF3 RID: 23539
		public float tuneSeconds = 5f;

		// Token: 0x04005BF4 RID: 23540
		public int powerConsumptionIdle = 100;

		// Token: 0x04005BF5 RID: 23541
		public int emissionInterval;

		// Token: 0x04005BF6 RID: 23542
		public EffecterDef untunedEffect;

		// Token: 0x04005BF7 RID: 23543
		public EffecterDef tuningEffect;

		// Token: 0x04005BF8 RID: 23544
		public EffecterDef tunedEffect;

		// Token: 0x04005BF9 RID: 23545
		public EffecterDef retuningEffect;

		// Token: 0x04005BFA RID: 23546
		public SoundDef tuningCompleteSound;
	}
}
