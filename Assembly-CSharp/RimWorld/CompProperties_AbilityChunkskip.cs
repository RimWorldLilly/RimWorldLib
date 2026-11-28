using System;

namespace RimWorld
{
	// Token: 0x020013DE RID: 5086
	public class CompProperties_AbilityChunkskip : CompProperties_AbilityEffect
	{
		// Token: 0x06007E79 RID: 32377 RVA: 0x002D3032 File Offset: 0x002D1232
		public CompProperties_AbilityChunkskip()
		{
			this.compClass = typeof(CompAbilityEffect_Chunkskip);
		}

		// Token: 0x04004F5E RID: 20318
		public int chunkCount;

		// Token: 0x04004F5F RID: 20319
		public float scatterRadius;
	}
}
