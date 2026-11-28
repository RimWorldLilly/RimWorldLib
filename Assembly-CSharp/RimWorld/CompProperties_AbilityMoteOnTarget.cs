using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200141D RID: 5149
	public class CompProperties_AbilityMoteOnTarget : CompProperties_AbilityEffect
	{
		// Token: 0x06007F44 RID: 32580 RVA: 0x002D76E5 File Offset: 0x002D58E5
		public CompProperties_AbilityMoteOnTarget()
		{
			this.compClass = typeof(CompAbilityEffect_MoteOnTarget);
		}

		// Token: 0x04004FBC RID: 20412
		public ThingDef moteDef;

		// Token: 0x04004FBD RID: 20413
		public List<ThingDef> moteDefs;

		// Token: 0x04004FBE RID: 20414
		public float scale = 1f;

		// Token: 0x04004FBF RID: 20415
		public int preCastTicks;
	}
}
