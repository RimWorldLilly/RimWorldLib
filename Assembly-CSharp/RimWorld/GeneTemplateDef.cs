using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E98 RID: 3736
	public class GeneTemplateDef : Def
	{
		// Token: 0x06006280 RID: 25216 RVA: 0x00236BE2 File Offset: 0x00234DE2
		public override IEnumerable<string> ConfigErrors()
		{
			GeneTemplateDef.<ConfigErrors>d__17 <ConfigErrors>d__ = new GeneTemplateDef.<ConfigErrors>d__17(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0400362A RID: 13866
		public Type geneClass = typeof(Gene);

		// Token: 0x0400362B RID: 13867
		public int biostatCpx;

		// Token: 0x0400362C RID: 13868
		public int biostatMet;

		// Token: 0x0400362D RID: 13869
		public int aptitudeOffset;

		// Token: 0x0400362E RID: 13870
		public float addictionChanceFactor = 1f;

		// Token: 0x0400362F RID: 13871
		public PassionMod.PassionModType passionModType;

		// Token: 0x04003630 RID: 13872
		public GeneTemplateDef.GeneTemplateType geneTemplateType;

		// Token: 0x04003631 RID: 13873
		public float minAgeActive;

		// Token: 0x04003632 RID: 13874
		public GeneCategoryDef displayCategory;

		// Token: 0x04003633 RID: 13875
		public int displayOrderOffset;

		// Token: 0x04003634 RID: 13876
		public float selectionWeight = 1f;

		// Token: 0x04003635 RID: 13877
		public List<GeneTemplateDef.ChemicalBiostatOverride> chemicalBiostatOverrides;

		// Token: 0x04003636 RID: 13878
		[MustTranslate]
		public string labelShortAdj;

		// Token: 0x04003637 RID: 13879
		[NoTranslate]
		public string iconPath;

		// Token: 0x04003638 RID: 13880
		[NoTranslate]
		public string exclusionTagPrefix;

		// Token: 0x02002EEE RID: 12014
		public enum GeneTemplateType
		{
			// Token: 0x0400BE3C RID: 48700
			Skill,
			// Token: 0x0400BE3D RID: 48701
			Chemical
		}

		// Token: 0x02002EEF RID: 12015
		public class ChemicalBiostatOverride
		{
			// Token: 0x0400BE3E RID: 48702
			public ChemicalDef chemical;

			// Token: 0x0400BE3F RID: 48703
			public int? biostatCpx;

			// Token: 0x0400BE40 RID: 48704
			public int? biostatMet;

			// Token: 0x0400BE41 RID: 48705
			public int? biostatArc;
		}
	}
}
