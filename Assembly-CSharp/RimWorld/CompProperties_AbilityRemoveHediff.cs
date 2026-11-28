using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001436 RID: 5174
	public class CompProperties_AbilityRemoveHediff : CompProperties_AbilityEffect
	{
		// Token: 0x06007F8F RID: 32655 RVA: 0x002D8B91 File Offset: 0x002D6D91
		public CompProperties_AbilityRemoveHediff()
		{
			this.compClass = typeof(CompAbilityEffect_RemoveHediff);
		}

		// Token: 0x04004FD7 RID: 20439
		public HediffDef hediffDef;

		// Token: 0x04004FD8 RID: 20440
		public bool applyToSelf;

		// Token: 0x04004FD9 RID: 20441
		public bool applyToTarget;
	}
}
