using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005CE RID: 1486
	public static class DebugOutputsMisc
	{
		// Token: 0x06002ECB RID: 11979 RVA: 0x00124FCC File Offset: 0x001231CC
		[DebugOutput]
		public static void DrawerTypes()
		{
			IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[6];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category.ToString());
			array[2] = new TableDataGetter<ThingDef>("drawType", (ThingDef d) => d.drawerType.ToString());
			array[3] = new TableDataGetter<ThingDef>("thingClass", (ThingDef d) => d.thingClass.Name);
			array[4] = new TableDataGetter<ThingDef>("draw() users (comps)", (ThingDef d) => DebugOutputsMisc.<DrawerTypes>g__GetDrawUsers|0_0(d));
			array[5] = new TableDataGetter<ThingDef>("potential issue", delegate(ThingDef d)
			{
				if (d.drawerType != DrawerType.MapMeshOnly || string.IsNullOrWhiteSpace(DebugOutputsMisc.<DrawerTypes>g__GetDrawUsers|0_0(d)))
				{
					return "No";
				}
				return "Yes";
			});
			DebugTables.MakeTablesDialog<ThingDef>(allDefs, array);
		}

		// Token: 0x06002ECC RID: 11980 RVA: 0x001250F4 File Offset: 0x001232F4
		[DebugOutput]
		public static void DynamicDrawnThingsNow()
		{
			IEnumerable<IGrouping<ThingDef, Thing>> dataSources = (from t in Find.CurrentMap.dynamicDrawManager.DrawThings
			group t by t.def).ToList<IGrouping<ThingDef, Thing>>();
			TableDataGetter<IGrouping<ThingDef, Thing>>[] array = new TableDataGetter<IGrouping<ThingDef, Thing>>[2];
			array[0] = new TableDataGetter<IGrouping<ThingDef, Thing>>("defName", (IGrouping<ThingDef, Thing> d) => d.Key.defName);
			array[1] = new TableDataGetter<IGrouping<ThingDef, Thing>>("count", (IGrouping<ThingDef, Thing> d) => d.Count<Thing>());
			DebugTables.MakeTablesDialog<IGrouping<ThingDef, Thing>>(dataSources, array);
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x0012519C File Offset: 0x0012339C
		[DebugOutput]
		public static void DynamicDrawnThingsByCategoryNow()
		{
			IEnumerable<IGrouping<ThingCategory, Thing>> dataSources = (from t in Find.CurrentMap.dynamicDrawManager.DrawThings
			group t by t.def.category).ToList<IGrouping<ThingCategory, Thing>>();
			TableDataGetter<IGrouping<ThingCategory, Thing>>[] array = new TableDataGetter<IGrouping<ThingCategory, Thing>>[2];
			array[0] = new TableDataGetter<IGrouping<ThingCategory, Thing>>("category", (IGrouping<ThingCategory, Thing> d) => d.Key.ToString());
			array[1] = new TableDataGetter<IGrouping<ThingCategory, Thing>>("count", (IGrouping<ThingCategory, Thing> d) => d.Count<Thing>());
			DebugTables.MakeTablesDialog<IGrouping<ThingCategory, Thing>>(dataSources, array);
		}

		// Token: 0x06002ECE RID: 11982 RVA: 0x00125244 File Offset: 0x00123444
		[DebugOutput]
		public static void MiningResourceGeneration()
		{
			Func<ThingDef, ThingDef> mineable = delegate(ThingDef d)
			{
				List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].building != null && allDefsListForReading[i].building.mineableThing == d)
					{
						return allDefsListForReading[i];
					}
				}
				return null;
			};
			Func<ThingDef, float> mineableCommonality = delegate(ThingDef d)
			{
				if (mineable(d) != null)
				{
					return mineable(d).building.mineableScatterCommonality;
				}
				return 0f;
			};
			Func<ThingDef, IntRange> mineableLumpSizeRange = delegate(ThingDef d)
			{
				if (mineable(d) != null)
				{
					return mineable(d).building.mineableScatterLumpSizeRange;
				}
				return IntRange.Zero;
			};
			Func<ThingDef, float> mineableYield = delegate(ThingDef d)
			{
				if (mineable(d) != null)
				{
					return (float)mineable(d).building.EffectiveMineableYield;
				}
				return 0f;
			};
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.deepCommonality > 0f || mineableCommonality(d) > 0f
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[14];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("market value", (ThingDef d) => d.BaseMarketValue.ToString("F2"));
			array[2] = new TableDataGetter<ThingDef>("stackLimit", (ThingDef d) => d.stackLimit);
			array[3] = new TableDataGetter<ThingDef>("deep\ncommonality", (ThingDef d) => d.deepCommonality.ToString("F2"));
			array[4] = new TableDataGetter<ThingDef>("deep\nlump size", (ThingDef d) => d.deepLumpSizeRange);
			array[5] = new TableDataGetter<ThingDef>("deep lump\nvalue min", (ThingDef d) => ((float)d.deepLumpSizeRange.min * d.BaseMarketValue * (float)d.deepCountPerCell).ToStringMoney(null));
			array[6] = new TableDataGetter<ThingDef>("deep lump\nvalue avg", (ThingDef d) => (d.deepLumpSizeRange.Average * d.BaseMarketValue * (float)d.deepCountPerCell).ToStringMoney(null));
			array[7] = new TableDataGetter<ThingDef>("deep lump\nvalue max", (ThingDef d) => ((float)d.deepLumpSizeRange.max * d.BaseMarketValue * (float)d.deepCountPerCell).ToStringMoney(null));
			array[8] = new TableDataGetter<ThingDef>("deep count\nper cell", (ThingDef d) => d.deepCountPerCell);
			array[9] = new TableDataGetter<ThingDef>("deep count\nper portion", (ThingDef d) => d.deepCountPerPortion);
			array[10] = new TableDataGetter<ThingDef>("deep portion\nvalue", (ThingDef d) => ((float)d.deepCountPerPortion * d.BaseMarketValue).ToStringMoney(null));
			array[11] = new TableDataGetter<ThingDef>("mineable\ncommonality", (ThingDef d) => mineableCommonality(d).ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("mineable\nlump size", (ThingDef d) => mineableLumpSizeRange(d));
			array[13] = new TableDataGetter<ThingDef>("mineable yield\nper cell", (ThingDef d) => mineableYield(d));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002ECF RID: 11983 RVA: 0x00125508 File Offset: 0x00123708
		[DebugOutput]
		public static void NaturalRocks()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building && (d.building.isNaturalRock || d.building.isResourceRock) && !d.IsSmoothed
			select d into x
			orderby x.building.isNaturalRock descending, x.building.isResourceRock descending
			select x;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[9];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("isNaturalRock", (ThingDef d) => d.building.isNaturalRock.ToStringCheckBlank());
			array[2] = new TableDataGetter<ThingDef>("isResourceRock", (ThingDef d) => d.building.isResourceRock.ToStringCheckBlank());
			array[3] = new TableDataGetter<ThingDef>("smoothed", delegate(ThingDef d)
			{
				if (d.building.smoothedThing == null)
				{
					return "";
				}
				return d.building.smoothedThing.defName;
			});
			array[4] = new TableDataGetter<ThingDef>("mineableThing", delegate(ThingDef d)
			{
				if (d.building.mineableThing == null)
				{
					return "";
				}
				return d.building.mineableThing.defName;
			});
			array[5] = new TableDataGetter<ThingDef>("mineableYield", (ThingDef d) => d.building.EffectiveMineableYield);
			array[6] = new TableDataGetter<ThingDef>("mineableYieldWasteable", (ThingDef d) => d.building.mineableYieldWasteable);
			array[7] = new TableDataGetter<ThingDef>("NaturalRockType\never possible", (ThingDef d) => d.IsNonResourceNaturalRock.ToStringCheckBlank());
			array[8] = new TableDataGetter<ThingDef>("NaturalRockType\nin CurrentMap", delegate(ThingDef d)
			{
				if (Find.CurrentMap == null)
				{
					return "";
				}
				return Find.World.NaturalRockTypesIn(Find.CurrentMap.Tile).Contains(d).ToStringCheckBlank();
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002ED0 RID: 11984 RVA: 0x00125720 File Offset: 0x00123920
		[DebugOutput]
		public static void AncientJunk()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.defName.StartsWith("Ancient")
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("edifice", (ThingDef d) => d.IsEdifice());
			array[2] = new TableDataGetter<ThingDef>("scatterers", delegate(ThingDef d)
			{
				string text = null;
				foreach (GenStepDef genStepDef in DefDatabase<GenStepDef>.AllDefsListForReading)
				{
					GenStep_ScatterThings genStep_ScatterThings = genStepDef.genStep as GenStep_ScatterThings;
					if (genStep_ScatterThings != null && genStep_ScatterThings.thingDef == d)
					{
						if (text != null)
						{
							text += ", ";
						}
						text = text + "ScatterThings " + genStepDef.defName;
					}
					GenStep_ScatterGroup genStep_ScatterGroup = genStepDef.genStep as GenStep_ScatterGroup;
					if (genStep_ScatterGroup != null)
					{
						string text2 = null;
						for (int i = 0; i < genStep_ScatterGroup.groups.Count; i++)
						{
							GenStep_ScatterGroup.ScatterGroup scatterGroup = genStep_ScatterGroup.groups[i];
							for (int j = 0; j < scatterGroup.things.Count; j++)
							{
								GenStep_ScatterGroup.ThingWeight thingWeight = scatterGroup.things[j];
								if (thingWeight.thing == d)
								{
									if (text2 != null)
									{
										text2 += ", ";
									}
									text2 = text2 + i.ToString() + "|" + thingWeight.weight.ToString();
								}
							}
						}
						if (text2 != null)
						{
							if (text != null)
							{
								text += ", ";
							}
							text = string.Concat(new string[]
							{
								text,
								"ScatterGroup ",
								genStepDef.defName,
								" {",
								text2,
								"}"
							});
						}
					}
				}
				return text;
			});
			array[3] = new TableDataGetter<ThingDef>("rotatable", (ThingDef d) => d.rotatable);
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002ED1 RID: 11985 RVA: 0x00125814 File Offset: 0x00123A14
		[DebugOutput]
		public static void MeditationFoci()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.StatBaseDefined(StatDefOf.MeditationFocusStrength)
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[13];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("base", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MeditationFocusStrength, null).ToStringPercent());
			array[2] = new TableDataGetter<ThingDef>("max\ntotal", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__TotalMax|6_3(d).ToStringPercent());
			array[3] = new TableDataGetter<ThingDef>("offset 0\nname", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetClassName|6_1(d, 0));
			array[4] = new TableDataGetter<ThingDef>("offset 0\nmax", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetMax|6_2(d, 0).ToStringPercentEmptyZero("F0"));
			array[5] = new TableDataGetter<ThingDef>("offset 1\nname", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetClassName|6_1(d, 1));
			array[6] = new TableDataGetter<ThingDef>("offset 1\nmax", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetMax|6_2(d, 1).ToStringPercentEmptyZero("F0"));
			array[7] = new TableDataGetter<ThingDef>("offset 2\nname", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetClassName|6_1(d, 2));
			array[8] = new TableDataGetter<ThingDef>("offset 2\nmax", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetMax|6_2(d, 2).ToStringPercentEmptyZero("F0"));
			array[9] = new TableDataGetter<ThingDef>("offset 3\nname", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetClassName|6_1(d, 3));
			array[10] = new TableDataGetter<ThingDef>("offset 3\nmax", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetMax|6_2(d, 3).ToStringPercentEmptyZero("F0"));
			array[11] = new TableDataGetter<ThingDef>("offset 4\nname", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetClassName|6_1(d, 4));
			array[12] = new TableDataGetter<ThingDef>("offset 4\nmax", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetMax|6_2(d, 4).ToStringPercentEmptyZero("F0"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002ED2 RID: 11986 RVA: 0x00125A98 File Offset: 0x00123C98
		[DebugOutput]
		public static void DefaultStuffs()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.MadeFromStuff && !d.IsFrame
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
			array[0] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category.ToString());
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("default stuff", (ThingDef d) => GenStuff.DefaultStuffFor(d).defName);
			array[3] = new TableDataGetter<ThingDef>("stuff categories", (ThingDef d) => (from c in d.stuffCategories
			select c.defName).ToCommaList(false, false));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002ED3 RID: 11987 RVA: 0x00125B8C File Offset: 0x00123D8C
		[DebugOutput]
		public static void Beauties()
		{
			IEnumerable<BuildableDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>()).Where(delegate(BuildableDef d)
			{
				ThingDef thingDef = d as ThingDef;
				if (thingDef != null)
				{
					return BeautyUtility.BeautyRelevant(thingDef.category);
				}
				return d is TerrainDef;
			})
			orderby (int)d.GetStatValueAbstract(StatDefOf.Beauty, null) descending
			select d;
			TableDataGetter<BuildableDef>[] array = new TableDataGetter<BuildableDef>[7];
			array[0] = new TableDataGetter<BuildableDef>("category", delegate(BuildableDef d)
			{
				if (!(d is ThingDef))
				{
					return "Terrain";
				}
				return ((ThingDef)d).category.ToString();
			});
			array[1] = new TableDataGetter<BuildableDef>("defName", (BuildableDef d) => d.defName);
			array[2] = new TableDataGetter<BuildableDef>("beauty", (BuildableDef d) => d.GetStatValueAbstract(StatDefOf.Beauty, null).ToString());
			array[3] = new TableDataGetter<BuildableDef>("beauty outdoors", delegate(BuildableDef d)
			{
				if (!d.StatBaseDefined(StatDefOf.BeautyOutdoors))
				{
					return "";
				}
				return d.GetStatValueAbstract(StatDefOf.BeautyOutdoors, null).ToString();
			});
			array[4] = new TableDataGetter<BuildableDef>("market value", (BuildableDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F1"));
			array[5] = new TableDataGetter<BuildableDef>("work to produce", (BuildableDef d) => DebugOutputsEconomy.WorkToProduceBest(d).ToString());
			array[6] = new TableDataGetter<BuildableDef>("beauty per market value", delegate(BuildableDef d)
			{
				if (d.GetStatValueAbstract(StatDefOf.Beauty, null) <= 0f)
				{
					return "";
				}
				return (d.GetStatValueAbstract(StatDefOf.Beauty, null) / d.GetStatValueAbstract(StatDefOf.MarketValue, null)).ToString("F5");
			});
			DebugTables.MakeTablesDialog<BuildableDef>(dataSources, array);
		}

		// Token: 0x06002ED4 RID: 11988 RVA: 0x00125D3C File Offset: 0x00123F3C
		[DebugOutput]
		public static void StuffBeauty()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsStuff
			orderby DebugOutputsMisc.<StuffBeauty>g__getStatFactorVal|9_0(d, StatDefOf.Beauty) descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[5];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("beauty factor", (ThingDef d) => DebugOutputsMisc.<StuffBeauty>g__getStatFactorVal|9_0(d, StatDefOf.Beauty).ToString());
			array[2] = new TableDataGetter<ThingDef>("beauty offset", (ThingDef d) => DebugOutputsMisc.<StuffBeauty>g__getStatOffsetVal|9_1(d, StatDefOf.Beauty).ToString());
			array[3] = new TableDataGetter<ThingDef>("market value", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F1"));
			array[4] = new TableDataGetter<ThingDef>("beauty factor per market value", delegate(ThingDef d)
			{
				if (DebugOutputsMisc.<StuffBeauty>g__getStatFactorVal|9_0(d, StatDefOf.Beauty) <= 0f)
				{
					return "";
				}
				return (DebugOutputsMisc.<StuffBeauty>g__getStatFactorVal|9_0(d, StatDefOf.Beauty) / d.GetStatValueAbstract(StatDefOf.MarketValue, null)).ToString("F5");
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002ED5 RID: 11989 RVA: 0x00125E80 File Offset: 0x00124080
		[DebugOutput]
		public static void ThingsPowerAndHeat()
		{
			Func<ThingDef, CompProperties_HeatPusher> heatPusher = delegate(ThingDef d)
			{
				if (d.comps == null)
				{
					return null;
				}
				for (int i = 0; i < d.comps.Count; i++)
				{
					CompProperties_HeatPusher compProperties_HeatPusher = d.comps[i] as CompProperties_HeatPusher;
					if (compProperties_HeatPusher != null)
					{
						return compProperties_HeatPusher;
					}
				}
				return null;
			};
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Building || d.GetCompProperties<CompProperties_Power>() != null || heatPusher(d) != null) && !d.IsFrame
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[10];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("base\npower consumption", delegate(ThingDef d)
			{
				if (d.GetCompProperties<CompProperties_Power>() != null)
				{
					return d.GetCompProperties<CompProperties_Power>().PowerConsumption.ToString();
				}
				return "";
			});
			array[2] = new TableDataGetter<ThingDef>("short circuit\nin rain", delegate(ThingDef d)
			{
				if (d.GetCompProperties<CompProperties_Power>() == null)
				{
					return "";
				}
				if (!d.GetCompProperties<CompProperties_Power>().shortCircuitInRain)
				{
					return "";
				}
				return "rainfire";
			});
			array[3] = new TableDataGetter<ThingDef>("transmits\npower", delegate(ThingDef d)
			{
				if (d.GetCompProperties<CompProperties_Power>() == null)
				{
					return "";
				}
				if (!d.GetCompProperties<CompProperties_Power>().transmitsPower)
				{
					return "";
				}
				return "transmit";
			});
			array[4] = new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue);
			array[5] = new TableDataGetter<ThingDef>("cost list", (ThingDef d) => DebugOutputsEconomy.CostListString(d, true, false));
			array[6] = new TableDataGetter<ThingDef>("heat pusher\ncompClass", delegate(ThingDef d)
			{
				if (heatPusher(d) != null)
				{
					return heatPusher(d).compClass.ToString();
				}
				return "";
			});
			array[7] = new TableDataGetter<ThingDef>("heat pusher\nheat per sec", delegate(ThingDef d)
			{
				if (heatPusher(d) != null)
				{
					return heatPusher(d).heatPerSecond.ToString();
				}
				return "";
			});
			array[8] = new TableDataGetter<ThingDef>("heat pusher\nmin temp", delegate(ThingDef d)
			{
				if (heatPusher(d) != null)
				{
					return heatPusher(d).heatPushMinTemperature.ToStringTemperature("F1");
				}
				return "";
			});
			array[9] = new TableDataGetter<ThingDef>("heat pusher\nmax temp", delegate(ThingDef d)
			{
				if (heatPusher(d) != null)
				{
					return heatPusher(d).heatPushMaxTemperature.ToStringTemperature("F1");
				}
				return "";
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002ED6 RID: 11990 RVA: 0x00126048 File Offset: 0x00124248
		[DebugOutput]
		public static void FoodPoisonChances()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsIngestible
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[3];
			array[0] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category);
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("food poison chance", delegate(ThingDef d)
			{
				if (d.GetCompProperties<CompProperties_FoodPoisonable>() != null)
				{
					return "poisonable by cook";
				}
				float statValueAbstract = d.GetStatValueAbstract(StatDefOf.FoodPoisonChanceFixedHuman, null);
				if (statValueAbstract != 0f)
				{
					return statValueAbstract.ToStringPercent();
				}
				return "";
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002ED7 RID: 11991 RVA: 0x00126110 File Offset: 0x00124310
		[DebugOutput]
		public static void TechLevels()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building || d.category == ThingCategory.Item
			where !d.IsFrame && (d.building == null || !d.building.isNaturalRock)
			orderby (int)d.techLevel descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[3];
			array[0] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category.ToString());
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("tech level", (ThingDef d) => d.techLevel.ToString());
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002ED8 RID: 11992 RVA: 0x00126220 File Offset: 0x00124420
		[DebugOutput]
		public static void Stuffs()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsStuff
			orderby d.BaseMarketValue
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[26];
			array[0] = new TableDataGetter<ThingDef>("fabric", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Fabric).ToStringCheckBlank());
			array[1] = new TableDataGetter<ThingDef>("leather", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Leathery).ToStringCheckBlank());
			array[2] = new TableDataGetter<ThingDef>("metal", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Metallic).ToStringCheckBlank());
			array[3] = new TableDataGetter<ThingDef>("stony", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Stony).ToStringCheckBlank());
			array[4] = new TableDataGetter<ThingDef>("woody", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Woody).ToStringCheckBlank());
			array[5] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[6] = new TableDataGetter<ThingDef>("burnable", (ThingDef d) => d.burnableByRecipe.ToStringCheckBlank());
			array[7] = new TableDataGetter<ThingDef>("smeltable", (ThingDef d) => d.smeltable.ToStringCheckBlank());
			array[8] = new TableDataGetter<ThingDef>("base\nmarket\nvalue", (ThingDef d) => d.BaseMarketValue.ToStringMoney(null));
			array[9] = new TableDataGetter<ThingDef>("melee\ncooldown\nmultiplier", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|13_0(d, StatDefOf.MeleeWeapon_CooldownMultiplier));
			array[10] = new TableDataGetter<ThingDef>("melee\nsharp\ndamage\nmultiplier", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier, null).ToString("F2"));
			array[11] = new TableDataGetter<ThingDef>("melee\nsharp\ndps factor\noverall", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__meleeDpsSharpFactorOverall|13_1(d).ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("melee\nblunt\ndamage\nmultiplier", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.BluntDamageMultiplier, null).ToString("F2"));
			array[13] = new TableDataGetter<ThingDef>("melee\nblunt\ndps factor\noverall", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__meleeDpsBluntFactorOverall|13_2(d).ToString("F2"));
			array[14] = new TableDataGetter<ThingDef>("armor power\nsharp", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Sharp, null).ToString("F2"));
			array[15] = new TableDataGetter<ThingDef>("armor power\nblunt", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Blunt, null).ToString("F2"));
			array[16] = new TableDataGetter<ThingDef>("armor power\nheat", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Heat, null).ToString("F2"));
			array[17] = new TableDataGetter<ThingDef>("insulation\ncold", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Cold, null).ToString("F2"));
			array[18] = new TableDataGetter<ThingDef>("insulation\nheat", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Heat, null).ToString("F2"));
			array[19] = new TableDataGetter<ThingDef>("flammability", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Flammability, null).ToString("F2"));
			array[20] = new TableDataGetter<ThingDef>("factor\nFlammability", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|13_0(d, StatDefOf.Flammability));
			array[21] = new TableDataGetter<ThingDef>("factor\nWorkToMake", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|13_0(d, StatDefOf.WorkToMake));
			array[22] = new TableDataGetter<ThingDef>("factor\nWorkToBuild", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|13_0(d, StatDefOf.WorkToBuild));
			array[23] = new TableDataGetter<ThingDef>("factor\nMaxHp", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|13_0(d, StatDefOf.MaxHitPoints));
			array[24] = new TableDataGetter<ThingDef>("factor\nBeauty", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|13_0(d, StatDefOf.Beauty));
			array[25] = new TableDataGetter<ThingDef>("factor\nDoorspeed", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|13_0(d, StatDefOf.DoorOpenSpeed));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002ED9 RID: 11993 RVA: 0x00126710 File Offset: 0x00124910
		[DebugOutput]
		public static void BurningAndSmeltingThings()
		{
			List<RecipeDef> burnRecipes = new List<RecipeDef>();
			foreach (RecipeDef recipeDef in DefDatabase<RecipeDef>.AllDefsListForReading)
			{
				if (recipeDef.defName.Substring(0, 4).ToLower().Equals("burn"))
				{
					burnRecipes.Add(recipeDef);
				}
			}
			List<RecipeDef> smeltRecipes = new List<RecipeDef>();
			foreach (RecipeDef recipeDef2 in DefDatabase<RecipeDef>.AllDefsListForReading)
			{
				if (recipeDef2.defName.Substring(0, 5).ToLower().Equals("smelt"))
				{
					smeltRecipes.Add(recipeDef2);
				}
			}
			IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[8];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("flammability", (ThingDef d) => d.BaseFlammability);
			array[2] = new TableDataGetter<ThingDef>("burn- or smeltable", (ThingDef d) => (smeltRecipes.Any((RecipeDef r) => r.IsIngredient(d)) || burnRecipes.Any((RecipeDef r) => r.IsIngredient(d))).ToStringCheckBlank());
			array[3] = new TableDataGetter<ThingDef>("burnable", (ThingDef d) => d.burnableByRecipe.ToStringCheckBlank());
			array[4] = new TableDataGetter<ThingDef>("smeltable", (ThingDef d) => d.smeltable.ToStringCheckBlank());
			array[5] = new TableDataGetter<ThingDef>("burn recipe", delegate(ThingDef d)
			{
				string[] array2 = (from r in burnRecipes
				where r.IsIngredient(d)
				select r.ToString()).ToArray<string>();
				if (array2.Length != 0)
				{
					return string.Join(",", array2);
				}
				return "NONE";
			});
			array[6] = new TableDataGetter<ThingDef>("smelt recipe", delegate(ThingDef d)
			{
				string[] array2 = (from r in smeltRecipes
				where r.IsIngredient(d)
				select r.ToString()).ToArray<string>();
				if (array2.Length != 0)
				{
					return string.Join(",", array2);
				}
				return "NONE";
			});
			array[7] = new TableDataGetter<ThingDef>("category", delegate(ThingDef d)
			{
				if (d.thingCategories != null)
				{
					return string.Join(",", (from c in d.thingCategories
					select c.defName).ToArray<string>());
				}
				return "NULL";
			});
			DebugTables.MakeTablesDialog<ThingDef>(allDefs, array);
		}

		// Token: 0x06002EDA RID: 11994 RVA: 0x0012693C File Offset: 0x00124B3C
		[DebugOutput]
		public static void Medicines()
		{
			List<float> list = new List<float>();
			list.Add(0.3f);
			list.AddRange(from d in DefDatabase<ThingDef>.AllDefs
			where typeof(Medicine).IsAssignableFrom(d.thingClass)
			select d.GetStatValueAbstract(StatDefOf.MedicalPotency, null));
			SkillNeed_Direct skillNeed_Direct = (SkillNeed_Direct)StatDefOf.MedicalTendQuality.skillNeedFactors[0];
			TableDataGetter<float>[] array = new TableDataGetter<float>[21];
			array[0] = new TableDataGetter<float>("potency", (float p) => p.ToStringPercent());
			for (int i = 0; i < 20; i++)
			{
				float factor = skillNeed_Direct.valuesPerLevel[i];
				array[i + 1] = new TableDataGetter<float>((i + 1).ToString(), (float p) => (p * factor).ToStringPercent());
			}
			DebugTables.MakeTablesDialog<float>(list, array);
		}

		// Token: 0x06002EDB RID: 11995 RVA: 0x00126A48 File Offset: 0x00124C48
		[DebugOutput]
		public static void ShootingAccuracy()
		{
			StatDef stat = StatDefOf.ShootingAccuracyPawn;
			Func<StatModifier, bool> <>9__19;
			Func<int, float, int, float> accAtDistance = delegate(int level, float dist, int traitDegree)
			{
				float num = 1f;
				if (traitDegree != 0)
				{
					IEnumerable<StatModifier> statOffsets = TraitDef.Named("ShootingAccuracy").DataAtDegree(traitDegree).statOffsets;
					Func<StatModifier, bool> predicate;
					if ((predicate = <>9__19) == null)
					{
						predicate = (<>9__19 = ((StatModifier so) => so.stat == stat));
					}
					float value = statOffsets.First(predicate).value;
					num += value;
				}
				foreach (SkillNeed skillNeed in stat.skillNeedFactors)
				{
					SkillNeed_Direct skillNeed_Direct = skillNeed as SkillNeed_Direct;
					num *= skillNeed_Direct.valuesPerLevel[level];
				}
				num = stat.postProcessCurve.Evaluate(num);
				return Mathf.Pow(num, dist);
			};
			List<int> list = new List<int>();
			for (int i = 0; i <= 20; i++)
			{
				list.Add(i);
			}
			IEnumerable<int> dataSources = list;
			TableDataGetter<int>[] array = new TableDataGetter<int>[18];
			array[0] = new TableDataGetter<int>("No trait skill", (int lev) => lev.ToString());
			array[1] = new TableDataGetter<int>("acc at 1", (int lev) => accAtDistance(lev, 1f, 0).ToStringPercent("F2"));
			array[2] = new TableDataGetter<int>("acc at 10", (int lev) => accAtDistance(lev, 10f, 0).ToStringPercent("F2"));
			array[3] = new TableDataGetter<int>("acc at 20", (int lev) => accAtDistance(lev, 20f, 0).ToStringPercent("F2"));
			array[4] = new TableDataGetter<int>("acc at 30", (int lev) => accAtDistance(lev, 30f, 0).ToStringPercent("F2"));
			array[5] = new TableDataGetter<int>("acc at 50", (int lev) => accAtDistance(lev, 50f, 0).ToStringPercent("F2"));
			array[6] = new TableDataGetter<int>("Careful shooter skill", (int lev) => lev.ToString());
			array[7] = new TableDataGetter<int>("acc at 1", (int lev) => accAtDistance(lev, 1f, 1).ToStringPercent("F2"));
			array[8] = new TableDataGetter<int>("acc at 10", (int lev) => accAtDistance(lev, 10f, 1).ToStringPercent("F2"));
			array[9] = new TableDataGetter<int>("acc at 20", (int lev) => accAtDistance(lev, 20f, 1).ToStringPercent("F2"));
			array[10] = new TableDataGetter<int>("acc at 30", (int lev) => accAtDistance(lev, 30f, 1).ToStringPercent("F2"));
			array[11] = new TableDataGetter<int>("acc at 50", (int lev) => accAtDistance(lev, 50f, 1).ToStringPercent("F2"));
			array[12] = new TableDataGetter<int>("Trigger-happy skill", (int lev) => lev.ToString());
			array[13] = new TableDataGetter<int>("acc at 1", (int lev) => accAtDistance(lev, 1f, -1).ToStringPercent("F2"));
			array[14] = new TableDataGetter<int>("acc at 10", (int lev) => accAtDistance(lev, 10f, -1).ToStringPercent("F2"));
			array[15] = new TableDataGetter<int>("acc at 20", (int lev) => accAtDistance(lev, 20f, -1).ToStringPercent("F2"));
			array[16] = new TableDataGetter<int>("acc at 30", (int lev) => accAtDistance(lev, 30f, -1).ToStringPercent("F2"));
			array[17] = new TableDataGetter<int>("acc at 50", (int lev) => accAtDistance(lev, 50f, -1).ToStringPercent("F2"));
			DebugTables.MakeTablesDialog<int>(dataSources, array);
		}

		// Token: 0x06002EDC RID: 11996 RVA: 0x00126CA3 File Offset: 0x00124EA3
		[DebugOutput(true)]
		public static void TemperatureData()
		{
			Find.CurrentMap.mapTemperature.DebugLogTemps();
		}

		// Token: 0x06002EDD RID: 11997 RVA: 0x00126CB4 File Offset: 0x00124EB4
		[DebugOutput(false)]
		public static void TemperatureOverlayColors()
		{
			Find.CurrentMap.mapTemperature.DebugLogTemperatureOverlayColors();
		}

		// Token: 0x06002EDE RID: 11998 RVA: 0x00126CC5 File Offset: 0x00124EC5
		[DebugOutput(true)]
		public static void WeatherChances()
		{
			Find.CurrentMap.weatherDecider.LogWeatherChances();
		}

		// Token: 0x06002EDF RID: 11999 RVA: 0x00126CD8 File Offset: 0x00124ED8
		[DebugOutput]
		public static void WeatherCommonalities()
		{
			IEnumerable<TableDataGetter<BiomeDef>> source = DefDatabase<WeatherDef>.AllDefsListForReading.Select(delegate(WeatherDef w)
			{
				Predicate<WeatherCommonalityRecord> <>9__3;
				return new TableDataGetter<BiomeDef>(w.label, delegate(BiomeDef b)
				{
					List<WeatherCommonalityRecord> baseWeatherCommonalities = b.baseWeatherCommonalities;
					Predicate<WeatherCommonalityRecord> predicate;
					if ((predicate = <>9__3) == null)
					{
						predicate = (<>9__3 = ((WeatherCommonalityRecord c) => c.weather == w));
					}
					WeatherCommonalityRecord weatherCommonalityRecord = baseWeatherCommonalities.FirstOrDefault(predicate);
					return ((weatherCommonalityRecord != null) ? weatherCommonalityRecord.commonality.ToString() : null) ?? "-";
				});
			});
			DebugTables.MakeTablesDialog<BiomeDef>(DefDatabase<BiomeDef>.AllDefs, source.Prepend(new TableDataGetter<BiomeDef>("biome", (BiomeDef w) => w.defName)).ToArray<TableDataGetter<BiomeDef>>());
		}

		// Token: 0x06002EE0 RID: 12000 RVA: 0x00126D4D File Offset: 0x00124F4D
		[DebugOutput(true)]
		public static void CelestialGlow()
		{
			GenCelestial.LogSunGlowForYear();
		}

		// Token: 0x06002EE1 RID: 12001 RVA: 0x00126D54 File Offset: 0x00124F54
		[DebugOutput(true)]
		public static void SunAngle()
		{
			GenCelestial.LogSunAngleForYear();
		}

		// Token: 0x06002EE2 RID: 12002 RVA: 0x00126D5B File Offset: 0x00124F5B
		[DebugOutput(true)]
		public static void FallColor()
		{
			PlantUtility.LogFallColorForYear();
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x00126D62 File Offset: 0x00124F62
		[DebugOutput(true)]
		public static void PawnsListAllOnMap()
		{
			Find.CurrentMap.mapPawns.LogListedPawns();
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x00126D73 File Offset: 0x00124F73
		[DebugOutput(true)]
		public static void WindSpeeds()
		{
			Find.CurrentMap.windManager.LogWindSpeeds();
		}

		// Token: 0x06002EE5 RID: 12005 RVA: 0x00126D84 File Offset: 0x00124F84
		[DebugOutput(true)]
		public static void MapPawnsList()
		{
			Find.CurrentMap.mapPawns.LogListedPawns();
		}

		// Token: 0x06002EE6 RID: 12006 RVA: 0x00126D95 File Offset: 0x00124F95
		[DebugOutput]
		public static void Lords()
		{
			Find.CurrentMap.lordManager.LogLords();
		}

		// Token: 0x06002EE7 RID: 12007 RVA: 0x00126DA6 File Offset: 0x00124FA6
		[DebugOutput]
		public static void LogEnroute()
		{
			Find.CurrentMap.enrouteManager.LogEnroute();
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x00126DB8 File Offset: 0x00124FB8
		[DebugOutput]
		public static void BodyPartTagGroups()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (BodyDef localBd2 in DefDatabase<BodyDef>.AllDefs)
			{
				BodyDef localBd = localBd2;
				FloatMenuOption item = new FloatMenuOption(localBd.defName, delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(localBd.defName + "\n----------------");
					using (IEnumerator<BodyPartTagDef> enumerator2 = localBd.AllParts.SelectMany((BodyPartRecord part) => part.def.tags).Distinct<BodyPartTagDef>().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							BodyPartTagDef tag = enumerator2.Current;
							stringBuilder.AppendLine(tag.defName);
							IEnumerable<BodyPartRecord> allParts = localBd.AllParts;
							Func<BodyPartRecord, bool> predicate;
							Func<BodyPartRecord, bool> <>9__2;
							if ((predicate = <>9__2) == null)
							{
								predicate = (<>9__2 = ((BodyPartRecord part) => part.def.tags.Contains(tag)));
							}
							foreach (BodyPartRecord bodyPartRecord in from part in allParts.Where(predicate)
							orderby part.def.defName
							select part)
							{
								stringBuilder.AppendLine("  " + bodyPartRecord.def.defName);
							}
						}
					}
					Log.Message(stringBuilder.ToString());
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06002EE9 RID: 12009 RVA: 0x00126E54 File Offset: 0x00125054
		[DebugOutput]
		public static void MinifiableTags()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.Minifiable)
				{
					stringBuilder.Append(thingDef.defName);
					if (!thingDef.tradeTags.NullOrEmpty<string>())
					{
						stringBuilder.Append(" - ");
						stringBuilder.Append(thingDef.tradeTags.ToCommaList(false, false));
					}
					stringBuilder.AppendLine();
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x00126EF4 File Offset: 0x001250F4
		[DebugOutput]
		public static void ThingSetMakerTest()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ThingSetMakerDef localDef2 in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef localDef = localDef2;
				Action<ThingSetMakerParams> <>9__1;
				DebugMenuOption item = new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					DebugOutputsMisc.<>c__DisplayClass31_1 CS$<>8__locals2 = new DebugOutputsMisc.<>c__DisplayClass31_1();
					DebugOutputsMisc.<>c__DisplayClass31_1 CS$<>8__locals3 = CS$<>8__locals2;
					Action<ThingSetMakerParams> generate;
					if ((generate = <>9__1) == null)
					{
						generate = (<>9__1 = delegate(ThingSetMakerParams parms)
						{
							StringBuilder stringBuilder = new StringBuilder();
							float num = 0f;
							float num2 = 0f;
							for (int i = 0; i < 50; i++)
							{
								List<Thing> list3 = localDef.root.Generate(parms);
								if (stringBuilder.Length > 0)
								{
									stringBuilder.AppendLine();
								}
								if (list3.NullOrEmpty<Thing>())
								{
									stringBuilder.AppendLine("-(nothing generated)");
								}
								float num3 = 0f;
								float num4 = 0f;
								for (int j = 0; j < list3.Count; j++)
								{
									stringBuilder.AppendLine("-" + list3[j].LabelCap + " - $" + (list3[j].MarketValue * (float)list3[j].stackCount).ToString("F0"));
									num3 += list3[j].MarketValue * (float)list3[j].stackCount;
									if (!(list3[j] is Pawn))
									{
										num4 += list3[j].GetStatValue(StatDefOf.Mass, true, -1) * (float)list3[j].stackCount;
									}
									list3[j].Destroy(DestroyMode.Vanish);
								}
								num += num3;
								num2 += num4;
								stringBuilder.AppendLine("   Total market value: $" + num3.ToString("F0"));
								stringBuilder.AppendLine("   Total mass: " + num4.ToStringMass());
							}
							StringBuilder stringBuilder2 = new StringBuilder();
							stringBuilder2.AppendLine("Default thing sets generated by: " + localDef.defName);
							string nonNullFieldsDebugInfo = Gen.GetNonNullFieldsDebugInfo(localDef.root.fixedParams);
							stringBuilder2.AppendLine("root fixedParams: " + (nonNullFieldsDebugInfo.NullOrEmpty() ? "none" : nonNullFieldsDebugInfo));
							string nonNullFieldsDebugInfo2 = Gen.GetNonNullFieldsDebugInfo(parms);
							if (!nonNullFieldsDebugInfo2.NullOrEmpty())
							{
								stringBuilder2.AppendLine("(used custom debug params: " + nonNullFieldsDebugInfo2 + ")");
							}
							stringBuilder2.AppendLine("Average market value: $" + (num / 50f).ToString("F1"));
							stringBuilder2.AppendLine("Average mass: " + (num2 / 50f).ToStringMass());
							stringBuilder2.AppendLine();
							stringBuilder2.Append(stringBuilder.ToString());
							Log.Message(stringBuilder2.ToString());
						});
					}
					CS$<>8__locals3.generate = generate;
					if (localDef == ThingSetMakerDefOf.TraderStock)
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (Faction faction in Find.FactionManager.AllFactions)
						{
							if (faction != Faction.OfPlayer)
							{
								Faction localF = faction;
								list2.Add(new DebugMenuOption(localF.Name + " (" + localF.def.defName + ")", DebugMenuOptionMode.Action, delegate()
								{
									List<DebugMenuOption> list3 = new List<DebugMenuOption>();
									foreach (TraderKindDef localKind2 in (from x in DefDatabase<TraderKindDef>.AllDefs
									where x.orbital
									select x).Concat(localF.def.caravanTraderKinds).Concat(localF.def.visitorTraderKinds).Concat(localF.def.baseTraderKinds))
									{
										TraderKindDef localKind = localKind2;
										list3.Add(new DebugMenuOption(localKind.defName, DebugMenuOptionMode.Action, delegate()
										{
											ThingSetMakerParams obj = default(ThingSetMakerParams);
											obj.makingFaction = localF;
											obj.traderDef = localKind;
											CS$<>8__locals2.generate(obj);
										}));
									}
									Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3, null));
								}));
							}
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
						return;
					}
					CS$<>8__locals2.generate(localDef.debugParams);
				});
				list.Add(item);
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x00126F84 File Offset: 0x00125184
		[DebugOutput]
		public static void ThingSetMakerPossibleDefs()
		{
			Dictionary<ThingSetMakerDef, List<ThingDef>> generatableThings = new Dictionary<ThingSetMakerDef, List<ThingDef>>();
			foreach (ThingSetMakerDef thingSetMakerDef in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef thingSetMakerDef2 = thingSetMakerDef;
				generatableThings[thingSetMakerDef] = thingSetMakerDef2.root.AllGeneratableThingsDebug(thingSetMakerDef2.debugParams).ToList<ThingDef>();
			}
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName));
			list.Add(new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue.ToStringMoney(null)));
			list.Add(new TableDataGetter<ThingDef>("mass", (ThingDef d) => d.BaseMass.ToStringMass()));
			list.Add(new TableDataGetter<ThingDef>("min\ncount", delegate(ThingDef d)
			{
				if (d.stackLimit == 1)
				{
					return "";
				}
				return d.minRewardCount.ToString();
			}));
			foreach (ThingSetMakerDef localDef2 in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef localDef = localDef2;
				list.Add(new TableDataGetter<ThingDef>(localDef.defName.Shorten(), (ThingDef d) => generatableThings[localDef].Contains(d).ToStringCheckBlank()));
			}
			DebugTables.MakeTablesDialog<ThingDef>(from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Item && !d.IsCorpse && !d.isUnfinishedThing) || (d.category == ThingCategory.Building && d.Minifiable) || d.category == ThingCategory.Pawn
			orderby d.BaseMarketValue descending
			select d, list.ToArray());
		}

		// Token: 0x06002EEC RID: 12012 RVA: 0x00127194 File Offset: 0x00125394
		[DebugOutput]
		public static void ThingSetMakerSampled()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ThingSetMakerDef localDef2 in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef localDef = localDef2;
				Action<ThingSetMakerParams> <>9__1;
				DebugMenuOption item = new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					DebugOutputsMisc.<>c__DisplayClass33_1 CS$<>8__locals2 = new DebugOutputsMisc.<>c__DisplayClass33_1();
					DebugOutputsMisc.<>c__DisplayClass33_1 CS$<>8__locals3 = CS$<>8__locals2;
					Action<ThingSetMakerParams> generate;
					if ((generate = <>9__1) == null)
					{
						generate = (<>9__1 = delegate(ThingSetMakerParams parms)
						{
							Dictionary<ThingDef, int> counts = new Dictionary<ThingDef, int>();
							for (int i = 0; i < 500; i++)
							{
								List<Thing> list3 = localDef.root.Generate(parms);
								foreach (ThingDef thingDef in (from th in list3
								select th.GetInnerIfMinified().def).Distinct<ThingDef>())
								{
									if (!counts.ContainsKey(thingDef))
									{
										counts.Add(thingDef, 0);
									}
									Dictionary<ThingDef, int> counts2 = counts;
									ThingDef key = thingDef;
									int num = counts2[key];
									counts2[key] = num + 1;
								}
								for (int j = 0; j < list3.Count; j++)
								{
									list3[j].Destroy(DestroyMode.Vanish);
								}
							}
							IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
							where counts.ContainsKey(d)
							orderby counts[d] descending
							select d;
							TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
							array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
							array[1] = new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue.ToStringMoney(null));
							array[2] = new TableDataGetter<ThingDef>("mass", (ThingDef d) => d.BaseMass.ToStringMass());
							array[3] = new TableDataGetter<ThingDef>("appearance rate in " + localDef.defName, (ThingDef d) => ((float)counts[d] / 500f).ToStringPercent());
							DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
						});
					}
					CS$<>8__locals3.generate = generate;
					if (localDef == ThingSetMakerDefOf.TraderStock)
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (Faction faction in Find.FactionManager.AllFactions)
						{
							if (faction != Faction.OfPlayer)
							{
								Faction localF = faction;
								list2.Add(new DebugMenuOption(localF.Name + " (" + localF.def.defName + ")", DebugMenuOptionMode.Action, delegate()
								{
									List<DebugMenuOption> list3 = new List<DebugMenuOption>();
									foreach (TraderKindDef localKind2 in (from x in DefDatabase<TraderKindDef>.AllDefs
									where x.orbital
									select x).Concat(localF.def.caravanTraderKinds).Concat(localF.def.visitorTraderKinds).Concat(localF.def.baseTraderKinds))
									{
										TraderKindDef localKind = localKind2;
										list3.Add(new DebugMenuOption(localKind.defName, DebugMenuOptionMode.Action, delegate()
										{
											ThingSetMakerParams obj = default(ThingSetMakerParams);
											obj.makingFaction = localF;
											obj.traderDef = localKind;
											CS$<>8__locals2.generate(obj);
										}));
									}
									Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3, null));
								}));
							}
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
						return;
					}
					CS$<>8__locals2.generate(localDef.debugParams);
				});
				list.Add(item);
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002EED RID: 12013 RVA: 0x00127224 File Offset: 0x00125424
		[DebugOutput]
		public static void RewardsGeneration()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (faction != Faction.OfPlayer)
				{
					Faction localF = faction;
					list.Add(new DebugMenuOption(localF.Name + " (" + localF.def.defName + ")", DebugMenuOptionMode.Action, delegate()
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (float localPoints2 in DebugActionsUtility.PointsOptions(false))
						{
							float localPoints = localPoints2;
							list2.Add(new DebugMenuOption(localPoints2.ToString("F0"), DebugMenuOptionMode.Action, delegate()
							{
								StringBuilder stringBuilder = new StringBuilder();
								for (int i = 0; i < 30; i++)
								{
									RewardsGeneratorParams rewardsGeneratorParams = new RewardsGeneratorParams
									{
										allowGoodwill = true,
										allowRoyalFavor = true,
										populationIntent = StorytellerUtilityPopulation.PopulationIntentForQuest,
										giverFaction = localF,
										rewardValue = localPoints
									};
									float f;
									List<Reward> source = RewardsGenerator.Generate(rewardsGeneratorParams, out f);
									StringBuilder stringBuilder2 = stringBuilder;
									string[] array = new string[8];
									array[0] = "giver: ";
									array[1] = rewardsGeneratorParams.giverFaction.Name;
									array[2] = ", input value: ";
									array[3] = rewardsGeneratorParams.rewardValue.ToStringMoney(null);
									array[4] = ", output value: ";
									array[5] = f.ToStringMoney(null);
									array[6] = "\n";
									array[7] = (from x in source
									select "-" + x.ToString()).ToLineList(null, false).Indented("  ");
									stringBuilder2.AppendLine(string.Concat(array));
									stringBuilder.AppendLine();
								}
								Log.Message(stringBuilder.ToString());
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002EEE RID: 12014 RVA: 0x001272DC File Offset: 0x001254DC
		[DebugOutput]
		public static void RewardsGenerationSampled()
		{
			RewardsGeneratorParams parms = default(RewardsGeneratorParams);
			parms.allowGoodwill = true;
			parms.allowRoyalFavor = true;
			parms.populationIntent = StorytellerUtilityPopulation.PopulationIntentForQuest;
			parms.giverFaction = (from x in Find.FactionManager.GetFactions(false, false, true, TechLevel.Undefined, false)
			where !x.Hidden && x.def.humanlikeFaction && !x.HostileTo(Faction.OfPlayer)
			select x).First<Faction>();
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (float localPoints2 in DebugActionsUtility.PointsOptions(false))
			{
				float localPoints = localPoints2;
				list.Add(new DebugMenuOption(localPoints2.ToString("F0"), DebugMenuOptionMode.Action, delegate()
				{
					parms.rewardValue = localPoints;
					Dictionary<Type, int> countByType = new Dictionary<Type, int>();
					Dictionary<ThingDef, int> countByThingDef = new Dictionary<ThingDef, int>();
					for (int i = 0; i < 1000; i++)
					{
						foreach (Reward reward in RewardsGenerator.Generate(parms))
						{
							countByType.Increment(reward.GetType());
							Reward_Items reward_Items = reward as Reward_Items;
							if (reward_Items != null)
							{
								foreach (ThingDef key in (from x in reward_Items.items
								select x.GetInnerIfMinified().def).Distinct<ThingDef>())
								{
									countByThingDef.Increment(key);
								}
							}
						}
					}
					Dictionary<ThingSetMakerDef, List<ThingDef>> dictionary = new Dictionary<ThingSetMakerDef, List<ThingDef>>();
					foreach (ThingSetMakerDef thingSetMakerDef in DefDatabase<ThingSetMakerDef>.AllDefs)
					{
						ThingSetMakerDef thingSetMakerDef2 = thingSetMakerDef;
						dictionary[thingSetMakerDef] = thingSetMakerDef2.root.AllGeneratableThingsDebug(thingSetMakerDef2.debugParams).ToList<ThingDef>();
					}
					List<TableDataGetter<object>> list2 = new List<TableDataGetter<object>>();
					list2.Add(new TableDataGetter<object>("defName", delegate(object d)
					{
						if (!(d is Type))
						{
							return ((ThingDef)d).defName;
						}
						return "*" + ((Type)d).Name;
					}));
					list2.Add(new TableDataGetter<object>("times appeared\nin " + 1000.ToString() + " rewards\nof value " + localPoints.ToString(), delegate(object d)
					{
						if (!(d is Type))
						{
							return countByThingDef.TryGetValue((ThingDef)d, 0);
						}
						return countByType.TryGetValue((Type)d, 0);
					}));
					list2.Add(new TableDataGetter<object>("reward tags", delegate(object d)
					{
						ThingDef thingDef = d as ThingDef;
						if (thingDef != null && !thingDef.thingSetMakerTags.NullOrEmpty<string>())
						{
							if (thingDef.thingSetMakerTags.Any((string x) => x.Contains("Reward")))
							{
								return thingDef.thingSetMakerTags.ToCommaList(false, false);
							}
						}
						return "-";
					}));
					list2.Add(new TableDataGetter<object>("market value", delegate(object d)
					{
						ThingDef thingDef = d as ThingDef;
						if (thingDef != null)
						{
							return thingDef.BaseMarketValue.ToStringMoney(null);
						}
						return "-";
					}));
					DebugTables.MakeTablesDialog<object>(typeof(Reward).AllSubclassesNonAbstract().Cast<object>().Union(countByThingDef.Keys.Cast<object>()).OrderByDescending(delegate(object d)
					{
						if (!(d is Type))
						{
							return countByThingDef.TryGetValue((ThingDef)d, 0);
						}
						return countByType.TryGetValue((Type)d, 0);
					}), list2.ToArray());
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002EEF RID: 12015 RVA: 0x001273F4 File Offset: 0x001255F4
		[DebugOutput]
		public static void WorkDisables()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (PawnKindDef pkInner2 in from ki in DefDatabase<PawnKindDef>.AllDefs
			where ki.RaceProps.Humanlike
			select ki)
			{
				PawnKindDef pkInner = pkInner2;
				Faction faction = FactionUtility.DefaultFactionFrom(pkInner.defaultFactionDef);
				FloatMenuOption item = new FloatMenuOption(pkInner.defName, delegate()
				{
					int num = 500;
					DefMap<WorkTypeDef, int> defMap = new DefMap<WorkTypeDef, int>();
					for (int i = 0; i < num; i++)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(pkInner, faction, null);
						if (pawn.workSettings != null)
						{
							foreach (WorkTypeDef workTypeDef in pawn.GetDisabledWorkTypes(true))
							{
								DefMap<WorkTypeDef, int> defMap2 = defMap;
								WorkTypeDef def = workTypeDef;
								int num2 = defMap2[def];
								defMap2[def] = num2 + 1;
							}
						}
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						"Generated ",
						num.ToString(),
						" pawns of kind ",
						pkInner.defName,
						" on faction ",
						faction.ToStringSafe<Faction>()
					}));
					stringBuilder.AppendLine("Work types disabled:");
					foreach (WorkTypeDef workTypeDef2 in DefDatabase<WorkTypeDef>.AllDefs)
					{
						if (workTypeDef2.workTags != WorkTags.None)
						{
							stringBuilder.AppendLine(string.Concat(new string[]
							{
								"   ",
								workTypeDef2.defName,
								": ",
								defMap[workTypeDef2].ToString(),
								"        ",
								((float)defMap[workTypeDef2] / (float)num).ToStringPercent()
							}));
						}
					}
					IEnumerable<BackstoryDef> allDefs = DefDatabase<BackstoryDef>.AllDefs;
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Backstories WorkTypeDef disable rates (there are " + allDefs.Count<BackstoryDef>().ToString() + " backstories):");
					using (IEnumerator<WorkTypeDef> enumerator3 = DefDatabase<WorkTypeDef>.AllDefs.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							WorkTypeDef wt = enumerator3.Current;
							int num3 = 0;
							Predicate<WorkTypeDef> <>9__2;
							foreach (BackstoryDef backstoryDef in allDefs)
							{
								List<WorkTypeDef> disabledWorkTypes = backstoryDef.DisabledWorkTypes;
								Predicate<WorkTypeDef> predicate;
								if ((predicate = <>9__2) == null)
								{
									predicate = (<>9__2 = ((WorkTypeDef wd) => wt == wd));
								}
								if (disabledWorkTypes.Any(predicate))
								{
									num3++;
								}
							}
							stringBuilder.AppendLine(string.Concat(new string[]
							{
								"   ",
								wt.defName,
								": ",
								num3.ToString(),
								"     ",
								((float)num3 / (float)allDefs.Count<BackstoryDef>()).ToStringPercent()
							}));
						}
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Backstories WorkTag disable rates (there are " + allDefs.Count<BackstoryDef>().ToString() + " backstories):");
					foreach (object obj in Enum.GetValues(typeof(WorkTags)))
					{
						WorkTags workTags = (WorkTags)obj;
						int num4 = 0;
						foreach (BackstoryDef backstoryDef2 in allDefs)
						{
							if ((workTags & backstoryDef2.workDisables) != WorkTags.None)
							{
								num4++;
							}
						}
						stringBuilder.AppendLine(string.Concat(new string[]
						{
							"   ",
							workTags.ToString(),
							": ",
							num4.ToString(),
							"     ",
							((float)num4 / (float)allDefs.Count<BackstoryDef>()).ToStringPercent()
						}));
					}
					Log.Message(stringBuilder.ToString());
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06002EF0 RID: 12016 RVA: 0x001274C8 File Offset: 0x001256C8
		[DebugOutput]
		public static void FoodPreferability()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Food, ordered by preferability:");
			foreach (ThingDef thingDef in from td in DefDatabase<ThingDef>.AllDefs
			where td.ingestible != null
			orderby td.ingestible.preferability
			select td)
			{
				stringBuilder.AppendLine(string.Format("  {0}: {1}", thingDef.ingestible.preferability, thingDef.defName));
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06002EF1 RID: 12017 RVA: 0x0012759C File Offset: 0x0012579C
		[DebugOutput]
		public static void IngestibleMaxSatisfiedTitle()
		{
			RoyalTitleUtility.DoTable_IngestibleMaxSatisfiedTitle();
		}

		// Token: 0x06002EF2 RID: 12018 RVA: 0x001275A3 File Offset: 0x001257A3
		[DebugOutput]
		public static void AbilityCosts()
		{
			AbilityUtility.DoTable_AbilityCosts();
		}

		// Token: 0x06002EF3 RID: 12019 RVA: 0x001275AC File Offset: 0x001257AC
		[DebugOutput]
		public static void SitePartDefs()
		{
			List<TableDataGetter<SitePartDef>> list = new List<TableDataGetter<SitePartDef>>();
			list.Add(new TableDataGetter<SitePartDef>("defName", (SitePartDef a) => a.defName));
			list.Add(new TableDataGetter<SitePartDef>("label", (SitePartDef a) => a.LabelCap));
			list.Add(new TableDataGetter<SitePartDef>("tags", (SitePartDef a) => string.Join(", ", a.tags)));
			list.Add(new TableDataGetter<SitePartDef>("excludeTags", (SitePartDef a) => string.Join(", ", a.excludesTags)));
			list.Add(new TableDataGetter<SitePartDef>("incompatible\nothers", (SitePartDef a) => string.Join<TaggedString>(", ", from def in DefDatabase<SitePartDef>.AllDefs
			where !def.CompatibleWith(a)
			select def.LabelCap)));
			DebugTables.MakeTablesDialog<SitePartDef>(DefDatabase<SitePartDef>.AllDefsListForReading, list.ToArray());
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x001276BC File Offset: 0x001258BC
		[DebugOutput]
		public static void MapDanger()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Map danger status:");
			foreach (Map map in Find.Maps)
			{
				stringBuilder.AppendLine(string.Format("{0}: {1}", map, map.dangerWatcher.DangerRating));
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x00127748 File Offset: 0x00125948
		[DebugOutput]
		public static void GenSteps()
		{
			IEnumerable<GenStepDef> dataSources = from x in DefDatabase<GenStepDef>.AllDefsListForReading
			orderby x.order, x.index
			select x;
			TableDataGetter<GenStepDef>[] array = new TableDataGetter<GenStepDef>[4];
			array[0] = new TableDataGetter<GenStepDef>("defName", (GenStepDef x) => x.defName);
			array[1] = new TableDataGetter<GenStepDef>("order", (GenStepDef x) => x.order.ToString("0.##"));
			array[2] = new TableDataGetter<GenStepDef>("class", (GenStepDef x) => x.genStep.GetType().Name);
			array[3] = new TableDataGetter<GenStepDef>("site", delegate(GenStepDef x)
			{
				if (x.linkWithSite == null)
				{
					return "";
				}
				return x.linkWithSite.defName;
			});
			DebugTables.MakeTablesDialog<GenStepDef>(dataSources, array);
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x00127860 File Offset: 0x00125A60
		[DebugOutput]
		public static void WorldGenSteps()
		{
			IEnumerable<WorldGenStepDef> dataSources = from x in DefDatabase<WorldGenStepDef>.AllDefsListForReading
			orderby x.order, x.index
			select x;
			TableDataGetter<WorldGenStepDef>[] array = new TableDataGetter<WorldGenStepDef>[3];
			array[0] = new TableDataGetter<WorldGenStepDef>("defName", (WorldGenStepDef x) => x.defName);
			array[1] = new TableDataGetter<WorldGenStepDef>("order", (WorldGenStepDef x) => x.order.ToString("0.##"));
			array[2] = new TableDataGetter<WorldGenStepDef>("class", (WorldGenStepDef x) => x.worldGenStep.GetType().Name);
			DebugTables.MakeTablesDialog<WorldGenStepDef>(dataSources, array);
		}

		// Token: 0x06002EF7 RID: 12023 RVA: 0x0012794C File Offset: 0x00125B4C
		[DebugOutput]
		public static void ShuttleDefsToAvoid()
		{
			IEnumerable<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[2];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef x) => x.defName);
			array[1] = new TableDataGetter<ThingDef>("avoid", (ThingDef x) => (GenSpawn.SpawningWipes(ThingDefOf.ActiveDropPod, x, false) || (x.plant != null && x.plant.IsTree) || x.category == ThingCategory.Item || x.category == ThingCategory.Building).ToStringCheckBlank());
			DebugTables.MakeTablesDialog<ThingDef>(allDefsListForReading, array);
		}

		// Token: 0x06002EF8 RID: 12024 RVA: 0x001279C4 File Offset: 0x00125BC4
		[DebugOutput]
		public static void RitualDuration()
		{
			IEnumerable<RitualBehaviorDef> allDefsListForReading = DefDatabase<RitualBehaviorDef>.AllDefsListForReading;
			TableDataGetter<RitualBehaviorDef>[] array = new TableDataGetter<RitualBehaviorDef>[2];
			array[0] = new TableDataGetter<RitualBehaviorDef>("defName", (RitualBehaviorDef x) => x.defName);
			array[1] = new TableDataGetter<RitualBehaviorDef>("duration", (RitualBehaviorDef x) => x.durationTicks.max.ToStringTicksToPeriod(true, false, true, true, false));
			DebugTables.MakeTablesDialog<RitualBehaviorDef>(allDefsListForReading, array);
		}

		// Token: 0x06002EF9 RID: 12025 RVA: 0x00127A39 File Offset: 0x00125C39
		[DebugOutput]
		public static void PlayerWealth()
		{
			Log.Message("Player wealth: " + WealthUtility.PlayerWealth.ToStringMoney(null));
		}

		// Token: 0x06002EFA RID: 12026 RVA: 0x00127A58 File Offset: 0x00125C58
		[DebugOutput]
		public static void MemesAndPrecepts()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Memes:");
			foreach (MemeDef memeDef in from x in DefDatabase<MemeDef>.AllDefs
			orderby x.category
			select x)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(memeDef.LabelCap);
				defaultInterpolatedStringHandler.AppendLiteral(": \"");
				defaultInterpolatedStringHandler.AppendFormatted(memeDef.description);
				defaultInterpolatedStringHandler.AppendLiteral("\"");
				stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Precepts:");
			using (IEnumerator<IssueDef> enumerator2 = DefDatabase<IssueDef>.AllDefs.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					IssueDef i = enumerator2.Current;
					stringBuilder.AppendLine(i.LabelCap);
					IEnumerable<PreceptDef> allDefs = DefDatabase<PreceptDef>.AllDefs;
					Func<PreceptDef, bool> predicate;
					Func<PreceptDef, bool> <>9__1;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((PreceptDef x) => x.issue == i && !x.label.NullOrEmpty() && !x.description.NullOrEmpty()));
					}
					foreach (PreceptDef preceptDef in allDefs.Where(predicate))
					{
						StringBuilder stringBuilder3 = stringBuilder;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(5, 2);
						defaultInterpolatedStringHandler2.AppendLiteral("\t");
						defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>(preceptDef.LabelCap);
						defaultInterpolatedStringHandler2.AppendLiteral(": \"");
						defaultInterpolatedStringHandler2.AppendFormatted(preceptDef.description);
						defaultInterpolatedStringHandler2.AppendLiteral("\"");
						stringBuilder3.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
					}
				}
			}
			Log.Message(GUIUtility.systemCopyBuffer = stringBuilder.ToString());
		}

		// Token: 0x06002EFB RID: 12027 RVA: 0x00127C60 File Offset: 0x00125E60
		[DebugOutput]
		public static void GenerateGeneSetsX10()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 10; i++)
			{
				GeneSet geneSet = GeneUtility.GenerateGeneSet(null);
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(geneSet.ToString());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06002EFC RID: 12028 RVA: 0x00127CB8 File Offset: 0x00125EB8
		[DebugOutput]
		public static void Genes()
		{
			IEnumerable<GeneDef> allDefs = DefDatabase<GeneDef>.AllDefs;
			TableDataGetter<GeneDef>[] array = new TableDataGetter<GeneDef>[7];
			array[0] = new TableDataGetter<GeneDef>("defName", (GeneDef g) => g.defName);
			array[1] = new TableDataGetter<GeneDef>("label", (GeneDef g) => g.label);
			array[2] = new TableDataGetter<GeneDef>("labelShortAdj", (GeneDef g) => g.LabelShortAdj);
			array[3] = new TableDataGetter<GeneDef>("CPX", (GeneDef g) => g.biostatCpx);
			array[4] = new TableDataGetter<GeneDef>("MET", (GeneDef g) => g.biostatMet);
			array[5] = new TableDataGetter<GeneDef>("ARC", (GeneDef g) => g.biostatArc);
			array[6] = new TableDataGetter<GeneDef>("prereq", delegate(GeneDef g)
			{
				if (g.prerequisite != null)
				{
					return g.prerequisite.defName;
				}
				return string.Empty;
			});
			DebugTables.MakeTablesDialog<GeneDef>(allDefs, array);
		}

		// Token: 0x06002EFD RID: 12029 RVA: 0x00127E0C File Offset: 0x0012600C
		[DebugOutput]
		public static void Xenotypes()
		{
			IEnumerable<XenotypeDef> allDefs = DefDatabase<XenotypeDef>.AllDefs;
			TableDataGetter<XenotypeDef>[] array = new TableDataGetter<XenotypeDef>[4];
			array[0] = new TableDataGetter<XenotypeDef>("defName", (XenotypeDef x) => x.defName);
			array[1] = new TableDataGetter<XenotypeDef>("CPX", (XenotypeDef x) => x.AllGenes.Sum((GeneDef y) => y.biostatCpx));
			array[2] = new TableDataGetter<XenotypeDef>("MET", (XenotypeDef x) => x.AllGenes.Sum((GeneDef y) => y.biostatMet));
			array[3] = new TableDataGetter<XenotypeDef>("ARC", (XenotypeDef x) => x.AllGenes.Sum((GeneDef y) => y.biostatArc));
			DebugTables.MakeTablesDialog<XenotypeDef>(allDefs, array);
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x00127EDC File Offset: 0x001260DC
		[DebugOutput]
		public static void PreceptDefs()
		{
			IEnumerable<PreceptDef> dataSources = from x in DefDatabase<PreceptDef>.AllDefs
			orderby x.defName
			select x;
			TableDataGetter<PreceptDef>[] array = new TableDataGetter<PreceptDef>[8];
			array[0] = new TableDataGetter<PreceptDef>("defName", (PreceptDef x) => x.defName);
			array[1] = new TableDataGetter<PreceptDef>("impact", (PreceptDef x) => x.impact);
			array[2] = new TableDataGetter<PreceptDef>("duplicates", (PreceptDef x) => x.allowDuplicates.ToStringCheckBlank());
			array[3] = new TableDataGetter<PreceptDef>("defaultSelectionWeight", (PreceptDef x) => x.defaultSelectionWeight.ToString());
			array[4] = new TableDataGetter<PreceptDef>("noExpansion", (PreceptDef x) => x.classic.ToStringCheckBlank());
			array[5] = new TableDataGetter<PreceptDef>("exclusionTags", (PreceptDef x) => x.exclusionTags.ToCommaList(false, true));
			array[6] = new TableDataGetter<PreceptDef>("effects", (PreceptDef x) => string.Join("\n", x.comps.SelectMany((PreceptComp y) => y.GetDescriptions()).Distinct<string>()));
			array[7] = new TableDataGetter<PreceptDef>("stats", delegate(PreceptDef x)
			{
				IEnumerable<string> first;
				if (x.statOffsets == null)
				{
					first = Enumerable.Empty<string>();
				}
				else
				{
					first = from y in x.statOffsets
					select y.stat.defName + ": " + y.ValueToStringAsOffset;
				}
				IEnumerable<string> second;
				if (x.statFactors == null)
				{
					second = Enumerable.Empty<string>();
				}
				else
				{
					second = from y in x.statFactors
					select y.stat.defName + ": " + y.ToStringAsFactor;
				}
				return first.Concat(second).ToCommaList(false, true);
			});
			DebugTables.MakeTablesDialog<PreceptDef>(dataSources, array);
		}

		// Token: 0x06002EFF RID: 12031 RVA: 0x00128080 File Offset: 0x00126280
		[DebugOutput]
		public static void ThingTradeTags()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<string> enumerator = (from t in (from k in DefDatabase<ThingDef>.AllDefs
			where k.tradeTags != null
			select k).SelectMany((ThingDef k) => k.tradeTags).Distinct<string>()
			orderby t
			select t).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string tag = enumerator.Current;
					Func<ThingDef, bool> <>9__5;
					list.Add(new DebugMenuOption(tag, DebugMenuOptionMode.Action, delegate()
					{
						List<TableDataGetter<ThingDef>> list2 = new List<TableDataGetter<ThingDef>>();
						list2.Add(new TableDataGetter<ThingDef>("defName", (ThingDef k) => k.defName));
						IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
						Func<ThingDef, bool> predicate;
						if ((predicate = <>9__5) == null)
						{
							predicate = (<>9__5 = ((ThingDef t) => t.tradeTags.NotNullAndContains(tag)));
						}
						DebugTables.MakeTablesDialog<ThingDef>(allDefs.Where(predicate), list2.ToArray());
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002F00 RID: 12032 RVA: 0x00128178 File Offset: 0x00126378
		[DebugOutput]
		public static void StructureColorDefs()
		{
			IEnumerable<ColorDef> dataSources = from x in DefDatabase<ColorDef>.AllDefs
			where x.colorType == ColorType.Structure
			orderby x.displayOrder
			select x;
			TableDataGetter<ColorDef>[] array = new TableDataGetter<ColorDef>[5];
			array[0] = new TableDataGetter<ColorDef>("defName", (ColorDef x) => x.defName);
			array[1] = new TableDataGetter<ColorDef>("label", (ColorDef x) => x.label);
			array[2] = new TableDataGetter<ColorDef>("rgb", delegate(ColorDef x)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 3);
				defaultInterpolatedStringHandler.AppendLiteral("(");
				defaultInterpolatedStringHandler.AppendFormatted<int>(Mathf.RoundToInt(x.color.r * 255f));
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(Mathf.RoundToInt(x.color.r * 255f));
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(Mathf.RoundToInt(x.color.r * 2555f));
				defaultInterpolatedStringHandler.AppendLiteral(")");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			});
			array[3] = new TableDataGetter<ColorDef>("hsl", delegate(ColorDef x)
			{
				float num;
				float f;
				float f2;
				Color.RGBToHSV(x.color, out num, out f, out f2);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 3);
				defaultInterpolatedStringHandler.AppendLiteral("(");
				defaultInterpolatedStringHandler.AppendFormatted<int>(Mathf.RoundToInt(num * 360f));
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted(f.ToStringPercent());
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted(f2.ToStringPercent());
				defaultInterpolatedStringHandler.AppendLiteral(")");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			});
			array[4] = new TableDataGetter<ColorDef>("order", (ColorDef x) => x.displayOrder);
			DebugTables.MakeTablesDialog<ColorDef>(dataSources, array);
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x001282BC File Offset: 0x001264BC
		[DebugOutput]
		public static void PsychicRituals()
		{
			IEnumerable<PsychicRitualDef_InvocationCircle> allDefs = DefDatabase<PsychicRitualDef_InvocationCircle>.AllDefs;
			TableDataGetter<PsychicRitualDef_InvocationCircle>[] array = new TableDataGetter<PsychicRitualDef_InvocationCircle>[5];
			array[0] = new TableDataGetter<PsychicRitualDef_InvocationCircle>("defName", (PsychicRitualDef_InvocationCircle g) => g.defName);
			array[1] = new TableDataGetter<PsychicRitualDef_InvocationCircle>("label", (PsychicRitualDef_InvocationCircle g) => g.label);
			array[2] = new TableDataGetter<PsychicRitualDef_InvocationCircle>("duration", (PsychicRitualDef_InvocationCircle g) => g.DurationLabel());
			array[3] = new TableDataGetter<PsychicRitualDef_InvocationCircle>("cooldown", (PsychicRitualDef_InvocationCircle g) => g.CooldownLabel);
			array[4] = new TableDataGetter<PsychicRitualDef_InvocationCircle>("required offering", delegate(PsychicRitualDef_InvocationCircle g)
			{
				if (g.RequiredOffering == null)
				{
					return "-";
				}
				return g.RequiredOffering.SummaryFilterFirst;
			});
			DebugTables.MakeTablesDialog<PsychicRitualDef_InvocationCircle>(allDefs, array);
		}

		// Token: 0x06002F02 RID: 12034 RVA: 0x001283B8 File Offset: 0x001265B8
		[DebugOutput]
		public static void AnalysisDetails()
		{
			IEnumerable<AnalysisDetails> analysisDetailsForReading = Find.AnalysisManager.AnalysisDetailsForReading;
			TableDataGetter<AnalysisDetails>[] array = new TableDataGetter<AnalysisDetails>[3];
			array[0] = new TableDataGetter<AnalysisDetails>("id", (AnalysisDetails g) => g.id);
			array[1] = new TableDataGetter<AnalysisDetails>("times done", (AnalysisDetails g) => g.timesDone);
			array[2] = new TableDataGetter<AnalysisDetails>("required", (AnalysisDetails g) => g.required);
			DebugTables.MakeTablesDialog<AnalysisDetails>(analysisDetailsForReading, array);
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x00128460 File Offset: 0x00126660
		[DebugOutput]
		private static void RelicThings()
		{
			List<PreceptThingChance> relicDefs = PreceptDefOf.IdeoRelic.Worker.ThingDefs.ToList<PreceptThingChance>();
			IEnumerable<ThingDef> dataSources = from td in DefDatabase<ThingDef>.AllDefsListForReading
			where td.category == ThingCategory.Item
			select td;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[7];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("chance", delegate(ThingDef d)
			{
				PreceptThingChance preceptThingChance = relicDefs.FirstOrDefault((PreceptThingChance def) => def.def == d);
				if (preceptThingChance.chance <= 0f)
				{
					return "";
				}
				return preceptThingChance.chance;
			});
			array[2] = new TableDataGetter<ThingDef>("IsRangedWeapon", (ThingDef d) => d.IsRangedWeapon.ToStringCheckBlank());
			array[3] = new TableDataGetter<ThingDef>("IsMeleeWeapon", (ThingDef d) => d.IsMeleeWeapon.ToStringCheckBlank());
			array[4] = new TableDataGetter<ThingDef>("IsWeapon", (ThingDef d) => d.IsWeapon.ToStringCheckBlank());
			array[5] = new TableDataGetter<ThingDef>("weaponTags", delegate(ThingDef d)
			{
				if (d.weaponTags != null)
				{
					return string.Join(", ", d.weaponTags);
				}
				return "NULL";
			});
			array[6] = new TableDataGetter<ThingDef>("comps", delegate(ThingDef d)
			{
				if (d.comps != null)
				{
					return string.Join(", ", from c in d.comps
					select c.compClass.Name);
				}
				return "NULL";
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x001285E4 File Offset: 0x001267E4
		[DebugOutput]
		private static void RelicMarketValues()
		{
			IEnumerable<Tuple<ThingDef, ThingDef>> dataSources = (from td in PreceptDefOf.IdeoRelic.Worker.ThingDefs
			select td.def into td
			where td.category == ThingCategory.Item
			select td).SelectMany((ThingDef td) => from s in GenStuff.AllowedStuffsFor(td, TechLevel.Undefined, false)
			select new Tuple<ThingDef, ThingDef>(td, s));
			TableDataGetter<Tuple<ThingDef, ThingDef>>[] array = new TableDataGetter<Tuple<ThingDef, ThingDef>>[2];
			array[0] = new TableDataGetter<Tuple<ThingDef, ThingDef>>("Relic", (Tuple<ThingDef, ThingDef> d) => d.Item1.defName + "_" + d.Item2.defName);
			array[1] = new TableDataGetter<Tuple<ThingDef, ThingDef>>("market value", (Tuple<ThingDef, ThingDef> d) => d.Item1.GetStatValueAbstract(StatDefOf.MarketValue, d.Item2).ToStringMoney(null));
			DebugTables.MakeTablesDialog<Tuple<ThingDef, ThingDef>>(dataSources, array);
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x001286D0 File Offset: 0x001268D0
		[DebugOutput]
		private static void RelicStuffs()
		{
			List<ThingDef> list = (from td in PreceptDefOf.IdeoRelic.Worker.ThingDefs
			select td.def).ToList<ThingDef>();
			HashSet<ThingDef> hashSet = new HashSet<ThingDef>();
			Dictionary<ThingDef, Dictionary<ThingDef, int>> stuffSamples = new Dictionary<ThingDef, Dictionary<ThingDef, int>>();
			foreach (ThingDef thingDef in list)
			{
				Dictionary<ThingDef, int> dictionary = new Dictionary<ThingDef, int>();
				if (thingDef.MadeFromStuff)
				{
					for (int i = 0; i < 1000; i++)
					{
						ThingDef thingDef2 = Precept_Relic.GenerateStuffFor(thingDef, null);
						hashSet.Add(thingDef2);
						int num;
						if (dictionary.TryGetValue(thingDef2, out num))
						{
							dictionary[thingDef2] = num + 1;
						}
						else
						{
							dictionary.Add(thingDef2, 1);
						}
					}
				}
				stuffSamples.Add(thingDef, dictionary);
			}
			List<TableDataGetter<ThingDef>> list2 = new List<TableDataGetter<ThingDef>>();
			list2.Add(new TableDataGetter<ThingDef>("relic", (ThingDef relic) => relic.LabelCap));
			using (IEnumerator<ThingDef> enumerator2 = (from td in DefDatabase<ThingDef>.AllDefsListForReading
			where td.IsStuff
			select td).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ThingDef stuff = enumerator2.Current;
					if (hashSet.Contains(stuff))
					{
						list2.Add(new TableDataGetter<ThingDef>(stuff.LabelCap, delegate(ThingDef relic)
						{
							int num2;
							if (stuffSamples[relic].TryGetValue(stuff, out num2))
							{
								return ((float)stuffSamples[relic][stuff] / 1000f).ToStringPercent();
							}
							return 0f.ToStringPercent();
						}));
					}
				}
			}
			list2.Add(new TableDataGetter<ThingDef>("Made from stuff", (ThingDef relic) => relic.MadeFromStuff));
			list2.Add(new TableDataGetter<ThingDef>("Stuff categories", delegate(ThingDef relic)
			{
				if (relic.stuffCategories != null)
				{
					return string.Join<StuffCategoryDef>(", ", relic.stuffCategories);
				}
				return "NULL";
			}));
			DebugTables.MakeTablesDialog<ThingDef>(list, list2.ToArray());
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x00128920 File Offset: 0x00126B20
		[DebugOutput]
		public static void CollectionsMemoryUsage()
		{
			DebugOutputsMisc.<>c__DisplayClass59_0 CS$<>8__locals1;
			CS$<>8__locals1.collections = new List<ValueTuple<ICollection, string>>();
			CS$<>8__locals1.visited = new HashSet<object>();
			foreach (Type type in GenTypes.AllTypes)
			{
				if (!type.IsGenericType || !type.ContainsGenericParameters)
				{
					foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
					{
						if (typeof(ICollection).IsAssignableFrom(fieldInfo.FieldType))
						{
							ICollection collection = (ICollection)fieldInfo.GetValue(null);
							if (collection != null && CS$<>8__locals1.visited.Add(collection))
							{
								CS$<>8__locals1.collections.Add(new ValueTuple<ICollection, string>(collection, type.Name + "." + fieldInfo.Name + " (static collection)"));
							}
						}
					}
				}
			}
			foreach (Type type2 in GenTypes.AllTypes)
			{
				if ((!type2.IsGenericType || !type2.ContainsGenericParameters) && !typeof(ICollection).IsAssignableFrom(type2))
				{
					foreach (FieldInfo fieldInfo2 in type2.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
					{
						object value = fieldInfo2.GetValue(null);
						if (value != null)
						{
							DebugOutputsMisc.<CollectionsMemoryUsage>g__CheckRecursively|59_0(value, true, 0, type2.Name + "." + fieldInfo2.Name, ref CS$<>8__locals1);
						}
					}
				}
			}
			GameObject[] array = Object.FindObjectsOfType<GameObject>();
			for (int i = 0; i < array.Length; i++)
			{
				DebugOutputsMisc.<CollectionsMemoryUsage>g__CheckRecursively|59_0(array[i], false, 0, null, ref CS$<>8__locals1);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Collections found: " + CS$<>8__locals1.collections.Count.ToString("#,##0").Replace(',', ' '));
			stringBuilder.AppendLine("Total elements: " + CS$<>8__locals1.collections.Sum((ValueTuple<ICollection, string> x) => x.Item1.Count).ToString("#,##0").Replace(',', ' '));
			ValueTuple<ICollection, string> valueTuple = CS$<>8__locals1.collections.MaxBy((ValueTuple<ICollection, string> x) => x.Item1.Count);
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"Max collection size: ",
				valueTuple.Item1.Count.ToString("#,##0").Replace(',', ' '),
				" (",
				valueTuple.Item2,
				")"
			}));
			stringBuilder.AppendLine("30 largest collections:");
			foreach (ValueTuple<ICollection, string> valueTuple2 in (from x in CS$<>8__locals1.collections
			orderby x.Item1.Count descending
			select x).Take(30))
			{
				stringBuilder.AppendLine(string.Concat(new string[]
				{
					"- ",
					valueTuple2.Item2,
					" (",
					valueTuple2.Item1.Count.ToString("#,##0").Replace(',', ' '),
					")"
				}));
			}
			stringBuilder.AppendLine();
			List<ValueTuple<ICollection, string, int>> source = (from x in CS$<>8__locals1.collections
			where x.Item1 != null && x.Item1.GetType().GetProperty("Capacity") != null
			select new ValueTuple<ICollection, string, int>(x.Item1, x.Item2, (int)x.Item1.GetType().GetProperty("Capacity").GetValue(x.Item1))).ToList<ValueTuple<ICollection, string, int>>();
			ValueTuple<ICollection, string, int> valueTuple3 = source.MaxBy((ValueTuple<ICollection, string, int> x) => x.Item3);
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"Max collection capacity: ",
				valueTuple3.Item3.ToString("#,##0").Replace(',', ' '),
				" (",
				valueTuple3.Item2,
				")"
			}));
			stringBuilder.AppendLine("30 largest collections by capacity (mostly only Lists define Capacity):");
			foreach (ValueTuple<ICollection, string, int> valueTuple4 in (from x in source
			orderby x.Item3 descending
			select x).Take(30))
			{
				stringBuilder.AppendLine(string.Concat(new string[]
				{
					"- ",
					valueTuple4.Item2,
					" (",
					valueTuple4.Item3.ToString("#,##0").Replace(',', ' '),
					")"
				}));
			}
			Log.Message(stringBuilder.ToString());
			if (CS$<>8__locals1.collections.Distinct<ValueTuple<ICollection, string>>().Count<ValueTuple<ICollection, string>>() != CS$<>8__locals1.collections.Count)
			{
				Log.Error("Duplicate collections found. Could be a bug in the algorithm.");
			}
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x00128EB4 File Offset: 0x001270B4
		[DebugOutput]
		public static void Landmarks()
		{
			IEnumerable<LandmarkDef> allDefs = DefDatabase<LandmarkDef>.AllDefs;
			TableDataGetter<LandmarkDef>[] array = new TableDataGetter<LandmarkDef>[4];
			array[0] = new TableDataGetter<LandmarkDef>("defName", (LandmarkDef d) => d.defName);
			array[1] = new TableDataGetter<LandmarkDef>("commonality", (LandmarkDef d) => d.commonality);
			array[2] = new TableDataGetter<LandmarkDef>("mutators", (LandmarkDef d) => (from mc in d.mutatorChances
			orderby mc.chance descending
			select mc.mutator.defName + ((mc.chance < 1f) ? (" (" + mc.chance.ToStringPercent() + ")") : "")).ToCommaList(false, false));
			array[3] = new TableDataGetter<LandmarkDef>("biomes", (LandmarkDef d) => (from biome in DefDatabase<BiomeDef>.AllDefs
			where d.mutatorChances.All((MutatorChance mc) => mc.chance < 1f || mc.mutator.biomeWhitelist == null || mc.mutator.biomeWhitelist.Contains(biome)) && d.mutatorChances.All((MutatorChance mc) => mc.chance < 1f || mc.mutator.biomeBlacklist == null || !mc.mutator.biomeBlacklist.Contains(biome))
			select biome.defName).ToCommaList(false, false));
			DebugTables.MakeTablesDialog<LandmarkDef>(allDefs, array);
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x00128F84 File Offset: 0x00127184
		[DebugOutput]
		public static void AdjacentDistanceBetweenLayerTiles()
		{
			if (Find.WorldGrid.PlanetLayers.Count < 2)
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
			{
				int num;
				PlanetLayer from2;
				keyValuePair.Deconstruct(out num, out from2);
				PlanetLayer from = from2;
				list.Add(new DebugMenuOption(from.Def.label, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (KeyValuePair<int, PlanetLayer> keyValuePair2 in Find.WorldGrid.PlanetLayers)
					{
						int num2;
						PlanetLayer to2;
						keyValuePair2.Deconstruct(out num2, out to2);
						PlanetLayer to = to2;
						if (to != from)
						{
							list2.Add(new DebugMenuOption(to.Def.label, DebugMenuOptionMode.Action, delegate()
							{
								float num3 = 0f;
								for (int i = 0; i < 10; i++)
								{
									PlanetTile tile = from.Tiles.RandomElement<Tile>().tile;
									PlanetTile tileNeighbor = from.GetTileNeighbor(tile, 0);
									PlanetTile closestTile_NewTemp = to.GetClosestTile_NewTemp(tile, false);
									PlanetTile closestTile_NewTemp2 = to.GetClosestTile_NewTemp(tileNeighbor, false);
									num3 += to.ApproxDistanceInTiles(closestTile_NewTemp, closestTile_NewTemp2);
								}
								num3 /= 10f;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 3);
								defaultInterpolatedStringHandler.AppendLiteral("Approximate average distance between adjacent tiles on ");
								defaultInterpolatedStringHandler.AppendFormatted(from.Def.label);
								defaultInterpolatedStringHandler.AppendLiteral(" to ");
								defaultInterpolatedStringHandler.AppendFormatted(to.Def.label);
								defaultInterpolatedStringHandler.AppendLiteral(": ");
								defaultInterpolatedStringHandler.AppendFormatted<float>(num3, "0.###");
								Log.Message(defaultInterpolatedStringHandler.ToStringAndClear());
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, "Select destination layer"));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, "Select origin layer"));
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x00129040 File Offset: 0x00127240
		[DebugOutput]
		public static void LogAnyPlayerHomeMap()
		{
			Log.Message(Find.AnyPlayerHomeMap);
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x0012904C File Offset: 0x0012724C
		[DebugOutput]
		public static void GravshipBuildingChecklist()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[5];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("rotatable", delegate(ThingDef d)
			{
				if (!d.rotatable)
				{
					return " ";
				}
				return "✔";
			});
			array[2] = new TableDataGetter<ThingDef>("size", (ThingDef d) => d.size);
			array[3] = new TableDataGetter<ThingDef>("interaction", delegate(ThingDef d)
			{
				if (!d.hasInteractionCell)
				{
					return " ";
				}
				return "✔";
			});
			array[4] = new TableDataGetter<ThingDef>("valid for gravship", delegate(ThingDef d)
			{
				if ((d.size.x != d.size.z && !d.rotatable) || (!d.rotatable && d.hasInteractionCell))
				{
					return " ";
				}
				return "✔";
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002F0B RID: 12043 RVA: 0x0012916C File Offset: 0x0012736C
		[CompilerGenerated]
		internal static string <DrawerTypes>g__GetDrawUsers|0_0(ThingDef def)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (CompProperties compProperties in def.comps)
			{
				MethodInfo method = compProperties.compClass.GetMethod("PostDraw");
				if (method.DeclaringType != typeof(ThingComp))
				{
					stringBuilder.AppendLine(method.DeclaringType.Name);
				}
			}
			return stringBuilder.ToString().TrimEndNewlines().Replace(Environment.NewLine, ",");
		}

		// Token: 0x06002F0C RID: 12044 RVA: 0x00129210 File Offset: 0x00127410
		[CompilerGenerated]
		internal static bool <MeditationFoci>g__TryGetOffset|6_0(ThingDef d, int index, out FocusStrengthOffset result)
		{
			result = null;
			CompProperties_MeditationFocus compProperties = d.GetCompProperties<CompProperties_MeditationFocus>();
			if (compProperties == null)
			{
				return false;
			}
			if (compProperties.offsets.Count <= index)
			{
				return false;
			}
			result = compProperties.offsets[index];
			return true;
		}

		// Token: 0x06002F0D RID: 12045 RVA: 0x0012924C File Offset: 0x0012744C
		[CompilerGenerated]
		internal static string <MeditationFoci>g__GetOffsetClassName|6_1(ThingDef d, int index)
		{
			FocusStrengthOffset focusStrengthOffset;
			if (!DebugOutputsMisc.<MeditationFoci>g__TryGetOffset|6_0(d, index, out focusStrengthOffset))
			{
				return "";
			}
			return focusStrengthOffset.GetType().Name;
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x00129278 File Offset: 0x00127478
		[CompilerGenerated]
		internal static float <MeditationFoci>g__GetOffsetMax|6_2(ThingDef d, int index)
		{
			FocusStrengthOffset focusStrengthOffset;
			if (!DebugOutputsMisc.<MeditationFoci>g__TryGetOffset|6_0(d, index, out focusStrengthOffset))
			{
				return 0f;
			}
			return Mathf.Max(focusStrengthOffset.MaxOffset(null), 0f);
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x001292A8 File Offset: 0x001274A8
		[CompilerGenerated]
		internal static float <MeditationFoci>g__TotalMax|6_3(ThingDef d)
		{
			float num = d.GetStatValueAbstract(StatDefOf.MeditationFocusStrength, null);
			for (int i = 0; i < 5; i++)
			{
				num += DebugOutputsMisc.<MeditationFoci>g__GetOffsetMax|6_2(d, i);
			}
			return num;
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x001292DC File Offset: 0x001274DC
		[CompilerGenerated]
		internal static float <StuffBeauty>g__getStatFactorVal|9_0(ThingDef d, StatDef stat)
		{
			if (d.stuffProps.statFactors == null)
			{
				return 0f;
			}
			StatModifier statModifier = d.stuffProps.statFactors.FirstOrDefault((StatModifier fa) => fa.stat == stat);
			if (statModifier == null)
			{
				return 0f;
			}
			return statModifier.value;
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x00129338 File Offset: 0x00127538
		[CompilerGenerated]
		internal static float <StuffBeauty>g__getStatOffsetVal|9_1(ThingDef d, StatDef stat)
		{
			if (d.stuffProps.statOffsets == null)
			{
				return 0f;
			}
			StatModifier statModifier = d.stuffProps.statOffsets.FirstOrDefault((StatModifier fa) => fa.stat == stat);
			if (statModifier == null)
			{
				return 0f;
			}
			return statModifier.value;
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x00129394 File Offset: 0x00127594
		[CompilerGenerated]
		internal static string <Stuffs>g__getStatFactorString|13_0(ThingDef d, StatDef stat)
		{
			if (d.stuffProps.statFactors == null)
			{
				return "";
			}
			StatModifier statModifier = d.stuffProps.statFactors.FirstOrDefault((StatModifier fa) => fa.stat == stat);
			if (statModifier == null)
			{
				return "";
			}
			return stat.ValueToString(statModifier.value, ToStringNumberSense.Absolute, true);
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x001293FC File Offset: 0x001275FC
		[CompilerGenerated]
		internal static float <Stuffs>g__meleeDpsSharpFactorOverall|13_1(ThingDef d)
		{
			float statValueAbstract = d.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier, null);
			float statFactorFromList = d.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.MeleeWeapon_CooldownMultiplier);
			return statValueAbstract / statFactorFromList;
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x00129430 File Offset: 0x00127630
		[CompilerGenerated]
		internal static float <Stuffs>g__meleeDpsBluntFactorOverall|13_2(ThingDef d)
		{
			float statValueAbstract = d.GetStatValueAbstract(StatDefOf.BluntDamageMultiplier, null);
			float statFactorFromList = d.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.MeleeWeapon_CooldownMultiplier);
			return statValueAbstract / statFactorFromList;
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x00129464 File Offset: 0x00127664
		[CompilerGenerated]
		internal static void <CollectionsMemoryUsage>g__CheckRecursively|59_0(object obj, bool fromStatic, int depth = 0, string cameFrom = null, ref DebugOutputsMisc.<>c__DisplayClass59_0 A_4)
		{
			if (obj == null)
			{
				return;
			}
			if (obj is Type || obj is Pointer || obj is IntPtr || obj is UIntPtr || obj is ICollection)
			{
				return;
			}
			if (!A_4.visited.Add(obj))
			{
				return;
			}
			foreach (FieldInfo fieldInfo in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (typeof(ICollection).IsAssignableFrom(fieldInfo.FieldType))
				{
					ICollection collection = (ICollection)fieldInfo.GetValue(obj);
					if (collection != null)
					{
						if (!A_4.visited.Add(collection))
						{
							return;
						}
						string str;
						if (obj.GetType().Name.Contains("HashSet"))
						{
							str = (cameFrom ?? "HashSet");
						}
						else
						{
							str = obj.GetType().Name + "." + fieldInfo.Name;
						}
						A_4.collections.Add(new ValueTuple<ICollection, string>(collection, str + (fromStatic ? " (from static object)" : " (from instance)")));
					}
				}
				else
				{
					object value = fieldInfo.GetValue(obj);
					if (value != null)
					{
						DebugOutputsMisc.<CollectionsMemoryUsage>g__CheckRecursively|59_0(value, fromStatic, depth + 1, obj.GetType().Name + "." + fieldInfo.Name, ref A_4);
					}
				}
			}
		}
	}
}
