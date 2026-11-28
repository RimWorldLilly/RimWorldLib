using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000435 RID: 1077
	public class HediffCompProperties_GiveAbility : HediffCompProperties
	{
		// Token: 0x06001F3B RID: 7995 RVA: 0x000BE367 File Offset: 0x000BC567
		public HediffCompProperties_GiveAbility()
		{
			this.compClass = typeof(HediffComp_GiveAbility);
		}

		// Token: 0x04001863 RID: 6243
		public AbilityDef abilityDef;

		// Token: 0x04001864 RID: 6244
		public List<AbilityDef> abilityDefs;
	}
}
