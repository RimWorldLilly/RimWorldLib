using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013D4 RID: 5076
	public class CompProperties_AbilityConnectingFleckLine : CompProperties_AbilityEffect
	{
		// Token: 0x06007E5E RID: 32350 RVA: 0x002D28F7 File Offset: 0x002D0AF7
		public CompProperties_AbilityConnectingFleckLine()
		{
			this.compClass = typeof(CompAbilityEffect_ConnectingFleckLine);
		}

		// Token: 0x04004F53 RID: 20307
		public FleckDef fleckDef;
	}
}
