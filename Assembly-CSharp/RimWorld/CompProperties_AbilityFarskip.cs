using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013F9 RID: 5113
	public class CompProperties_AbilityFarskip : CompProperties_AbilityEffect
	{
		// Token: 0x06007ECD RID: 32461 RVA: 0x002D517E File Offset: 0x002D337E
		public CompProperties_AbilityFarskip()
		{
			this.compClass = typeof(CompAbilityEffect_Farskip);
		}

		// Token: 0x04004F93 RID: 20371
		public IntRange stunTicks;
	}
}
