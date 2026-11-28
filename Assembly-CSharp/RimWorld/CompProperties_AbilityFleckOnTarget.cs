using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001406 RID: 5126
	public class CompProperties_AbilityFleckOnTarget : CompProperties_AbilityEffect
	{
		// Token: 0x06007F02 RID: 32514 RVA: 0x002D67A8 File Offset: 0x002D49A8
		public CompProperties_AbilityFleckOnTarget()
		{
			this.compClass = typeof(CompAbilityEffect_FleckOnTarget);
		}

		// Token: 0x04004F9F RID: 20383
		public FleckDef fleckDef;

		// Token: 0x04004FA0 RID: 20384
		public List<FleckDef> fleckDefs;

		// Token: 0x04004FA1 RID: 20385
		public float scale = 1f;

		// Token: 0x04004FA2 RID: 20386
		public int preCastTicks;
	}
}
