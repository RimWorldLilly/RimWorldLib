using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001864 RID: 6244
	public class CompProperties_DisruptorFlare : CompProperties
	{
		// Token: 0x06009C43 RID: 40003 RVA: 0x003842B5 File Offset: 0x003824B5
		public CompProperties_DisruptorFlare()
		{
			this.compClass = typeof(CompDisruptorFlare);
		}

		// Token: 0x04005CD4 RID: 23764
		public float radius;

		// Token: 0x04005CD5 RID: 23765
		public EffecterDef destroyWarningEffecterDef;

		// Token: 0x04005CD6 RID: 23766
		public EffecterDef effecterDef;
	}
}
