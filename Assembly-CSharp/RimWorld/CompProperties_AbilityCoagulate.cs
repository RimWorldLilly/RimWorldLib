using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013E0 RID: 5088
	public class CompProperties_AbilityCoagulate : CompProperties_AbilityEffect
	{
		// Token: 0x06007E87 RID: 32391 RVA: 0x002D3609 File Offset: 0x002D1809
		public CompProperties_AbilityCoagulate()
		{
			this.compClass = typeof(CompAbilityEffect_Coagulate);
		}

		// Token: 0x04004F63 RID: 20323
		public FloatRange tendQualityRange;
	}
}
