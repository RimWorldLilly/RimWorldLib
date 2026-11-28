using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001932 RID: 6450
	public class CompProperties_PlantHarmRadius : CompProperties
	{
		// Token: 0x0600A1D6 RID: 41430 RVA: 0x0039DAE5 File Offset: 0x0039BCE5
		public CompProperties_PlantHarmRadius()
		{
			this.compClass = typeof(CompPlantHarmRadius);
		}

		// Token: 0x04005F9A RID: 24474
		public float harmFrequencyPerArea = 0.011f;

		// Token: 0x04005F9B RID: 24475
		public float leaflessPlantKillChance = 0.05f;

		// Token: 0x04005F9C RID: 24476
		public bool ignoreSpecialTrees;

		// Token: 0x04005F9D RID: 24477
		public bool messageOnCropDeath = true;

		// Token: 0x04005F9E RID: 24478
		public SimpleCurve radiusPerDayCurve;
	}
}
