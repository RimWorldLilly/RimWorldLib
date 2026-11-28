using System;

namespace RimWorld
{
	// Token: 0x0200145F RID: 5215
	public class CompProperties_AbilityTransferEntropy : CompProperties_AbilityEffect
	{
		// Token: 0x06008020 RID: 32800 RVA: 0x002DB23F File Offset: 0x002D943F
		public CompProperties_AbilityTransferEntropy()
		{
			this.compClass = typeof(CompAbilityEffect_TransferEntropy);
		}

		// Token: 0x04005007 RID: 20487
		public bool targetReceivesEntropy = true;
	}
}
