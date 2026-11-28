using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002095 RID: 8341
	public class CompProperties_HeatPusherEffecter : CompProperties
	{
		// Token: 0x0600D247 RID: 53831 RVA: 0x004D3EB5 File Offset: 0x004D20B5
		public CompProperties_HeatPusherEffecter()
		{
			this.compClass = typeof(CompHeatPusherEffecter);
		}

		// Token: 0x04008535 RID: 34101
		public EffecterDef effecterDef;
	}
}
