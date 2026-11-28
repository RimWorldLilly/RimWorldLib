using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013EF RID: 5103
	public class CompProperties_DropMechs : CompProperties_EffectWithDest
	{
		// Token: 0x06007EB7 RID: 32439 RVA: 0x002D4B55 File Offset: 0x002D2D55
		public CompProperties_DropMechs()
		{
			this.compClass = typeof(CompAbilityEffect_DropMechs);
		}

		// Token: 0x04004F75 RID: 20341
		public FloatRange points;
	}
}
