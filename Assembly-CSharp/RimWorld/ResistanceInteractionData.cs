using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001554 RID: 5460
	public class ResistanceInteractionData : IExposable
	{
		// Token: 0x06008568 RID: 34152 RVA: 0x003017DC File Offset: 0x002FF9DC
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.resistanceReduction, "resistanceReduction", 0f, false);
			Scribe_Values.Look<float>(ref this.initiatorNegotiationAbilityFactor, "initiatorNegotiationAbilityFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.recruiteeMoodFactor, "recruiteeMoodFactor", 0f, false);
			Scribe_Values.Look<string>(ref this.initiatorName, "initiatorName", null, false);
			Scribe_Values.Look<float>(ref this.recruiterOpinionFactor, "recruiterOpinionFactor", 0f, false);
		}

		// Token: 0x04005219 RID: 21017
		public float resistanceReduction;

		// Token: 0x0400521A RID: 21018
		public float initiatorNegotiationAbilityFactor;

		// Token: 0x0400521B RID: 21019
		public float recruiteeMoodFactor;

		// Token: 0x0400521C RID: 21020
		public string initiatorName;

		// Token: 0x0400521D RID: 21021
		public float recruiterOpinionFactor;
	}
}
