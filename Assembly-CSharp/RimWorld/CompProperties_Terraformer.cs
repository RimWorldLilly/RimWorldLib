using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019B2 RID: 6578
	public class CompProperties_Terraformer : CompProperties, IPlantEffectRadius
	{
		// Token: 0x0600A51F RID: 42271 RVA: 0x003AF0E4 File Offset: 0x003AD2E4
		public CompProperties_Terraformer()
		{
			this.compClass = typeof(CompTerraformer);
		}

		// Token: 0x17001CBC RID: 7356
		// (get) Token: 0x0600A520 RID: 42272 RVA: 0x003AF112 File Offset: 0x003AD312
		public float PlantEffectRadius
		{
			get
			{
				return this.radius;
			}
		}

		// Token: 0x04006188 RID: 24968
		public float radius = 6.9f;

		// Token: 0x04006189 RID: 24969
		public float secondsPerConvert = 20f;

		// Token: 0x0400618A RID: 24970
		public TerrainDef convertTerrainDef;
	}
}
