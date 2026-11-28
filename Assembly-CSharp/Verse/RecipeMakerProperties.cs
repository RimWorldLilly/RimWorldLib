using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000151 RID: 337
	public class RecipeMakerProperties
	{
		// Token: 0x040006FF RID: 1791
		[MustTranslate]
		public string label;

		// Token: 0x04000700 RID: 1792
		public int productCount = 1;

		// Token: 0x04000701 RID: 1793
		public int targetCountAdjustment = 1;

		// Token: 0x04000702 RID: 1794
		public int bulkRecipeCount = -1;

		// Token: 0x04000703 RID: 1795
		public bool useIngredientsForColor = true;

		// Token: 0x04000704 RID: 1796
		public int workAmount = -1;

		// Token: 0x04000705 RID: 1797
		public StatDef workSpeedStat;

		// Token: 0x04000706 RID: 1798
		public StatDef efficiencyStat;

		// Token: 0x04000707 RID: 1799
		public ThingDef unfinishedThingDef;

		// Token: 0x04000708 RID: 1800
		public ThingFilter defaultIngredientFilter;

		// Token: 0x04000709 RID: 1801
		public List<SkillRequirement> skillRequirements;

		// Token: 0x0400070A RID: 1802
		public SkillDef workSkill;

		// Token: 0x0400070B RID: 1803
		public float workSkillLearnPerTick = 1f;

		// Token: 0x0400070C RID: 1804
		public WorkTypeDef requiredGiverWorkType;

		// Token: 0x0400070D RID: 1805
		public EffecterDef effectWorking;

		// Token: 0x0400070E RID: 1806
		public SoundDef soundWorking;

		// Token: 0x0400070F RID: 1807
		public List<ThingDef> recipeUsers;

		// Token: 0x04000710 RID: 1808
		public ResearchProjectDef researchPrerequisite;

		// Token: 0x04000711 RID: 1809
		public List<MemeDef> memePrerequisitesAny;

		// Token: 0x04000712 RID: 1810
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x04000713 RID: 1811
		[NoTranslate]
		public List<string> factionPrerequisiteTags;

		// Token: 0x04000714 RID: 1812
		public bool mechanitorOnlyRecipe;

		// Token: 0x04000715 RID: 1813
		public bool fromIdeoBuildingPreceptOnly;

		// Token: 0x04000716 RID: 1814
		public int displayPriority = 99999;
	}
}
