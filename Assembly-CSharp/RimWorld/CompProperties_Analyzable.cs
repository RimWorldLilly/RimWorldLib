using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017EE RID: 6126
	public abstract class CompProperties_Analyzable : CompProperties_Interactable
	{
		// Token: 0x060098E4 RID: 39140 RVA: 0x00376001 File Offset: 0x00374201
		public CompProperties_Analyzable()
		{
			this.compClass = typeof(CompAnalyzable);
		}

		// Token: 0x060098E5 RID: 39141 RVA: 0x0037603C File Offset: 0x0037423C
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_Analyzable.<ConfigErrors>d__16 <ConfigErrors>d__ = new CompProperties_Analyzable.<ConfigErrors>d__16(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04005B5A RID: 23386
		public IntRange analysisRequiredRange = new IntRange(1, 1);

		// Token: 0x04005B5B RID: 23387
		public bool destroyedOnAnalyzed;

		// Token: 0x04005B5C RID: 23388
		public float analysisDurationHours = 1.5f;

		// Token: 0x04005B5D RID: 23389
		public bool showProgress;

		// Token: 0x04005B5E RID: 23390
		public bool allowRepeatAnalysis;

		// Token: 0x04005B5F RID: 23391
		[MustTranslate]
		public string progressedLetterLabel;

		// Token: 0x04005B60 RID: 23392
		[MustTranslate]
		public List<string> progressedLetters = new List<string>();

		// Token: 0x04005B61 RID: 23393
		public LetterDef progressedLetterDef;

		// Token: 0x04005B62 RID: 23394
		[MustTranslate]
		public string completedLetterLabel;

		// Token: 0x04005B63 RID: 23395
		[MustTranslate]
		public string completedLetter;

		// Token: 0x04005B64 RID: 23396
		public LetterDef completedLetterDef;

		// Token: 0x04005B65 RID: 23397
		[MustTranslate]
		public string repeatCompletedLetterLabel;

		// Token: 0x04005B66 RID: 23398
		[MustTranslate]
		public string repeatCompletedLetter;

		// Token: 0x04005B67 RID: 23399
		public LetterDef repeatCompletedLetterDef;

		// Token: 0x04005B68 RID: 23400
		public bool canStudyInPlace;
	}
}
