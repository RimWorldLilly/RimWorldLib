using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022FD RID: 8957
	public static class OverallRainfallUtility
	{
		// Token: 0x170026A6 RID: 9894
		// (get) Token: 0x0600DD50 RID: 56656 RVA: 0x005264B0 File Offset: 0x005246B0
		public static int EnumValuesCount
		{
			get
			{
				if (OverallRainfallUtility.cachedEnumValuesCount < 0)
				{
					OverallRainfallUtility.cachedEnumValuesCount = Enum.GetNames(typeof(OverallRainfall)).Length;
				}
				return OverallRainfallUtility.cachedEnumValuesCount;
			}
		}

		// Token: 0x0600DD51 RID: 56657 RVA: 0x005264D8 File Offset: 0x005246D8
		public static SimpleCurve GetRainfallCurve(this OverallRainfall overallRainfall)
		{
			switch (overallRainfall)
			{
			case OverallRainfall.AlmostNone:
				return OverallRainfallUtility.Curve_AlmostNone;
			case OverallRainfall.Little:
				return OverallRainfallUtility.Curve_Little;
			case OverallRainfall.LittleBitLess:
				return OverallRainfallUtility.Curve_LittleBitLess;
			case OverallRainfall.LittleBitMore:
				return OverallRainfallUtility.Curve_LittleBitMore;
			case OverallRainfall.High:
				return OverallRainfallUtility.Curve_High;
			case OverallRainfall.VeryHigh:
				return OverallRainfallUtility.Curve_VeryHigh;
			}
			return null;
		}

		// Token: 0x04008DF2 RID: 36338
		private static int cachedEnumValuesCount = -1;

		// Token: 0x04008DF3 RID: 36339
		private static readonly SimpleCurve Curve_AlmostNone = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1500f, 120f),
				true
			},
			{
				new CurvePoint(3500f, 180f),
				true
			},
			{
				new CurvePoint(6000f, 200f),
				true
			},
			{
				new CurvePoint(12000f, 250f),
				true
			}
		};

		// Token: 0x04008DF4 RID: 36340
		private static readonly SimpleCurve Curve_Little = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1500f, 300f),
				true
			},
			{
				new CurvePoint(6000f, 1100f),
				true
			},
			{
				new CurvePoint(12000f, 1400f),
				true
			}
		};

		// Token: 0x04008DF5 RID: 36341
		private static readonly SimpleCurve Curve_LittleBitLess = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1000f, 700f),
				true
			},
			{
				new CurvePoint(5000f, 4700f),
				true
			},
			{
				new CurvePoint(12000f, 12000f),
				true
			},
			{
				new CurvePoint(99999f, 99999f),
				true
			}
		};

		// Token: 0x04008DF6 RID: 36342
		private static readonly SimpleCurve Curve_LittleBitMore = new SimpleCurve
		{
			{
				new CurvePoint(0f, 50f),
				true
			},
			{
				new CurvePoint(5000f, 5300f),
				true
			},
			{
				new CurvePoint(12000f, 12000f),
				true
			},
			{
				new CurvePoint(99999f, 99999f),
				true
			}
		};

		// Token: 0x04008DF7 RID: 36343
		private static readonly SimpleCurve Curve_High = new SimpleCurve
		{
			{
				new CurvePoint(0f, 500f),
				true
			},
			{
				new CurvePoint(150f, 950f),
				true
			},
			{
				new CurvePoint(500f, 2000f),
				true
			},
			{
				new CurvePoint(1000f, 2800f),
				true
			},
			{
				new CurvePoint(5000f, 6000f),
				true
			},
			{
				new CurvePoint(12000f, 12000f),
				true
			},
			{
				new CurvePoint(99999f, 99999f),
				true
			}
		};

		// Token: 0x04008DF8 RID: 36344
		private static readonly SimpleCurve Curve_VeryHigh = new SimpleCurve
		{
			{
				new CurvePoint(0f, 750f),
				true
			},
			{
				new CurvePoint(125f, 2000f),
				true
			},
			{
				new CurvePoint(500f, 3000f),
				true
			},
			{
				new CurvePoint(1000f, 3800f),
				true
			},
			{
				new CurvePoint(5000f, 7500f),
				true
			},
			{
				new CurvePoint(12000f, 12000f),
				true
			},
			{
				new CurvePoint(99999f, 99999f),
				true
			}
		};
	}
}
