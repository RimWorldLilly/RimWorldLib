using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001832 RID: 6194
	public class CompProperties_BiosculpterPod_HealingCycle : CompProperties_BiosculpterPod_BaseCycle
	{
		// Token: 0x04005C4B RID: 23627
		public List<BodyPartDef> bodyPartsToRestore;

		// Token: 0x04005C4C RID: 23628
		public List<HediffDef> conditionsToPossiblyCure;
	}
}
