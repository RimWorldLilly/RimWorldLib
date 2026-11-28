using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001452 RID: 5202
	public class CompProperties_AbilityStartRitualOnPawn : CompProperties_AbilityStartRitual
	{
		// Token: 0x06007FEF RID: 32751 RVA: 0x002DA3DE File Offset: 0x002D85DE
		public CompProperties_AbilityStartRitualOnPawn()
		{
			this.compClass = typeof(CompAbilityEffect_StartRitualOnPawn);
		}

		// Token: 0x04004FFB RID: 20475
		[NoTranslate]
		public string targetRoleId;
	}
}
