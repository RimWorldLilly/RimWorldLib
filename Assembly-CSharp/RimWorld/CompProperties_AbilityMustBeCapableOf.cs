using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200141F RID: 5151
	public class CompProperties_AbilityMustBeCapableOf : CompProperties_AbilityEffect
	{
		// Token: 0x06007F4C RID: 32588 RVA: 0x002D7897 File Offset: 0x002D5A97
		public CompProperties_AbilityMustBeCapableOf()
		{
			this.compClass = typeof(CompAbilityEffect_MustBeCapableOf);
		}

		// Token: 0x04004FC0 RID: 20416
		public WorkTags workTags;
	}
}
