using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002021 RID: 8225
	public class SurgeryOutcome_FailureWithHediff : SurgeryOutcome_Failure
	{
		// Token: 0x0600D014 RID: 53268 RVA: 0x004C7C09 File Offset: 0x004C5E09
		protected override bool CanApply(RecipeDef recipe)
		{
			return this.failedHediff != null && base.CanApply(recipe) && (this.applyToRecipes.NullOrEmpty<RecipeDef>() || this.applyToRecipes.Contains(recipe));
		}

		// Token: 0x0600D015 RID: 53269 RVA: 0x004C7C3C File Offset: 0x004C5E3C
		protected override void PostDamagedApplied(Pawn patient)
		{
			patient.health.AddHediff(this.failedHediff, null, null, null);
		}

		// Token: 0x0400844B RID: 33867
		public List<RecipeDef> applyToRecipes;

		// Token: 0x0400844C RID: 33868
		public HediffDef failedHediff;
	}
}
