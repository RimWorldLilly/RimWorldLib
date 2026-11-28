using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019B8 RID: 6584
	public class CompProperties_ThingContainer : CompProperties
	{
		// Token: 0x04006192 RID: 24978
		public int stackLimit;

		// Token: 0x04006193 RID: 24979
		public int minCountToEmpty;

		// Token: 0x04006194 RID: 24980
		public IntVec3 containedThingOffset;

		// Token: 0x04006195 RID: 24981
		public bool drawStackLabel;

		// Token: 0x04006196 RID: 24982
		public bool drawContainedThing = true;

		// Token: 0x04006197 RID: 24983
		public EffecterDef dropEffecterDef;
	}
}
