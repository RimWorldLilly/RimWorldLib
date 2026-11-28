using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002012 RID: 8210
	public class SurgeryOutcomeComp
	{
		// Token: 0x0600CFEB RID: 53227 RVA: 0x004C77FD File Offset: 0x004C59FD
		public virtual bool Affects(RecipeDef recipe, Pawn surgeon, Pawn patient, BodyPartRecord part)
		{
			return true;
		}

		// Token: 0x0600CFEC RID: 53228 RVA: 0x004C7800 File Offset: 0x004C5A00
		public virtual void AffectQuality(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill, ref float quality)
		{
		}

		// Token: 0x0600CFED RID: 53229 RVA: 0x004C7802 File Offset: 0x004C5A02
		public virtual void PreApply(float quality, RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
		{
		}
	}
}
