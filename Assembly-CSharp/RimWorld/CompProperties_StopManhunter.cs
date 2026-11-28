using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001456 RID: 5206
	public class CompProperties_StopManhunter : CompProperties_AbilityEffect
	{
		// Token: 0x06007FF9 RID: 32761 RVA: 0x002DA5BC File Offset: 0x002D87BC
		public CompProperties_StopManhunter()
		{
			this.compClass = typeof(CompAbilityEffect_StopManhunter);
		}

		// Token: 0x04004FFE RID: 20478
		[MustTranslate]
		public string successMessage;
	}
}
