using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FFF RID: 4095
	public struct ThingSetMakerParams
	{
		// Token: 0x04004107 RID: 16647
		public TechLevel? techLevel;

		// Token: 0x04004108 RID: 16648
		public IntRange? countRange;

		// Token: 0x04004109 RID: 16649
		public ThingFilter filter;

		// Token: 0x0400410A RID: 16650
		public Predicate<ThingDef> validator;

		// Token: 0x0400410B RID: 16651
		public QualityGenerator? qualityGenerator;

		// Token: 0x0400410C RID: 16652
		public float? maxTotalMass;

		// Token: 0x0400410D RID: 16653
		public float? maxThingMarketValue;

		// Token: 0x0400410E RID: 16654
		public bool? allowNonStackableDuplicates;

		// Token: 0x0400410F RID: 16655
		public float? minSingleItemMarketValuePct;

		// Token: 0x04004110 RID: 16656
		public FloatRange? totalMarketValueRange;

		// Token: 0x04004111 RID: 16657
		public FloatRange? totalNutritionRange;

		// Token: 0x04004112 RID: 16658
		public PodContentsType? podContentsType;

		// Token: 0x04004113 RID: 16659
		public Faction makingFaction;

		// Token: 0x04004114 RID: 16660
		public TraderKindDef traderDef;

		// Token: 0x04004115 RID: 16661
		public PlanetTile? tile;

		// Token: 0x04004116 RID: 16662
		public Dictionary<string, object> custom;
	}
}
