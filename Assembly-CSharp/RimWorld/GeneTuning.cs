using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020020A3 RID: 8355
	public static class GeneTuning
	{
		// Token: 0x0400856F RID: 34159
		public static readonly SimpleCurve ComplexityToCreationHoursCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 3f),
				true
			},
			{
				new CurvePoint(4f, 5f),
				true
			},
			{
				new CurvePoint(8f, 8f),
				true
			},
			{
				new CurvePoint(12f, 12f),
				true
			},
			{
				new CurvePoint(16f, 17f),
				true
			},
			{
				new CurvePoint(20f, 23f),
				true
			}
		};

		// Token: 0x04008570 RID: 34160
		public static readonly SimpleCurve MetabolismToFoodConsumptionFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(-5f, 2.25f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(5f, 0.5f),
				true
			}
		};

		// Token: 0x04008571 RID: 34161
		public static readonly IntRange BiostatRange = new IntRange(-5, 5);

		// Token: 0x04008572 RID: 34162
		public const int BaseMaxComplexity = 6;

		// Token: 0x04008573 RID: 34163
		public const float ChemicalDependencyDurationDays_Deficiency = 5f;

		// Token: 0x04008574 RID: 34164
		public const float ChemicalDependencyDurationDays_Coma = 30f;

		// Token: 0x04008575 RID: 34165
		public const float ChemicalDependencyDurationDays_Death = 60f;

		// Token: 0x04008576 RID: 34166
		public const float HemogenGainPerCannibalNutrition = 0.0375f;

		// Token: 0x04008577 RID: 34167
		public static readonly FloatRange GeneExtractorRegrowingDurationDaysRange = new FloatRange(12f, 20f);

		// Token: 0x04008578 RID: 34168
		public static readonly FloatRange SkinColorValueRange = new FloatRange(0.23f, 0.94f);

		// Token: 0x04008579 RID: 34169
		public static readonly FloatRange HairColorValueRange = new FloatRange(0.1f, 0.98f);
	}
}
