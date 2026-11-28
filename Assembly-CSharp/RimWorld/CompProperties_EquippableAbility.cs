using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001874 RID: 6260
	public class CompProperties_EquippableAbility : CompProperties
	{
		// Token: 0x06009C9A RID: 40090 RVA: 0x00385FF2 File Offset: 0x003841F2
		public CompProperties_EquippableAbility()
		{
			this.compClass = typeof(CompEquippableAbility);
		}

		// Token: 0x04005CF7 RID: 23799
		public AbilityDef abilityDef;
	}
}
