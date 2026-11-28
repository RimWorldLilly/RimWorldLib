using System;

namespace Verse
{
	// Token: 0x020001A3 RID: 419
	public class PawnKindLifeStage
	{
		// Token: 0x06000B90 RID: 2960 RVA: 0x0003E14C File Offset: 0x0003C34C
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
			this.untranslatedLabelMale = this.labelMale;
			this.untranslatedLabelFemale = this.labelFemale;
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0003E174 File Offset: 0x0003C374
		public void ResolveReferences()
		{
			if (this.bodyGraphicData != null && this.bodyGraphicData.graphicClass == null)
			{
				this.bodyGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.femaleGraphicData != null && this.femaleGraphicData.graphicClass == null)
			{
				this.femaleGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.dessicatedBodyGraphicData != null && this.dessicatedBodyGraphicData.graphicClass == null)
			{
				this.dessicatedBodyGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.femaleDessicatedBodyGraphicData != null && this.femaleDessicatedBodyGraphicData.graphicClass == null)
			{
				this.femaleDessicatedBodyGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.corpseGraphicData != null && this.corpseGraphicData.graphicClass == null)
			{
				this.corpseGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.femaleCorpseGraphicData != null && this.femaleCorpseGraphicData.graphicClass == null)
			{
				this.femaleCorpseGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.silhouetteGraphicData != null && this.silhouetteGraphicData.graphicClass == null)
			{
				this.silhouetteGraphicData.graphicClass = typeof(Graphic_Single);
			}
			if (this.stationaryGraphicData != null && this.stationaryGraphicData.graphicClass == null)
			{
				this.stationaryGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.femaleStationaryGraphicData != null && this.femaleStationaryGraphicData.graphicClass == null)
			{
				this.femaleStationaryGraphicData.graphicClass = typeof(Graphic_Multi);
			}
		}

		// Token: 0x04000AA8 RID: 2728
		[MustTranslate]
		public string label;

		// Token: 0x04000AA9 RID: 2729
		[MustTranslate]
		public string labelPlural;

		// Token: 0x04000AAA RID: 2730
		[MustTranslate]
		public string labelMale;

		// Token: 0x04000AAB RID: 2731
		[MustTranslate]
		public string labelMalePlural;

		// Token: 0x04000AAC RID: 2732
		[MustTranslate]
		public string labelFemale;

		// Token: 0x04000AAD RID: 2733
		[MustTranslate]
		public string labelFemalePlural;

		// Token: 0x04000AAE RID: 2734
		[Unsaved(false)]
		[TranslationHandle(Priority = 200)]
		public string untranslatedLabel;

		// Token: 0x04000AAF RID: 2735
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabelMale;

		// Token: 0x04000AB0 RID: 2736
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabelFemale;

		// Token: 0x04000AB1 RID: 2737
		public GraphicData bodyGraphicData;

		// Token: 0x04000AB2 RID: 2738
		public GraphicData femaleGraphicData;

		// Token: 0x04000AB3 RID: 2739
		public GraphicData dessicatedBodyGraphicData;

		// Token: 0x04000AB4 RID: 2740
		public GraphicData femaleDessicatedBodyGraphicData;

		// Token: 0x04000AB5 RID: 2741
		public GraphicData corpseGraphicData;

		// Token: 0x04000AB6 RID: 2742
		public GraphicData swimmingGraphicData;

		// Token: 0x04000AB7 RID: 2743
		public GraphicData femaleSwimmingGraphicData;

		// Token: 0x04000AB8 RID: 2744
		public GraphicData femaleCorpseGraphicData;

		// Token: 0x04000AB9 RID: 2745
		public GraphicData silhouetteGraphicData;

		// Token: 0x04000ABA RID: 2746
		public GraphicData rottingGraphicData;

		// Token: 0x04000ABB RID: 2747
		public GraphicData femaleRottingGraphicData;

		// Token: 0x04000ABC RID: 2748
		public GraphicData stationaryGraphicData;

		// Token: 0x04000ABD RID: 2749
		public GraphicData femaleStationaryGraphicData;

		// Token: 0x04000ABE RID: 2750
		public AnimationDef flyingAnimationEast;

		// Token: 0x04000ABF RID: 2751
		public AnimationDef flyingAnimationNorth;

		// Token: 0x04000AC0 RID: 2752
		public AnimationDef flyingAnimationSouth;

		// Token: 0x04000AC1 RID: 2753
		public AnimationDef flyingAnimationEastFemale;

		// Token: 0x04000AC2 RID: 2754
		public AnimationDef flyingAnimationNorthFemale;

		// Token: 0x04000AC3 RID: 2755
		public AnimationDef flyingAnimationSouthFemale;

		// Token: 0x04000AC4 RID: 2756
		public BodyPartToDrop butcherBodyPart;
	}
}
