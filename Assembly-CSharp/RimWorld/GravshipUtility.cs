using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Unity.Profiling;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F24 RID: 7972
	public static class GravshipUtility
	{
		// Token: 0x17002526 RID: 9510
		// (get) Token: 0x0600CB6B RID: 52075 RVA: 0x004AE2E0 File Offset: 0x004AC4E0
		public static bool ShowConnectedSubstructure
		{
			get
			{
				if (!ModsConfig.OdysseyActive)
				{
					return false;
				}
				Designator selectedDesignator = Find.DesignatorManager.SelectedDesignator;
				if (selectedDesignator is Designator_Install)
				{
					return false;
				}
				Designator_Place designator_Place = selectedDesignator as Designator_Place;
				if (designator_Place != null)
				{
					TerrainDef terrainDef = designator_Place.PlacingDef as TerrainDef;
					if (terrainDef != null && terrainDef.IsSubstructure)
					{
						return true;
					}
					ThingDef thingDef = designator_Place.PlacingDef as ThingDef;
					if (thingDef != null && thingDef.GetCompProperties<CompProperties_SubstructureFootprint>() != null)
					{
						return true;
					}
				}
				foreach (object obj in Find.Selector.SelectedObjects)
				{
					Thing thing = obj as Thing;
					CompSubstructureFootprint compSubstructureFootprint;
					if (thing != null && thing.TryGetComp(out compSubstructureFootprint) && compSubstructureFootprint.Valid && compSubstructureFootprint.DisplaySubstructureOverlay)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600CB6C RID: 52076 RVA: 0x004AE3BC File Offset: 0x004AC5BC
		public static bool PlayerHasGravEngine()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			if (Find.CurrentGravship != null)
			{
				return true;
			}
			bool result;
			using (GravshipUtility.PlayerHasGravEngineMarker.Auto())
			{
				using (List<Map>.Enumerator enumerator = Find.Maps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (GravshipUtility.PlayerHasGravEngine(enumerator.Current))
						{
							return true;
						}
					}
				}
				foreach (WorldObject worldObject in Find.WorldObjects.AllWorldObjects)
				{
					if (worldObject.Faction != null && worldObject.Faction.IsPlayer && !(worldObject is MapParent))
					{
						IThingHolder thingHolder = worldObject as IThingHolder;
						if (thingHolder != null)
						{
							using (List<Thing>.Enumerator enumerator3 = ThingOwnerUtility.GetAllThingsRecursively(thingHolder, true).GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									if (enumerator3.Current.GetInnerIfMinified().def == ThingDefOf.GravEngine)
									{
										return true;
									}
								}
							}
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600CB6D RID: 52077 RVA: 0x004AE518 File Offset: 0x004AC718
		[Obsolete]
		public static Thing GetPlayerGravEngine(Map map)
		{
			return GravshipUtility.GetPlayerGravEngine_NewTemp(map);
		}

		// Token: 0x0600CB6E RID: 52078 RVA: 0x004AE520 File Offset: 0x004AC720
		public static Building_GravEngine GetPlayerGravEngine_NewTemp(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return null;
			}
			if (Find.TickManager.TicksGame != GravshipUtility.lastCachedEngineTick)
			{
				GravshipUtility.tmpMapToGravship.Clear();
			}
			Building_GravEngine result;
			if (GravshipUtility.tmpMapToGravship.TryGetValue(map, out result))
			{
				return result;
			}
			ThingDef def = ThingDefOf.GravEngine;
			Building_GravEngine building_GravEngine = map.listerThings.ThingsOfDef(def).FirstOrDefault<Thing>() as Building_GravEngine;
			if (building_GravEngine == null)
			{
				building_GravEngine = (map.listerThings.ThingsOfDef(def.minifiedDef).FirstOrDefault<Thing>().GetInnerIfMinified() as Building_GravEngine);
			}
			if (building_GravEngine == null)
			{
				List<Thing> list = new List<Thing>();
				ThingOwnerUtility.GetAllThingsRecursively<Thing>(map, ThingRequest.ForDef(def.minifiedDef), list, true, null, false);
				Thing thing = list.Find(delegate(Thing t)
				{
					Thing innerIfMinified = t.GetInnerIfMinified();
					return ((innerIfMinified != null) ? innerIfMinified.def : null) == def;
				});
				if (thing != null)
				{
					building_GravEngine = (thing.GetInnerIfMinified() as Building_GravEngine);
				}
			}
			GravshipUtility.tmpMapToGravship[map] = building_GravEngine;
			GravshipUtility.lastCachedEngineTick = Find.TickManager.TicksGame;
			return building_GravEngine;
		}

		// Token: 0x0600CB6F RID: 52079 RVA: 0x004AE61C File Offset: 0x004AC81C
		public static bool PlayerHasGravEngine(Map map)
		{
			return ModsConfig.OdysseyActive && GravshipUtility.GetPlayerGravEngine_NewTemp(map) != null;
		}

		// Token: 0x0600CB70 RID: 52080 RVA: 0x004AE630 File Offset: 0x004AC830
		public static bool TryGetNameOfGravshipOnMap(Map map, out string name)
		{
			name = null;
			if (!ModsConfig.OdysseyActive || map == null || !GravshipUtility.PlayerHasGravEngine(map))
			{
				return false;
			}
			Building_GravEngine playerGravEngine_NewTemp = GravshipUtility.GetPlayerGravEngine_NewTemp(map);
			if (playerGravEngine_NewTemp == null)
			{
				return false;
			}
			if (playerGravEngine_NewTemp.nameHidden)
			{
				return false;
			}
			name = playerGravEngine_NewTemp.RenamableLabel;
			return true;
		}

		// Token: 0x0600CB71 RID: 52081 RVA: 0x004AE674 File Offset: 0x004AC874
		public static void GetConnectedSubstructure(Building_GravEngine engine, HashSet<IntVec3> cells, int maxCells, bool requireInsideFootprint = true)
		{
			cells.Clear();
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			if (!engine.Spawned)
			{
				Log.Error("Tried to get connected substructure for an unspawned engine.");
				return;
			}
			List<Thing> footprintMakers = engine.Map.listerThings.ThingsInGroup(ThingRequestGroup.SubstructureFootprint);
			engine.Map.floodFiller.FloodFill(engine.Position, delegate(IntVec3 x)
			{
				if (x.InBounds(engine.Map))
				{
					TerrainDef terrainDef = engine.Map.terrainGrid.FoundationAt(x);
					if (terrainDef != null && terrainDef.IsSubstructure)
					{
						return !requireInsideFootprint || GravshipUtility.InsideFootprint(x, engine.Map, footprintMakers, null);
					}
				}
				return false;
			}, delegate(IntVec3 x)
			{
				cells.Add(x);
				return false;
			}, maxCells, false, null);
		}

		// Token: 0x0600CB72 RID: 52082 RVA: 0x004AE720 File Offset: 0x004AC920
		public static bool InsideFootprint(IntVec3 loc, Map map, List<Thing> footprintMakers = null, Thing thingToExclude = null)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			if (footprintMakers == null)
			{
				footprintMakers = map.listerThings.ThingsInGroup(ThingRequestGroup.SubstructureFootprint);
			}
			foreach (Thing thing in footprintMakers)
			{
				CompSubstructureFootprint compSubstructureFootprint;
				if (thing.Spawned && thing.TryGetComp(out compSubstructureFootprint) && compSubstructureFootprint.Valid && loc.InHorDistOf(thing.Position, compSubstructureFootprint.Props.radius) && (thingToExclude == null || thing != thingToExclude))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600CB73 RID: 52083 RVA: 0x004AE7C4 File Offset: 0x004AC9C4
		public static void PreLaunchConfirmation(Building_GravEngine engine, Action launchAction)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			TaggedString taggedString = "ConfirmGravEngineLaunch".Translate();
			List<Pawn> list = new List<Pawn>();
			List<Pawn> list2 = new List<Pawn>();
			List<Thing> list3 = new List<Thing>();
			List<Pawn> list4 = new List<Pawn>();
			foreach (Pawn pawn in engine.Map.mapPawns.AllPawns)
			{
				if (pawn.Faction == Faction.OfPlayer && (!engine.ValidSubstructureAt(pawn.PositionHeld) || pawn.ParentHolder is ActiveTransporterInfo))
				{
					if (pawn.RaceProps.Humanlike)
					{
						list.Add(pawn);
					}
					else
					{
						list2.Add(pawn);
					}
				}
				if (GravshipUtility.ConsideredKidnapped(engine, pawn))
				{
					list4.Add(pawn);
				}
			}
			foreach (Pawn pawn2 in engine.Map.mapPawns.PrisonersOfColony)
			{
				if (!engine.ValidSubstructureAt(pawn2.PositionHeld) && !list.Contains(pawn2))
				{
					list.Add(pawn2);
				}
			}
			if (list.Count > 0)
			{
				TaggedString t = taggedString;
				string[] array = new string[5];
				array[0] = "\n\n";
				array[1] = ("GravEngineWarning".Translate() + ": ").Colorize(ColorLibrary.RedReadable);
				array[2] = "PeopleWillBeLeftBehind".Translate().Resolve();
				array[3] = ":\n";
				array[4] = (from p in list
				select p.NameFullColored.Resolve()).ToLineList("  - ", true);
				taggedString = t + string.Concat(array);
			}
			if (list2.Count > 0)
			{
				TaggedString t2 = taggedString;
				string[] array2 = new string[5];
				array2[0] = "\n\n";
				array2[1] = ("GravEngineWarning".Translate() + ": ").Colorize(ColorLibrary.RedReadable);
				array2[2] = "AnimalsWillBeLeftBehind".Translate().Resolve();
				array2[3] = ":\n";
				array2[4] = (from p in list2
				select p.NameFullColored.Resolve()).ToLineList("  - ", true);
				taggedString = t2 + string.Concat(array2);
			}
			if (list4.Count > 0)
			{
				TaggedString t3 = taggedString;
				string[] array3 = new string[5];
				array3[0] = "\n\n";
				array3[1] = ("GravEngineWarning".Translate() + ": ").Colorize(ColorLibrary.RedReadable);
				array3[2] = "PawnsKidnappedOnGravship".Translate().Resolve();
				array3[3] = ":\n";
				array3[4] = (from p in list4
				select p.NameFullColored.Resolve()).ToLineList("  - ", true);
				taggedString = t3 + string.Concat(array3);
			}
			foreach (IntVec3 c in engine.AllConnectedSubstructure)
			{
				Building edifice = c.GetEdifice(engine.Map);
				if (edifice != null && edifice.def.Size != IntVec2.One && !engine.OnValidSubstructure(edifice) && !list3.Contains(edifice))
				{
					list3.Add(edifice);
				}
			}
			if (list3.Count > 0)
			{
				TaggedString t4 = taggedString;
				string[] array4 = new string[5];
				array4[0] = "\n\n";
				array4[1] = ("GravEngineWarning".Translate() + ": ").Colorize(ColorLibrary.RedReadable);
				array4[2] = "BuildingsWillBeLeftBehind".Translate().Resolve();
				array4[3] = ":\n";
				array4[4] = (from b in list3
				select b.LabelShort).ToLineList("  - ", true);
				taggedString = t4 + string.Concat(array4);
			}
			HashSet<string> hashSet = new HashSet<string>();
			foreach (IntVec3 intVec in engine.Map.AllCells)
			{
				TerrainDef terrainDef = engine.Map.terrainGrid.FoundationAt(intVec);
				if (terrainDef != null && terrainDef.IsSubstructure)
				{
					if (!GravshipUtility.InsideFootprint(intVec, engine.Map, null, null))
					{
						hashSet.Add("SubstructureOutsideFootprint".Translate());
					}
					if (!engine.ValidSubstructureAt(intVec))
					{
						hashSet.Add("DisconnectedSubstructure".Translate());
					}
					RoofDef roofDef = engine.Map.roofGrid.RoofAt(intVec);
					if (roofDef != null && roofDef.isThickRoof)
					{
						hashSet.Add("SubstructureUnderRockRoof".Translate());
					}
				}
			}
			if (hashSet.Count > 0)
			{
				taggedString += "\n\n" + ("GravEngineWarning".Translate() + ": ").Colorize(ColorLibrary.RedReadable) + "SubstructureWillBeLeftBehind".Translate() + ":\n" + hashSet.ToLineList("  - ", true);
			}
			Find.WindowStack.Add(new Dialog_MessageBox(taggedString, null, launchAction, "Cancel".Translate(), delegate()
			{
			}, null, true, launchAction, null, WindowLayer.Dialog));
		}

		// Token: 0x0600CB74 RID: 52084 RVA: 0x004AED90 File Offset: 0x004ACF90
		public static void TravelTo(Gravship gravship, PlanetTile oldTile, PlanetTile newTile)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			gravship.SetFaction(gravship.Engine.Faction);
			if (oldTile.Layer != newTile.Layer)
			{
				oldTile = newTile.Layer.GetClosestTile_NewTemp(oldTile, false);
			}
			gravship.Tile = oldTile;
			gravship.destinationTile = newTile;
			Find.WorldObjects.Add(gravship);
			CameraJumper.TryJump(gravship, CameraJumper.MovementMode.Pan);
		}

		// Token: 0x0600CB75 RID: 52085 RVA: 0x004AEDFC File Offset: 0x004ACFFC
		public static Gravship GenerateGravship(Building_GravEngine engine)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return null;
			}
			GravshipUtility.generatingGravship = true;
			Map map = engine.Map;
			IntVec3[] array = engine.ValidSubstructure.ToArray<IntVec3>();
			Gravship gravship = new Gravship(engine);
			gravship = (WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Gravship, gravship) as Gravship);
			Current.Game.Gravship = gravship;
			List<Thing> list = gravship.Things.ToList<Thing>();
			list.SortByDescending(new Func<Thing, int>(GravshipUtility.ThingSpawnPriority));
			foreach (Thing thing in list)
			{
				thing.PreSwapMap();
			}
			foreach (Thing thing2 in list)
			{
				if (thing2.Spawned)
				{
					thing2.DeSpawn(DestroyMode.WillReplace);
				}
			}
			foreach (Pawn pawn in gravship.Pawns)
			{
				pawn.PreSwapMap();
				if (pawn.Spawned)
				{
					pawn.DeSpawn(DestroyMode.WillReplace);
				}
			}
			ulong dirtyFlags = MapMeshFlagDefOf.Terrain | MapMeshFlagDefOf.Roofs | MapMeshFlagDefOf.Snow | MapMeshFlagDefOf.Sand | MapMeshFlagDefOf.Gas;
			for (int i = 0; i < array.Length; i++)
			{
				IntVec3 intVec = array[i];
				int index = map.cellIndices.CellToIndex(intVec);
				map.terrainGrid.RemoveGravshipTerrainUnsafe(intVec, index);
				map.roofGrid.RemoveRoofUnsafe(index);
				map.areaManager.BuildRoof[index] = false;
				map.areaManager.NoRoof[index] = false;
				map.glowGrid.DirtyCell(intVec);
				map.snowGrid.DepthGrid_Unsafe[index] = 0f;
				map.sandGrid.DepthGrid_Unsafe[index] = 0f;
				map.gasGrid.ClearCellUnsafe(intVec);
				foreach (Designation des in map.designationManager.AllDesignationsAt(intVec))
				{
					map.designationManager.RemoveDesignation(des);
				}
				IntVec2 cell2D = intVec.ToIntVec2;
				Predicate<IFleck> <>9__0;
				foreach (FleckSystem fleckSystem in map.flecks.Systems)
				{
					Predicate<IFleck> shouldRemove;
					if ((shouldRemove = <>9__0) == null)
					{
						shouldRemove = (<>9__0 = ((IFleck fleck) => fleck.GetPosition().ToIntVec2() == cell2D));
					}
					fleckSystem.RemoveAllFlecks(shouldRemove);
				}
				map.mapDrawer.MapMeshDirty(intVec, dirtyFlags, true, false);
				map.pathing.RecalculatePerceivedPathCostAt(intVec);
			}
			RoofCollapseCellsFinder.CheckAndRemoveCollpsingRoofs(map);
			map.roofGrid.Drawer.SetDirty();
			GravshipUtility.generatingGravship = false;
			return gravship;
		}

		// Token: 0x0600CB76 RID: 52086 RVA: 0x004AF14C File Offset: 0x004AD34C
		public static void AbandonMap(Map map)
		{
			if (!ModLister.CheckOdyssey("Gravship"))
			{
				return;
			}
			List<Pawn> list = map.mapPawns.AllPawnsSpawned.ToList<Pawn>();
			PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(list, PawnDiedOrDownedThoughtsKind.Lost);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				list[i].Notify_LeftBehind();
				if (list[i].Spawned)
				{
					list[i].DeSpawn(DestroyMode.Vanish);
				}
				if (!list[i].Destroyed)
				{
					list[i].DestroyOrPassToWorld(DestroyMode.Vanish);
				}
			}
			map.Parent.Abandon(true);
		}

		// Token: 0x0600CB77 RID: 52087 RVA: 0x004AF1E0 File Offset: 0x004AD3E0
		public static void ArriveExistingMap(Gravship gravship)
		{
			GravshipUtility.<>c__DisplayClass21_0 CS$<>8__locals1 = new GravshipUtility.<>c__DisplayClass21_0();
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			GravshipUtility.<>c__DisplayClass21_0 CS$<>8__locals2 = CS$<>8__locals1;
			MapParent mapParent = Find.WorldObjects.MapParentAt(gravship.destinationTile);
			CS$<>8__locals2.map = ((mapParent != null) ? mapParent.Map : null);
			if (CS$<>8__locals1.map == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to arrive gravship ");
				defaultInterpolatedStringHandler.AppendFormatted(gravship.Label);
				defaultInterpolatedStringHandler.AppendLiteral(" at tile ");
				defaultInterpolatedStringHandler.AppendFormatted<PlanetTile>(gravship.destinationTile);
				defaultInterpolatedStringHandler.AppendLiteral(" but no map exists there.");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			Find.GravshipController.landingMap = CS$<>8__locals1.map;
			GravshipLandingMarker gravshipLandingMarker = ThingMaker.MakeThing(ThingDefOf.GravshipLandingMarker, null) as GravshipLandingMarker;
			gravshipLandingMarker.gravship = gravship;
			gravshipLandingMarker.CacheCells();
			Find.GravshipController.Notify_LandingAreaConfirmationStarted(gravshipLandingMarker);
			Find.WorldObjects.Remove(gravship);
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (Prefs.PauseOnLoad)
				{
					Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
				}
				CameraJumper.TryJump(CS$<>8__locals1.map.Center, CS$<>8__locals1.map, CameraJumper.MovementMode.Cut);
				Find.DesignatorManager.Select(Find.GravshipController.MoveDesignator());
			});
		}

		// Token: 0x0600CB78 RID: 52088 RVA: 0x004AF2D0 File Offset: 0x004AD4D0
		public static void ArriveNewMap(Gravship gravship)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			PlanetTile destinationTile = gravship.destinationTile;
			MapParent mapParent = Find.WorldObjects.MapParentAt(destinationTile);
			if (mapParent == null)
			{
				if (destinationTile.LayerDef.DefaultWorldObject == destinationTile.LayerDef.SettlementWorldObjectDef)
				{
					mapParent = SettleUtility.AddNewHome(destinationTile, Faction.OfPlayer);
				}
				else
				{
					mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(destinationTile.LayerDef.DefaultWorldObject);
					mapParent.Tile = destinationTile;
					Find.WorldObjects.Add(mapParent);
				}
				if (mapParent.def.canHaveFaction && mapParent.Faction == null)
				{
					mapParent.SetFaction(Faction.OfPlayer);
				}
				mapParent.Tile = destinationTile;
				Settlement settlement = mapParent as Settlement;
				if (settlement != null)
				{
					settlement.Name = "GravshipLandingSite".Translate(gravship.Label).CapitalizeFirst();
					settlement.namedByPlayer = true;
				}
			}
			else if (mapParent.def.canHaveFaction && mapParent.Faction == null)
			{
				mapParent.SetFaction(Faction.OfPlayer);
			}
			IntVec3 intVec = Find.World.info.initialMapSize;
			if (mapParent.def.overrideMapSize != null)
			{
				intVec = mapParent.def.overrideMapSize.Value;
			}
			else if (mapParent.def.canResizeToGravship)
			{
				intVec.x = Mathf.RoundToInt(Mathf.Ceil((float)gravship.Bounds.maxX / 100f) * 100f);
				intVec.z = Mathf.RoundToInt(Mathf.Ceil((float)gravship.Bounds.maxZ / 100f) * 100f);
				intVec.y = 1;
			}
			Site site = mapParent as Site;
			if (site != null)
			{
				IntVec3 preferredMapSize = site.PreferredMapSize;
				intVec = new IntVec3(Mathf.Max(intVec.x, preferredMapSize.x), Mathf.Max(intVec.y, preferredMapSize.y), Mathf.Max(intVec.z, preferredMapSize.z));
				if (site.MainSitePartDef.minMapSize != null)
				{
					IntVec3 value = site.MainSitePartDef.minMapSize.Value;
					intVec = new IntVec3(Mathf.Max(intVec.x, value.x), Mathf.Max(intVec.y, value.y), Mathf.Max(intVec.z, value.z));
				}
			}
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(destinationTile, intVec, mapParent.def, GravshipUtility.GetGenSteps(gravship), false);
			bool flag;
			if (!(orGenerateMap.Parent is Settlement))
			{
				Site site2 = orGenerateMap.Parent as Site;
				if (site2 != null)
				{
					flag = site2.parts.All((SitePart part) => part.def.considerEnteringAsAttack);
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				flag = true;
			}
			if (flag && orGenerateMap.Parent.Faction != null && orGenerateMap.Parent.Faction != Faction.OfPlayer)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				TaggedString label = "LetterLabelGravshipEnteredEnemyBase".Translate();
				TaggedString text = "LetterGravshipEnteredEnemyBase".Translate(orGenerateMap.Parent.Label.ApplyTag(TagType.Settlement, orGenerateMap.Parent.Faction.GetUniqueLoadID())).CapitalizeFirst();
				SettlementUtility.AffectRelationsOnAttacked(orGenerateMap.Parent, ref text);
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(from p in orGenerateMap.mapPawns.AllPawns
				where !gravship.ContainsPawn(p)
				select p, ref label, ref text, "LetterRelatedPawnsSettlement".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, null, 0, true);
				Find.GoodwillSituationManager.RecalculateAll(true);
			}
			if (gravship.Spawned)
			{
				Find.WorldObjects.Remove(gravship);
			}
			TaleRecorder.RecordTale(TaleDefOf.TileSettled, Array.Empty<object>()).customLabel = "NewSettlement".Translate();
			if (Find.IdeoManager != null)
			{
				Find.IdeoManager.lastResettledTick = GenTicks.TicksGame;
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (Prefs.PauseOnLoad)
				{
					Find.TickManager.DoSingleTick();
					Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
				}
				CameraJumper.TryJump(gravship.Engine, CameraJumper.MovementMode.Cut);
				Find.CameraDriver.shaker.DoShake(0.1f, 180);
			});
		}

		// Token: 0x0600CB79 RID: 52089 RVA: 0x004AF718 File Offset: 0x004AD918
		public static int ThingSpawnPriority(Thing a)
		{
			if (!ModLister.CheckOdyssey("Gravship thing spawn priority"))
			{
				return 0;
			}
			if (a is Building_Bed)
			{
				return 20;
			}
			BuildingProperties building = a.def.building;
			if (building != null && building.isAttachment)
			{
				return 10;
			}
			if (a.def.category == ThingCategory.Item)
			{
				return 20;
			}
			return a.def.gravshipSpawnPriority;
		}

		// Token: 0x0600CB7A RID: 52090 RVA: 0x004AF778 File Offset: 0x004AD978
		public static bool TryGetPathFuelCost(PlanetTile from, PlanetTile to, out float cost, out int distance, float fuelPerTile = 10f, float fuelFactor = 1f)
		{
			cost = 0f;
			distance = 0;
			if (!ModLister.CheckOdyssey("Gravship path fuel cost"))
			{
				return false;
			}
			if (GravshipUtility.cachedOrigin == from && GravshipUtility.cachedDest == to)
			{
				cost = GravshipUtility.cachedCost;
				distance = GravshipUtility.cachedDistance;
				return GravshipUtility.cachedResult;
			}
			GravshipUtility.cachedOrigin = from;
			GravshipUtility.cachedDest = to;
			GravshipUtility.cachedCost = (cost = 0f);
			GravshipUtility.cachedDistance = (distance = 0);
			if (from.Layer != to.Layer)
			{
				if (GravshipUtility.cachedOriginLayer == from.Layer && GravshipUtility.cachedDestLayer == to.Layer)
				{
					cost += GravshipUtility.cachedLayerCost;
				}
				else
				{
					float num;
					if (!from.Layer.TryGetPath(to.Layer, GravshipUtility.connections, out num, 2147483647))
					{
						return GravshipUtility.cachedResult = false;
					}
					GravshipUtility.cachedOriginLayer = to.Layer;
					GravshipUtility.cachedDestLayer = from.Layer;
					cost += (GravshipUtility.cachedLayerCost = num);
					GravshipUtility.connections.Clear();
				}
				from = to.Layer.GetClosestTile_NewTemp(from, false);
			}
			GravshipUtility.cachedDistance = (distance = Find.WorldGrid.TraversalDistanceBetween(from, to, true, int.MaxValue, false));
			cost += (float)distance * to.LayerDef.rangeDistanceFactor * (fuelPerTile * fuelFactor);
			GravshipUtility.cachedCost = (cost = Mathf.Max(cost, 50f));
			return GravshipUtility.cachedResult = true;
		}

		// Token: 0x0600CB7B RID: 52091 RVA: 0x004AF8E8 File Offset: 0x004ADAE8
		public static int MaxDistForFuel(float fuel, PlanetLayer fromLayer, PlanetLayer toLayer, float fuelPerTile = 10f, float fuelFactor = 1f)
		{
			if (fromLayer != toLayer)
			{
				if (GravshipUtility.cachedOriginLayer == fromLayer && GravshipUtility.cachedDestLayer == toLayer)
				{
					fuel -= GravshipUtility.cachedLayerCost;
				}
				else
				{
					float num;
					if (!fromLayer.TryGetPath(toLayer, GravshipUtility.connections, out num, 2147483647))
					{
						return 0;
					}
					GravshipUtility.cachedOriginLayer = toLayer;
					GravshipUtility.cachedDestLayer = fromLayer;
					fuel -= (GravshipUtility.cachedLayerCost = num);
					GravshipUtility.connections.Clear();
				}
			}
			return Mathf.FloorToInt(fuel / (toLayer.Def.rangeDistanceFactor * (fuelPerTile * fuelFactor)));
		}

		// Token: 0x0600CB7C RID: 52092 RVA: 0x004AF963 File Offset: 0x004ADB63
		private static IEnumerable<GenStepWithParams> GetGenSteps(Gravship gravship)
		{
			GravshipUtility.<GetGenSteps>d__35 <GetGenSteps>d__ = new GravshipUtility.<GetGenSteps>d__35(-2);
			<GetGenSteps>d__.<>3__gravship = gravship;
			return <GetGenSteps>d__;
		}

		// Token: 0x0600CB7D RID: 52093 RVA: 0x004AF974 File Offset: 0x004ADB74
		public static void SettleTile(Map map)
		{
			if (!ModLister.CheckOdyssey("Gravship settle tile"))
			{
				return;
			}
			MapParent parent = map.Parent;
			Settlement settlement = SettleUtility.AddNewHome(map.Tile, Faction.OfPlayer);
			map.info.parent = settlement;
			if (parent != null)
			{
				settlement.questTags = parent.questTags;
				parent.Notify_MyMapSettled(map);
				parent.Destroy();
			}
			settlement.Notify_MyMapSettled(map);
			Settlement settlement2 = parent as Settlement;
			if (settlement2 != null)
			{
				TaggedString taggedString = "";
				SettlementUtility.AffectRelationsOnAttacked(settlement2, ref taggedString);
			}
		}

		// Token: 0x0600CB7E RID: 52094 RVA: 0x004AF9F4 File Offset: 0x004ADBF4
		public static bool ConsideredKidnapped(Building_GravEngine engine, Pawn pawn)
		{
			return ModsConfig.OdysseyActive && engine.ValidSubstructureAt(pawn.PositionHeld) && pawn.Faction != Faction.OfPlayer && !pawn.IsPlayerControlled && !pawn.IsPrisonerOfColony && !pawn.IsSlaveOfColony && !pawn.Faction.HostileTo(Faction.OfPlayer) && pawn.Faction != null;
		}

		// Token: 0x0600CB7F RID: 52095 RVA: 0x004AFA5E File Offset: 0x004ADC5E
		public static float LaunchCooldownFromQuality(float quality)
		{
			return GravshipUtility.LaunchCooldownFromQualityCurve.Evaluate(quality);
		}

		// Token: 0x0600CB80 RID: 52096 RVA: 0x004AFA6B File Offset: 0x004ADC6B
		public static float NegativeLandingOutcomeFromQuality(float quality)
		{
			return GravshipUtility.NegativeLandingOutcomeFromQualityCurve.Evaluate(quality);
		}

		// Token: 0x0600CB81 RID: 52097 RVA: 0x004AFA78 File Offset: 0x004ADC78
		public static bool IsOnboardGravship(IntVec3 cell, Building_GravEngine engine, Pawn pawn = null, bool desperate = false)
		{
			return GravshipUtility.IsOnboardGravship_NewTemp(cell, engine, pawn, desperate, true);
		}

		// Token: 0x0600CB82 RID: 52098 RVA: 0x004AFA84 File Offset: 0x004ADC84
		public static bool IsOnboardGravship_NewTemp(IntVec3 cell, Building_GravEngine engine, Pawn pawn = null, bool desperate = false, bool respectAllowedAreas = true)
		{
			return ModsConfig.OdysseyActive && (!respectAllowedAreas || pawn == null || cell.InAllowedArea(pawn)) && (desperate || !cell.GetRoom(engine.Map).PsychologicallyOutdoors) && engine.ValidSubstructure.Contains(cell);
		}

		// Token: 0x0600CB83 RID: 52099 RVA: 0x004AFAD0 File Offset: 0x004ADCD0
		public static bool TryFindSpotOnGravship(Pawn pawn, Building_GravEngine engine, out IntVec3 spot)
		{
			GravshipUtility.<>c__DisplayClass44_0 CS$<>8__locals1 = new GravshipUtility.<>c__DisplayClass44_0();
			CS$<>8__locals1.engine = engine;
			spot = IntVec3.Invalid;
			bool desperate = false;
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			CS$<>8__locals1.tmpWorkingCellList = new List<IntVec3>();
			TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false);
			Region region = CS$<>8__locals1.<TryFindSpotOnGravship>g__ClosestShipRegion|0(pawn.Position, pawn.MapHeld, pawn, traverseParms, false);
			if (region == null)
			{
				desperate = true;
				region = CS$<>8__locals1.<TryFindSpotOnGravship>g__ClosestShipRegion|0(pawn.Position, pawn.MapHeld, pawn, traverseParms, true);
			}
			return region != null && CS$<>8__locals1.<TryFindSpotOnGravship>g__TryGetAllowedCellInRegion|1(region, pawn, out spot, desperate);
		}

		// Token: 0x0600CB84 RID: 52100 RVA: 0x004AFB5C File Offset: 0x004ADD5C
		public static bool TryFindSpotOffGravship(Pawn pawn, Building_GravEngine engine, out IntVec3 spot)
		{
			GravshipUtility.<>c__DisplayClass45_0 CS$<>8__locals1 = new GravshipUtility.<>c__DisplayClass45_0();
			CS$<>8__locals1.engine = engine;
			spot = IntVec3.Invalid;
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			CS$<>8__locals1.tmpWorkingCellList = new List<IntVec3>();
			Region region = CS$<>8__locals1.<TryFindSpotOffGravship>g__ClosestNonShipRegion|0(pawn.Position, pawn.MapHeld, pawn, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true));
			return region != null && CS$<>8__locals1.<TryFindSpotOffGravship>g__TryGetAllowedCellInRegion|1(region, pawn, out spot);
		}

		// Token: 0x0600CB85 RID: 52101 RVA: 0x004AFBC8 File Offset: 0x004ADDC8
		public static void UpdateBillDestinations(Map map)
		{
			using (IEnumerator<Bill> enumerator = BillUtility.MapBills(map).GetEnumerator())
			{
				IL_12A:
				while (enumerator.MoveNext())
				{
					Bill bill = enumerator.Current;
					foreach (StorageGroup storageGroup in map.storageGroups.StorageGroupsForReading)
					{
						StorageGroup storageGroup2 = bill.GetSlotGroup() as StorageGroup;
						if (storageGroup2 != null && storageGroup2.RenamableLabel == storageGroup.RenamableLabel)
						{
							bill.SetStoreMode(BillStoreModeDefOf.SpecificStockpile, storageGroup);
							goto IL_12A;
						}
					}
					foreach (Zone zone in map.zoneManager.AllZones)
					{
						Zone_Stockpile zone_Stockpile = zone as Zone_Stockpile;
						if (zone_Stockpile != null)
						{
							SlotGroup slotGroup = bill.GetSlotGroup() as SlotGroup;
							if (slotGroup != null)
							{
								Zone zone2 = slotGroup.parent as Zone;
								if (zone2 != null && zone2.RenamableLabel == zone.RenamableLabel)
								{
									bill.SetStoreMode(BillStoreModeDefOf.SpecificStockpile, zone_Stockpile.GetSlotGroup());
									goto IL_12A;
								}
							}
						}
					}
					if (bill.GetStoreMode() == BillStoreModeDefOf.SpecificStockpile)
					{
						bill.SetStoreMode(BillStoreModeDefOf.DropOnFloor, null);
					}
				}
			}
		}

		// Token: 0x040081AA RID: 33194
		private static List<ThingDef> allShipComponents;

		// Token: 0x040081AB RID: 33195
		public static bool generatingGravship = false;

		// Token: 0x040081AC RID: 33196
		public static readonly Color ConnectedSubstructureColor = ColorLibrary.Cyan;

		// Token: 0x040081AD RID: 33197
		public static readonly Color DisconnectedSubstructureColor = ColorLibrary.Red;

		// Token: 0x040081AE RID: 33198
		private const float MinFuelCost = 50f;

		// Token: 0x040081AF RID: 33199
		private static readonly ProfilerMarker PlayerHasGravEngineMarker = new ProfilerMarker("PlayerHasGravEngine()");

		// Token: 0x040081B0 RID: 33200
		private static int lastCachedEngineTick = -1;

		// Token: 0x040081B1 RID: 33201
		private static readonly Dictionary<Map, Building_GravEngine> tmpMapToGravship = new Dictionary<Map, Building_GravEngine>();

		// Token: 0x040081B2 RID: 33202
		private static readonly List<PlanetLayerConnection> connections = new List<PlanetLayerConnection>();

		// Token: 0x040081B3 RID: 33203
		private static PlanetTile cachedOrigin;

		// Token: 0x040081B4 RID: 33204
		private static PlanetTile cachedDest;

		// Token: 0x040081B5 RID: 33205
		private static float cachedCost;

		// Token: 0x040081B6 RID: 33206
		private static float cachedLayerCost;

		// Token: 0x040081B7 RID: 33207
		private static PlanetLayer cachedOriginLayer;

		// Token: 0x040081B8 RID: 33208
		private static PlanetLayer cachedDestLayer;

		// Token: 0x040081B9 RID: 33209
		private static bool cachedResult;

		// Token: 0x040081BA RID: 33210
		private static int cachedDistance;

		// Token: 0x040081BB RID: 33211
		private static readonly SimpleCurve LaunchCooldownFromQualityCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 600000f),
				true
			},
			{
				new CurvePoint(0.25f, 300000f),
				true
			},
			{
				new CurvePoint(0.5f, 120000f),
				true
			},
			{
				new CurvePoint(0.8f, 30000f),
				true
			},
			{
				new CurvePoint(1f, 20000f),
				true
			}
		};

		// Token: 0x040081BC RID: 33212
		private static readonly SimpleCurve NegativeLandingOutcomeFromQualityCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(0.5f, 0.25f),
				true
			},
			{
				new CurvePoint(1f, 0.02f),
				true
			}
		};
	}
}
