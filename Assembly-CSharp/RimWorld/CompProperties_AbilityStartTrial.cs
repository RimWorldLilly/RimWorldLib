using System;

namespace RimWorld
{
	// Token: 0x02001454 RID: 5204
	public class CompProperties_AbilityStartTrial : CompProperties_AbilityStartRitualOnPawn
	{
		// Token: 0x06007FF4 RID: 32756 RVA: 0x002DA49E File Offset: 0x002D869E
		public CompProperties_AbilityStartTrial()
		{
			this.compClass = typeof(CompAbilityEffect_StartTrial);
		}

		// Token: 0x04004FFC RID: 20476
		public PreceptDef ritualDefForPrisoner;

		// Token: 0x04004FFD RID: 20477
		public PreceptDef ritualDefForMentalState;
	}
}
