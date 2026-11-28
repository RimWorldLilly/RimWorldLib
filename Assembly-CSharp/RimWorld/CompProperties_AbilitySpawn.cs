using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001448 RID: 5192
	public class CompProperties_AbilitySpawn : CompProperties_AbilityEffect
	{
		// Token: 0x06007FD0 RID: 32720 RVA: 0x002D9915 File Offset: 0x002D7B15
		public CompProperties_AbilitySpawn()
		{
			this.compClass = typeof(CompAbilityEffect_Spawn);
		}

		// Token: 0x04004FF0 RID: 20464
		public ThingDef thingDef;

		// Token: 0x04004FF1 RID: 20465
		public bool allowOnBuildings = true;

		// Token: 0x04004FF2 RID: 20466
		public bool sendSkipSignal = true;
	}
}
