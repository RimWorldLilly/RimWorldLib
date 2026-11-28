using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Waylith;

namespace Verse
{
	// Token: 0x02000721 RID: 1825
	public class PrefsData
	{
		// Token: 0x060039BC RID: 14780 RVA: 0x0016FCC4 File Offset: 0x0016DEC4
		public void Apply()
		{
			if (!UnityData.IsInMainThread)
			{
				return;
			}
			if (this.customCursorEnabled)
			{
				CustomCursor.Activate();
			}
			else
			{
				CustomCursor.Deactivate();
			}
			AudioListener.volume = this.volumeMaster;
			Application.runInBackground = this.runInBackground;
			EngineWindow.SetPreferredMonitor(this.preferredMonitorId.NullOrEmpty() ? null : this.preferredMonitorId);
			if (this.screenWidth == 0 || this.screenHeight == 0)
			{
				ResolutionUtility.SetNativeResolutionRaw();
				return;
			}
			if (Prefs.FullScreenMode == FullScreenMode.Windowed)
			{
				if (!Application.isBatchMode)
				{
					Screen.fullScreenMode = FullScreenMode.Windowed;
					EngineWindow.SetResolution(this.screenWidth, this.screenHeight);
					return;
				}
			}
			else
			{
				ResolutionUtility.SetResolutionRaw(this.screenWidth, this.screenHeight, Prefs.FullScreenMode);
			}
		}

		// Token: 0x040025C4 RID: 9668
		public const int UnsetWindowPosition = -2147483648;

		// Token: 0x040025C5 RID: 9669
		public float volumeMaster = 0.8f;

		// Token: 0x040025C6 RID: 9670
		public float volumeGame = 1f;

		// Token: 0x040025C7 RID: 9671
		public float volumeMusic = 0.4f;

		// Token: 0x040025C8 RID: 9672
		public float volumeAmbient = 1f;

		// Token: 0x040025C9 RID: 9673
		public float volumeUI = 1f;

		// Token: 0x040025CA RID: 9674
		public int screenWidth;

		// Token: 0x040025CB RID: 9675
		public int screenHeight;

		// Token: 0x040025CC RID: 9676
		public int windowPositionX = int.MinValue;

		// Token: 0x040025CD RID: 9677
		public int windowPositionY = int.MinValue;

		// Token: 0x040025CE RID: 9678
		public bool fullscreen;

		// Token: 0x040025CF RID: 9679
		[DefaultValue(false)]
		public bool exclusiveFullscreen;

		// Token: 0x040025D0 RID: 9680
		[DefaultValue("")]
		public string preferredMonitorId = "";

		// Token: 0x040025D1 RID: 9681
		[DefaultValue("")]
		public string preferredGpuName = "";

		// Token: 0x040025D2 RID: 9682
		[DefaultValue("")]
		public string preferredGpuUuid = "";

		// Token: 0x040025D3 RID: 9683
		public bool scaleRelativeToSystem;

		// Token: 0x040025D4 RID: 9684
		public bool windowedSizeLogical;

		// Token: 0x040025D5 RID: 9685
		public float uiScale = 1f;

		// Token: 0x040025D6 RID: 9686
		[DefaultValue(false)]
		public bool ignoreSystemScaling;

		// Token: 0x040025D7 RID: 9687
		public bool customCursorEnabled = true;

		// Token: 0x040025D8 RID: 9688
		public bool hatsOnlyOnMap;

		// Token: 0x040025D9 RID: 9689
		public bool plantWindSway = true;

		// Token: 0x040025DA RID: 9690
		public float screenShakeIntensity = 1f;

		// Token: 0x040025DB RID: 9691
		public bool textureCompression = true;

		// Token: 0x040025DC RID: 9692
		public bool showRealtimeClock;

		// Token: 0x040025DD RID: 9693
		public bool disableTinyText;

		// Token: 0x040025DE RID: 9694
		public AnimalNameDisplayMode animalNameMode;

		// Token: 0x040025DF RID: 9695
		public MechNameDisplayMode mechNameMode = MechNameDisplayMode.WhileDrafted;

		// Token: 0x040025E0 RID: 9696
		public string backgroundExpansionId;

		// Token: 0x040025E1 RID: 9697
		public bool randomBackground;

