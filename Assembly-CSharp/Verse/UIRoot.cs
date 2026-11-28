using System;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Noise;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000643 RID: 1603
	public abstract class UIRoot
	{
		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x0600323B RID: 12859 RVA: 0x001402E8 File Offset: 0x0013E4E8
		public bool HideMotes
		{
			get
			{
				return WorldComponent_GravshipController.CutsceneInProgress;
			}
		}

		// Token: 0x0600323C RID: 12860 RVA: 0x001402F4 File Offset: 0x0013E4F4
		public virtual void Init()
		{
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x001402F8 File Offset: 0x0013E4F8
		public virtual void UIRootOnGUI()
		{
			DebugInputLogger.InputLogOnGUI();
			UnityGUIBugsFixer.OnGUI();
			SteamDeck.OnGUI();
			SteamDeck.RootOnGUI();
			OriginalEventUtility.RecordOriginalEvent(Event.current);
			Text.StartOfOnGUI();
			this.CheckOpenLogWindow();
			DelayedErrorWindowRequest.DelayedErrorWindowRequestOnGUI();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				this.debugWindowOpener.DevToolStarterOnGUI();
			}
			this.windows.HandleEventsHighPriority();
			this.screenshotMode.ScreenshotModesOnGUI();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				TooltipHandler.DoTooltipGUI();
				this.feedbackFloaters.FeedbackOnGUI();
				DragSliderManager.DragSlidersOnGUI();
				Messages.MessagesDoGUI();
			}
			this.shortcutKeys.ShortcutKeysOnGUI();
			NoiseDebugUI.NoiseDebugOnGUI();
			Debug.developerConsoleVisible = false;
			if (Current.Game != null)
			{
				GameComponentUtility.GameComponentOnGUI();
				CellInspectorDrawer.OnGUI();
			}
			World world = Find.World;
			if (world != null)
			{
				world.WorldOnGUI();
			}
			OriginalEventUtility.Reset();
		}

		// Token: 0x0600323E RID: 12862 RVA: 0x001403C4 File Offset: 0x0013E5C4
		public virtual void UIRootUpdate()
		{
			ScreenshotTaker.Update();
			DragSliderManager.DragSlidersUpdate();
			this.windows.WindowsUpdate();
			MouseoverSounds.ResolveFrame();
			UIHighlighter.UIHighlighterUpdate();
			Messages.Update();
			CellInspectorDrawer.Update();
		}

		// Token: 0x0600323F RID: 12863 RVA: 0x001403EF File Offset: 0x0013E5EF
		private void CheckOpenLogWindow()
		{
			if (EditWindow_Log.wantsToOpen && !Find.WindowStack.IsOpen(typeof(EditWindow_Log)))
			{
				Find.WindowStack.Add(new EditWindow_Log());
				EditWindow_Log.wantsToOpen = false;
			}
		}

		// Token: 0x0400201F RID: 8223
		public WindowStack windows = new WindowStack();

		// Token: 0x04002020 RID: 8224
		public DebugWindowsOpener debugWindowOpener = new DebugWindowsOpener();

		// Token: 0x04002021 RID: 8225
		public ScreenshotModeHandler screenshotMode = new ScreenshotModeHandler();

		// Token: 0x04002022 RID: 8226
		private ShortcutKeys shortcutKeys = new ShortcutKeys();

		// Token: 0x04002023 RID: 8227
		public FeedbackFloaters feedbackFloaters = new FeedbackFloaters();
	}
}
