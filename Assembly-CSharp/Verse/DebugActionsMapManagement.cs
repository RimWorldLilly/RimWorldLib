using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using RimWorld;
using RimWorld.BaseGen;
using RimWorld.Planet;
using RimWorld.SketchGen;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005B7 RID: 1463
	public static class DebugActionsMapManagement
	{
		// Token: 0x06002C89 RID: 11401 RVA: 0x0010E008 File Offset: 0x0010C208
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> UseScatterer()
		{
			return DebugTools_MapGen.Options_Scatterers();
		}

		// Token: 0x06002C8A RID: 11402 RVA: 0x0010E010 File Offset: 0x0010C210
		[DebugAction("Map", "BaseGen", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> BaseGen()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (string localSymbol2 in (from x in DefDatabase<RuleDef>.AllDefs
			select x.symbol).Distinct<string>())
			{
				string localSymbol = localSymbol2;
				list.Add(new DebugActionNode(localSymbol, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						DebugTool tool = null;
						IntVec3 firstCorner;
						Action <>9__3;
						tool = new DebugTool("first corner...", delegate()
						{
							firstCorner = UI.MouseCell();
							string label = "second corner...";
							Action clickAction;
							if ((clickAction = <>9__3) == null)
							{
								clickAction = (<>9__3 = delegate()
								{
									IntVec3 second = UI.MouseCell();
									CellRect rect = CellRect.FromLimits(firstCorner, second).ClipInsideMap(Find.CurrentMap);
									RimWorld.BaseGen.BaseGen.globalSettings.map = Find.CurrentMap;
									RimWorld.BaseGen.BaseGen.symbolStack.Push(localSymbol, rect, null);
									RimWorld.BaseGen.BaseGen.Generate();
									DebugTools.curTool = tool;
								});
							}
							DebugTools.curTool = new DebugTool(label, clickAction, firstCorner);
						}, null);
						DebugTools.curTool = tool;
					}
				});
			}
			return list;
		}

		// Token: 0x06002C8B RID: 11403 RVA: 0x0010E0B8 File Offset: 0x0010C2B8
		[DebugAction("Map", "SketchGen", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> SketchGen()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SketchResolverDef localResolver2 in from x in DefDatabase<SketchResolverDef>.AllDefs
			where x.isRoot
			select x)
			{
				SketchResolverDef localResolver = localResolver2;
				DebugActionNode debugActionNode = new DebugActionNode(localResolver.defName, DebugActionType.Action, null, null);
				if (localResolver == SketchResolverDefOf.Monument || localResolver == SketchResolverDefOf.MonumentRuin)
				{
					new List<DebugMenuOption>();
					for (int i = 1; i <= 60; i++)
					{
						int localIndex = i;
						debugActionNode.AddChild(new DebugActionNode(localIndex.ToString(), DebugActionType.ToolMap, null, null)
						{
							action = delegate()
							{
								SketchResolveParams parms = default(SketchResolveParams);
								parms.sketch = new Sketch();
								parms.monumentSize = new IntVec2?(new IntVec2(localIndex, localIndex));
								RimWorld.SketchGen.SketchGen.Generate(localResolver, parms).Spawn(Find.CurrentMap, UI.MouseCell(), null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, false, null, false, true, null, null, null);
							}
						});
					}
				}
				else
				{
					debugActionNode.actionType = DebugActionType.ToolMap;
					debugActionNode.action = delegate()
					{
						SketchResolveParams parms = default(SketchResolveParams);
						parms.sketch = new Sketch();
						RimWorld.SketchGen.SketchGen.Generate(localResolver, parms).Spawn(Find.CurrentMap, UI.MouseCell(), null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, false, null, false, true, null, null, null);
					};
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x06002C8C RID: 11404 RVA: 0x0010E1EC File Offset: 0x0010C3EC
		[DebugAction("Map", "Set terrain (rect)", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static List<DebugActionNode> SetTerrainRect()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (TerrainDef localDef2 in from terr in DefDatabase<TerrainDef>.AllDefs
			where !terr.temporary
			select terr)
			{
				TerrainDef localDef = localDef2;
				Action<CellRect> <>9__2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						string defName = localDef.defName;
						Action<CellRect> rectAction;
						if ((rectAction = <>9__2) == null)
						{
							rectAction = (<>9__2 = delegate(CellRect rect)
							{
								foreach (IntVec3 c in rect)
								{
									Find.CurrentMap.terrainGrid.SetTerrain(c, localDef);
								}
							});
						}
						DebugToolsGeneral.GenericRectTool(defName, rectAction, false);
					}
				});
			}
			return list;
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x0010E294 File Offset: 0x0010C494
		[DebugAction("Map", "Set temp terrain (rect)", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static List<DebugActionNode> SetTempTerrainRect()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (TerrainDef localDef2 in from terr in DefDatabase<TerrainDef>.AllDefs
			where terr.temporary
			select terr)
			{
				TerrainDef localDef = localDef2;
				Action<CellRect> <>9__2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						string defName = localDef.defName;
						Action<CellRect> rectAction;
						if ((rectAction = <>9__2) == null)
						{
							rectAction = (<>9__2 = delegate(CellRect rect)
							{
								foreach (IntVec3 c in rect)
								{
									Find.CurrentMap.terrainGrid.SetTempTerrain(c, localDef);
								}
							});
						}
						DebugToolsGeneral.GenericRectTool(defName, rectAction, false);
					}
				});
			}
			return list;
		}

		// Token: 0x06002C8E RID: 11406 RVA: 0x0010E33C File Offset: 0x0010C53C
		[DebugAction("Map", "Clear temp terrain (rect)", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static void ClearTempTerrainRect()
		{
			DebugToolsGeneral.GenericRectTool("Clear temp terrain", delegate(CellRect rect)
			{
				foreach (IntVec3 c in rect)
				{
					Find.CurrentMap.terrainGrid.RemoveTempTerrain(c, false, false);
				}
			}, false);
		}

		// Token: 0x06002C8F RID: 11407 RVA: 0x0010E368 File Offset: 0x0010C568
		[DebugAction("Lighting", "Log lights affecting cell", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, actionType = DebugActionType.ToolMap)]
		private static void LogLightsAffectinCell()
		{
			Find.CurrentMap.glowGrid.DevPrintLightIdsAffectingCell(UI.MouseCell());
		}

		// Token: 0x06002C90 RID: 11408 RVA: 0x0010E37E File Offset: 0x0010C57E
		[DebugAction("Lighting", "Regen glow grid cells (rect)", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static void RegenGlowGridCells()
		{
			DebugToolsGeneral.GenericRectTool("Regen glow grid cells", delegate(CellRect rect)
			{
				Find.CurrentMap.glowGrid.DevDirtyRect(rect);
			}, false);
		}

		// Token: 0x06002C91 RID: 11409 RVA: 0x0010E3AA File Offset: 0x0010C5AA
		[DebugAction("Map", "Pollute (rect)", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100, requiresBiotech = true)]
		private static void PolluteRect()
		{
			DebugToolsGeneral.GenericRectTool("Pollute", delegate(CellRect rect)
			{
				foreach (IntVec3 cell in rect)
				{
					Find.CurrentMap.pollutionGrid.SetPolluted(cell, true, false);
				}
			}, false);
		}

		// Token: 0x06002C92 RID: 11410 RVA: 0x0010E3D6 File Offset: 0x0010C5D6
		[DebugAction("Map", "Unpollute (rect)", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100, requiresBiotech = true)]
		private static void UnpolluteRect()
		{
			DebugToolsGeneral.GenericRectTool("Unpollute", delegate(CellRect rect)
			{
				foreach (IntVec3 cell in rect)
				{
					Find.CurrentMap.pollutionGrid.SetPolluted(cell, false, false);
				}
			}, false);
		}

		// Token: 0x06002C93 RID: 11411 RVA: 0x0010E402 File Offset: 0x0010C602
		[DebugAction("Map", "Make rock (rect)", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static void MakeRock()
		{
			DebugToolsGeneral.GenericRectTool("Make rock", delegate(CellRect rect)
			{
				foreach (IntVec3 loc in rect)
				{
					GenSpawn.Spawn(ThingDefOf.Granite, loc, Find.CurrentMap, WipeMode.Vanish);
				}
			}, false);
		}

		// Token: 0x06002C94 RID: 11412 RVA: 0x0010E42E File Offset: 0x0010C62E
		[DebugAction("Map", "Grow pollution (x10 cell)", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -200, requiresBiotech = true)]
		private static void PolluteCellTen()
		{
			PollutionUtility.GrowPollutionAt(UI.MouseCell(), Find.CurrentMap, 10, null, false, null);
		}

		// Token: 0x06002C95 RID: 11413 RVA: 0x0010E444 File Offset: 0x0010C644
		[DebugAction("Map", "Grow pollution (x100 cell)", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -200, requiresBiotech = true)]
		private static void PolluteCellHundred()
		{
			PollutionUtility.GrowPollutionAt(UI.MouseCell(), Find.CurrentMap, 100, null, false, null);
		}

		// Token: 0x06002C96 RID: 11414 RVA: 0x0010E45A File Offset: 0x0010C65A
		[DebugAction("Map", "Grow pollution (x1000 cell)", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -200, requiresBiotech = true)]
		private static void PolluteCellThousand()
		{
			PollutionUtility.GrowPollutionAt(UI.MouseCell(), Find.CurrentMap, 1000, null, false, null);
		}

		// Token: 0x06002C97 RID: 11415 RVA: 0x0010E474 File Offset: 0x0010C674
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static List<DebugActionNode> AddGameCondition()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (GameConditionDef localDef2 in DefDatabase<GameConditionDef>.AllDefs)
			{
				GameConditionDef localDef = localDef2;
				DebugActionNode debugActionNode = new DebugActionNode(localDef.LabelCap, DebugActionType.Action, null, null);
				debugActionNode.AddChild(new DebugActionNode("Permanent", DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						GameCondition gameCondition = GameConditionMaker.MakeCondition(localDef, -1);
						gameCondition.Permanent = true;
						if (Find.CurrentMap != null)
						{
							Find.CurrentMap.GameConditionManager.RegisterCondition(gameCondition);
							return;
						}
						Find.World.GameConditionManager.RegisterCondition(gameCondition);
					}
				});
				for (int i = 2500; i <= 60000; i += 2500)
				{
					int localTicks = i;
					debugActionNode.AddChild(new DebugActionNode(localTicks.ToStringTicksToPeriod(true, false, true, true, false) ?? "", DebugActionType.Action, null, null)
					{
						action = delegate()
						{
							GameCondition gameCondition = GameConditionMaker.MakeCondition(localDef, -1);
							gameCondition.Duration = localTicks;
							if (Find.CurrentMap != null)
							{
								Find.CurrentMap.GameConditionManager.RegisterCondition(gameCondition);
								return;
							}
							Find.World.GameConditionManager.RegisterCondition(gameCondition);
						}
					});
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x06002C98 RID: 11416 RVA: 0x0010E58C File Offset: 0x0010C78C
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static List<DebugActionNode> RemoveGameCondition()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (GameConditionDef localDef2 in DefDatabase<GameConditionDef>.AllDefs)
			{
				GameConditionDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.LabelCap, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						GameCondition activeCondition = Find.CurrentMap.gameConditionManager.GetActiveCondition(localDef);
						if (activeCondition != null)
						{
							activeCondition.Duration = 0;
						}
					},
					visibilityGetter = (() => Find.CurrentMap != null && Find.CurrentMap.gameConditionManager.ConditionIsActive(localDef))
				});
			}
			return list;
		}

		// Token: 0x06002C99 RID: 11417 RVA: 0x0010E628 File Offset: 0x0010C828
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RefogMap()
		{
			FloodFillerFog.DebugRefogMap(Find.CurrentMap);
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x0010E634 File Offset: 0x0010C834
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> UseGenStep()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (Type localGenStep2 in typeof(GenStep).AllSubclassesNonAbstract())
			{
				Type localGenStep = localGenStep2;
				list.Add(new DebugActionNode(localGenStep.Name, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						((GenStep)Activator.CreateInstance(localGenStep)).Generate(Find.CurrentMap, default(GenStepParams));
					}
				});
			}
			return list;
		}

		// Token: 0x06002C9B RID: 11419 RVA: 0x0010E6C8 File Offset: 0x0010C8C8
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RegenSection()
		{
			Find.CurrentMap.mapDrawer.SectionAt(UI.MouseCell()).RegenerateAllLayers();
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x0010E6E3 File Offset: 0x0010C8E3
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RegenAllMapMeshSections()
		{
			Find.CurrentMap.mapDrawer.RegenerateEverythingNow();
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x0010E6F4 File Offset: 0x0010C8F4
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddSnow()
		{
			WeatherBuildupUtility.AddSnowRadial(UI.MouseCell(), Find.CurrentMap, 5f, 1f);
		}

		// Token: 0x06002C9E RID: 11422 RVA: 0x0010E70F File Offset: 0x0010C90F
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddSand()
		{
			WeatherBuildupUtility.AddSandRadial(UI.MouseCell(), Find.CurrentMap, 5f, 1f);
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x0010E72A File Offset: 0x0010C92A
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemoveSnow()
		{
			WeatherBuildupUtility.AddSnowRadial(UI.MouseCell(), Find.CurrentMap, 5f, -1f);
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x0010E745 File Offset: 0x0010C945
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemoveSand()
		{
			WeatherBuildupUtility.AddSandRadial(UI.MouseCell(), Find.CurrentMap, 5f, -1f);
		}

		// Token: 0x06002CA1 RID: 11425 RVA: 0x0010E760 File Offset: 0x0010C960
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ClearAllSnow()
		{
			foreach (IntVec3 c in Find.CurrentMap.AllCells)
			{
				Find.CurrentMap.snowGrid.SetDepth(c, 0f);
			}
		}

		// Token: 0x06002CA2 RID: 11426 RVA: 0x0010E7C0 File Offset: 0x0010C9C0
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresOdyssey = true)]
		private static void ClearAllSand()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			foreach (IntVec3 c in Find.CurrentMap.AllCells)
			{
				Find.CurrentMap.sandGrid.SetDepth(c, 0f);
			}
		}

		// Token: 0x06002CA3 RID: 11427 RVA: 0x0010E828 File Offset: 0x0010CA28
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing, hideInSubMenu = true)]
		private static void GenerateMap()
		{
			PlanetTile tile = TileFinder.RandomStartingTile();
			MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(tile.LayerDef.SettlementWorldObjectDef);
			mapParent.Tile = tile;
			mapParent.SetFaction(Faction.OfPlayer);
			Find.WorldObjects.Add(mapParent);
			GetOrGenerateMapUtility.GetOrGenerateMap(mapParent.Tile, new IntVec3(50, 1, 50), null, null, false);
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x0010E888 File Offset: 0x0010CA88
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static void DestroyMap()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate()
				{
					Current.Game.DeinitAndRemoveMap(map, true);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x0010E8FC File Offset: 0x0010CAFC
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing, hideInSubMenu = true)]
		private static void LeakMap()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate()
				{
					DebugActionsMapManagement.mapLeak = map;
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x0010E96D File Offset: 0x0010CB6D
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing, hideInSubMenu = true)]
		private static void PrintLeakedMap()
		{
			Log.Message(string.Format("Leaked map {0}", DebugActionsMapManagement.mapLeak));
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x0010E984 File Offset: 0x0010CB84
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing, actionType = DebugActionType.ToolMap)]
		private static void Transfer()
		{
			List<Thing> toTransfer = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>();
			if (!toTransfer.Any<Thing>())
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map != Find.CurrentMap)
				{
					Predicate<IntVec3> <>9__1;
					list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate()
					{
						for (int j = 0; j < toTransfer.Count; j++)
						{
							Map map;
							IntVec3 center = map.Center;
							map = map;
							int squareRadius = Mathf.Max(map.Size.x, map.Size.z);
							Predicate<IntVec3> validator;
							if ((validator = <>9__1) == null)
							{
								validator = (<>9__1 = ((IntVec3 x) => !x.Fogged(map) && x.Standable(map)));
							}
							IntVec3 center2;
							if (CellFinder.TryFindRandomCellNear(center, map, squareRadius, validator, out center2, -1))
							{
								toTransfer[j].DeSpawn(DestroyMode.Vanish);
								GenPlace.TryPlaceThing(toTransfer[j], center2, map, ThingPlaceMode.Near, null, null, null, 1);
							}
							else
							{
								Log.Error("Could not find spawn cell.");
							}
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x0010EA44 File Offset: 0x0010CC44
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static void ChangeMap()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map != Find.CurrentMap)
				{
					list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate()
					{
						Current.Game.CurrentMap = map;
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x0010EAC4 File Offset: 0x0010CCC4
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static void RegenerateCurrentMap()
		{
			RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
			PlanetTile tile = Find.CurrentMap.Tile;
			MapParent parent = Find.CurrentMap.Parent;
			IntVec3 size = Find.CurrentMap.Size;
			bool isPocketMap = Find.CurrentMap.IsPocketMap;
			Current.Game.DeinitAndRemoveMap(Find.CurrentMap, true);
			if (isPocketMap)
			{
				Map currentMap = PocketMapUtility.GeneratePocketMap(size, parent.MapGeneratorDef, null, Find.AnyPlayerHomeMap);
				Current.Game.CurrentMap = currentMap;
			}
			else
			{
				Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, size, parent.def, null, false);
				Current.Game.CurrentMap = orGenerateMap;
			}
			Find.World.renderer.wantedMode = WorldRenderMode.None;
			Find.CameraDriver.SetRootPosAndSize(rememberedCameraPos.rootPos, rememberedCameraPos.rootSize);
		}

		// Token: 0x06002CAA RID: 11434 RVA: 0x0010EB80 File Offset: 0x0010CD80
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static void RegenerateCurrentMapStepped()
		{
			RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
			PlanetTile tile = Find.CurrentMap.Tile;
			MapParent parent = Find.CurrentMap.Parent;
			IntVec3 size = Find.CurrentMap.Size;
			bool isPocketMap = Find.CurrentMap.IsPocketMap;
			Current.Game.DeinitAndRemoveMap(Find.CurrentMap, true);
			if (isPocketMap)
			{
				Log.Error("Pocket maps are not supported for stepped generation (yet)");
			}
			else
			{
				Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, size, parent.def, null, true);
				Current.Game.CurrentMap = orGenerateMap;
			}
			Find.World.renderer.wantedMode = WorldRenderMode.None;
			Find.CameraDriver.SetRootPosAndSize(rememberedCameraPos.rootPos, rememberedCameraPos.rootSize);
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x0010EC24 File Offset: 0x0010CE24
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static void DoNextGenStep()
		{
			if (!MapGenerator.DebugDoNextGenStep(Current.Game.CurrentMap))
			{
				Log.Message("No more gen steps to do.");
			}
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x0010EC44 File Offset: 0x0010CE44
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static void GenerateMapWithCaves()
		{
			PlanetTile tile = TileFinder.RandomSettlementTileFor(Faction.OfPlayer, false, (PlanetTile x) => Find.World.HasCaves(x));
			if (Find.CurrentMap != null)
			{
				Find.CurrentMap.Parent.Destroy();
			}
			MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(tile.LayerDef.SettlementWorldObjectDef);
			mapParent.Tile = tile;
			mapParent.SetFaction(Faction.OfPlayer);
			Find.WorldObjects.Add(mapParent);
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, Find.World.info.initialMapSize, null, null, false);
			Current.Game.CurrentMap = orGenerateMap;
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x0010ECFC File Offset: 0x0010CEFC
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static List<DebugActionNode> RunMapGenerator()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (MapGeneratorDef defLocal2 in DefDatabase<MapGeneratorDef>.AllDefsListForReading)
			{
				MapGeneratorDef defLocal = defLocal2;
				list.Add(new DebugActionNode(defLocal.defName, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						PlanetTile tile2 = (from tile in Find.WorldGrid.Surface.Tiles
						where tile.PrimaryBiome.canBuildBase
						select tile).RandomElement<Tile>().tile;
						MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(tile2.LayerDef.SettlementWorldObjectDef);
						mapParent.Tile = tile2;
						mapParent.SetFaction(Faction.OfPlayer);
						Find.WorldObjects.Add(mapParent);
						Map currentMap = MapGenerator.GenerateMap(Find.World.info.initialMapSize, mapParent, defLocal, null, null, false, false);
						Current.Game.CurrentMap = currentMap;
					}
				});
			}
			return list;
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x0010ED88 File Offset: 0x0010CF88
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static List<DebugActionNode> GeneratePocketMap()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (MapGeneratorDef defLocal2 in DefDatabase<MapGeneratorDef>.AllDefsListForReading)
			{
				MapGeneratorDef defLocal = defLocal2;
				list.Add(new DebugActionNode(defLocal.defName, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						Map currentMap = PocketMapUtility.GeneratePocketMap(new IntVec3(100, 1, 100), defLocal, null, Find.CurrentMap);
						Current.Game.CurrentMap = currentMap;
					}
				});
			}
			return list;
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x0010EE14 File Offset: 0x0010D014
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static void ForceReformInCurrentMap()
		{
			if (Find.CurrentMap != null)
			{
				MapParent mapParent = Find.CurrentMap.Parent;
				List<Pawn> list = new List<Pawn>();
				if (Dialog_FormCaravan.AllSendablePawns(mapParent.Map, true).Any((Pawn x) => x.IsColonist))
				{
					Messages.Message("MessageYouHaveToReformCaravanNow".Translate(), new GlobalTargetInfo(mapParent.Tile), MessageTypeDefOf.NeutralEvent, true);
					Current.Game.CurrentMap = mapParent.Map;
					Dialog_FormCaravan window = new Dialog_FormCaravan(mapParent.Map, true, delegate()
					{
						if (mapParent.HasMap)
						{
							mapParent.Destroy();
						}
					}, true, null);
					Find.WindowStack.Add(window);
					return;
				}
				list.Clear();
				list.AddRange(from x in mapParent.Map.mapPawns.AllPawns
				where x.Faction == Faction.OfPlayer || x.HostFaction == Faction.OfPlayer
				select x);
				if (list.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer)))
				{
					CaravanExitMapUtility.ExitMapAndCreateCaravan(list, Faction.OfPlayer, mapParent.Tile, mapParent.Tile, PlanetTile.Invalid, true);
				}
				list.Clear();
				mapParent.Destroy();
			}
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x0010EF9C File Offset: 0x0010D19C
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FillMapWithTrees()
		{
			Map currentMap = Find.CurrentMap;
			foreach (IntVec3 intVec in currentMap.AllCells)
			{
				if (intVec.Standable(currentMap))
				{
					GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Plant_TreeOak, null), intVec, currentMap, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x0010F008 File Offset: 0x0010D208
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true)]
		private static void LogMapPollution()
		{
			Log.Message("Polluted (of all possible pollutable cells): " + Find.CurrentMap.pollutionGrid.TotalPollutionPercent.ToStringPercent());
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x0010F030 File Offset: 0x0010D230
		[DebugAction("Map", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> RegenerateMapWithLandmark()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (LandmarkDef defLocal2 in DefDatabase<LandmarkDef>.AllDefsListForReading)
			{
				LandmarkDef defLocal = defLocal2;
				list.Add(new DebugActionNode(defLocal.defName, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
						PlanetTile tile = Find.CurrentMap.Tile;
						MapParent parent = Find.CurrentMap.Parent;
						IntVec3 size = Find.CurrentMap.Size;
						Current.Game.DeinitAndRemoveMap(Find.CurrentMap, true);
						Find.WorldGrid[tile].mutatorsNullable = new List<TileMutatorDef>();
						Find.World.landmarks.AddLandmark(defLocal, tile, null, true);
						Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, size, parent.def, null, false);
						Current.Game.CurrentMap = orGenerateMap;
						Find.World.renderer.wantedMode = WorldRenderMode.None;
						Find.CameraDriver.SetRootPosAndSize(rememberedCameraPos.rootPos, rememberedCameraPos.rootSize);
					}
				});
			}
			return list;
		}

		// Token: 0x06002CB3 RID: 11443 RVA: 0x0010F0BC File Offset: 0x0010D2BC
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GenerateLandmarkScreenshots()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (LandmarkDef landmarkDef2 in DefDatabase<LandmarkDef>.AllDefsListForReading)
			{
				LandmarkDef landmarkDef = landmarkDef2;
				list.Add(new DebugMenuOption(landmarkDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (int num in Enumerable.Range(0, 10))
					{
						float localCount = (float)(num * 5);
						if (num == 0)
						{
							localCount = 1f;
						}
						list2.Add(new DebugMenuOption(localCount.ToString() + " screenshots", DebugMenuOptionMode.Action, delegate()
						{
							List<DebugMenuOption> list3 = new List<DebugMenuOption>();
							int[] mapSizes = Dialog_AdvancedGameConfig.MapSizes;
							for (int i = 0; i < mapSizes.Length; i++)
							{
								int localSize2 = mapSizes[i];
								int localSize = localSize2;
								List<DebugMenuOption> list4 = list3;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
								defaultInterpolatedStringHandler.AppendFormatted<int>(localSize);
								defaultInterpolatedStringHandler.AppendLiteral(" x ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(localSize);
								list4.Add(new DebugMenuOption(defaultInterpolatedStringHandler.ToStringAndClear(), DebugMenuOptionMode.Action, delegate()
								{
									Find.UIRoot.screenshotMode.Active = true;
									Enumerable.Range(0, Find.WorldGrid.TilesCount - 1).ToList<int>().Shuffle<int>();
									WorldObject worldObject = null;
									StringBuilder log = new StringBuilder();
									log.AppendLine("Landmark info:");
									log.AppendLine("Seed: " + Find.World.info.Seed.ToString());
									int num2 = 0;
									foreach (SurfaceTile surfaceTile in Find.WorldGrid.Tiles.InRandomOrder(null))
									{
										PlanetTile tile = surfaceTile.tile;
										if (!Find.WorldObjects.AnyWorldObjectAt(tile) && landmarkDef.IsValidTile(tile, tile.Layer, false))
										{
											if ((float)num2 >= localCount)
											{
												break;
											}
											num2++;
											LongEventHandler.QueueLongEvent(delegate()
											{
												Find.World.landmarks.AddLandmark(landmarkDef, tile, null, false);
												Current.Game.DeinitAndRemoveMap(Find.CurrentMap, false);
												WorldObject worldObject = worldObject;
												if (worldObject != null)
												{
													worldObject.Destroy();
												}
												Find.World.renderer.wantedMode = WorldRenderMode.None;
												worldObject = (MapParent)WorldObjectMaker.MakeWorldObject(tile.LayerDef.SettlementWorldObjectDef);
												worldObject.Tile = tile;
												worldObject.SetFaction(Faction.OfPlayer);
												Find.WorldObjects.Add(worldObject);
												Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(worldObject.Tile, new IntVec3(localSize, 1, localSize), null, null, false);
												Current.Game.CurrentMap = orGenerateMap;
												orGenerateMap.fogGrid.ClearAllFog();
											}, null, false, null, true, true, null);
											int ssIndex = num2;
											LongEventHandler.QueueLongEvent(delegate()
											{
												Find.CameraDriver.config.sizeRange.max = 150f;
												Find.CameraDriver.SetRootPosAndSize(Find.CurrentMap.Center.ToVector3(), 150f);
												DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(2, 3);
												defaultInterpolatedStringHandler2.AppendFormatted(landmarkDef.defName);
												defaultInterpolatedStringHandler2.AppendLiteral("_");
												defaultInterpolatedStringHandler2.AppendFormatted(Find.CurrentMap.Biome.defName);
												defaultInterpolatedStringHandler2.AppendLiteral("_");
												defaultInterpolatedStringHandler2.AppendFormatted<int>(ssIndex);
												string text = defaultInterpolatedStringHandler2.ToStringAndClear();
												StringBuilder log;
												log.AppendLine(text);
												log = log;
												DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(24, 2);
												defaultInterpolatedStringHandler3.AppendLiteral("   tile ID: ");
												defaultInterpolatedStringHandler3.AppendFormatted<PlanetTile>(tile);
												defaultInterpolatedStringHandler3.AppendLiteral(", mutators: ");
												defaultInterpolatedStringHandler3.AppendFormatted((from x in Find.CurrentMap.TileInfo.Mutators
												select x.defName).ToCommaList(false, false));
												log.AppendLine(defaultInterpolatedStringHandler3.ToStringAndClear());
												ScreenshotTaker.TakeNonSteamShot(text);
											}, null, false, null, true, true, null);
										}
									}
									LongEventHandler.QueueLongEvent(delegate()
									{
										Log.Message(log.ToString());
										Find.UIRoot.screenshotMode.Active = false;
									}, null, false, null, true, false, null);
								}));
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3, null));
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x0010F150 File Offset: 0x0010D350
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddMutatorToCurrentMapTile()
		{
			Tile tile = Find.CurrentMap.TileInfo;
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<TileMutatorDef>.Enumerator enumerator = DefDatabase<TileMutatorDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TileMutatorDef mutatorDef = enumerator.Current;
					list.Add(new DebugMenuOption(mutatorDef.defName, DebugMenuOptionMode.Action, delegate()
					{
						tile.AddMutator(mutatorDef);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x0010F200 File Offset: 0x0010D400
		[DebugAction("Map", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemoveMutatorFromCurrentMapTile()
		{
			Tile tile = Find.CurrentMap.TileInfo;
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<TileMutatorDef> enumerator = tile.Mutators.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TileMutatorDef mutatorDef = enumerator.Current;
					list.Add(new DebugMenuOption(mutatorDef.defName, DebugMenuOptionMode.Action, delegate()
					{
						tile.RemoveMutator(mutatorDef);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x04001E26 RID: 7718
		private static Map mapLeak;
	}
}
