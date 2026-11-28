using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200193A RID: 6458
	public class CompProperties_PollutionPump : CompProperties, IPlantEffectRadius
	{
		// Token: 0x0600A1F6 RID: 41462 RVA: 0x0039E087 File Offset: 0x0039C287
		public CompProperties_PollutionPump()
		{
			this.compClass = typeof(CompPollutionPump);
		}

		// Token: 0x17001BFE RID: 7166
		// (get) Token: 0x0600A1F7 RID: 41463 RVA: 0x0039E0BC File Offset: 0x0039C2BC
		public float PlantEffectRadius
		{
			get
			{
				return this.radius;
			}
		}

		// Token: 0x04005FA8 RID: 24488
		public float radius = 9.9f;

		// Token: 0x04005FA9 RID: 24489
		public int intervalTicks = 60000;

		// Token: 0x04005FAA RID: 24490
		public EffecterDef pumpEffecterDef;

		// Token: 0x04005FAB RID: 24491
		public int pumpsPerWastepack = 3;

		// Token: 0x04005FAC RID: 24492
		public bool disabledByArtificialBuildings;
	}
}
