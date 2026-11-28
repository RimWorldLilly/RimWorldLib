using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002018 RID: 8216
	public class SurgeryOutcomeComp_BedAndRoomQuality : SurgeryOutcomeComp
	{
		// Token: 0x0600CFFB RID: 53243 RVA: 0x004C78CE File Offset: 0x004C5ACE
		public override bool Affects(RecipeDef recipe, Pawn surgeon, Pawn patient, BodyPartRecord part)
		{
			return !recipe.surgeryIgnoreEnvironment && patient.InBed();
		}

		// Token: 0x0600CFFC RID: 53244 RVA: 0x004C78E0 File Offset: 0x004C5AE0
		public override void AffectQuality(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill, ref float quality)
		{
			quality *= patient.CurrentBed().GetStatValue(StatDefOf.SurgerySuccessChanceFactor, true, -1);
		}
	}
}
