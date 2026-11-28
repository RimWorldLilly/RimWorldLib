using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using LudeonTK;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000713 RID: 1811
	public static class Prefs
	{
		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x0600392F RID: 14639 RVA: 0x0016EDFB File Offset: 0x0016CFFB
		// (set) Token: 0x06003930 RID: 14640 RVA: 0x0016EE07 File Offset: 0x0016D007
		public static float VolumeMaster
		{
			get
			{
				return Prefs.data.volumeMaster;
			}
			set
			{
				if (Prefs.data.volumeMaster == value)
				{
					return;
				}
				Prefs.data.volumeMaster = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06003931 RID: 14641 RVA: 0x0016EE27 File Offset: 0x0016D027
		// (set) Token: 0x06003932 RID: 14642 RVA: 0x0016EE33 File Offset: 0x0016D033
		public static float VolumeGame
		{
			get
			{
				return Prefs.data.volumeGame;
			}
			set
			{
				if (Prefs.data.volumeGame == value)
				{
					return;
				}
				Prefs.data.volumeGame = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x06003933 RID: 14643 RVA: 0x0016EE53 File Offset: 0x0016D053
		// (set) Token: 0x06003934 RID: 14644 RVA: 0x0016EE5F File Offset: 0x0016D05F
		public static float VolumeMusic
		{
			get
			{
				return Prefs.data.volumeMusic;
			}
			set
			{
				if (Prefs.data.volumeMusic == value)
				{
					return;
				}
				Prefs.data.volumeMusic = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06003935 RID: 14645 RVA: 0x0016EE7F File Offset: 0x0016D07F
		// (set) Token: 0x06003936 RID: 14646 RVA: 0x0016EE8B File Offset: 0x0016D08B
		public static float VolumeAmbient
		{
			get
			{
				return Prefs.data.volumeAmbient;
			}
			set
			{
				if (Prefs.data.volumeAmbient == value)
				{
					return;
				}
				Prefs.data.volumeAmbient = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06003937 RID: 14647 RVA: 0x0016EEAB File Offset: 0x0016D0AB
		// (set) Token: 0x06003938 RID: 14648 RVA: 0x0016EEB7 File Offset: 0x0016D0B7
		public static float VolumeUI
		{
			get
			{
				return Prefs.data.volumeUI;
			}
			set
			{
				if (Prefs.data.volumeUI == value)
				{
					return;
				}
				Prefs.data.volumeUI = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x06003939 RID: 14649 RVA: 0x0016EED7 File Offset: 0x0016D0D7
		// (set) Token: 0x0600393A RID: 14650 RVA: 0x0016EEE3 File Offset: 0x0016D0E3
		public static bool AdaptiveTrainingEnabled
		{
			get
			{
				return Prefs.data.adaptiveTrainingEnabled;
			}
			set
			{
				if (Prefs.data.adaptiveTrainingEnabled == value)
				{
					return;
				}
				Prefs.data.adaptiveTrainingEnabled = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x0600393B RID: 14651 RVA: 0x0016EF03 File Offset: 0x0016D103
		// (set) Token: 0x0600393C RID: 14652 RVA: 0x0016EF0F File Offset: 0x0016D10F
		public static bool SteamDeckKeyboardMode
		{
			get
			{
				return Prefs.data.steamDeckKeyboardMode;
			}
			set
			{
				if (Prefs.data.steamDeckKeyboardMode == value)
				{
					return;
				}
				Prefs.data.steamDeckKeyboardMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x0600393D RID: 14653 RVA: 0x0016EF2F File Offset: 0x0016D12F
		// (set) Token: 0x0600393E RID: 14654 RVA: 0x0016EF3B File Offset: 0x0016D13B
		public static bool EdgeScreenScroll
		{
			get
			{
				return Prefs.data.edgeScreenScroll;
			}
			set
			{
				if (Prefs.data.edgeScreenScroll == value)
				{
					return;
				}
				Prefs.data.edgeScreenScroll = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x0600393F RID: 14655 RVA: 0x0016EF5B File Offset: 0x0016D15B
		// (set) Token: 0x06003940 RID: 14656 RVA: 0x0016EF67 File Offset: 0x0016D167
		public static bool RememberDrawStlyes
		{
			get
			{
				return Prefs.data.rememberDrawStyles;
			}
			set
			{
				if (Prefs.data.rememberDrawStyles == value)
				{
					return;
				}
				Prefs.data.rememberDrawStyles = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06003941 RID: 14657 RVA: 0x0016EF87 File Offset: 0x0016D187
		// (set) Token: 0x06003942 RID: 14658 RVA: 0x0016EF93 File Offset: 0x0016D193
		public static bool ZoomSwitchWorldLayer
		{
			get
			{
				return Prefs.data.zoomSwitchWorldLayer;
			}
			set
			{
				if (Prefs.data.zoomSwitchWorldLayer == value)
				{
					return;
				}
				Prefs.data.zoomSwitchWorldLayer = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06003943 RID: 14659 RVA: 0x0016EFB3 File Offset: 0x0016D1B3
		// (set) Token: 0x06003944 RID: 14660 RVA: 0x0016EFBF File Offset: 0x0016D1BF
		public static bool ZoomToMouse
		{
			get
			{
				return Prefs.data.zoomToMouse;
			}
			set
			{
				if (Prefs.data.zoomToMouse == value)
				{
					return;
				}
				Prefs.data.zoomToMouse = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06003945 RID: 14661 RVA: 0x0016EFDF File Offset: 0x0016D1DF
		// (set) Token: 0x06003946 RID: 14662 RVA: 0x0016EFEB File Offset: 0x0016D1EB
		public static float ScreenShakeIntensity
		{
			get
			{
				return Prefs.data.screenShakeIntensity;
			}
			set
			{
				if (Prefs.data.screenShakeIntensity == value)
				{
					return;
				}
				Prefs.data.screenShakeIntensity = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06003947 RID: 14663 RVA: 0x0016F00B File Offset: 0x0016D20B
		// (set) Token: 0x06003948 RID: 14664 RVA: 0x0016F017 File Offset: 0x0016D217
		public static bool RunInBackground
		{
			get
			{
				return Prefs.data.runInBackground;
			}
			set
			{
				if (Prefs.data.runInBackground == value)
				{
					return;
				}
				Prefs.data.runInBackground = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06003949 RID: 14665 RVA: 0x0016F037 File Offset: 0x0016D237
		// (set) Token: 0x0600394A RID: 14666 RVA: 0x0016F043 File Offset: 0x0016D243
		public static TemperatureDisplayMode TemperatureMode
		{
			get
			{
				return Prefs.data.temperatureMode;
			}
			set
			{
				if (Prefs.data.temperatureMode == value)
				{
					return;
				}
				Prefs.data.temperatureMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x0600394B RID: 14667 RVA: 0x0016F063 File Offset: 0x0016D263
		// (set) Token: 0x0600394C RID: 14668 RVA: 0x0016F06F File Offset: 0x0016D26F
		public static float AutosaveIntervalDays
		{
			get
			{
				return Prefs.data.autosaveIntervalDays;
			}
			set
			{
				if (Prefs.data.autosaveIntervalDays == value)
				{
					return;
				}
				Prefs.data.autosaveIntervalDays = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x0600394D RID: 14669 RVA: 0x0016F08F File Offset: 0x0016D28F
		// (set) Token: 0x0600394E RID: 14670 RVA: 0x0016F09B File Offset: 0x0016D29B
		public static bool CustomCursorEnabled
		{
			get
			{
				return Prefs.data.customCursorEnabled;
			}
			set
			{
				if (Prefs.data.customCursorEnabled == value)
				{
					return;
				}
				Prefs.data.customCursorEnabled = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x0600394F RID: 14671 RVA: 0x0016F0BB File Offset: 0x0016D2BB
		// (set) Token: 0x06003950 RID: 14672 RVA: 0x0016F0C7 File Offset: 0x0016D2C7
		public static DotHighlightDisplayMode DotHighlightDisplayMode
		{
			get
			{
				return Prefs.data.dotHighlightDisplayMode;
			}
			set
			{
				if (Prefs.data.dotHighlightDisplayMode == value)
				{
					return;
				}
				Prefs.data.dotHighlightDisplayMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06003951 RID: 14673 RVA: 0x0016F0E7 File Offset: 0x0016D2E7
		// (set) Token: 0x06003952 RID: 14674 RVA: 0x0016F0F3 File Offset: 0x0016D2F3
		public static HighlightStyleMode HighlightStyleMode
		{
			get
			{
				return Prefs.data.highlightStyleMode;
			}
			set
			{
				if (Prefs.data.highlightStyleMode == value)
				{
					return;
				}
				Prefs.data.highlightStyleMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06003953 RID: 14675 RVA: 0x0016F113 File Offset: 0x0016D313
		// (set) Token: 0x06003954 RID: 14676 RVA: 0x0016F11F File Offset: 0x0016D31F
		public static AnimalNameDisplayMode AnimalNameMode
		{
			get
			{
				return Prefs.data.animalNameMode;
			}
			set
			{
				if (Prefs.data.animalNameMode == value)
				{
					return;
				}
				Prefs.data.animalNameMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06003955 RID: 14677 RVA: 0x0016F13F File Offset: 0x0016D33F
		// (set) Token: 0x06003956 RID: 14678 RVA: 0x0016F14B File Offset: 0x0016D34B
		public static MechNameDisplayMode MechNameMode
		{
			get
			{
				return Prefs.data.mechNameMode;
			}
			set
			{
				if (Prefs.data.mechNameMode == value)
				{
					return;
				}
				Prefs.data.mechNameMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x06003957 RID: 14679 RVA: 0x0016F16B File Offset: 0x0016D36B
		// (set) Token: 0x06003958 RID: 14680 RVA: 0x0016F177 File Offset: 0x0016D377
		public static ShowWeaponsUnderPortraitMode ShowWeaponsUnderPortraitMode
		{
			get
			{
				return Prefs.data.showWeaponsUnderPortraitMode;
			}
			set
			{
				if (Prefs.data.showWeaponsUnderPortraitMode == value)
				{
					return;
				}
				Prefs.data.showWeaponsUnderPortraitMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06003959 RID: 14681 RVA: 0x0016F197 File Offset: 0x0016D397
		// (set) Token: 0x0600395A RID: 14682 RVA: 0x0016F1A3 File Offset: 0x0016D3A3
		public static bool VisibleMood
		{
			get
			{
				return Prefs.data.visibleMood;
			}
			set
			{
				if (Prefs.data.visibleMood == value)
				{
					return;
				}
				Prefs.data.visibleMood = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x0600395B RID: 14683 RVA: 0x0016F1C3 File Offset: 0x0016D3C3
		// (set) Token: 0x0600395C RID: 14684 RVA: 0x0016F1D8 File Offset: 0x0016D3D8
		public static bool DevMode
		{
			get
			{
				return Prefs.data == null || Prefs.data.devMode;
			}
			set
			{
				if (Prefs.data.devMode == value)
				{
					return;
				}
				Prefs.data.devMode = value;
				if (!Prefs.data.devMode)
				{
					Prefs.data.logVerbose = false;
					Prefs.data.resetModsConfigOnCrash = true;
					DebugSettings.godMode = false;
					Find.WindowStack.TryRemove(typeof(Dialog_DevPalette), true);
				}
				Prefs.Apply();
			}
		}

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x0600395D RID: 14685 RVA: 0x0016F241 File Offset: 0x0016D441
		// (set) Token: 0x0600395E RID: 14686 RVA: 0x0016F256 File Offset: 0x0016D456
		public static bool ResetModsConfigOnCrash
		{
			get
			{
				return Prefs.data == null || Prefs.data.resetModsConfigOnCrash;
			}
			set
			{
				if (Prefs.data.resetModsConfigOnCrash == value)
				{
					return;
				}
				Prefs.data.resetModsConfigOnCrash = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x0600395F RID: 14687 RVA: 0x0016F276 File Offset: 0x0016D476
		// (set) Token: 0x06003960 RID: 14688 RVA: 0x0016F282 File Offset: 0x0016D482
		public static List<string> PreferredNames
		{
			get
			{
				return Prefs.data.preferredNames;
			}
			set
			{
				if (Prefs.data.preferredNames == value)
				{
					return;
				}
				Prefs.data.preferredNames = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06003961 RID: 14689 RVA: 0x0016F2A2 File Offset: 0x0016D4A2
		// (set) Token: 0x06003962 RID: 14690 RVA: 0x0016F2AE File Offset: 0x0016D4AE
		public static string LangFolderName
		{
			get
			{
				return Prefs.data.langFolderName;
			}
			set
			{
				if (Prefs.data.langFolderName == value)
				{
					return;
				}
				Prefs.data.langFolderName = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06003963 RID: 14691 RVA: 0x0016F2D3 File Offset: 0x0016D4D3
		// (set) Token: 0x06003964 RID: 14692 RVA: 0x0016F2DF File Offset: 0x0016D4DF
		public static bool LogVerbose
		{
			get
			{
				return Prefs.data.logVerbose;
			}
			set
			{
				if (Prefs.data.logVerbose == value)
				{
					return;
				}
				Prefs.data.logVerbose = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06003965 RID: 14693 RVA: 0x0016F2FF File Offset: 0x0016D4FF
		// (set) Token: 0x06003966 RID: 14694 RVA: 0x0016F30B File Offset: 0x0016D50B
		public static bool DisableQuickStartCryptoSickness
		{
			get
			{
				return Prefs.data.disableQuickStartCryptoSickness;
			}
			set
			{
				if (Prefs.data.disableQuickStartCryptoSickness == value)
				{
					return;
				}
				Prefs.data.disableQuickStartCryptoSickness = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06003967 RID: 14695 RVA: 0x0016F32B File Offset: 0x0016D52B
		// (set) Token: 0x06003968 RID: 14696 RVA: 0x0016F337 File Offset: 0x0016D537
		public static bool StartDevPaletteOn
		{
			get
			{
				return Prefs.data.quickStartDevPaletteOn;
			}
			set
			{
				if (Prefs.data.quickStartDevPaletteOn == value)
				{
					return;
				}
				Prefs.data.quickStartDevPaletteOn = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06003969 RID: 14697 RVA: 0x0016F357 File Offset: 0x0016D557
		// (set) Token: 0x0600396A RID: 14698 RVA: 0x0016F363 File Offset: 0x0016D563
		public static bool PauseOnLoad
		{
			get
			{
				return Prefs.data.pauseOnLoad;
			}
			set
			{
				Prefs.data.pauseOnLoad = value;
			}
		}

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x0600396B RID: 14699 RVA: 0x0016F370 File Offset: 0x0016D570
		// (set) Token: 0x0600396C RID: 14700 RVA: 0x0016F37C File Offset: 0x0016D57C
		public static AutomaticPauseMode AutomaticPauseMode
		{
			get
			{
				return Prefs.data.automaticPauseMode;
			}
			set
			{
				Prefs.data.automaticPauseMode = value;
			}
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x0600396D RID: 14701 RVA: 0x0016F389 File Offset: 0x0016D589
		// (set) Token: 0x0600396E RID: 14702 RVA: 0x0016F395 File Offset: 0x0016D595
		public static bool ShowRealtimeClock
		{
			get
			{
				return Prefs.data.showRealtimeClock;
			}
			set
			{
				Prefs.data.showRealtimeClock = value;
			}
		}

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x0600396F RID: 14703 RVA: 0x0016F3A2 File Offset: 0x0016D5A2
		// (set) Token: 0x06003970 RID: 14704 RVA: 0x0016F3AE File Offset: 0x0016D5AE
		public static bool TwelveHourClockMode
		{
			get
			{
				return Prefs.data.twelveHourClock;
			}
			set
			{
				Prefs.data.twelveHourClock = value;
			}
		}

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06003971 RID: 14705 RVA: 0x0016F3BB File Offset: 0x0016D5BB
		// (set) Token: 0x06003972 RID: 14706 RVA: 0x0016F3C7 File Offset: 0x0016D5C7
		public static bool DisableTinyText
		{
			get
			{
				return Prefs.data.disableTinyText;
			}
			set
			{
				Prefs.data.disableTinyText = value;
			}
		}

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06003973 RID: 14707 RVA: 0x0016F3D4 File Offset: 0x0016D5D4
		// (set) Token: 0x06003974 RID: 14708 RVA: 0x0016F3E0 File Offset: 0x0016D5E0
		public static bool TestMapSizes
		{
			get
			{
				return Prefs.data.testMapSizes;
			}
			set
			{
				Prefs.data.testMapSizes = value;
			}
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06003975 RID: 14709 RVA: 0x0016F3ED File Offset: 0x0016D5ED
		// (set) Token: 0x06003976 RID: 14710 RVA: 0x0016F3F9 File Offset: 0x0016D5F9
		public static int MaxNumberOfPlayerSettlements
		{
			get
			{
				return Prefs.data.maxNumberOfPlayerSettlements;
			}
			set
			{
				Prefs.data.maxNumberOfPlayerSettlements = value;
			}
		}

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x06003977 RID: 14711 RVA: 0x0016F406 File Offset: 0x0016D606
		// (set) Token: 0x06003978 RID: 14712 RVA: 0x0016F412 File Offset: 0x0016D612
		public static bool PlantWindSway
		{
			get
			{
				return Prefs.data.plantWindSway;
			}
			set
			{
				Prefs.data.plantWindSway = value;
			}
		}

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06003979 RID: 14713 RVA: 0x0016F41F File Offset: 0x0016D61F
		// (set) Token: 0x0600397A RID: 14714 RVA: 0x0016F422 File Offset: 0x0016D622
		public static bool TextureCompression
		{
			get
			{
				return true;
			}
			set
			{
				Prefs.data.textureCompression = value;
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x0600397B RID: 14715 RVA: 0x0016F42F File Offset: 0x0016D62F
		// (set) Token: 0x0600397C RID: 14716 RVA: 0x0016F43B File Offset: 0x0016D63B
		public static bool ResourceReadoutCategorized
		{
			get
			{
				return Prefs.data.resourceReadoutCategorized;
			}
			set
			{
				if (value == Prefs.data.resourceReadoutCategorized)
				{
					return;
				}
				Prefs.data.resourceReadoutCategorized = value;
				Prefs.Save();
			}
		}

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x0600397D RID: 14717 RVA: 0x0016F45B File Offset: 0x0016D65B
		// (set) Token: 0x0600397E RID: 14718 RVA: 0x0016F46D File Offset: 0x0016D66D
		public static float UIScale
		{
			get
			{
				return Prefs.data.uiScale * ResolutionUtility.DpiScale;
			}
			set
			{
				Prefs.data.uiScale = value / ResolutionUtility.DpiScale;
			}
		}

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x0600397F RID: 14719 RVA: 0x0016F480 File Offset: 0x0016D680
		// (set) Token: 0x06003980 RID: 14720 RVA: 0x0016F48C File Offset: 0x0016D68C
		public static float UIScalePreference
		{
			get
			{
				return Prefs.data.uiScale;
			}
			set
			{
				Prefs.data.uiScale = value;
			}
		}

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x06003981 RID: 14721 RVA: 0x0016F499 File Offset: 0x0016D699
		// (set) Token: 0x06003982 RID: 14722 RVA: 0x0016F4A8 File Offset: 0x0016D6A8
		public static bool RespectSystemScaling
		{
			get
			{
				return !Prefs.data.ignoreSystemScaling;
			}
			set
			{
				Prefs.data.ignoreSystemScaling = !value;
			}
		}

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06003983 RID: 14723 RVA: 0x0016F4B8 File Offset: 0x0016D6B8
		// (set) Token: 0x06003984 RID: 14724 RVA: 0x0016F4C4 File Offset: 0x0016D6C4
		public static int ScreenWidth
		{
			get
			{
				return Prefs.data.screenWidth;
			}
			set
			{
				Prefs.data.screenWidth = value;
			}
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x06003985 RID: 14725 RVA: 0x0016F4D1 File Offset: 0x0016D6D1
		// (set) Token: 0x06003986 RID: 14726 RVA: 0x0016F4DD File Offset: 0x0016D6DD
		public static int ScreenHeight
		{
			get
			{
				return Prefs.data.screenHeight;
			}
			set
			{
				Prefs.data.screenHeight = value;
			}
		}

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x06003987 RID: 14727 RVA: 0x0016F4EA File Offset: 0x0016D6EA
		// (set) Token: 0x06003988 RID: 14728 RVA: 0x0016F4F6 File Offset: 0x0016D6F6
		public static int WindowPositionX
		{
			get
			{
				return Prefs.data.windowPositionX;
			}
			set
			{
				Prefs.data.windowPositionX = value;
			}
		}

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x06003989 RID: 14729 RVA: 0x0016F503 File Offset: 0x0016D703
		// (set) Token: 0x0600398A RID: 14730 RVA: 0x0016F50F File Offset: 0x0016D70F
		public static int WindowPositionY
		{
			get
			{
				return Prefs.data.windowPositionY;
			}
			set
			{
				Prefs.data.windowPositionY = value;
			}
		}

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x0600398B RID: 14731 RVA: 0x0016F51C File Offset: 0x0016D71C
		public static bool HasWindowPosition
		{
			get
			{
				return Prefs.data.windowPositionX != int.MinValue && Prefs.data.windowPositionY != int.MinValue;
			}
		}

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x0600398C RID: 14732 RVA: 0x0016F545 File Offset: 0x0016D745
		// (set) Token: 0x0600398D RID: 14733 RVA: 0x0016F551 File Offset: 0x0016D751
		public static string PreferredMonitorId
		{
			get
			{
				return Prefs.data.preferredMonitorId;
			}
			set
			{
				Prefs.data.preferredMonitorId = value;
			}
		}

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x0600398E RID: 14734 RVA: 0x0016F55E File Offset: 0x0016D75E
		// (set) Token: 0x0600398F RID: 14735 RVA: 0x0016F56A File Offset: 0x0016D76A
		public static string PreferredGpuName
		{
			get
			{
				return Prefs.data.preferredGpuName;
			}
			set
			{
				Prefs.data.preferredGpuName = value;
			}
		}

		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x06003990 RID: 14736 RVA: 0x0016F577 File Offset: 0x0016D777
		// (set) Token: 0x06003991 RID: 14737 RVA: 0x0016F583 File Offset: 0x0016D783
		public static string PreferredGpuUuid
		{
			get
			{
				return Prefs.data.preferredGpuUuid;
			}
			set
			{
				Prefs.data.preferredGpuUuid = value;
			}
		}

		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x06003992 RID: 14738 RVA: 0x0016F590 File Offset: 0x0016D790
		public static bool IsInitialized
		{
			get
			{
				return Prefs.data != null;
			}
		}

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x06003993 RID: 14739 RVA: 0x0016F59A File Offset: 0x0016D79A
		// (set) Token: 0x06003994 RID: 14740 RVA: 0x0016F5A6 File Offset: 0x0016D7A6
		public static bool FullScreen
		{
			get
			{
				return Prefs.data.fullscreen;
			}
			set
			{
				Prefs.data.fullscreen = value;
			}
		}

		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x06003995 RID: 14741 RVA: 0x0016F5B3 File Offset: 0x0016D7B3
		// (set) Token: 0x06003996 RID: 14742 RVA: 0x0016F5D2 File Offset: 0x0016D7D2
		public static FullScreenMode FullScreenMode
		{
			get
			{
				if (!Prefs.data.fullscreen)
				{
					return FullScreenMode.Windowed;
				}
				if (!Prefs.data.exclusiveFullscreen)
				{
					return FullScreenMode.FullScreenWindow;
				}
				return FullScreenMode.ExclusiveFullScreen;
			}
			set
			{
				Prefs.data.fullscreen = (value != FullScreenMode.Windowed);
				Prefs.data.exclusiveFullscreen = (value == FullScreenMode.ExclusiveFullScreen);
			}
		}

		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x06003997 RID: 14743 RVA: 0x0016F5F3 File Offset: 0x0016D7F3
		// (set) Token: 0x06003998 RID: 14744 RVA: 0x0016F5FF File Offset: 0x0016D7FF
		public static bool HatsOnlyOnMap
		{
			get
			{
				return Prefs.data.hatsOnlyOnMap;
			}
			set
			{
				if (Prefs.data.hatsOnlyOnMap == value)
				{
					return;
				}
				Prefs.data.hatsOnlyOnMap = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06003999 RID: 14745 RVA: 0x0016F61F File Offset: 0x0016D81F
		// (set) Token: 0x0600399A RID: 14746 RVA: 0x0016F62B File Offset: 0x0016D82B
		public static float MapDragSensitivity
		{
			get
			{
				return Prefs.data.mapDragSensitivity;
			}
			set
			{
				Prefs.data.mapDragSensitivity = value;
			}
		}

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x0600399B RID: 14747 RVA: 0x0016F638 File Offset: 0x0016D838
		// (set) Token: 0x0600399C RID: 14748 RVA: 0x0016F67F File Offset: 0x0016D87F
		public static ExpansionDef BackgroundImageExpansion
		{
			get
			{
				if (Prefs.data.backgroundExpansionId != null)
				{
					ExpansionDef expansionWithIdentifier = ModLister.GetExpansionWithIdentifier(Prefs.data.backgroundExpansionId);
					if (expansionWithIdentifier != null && expansionWithIdentifier.Status != ExpansionStatus.NotInstalled)
					{
						return expansionWithIdentifier;
					}
				}
				ExpansionDef lastInstalledExpansion = ModsConfig.LastInstalledExpansion;
				if (lastInstalledExpansion != null)
				{
					return lastInstalledExpansion;
				}
				return ExpansionDefOf.Core;
			}
			set
			{
				Prefs.data.backgroundExpansionId = ((value != null) ? value.linkedMod : null);
				((UI_BackgroundMain)UIMenuBackgroundManager.background).overrideBGImage = ((value != null) ? value.BackgroundImage : null);
			}
		}

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x0600399D RID: 14749 RVA: 0x0016F6B2 File Offset: 0x0016D8B2
		// (set) Token: 0x0600399E RID: 14750 RVA: 0x0016F6BE File Offset: 0x0016D8BE
		public static bool RandomBackgroundImage
		{
			get
			{
				return Prefs.data.randomBackground;
			}
			set
			{
				Prefs.data.randomBackground = value;
			}
		}

		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x0600399F RID: 14751 RVA: 0x0016F6CB File Offset: 0x0016D8CB
		// (set) Token: 0x060039A0 RID: 14752 RVA: 0x0016F6D7 File Offset: 0x0016D8D7
		public static List<string> DebugActionsPalette
		{
			get
			{
				return Prefs.data.debugActionPalette;
			}
			set
			{
				if (Prefs.data.debugActionPalette != value)
				{
					Prefs.data.debugActionPalette = value;
					Prefs.Save();
				}
			}
		}

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x060039A1 RID: 14753 RVA: 0x0016F6F6 File Offset: 0x0016D8F6
		// (set) Token: 0x060039A2 RID: 14754 RVA: 0x0016F702 File Offset: 0x0016D902
		public static Vector2 DevPalettePosition
		{
			get
			{
				return Prefs.data.devPalettePosition;
			}
			set
			{
				if (Prefs.data.devPalettePosition != value)
				{
					Prefs.data.devPalettePosition = value;
					Prefs.Save();
				}
			}
		}

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x060039A3 RID: 14755 RVA: 0x0016F726 File Offset: 0x0016D926
		// (set) Token: 0x060039A4 RID: 14756 RVA: 0x0016F732 File Offset: 0x0016D932
		public static bool SmoothCameraJumps
		{
			get
			{
				return Prefs.data.smoothCameraJumps;
			}
			set
			{
				if (Prefs.data.smoothCameraJumps == value)
				{
					return;
				}
				Prefs.data.smoothCameraJumps = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x060039A5 RID: 14757 RVA: 0x0016F752 File Offset: 0x0016D952
		// (set) Token: 0x060039A6 RID: 14758 RVA: 0x0016F75E File Offset: 0x0016D95E
		public static bool GravshipCutscenes
		{
			get
			{
				return Prefs.data.gravshipCutscenes;
			}
			set
			{
				if (Prefs.data.gravshipCutscenes == value)
				{
					return;
				}
				Prefs.data.gravshipCutscenes = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x060039A7 RID: 14759 RVA: 0x0016F77E File Offset: 0x0016D97E
		// (set) Token: 0x060039A8 RID: 14760 RVA: 0x0016F790 File Offset: 0x0016D990
		public static bool OpenLogOnWarnings
		{
			get
			{
				PrefsData prefsData = Prefs.data;
				return prefsData != null && prefsData.openLogOnWarnings;
			}
			set
			{
				if (Prefs.data.openLogOnWarnings == value)
				{
					return;
				}
				Prefs.data.openLogOnWarnings = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x060039A9 RID: 14761 RVA: 0x0016F7B0 File Offset: 0x0016D9B0
		// (set) Token: 0x060039AA RID: 14762 RVA: 0x0016F7BC File Offset: 0x0016D9BC
		public static bool CloseLogWindowOnEscape
		{
			get
			{
				return Prefs.data.closeLogWindowOnEscape;
			}
			set
			{
				if (Prefs.data.closeLogWindowOnEscape == value)
				{
					return;
				}
				Prefs.data.closeLogWindowOnEscape = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x060039AB RID: 14763 RVA: 0x0016F7DC File Offset: 0x0016D9DC
		// (set) Token: 0x060039AC RID: 14764 RVA: 0x0016F7E8 File Offset: 0x0016D9E8
		public static int AutosavesCount
		{
			get
			{
				return Prefs.data.autosavesCount;
			}
			set
			{
				if (Prefs.data.autosavesCount == value)
				{
					return;
				}
				Prefs.data.autosavesCount = value;
				Prefs.Apply();
			}
		}

		// Token: 0x060039AD RID: 14765 RVA: 0x0016F808 File Offset: 0x0016DA08
		public static void UseDefaultsWithoutFile()
		{
			Prefs.data = new PrefsData();
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x0016F814 File Offset: 0x0016DA14
		public static void Init()
		{
			bool flag = !new FileInfo(GenFilePaths.PrefsFilePath).Exists;
			Prefs.data = new PrefsData();
			Prefs.data = DirectXmlLoader.ItemFromXmlFile<PrefsData>(GenFilePaths.PrefsFilePath, true);
			BackCompatibility.PrefsDataPostLoad(Prefs.data);
			if (flag)
			{
				Prefs.data.langFolderName = LanguageDatabase.SystemLanguageFolderName();
				Prefs.data.uiScale = ResolutionUtility.GetRecommendedUIScale(Prefs.data.screenWidth, Prefs.data.screenHeight);
			}
			if (DevModePermanentlyDisabledUtility.Disabled)
			{
				Prefs.DevMode = false;
			}
			Prefs.Apply();
		}

		// Token: 0x060039AF RID: 14767 RVA: 0x0016F8A0 File Offset: 0x0016DAA0
		public static void Save()
		{
			try
			{
				XDocument xdocument = new XDocument();
				XElement content = DirectXmlSaver.XElementFromObject(Prefs.data, typeof(PrefsData));
				xdocument.Add(content);
				xdocument.Save(GenFilePaths.PrefsFilePath);
			}
			catch (Exception ex)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(GenFilePaths.PrefsFilePath, ex.ToString()));
				string str = "Exception saving prefs: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x060039B0 RID: 14768 RVA: 0x0016F934 File Offset: 0x0016DB34
		public static void Apply()
		{
			Prefs.data.Apply();
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x0016F940 File Offset: 0x0016DB40
		public static void Notify_NewExpansion()
		{
			Prefs.data.backgroundExpansionId = null;
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x0016F950 File Offset: 0x0016DB50
		public static NameTriple RandomPreferredName()
		{
			string rawName;
			if ((from name in Prefs.PreferredNames
			where !name.NullOrEmpty()
			select name).TryRandomElement(out rawName))
			{
				return NameTriple.FromString(rawName, false);
			}
			return null;
		}

		// Token: 0x040025A7 RID: 9639
		private static PrefsData data;
	}
}