		// Token: 0x040025E2 RID: 9698
		public ShowWeaponsUnderPortraitMode showWeaponsUnderPortraitMode = ShowWeaponsUnderPortraitMode.WhileDrafted;

		// Token: 0x040025E3 RID: 9699
		public DotHighlightDisplayMode dotHighlightDisplayMode = DotHighlightDisplayMode.HighlightAll;

		// Token: 0x040025E4 RID: 9700
		public HighlightStyleMode highlightStyleMode = HighlightStyleMode.Silhouettes;

		// Token: 0x040025E5 RID: 9701
		public bool visibleMood = true;

		// Token: 0x040025E6 RID: 9702
		public bool twelveHourClock;

		// Token: 0x040025E7 RID: 9703
		public bool adaptiveTrainingEnabled = true;

		// Token: 0x040025E8 RID: 9704
		public bool steamDeckKeyboardMode;

		// Token: 0x040025E9 RID: 9705
		public List<string> preferredNames = new List<string>();

		// Token: 0x040025EA RID: 9706
		public bool resourceReadoutCategorized;

		// Token: 0x040025EB RID: 9707
		public bool runInBackground;

		// Token: 0x040025EC RID: 9708
		public bool edgeScreenScroll = true;

		// Token: 0x040025ED RID: 9709
		public bool rememberDrawStyles;

		// Token: 0x040025EE RID: 9710
		public bool zoomSwitchWorldLayer = true;

		// Token: 0x040025EF RID: 9711
		public bool zoomToMouse;

		// Token: 0x040025F0 RID: 9712
		public TemperatureDisplayMode temperatureMode;

		// Token: 0x040025F1 RID: 9713
		public float autosaveIntervalDays = 1f;

		// Token: 0x040025F2 RID: 9714
		public bool testMapSizes;

		// Token: 0x040025F3 RID: 9715
		[LoadAlias("maxNumberOfPlayerHomes")]
		public int maxNumberOfPlayerSettlements = 1;

		// Token: 0x040025F4 RID: 9716
		public bool pauseOnLoad;

		// Token: 0x040025F5 RID: 9717
		public AutomaticPauseMode automaticPauseMode = AutomaticPauseMode.MajorThreat;

		// Token: 0x040025F6 RID: 9718
		public float mapDragSensitivity = 1.3f;

		// Token: 0x040025F7 RID: 9719
		public bool smoothCameraJumps = true;

		// Token: 0x040025F8 RID: 9720
		public bool gravshipCutscenes = true;

		// Token: 0x040025F9 RID: 9721
		public int autosavesCount = 5;

		// Token: 0x040025FA RID: 9722
		[Unsaved(true)]
		public bool? pauseOnUrgentLetter;

		// Token: 0x040025FB RID: 9723
		[Obsolete]
		public bool extremeDifficultyUnlocked;

		// Token: 0x040025FC RID: 9724
		[Obsolete]
		public bool pauseOnError;

		// Token: 0x040025FD RID: 9725
		[Unsaved(true)]
		public int preferredMonitor;

		// Token: 0x040025FE RID: 9726
		public bool devMode;

		// Token: 0x040025FF RID: 9727
		public List<string> debugActionPalette = new List<string>();

		// Token: 0x04002600 RID: 9728
		public Vector2 devPalettePosition;

		// Token: 0x04002601 RID: 9729
		public string langFolderName = "unknown";

		// Token: 0x04002602 RID: 9730
		public bool logVerbose;

		// Token: 0x04002603 RID: 9731
		public bool openLogOnWarnings;

		// Token: 0x04002604 RID: 9732
		public bool closeLogWindowOnEscape = true;

		// Token: 0x04002605 RID: 9733
		public bool disableQuickStartCryptoSickness = true;

		// Token: 0x04002606 RID: 9734
		public bool quickStartDevPaletteOn;

		// Token: 0x04002607 RID: 9735
		public bool resetModsConfigOnCrash = true;

		// Token: 0x04002608 RID: 9736
		[Obsolete]
		public bool simulateNotOwningRoyalty;

		// Token: 0x04002609 RID: 9737
		[Obsolete]
		public bool simulateNotOwningIdeology;

		// Token: 0x0400260A RID: 9738
		[Obsolete]
		public bool simulateNotOwningBiotech;

		// Token: 0x0400260B RID: 9739
		[Obsolete]
		public bool simulateNotOwningAnomaly;
	}
}
