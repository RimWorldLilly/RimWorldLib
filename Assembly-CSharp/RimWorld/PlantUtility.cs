using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B3D RID: 6973
	public static class PlantUtility
	{
		// Token: 0x0600B320 RID: 45856 RVA: 0x003F1BEC File Offset: 0x003EFDEC
		public static bool GrowthSeasonNow(Map map, ThingDef plantDef)
		{
			float outdoorTemp = map.mapTemperature.OutdoorTemp;
			return outdoorTemp > plantDef.plant.minGrowthTemperature && outdoorTemp < plantDef.plant.maxGrowthTemperature;
		}

		// Token: 0x0600B321 RID: 45857 RVA: 0x003F1C24 File Offset: 0x003EFE24
		public static bool GrowthSeasonNow(IntVec3 c, Map map, ThingDef plantDef)
		{
			if (c.GetRoomOrAdjacent(map, RegionType.Set_All) == null)
			{
				return false;
			}
			float temperature = c.GetTemperature(map);
			return temperature > plantDef.plant.minGrowthTemperature && temperature < plantDef.plant.maxGrowthTemperature;
		}

		// Token: 0x0600B322 RID: 45858 RVA: 0x003F1C64 File Offset: 0x003EFE64
		public static bool SnowAllowsPlanting(IntVec3 c, Map map)
		{
			return c.GetSnowDepth(map) < 0.2f;
		}

		// Token: 0x0600B323 RID: 45859 RVA: 0x003F1C74 File Offset: 0x003EFE74
		public static bool SandAllowsPlanting(IntVec3 c, Map map)
		{
			return c.GetSandDepth(map) < 0.2f;
		}

		// Token: 0x0600B324 RID: 45860 RVA: 0x003F1C84 File Offset: 0x003EFE84
		public static bool CanNowPlantAt(this ThingDef plantDef, IntVec3 c, Map map, bool canWipePlantsExceptTree = false)
		{
			if (!plantDef.CanEverPlantAt(c, map, canWipePlantsExceptTree, false))
			{
				return false;
			}
			foreach (Thing thing in c.GetThingList(map))
			{
				if (map.designationManager.DesignationOn(thing, DesignationDefOf.Uninstall) != null)
				{
					return false;
				}
				if (map.designationManager.DesignationOn(thing, DesignationDefOf.Deconstruct) != null)
				{
					return false;
				}
				Building building = thing as Building;
				Blueprint_Install blueprint_Install;
				if (building != null && map.listerBuildings.TryGetReinstallBlueprint(building, out blueprint_Install))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B325 RID: 45861 RVA: 0x003F1D30 File Offset: 0x003EFF30
		public static AcceptanceReport CanEverPlantAt(this ThingDef plantDef, IntVec3 c, Map map, out Thing blockingThing, bool canWipePlantsExceptTree = false, bool checkMapTemperature = true, bool writeNoReason = false)
		{
			blockingThing = null;
			if (plantDef.category != ThingCategory.Plant)
			{
				Log.Error("Checking CanGrowAt with " + ((plantDef != null) ? plantDef.ToString() : null) + " which is not a plant.");
			}
			if (!c.InBounds(map))
			{
				if (!writeNoReason)
				{
					return "OutOfBounds".Translate();
				}
				return false;
			}
			else
			{
				TerrainDef terrain = c.GetTerrain(map);
				if (!plantDef.plant.completelyIgnoreFertility && map.fertilityGrid.FertilityAt(c) < plantDef.plant.fertilityMin)
				{
					if (!writeNoReason)
					{
						return "MessageWarningNotEnoughFertility".Translate();
					}
					return false;
				}
				else if (checkMapTemperature && (map.TileInfo.MinTemperature > plantDef.plant.maxGrowthTemperature || map.TileInfo.MaxTemperature < plantDef.plant.minGrowthTemperature))
				{
					if (!writeNoReason)
					{
						return "CannotPlantExtremeTemp".Translate();
					}
					return false;
				}
				else
				{
					if (c.IsPolluted(map))
					{
						if (plantDef.plant.pollution == Pollution.CleanOnly)
						{
							if (!writeNoReason)
							{
								return "MessageWarningPollutedCell".Translate();
							}
							return false;
						}
					}
					else if (plantDef.plant.pollution == Pollution.PollutedOnly)
					{
						if (!writeNoReason)
						{
							return "MessageWarningNotPollutedCell".Translate();
						}
						return false;
					}
					if (!plantDef.plant.terraformable || CompTerraformer.CanEverConvertCell(c, map, null))
					{
						List<Thing> list = map.thingGrid.ThingsListAt(c);
						bool flag = false;
						for (int i = 0; i < list.Count; i++)
						{
							if (list[i] is Building_PlantGrower)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							if (plantDef.plant.WildTerrainTags.Count > 0 && !plantDef.plant.WildTerrainTags.Overlaps(terrain.tags.OrElseEmptyEnumerable<string>()))
							{
								if (!writeNoReason)
								{
									return "CannotPlantMissingTerrainTag".Translate();
								}
								return false;
							}
							else if (plantDef.plant.terrainBlacklist != null && plantDef.plant.terrainBlacklist.Contains(terrain))
							{
								if (!writeNoReason)
								{
									return "CannotPlantMissingTerrainTag".Translate(terrain);
								}
								return false;
							}
						}
						int j = 0;
						while (j < list.Count)
						{
							Thing thing = list[j];
							if (!flag && thing.def.BlocksPlanting(canWipePlantsExceptTree))
							{
								blockingThing = thing;
								if (!writeNoReason)
								{
									return "BlockedBy".Translate(thing);
								}
								return false;
							}
							else
							{
								if (plantDef.passability == Traversability.Impassable)
								{
									if (thing.def.category == ThingCategory.Pawn || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Building)
									{
										blockingThing = thing;
										if (!writeNoReason)
										{
											return "BlockedBy".Translate(thing);
										}
										return false;
									}
									else if (thing.def.category == ThingCategory.Plant && canWipePlantsExceptTree && thing.def.plant.IsTree)
									{
										blockingThing = thing;
										if (!writeNoReason)
										{
											return "BlockedBy".Translate(thing);
										}
										return false;
									}
								}
								j++;
							}
						}
						if (plantDef.passability == Traversability.Impassable)
						{
							for (int k = 0; k < 4; k++)
							{
								IntVec3 c2 = c + GenAdj.CardinalDirections[k];
								if (c2.InBounds(map))
								{
									Building edifice = c2.GetEdifice(map);
									if (edifice != null && edifice.def.IsDoor)
									{
										blockingThing = edifice;
										if (!writeNoReason)
										{
											return "BlockedBy".Translate(edifice);
										}
										return false;
									}
								}
							}
						}
						return true;
					}
					if (!writeNoReason)
					{
						return "MessageCannotBePlacedOn".Translate(terrain);
					}
					return false;
				}
			}
		}

		// Token: 0x0600B326 RID: 45862 RVA: 0x003F2108 File Offset: 0x003F0308
		public static bool CanEverPlantAt(this ThingDef plantDef, IntVec3 c, Map map, bool canWipePlantsExceptTree = false, bool checkMapTemperature = true)
		{
			Thing thing;
			return plantDef.CanEverPlantAt(c, map, out thing, canWipePlantsExceptTree, checkMapTemperature, true).Accepted;
		}

		// Token: 0x0600B327 RID: 45863 RVA: 0x003F212C File Offset: 0x003F032C
		public static void LogPlantProportions()
		{
			Dictionary<ThingDef, float> dictionary = new Dictionary<ThingDef, float>();
			foreach (ThingDef key in Find.CurrentMap.wildPlantSpawner.AllWildPlants)
			{
				dictionary.Add(key, 0f);
			}
			float num = 0f;
			foreach (IntVec3 c in Find.CurrentMap.AllCells)
			{
				Plant plant = c.GetPlant(Find.CurrentMap);
				if (plant != null && dictionary.ContainsKey(plant.def))
				{
					Dictionary<ThingDef, float> dictionary2 = dictionary;
					ThingDef key2 = plant.def;
					float num2 = dictionary2[key2];
					dictionary2[key2] = num2 + 1f;
					num += 1f;
				}
			}
			foreach (ThingDef thingDef in Find.CurrentMap.wildPlantSpawner.AllWildPlants)
			{
				Dictionary<ThingDef, float> dictionary3 = dictionary;
				ThingDef key2 = thingDef;
				dictionary3[key2] /= num;
			}
			Dictionary<ThingDef, float> dictionary4 = PlantUtility.CalculateDesiredPlantProportions(Find.CurrentMap.Biome);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("PLANT           EXPECTED             FOUND");
			foreach (ThingDef thingDef2 in Find.CurrentMap.wildPlantSpawner.AllWildPlants)
			{
				stringBuilder.AppendLine(thingDef2.LabelCap + "       " + dictionary4[thingDef2].ToStringPercent() + "        " + dictionary[thingDef2].ToStringPercent());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600B328 RID: 45864 RVA: 0x003F2340 File Offset: 0x003F0540
		private static Dictionary<ThingDef, float> CalculateDesiredPlantProportions(BiomeDef biome)
		{
			Dictionary<ThingDef, float> dictionary = new Dictionary<ThingDef, float>();
			float num = 0f;
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.plant != null)
				{
					float num2 = biome.CommonalityOfPlant(thingDef);
					dictionary.Add(thingDef, num2);
					num += num2;
				}
			}
			foreach (ThingDef thingDef2 in biome.AllWildPlants)
			{
				Dictionary<ThingDef, float> dictionary2 = dictionary;
				ThingDef key = thingDef2;
				dictionary2[key] /= num;
			}
			return dictionary;
		}

		// Token: 0x0600B329 RID: 45865 RVA: 0x003F2408 File Offset: 0x003F0608
		public static IEnumerable<ThingDef> ValidPlantTypesForGrowers(List<IPlantToGrowSettable> sel)
		{
			PlantUtility.<ValidPlantTypesForGrowers>d__9 <ValidPlantTypesForGrowers>d__ = new PlantUtility.<ValidPlantTypesForGrowers>d__9(-2);
			<ValidPlantTypesForGrowers>d__.<>3__sel = sel;
			return <ValidPlantTypesForGrowers>d__;
		}

		// Token: 0x0600B32A RID: 45866 RVA: 0x003F2418 File Offset: 0x003F0618
		public static bool CanSowOnGrower(ThingDef plantDef, object obj)
		{
			IPlantToGrowSettable plantToGrowSettable = obj as IPlantToGrowSettable;
			if (plantToGrowSettable != null && !PollutionUtility.CanPlantAt(plantDef, plantToGrowSettable))
			{
				return false;
			}
			if (obj is Zone)
			{
				return plantDef.plant.sowTags.Contains("Ground");
			}
			Thing thing = obj as Thing;
			return thing != null && thing.def.building != null && plantDef.plant.sowTags.Contains(thing.def.building.sowTag);
		}

		// Token: 0x0600B32B RID: 45867 RVA: 0x003F2494 File Offset: 0x003F0694
		public static Thing AdjacentSowBlocker(ThingDef plantDef, IntVec3 c, Map map)
		{
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCells[i];
				if (c2.InBounds(map))
				{
					Plant plant = c2.GetPlant(map);
					if (plant != null && (plant.def.plant.blockAdjacentSow || (plantDef.plant.blockAdjacentSow && plant.sown)))
					{
						return plant;
					}
				}
			}
			return null;
		}

		// Token: 0x0600B32C RID: 45868 RVA: 0x003F24FD File Offset: 0x003F06FD
		public static byte GetWindExposure(Plant plant)
		{
			return (byte)Mathf.Min(255f * plant.def.plant.topWindExposure, 255f);
		}

		// Token: 0x0600B32D RID: 45869 RVA: 0x003F2520 File Offset: 0x003F0720
		public static void SetWindExposureColors(Color32[] colors, Plant plant)
		{
			colors[1].a = (colors[2].a = PlantUtility.GetWindExposure(plant));
			colors[0].a = (colors[3].a = 0);
		}

		// Token: 0x0600B32E RID: 45870 RVA: 0x003F256C File Offset: 0x003F076C
		public static void LogFallColorForYear()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Fall color amounts for each latitude and each day of the year");
			stringBuilder.AppendLine("---------------------------------------");
			stringBuilder.Append("Lat".PadRight(6));
			for (int i = -90; i <= 90; i += 10)
			{
				stringBuilder.Append((i.ToString() + "d").PadRight(6));
			}
			stringBuilder.AppendLine();
			for (int j = 0; j < 60; j += 5)
			{
				stringBuilder.Append(j.ToString().PadRight(6));
				for (int k = -90; k <= 90; k += 10)
				{
					stringBuilder.Append(PlantFallColors.GetFallColorFactor((float)k, j).ToString("F3").PadRight(6));
				}
				stringBuilder.AppendLine();
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600B32F RID: 45871 RVA: 0x003F2642 File Offset: 0x003F0842
		public static float GrowthRateFactorFor_Fertility(ThingDef def, float fertilityAtCell)
		{
			if (def.plant.completelyIgnoreFertility)
			{
				return 1f;
			}
			return fertilityAtCell * def.plant.fertilitySensitivity + (1f - def.plant.fertilitySensitivity);
		}

		// Token: 0x0600B330 RID: 45872 RVA: 0x003F2678 File Offset: 0x003F0878
		public static float GrowthRateFactorFor_Light(ThingDef def, float glow)
		{
			if (def.plant.growMinGlow == def.plant.growOptimalGlow && glow == def.plant.growOptimalGlow)
			{
				return 1f;
			}
			return GenMath.InverseLerp(def.plant.growMinGlow, def.plant.growOptimalGlow, glow);
		}

		// Token: 0x0600B331 RID: 45873 RVA: 0x003F26D0 File Offset: 0x003F08D0
		public static float GrowthRateFactorFor_Temperature(ThingDef plant, float cellTemp)
		{
			if (cellTemp < plant.plant.minOptimalGrowthTemperature)
			{
				return Mathf.InverseLerp(plant.plant.minGrowthTemperature, plant.plant.minOptimalGrowthTemperature, cellTemp);
			}
			if (cellTemp > plant.plant.maxOptimalGrowthTemperature)
			{
				return Mathf.InverseLerp(plant.plant.maxGrowthTemperature, plant.plant.maxOptimalGrowthTemperature, cellTemp);
			}
			return 1f;
		}

		// Token: 0x0600B332 RID: 45874 RVA: 0x003F2738 File Offset: 0x003F0938
		public static float NutritionFactorFromGrowth(ThingDef def, float plantGrowth)
		{
			if (def.plant.Sowable)
			{
				return plantGrowth;
			}
			return Mathf.Lerp(0.5f, 1f, plantGrowth);
		}

		// Token: 0x0600B333 RID: 45875 RVA: 0x003F275C File Offset: 0x003F095C
		public static bool PawnWillingToCutPlant_Job(Thing plant, Pawn pawn)
		{
			return !plant.def.plant.IsTree || !plant.def.plant.treeLoversCareIfChopped || new HistoryEvent(HistoryEventDefOf.CutTree, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job();
		}

		// Token: 0x0600B334 RID: 45876 RVA: 0x003F27A9 File Offset: 0x003F09A9
		public static bool TreeMarkedForExtraction(Thing plant)
		{
			return plant.def.plant.IsTree && plant.MapHeld.designationManager.DesignationOn(plant, DesignationDefOf.ExtractTree) != null;
		}
	}
}
