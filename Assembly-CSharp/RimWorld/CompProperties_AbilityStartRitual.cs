using System;

namespace RimWorld
{
	// Token: 0x02001450 RID: 5200
	public class CompProperties_AbilityStartRitual : CompProperties_AbilityEffect
	{
		// Token: 0x06007FE6 RID: 32742 RVA: 0x002DA14A File Offset: 0x002D834A
		public CompProperties_AbilityStartRitual()
		{
			this.compClass = typeof(CompAbilityEffect_StartRitual);
		}

		// Token: 0x04004FF8 RID: 20472
		public PreceptDef ritualDef;

		// Token: 0x04004FF9 RID: 20473
		public bool allowedForChild = true;
	}
}
