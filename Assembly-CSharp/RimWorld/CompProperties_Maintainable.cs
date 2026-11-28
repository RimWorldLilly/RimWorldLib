using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018E5 RID: 6373
	public class CompProperties_Maintainable : CompProperties
	{
		// Token: 0x06009FBC RID: 40892 RVA: 0x00393FE3 File Offset: 0x003921E3
		public CompProperties_Maintainable()
		{
			this.compClass = typeof(CompMaintainable);
		}

		// Token: 0x04005E75 RID: 24181
		public int ticksHealthy = 1000;

		// Token: 0x04005E76 RID: 24182
		public int ticksNeedsMaintenance = 1000;

		// Token: 0x04005E77 RID: 24183
		public int damagePerTickRare = 10;
	}
}
