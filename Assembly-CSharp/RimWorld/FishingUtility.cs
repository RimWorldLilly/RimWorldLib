using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020020A2 RID: 8354
	public static class FishingUtility
	{
		// Token: 0x0600D290 RID: 53904 RVA: 0x004D5814 File Offset: 0x004D3A14
		public static string FishPopulationLabel(float population)
		{
			if (population < 50f)
			{
				return "FishPopulation_VeryLow".Translate();
			}
			if (population < 200f)
			{
				return "FishPopulation_Low".Translate();
			}
			if (population < 300f)
			{
				return "FishPopulation_Moderate".Translate();
			}
			return "FishPopulation_Healthy".Translate();
		}

		// Token: 0x0600D291 RID: 53905 RVA: 0x004D5878 File Offset: 0x004D3A78
		public static WaterBody GetWaterBody(this IntVec3 c, Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return null;
			}
			return map.waterBodyTracker.WaterBodyAt(c);
		}

		// Token: 0x0600D292 RID: 53906 RVA: 0x004D5890 File Offset: 0x004D3A90
		public static List<NegativeFishingOutcomeDef> GetNegativeFishingOutcomes(Pawn pawn, IntVec3 cell)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return null;
			}
			FishingUtility.tmpNegativeCatches.Clear();
			if (!DebugSettings.alwaysNegativeCatches)
			{
				if (!Rand.Chance(0.02f))
				{
					return FishingUtility.tmpNegativeCatches;
				}
				if (pawn.Map.waterBodyTracker.lastNegativeCatchTick != 0 && GenTicks.TicksGame - pawn.Map.waterBodyTracker.lastNegativeCatchTick <= 300000)
				{
					return FishingUtility.tmpNegativeCatches;
				}
			}
			WaterBody waterBody = cell.GetWaterBody(pawn.Map);
			foreach (NegativeFishingOutcomeDef negativeFishingOutcomeDef in DefDatabase<NegativeFishingOutcomeDef>.AllDefsListForReading)
			{
				if (waterBody.CommonFish.Contains(negativeFishingOutcomeDef.fishType) || waterBody.UncommonFish.Contains(negativeFishingOutcomeDef.fishType))
				{
					FishingUtility.tmpNegativeCatches.Add(negativeFishingOutcomeDef);
				}
			}
			return FishingUtility.tmpNegativeCatches;
		}

		// Token: 0x0600D293 RID: 53907 RVA: 0x004D5980 File Offset: 0x004D3B80
		public static List<Thing> GetCatchesFor(Pawn pawn, IntVec3 cell, bool animalFishing, out bool rare)
		{
			FishingUtility.tmpCatches.Clear();
			rare = false;
			if (!ModsConfig.OdysseyActive)
			{
				return FishingUtility.tmpCatches;
			}
			WaterBody waterBody = cell.GetWaterBody(pawn.Map);
			if (waterBody == null || waterBody.waterBodyType == WaterBodyType.None)
			{
				return FishingUtility.tmpCatches;
			}
			ThingDef def;
			if (Rand.Chance(FishingUtility.PollutionToxfishChanceCurve.Evaluate(waterBody.PollutionPct)))
			{
				def = ThingDefOf.Fish_Toxfish;
				rare = false;
			}
			else
			{
				if (!animalFishing && pawn.Map.Biome.fishTypes.rareCatchesSetMaker != null && (DebugSettings.alwaysRareCatches || pawn.Map.waterBodyTracker.lastRareCatchTick == 0 || GenTicks.TicksGame - pawn.Map.waterBodyTracker.lastRareCatchTick > 300000) && Rand.Chance(0.01f))
				{
					FishingUtility.tmpCatches.AddRange(pawn.Map.Biome.fishTypes.rareCatchesSetMaker.root.Generate());
					if (FishingUtility.tmpCatches.Any<Thing>())
					{
						rare = true;
						return FishingUtility.tmpCatches;
					}
				}
				rare = false;
				ThingDef thingDef;
				if ((animalFishing || !Rand.Chance(0.05f) || !waterBody.UncommonFish.TryRandomElement(out thingDef)) && !waterBody.CommonFishIncludingExtras.TryRandomElement(out thingDef))
				{
					return FishingUtility.tmpCatches;
				}
				def = thingDef;
			}
			float x = pawn.Map.waterBodyTracker.FishPopulationAt(cell);
			float num;
			if (animalFishing)
			{
				float statValueAbstract = def.GetStatValueAbstract(StatDefOf.Nutrition, null);
				num = Mathf.Ceil((float)FoodUtility.WillIngestStackCountOf(pawn, def, statValueAbstract));
			}
			else
			{
				num = Mathf.Min(new float[]
				{
					FishingUtility.PopulationToFishYieldCurve.Evaluate(x) * pawn.GetStatValue(StatDefOf.FishingYield, true, -1)
				});
			}
			float num2 = pawn.Map.waterBodyTracker.FishPopulationAt(cell);
			if (num > num2)
			{
				num = num2;
			}
			int stackCount = Mathf.Max(1, Mathf.RoundToInt(num));
			Thing thing = ThingMaker.MakeThing(def, null);
			thing.stackCount = stackCount;
			FishingUtility.tmpCatches.Add(thing);
			return FishingUtility.tmpCatches;
		}

		// Token: 0x0400855E RID: 34142
		public const float FishPopulationRegenerationFactorPerDay = 0.025f;

		// Token: 0x0400855F RID: 34143
		public const int BaseFishingDurationTicks = 7500;

		// Token: 0x04008560 RID: 34144
		public const int AnimalFishingDurationTicks = 1500;

		// Token: 0x04008561 RID: 34145
		public const float ChanceToCatchUncommonFish = 0.05f;

		// Token: 0x04008562 RID: 34146
		public const float ChanceForRareCatch = 0.01f;

		// Token: 0x04008563 RID: 34147
		private const int RareCatchCooldownTicks = 300000;

		// Token: 0x04008564 RID: 34148
		public const float ChanceForNegativeCatch = 0.02f;

		// Token: 0x04008565 RID: 34149
		private const int NegativeCatchCooldownTicks = 120000;

		// Token: 0x04008566 RID: 34150
		public const int MinFishPopulationForFishing = 10;

		// Token: 0x04008567 RID: 34151
		public const int MinFishPopulationPerWaterBody = 20;

		// Token: 0x04008568 RID: 34152
		public static readonly SimpleCurve FishPopulationFactorPerBodySizeCurve = new SimpleCurve
		{
			{
				new CurvePoint(74.9f, 0f),
				true
			},
			{
				new CurvePoint(75f, 0.03f),
				true
			},
			{
				new CurvePoint(400f, 0.04f),
				true
			},
			{
				new CurvePoint(1600f, 0.1f),
				true
			},
			{
				new CurvePoint(3200f, 0.24f),
				true
			},
			{
				new CurvePoint(6000f, 0.8f),
				true
			},
			{
				new CurvePoint(20000f, 1f),
				true
			}
		};

		// Token: 0x04008569 RID: 34153
		public static readonly SimpleCurve ChanceForCommonFishFromWaterBodySizeCurve = new SimpleCurve
		{
			{
				new CurvePoint(74.9f, 0f),
				true
			},
			{
				new CurvePoint(75f, 0.05f),
				true
			},
			{
				new CurvePoint(100f, 0.5f),
				true
			},
			{
				new CurvePoint(200f, 1f),
				true
			}
		};

		// Token: 0x0400856A RID: 34154
		public static readonly SimpleCurve ChanceForUncommonFishFromWaterBodySizeCurve = new SimpleCurve
		{
			{
				new CurvePoint(399.9f, 0f),
				true
			},
			{
				new CurvePoint(400f, 0.1f),
				true
			},
			{
				new CurvePoint(1600f, 0.25f),
				true
			},
			{
				new CurvePoint(6000f, 0.7f),
				true
			}
		};

		// Token: 0x0400856B RID: 34155
		public static readonly SimpleCurve PopulationToFishYieldCurve = new SimpleCurve
		{
			{
				new CurvePoint(25f, 1f),
				true
			},
			{
				new CurvePoint(100f, 3f),
				true
			},
			{
				new CurvePoint(300f, 6f),
				true
			}
		};

		// Token: 0x0400856C RID: 34156
		public static readonly SimpleCurve PollutionToxfishChanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.05f, 0f),
				true
			},
			{
				new CurvePoint(0.8f, 0.6f),
				true
			}
		};

		// Token: 0x0400856D RID: 34157
		private static List<NegativeFishingOutcomeDef> tmpNegativeCatches = new List<NegativeFishingOutcomeDef>();

		// Token: 0x0400856E RID: 34158
		private static List<Thing> tmpCatches = new List<Thing>();
	}
}
