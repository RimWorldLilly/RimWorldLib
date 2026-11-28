using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200015B RID: 347
	public class StuffProperties
	{
		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000A7D RID: 2685 RVA: 0x0003A2D4 File Offset: 0x000384D4
		public ThingDef SourceNaturalRock
		{
			get
			{
				if (!this.sourceNaturalRockCached)
				{
					List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
					List<RecipeDef> allDefsListForReading2 = DefDatabase<RecipeDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].category == ThingCategory.Building && allDefsListForReading[i].building.isNaturalRock && allDefsListForReading[i].building.mineableThing != null && !allDefsListForReading[i].IsSmoothed)
						{
							if (allDefsListForReading[i].building.mineableThing == this.parent)
							{
								this.sourceNaturalRockCachedValue = allDefsListForReading[i];
								break;
							}
							for (int j = 0; j < allDefsListForReading2.Count; j++)
							{
								if (allDefsListForReading2[j].IsIngredient(allDefsListForReading[i].building.mineableThing))
								{
									bool flag = false;
									for (int k = 0; k < allDefsListForReading2[j].products.Count; k++)
									{
										if (allDefsListForReading2[j].products[k].thingDef == this.parent)
										{
											flag = true;
											break;
										}
									}
									if (flag)
									{
										this.sourceNaturalRockCachedValue = allDefsListForReading[i];
										break;
									}
								}
							}
						}
					}
					this.sourceNaturalRockCached = true;
				}
				return this.sourceNaturalRockCachedValue;
			}
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x0003A424 File Offset: 0x00038624
		public bool CanMake(BuildableDef t)
		{
			if (!t.MadeFromStuff)
			{
				return false;
			}
			for (int i = 0; i < t.stuffCategories.Count; i++)
			{
				for (int j = 0; j < this.categories.Count; j++)
				{
					if (t.stuffCategories[i] == this.categories[j])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x0003A484 File Offset: 0x00038684
		public void ResolveReferencesSpecial()
		{
			if (this.appearance == null)
			{
				this.appearance = StuffAppearanceDefOf.Smooth;
			}
		}

		// Token: 0x04000746 RID: 1862
		[Unsaved(false)]
		public ThingDef parent;

		// Token: 0x04000747 RID: 1863
		[MustTranslate]
		public string stuffAdjective;

		// Token: 0x04000748 RID: 1864
		public float commonality = 1f;

		// Token: 0x04000749 RID: 1865
		public List<StuffCategoryDef> categories = new List<StuffCategoryDef>();

		// Token: 0x0400074A RID: 1866
		public bool allowedInStuffGeneration = true;

		// Token: 0x0400074B RID: 1867
		public List<StatModifier> statOffsets;

		// Token: 0x0400074C RID: 1868
		public List<StatModifier> statFactors;

		// Token: 0x0400074D RID: 1869
		public List<StatModifierQuality> statOffsetsQuality;

		// Token: 0x0400074E RID: 1870
		public List<StatModifierQuality> statFactorsQuality;

		// Token: 0x0400074F RID: 1871
		public Color color = new Color(0.8f, 0.8f, 0.8f);

		// Token: 0x04000750 RID: 1872
		public EffecterDef constructEffect;

		// Token: 0x04000751 RID: 1873
		public StuffAppearanceDef appearance;

		// Token: 0x04000752 RID: 1874
		public bool allowColorGenerators;

		// Token: 0x04000753 RID: 1875
		public bool canSuggestUseDefaultStuff;

		// Token: 0x04000754 RID: 1876
		public bool isAirtight;

		// Token: 0x04000755 RID: 1877
		public SoundDef soundImpactBullet;

		// Token: 0x04000756 RID: 1878
		public SoundDef soundImpactMelee;

		// Token: 0x04000757 RID: 1879
		public SoundDef soundMeleeHitSharp;

		// Token: 0x04000758 RID: 1880
		public SoundDef soundMeleeHitBlunt;

		// Token: 0x04000759 RID: 1881
		[Unsaved(false)]
		private bool sourceNaturalRockCached;

		// Token: 0x0400075A RID: 1882
		[Unsaved(false)]
		private ThingDef sourceNaturalRockCachedValue;
	}
}
