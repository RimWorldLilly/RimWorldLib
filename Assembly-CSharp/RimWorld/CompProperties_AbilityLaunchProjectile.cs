using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200141B RID: 5147
	public class CompProperties_AbilityLaunchProjectile : CompProperties_AbilityEffect
	{
		// Token: 0x06007F3E RID: 32574 RVA: 0x002D7630 File Offset: 0x002D5830
		public CompProperties_AbilityLaunchProjectile()
		{
			this.compClass = typeof(CompAbilityEffect_LaunchProjectile);
		}

		// Token: 0x04004FBB RID: 20411
		public ThingDef projectileDef;
	}
}
