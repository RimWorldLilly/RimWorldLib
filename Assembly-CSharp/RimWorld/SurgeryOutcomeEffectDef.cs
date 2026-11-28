using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002011 RID: 8209
	public class SurgeryOutcomeEffectDef : Def
	{
		// Token: 0x0600CFE8 RID: 53224 RVA: 0x004C76CC File Offset: 0x004C58CC
		public float GetQuality(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
		{
			float result = 1f;
			if (!this.comps.NullOrEmpty<SurgeryOutcomeComp>())
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].Affects(recipe, surgeon, patient, part))
					{
						this.comps[i].AffectQuality(recipe, surgeon, patient, ingredients, part, bill, ref result);
					}
				}
			}
			return result;
		}

		// Token: 0x0600CFE9 RID: 53225 RVA: 0x004C7738 File Offset: 0x004C5938
		public SurgeryOutcome GetOutcome(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
		{
			float quality = this.GetQuality(recipe, surgeon, patient, ingredients, part, bill);
			if (!this.comps.NullOrEmpty<SurgeryOutcomeComp>())
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].Affects(recipe, surgeon, patient, part))
					{
						this.comps[i].PreApply(quality, recipe, surgeon, patient, ingredients, part, bill);
					}
				}
			}
			if (!this.outcomes.NullOrEmpty<SurgeryOutcome>())
			{
				for (int j = 0; j < this.outcomes.Count; j++)
				{
					if (this.outcomes[j].Apply(quality, recipe, surgeon, patient, part))
					{
						return this.outcomes[j];
					}
				}
			}
			return null;
		}

		// Token: 0x0400843F RID: 33855
		public List<SurgeryOutcomeComp> comps;

		// Token: 0x04008440 RID: 33856
		public List<SurgeryOutcome> outcomes;
	}
}
