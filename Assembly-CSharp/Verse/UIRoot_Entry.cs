using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x020005F9 RID: 1529
	public class UIRoot_Entry : UIRoot
	{
		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06003003 RID: 12291 RVA: 0x00133724 File Offset: 0x00131924
		private bool ShouldDoMainMenu
		{
			get
			{
				if (LongEventHandler.AnyEventNowOrWaiting)
				{
					return false;
				}
				for (int i = 0; i < Find.WindowStack.Count; i++)
				{
					if (this.windows[i].layer == WindowLayer.Dialog && !Find.WindowStack[i].IsDebug)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06003004 RID: 12292 RVA: 0x00133778 File Offset: 0x00131978
		public override void Init()
		{
			base.Init();
			UIMenuBackgroundManager.background = new UI_BackgroundMain();
			MainMenuDrawer.Init();
			QuickStarter.CheckQuickStart();
			VersionUpdateDialogMaker.CreateVersionUpdateDialogIfNecessary();
			if (!SteamManager.Initialized)
			{
				Dialog_MessageBox window = new Dialog_MessageBox("SteamClientMissing".Translate(), "Quit".Translate(), delegate()
				{
					Application.Quit();
				}, "Ignore".Translate(), null, null, false, null, null, WindowLayer.Dialog);
				Find.WindowStack.Add(window);
			}
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x00133814 File Offset: 0x00131A14
		public override void UIRootOnGUI()
		{
			base.UIRootOnGUI();
			if (Find.World != null)
			{
				Find.World.UI.WorldInterfaceOnGUI();
			}
			this.DoMainMenu();
			if (Current.Game != null)
			{
				Find.Tutor.TutorOnGUI();
			}
			ReorderableWidget.ReorderableWidgetOnGUI_BeforeWindowStack();
			DragAndDropWidget.DragAndDropWidgetOnGUI_BeforeWindowStack();
			this.windows.WindowStackOnGUI();
			DragAndDropWidget.DragAndDropWidgetOnGUI_AfterWindowStack();
			ReorderableWidget.ReorderableWidgetOnGUI_AfterWindowStack();
			Widgets.WidgetsOnGUI();
			if (Find.World != null)
			{
				Find.World.UI.HandleLowPriorityInput();
			}
		}

		// Token: 0x06003006 RID: 12294 RVA: 0x0013388E File Offset: 0x00131A8E
		public override void UIRootUpdate()
		{
			base.UIRootUpdate();
			if (Find.World != null)
			{
				Find.World.UI.WorldInterfaceUpdate();
			}
			if (Current.Game != null)
			{
				LessonAutoActivator.LessonAutoActivatorUpdate();
				Find.Tutor.TutorUpdate();
			}
		}

		// Token: 0x06003007 RID: 12295 RVA: 0x001338C2 File Offset: 0x00131AC2
		private void DoMainMenu()
		{
			if (!WorldRendererUtility.WorldSelected)
			{
				UIMenuBackgroundManager.background.BackgroundOnGUI();
				if (this.ShouldDoMainMenu)
				{
					Current.Game = null;
					MainMenuDrawer.MainMenuOnGUI();
				}
			}
		}
	}
}
