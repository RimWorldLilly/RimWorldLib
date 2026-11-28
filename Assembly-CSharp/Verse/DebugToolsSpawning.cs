using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020005C3 RID: 1475
	public static class DebugToolsSpawning
	{
		// Token: 0x06002DEF RID: 11759 RVA: 0x00119824 File Offset: 0x00117A24
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> SpawnPawn()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (PawnKindDef localKindDef2 in from x in DefDatabase<PawnKindDef>.AllDefs
			where x.showInDebugSpawner
			select x into kd
			orderby kd.defName
			select kd)
			{
				PawnKindDef localKindDef = localKindDef2;
				list.Add(new DebugActionNode(localKindDef.defName, DebugActionType.ToolMap, null, null)
				{
					category = DebugToolsSpawning.GetCategoryForPawnKind(localKindDef),
					action = delegate()
					{
						Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionDef);
						Pawn pawn = PawnGenerator.GeneratePawn(localKindDef, faction, new PlanetTile?(Find.CurrentMap.Tile));
						GenSpawn.Spawn(pawn, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
						DebugToolsSpawning.PostPawnSpawn(pawn);
					}
				});
			}
			return list;
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x00119904 File Offset: 0x00117B04
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> SpawnPawnWithLifestage()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (PawnKindDef localKindDef2 in from x in DefDatabase<PawnKindDef>.AllDefs
			where x.showInDebugSpawner
			select x into kd
			orderby kd.defName
			select kd)
			{
				PawnKindDef localKindDef = localKindDef2;
				DebugActionNode debugActionNode = new DebugActionNode(localKindDef.defName, DebugActionType.ToolMap, null, null)
				{
					category = DebugToolsSpawning.GetCategoryForPawnKind(localKindDef)
				};
				for (int i = 0; i < localKindDef.RaceProps.lifeStageAges.Count; i++)
				{
					LifeStageAge raceStage = localKindDef.RaceProps.lifeStageAges[i];
					debugActionNode.AddChild(new DebugActionNode(raceStage.def.defName, DebugActionType.ToolMap, null, null)
					{
						action = delegate()
						{
							Pawn pawn = PawnGenerator.GeneratePawn(localKindDef, FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionDef), null);
							pawn.ageTracker.AgeBiologicalTicks = (long)(raceStage.minAge * 3600000f);
							GenSpawn.Spawn(pawn, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
							DebugToolsSpawning.PostPawnSpawn(pawn);
						}
					});
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x00119A68 File Offset: 0x00117C68
		[DebugAction("Anomaly", "Generate CreepJoiner...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresAnomaly = true)]
		private static void GenerateCreeperJoiner()
		{
			DebugToolsSpawning.requires.Clear();
			DebugToolsSpawning.excluded.Clear();
			Slate slate = new Slate();
			Action <>9__4;
			Action <>9__3;
			Action <>9__2;
			Action <>9__1;
			DebugToolsSpawning.ShowOptions<CreepJoinerFormKindDef>(slate, "form", DefDatabase<CreepJoinerFormKindDef>.AllDefs, DebugToolsSpawning.requires, DebugToolsSpawning.excluded, delegate
			{
				Slate slate = slate;
				string key = "benefit";
				IEnumerable<CreepJoinerBenefitDef> allDefs = DefDatabase<CreepJoinerBenefitDef>.AllDefs;
				List<Def> list = DebugToolsSpawning.requires;
				List<Def> list2 = DebugToolsSpawning.excluded;
				Action selected;
				if ((selected = <>9__1) == null)
				{
					selected = (<>9__1 = delegate()
					{
						Slate slate2 = slate;
						string key2 = "downside";
						IEnumerable<CreepJoinerDownsideDef> allDefs2 = DefDatabase<CreepJoinerDownsideDef>.AllDefs;
						List<Def> list3 = DebugToolsSpawning.requires;
						List<Def> list4 = DebugToolsSpawning.excluded;
						Action selected2;
						if ((selected2 = <>9__2) == null)
						{
							selected2 = (<>9__2 = delegate()
							{
								Slate slate3 = slate;
								string key3 = "aggressive";
								IEnumerable<CreepJoinerAggressiveDef> allDefs3 = DefDatabase<CreepJoinerAggressiveDef>.AllDefs;
								List<Def> list5 = DebugToolsSpawning.requires;
								List<Def> list6 = DebugToolsSpawning.excluded;
								Action selected3;
								if ((selected3 = <>9__3) == null)
								{
									selected3 = (<>9__3 = delegate()
									{
										Slate slate4 = slate;
										string key4 = "rejection";
										IEnumerable<CreepJoinerRejectionDef> allDefs4 = DefDatabase<CreepJoinerRejectionDef>.AllDefs;
										List<Def> list7 = DebugToolsSpawning.requires;
										List<Def> list8 = DebugToolsSpawning.excluded;
										Action selected4;
										if ((selected4 = <>9__4) == null)
										{
											selected4 = (<>9__4 = delegate()
											{
												Quest quest = QuestGen.Generate(QuestScriptDefOf.CreepJoinerArrival, slate);
												Find.QuestManager.Add(quest);
												DebugToolsSpawning.requires.Clear();
												DebugToolsSpawning.excluded.Clear();
											});
										}
										DebugToolsSpawning.ShowOptions<CreepJoinerRejectionDef>(slate4, key4, allDefs4, list7, list8, selected4);
									});
								}
								DebugToolsSpawning.ShowOptions<CreepJoinerAggressiveDef>(slate3, key3, allDefs3, list5, list6, selected3);
							});
						}
						DebugToolsSpawning.ShowOptions<CreepJoinerDownsideDef>(slate2, key2, allDefs2, list3, list4, selected2);
					});
				}
				DebugToolsSpawning.ShowOptions<CreepJoinerBenefitDef>(slate, key, allDefs, list, list2, selected);
			});
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x00119AC8 File Offset: 0x00117CC8
		private static void ShowOptions<T>(Slate slate, string key, IEnumerable<T> defs, List<Def> requires, List<Def> excluded, Action selected) where T : Def, ICreepJoinerDef
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			float num = StorytellerUtility.DefaultThreatPointsNow(Find.CurrentMap);
			using (IEnumerator<T> enumerator = defs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T def = enumerator.Current;
					DebugMenuOption item = new DebugMenuOption(def.label, DebugMenuOptionMode.Action, delegate()
					{
						if (excluded.Contains(def))
						{
							return;
						}
						slate.Set<T>(key, def, false);
						excluded.AddRange(def.Excludes);
						requires.AddRange(def.Requires);
						Action selected2 = selected;
						if (selected2 == null)
						{
							return;
						}
						selected2();
					});
					if (excluded.Contains(def))
					{
						item.label += " [NO]";
					}
					else if (num < def.MinCombatPoints)
					{
						item.label += " [PNTS LOW]";
					}
					else if (requires.Contains(def))
					{
						item.label += " [REQ]";
					}
					list.Add(item);
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, "Choose " + key));
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x00119C40 File Offset: 0x00117E40
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000, requiresBiotech = true)]
		private static List<DebugActionNode> SpawnNewborn()
		{
			return DebugToolsSpawning.SpawnAtDevelopmentalStages(DevelopmentalStage.Newborn);
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x00119C48 File Offset: 0x00117E48
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000, requiresBiotech = true)]
		private static List<DebugActionNode> SpawnChild()
		{
			return DebugToolsSpawning.SpawnAtDevelopmentalStages(DevelopmentalStage.Child);
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x00119C50 File Offset: 0x00117E50
		private static List<DebugActionNode> SpawnAtDevelopmentalStages(DevelopmentalStage stages)
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (PawnKindDef localKindDef3 in from kd in DefDatabase<PawnKindDef>.AllDefs
			orderby kd.defName
			select kd)
			{
				PawnKindDef localKindDef = localKindDef3;
				list.Add(new DebugActionNode(localKindDef.defName, DebugActionType.ToolMap, null, null)
				{
					category = DebugToolsSpawning.GetCategoryForPawnKind(localKindDef),
					action = delegate()
					{
						Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionDef);
						PawnKindDef localKindDef2 = localKindDef;
						Faction faction2 = faction;
						PawnGenerationContext context = PawnGenerationContext.NonPlayer;
						DevelopmentalStage stages2 = stages;
						Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(localKindDef2, faction2, context, null, false, false, true, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, stages2, null, null, null, false, false, false, -1, 0, false));
						GenSpawn.Spawn(pawn, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
						DebugToolsSpawning.PostPawnSpawn(pawn);
					}
				});
			}
			return list;
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x00119D24 File Offset: 0x00117F24
		public static string GetCategoryForPawnKind(PawnKindDef kindDef)
		{
			if (!kindDef.overrideDebugActionCategory.NullOrEmpty())
			{
				return kindDef.overrideDebugActionCategory;
			}
			if (kindDef.RaceProps.Humanlike)
			{
				return "Humanlike";
			}
			if (kindDef.RaceProps.Insect)
			{
				return "Insect";
			}
			if (kindDef.RaceProps.IsMechanoid)
			{
				return "Mechanoid";
			}
			if (kindDef.RaceProps.Animal)
			{
				return "Animal";
			}
			return "Other";
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x00119D98 File Offset: 0x00117F98
		private static void PostPawnSpawn(Pawn pawn)
		{
			if (pawn.Spawned && pawn.Faction != null && pawn.Faction != Faction.OfPlayer)
			{
				Lord lord = null;
				if (pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction).Any((Pawn p) => p != pawn))
				{
					Pawn pawn2 = (Pawn)GenClosest.ClosestThing_Global(pawn.Position, pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction), 99999f, (Thing p) => p != pawn && ((Pawn)p).GetLord() != null, null, false);
					lord = ((pawn2 != null) ? pawn2.GetLord() : null);
				}
				if (lord == null || !lord.CanAddPawn(pawn))
				{
					lord = LordMaker.MakeNewLord(pawn.Faction, new LordJob_DefendPoint(pawn.Position, null, null, false, true), Find.CurrentMap, null);
				}
				if (lord != null && lord.LordJob.CanAutoAddPawns)
				{
					lord.AddPawn(pawn);
				}
			}
			pawn.Rotation = Rot4.South;
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x00119EE9 File Offset: 0x001180E9
		[DebugAction("Spawning", "Spawn thing", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> TryPlaceNearThing()
		{
			return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(1, false)
			orderby x.label
			select x).ToList<DebugActionNode>();
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x00119F1C File Offset: 0x0011811C
		[DebugAction("Spawning", "Spawn thing with style", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> TryPlaceNearThingWithStyle()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (ThingDef localDef2 in from d in DefDatabase<ThingDef>.AllDefs
			orderby d.defName
			select d)
			{
				ThingDef localDef = localDef2;
				if (!localDef.randomStyle.NullOrEmpty<ThingStyleChance>() || DefDatabase<StyleCategoryDef>.AllDefs.Any((StyleCategoryDef s) => s.GetStyleForThingDef(localDef, null) != null))
				{
					DebugActionNode debugActionNode = new DebugActionNode(localDef.LabelCap, DebugActionType.Action, null, null);
					debugActionNode.AddChild(new DebugActionNode("Standard", DebugActionType.ToolMap, delegate()
					{
						DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, false, null, true, null);
					}, null));
					IEnumerable<StyleCategoryDef> allDefs = DefDatabase<StyleCategoryDef>.AllDefs;
					Func<StyleCategoryDef, bool> predicate;
					Func<StyleCategoryDef, bool> <>9__4;
					if ((predicate = <>9__4) == null)
					{
						Predicate<ThingDefStyle> <>9__5;
						predicate = (<>9__4 = delegate(StyleCategoryDef x)
						{
							List<ThingDefStyle> thingDefStyles = x.thingDefStyles;
							Predicate<ThingDefStyle> predicate2;
							if ((predicate2 = <>9__5) == null)
							{
								predicate2 = (<>9__5 = ((ThingDefStyle y) => y.ThingDef == localDef));
							}
							return thingDefStyles.Any(predicate2);
						});
					}
					using (IEnumerator<StyleCategoryDef> enumerator2 = allDefs.Where(predicate).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							StyleCategoryDef cat = enumerator2.Current;
							StyleCategoryDef cat2 = cat;
							debugActionNode.AddChild(new DebugActionNode(cat2.defName, DebugActionType.ToolMap, delegate()
							{
								DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, false, cat.GetStyleForThingDef(localDef, null), true, null);
							}, null));
						}
					}
					if (localDef.randomStyle != null)
					{
						foreach (ThingStyleChance style2 in localDef.randomStyle)
						{
							ThingStyleChance style = style2;
							debugActionNode.AddChild(new DebugActionNode(style.StyleDef.defName, DebugActionType.ToolMap, delegate()
							{
								DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, false, style.StyleDef, true, null);
							}, null));
						}
					}
					list.Add(debugActionNode);
				}
			}
			if (list.Count == 0)
			{
				list.Add(new DebugActionNode("No styleable things", DebugActionType.Action, delegate()
				{
				}, null));
			}
			return list;
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x0011A19C File Offset: 0x0011839C
		[DebugAction("Spawning", "Spawn unminified thing", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> TryPlaceMinifiedThing()
		{
			return (from x in DebugThingPlaceHelper.TryPlaceOptionsUnminified()
			orderby x.label
			select x).ToList<DebugActionNode>();
		}

		// Token: 0x06002DFB RID: 11771 RVA: 0x0011A1CC File Offset: 0x001183CC
		[DebugAction("Spawning", "Spawn full thing stack", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> TryPlaceNearFullStack()
		{
			return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(-1, false)
			orderby x.label
			select x).ToList<DebugActionNode>();
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x0011A1FE File Offset: 0x001183FE
		[DebugAction("Spawning", "Spawn stack of 25", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static List<DebugActionNode> TryPlaceNearStacksOf25()
		{
			return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(25, false)
			orderby x.label
			select x).ToList<DebugActionNode>();
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x0011A231 File Offset: 0x00118431
		[DebugAction("Spawning", "Spawn stack of 75", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static List<DebugActionNode> TryPlaceNearStacksOf75()
		{
			return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(75, false)
			orderby x.label
			select x).ToList<DebugActionNode>();
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x0011A264 File Offset: 0x00118464
		[DebugAction("Spawning", "Try place direct thing", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static List<DebugActionNode> TryPlaceDirectThing()
		{
			return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(1, true)
			orderby x.label
			select x).ToList<DebugActionNode>();
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x0011A296 File Offset: 0x00118496
		[DebugAction("Spawning", "Try place direct full stack", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static List<DebugActionNode> TryPlaceDirectFullStack()
		{
			return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(-1, true)
			orderby x.label
			select x).ToList<DebugActionNode>();
		}

		// Token: 0x06002E00 RID: 11776 RVA: 0x0011A2C8 File Offset: 0x001184C8
		[DebugAction("Spawning", "Try place direct stack of 25", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static List<DebugActionNode> TryPlaceDirectStackOf25()
		{
			return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(25, true)
			orderby x.label
			select x).ToList<DebugActionNode>();
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x0011A2FC File Offset: 0x001184FC
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SpawnWeapon()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
			where def.equipmentType == EquipmentType.Primary
			select def into d
			orderby d.defName
			select d)
			{
				ThingDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate()
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, false, null, true, null);
				}, null));
			}
			return list;
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x0011A3C4 File Offset: 0x001185C4
		[DebugAction("Spawning", "Spawn apparel", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SpawnApparel()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
			where def.IsApparel
			select def into d
			orderby d.defName
			select d)
			{
				ThingDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate()
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, false, null, true, null);
				}, null));
			}
			return list;
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x0011A48C File Offset: 0x0011868C
		[DebugAction("Spawning", "Try spawn stack of market value...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> TryPlaceNearMarketValue()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (float num in DebugToolsSpawning.MarketValues)
			{
				DebugActionNode debugActionNode = new DebugActionNode(num.ToStringMoney(null), DebugActionType.Action, null, null);
				foreach (DebugActionNode child in DebugThingPlaceHelper.TryPlaceOptionsForBaseMarketValue(num, false))
				{
					debugActionNode.AddChild(child);
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x0011A518 File Offset: 0x00118718
		[DebugAction("Spawning", "Spawn meal with specifics...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CreateMealWithSpecifics()
		{
			DebugToolsSpawning.<>c__DisplayClass24_0 CS$<>8__locals1 = new DebugToolsSpawning.<>c__DisplayClass24_0();
			IEnumerable<ThingDef> enumerable = from x in DefDatabase<ThingDef>.AllDefs
			where x.IsNutritionGivingIngestible && x.ingestible.IsMeal
			select x;
			CS$<>8__locals1.ingredientDefs = from x in DefDatabase<ThingDef>.AllDefs
			where x.IsNutritionGivingIngestible && x.ingestible.HumanEdible && !x.ingestible.IsMeal && !x.IsCorpse
			select x;
			CS$<>8__locals1.mealDef = null;
			CS$<>8__locals1.ingredients = new List<ThingDef>();
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<ThingDef> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef d = enumerator.Current;
					list.Add(new DebugMenuOption(d.defName, DebugMenuOptionMode.Action, delegate()
					{
						CS$<>8__locals1.mealDef = d;
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(CS$<>8__locals1.<CreateMealWithSpecifics>g__GetIngredientOptions|2(), null));
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x0011A614 File Offset: 0x00118814
		[DebugAction("Spawning", "Spawn thing with wipe mode", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static DebugActionNode SpawnThingWithWipeMode()
		{
			DebugActionNode debugActionNode = new DebugActionNode();
			WipeMode[] array = (WipeMode[])Enum.GetValues(typeof(WipeMode));
			for (int i = 0; i < array.Length; i++)
			{
				WipeMode localWipeMode2 = array[i];
				WipeMode localWipeMode = localWipeMode2;
				debugActionNode.AddChild(new DebugActionNode(localWipeMode2.ToString(), DebugActionType.Action, null, null)
				{
					childGetter = (() => DebugThingPlaceHelper.SpawnOptions(localWipeMode))
				});
			}
			return debugActionNode;
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x0011A691 File Offset: 0x00118891
		private static IEnumerable<float> PointsMechCluster()
		{
			return new DebugToolsSpawning.<PointsMechCluster>d__26(-2);
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x0011A69C File Offset: 0x0011889C
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresRoyalty = true)]
		private static DebugActionNode SpawnMechCluster()
		{
			DebugActionNode debugActionNode = new DebugActionNode();
			foreach (float localPoints2 in DebugToolsSpawning.PointsMechCluster())
			{
				float localPoints = localPoints2;
				DebugActionNode debugActionNode2 = new DebugActionNode(localPoints.ToString() + " points", DebugActionType.Action, null, null);
				debugActionNode2.AddChild(new DebugActionNode("In pods, click place", DebugActionType.ToolMap, delegate()
				{
					MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(localPoints, Find.CurrentMap, true, false);
					MechClusterUtility.SpawnCluster(UI.MouseCell(), Find.CurrentMap, sketch, true, false, null);
				}, null));
				debugActionNode2.AddChild(new DebugActionNode("In pods, autoplace", DebugActionType.ToolMap, delegate()
				{
					MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(localPoints, Find.CurrentMap, true, false);
					MechClusterUtility.SpawnCluster(MechClusterUtility.FindClusterPosition(Find.CurrentMap, sketch, 100, 0f), Find.CurrentMap, sketch, true, false, null);
				}, null));
				debugActionNode2.AddChild(new DebugActionNode("Direct spawn, click place", DebugActionType.ToolMap, delegate()
				{
					MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(localPoints, Find.CurrentMap, true, false);
					MechClusterUtility.SpawnCluster(UI.MouseCell(), Find.CurrentMap, sketch, false, false, null);
				}, null));
				debugActionNode2.AddChild(new DebugActionNode("Direct spawn, autoplace", DebugActionType.Action, delegate()
				{
					MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(localPoints, Find.CurrentMap, true, false);
					MechClusterUtility.SpawnCluster(MechClusterUtility.FindClusterPosition(Find.CurrentMap, sketch, 100, 0f), Find.CurrentMap, sketch, false, false, null);
				}, null));
				debugActionNode.AddChild(debugActionNode2);
			}
			debugActionNode.visibilityGetter = (() => Faction.OfMechanoids != null);
			return debugActionNode;
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x0011A7C4 File Offset: 0x001189C4
		[DebugAction("Spawning", "Make filth x100", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void MakeFilthx100()
		{
			for (int i = 0; i < 100; i++)
			{
				IntVec3 c = UI.MouseCell() + GenRadial.RadialPattern[i];
				if (c.InBounds(Find.CurrentMap) && c.WalkableByAny(Find.CurrentMap))
				{
					FilthMaker.TryMakeFilth(c, Find.CurrentMap, ThingDefOf.Filth_Dirt, 2, FilthSourceFlags.None, true);
					FleckMaker.ThrowMetaPuff(c.ToVector3Shifted(), Find.CurrentMap);
				}
			}
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x0011A834 File Offset: 0x00118A34
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnFactionLeader()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (Faction localFac2 in Find.FactionManager.AllFactions)
			{
				Faction localFac = localFac2;
				if (localFac.leader != null)
				{
					list.Add(new FloatMenuOption(localFac.Name + " - " + localFac.leader.Name.ToStringFull, delegate()
					{
						GenSpawn.Spawn(localFac.leader, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x0011A8FC File Offset: 0x00118AFC
		[DebugAction("Spawning", "Spawn world pawn...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnWorldPawn()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			Action<Pawn> act = delegate(Pawn p)
			{
				List<DebugMenuOption> list2 = new List<DebugMenuOption>();
				IEnumerable<PawnKindDef> allDefs = DefDatabase<PawnKindDef>.AllDefs;
				Func<PawnKindDef, bool> <>9__1;
				Func<PawnKindDef, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((PawnKindDef x) => x.race == p.def));
				}
				foreach (PawnKindDef kLocal2 in allDefs.Where(predicate))
				{
					PawnKindDef kLocal = kLocal2;
					list2.Add(new DebugMenuOption(kLocal.defName, DebugMenuOptionMode.Tool, delegate()
					{
						PawnGenerationRequest request = new PawnGenerationRequest(kLocal, p.Faction, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
						PawnGenerator.RedressPawn(p, request);
						GenSpawn.Spawn(p, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
						DebugTools.curTool = null;
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
			};
			foreach (Pawn pawn in Find.WorldPawns.AllPawnsAlive)
			{
				Pawn pLocal = pawn;
				list.Add(new DebugMenuOption(pawn.LabelShort, DebugMenuOptionMode.Action, delegate()
				{
					act(pLocal);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x0011A9C4 File Offset: 0x00118BC4
		[DebugAction("Spawning", "Spawn thing set", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SpawnThingSet()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (ThingSetMakerDef localGen2 in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef localGen = localGen2;
				list.Add(new DebugActionNode(localGen.defName, DebugActionType.ToolMap, delegate()
				{
					if (!UI.MouseCell().InBounds(Find.CurrentMap))
					{
						return;
					}
					StringBuilder stringBuilder = new StringBuilder();
					string nonNullFieldsDebugInfo = Gen.GetNonNullFieldsDebugInfo(localGen.debugParams);
					List<Thing> list2 = localGen.root.Generate(localGen.debugParams);
					stringBuilder.Append(localGen.defName + " generated " + list2.Count.ToString() + " things");
					if (!nonNullFieldsDebugInfo.NullOrEmpty())
					{
						stringBuilder.Append(" (used custom debug params: " + nonNullFieldsDebugInfo + ")");
					}
					stringBuilder.AppendLine(":");
					float num = 0f;
					float num2 = 0f;
					for (int i = 0; i < list2.Count; i++)
					{
						stringBuilder.AppendLine("   - " + list2[i].LabelCap);
						num += list2[i].MarketValue * (float)list2[i].stackCount;
						if (!(list2[i] is Pawn))
						{
							num2 += list2[i].GetStatValue(StatDefOf.Mass, true, -1) * (float)list2[i].stackCount;
						}
						if (!GenPlace.TryPlaceThing(list2[i], UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, null, 1))
						{
							list2[i].Destroy(DestroyMode.Vanish);
						}
					}
					stringBuilder.AppendLine("Total market value: " + num.ToString("0.##"));
					stringBuilder.AppendLine("Total mass: " + num2.ToStringMass());
					Log.Message(stringBuilder.ToString());
				}, null));
			}
			return list;
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x0011AA44 File Offset: 0x00118C44
		[DebugAction("Spawning", "Trigger effecter...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> TriggerEffecter()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (EffecterDef localDef2 in DefDatabase<EffecterDef>.AllDefs)
			{
				EffecterDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate()
				{
					Effecter effecter = localDef.Spawn();
					effecter.Trigger(new TargetInfo(UI.MouseCell(), Find.CurrentMap, false), new TargetInfo(UI.MouseCell(), Find.CurrentMap, false), -1);
					effecter.Cleanup();
				}, null));
			}
			return list;
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x0011AAC4 File Offset: 0x00118CC4
		[DebugAction("Spawning", "Trigger Maintained Effecter...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> TriggerMaintainedEffecter()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (EffecterDef localDef2 in DefDatabase<EffecterDef>.AllDefs)
			{
				EffecterDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate()
				{
					TargetInfo targetInfo = new TargetInfo(UI.MouseCell(), Find.CurrentMap, false);
					TargetInfo b = targetInfo;
					localDef.SpawnMaintained(targetInfo, b, 1f);
				}, null));
			}
			return list;
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x0011AB44 File Offset: 0x00118D44
		[DebugAction("Spawning", "Trigger Maintained Effecter (5s)...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> TriggerMaintainedEffecter5S()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (EffecterDef localDef2 in DefDatabase<EffecterDef>.AllDefs)
			{
				EffecterDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate()
				{
					Effecter effecter = localDef.Spawn();
					IntVec3 cell = UI.MouseCell();
					TargetInfo targetInfo = new TargetInfo(cell, Find.CurrentMap, false);
					TargetInfo b = targetInfo;
					effecter.Trigger(targetInfo, b, -1);
					Find.CurrentMap.effecterMaintainer.AddEffecterToMaintain(effecter, targetInfo, b, 300);
				}, null));
			}
			return list;
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x0011ABC4 File Offset: 0x00118DC4
		[DebugAction("Spawning", "Trigger Maintained Effecter (12s)...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> TriggerMaintainedEffecter12S()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (EffecterDef localDef2 in DefDatabase<EffecterDef>.AllDefs)
			{
				EffecterDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate()
				{
					Effecter effecter = localDef.Spawn();
					IntVec3 cell = UI.MouseCell();
					TargetInfo targetInfo = new TargetInfo(cell, Find.CurrentMap, false);
					TargetInfo b = targetInfo;
					effecter.Trigger(targetInfo, b, -1);
					Find.CurrentMap.effecterMaintainer.AddEffecterToMaintain(effecter, targetInfo, b, 720);
				}, null));
			}
			return list;
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x0011AC44 File Offset: 0x00118E44
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static DebugActionNode SpawnShuttle()
		{
			DebugActionNode debugActionNode = new DebugActionNode();
			debugActionNode.AddChild(new DebugActionNode("Incoming", DebugActionType.ToolMap, delegate()
			{
				GenPlace.TryPlaceThing(SkyfallerMaker.MakeSkyfaller(ThingDefOf.ShuttleIncoming, ThingMaker.MakeThing(ThingDefOf.Shuttle, null)), UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, null, 1);
			}, null));
			debugActionNode.AddChild(new DebugActionNode("Crashing", DebugActionType.ToolMap, delegate()
			{
				GenPlace.TryPlaceThing(SkyfallerMaker.MakeSkyfaller(ThingDefOf.ShuttleCrashing, ThingMaker.MakeThing(ThingDefOf.ShuttleCrashed, null)), UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, null, 1);
			}, null));
			debugActionNode.AddChild(new DebugActionNode("Stationary", DebugActionType.ToolMap, delegate()
			{
				GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.Shuttle, null), UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, null, 1);
			}, null));
			return debugActionNode;
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x0011ACEC File Offset: 0x00118EEC
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SpawnAnimalGroup()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (PawnKindDef localKindDef2 in from x in DefDatabase<PawnKindDef>.AllDefs
			where x.RaceProps.Animal
			select x into kd
			orderby kd.defName
			select kd)
			{
				PawnKindDef localKindDef = localKindDef2;
				list.Add(new DebugActionNode(localKindDef.defName, DebugActionType.Action, null, null)
				{
					category = DebugToolsSpawning.GetCategoryForPawnKind(localKindDef),
					action = delegate()
					{
						Find.CurrentMap.wildAnimalSpawner.SpawnRandomWildAnimalAt(RCellFinder.RandomAnimalSpawnCell_MapGen(Find.CurrentMap), true, localKindDef);
					}
				});
			}
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("[random suitable]", DebugActionType.Action, null, null);
			debugActionNode.action = delegate()
			{
				Find.CurrentMap.wildAnimalSpawner.SpawnRandomWildAnimalAt(RCellFinder.RandomAnimalSpawnCell_MapGen(Find.CurrentMap), true, null);
			};
			list2.Add(debugActionNode);
			return list;
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x0011AE04 File Offset: 0x00119004
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void SpawnRandomCaravan()
		{
			PlanetTile planetTile = GenWorld.MouseTile(false);
			if (Find.WorldGrid[planetTile].PrimaryBiome.impassable)
			{
				return;
			}
			List<Pawn> list = new List<Pawn>();
			int num = Rand.RangeInclusive(1, 10);
			for (int i = 0; i < num; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer, null);
				list.Add(pawn);
				if (!pawn.WorkTagIsDisabled(WorkTags.Violent))
				{
					ThingDef thingDef = (from def in DefDatabase<ThingDef>.AllDefs
					where def.IsWeapon && !def.weaponTags.NullOrEmpty<string>() && (def.weaponTags.Contains("SimpleGun") || def.weaponTags.Contains("IndustrialGunAdvanced") || def.weaponTags.Contains("SpacerGun") || def.weaponTags.Contains("MedievalMeleeAdvanced") || def.weaponTags.Contains("NeolithicRangedBasic") || def.weaponTags.Contains("NeolithicRangedDecent") || def.weaponTags.Contains("NeolithicRangedHeavy"))
					select def).RandomElementWithFallback(null);
					pawn.equipment.AddEquipment((ThingWithComps)ThingMaker.MakeThing(thingDef, GenStuff.RandomStuffFor(thingDef)));
				}
			}
			int num2 = Rand.RangeInclusive(-4, 10);
			for (int j = 0; j < num2; j++)
			{
				Pawn item = PawnGenerator.GeneratePawn((from d in DefDatabase<PawnKindDef>.AllDefs
				where d.RaceProps.Animal && d.race.GetStatValueAbstract(StatDefOf.Wildness, null) < 1f
				select d).RandomElement<PawnKindDef>(), Faction.OfPlayer, null);
				list.Add(item);
			}
			Caravan caravan = CaravanMaker.MakeCaravan(list, Faction.OfPlayer, planetTile, true);
			List<Thing> list2 = ThingSetMakerDefOf.DebugCaravanInventory.root.Generate();
			for (int k = 0; k < list2.Count; k++)
			{
				Thing thing = list2[k];
				if (thing.GetStatValue(StatDefOf.Mass, true, -1) * (float)thing.stackCount > caravan.MassCapacity - caravan.MassUsage)
				{
					break;
				}
				CaravanInventoryUtility.GiveThing(caravan, thing);
			}
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x0011AFB4 File Offset: 0x001191B4
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void SpawnRandomFactionBase()
		{
			Faction faction;
			if ((from x in Find.FactionManager.AllFactions
			where !x.IsPlayer && !x.Hidden
			select x).TryRandomElement(out faction))
			{
				PlanetTile tile = GenWorld.MouseTile(false);
				if (Find.WorldGrid[tile].PrimaryBiome.impassable)
				{
					return;
				}
				Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(tile.LayerDef.SettlementWorldObjectDef);
				settlement.SetFaction(faction);
				settlement.Tile = tile;
				settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, null);
				Find.WorldObjects.Add(settlement);
			}
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x0011B054 File Offset: 0x00119254
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void SpawnSite()
		{
			PlanetTile tile = GenWorld.MouseTile(false);
			if (!tile.Valid || Find.World.Impassable(tile))
			{
				Messages.Message("Impassable", MessageTypeDefOf.RejectInput, false);
				return;
			}
			List<SitePartDef> parts = new List<SitePartDef>();
			Action addPart = null;
			Action <>9__1;
			addPart = delegate()
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				List<DebugMenuOption> list2 = list;
				string label = "-Done (" + parts.Count.ToString() + " parts)-";
				DebugMenuOptionMode mode = DebugMenuOptionMode.Action;
				Action method;
				if ((method = <>9__1) == null)
				{
					method = (<>9__1 = delegate()
					{
						Site site = SiteMaker.TryMakeSite(parts, tile, true, null, true, null);
						if (site == null)
						{
							Messages.Message("Could not find any valid faction for this site.", MessageTypeDefOf.RejectInput, false);
							return;
						}
						Find.WorldObjects.Add(site);
					});
				}
				list2.Add(new DebugMenuOption(label, mode, method));
				foreach (SitePartDef sitePartDef in DefDatabase<SitePartDef>.AllDefs)
				{
					SitePartDef localPart = sitePartDef;
					list.Add(new DebugMenuOption(sitePartDef.defName, DebugMenuOptionMode.Action, delegate()
					{
						parts.Add(localPart);
						addPart();
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
			};
			addPart();
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x0011B0D4 File Offset: 0x001192D4
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void DestroySite()
		{
			PlanetTile tileID = GenWorld.MouseTile(false);
			foreach (WorldObject worldObject in Find.WorldObjects.ObjectsAt(tileID).ToList<WorldObject>())
			{
				worldObject.Destroy();
			}
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x0011B138 File Offset: 0x00119338
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void SpawnSiteWithPoints()
		{
			PlanetTile tile = GenWorld.MouseTile(false);
			if (!tile.Valid || Find.World.Impassable(tile))
			{
				Messages.Message("Impassable", MessageTypeDefOf.RejectInput, false);
				return;
			}
			List<SitePartDef> parts = new List<SitePartDef>();
			Action addPart = null;
			Action <>9__1;
			addPart = delegate()
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				List<DebugMenuOption> list2 = list;
				string label = "-Done (" + parts.Count.ToString() + " parts)-";
				DebugMenuOptionMode mode = DebugMenuOptionMode.Action;
				Action method;
				if ((method = <>9__1) == null)
				{
					method = (<>9__1 = delegate()
					{
						List<DebugMenuOption> list3 = new List<DebugMenuOption>();
						foreach (float localPoints2 in DebugActionsUtility.PointsOptions(true))
						{
							float localPoints = localPoints2;
							list3.Add(new DebugMenuOption(localPoints2.ToString("F0"), DebugMenuOptionMode.Action, delegate()
							{
								Site site = SiteMaker.TryMakeSite(parts, tile, true, null, true, new float?(localPoints));
								if (site == null)
								{
									Messages.Message("Could not find any valid faction for this site.", MessageTypeDefOf.RejectInput, false);
									return;
								}
								Find.WorldObjects.Add(site);
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3, null));
					});
				}
				list2.Add(new DebugMenuOption(label, mode, method));
				foreach (SitePartDef sitePartDef in DefDatabase<SitePartDef>.AllDefs)
				{
					SitePartDef localPart = sitePartDef;
					list.Add(new DebugMenuOption(sitePartDef.defName, DebugMenuOptionMode.Action, delegate()
					{
						parts.Add(localPart);
						addPart();
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
			};
			addPart();
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x0011B1B8 File Offset: 0x001193B8
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static List<DebugActionNode> SpawnWorldObject()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (WorldObjectDef localDef2 in DefDatabase<WorldObjectDef>.AllDefs)
			{
				WorldObjectDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolWorld, delegate()
				{
					PlanetTile planetTile = GenWorld.MouseTile(false);
					if (!planetTile.Valid || Find.World.Impassable(planetTile))
					{
						Messages.Message("Impassable", MessageTypeDefOf.RejectInput, false);
						return;
					}
					WorldObject worldObject = WorldObjectMaker.MakeWorldObject(localDef);
					worldObject.Tile = planetTile;
					Find.WorldObjects.Add(worldObject);
				}, null));
			}
			return list;
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x0011B238 File Offset: 0x00119438
		[DebugAction("General", "Change camera config", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static List<DebugActionNode> ChangeCameraConfigWorld()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (Type localType2 in typeof(WorldCameraConfig).AllSubclasses())
			{
				Type localType = localType2;
				string text = localType.Name;
				if (text.StartsWith("WorldCameraConfig_"))
				{
					text = text.Substring("WorldCameraConfig_".Length);
				}
				list.Add(new DebugActionNode(text, DebugActionType.Action, delegate()
				{
					Find.WorldCameraDriver.config = (WorldCameraConfig)Activator.CreateInstance(localType);
				}, null));
			}
			return list;
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x0011B2EC File Offset: 0x001194EC
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true)]
		private static List<DebugActionNode> SpawnBossgroup()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			using (IEnumerator<BossgroupDef> enumerator = DefDatabase<BossgroupDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BossgroupDef def = enumerator.Current;
					BossgroupDef localDef = def;
					string text = localDef.defName;
					Pawn caller;
					if (!PawnsFinder.AllMaps_FreeColonists.TryRandomElement(out caller) || !localDef.Worker.CanResolve(caller))
					{
						text += " [NO]";
					}
					list.Add(new DebugActionNode(text, DebugActionType.Action, null, null)
					{
						childGetter = delegate()
						{
							List<DebugActionNode> list2 = new List<DebugActionNode>();
							int currentWave = 0;
							GameComponent_Bossgroup component = Current.Game.GetComponent<GameComponent_Bossgroup>();
							if (component != null)
							{
								currentWave = component.NumTimesCalledBossgroup(def);
							}
							list2.Add(new DebugActionNode("*Current (times called: " + currentWave.ToString() + ")", DebugActionType.Action, delegate()
							{
								if (caller == null)
								{
									Messages.Message("No colonist found to call bossgroup.", MessageTypeDefOf.RejectInput, false);
									return;
								}
								localDef.Worker.Resolve(caller.Map, currentWave);
							}, null));
							for (int i = 0; i < def.waves.Count; i++)
							{
								int index = i;
								list2.Add(new DebugActionNode("Wave " + index.ToString(), DebugActionType.Action, delegate()
								{
									if (caller == null)
									{
										Messages.Message("No colonist found to call bossgroup.", MessageTypeDefOf.RejectInput, false);
										return;
									}
									localDef.Worker.Resolve(caller.Map, index);
								}, null));
							}
							return list2;
						}
					});
				}
			}
			return list;
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x0011B3C4 File Offset: 0x001195C4
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		public static List<DebugActionNode> AbandonThing()
		{
			return (from t in DebugThingPlaceHelper.TryAbandonOptionsForStackCount()
			orderby t.label
			select t).ToList<DebugActionNode>();
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x0011B3F4 File Offset: 0x001195F4
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestDeferredSpawner()
		{
			IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, Find.CurrentMap);
			incidentParms.forced = true;
			incidentParms.points = 200f;
			incidentParms.faction = Faction.OfHoraxCult;
			List<Pawn> list = new List<Pawn>(PawnGroupMakerUtility.GeneratePawns(IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, incidentParms, false), true));
			List<IntVec3> list2 = new List<IntVec3>();
			for (int i = 0; i < list.Count; i++)
			{
				list2.Add(CellFinder.RandomSpawnCellForPawnNear(Find.CurrentMap.Center, Find.CurrentMap, 4));
			}
			SpawnRequest spawnRequest = new SpawnRequest(list.Cast<Thing>().ToList<Thing>(), list2, 1, 2f, null);
			spawnRequest.initialDelay = 500;
			spawnRequest.preSpawnEffecterOffsetTicks = -120;
			spawnRequest.preSpawnEffect = EffecterDefOf.WaterMist;
			Find.CurrentMap.deferredSpawner.AddRequest(spawnRequest, true);
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x0011B4C4 File Offset: 0x001196C4
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresAnomaly = true)]
		private static void CreateColonistDuplicate()
		{
			Map currentMap = Find.CurrentMap;
			GenSpawn.Spawn(Find.PawnDuplicator.Duplicate(currentMap.mapPawns.FreeColonistsSpawned[0]), CellFinder.RandomSpawnCellForPawnNear(currentMap.Center, currentMap, 4), currentMap, WipeMode.Vanish);
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x0011B508 File Offset: 0x00119708
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresAnomaly = true)]
		private static void Duplicate()
		{
			Map currentMap = Find.CurrentMap;
			foreach (Thing thing in currentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.RaceProps.Humanlike)
				{
					GenSpawn.Spawn(Find.PawnDuplicator.Duplicate(pawn), CellFinder.RandomSpawnCellForPawnNear(pawn.PositionHeld, currentMap, 4), currentMap, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x0011B5A0 File Offset: 0x001197A0
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> WaterEmergePawn()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (PawnKindDef localKindDef2 in from x in DefDatabase<PawnKindDef>.AllDefs
			where x.showInDebugSpawner
			select x into kd
			orderby kd.defName
			select kd)
			{
				PawnKindDef localKindDef = localKindDef2;
				DebugActionNode debugActionNode = new DebugActionNode(localKindDef.defName, DebugActionType.ToolMap, null, null)
				{
					category = DebugToolsSpawning.GetCategoryForPawnKind(localKindDef)
				};
				for (int i = 1; i <= 3; i++)
				{
					int delaySeconds = i;
					DebugActionNode debugActionNode2 = debugActionNode;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 1);
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral("s delay");
					debugActionNode2.AddChild(new DebugActionNode(defaultInterpolatedStringHandler.ToStringAndClear(), DebugActionType.ToolMap, null, null)
					{
						action = delegate()
						{
							Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionDef);
							Pawn pawn = PawnGenerator.GeneratePawn(localKindDef, faction, null);
							int delayTicks = delaySeconds * 60;
							DelayedEffecterSpawner.Spawn(pawn, UI.MouseCell(), Find.CurrentMap, delayTicks, EffecterDefOf.PawnEmergeFromWaterLarge, EffecterDefOf.WaterMist, SoundDefOf.EmergeFromWater);
						}
					});
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x0011B6E4 File Offset: 0x001198E4
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000, requiresAnomaly = true)]
		private static List<DebugActionNode> SpawnFlare()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			int[] array = new int[]
			{
				1,
				2,
				3,
				4,
				5,
				15,
				30
			};
			for (int i = 0; i < array.Length; i++)
			{
				int delaySeconds = array[i];
				List<DebugActionNode> list2 = list;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 1);
				defaultInterpolatedStringHandler.AppendFormatted<int>(delaySeconds);
				defaultInterpolatedStringHandler.AppendLiteral("s duration");
				list2.Add(new DebugActionNode(defaultInterpolatedStringHandler.ToStringAndClear(), DebugActionType.ToolMap, null, null)
				{
					action = delegate()
					{
						int num = delaySeconds * 60;
						ThingWithComps thingWithComps = (ThingWithComps)ThingMaker.MakeThing(ThingDefOf.DisruptorFlare, null);
						GenSpawn.Spawn(thingWithComps, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
						CompDestroyAfterDelay comp = thingWithComps.GetComp<CompDestroyAfterDelay>();
						comp.spawnTick = GenTicks.TicksGame + num - comp.Props.delayTicks;
						EffecterDefOf.DisruptorFlareLanded.Spawn(UI.MouseCell(), Find.CurrentMap, 1f).Cleanup();
					}
				});
			}
			return list;
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x0011B774 File Offset: 0x00119974
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000, actionType = DebugActionType.ToolMapForPawns, requiresOdyssey = true)]
		private static void SpawnStatueCraftedBy(Pawn sculptor)
		{
			List<Pawn> allPossiblePawnsToSelectFrom = CompStatue.GetAllPossiblePawnsToSelectFrom(sculptor);
			if (allPossiblePawnsToSelectFrom.Count == 0)
			{
				Messages.Message("No suitable statue subjects found", MessageTypeDefOf.RejectInput, false);
				return;
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(from subject in allPossiblePawnsToSelectFrom
			select new DebugMenuOption(subject.Label, DebugMenuOptionMode.Tool, delegate()
			{
				IntVec3 center = UI.MouseCell();
				GenPlace.TryPlaceThing(CompStatue.GenerateSpecificStatue(sculptor, subject), center, Find.CurrentMap, ThingPlaceMode.Near, null, null, null, 1);
			}), null));
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x0011B7D8 File Offset: 0x001199D8
		[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnFilth()
		{
			List<ThingDef> source = (from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Filth
			select d).ToList<ThingDef>();
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(from filthDef in source
			select new DebugMenuOption(filthDef.defName, DebugMenuOptionMode.Tool, delegate()
			{
				FilthMaker.TryMakeFilth(UI.MouseCell(), Find.CurrentMap, filthDef, 1, FilthSourceFlags.None, true);
			}), null));
		}

		// Token: 0x04001E37 RID: 7735
		private static readonly List<Def> excluded = new List<Def>();

		// Token: 0x04001E38 RID: 7736
		private static readonly List<Def> requires = new List<Def>();

		// Token: 0x04001E39 RID: 7737
		private static readonly float[] MarketValues = new float[]
		{
			1000f,
			10000f,
			100000f
		};
	}
}
