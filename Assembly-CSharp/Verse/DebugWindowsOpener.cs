using System;
using LudeonTK;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020005D5 RID: 1493
	public class DebugWindowsOpener
	{
		// Token: 0x06002F6E RID: 12142 RVA: 0x0012FB47 File Offset: 0x0012DD47
		public DebugWindowsOpener()
		{
			this.drawButtonsCached = new Action(this.DrawButtons);
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x0012FB6C File Offset: 0x0012DD6C
		public void DevToolStarterOnGUI()
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			Vector2 vector = new Vector2((float)UI.screenWidth * 0.5f - this.widgetRowFinalX * 0.5f, 3f);
			float height = 25f;
			Find.WindowStack.ImmediateWindow(1593759361, new Rect(vector.x, vector.y, this.widgetRowFinalX, height).Rounded(), WindowLayer.GameUI, this.drawButtonsCached, false, false, 0f, null, false);
			if (KeyBindingDefOf.Dev_ToggleDebugLog.KeyDownEvent)
			{
				this.ToggleLogWindow();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugActionsMenu.KeyDownEvent)
			{
				this.ToggleDebugActionsMenu();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugLogMenu.KeyDownEvent)
			{
				this.ToggleDebugLogMenu();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugSettingsMenu.KeyDownEvent)
			{
				this.ToggleDebugSettingsMenu();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDevPalette.KeyDownEvent && Current.ProgramState == ProgramState.Playing)
			{
				DebugSettings.devPalette = !DebugSettings.devPalette;
				this.TryOpenOrClosePalette();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugInspector.KeyDownEvent)
			{
				this.ToggleDebugInspector();
				Event.current.Use();
			}
			if (Current.ProgramState == ProgramState.Playing && KeyBindingDefOf.Dev_ToggleGodMode.KeyDownEvent)
			{
				this.ToggleGodMode();
				Event.current.Use();
			}
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x0012FCCC File Offset: 0x0012DECC
		private void DrawButtons()
		{
			this.widgetRow.Init(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			if (this.widgetRow.ButtonIcon(TexButton.ToggleLog, "Open the debug log.", null, null, null, true, -1f))
			{
				this.ToggleLogWindow();
			}
			if (this.widgetRow.ButtonIcon(TexButton.ToggleTweak, "Open tweakvalues menu.\n\nThis lets you change internal values.", null, null, null, true, -1f))
			{
				this.ToggleTweakValuesMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenDebugActionsMenu, "Open debug actions menu.\n\nThis lets you spawn items and force various events.", null, null, null, true, -1f))
			{
				this.ToggleDebugActionsMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenInspectSettings, "Open the view settings.\n\nThis lets you see special debug visuals.", null, null, null, true, -1f))
			{
				this.ToggleDebugSettingsMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenDebugActionsMenu, "Open debug logging menu.", null, null, null, true, -1f))
			{
				this.ToggleDebugLogMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenInspector, "Open the inspector.\n\nThis lets you inspect what's happening in the game, down to individual variables.", null, null, null, true, -1f))
			{
				this.ToggleDebugInspector();
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				bool devPalette = DebugSettings.devPalette;
				this.widgetRow.ToggleableIcon(ref devPalette, TexButton.ToggleDevPalette, "Toggle the dev palette.\n\nAllows you to setup a palette of debug actions for ease of use.", null, null);
				if (devPalette != DebugSettings.devPalette)
				{
					DebugSettings.devPalette = devPalette;
					this.TryOpenOrClosePalette();
				}
				if (this.widgetRow.ButtonIcon(DebugSettings.godMode ? TexButton.GodModeEnabled : TexButton.GodModeDisabled, "Toggle god mode.\n\nWhen god mode is on, you can build stuff instantly, for free, and sell things that aren't yours.", null, null, null, true, -1f))
				{
					this.ToggleGodMode();
				}
			}
			this.widgetRowFinalX = this.widgetRow.FinalX;
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x0012FF05 File Offset: 0x0012E105
		private void ToggleLogWindow()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_Log), true))
			{
				Find.WindowStack.Add(new EditWindow_Log());
			}
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x0012FF30 File Offset: 0x0012E130
		private void ToggleDebugSettingsMenu()
		{
			Dialog_Debug dialog_Debug = Find.WindowStack.WindowOfType<Dialog_Debug>();
			if (dialog_Debug == null)
			{
				Find.WindowStack.Add(new Dialog_Debug(DebugTabMenuDefOf.Settings));
				return;
			}
			dialog_Debug.SwitchTab(DebugTabMenuDefOf.Settings);
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x0012FF6C File Offset: 0x0012E16C
		private void ToggleDebugActionsMenu()
		{
			Dialog_Debug dialog_Debug = Find.WindowStack.WindowOfType<Dialog_Debug>();
			if (dialog_Debug == null)
			{
				Find.WindowStack.Add(new Dialog_Debug(DebugTabMenuDefOf.Actions));
				return;
			}
			dialog_Debug.SwitchTab(DebugTabMenuDefOf.Actions);
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x0012FFA7 File Offset: 0x0012E1A7
		private void ToggleTweakValuesMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_TweakValues), true))
			{
				Find.WindowStack.Add(new EditWindow_TweakValues());
			}
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x0012FFD0 File Offset: 0x0012E1D0
		private void ToggleDebugLogMenu()
		{
			Dialog_Debug dialog_Debug = Find.WindowStack.WindowOfType<Dialog_Debug>();
			if (dialog_Debug == null)
			{
				Find.WindowStack.Add(new Dialog_Debug(DebugTabMenuDefOf.Output));
				return;
			}
			dialog_Debug.SwitchTab(DebugTabMenuDefOf.Output);
		}

		// Token: 0x06002F76 RID: 12150 RVA: 0x0013000B File Offset: 0x0012E20B
		private void ToggleDebugInspector()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_DebugInspector), true))
			{
				Find.WindowStack.Add(new EditWindow_DebugInspector());
			}
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x00130033 File Offset: 0x0012E233
		private void ToggleGodMode()
		{
			DebugSettings.godMode = !DebugSettings.godMode;
			if (DebugSettings.godMode)
			{
				SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				return;
			}
			SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x00130060 File Offset: 0x0012E260
		public void TryOpenOrClosePalette()
		{
			if (DebugSettings.devPalette)
			{
				Find.WindowStack.Add(new Dialog_DevPalette());
				return;
			}
			Find.WindowStack.TryRemove(typeof(Dialog_DevPalette), true);
		}

		// Token: 0x04001E3A RID: 7738
		private Action drawButtonsCached;

		// Token: 0x04001E3B RID: 7739
		private WidgetRow widgetRow = new WidgetRow();

		// Token: 0x04001E3C RID: 7740
		private float widgetRowFinalX;
	}
}
