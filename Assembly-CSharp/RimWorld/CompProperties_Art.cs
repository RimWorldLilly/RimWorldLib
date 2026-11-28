using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC1 RID: 3777
	public class CompProperties_Art : CompProperties
	{
		// Token: 0x0600635F RID: 25439 RVA: 0x0023B33F File Offset: 0x0023953F
		public CompProperties_Art()
		{
			this.compClass = typeof(CompArt);
		}

		// Token: 0x0400378D RID: 14221
		public RulePackDef nameMaker;

		// Token: 0x0400378E RID: 14222
		public RulePackDef descriptionMaker;

		// Token: 0x0400378F RID: 14223
		public QualityCategory minQualityForArtistic;

		// Token: 0x04003790 RID: 14224
		public bool mustBeFullGrave;

		// Token: 0x04003791 RID: 14225
		public bool canBeEnjoyedAsArt;
	}
}
