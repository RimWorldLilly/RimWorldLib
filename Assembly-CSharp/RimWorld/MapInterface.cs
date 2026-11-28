using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001DA5 RID: 7589
	public class MapInterface
	{
		// Token: 0x0600C54A RID: 50506 RVA: 0x004880F4 File Offset: 0x004862F4
		public void MapInterfaceOnGUI_BeforeMainTabs()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (WorldRendererUtility.DrawingMap)
			{
				ScreenshotModeHandler screenshotMode = Find.UIRoot.screenshotMode;
				this.thingOverlays.ThingOverlaysOnGUI();
				Find.CurrentMap.MapOnGUI();
				MapComponentUtility.MapComponentOnGUI(Find.CurrentMap);
				BeautyDrawer.BeautyDrawerOnGUI();
				if (!screenshotMode.FiltersCurrentEvent)
				{
					this.colonistBar.ColonistBarOnGUI();
				}
				this.selector.dragBox.DragBoxOnGUI();
				this.designatorManager.DesignationManagerOnGUI();
				this.targeter.TargeterOnGUI();
				this.selector.SelectorOnGUI_BeforeMainTabs();
				Find.CurrentMap.tooltipGiverList.DispenseAllThingTooltips();
				Find.CurrentMap.flecks.FleckManagerOnGUI();
				if (DebugViewSettings.drawFoodSearchFromMouse)
				{
					FoodUtility.DebugFoodSearchFromMouse_OnGUI();
				}
				if (DebugViewSettings.drawNonCombatantTimer)
				{
					AttackTargetFinder.DebugDrawNonCombatantTimer_OnGUI();
				}
				if (DebugViewSettings.drawAttackTargetScores)
				{
					AttackTargetFinder.DebugDrawAttackTargetScores_OnGUI();
				}
				if (ModsConfig.OdysseyActive && GravshipUtility.ShowConnectedSubstructure)
				{
					using (new ProfilerBlock("DrawSubstructureCountOnGUI()"))
					{
						SubstructureGrid.DrawSubstructureCountOnGUI();
					}
				}
				if (!screenshotMode.FiltersCurrentEvent)
				{
					this.mouseoverReadout.MouseoverReadoutOnGUI();
					this.globalControls.GlobalControlsOnGUI();
					this.resourceReadout.ResourceReadoutOnGUI();
					MapGizmoUtility.MapUIOnGUI();
					return;
				}
			}
			else
			{
				this.targeter.StopTargeting();
			}
		}

		// Token: 0x0600C54B RID: 50507 RVA: 0x00488244 File Offset: 0x00486444
		public void MapInterfaceOnGUI_AfterMainTabs()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.DrawingMap)
			{
				return;
			}
			if (!Find.UIRoot.screenshotMode.FiltersCurrentEvent)
			{
				EnvironmentStatsDrawer.EnvironmentStatsOnGUI();
				Find.CurrentMap.deepResourceGrid.DeepResourcesOnGUI();
				Find.CurrentMap.debugDrawer.DebugDrawerOnGUI();
			}
		}

		// Token: 0x0600C54C RID: 50508 RVA: 0x00488295 File Offset: 0x00486495
		public void HandleMapClicks()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.DrawingMap)
			{
				return;
			}
			this.designatorManager.ProcessInputEvents();
			this.targeter.ProcessInputEvents();
		}

		// Token: 0x0600C54D RID: 50509 RVA: 0x004882BD File Offset: 0x004864BD
		public void HandleLowPriorityInput()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.DrawingMap)
			{
				return;
			}
			this.selector.SelectorOnGUI();
			Find.CurrentMap.lordManager.LordManagerOnGUI();
		}

		// Token: 0x0600C54E RID: 50510 RVA: 0x004882EC File Offset: 0x004864EC
		public void MapInterfaceUpdate()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.DrawingMap)
			{
				return;
			}
			this.targeter.TargeterUpdate();
			SelectionDrawer.DrawSelectionOverlays();
			EnvironmentStatsDrawer.DrawRoomOverlays();
			this.designatorManager.DesignatorManagerUpdate();
			this.selector.gotoController.Draw();
			Find.CurrentMap.roofGrid.RoofGridUpdate();
			Find.CurrentMap.fertilityGrid.FertilityGridUpdate();
			if (ModsConfig.BiotechActive)
			{
				Find.CurrentMap.pollutionGrid.PollutionGridUpdate();
			}
			using (new ProfilerBlock("MapComponentOnDraw()"))
			{
				MapComponentUtility.MapComponentOnDraw(Find.CurrentMap);
			}
			Find.CurrentMap.pathFinder.OnDraw();
			if (ModsConfig.OdysseyActive)
			{
				using (new ProfilerBlock("SubstructureGridUpdate()"))
				{
					SubstructureGrid substructureGrid = Find.CurrentMap.substructureGrid;
					if (substructureGrid != null)
					{
						substructureGrid.DrawSubstructureGrid();
					}
				}
				if (GravshipUtility.ShowConnectedSubstructure)
				{
					using (new ProfilerBlock("SubstructureGridUpdate()"))
					{
						SubstructureGrid.DrawSubstructureFootprint(null, null);
					}
				}
			}
			Find.CurrentMap.terrainGrid.TerrainGridUpdate();
			Find.CurrentMap.exitMapGrid.ExitMapGridUpdate();
			Find.CurrentMap.deepResourceGrid.DeepResourceGridUpdate();
			Find.CurrentMap.mapTemperature.TemperatureUpdate();
			MapGizmoUtility.MapUIUpdate();
			if (DebugViewSettings.drawPawnDebug)
			{
				Find.CurrentMap.pawnDestinationReservationManager.DebugDrawDestinations();
				Find.CurrentMap.reservationManager.DebugDrawReservations();
			}
			if (DebugViewSettings.drawDestReservations)
			{
				Find.CurrentMap.pawnDestinationReservationManager.DebugDrawReservations();
			}
			if (DebugViewSettings.drawFoodSearchFromMouse)
			{
				FoodUtility.DebugFoodSearchFromMouse_Update();
			}
			if (DebugViewSettings.drawPreyInfo)
			{
				FoodUtility.DebugDrawPredatorFoodSource();
			}
			if (DebugViewSettings.drawAttackTargetScores)
			{
				AttackTargetFinder.DebugDrawAttackTargetScores_Update();
			}
			if (DebugViewSettings.drawFOVSymmetry)
			{
				GenSight.DebugDrawFOVSymmetry_Update();
			}
			MiscDebugDrawer.DebugDrawInteractionCells();
			Find.CurrentMap.debugDrawer.DebugDrawerUpdate();
			Find.CurrentMap.regionGrid.DebugDraw();
			InfestationCellFinder.DebugDraw();
			LargeBuildingCellFinder.DebugDraw();
			StealAIDebugDrawer.DebugDraw();
			MapGenerator.DebugDraw();
			Find.CurrentMap.waterInfo.DebugDrawRiver();
			BuildingsDamageSectionLayerUtility.DebugDraw();
			WaterBodyTracker waterBodyTracker = Find.CurrentMap.waterBodyTracker;
			if (waterBodyTracker == null)
			{
				return;
			}
			waterBodyTracker.DebugDraw();
		}

		// Token: 0x0600C54F RID: 50511 RVA: 0x00488538 File Offset: 0x00486738
		public void Notify_SwitchedMap()
		{
			this.designatorManager.Deselect();
			this.reverseDesignatorDatabase.Reinit();
			this.selector.ClearSelection();
			this.selector.dragBox.active = false;
			this.selector.gotoController.Deactivate();
			this.targeter.StopTargeting();
			Designator_AreaAllowed.ClearSelectedArea();
			MainButtonDef openTab = Find.MainTabsRoot.OpenTab;
			List<MainButtonDef> allDefsListForReading = DefDatabase<MainButtonDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				allDefsListForReading[i].Notify_SwitchedMap();
			}
			if (openTab != null && openTab != MainButtonDefOf.Inspect)
			{
				Find.MainTabsRoot.SetCurrentTab(openTab, false);
			}
			if (Find.CurrentMap != null)
			{
				RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
				Find.CameraDriver.SetRootPosAndSize(rememberedCameraPos.rootPos, rememberedCameraPos.rootSize);
			}
		}

		// Token: 0x040070D0 RID: 28880
		public ThingOverlays thingOverlays = new ThingOverlays();

		// Token: 0x040070D1 RID: 28881
		public Selector selector = new Selector();

		// Token: 0x040070D2 RID: 28882
		public Targeter targeter = new Targeter();

		// Token: 0x040070D3 RID: 28883
		public DesignatorManager designatorManager = new DesignatorManager();

		// Token: 0x040070D4 RID: 28884
		public ReverseDesignatorDatabase reverseDesignatorDatabase = new ReverseDesignatorDatabase();

		// Token: 0x040070D5 RID: 28885
		private MouseoverReadout mouseoverReadout = new MouseoverReadout();

		// Token: 0x040070D6 RID: 28886
		public GlobalControls globalControls = new GlobalControls();

		// Token: 0x040070D7 RID: 28887
		protected ResourceReadout resourceReadout = new ResourceReadout();

		// Token: 0x040070D8 RID: 28888
		public ColonistBar colonistBar = new ColonistBar();
	}
}
