using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E25 RID: 7717
	public class WorldInterface
	{
		// Token: 0x17002509 RID: 9481
		// (get) Token: 0x0600C886 RID: 51334 RVA: 0x004989A0 File Offset: 0x00496BA0
		// (set) Token: 0x0600C887 RID: 51335 RVA: 0x004989AD File Offset: 0x00496BAD
		public PlanetTile SelectedTile
		{
			get
			{
				return this.selector.SelectedTile;
			}
			set
			{
				this.selector.SelectedTile = value;
			}
		}

		// Token: 0x0600C888 RID: 51336 RVA: 0x004989BC File Offset: 0x00496BBC
		public void Reset()
		{
			this.everReset = true;
			this.inspectPane.Reset();
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.SelectedTile = ((Find.CurrentMap != null) ? Find.CurrentMap.Tile : PlanetTile.Invalid);
			}
			else if (Find.GameInitData != null)
			{
				if (Find.GameInitData.startingTile.Valid && Find.World != null && !Find.WorldGrid.InBounds(Find.GameInitData.startingTile))
				{
					Log.Error("Map world tile was out of bounds.");
					Find.GameInitData.startingTile = PlanetTile.Invalid;
				}
				this.SelectedTile = Find.GameInitData.startingTile;
				this.inspectPane.OpenTabType = typeof(WITab_Terrain);
			}
			else
			{
				this.SelectedTile = PlanetTile.Invalid;
			}
			if (this.SelectedTile.Valid)
			{
				Find.WorldCameraDriver.JumpTo(this.SelectedTile);
			}
			else
			{
				Find.WorldCameraDriver.JumpTo(Find.WorldGrid.SurfaceViewCenter);
			}
			Find.WorldCameraDriver.ResetAltitude();
		}

		// Token: 0x0600C889 RID: 51337 RVA: 0x00498AC8 File Offset: 0x00496CC8
		public void WorldInterfaceUpdate()
		{
			if (WorldRendererUtility.WorldSelected)
			{
				this.targeter.TargeterUpdate();
				WorldSelectionDrawer.DrawSelectionOverlays();
				if (this.tilePicker.Active)
				{
					this.tilePicker.TileSelectorUpdate();
				}
				Find.WorldDebugDrawer.WorldDebugDrawerUpdate();
			}
			else
			{
				this.targeter.StopTargeting();
				this.tilePicker.StopTargeting();
			}
			this.routePlanner.WorldRoutePlannerUpdate();
			WorldGizmoUtility.WorldUIUpdate();
		}

		// Token: 0x0600C88A RID: 51338 RVA: 0x00498B38 File Offset: 0x00496D38
		public void WorldInterfaceOnGUI()
		{
			this.CheckOpenOrCloseInspectPane();
			if (WorldRendererUtility.WorldSelected && Find.WorldCamera.gameObject.activeInHierarchy)
			{
				ScreenshotModeHandler screenshotMode = Find.UIRoot.screenshotMode;
				ExpandableWorldObjectsUtility.ExpandableWorldObjectsOnGUI();
				if (ModsConfig.OdysseyActive)
				{
					ExpandableLandmarksUtility.ExpandableLandmarksOnGUI();
				}
				WorldSelectionDrawer.SelectionOverlaysOnGUI();
				this.routePlanner.WorldRoutePlannerOnGUI();
				if (!screenshotMode.FiltersCurrentEvent && Current.ProgramState == ProgramState.Playing)
				{
					Find.ColonistBar.ColonistBarOnGUI();
				}
				this.selector.dragBox.DragBoxOnGUI();
				this.targeter.TargeterOnGUI();
				if (this.tilePicker.Active)
				{
					this.tilePicker.TileSelectorOnGUI();
				}
				if (!screenshotMode.FiltersCurrentEvent)
				{
					this.globalControls.WorldGlobalControlsOnGUI();
				}
				WorldGizmoUtility.WorldUIOnGUI();
				Find.WorldDebugDrawer.WorldDebugDrawerOnGUI();
			}
		}

		// Token: 0x0600C88B RID: 51339 RVA: 0x00498C02 File Offset: 0x00496E02
		public void HandleLowPriorityInput()
		{
			if (WorldRendererUtility.WorldSelected)
			{
				this.targeter.ProcessInputEvents();
				this.selector.WorldSelectorOnGUI();
			}
		}

		// Token: 0x0600C88C RID: 51340 RVA: 0x00498C24 File Offset: 0x00496E24
		private void CheckOpenOrCloseInspectPane()
		{
			if (this.selector.AnyObjectOrTileSelected && WorldRendererUtility.WorldSelected && (Current.ProgramState != ProgramState.Playing || Find.MainTabsRoot.OpenTab == null))
			{
				if (!Find.WindowStack.IsOpen<WorldInspectPane>())
				{
					Find.WindowStack.Add(this.inspectPane);
					return;
				}
			}
			else if (Find.WindowStack.IsOpen<WorldInspectPane>())
			{
				Find.WindowStack.TryRemove(this.inspectPane, false);
			}
		}

		// Token: 0x0400722D RID: 29229
		public WorldSelector selector = new WorldSelector();

		// Token: 0x0400722E RID: 29230
		public WorldTargeter targeter = new WorldTargeter();

		// Token: 0x0400722F RID: 29231
		public WorldInspectPane inspectPane = new WorldInspectPane();

		// Token: 0x04007230 RID: 29232
		public WorldGlobalControls globalControls = new WorldGlobalControls();

		// Token: 0x04007231 RID: 29233
		public WorldRoutePlanner routePlanner = new WorldRoutePlanner();

		// Token: 0x04007232 RID: 29234
		public TilePicker tilePicker = new TilePicker();

		// Token: 0x04007233 RID: 29235
		public bool everReset;
	}
}
