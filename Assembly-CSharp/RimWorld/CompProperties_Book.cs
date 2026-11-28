using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA8 RID: 3752
	public class CompProperties_Book : CompProperties_Readable
	{
		// Token: 0x060062D1 RID: 25297 RVA: 0x00239119 File Offset: 0x00237319
		public CompProperties_Book()
		{
			this.compClass = typeof(CompBook);
		}

		// Token: 0x04003680 RID: 13952
		public RulePackDef nameMaker;

		// Token: 0x04003681 RID: 13953
		public RulePackDef descriptionMaker;

		// Token: 0x04003682 RID: 13954
		public float pickWeight = 1f;

		// Token: 0x04003683 RID: 13955
		public GraphicData openGraphic;

		// Token: 0x04003684 RID: 13956
		public GraphicData verticalGraphic;

		// Token: 0x04003685 RID: 13957
		public FloatRange ageYearsRange = new FloatRange(5f, 10f);

		// Token: 0x04003686 RID: 13958
		public float questChance;
	}
}
