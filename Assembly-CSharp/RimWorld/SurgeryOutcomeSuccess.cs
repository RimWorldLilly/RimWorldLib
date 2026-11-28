using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200201E RID: 8222
	public class SurgeryOutcomeSuccess : SurgeryOutcome
	{
		// Token: 0x0600D00B RID: 53259 RVA: 0x004C7AEF File Offset: 0x004C5CEF
		public override bool Apply(float quality, RecipeDef recipe, Pawn surgeon, Pawn patient, BodyPartRecord part)
		{
			return Rand.Chance(quality);
		}
	}
}
