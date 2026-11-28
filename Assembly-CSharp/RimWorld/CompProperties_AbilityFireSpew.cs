using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001400 RID: 5120
	public class CompProperties_AbilityFireSpew : CompProperties_AbilityEffect
	{
		// Token: 0x06007EED RID: 32493 RVA: 0x002D5DFA File Offset: 0x002D3FFA
		public CompProperties_AbilityFireSpew()
		{
			this.compClass = typeof(CompAbilityEffect_FireSpew);
		}

		// Token: 0x04004F97 RID: 20375
		public float range;

		// Token: 0x04004F98 RID: 20376
		public float lineWidthEnd;

		// Token: 0x04004F99 RID: 20377
		public ThingDef filthDef;

		// Token: 0x04004F9A RID: 20378
		public int damAmount = -1;

		// Token: 0x04004F9B RID: 20379
		public EffecterDef effecterDef;

		// Token: 0x04004F9C RID: 20380
		public bool canHitFilledCells;
	}
}
