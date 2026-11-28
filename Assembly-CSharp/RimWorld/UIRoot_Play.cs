using System;
using LudeonTK;
using UnityEngine;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001E22 RID: 7714
	public class UIRoot_Play : UIRoot
	{
		// Token: 0x0600C86F RID: 51311 RVA: 0x0049795C File Offset: 0x00495B5C
		public override void Init()
		{
			base.Init();
			Messages.Clear();
			this.debugWindowOpener.TryOpenOrClosePalette();
		}

		// Token: 0x0600C870 RID: 51312 RVA: 0x00497974 File Offset: 0x00495B74
		public override void UIRootOnGUI()
		{
			base.UIRootOnGUI();
			Find.GameInfo.GameInfoOnGUI();
			Find.World.UI.WorldInterfaceOnGUI();
			this.mapUI.MapInterfaceOnGUI_BeforeMainTabs();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				this.mainButtonsRoot.MainButtonsOnGUI();
				this.alerts.AlertsReadoutOnGUI();
			}
			this.mapUI.MapInterfaceOnGUI_AfterMainTabs();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				Find.Tutor.TutorOnGUI();
			}
			Find.World.WorldOnGUI();
			ReorderableWidget.ReorderableWidgetOnGUI_BeforeWindowStack();
			DragAndDropWidget.DragAndDropWidgetOnGUI_BeforeWindowStack();
			this.windows.WindowStackOnGUI();
			DragAndDropWidget.DragAndDropWidgetOnGUI_AfterWindowStack();
			ReorderableWidget.ReorderableWidgetOnGUI_AfterWindowStack();
			Widgets.WidgetsOnGUI();
			this.mapUI.HandleMapClicks();
			if (Find.DesignatorManager.SelectedDesignator != null)
			{
				Find.DesignatorManager.SelectedDesignator.SelectedProcessInput(Event.current);
			}
			DebugTools.DebugToolsOnGUI();
			this.mainButtonsRoot.HandleLowPriorityShortcuts();
			Find.World.UI.HandleLowPriorityInput();
			this.mapUI.HandleLowPriorityInput();
			this.OpenMainMenuShortcut();
			SteamDeck.ShowSteamDeckGameControlsIfNotKnown();
		}

		// Token: 0x0600C871 RID: 51313 RVA: 0x00497A80 File Offset: 0x00495C80
		public override void UIRootUpdate()
		{
			base.UIRootUpdate();
			try
			{
				Find.World.UI.WorldInterfaceUpdate();
				this.mapUI.MapInterfaceUpdate();
				this.alerts.AlertsReadoutUpdate();
				LessonAutoActivator.LessonAutoActivatorUpdate();
				Find.Tutor.TutorUpdate();
			}
			catch (Exception ex)
			{
				Log.Error("Exception in UIRootUpdate: " + ex.ToString());
			}
		}

		// Token: 0x0600C872 RID: 51314 RVA: 0x00497AF4 File Offset: 0x00495CF4
		private void OpenMainMenuShortcut()
		{
			if ((Event.current.type == EventType.keyDown && Event.current.keyCode == KeyCode.Escape) || KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				Event.current.Use();
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Menu, true);
			}
		}

		// Token: 0x04007209 RID: 29193
		public MapInterface mapUI = new MapInterface();

		// Token: 0x0400720A RID: 29194
		public MainButtonsRoot mainButtonsRoot = new MainButtonsRoot();

		// Token: 0x0400720B RID: 29195
		public AlertsReadout alerts = new AlertsReadout();
	}
}
