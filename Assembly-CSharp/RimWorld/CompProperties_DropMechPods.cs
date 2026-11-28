using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013ED RID: 5101
	public class CompProperties_DropMechPods : CompProperties_EffectWithDest
	{
		// Token: 0x06007EB2 RID: 32434 RVA: 0x002D4A7D File Offset: 0x002D2C7D
		public CompProperties_DropMechPods()
		{
			this.compClass = typeof(CompAbilityEffect_DropMechPods);
		}

		// Token: 0x04004F73 RID: 20339
		public IntRange numPods;
	}
}
