using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002015 RID: 8213
	public class SurgeryOutcomeComp_ClampToRange : SurgeryOutcomeComp
	{
		// Token: 0x0600CFF4 RID: 53236 RVA: 0x004C786E File Offset: 0x004C5A6E
		public override void AffectQuality(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill, ref float quality)
		{
			quality = this.range.ClampToRange(quality);
		}

		// Token: 0x04008443 RID: 33859
		public FloatRange range;
	}
}
