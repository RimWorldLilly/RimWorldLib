using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200201D RID: 8221
	public abstract class SurgeryOutcome
	{
		// Token: 0x0600D007 RID: 53255
		public abstract bool Apply(float quality, RecipeDef recipe, Pawn surgeon, Pawn patient, BodyPartRecord part);

		// Token: 0x0600D008 RID: 53256 RVA: 0x004C7A36 File Offset: 0x004C5C36
		protected void ApplyDamage(Pawn patient, BodyPartRecord part)
		{
			if (this.totalDamage > 0)
			{
				HealthUtility.GiveRandomSurgeryInjuries(patient, this.totalDamage, this.applyEffectsToPart ? part : null);
			}
		}

		// Token: 0x0600D009 RID: 53257 RVA: 0x004C7A5C File Offset: 0x004C5C5C
		protected void SendLetter(Pawn surgeon, Pawn patient, RecipeDef recipe)
		{
			if (!this.letterLabel.NullOrEmpty() && !this.letterText.NullOrEmpty())
			{
				Find.LetterStack.ReceiveLetter(this.letterLabel.Formatted(patient.Named("PATIENT")), this.letterText.Formatted(surgeon.Named("SURGEON"), patient.Named("PATIENT"), recipe.Named("RECIPE")), LetterDefOf.NegativeEvent, patient, null, null, null, null, 0, true);
			}
		}

		// Token: 0x04008445 RID: 33861
		public float chance;

		// Token: 0x04008446 RID: 33862
		public int totalDamage;

		// Token: 0x04008447 RID: 33863
		public bool applyEffectsToPart = true;

		// Token: 0x04008448 RID: 33864
		public bool failure;

		// Token: 0x04008449 RID: 33865
		[MustTranslate]
		public string letterLabel;

		// Token: 0x0400844A RID: 33866
		[MustTranslate]
		public string letterText;
	}
}
