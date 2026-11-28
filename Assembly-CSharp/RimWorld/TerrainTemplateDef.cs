using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E9D RID: 3741
	public class TerrainTemplateDef : Def
	{
		// Token: 0x04003639 RID: 13881
		[NoTranslate]
		public string texturePath;

		// Token: 0x0400363A RID: 13882
		[NoTranslate]
		public List<string> tags;

		// Token: 0x0400363B RID: 13883
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x0400363C RID: 13884
		public TerrainDef burnedDef;

		// Token: 0x0400363D RID: 13885
		public List<ThingDefCountClass> costList;

		// Token: 0x0400363E RID: 13886
		public DesignatorDropdownGroupDef designatorDropdown;

		// Token: 0x0400363F RID: 13887
		public List<StatModifier> statBases;

		// Token: 0x04003640 RID: 13888
		public int uiOrder;

		// Token: 0x04003641 RID: 13889
		public int renderPrecedenceStart;

		// Token: 0x04003642 RID: 13890
		public int constructionSkillPrerequisite;

		// Token: 0x04003643 RID: 13891
		public bool canGenerateDefaultDesignator = true;

		// Token: 0x04003644 RID: 13892
		public StyleCategoryDef dominantStyleCategory;
	}
}
