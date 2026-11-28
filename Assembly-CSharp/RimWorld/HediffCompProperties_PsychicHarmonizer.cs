using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200208C RID: 8332
	public class HediffCompProperties_PsychicHarmonizer : HediffCompProperties
	{
		// Token: 0x0600D217 RID: 53783 RVA: 0x004D2E60 File Offset: 0x004D1060
		public HediffCompProperties_PsychicHarmonizer()
		{
			this.compClass = typeof(HediffComp_PsychicHarmonizer);
		}

		// Token: 0x04008523 RID: 34083
		public float range;

		// Token: 0x04008524 RID: 34084
		public ThoughtDef thought;
	}
}
