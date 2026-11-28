using System;

namespace Verse
{
	// Token: 0x02000139 RID: 313
	public class CompProperties_TemperatureDamaged : CompProperties
	{
		// Token: 0x060009F7 RID: 2551 RVA: 0x000370CF File Offset: 0x000352CF
		public CompProperties_TemperatureDamaged()
		{
			this.compClass = typeof(CompTemperatureDamaged);
		}

		// Token: 0x040005A0 RID: 1440
		public FloatRange safeTemperatureRange = new FloatRange(-30f, 30f);

		// Token: 0x040005A1 RID: 1441
		public int damagePerTickRare = 1;
	}
}
