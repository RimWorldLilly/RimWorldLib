using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019E1 RID: 6625
	public class CompProperties_DissolutionEffectPollution : CompProperties
	{
		// Token: 0x0600A6DD RID: 42717 RVA: 0x003B78BF File Offset: 0x003B5ABF
		public CompProperties_DissolutionEffectPollution()
		{
			this.compClass = typeof(CompDissolutionEffect_Pollution);
		}

		// Token: 0x0400624B RID: 25163
		public int cellsToPollutePerDissolution = 4;

		// Token: 0x0400624C RID: 25164
		public float tilePollutionPerDissolution = 0.0005f;

		// Token: 0x0400624D RID: 25165
		public float waterTilePollutionFactor = 8f;

		// Token: 0x0400624E RID: 25166
		public SimpleCurve goodWillFactorOverDistanceCurce;
	}
}
