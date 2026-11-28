using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200043B RID: 1083
	public class HediffCompProperties_GiveHediffsInRange : HediffCompProperties
	{
		// Token: 0x06001F4A RID: 8010 RVA: 0x000BE844 File Offset: 0x000BCA44
		public HediffCompProperties_GiveHediffsInRange()
		{
			this.compClass = typeof(HediffComp_GiveHediffsInRange);
		}

		// Token: 0x0400186F RID: 6255
		public float range;

		// Token: 0x04001870 RID: 6256
		public TargetingParameters targetingParameters;

		// Token: 0x04001871 RID: 6257
		public HediffDef hediff;

		// Token: 0x04001872 RID: 6258
		public ThingDef mote;

		// Token: 0x04001873 RID: 6259
		public bool hideMoteWhenNotDrafted;

		// Token: 0x04001874 RID: 6260
		public float initialSeverity = 1f;

		// Token: 0x04001875 RID: 6261
		public bool onlyPawnsInSameFaction = true;
	}
}
