using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020000CF RID: 207
	[StaticConstructorOnStartup]
	public class WorldComponent_GravshipController : WorldComponent
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x0002D05C File Offset: 0x0002B25C
		public static bool CutsceneInProgress
		{
			get
			{
				return WorldComponent_GravshipController.cutsceneInProgress;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x0002D063 File Offset: 0x0002B263
		public static bool GravshipRenderInProgess
		{
			get
			{
				return GravshipCapturer.IsGravshipRenderInProgress;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x0002D06A File Offset: 0x0002B26A
		// (set) Token: 0x060007C8 RID: 1992 RVA: 0x0002D071 File Offset: 0x0002B271
		public static bool DisableDrawingPollution { get; private set; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x0002D079 File Offset: 0x0002B279
		public static CellRect GravshipRenderBounds
		{
			get
			{
				return GravshipCapturer.GravshipCaptureBounds;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060007CA RID: 1994 RVA: 0x0002D080 File Offset: 0x0002B280
		public bool IsGravshipTravelling
		{
			get
			{
				return this.gravship != null;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x0002D08B File Offset: 0x0002B28B
		public bool LandingAreaConfirmationInProgress
		{
			get
			{
				return this.landingMarker != null;
			}
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0002D098 File Offset: 0x0002B298
		public WorldComponent_GravshipController(World world) : base(world)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.gravshipCapturer = new GravshipCapturer();
			this.gravshipRenderer = new GravshipRenderer();
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0002D0F8 File Offset: 0x0002B2F8
		public void InitiateTakeoff(Building_GravEngine engine, PlanetTile targetTile)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.map = engine.Map;
			Current.Game.CurrentMap = this.map;
			this.takeoffTile = this.map.Tile;
			this.landingTile = targetTile;
			this.mapHasGravAnchor = this.map.listerThings.AnyThingWithDef(ThingDefOf.GravAnchor);
			SectionLayer_GravshipMask.Engine = engine;
			this.RegenerateGravshipMask();
			WorldComponent_GravshipController.cutsceneInProgress = true;
			this.isTakeoff = true;
			this.gravshipRenderer.Init(this.map);
			this.gravshipCapturer.BeginGravshipRender(engine, new Action<Capture>(this.OnGravshipCaptureComplete));
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0002D1A0 File Offset: 0x0002B3A0
		public static void DestroyTreesAroundSubstructure(Map map, HashSet<IntVec3> substructure, int radius = 2, HashSet<IntVec3> borderCells = null)
		{
			if (!ModLister.CheckOdyssey("Gravship"))
			{
				return;
			}
			if (borderCells == null)
			{
				borderCells = new HashSet<IntVec3>(substructure);
			}
			HashSet<IntVec3> hashSet = (radius > 1) ? new HashSet<IntVec3>() : null;
			foreach (IntVec3 root in borderCells)
			{
				foreach (IntVec3 intVec in GenAdjFast.AdjacentCells8Way(root))
				{
					if (!substructure.Contains(intVec) && intVec.InBounds(map))
					{
						if (hashSet != null)
						{
							hashSet.Add(intVec);
						}
						List<Thing> list = map.thingGrid.ThingsListAtFast(intVec);
						for (int i = list.Count - 1; i >= 0; i--)
						{
							Thing thing = list[i];
							if (thing.def.plant != null && (thing.def.plant.IsTree || Mathf.Max(thing.DrawSize.x, thing.DrawSize.y) > 1.5f))
							{
								list[i].DeSpawn(DestroyMode.Vanish);
							}
						}
					}
				}
			}
			if (radius > 0)
			{
				WorldComponent_GravshipController.DestroyTreesAroundSubstructure(map, substructure, radius - 1, hashSet);
			}
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0002D308 File Offset: 0x0002B508
		private static HashSet<Section> SectionsWithPollution(Map map, IEnumerable<IntVec3> cells)
		{
			HashSet<Section> hashSet = new HashSet<Section>();
			if (!ModsConfig.BiotechActive || map.pollutionGrid.TotalPollution <= 0)
			{
				return hashSet;
			}
			foreach (IntVec3 intVec in cells)
			{
				if (intVec.IsPolluted(map))
				{
					hashSet.Add(map.mapDrawer.SectionAt(intVec));
				}
			}
			return hashSet;
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0002D384 File Offset: 0x0002B584
		private static void RegenerateTerrainInSections(IEnumerable<Section> sections)
		{
			foreach (Section section in sections)
			{
				section.RegenerateSingleLayer(section.GetLayer(typeof(SectionLayer_Terrain)));
			}
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0002D3DC File Offset: 0x0002B5DC
		private void RegenerateGravshipMask()
		{
			this.map.mapDrawer.RegenerateLayerNow(typeof(SectionLayer_GravshipMask));
			this.map.mapDrawer.RegenerateLayerNow(typeof(SectionLayer_GravshipHull));
			this.map.mapDrawer.RegenerateLayerNow(typeof(SectionLayer_SubstructureProps));
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0002D438 File Offset: 0x0002B638
		public void OnGravshipCaptureComplete(Capture capture)
		{
			this.zoomRange = this.GetCutsceneZoomRange(capture);
			Action <>9__2;
			Action <>9__1;
			this.PanIf(Find.CameraDriver.config.gravshipPanOnCutsceneStart, capture.captureCenter, this.zoomRange.min, 1f, delegate
			{
				float seconds = 0.5f;
				Action action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate()
					{
						Action action2;
						if ((action2 = <>9__2) == null)
						{
							action2 = (<>9__2 = delegate()
							{
								HashSet<IntVec3> validSubstructure = capture.engine.ValidSubstructure;
								LayerSubMesh item = SectionLayer_IndoorMask.BakeGravshipIndoorMesh(this.map, validSubstructure, validSubstructure.Count, WorldComponent_GravshipController.IndoorMaskGravship, capture.captureCenter);
								List<LayerSubMesh> collection = SectionLayer_GravshipHull.BakeGravshipIndoorMesh(this.map, capture.captureBounds, capture.captureCenter);
								this.gravship = this.RemoveGravshipFromMap(capture.engine);
								this.gravship.capture = capture;
								this.gravship.bakedIndoorMasks.Clear();
								this.gravship.bakedIndoorMasks.Add(item);
								this.gravship.bakedIndoorMasks.AddRange(collection);
								if (this.mapHasGravAnchor)
								{
									this.map.GetComponent<VacuumComponent>().RebuildData();
								}
								this.RegenerateGravshipMask();
								this.BeginTakeoffCutscene();
							});
						}
						LongEventHandler.QueueLongEvent(action2, "GeneratingGravship", false, null, true, false, null);
					});
				}
				Delay.AfterNSeconds(seconds, action);
			});
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0002D4A8 File Offset: 0x0002B6A8
		private void BeginTakeoffCutscene()
		{
			this.timeLeft = 10.5f;
			Find.CameraDriver.shaker.DoShake(0.2f, 120);
			this.gravshipRenderer.BeginCutscene(this.gravship, this.gravship.capture.captureCenter, this.gravship.originalPosition.ToVector3(), Rot4.North);
			this.gravshipAudio.BeginTakeoff();
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0002D518 File Offset: 0x0002B718
		public void InitiateLanding(Gravship gravship, Map map, IntVec3 landingPos, Rot4 landingRot)
		{
			WorldComponent_GravshipController.<>c__DisplayClass53_0 CS$<>8__locals1 = new WorldComponent_GravshipController.<>c__DisplayClass53_0();
			CS$<>8__locals1.landingPos = landingPos;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.gravship = gravship;
			CS$<>8__locals1.landingRot = landingRot;
			this.map = CS$<>8__locals1.map;
			this.gravship = null;
			WorldComponent_GravshipController.cutsceneInProgress = true;
			this.isTakeoff = false;
			Current.Game.CurrentMap = CS$<>8__locals1.map;
			Find.ScreenshotModeHandler.Active = true;
			int dist = Mathf.CeilToInt(Mathf.Max(15f, 15f));
			this.terrainCurtainBounds = CellRect.FromCellList(this.landingMarker.GravshipCells).ExpandedBy(dist).MovedBy(CS$<>8__locals1.landingPos.x, CS$<>8__locals1.landingPos.z);
			foreach (Pawn pawn in CS$<>8__locals1.map.mapPawns.AllPawnsSpawned)
			{
				if (this.terrainCurtainBounds.Contains(pawn.Position))
				{
					pawn.pather.ResetToCurrentPosition();
				}
			}
			CS$<>8__locals1.landingMarkerCells = new HashSet<IntVec3>(from c in this.landingMarker.GravshipCells
			select c + CS$<>8__locals1.landingPos);
			WorldComponent_GravshipController.DestroyTreesAroundSubstructure(CS$<>8__locals1.map, CS$<>8__locals1.landingMarkerCells, 2, null);
			if (WorldComponent_GravshipController.BiomeHasTransparency(CS$<>8__locals1.map.Biome))
			{
				GravshipCapturer.CaptureWorldSkybox(delegate(Texture2D skybox)
				{
					GravshipRenderer.MatTerrainCurtain.SetTexture(WorldComponent_GravshipController.ShaderPropertyBackgroundTex, skybox);
					base.<InitiateLanding>g__CaptureAndBeginCutscene|1();
				});
				return;
			}
			CS$<>8__locals1.<InitiateLanding>g__CaptureAndBeginCutscene|1();
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0002D6A4 File Offset: 0x0002B8A4
		private static bool BiomeHasTransparency(BiomeDef biome)
		{
			return biome == BiomeDefOf.Space || biome == BiomeDefOf.Orbit;
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0002D6B8 File Offset: 0x0002B8B8
		private void BeginLandingCutscene(IntVec3 landingPos, Rot4 landingRot)
		{
			this.timeLeft = 10f;
			Find.CameraDriver.shaker.DoShake(0.2f, 120);
			Vector3 b = WorldComponent_GravshipController.CornerOffsetsPerRotation[landingRot.AsInt];
			Vector3 takeoffOrLandingCenter = landingPos.ToVector3() + landingRot.AsQuat * this.gravship.engineToCenter + b;
			this.gravshipRenderer.BeginCutscene(this.gravship, takeoffOrLandingCenter, landingPos.ToVector3(), landingRot);
			this.gravshipAudio.BeginLanding();
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0002D747 File Offset: 0x0002B947
		private Gravship RemoveGravshipFromMap(Building_GravEngine engine)
		{
			if (!engine.Spawned)
			{
				Log.Error("Tried to make a gravship out of an unspawned engine");
				return null;
			}
			return GravshipUtility.GenerateGravship(engine);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0002D764 File Offset: 0x0002B964
		private void TakeoffEnded()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			WorldComponent_GravshipController.cutsceneInProgress = false;
			Find.CameraDriver.shaker.StopAllShaking();
			if (!this.mapHasGravAnchor)
			{
				GravshipUtility.AbandonMap(this.map);
			}
			else
			{
				GravshipUtility.UpdateBillDestinations(this.map);
			}
			GravshipUtility.TravelTo(this.gravship, this.takeoffTile, this.landingTile);
			Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
			this.gravshipAudio.EndTakeoff();
			this.map = null;
			this.ResetCutscene();
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x0002D7E8 File Offset: 0x0002B9E8
		public override void WorldComponentUpdate()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.gravshipCapturer.Update();
			float progress = Mathf.Clamp01(1f - this.timeLeft / 10f);
			this.gravshipAudio.Update(10f - this.timeLeft, progress, this.isTakeoff);
			if (this.forceDrawTerrainCurtain)
			{
				this.DrawTerrainCurtain(0f);
			}
			if (!this.gravshipCapturer.IsCaptureComplete || !WorldComponent_GravshipController.cutsceneInProgress || this.gravship == null)
			{
				return;
			}
			if (this.timeLeft > 0f && Prefs.GravshipCutscenes)
			{
				this.timeLeft -= Time.deltaTime;
				if (!this.isTakeoff && !this.forceDrawTerrainCurtain)
				{
					this.DrawTerrainCurtain(progress);
				}
				this.DrawGravship(progress);
				return;
			}
			if (DebugSettings.loopGravshipCutscene)
			{
				this.BeginTakeoffCutscene();
				return;
			}
			if (this.isTakeoff)
			{
				this.TakeoffEnded();
				return;
			}
			this.LandingEnded();
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x0002D8D8 File Offset: 0x0002BAD8
		private float EaseInOut(float x)
		{
			if (x >= 0.5f)
			{
				return 1f - Mathf.Pow(-2f * x + 2f, 3f) * 0.5f;
			}
			return 4f * x * x * x;
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0002D911 File Offset: 0x0002BB11
		private void PanIf(bool panCondition, Vector3 loc, float size, float duration = 0.25f, PanCompletionCallback onComplete = null)
		{
			if (panCondition)
			{
				Find.CameraDriver.PanToMapLocAndSize(loc, size, duration, onComplete);
				return;
			}
			if (onComplete != null)
			{
				onComplete();
			}
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0002D934 File Offset: 0x0002BB34
		private FloatRange GetCutsceneZoomRange(Capture capture)
		{
			CameraMapConfig config = Find.CameraDriver.config;
			if (config.gravshipEnableOverrideSizeRange)
			{
				return config.gravshipOverrideSizeRange;
			}
			return new FloatRange(capture.minCameraSize, capture.maxCameraSize);
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0002D96C File Offset: 0x0002BB6C
		private void DrawTerrainCurtain(float progress)
		{
			GravshipRenderer.MatTerrainCurtain.mainTexture = (Texture2D)this.terrainCapture.capture;
			float a = this.isPollutedLanding ? progress.RemapClamped(0.8f, 0.9f, 1f, 0f) : progress.RemapClamped(0.9f, 1f, 1f, 0f);
			Color color = new Color(1f, 1f, 1f, a);
			float num = WorldComponent_GravshipController.BiomeHasTransparency(this.map.Biome) ? 1f : 0f;
			Color value = (this.map.Biome != null && this.map.Biome.disableSkyLighting) ? new Color(1f, 1f, 1f, num) : this.map.skyManager.CurSky.colors.sky.WithAlpha(num);
			GravshipRenderer.MatTerrainCurtain.color = color;
			GravshipRenderer.MatTerrainCurtain.SetColor(WorldComponent_GravshipController.ShaderPropertyBackgroundColor, value);
			GenDraw.DrawCellRect(this.terrainCurtainBounds, Vector3.up * (AltitudeLayer.Skyfaller.AltitudeFor() - 0.03658537f), GravshipRenderer.MatTerrainCurtain, null, WorldComponent_GravshipController.LayerGravshipExclude);
			if (this.terrainCurtainIndoorMask != null)
			{
				Graphics.DrawMesh(this.terrainCurtainIndoorMask.mesh, Matrix4x4.TRS(this.terrainCurtainBounds.CenterVector3, Quaternion.identity, Vector3.one), this.terrainCurtainIndoorMask.material, 0);
			}
			if (this.terrainCurtainShadowMask != null)
			{
				Graphics.DrawMesh(this.terrainCurtainShadowMask.mesh, Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one), this.terrainCurtainShadowMask.material, 0);
			}
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0002DB20 File Offset: 0x0002BD20
		private void DrawGravship(float progress)
		{
			this.gravshipRenderer.BeginUpdate();
			FloatRange cutsceneZoomRange = this.GetCutsceneZoomRange(this.gravship.capture);
			float b;
			if (this.isTakeoff)
			{
				this.gravshipRenderer.UpdateTakeoff(progress);
				b = Mathf.Lerp(cutsceneZoomRange.min, cutsceneZoomRange.max, this.EaseInOut(progress.RemapClamped(0f, 0.7f, 0f, 1f)));
			}
			else
			{
				this.gravshipRenderer.UpdateLanding(progress, this.isPollutedLanding);
				b = Mathf.Lerp(cutsceneZoomRange.max, cutsceneZoomRange.min, this.EaseInOut(progress.RemapClamped(0f, 0.8f, 0f, 1f)));
			}
			CameraDriver cameraDriver = Find.CameraDriver;
			if (!cameraDriver.config.gravshipFreeCam)
			{
				cameraDriver.SetRootSize(Mathf.Lerp(cameraDriver.RootSize, b, 5f * Time.deltaTime));
			}
			this.gravshipRenderer.EndUpdate();
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0002DC11 File Offset: 0x0002BE11
		public override void FinalizeInit(bool fromLoad)
		{
			if (!fromLoad)
			{
				this.ResetCutscene();
			}
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0002DC1C File Offset: 0x0002BE1C
		private void ResetCutscene()
		{
			Find.ScreenshotModeHandler.Active = false;
			WorldComponent_GravshipController.cutsceneInProgress = false;
			this.landingMap = null;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0002DC36 File Offset: 0x0002BE36
		public void Notify_LandingAreaConfirmationStarted(GravshipLandingMarker marker)
		{
			this.landingMarker = marker;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0002DC40 File Offset: 0x0002BE40
		private void LandingEnded()
		{
			Map map = this.map;
			SectionLayer_GravshipMask.Engine = this.gravship.Engine;
			SectionLayer_GravshipMask.OverrideMode = SectionLayer_GravshipMask.MaskOverrideMode.Gravship;
			this.RegenerateGravshipMask();
			SectionLayer_GravshipMask.OverrideMode = SectionLayer_GravshipMask.MaskOverrideMode.None;
			if (this.gravship.Engine.launchInfo.doNegativeOutcome)
			{
				DefDatabase<LandingOutcomeDef>.AllDefsListForReading.RandomElementByWeight((LandingOutcomeDef d) => d.weight).Worker.ApplyOutcome(this.gravship);
			}
			this.terrainCapture = null;
			this.gravship = null;
			Current.Game.Gravship = null;
			Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
			this.terrainCurtainIndoorMask = null;
			this.terrainCurtainShadowMask = null;
			this.map = null;
			this.moveDesignator = null;
			this.ResetCutscene();
			Find.Scenario.PostGravshipLanded(map);
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0002DD18 File Offset: 0x0002BF18
		private Building_GravEngine PlaceGravship(Gravship gravship, IntVec3 root, Map map)
		{
			List<Thing> gravshipThings;
			GravshipPlacementUtility.PlaceGravshipInMap(gravship, root, map, out gravshipThings);
			GravshipPlacementUtility.ApplyTemperatureVacuumFromBase(gravship, root, map);
			map.listerFilthInHomeArea.RebuildAll();
			map.resourceCounter.UpdateResourceCounts();
			map.wealthWatcher.ForceRecount(true);
			map.powerNetManager.UpdatePowerNetsAndConnections_First();
			GravshipPlacementUtility.PostSwapMap(gravship, gravshipThings);
			return gravship.Engine;
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0002DD74 File Offset: 0x0002BF74
		public override void WorldComponentOnGUI()
		{
			if (!this.LandingAreaConfirmationInProgress || Find.ScreenshotModeHandler.Active || !WorldRendererUtility.DrawingMap)
			{
				return;
			}
			float num = 430f;
			if (this.landingMap != null)
			{
				num = 640f;
			}
			Rect rect = new Rect((float)UI.screenWidth / 2f - num / 2f, (float)UI.screenHeight - 150f - 70f, num, 70f);
			Widgets.DrawWindowBackground(rect);
			Rect rect2 = new Rect(rect.xMin + 10f, rect.yMin + 10f, 200f, 50f);
			if (this.landingMap != null)
			{
				if (Widgets.ButtonText(rect2, "AbortLandGravship".Translate(), true, true, true, null))
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("AbortLandConfirmation".Translate(), new Action(this.AbortLanding), false, null, WindowLayer.Dialog));
				}
				TooltipHandler.TipRegion(rect2, "AbortLandGravshipDesc".Translate());
				rect2.x += 210f;
			}
			Designator_MoveGravship designator_MoveGravship = this.MoveDesignator();
			if (Widgets.ButtonText(rect2, "DesignatorMoveGravship".Translate(), true, true, true, null))
			{
				Find.DesignatorManager.Select(designator_MoveGravship);
			}
			TooltipHandler.TipRegion(rect2, "DesignatorMoveGravshipDesc".Translate());
			rect2.x += 210f;
			if (Widgets.ButtonText(rect2, "ConfirmLandGravship".Translate(), true, true, true, null))
			{
				if (this.landingMarker.Spawned && Find.DesignatorManager.SelectedDesignator != designator_MoveGravship)
				{
					this.landingMarker.BeginLanding(this);
					this.landingMarker = null;
					SoundDefOf.Gravship_Land.PlayOneShotOnCamera(null);
				}
				else
				{
					Messages.Message("GravshipLandingMarkerNotPlaced".Translate(), MessageTypeDefOf.RejectInput, true);
				}
			}
			TooltipHandler.TipRegion(rect2, "ConfirmLandGravshipDesc".Translate());
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0002DF7C File Offset: 0x0002C17C
		private void AbortLanding()
		{
			PlanetLayer layer = this.landingMap.Tile.Layer;
			int num = 1;
			PlanetTile destinationTile = PlanetTile.Invalid;
			while (!destinationTile.Valid && num < 50)
			{
				FastTileFinder.TileQueryParams query = new FastTileFinder.TileQueryParams(this.landingMap.Tile, 1f, (float)num, FastTileFinder.LandmarkMode.Any, false, Hilliness.Undefined, Hilliness.Undefined, true, true, true);
				destinationTile = layer.FastTileFinder.Query(query, null, null, default(FastTileFinder.TileQueryParams)).RandomElementWithFallback(PlanetTile.Invalid);
				num++;
			}
			if (!destinationTile.Valid)
			{
				Messages.Message("FailedToFindCrashSite".Translate(), MessageTypeDefOf.RejectInput, true);
				return;
			}
			this.landingMap = null;
			this.landingMarker.Destroy(DestroyMode.Vanish);
			this.landingMarker = null;
			Find.CurrentGravship.destinationTile = destinationTile;
			float quality = Find.CurrentGravship.Engine.launchInfo.quality;
			Find.CurrentGravship.Engine.launchInfo.doNegativeOutcome = Rand.Chance(GravshipUtility.NegativeLandingOutcomeFromQuality(quality) * 2f);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GravshipUtility.ArriveNewMap(Find.CurrentGravship);
			}, "GeneratingMap", false, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true, false, null);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0002E0BA File Offset: 0x0002C2BA
		public Designator_MoveGravship MoveDesignator()
		{
			if (this.moveDesignator == null)
			{
				this.moveDesignator = new Designator_MoveGravship(this.landingMap, this.landingMarker);
			}
			this.moveDesignator.marker = this.landingMarker;
			return this.moveDesignator;
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0002E0F4 File Offset: 0x0002C2F4
		public override void ExposeData()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			Scribe_Values.Look<bool>(ref WorldComponent_GravshipController.cutsceneInProgress, "cutsceneInProgress", false, false);
			Scribe_Values.Look<PlanetTile>(ref this.takeoffTile, "takeoffTile", PlanetTile.Invalid, false);
			Scribe_Values.Look<PlanetTile>(ref this.landingTile, "targetTile", PlanetTile.Invalid, false);
			Scribe_References.Look<Map>(ref this.map, "takeoffMap", false);
			Scribe_References.Look<Gravship>(ref this.gravship, "gravship", false);
			Scribe_References.Look<Map>(ref this.landingMap, "landingMap", false);
			Scribe_Values.Look<FloatRange>(ref this.zoomRange, "zoomRange", default(FloatRange), false);
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				GravshipLandingMarker gravshipLandingMarker = this.landingMarker;
				this.landingMarkerSpawned = (gravshipLandingMarker != null && gravshipLandingMarker.Spawned);
			}
			Scribe_Values.Look<bool>(ref this.landingMarkerSpawned, "landingMarkerSpawned", false, false);
			if (this.landingMarkerSpawned)
			{
				Scribe_References.Look<GravshipLandingMarker>(ref this.landingMarker, "landingMarker", false);
			}
			else
			{
				Scribe_Deep.Look<GravshipLandingMarker>(ref this.landingMarker, "landingMarker", Array.Empty<object>());
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.landingMarker != null && !this.landingMarkerSpawned)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					CameraJumper.TryJump(this.landingMap.Center, this.landingMap, CameraJumper.MovementMode.Cut);
				});
			}
		}

		// Token: 0x0400037A RID: 890
		private Gravship gravship;

		// Token: 0x0400037B RID: 891
		private Map map;

		// Token: 0x0400037C RID: 892
		private PlanetTile takeoffTile = PlanetTile.Invalid;

		// Token: 0x0400037D RID: 893
		private PlanetTile landingTile = PlanetTile.Invalid;

		// Token: 0x0400037E RID: 894
		private static bool cutsceneInProgress = false;

		// Token: 0x0400037F RID: 895
		private float timeLeft = -1000f;

		// Token: 0x04000380 RID: 896
		private bool isTakeoff;

		// Token: 0x04000381 RID: 897
		public Map landingMap;

		// Token: 0x04000382 RID: 898
		private GravshipLandingMarker landingMarker;

		// Token: 0x04000383 RID: 899
		private FloatRange zoomRange;

		// Token: 0x04000384 RID: 900
		private GravshipCapturer gravshipCapturer;

		// Token: 0x04000385 RID: 901
		private GravshipRenderer gravshipRenderer;

		// Token: 0x04000386 RID: 902
		private Capture terrainCapture;

		// Token: 0x04000387 RID: 903
		private CellRect terrainCurtainBounds;

		// Token: 0x04000388 RID: 904
		private bool forceDrawTerrainCurtain;

		// Token: 0x04000389 RID: 905
		private LayerSubMesh terrainCurtainIndoorMask;

		// Token: 0x0400038A RID: 906
		private LayerSubMesh terrainCurtainShadowMask;

		// Token: 0x0400038B RID: 907
		private GravshipAudio gravshipAudio = new GravshipAudio();

		// Token: 0x0400038C RID: 908
		private bool mapHasGravAnchor;

		// Token: 0x0400038D RID: 909
		private const float InitialTime = 10f;

		// Token: 0x0400038E RID: 910
		private const float CameraSizeLerpSpeed = 5f;

		// Token: 0x0400038F RID: 911
		private const float CameraInitialPanDuration = 1f;

		// Token: 0x04000390 RID: 912
		public const int PlantDestroyRadius = 2;

		// Token: 0x04000391 RID: 913
		private static readonly int LayerGravshipExclude = LayerMask.NameToLayer("GravshipExclude");

		// Token: 0x04000392 RID: 914
		private static readonly int ShaderPropertyBackgroundTex = Shader.PropertyToID("_BackgroundTex");

		// Token: 0x04000393 RID: 915
		private static readonly int ShaderPropertyBackgroundColor = Shader.PropertyToID("_BackgroundColor");

		// Token: 0x04000394 RID: 916
		private static readonly Material IndoorMaskGravship = MatLoader.LoadMat("Misc/IndoorMaskGravship", -1);

		// Token: 0x04000395 RID: 917
		private static readonly Material IndoorMaskTerrainCurtain = MatLoader.LoadMat("Misc/IndoorMaskTerrainCurtain", -1);

		// Token: 0x04000396 RID: 918
		private static readonly Material GravshipShadowMaskTerrainCurtain = MatLoader.LoadMat("Misc/GravshipMaskTerrainCurtain", -1);

		// Token: 0x04000397 RID: 919
		private static readonly Vector3[] CornerOffsetsPerRotation = new Vector3[]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, 1f),
			new Vector3(1f, 0f, 1f),
			new Vector3(1f, 0f, 0f)
		};

		// Token: 0x04000399 RID: 921
		private bool isPollutedLanding;

		// Token: 0x0400039A RID: 922
		private Designator_MoveGravship moveDesignator;

		// Token: 0x0400039B RID: 923
		private bool landingMarkerSpawned;
	}
}
