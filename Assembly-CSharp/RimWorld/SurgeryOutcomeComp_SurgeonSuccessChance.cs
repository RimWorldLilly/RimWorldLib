using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002017 RID: 8215
	public class SurgeryOutcomeComp_SurgeonSuccessChance : SurgeryOutcomeComp
	{
		// Token: 0x0600CFF8 RID: 53240 RVA: 0x004C78A0 File Offset: 0x004C5AA0
		public override bool Affects(RecipeDef recipe, Pawn surgeon, Pawn patient, BodyPartRecord part)
		{
			return !patient.RaceProps.IsMechanoid;
		}

		// Token: 0x0600CFF9 RID: 53241 RVA: 0x004C78B0 File Offset: 0x004C5AB0
		public override void AffectQuality(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill, ref float quality)
		{
			quality *= surgeon.GetStatValue(StatDefOf.MedicalSurgerySuccessChance, true, -1);
		}
	}
}
