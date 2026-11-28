using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200201F RID: 8223
	public class SurgeryOutcome_Death : SurgeryOutcome
	{
		// Token: 0x0600D00D RID: 53261 RVA: 0x004C7B00 File Offset: 0x004C5D00
		public override bool Apply(float quality, RecipeDef recipe, Pawn surgeon, Pawn patient, BodyPartRecord part)
		{
			if (ModsConfig.BiotechActive && patient.genes != null && patient.genes.HasActiveGene(GeneDefOf.Deathless))
			{
				return false;
			}
			if (Rand.Chance(recipe.deathOnFailedSurgeryChance))
			{
				base.ApplyDamage(patient, part);
				if (!patient.Dead)
				{
					patient.Kill(null, null);
				}
				base.SendLetter(surgeon, patient, recipe);
				return true;
			}
			return false;
		}
	}
}
