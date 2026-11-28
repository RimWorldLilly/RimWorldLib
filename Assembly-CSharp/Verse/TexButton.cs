using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000657 RID: 1623
	[StaticConstructorOnStartup]
	public class TexButton
	{
		// Token: 0x04002088 RID: 8328
		public static readonly Texture2D CloseXBig = ContentFinder<Texture2D>.Get("UI/Widgets/CloseX", true);

		// Token: 0x04002089 RID: 8329
		public static readonly Texture2D CloseXSmall = ContentFinder<Texture2D>.Get("UI/Widgets/CloseXSmall", true);

		// Token: 0x0400208A RID: 8330
		public static readonly Texture2D NextBig = ContentFinder<Texture2D>.Get("UI/Widgets/NextArrow", true);

		// Token: 0x0400208B RID: 8331
		public static readonly Texture2D Delete = ContentFinder<Texture2D>.Get("UI/Buttons/Dismiss", true);

		// Token: 0x0400208C RID: 8332
		public static readonly Texture2D ReorderUp = ContentFinder<Texture2D>.Get("UI/Buttons/ReorderUp", true);

		// Token: 0x0400208D RID: 8333
		public static readonly Texture2D ReorderDown = ContentFinder<Texture2D>.Get("UI/Buttons/ReorderDown", true);

		// Token: 0x0400208E RID: 8334
		public static readonly Texture2D Plus = ContentFinder<Texture2D>.Get("UI/Buttons/Plus", true);

		// Token: 0x0400208F RID: 8335
		public static readonly Texture2D Minus = ContentFinder<Texture2D>.Get("UI/Buttons/Minus", true);

		// Token: 0x04002090 RID: 8336
		public static readonly Texture2D Suspend = ContentFinder<Texture2D>.Get("UI/Buttons/Suspend", true);

		// Token: 0x04002091 RID: 8337
		public static readonly Texture2D PlaceholderImage = ContentFinder<Texture2D>.Get("PlaceholderImage", true);

		// Token: 0x04002092 RID: 8338
		public static readonly Texture2D SelectOverlappingNext = ContentFinder<Texture2D>.Get("UI/Buttons/SelectNextOverlapping", true);

		// Token: 0x04002093 RID: 8339
		public static readonly Texture2D Info = ContentFinder<Texture2D>.Get("UI/Buttons/InfoButton", true);

		// Token: 0x04002094 RID: 8340
		public static readonly Texture2D Rename = ContentFinder<Texture2D>.Get("UI/Buttons/Rename", true);

		// Token: 0x04002095 RID: 8341
		public static readonly Texture2D Banish = ContentFinder<Texture2D>.Get("UI/Buttons/Banish", true);

		// Token: 0x04002096 RID: 8342
		public static readonly Texture2D OpenStatsReport = ContentFinder<Texture2D>.Get("UI/Buttons/OpenStatsReport", true);

		// Token: 0x04002097 RID: 8343
		public static readonly Texture2D RenounceTitle = ContentFinder<Texture2D>.Get("UI/Buttons/Renounce", true);

		// Token: 0x04002098 RID: 8344
		public static readonly Texture2D ExecuteColonist = ContentFinder<Texture2D>.Get("UI/Buttons/Execute", true);

		// Token: 0x04002099 RID: 8345
		public static readonly Texture2D Copy = ContentFinder<Texture2D>.Get("UI/Buttons/Copy", true);

		// Token: 0x0400209A RID: 8346
		public static readonly Texture2D Paste = ContentFinder<Texture2D>.Get("UI/Buttons/Paste", true);

		// Token: 0x0400209B RID: 8347
		public static readonly Texture2D Drop = ContentFinder<Texture2D>.Get("UI/Buttons/Drop", true);

		// Token: 0x0400209C RID: 8348
		public static readonly Texture2D Ingest = ContentFinder<Texture2D>.Get("UI/Buttons/Ingest", true);

		// Token: 0x0400209D RID: 8349
		public static readonly Texture2D DragHash = ContentFinder<Texture2D>.Get("UI/Buttons/DragHash", true);

		// Token: 0x0400209E RID: 8350
		public static readonly Texture2D Infinity = ContentFinder<Texture2D>.Get("UI/Buttons/Infinity", true);

		// Token: 0x0400209F RID: 8351
		public static readonly Texture2D Search = ContentFinder<Texture2D>.Get("UI/Widgets/Search", true);

		// Token: 0x040020A0 RID: 8352
		public static readonly Texture2D SearchButton = ContentFinder<Texture2D>.Get("UI/Widgets/SearchToggle", true);

		// Token: 0x040020A1 RID: 8353
		public static readonly Texture2D CodexButton = ContentFinder<Texture2D>.Get("UI/Buttons/OpenCodex", true);

		// Token: 0x040020A2 RID: 8354
		public static readonly Texture2D ToggleLog = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/ToggleLog", true);

		// Token: 0x040020A3 RID: 8355
		public static readonly Texture2D OpenDebugActionsMenu = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/OpenDebugActionsMenu", true);

		// Token: 0x040020A4 RID: 8356
		public static readonly Texture2D OpenInspector = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/OpenInspector", true);

		// Token: 0x040020A5 RID: 8357
		public static readonly Texture2D OpenInspectSettings = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/OpenInspectSettings", true);

		// Token: 0x040020A6 RID: 8358
		public static readonly Texture2D GodModeDisabled = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/GodModeDisabled", true);

		// Token: 0x040020A7 RID: 8359
		public static readonly Texture2D GodModeEnabled = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/GodModeEnabled", true);

		// Token: 0x040020A8 RID: 8360
		public static readonly Texture2D ToggleTweak = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/ToggleTweak", true);

		// Token: 0x040020A9 RID: 8361
		public static readonly Texture2D ToggleDevPalette = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/ToggleDevPalette", true);

		// Token: 0x040020AA RID: 8362
		public static readonly Texture2D HotReloadDefs = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/HotReloadDefs", true);

		// Token: 0x040020AB RID: 8363
		public static readonly Texture2D Add = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Add", true);

		// Token: 0x040020AC RID: 8364
		public static readonly Texture2D NewItem = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/NewItem", true);

		// Token: 0x040020AD RID: 8365
		public static readonly Texture2D Reveal = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Reveal", true);

		// Token: 0x040020AE RID: 8366
		public static readonly Texture2D Collapse = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Collapse", true);

		// Token: 0x040020AF RID: 8367
		public static readonly Texture2D Empty = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Empty", true);

		// Token: 0x040020B0 RID: 8368
		public static readonly Texture2D Save = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Save", true);

		// Token: 0x040020B1 RID: 8369
		public static readonly Texture2D NewFile = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/NewFile", true);

		// Token: 0x040020B2 RID: 8370
		public static readonly Texture2D RenameDev = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Rename", true);

		// Token: 0x040020B3 RID: 8371
		public static readonly Texture2D Reload = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Reload", true);

		// Token: 0x040020B4 RID: 8372
		public static readonly Texture2D Play = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Play", true);

		// Token: 0x040020B5 RID: 8373
		public static readonly Texture2D Stop = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Stop", true);

		// Token: 0x040020B6 RID: 8374
		public static readonly Texture2D RangeMatch = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/RangeMatch", true);

		// Token: 0x040020B7 RID: 8375
		public static readonly Texture2D CenterOnPointsTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/CenterOnPoints", true);

		// Token: 0x040020B8 RID: 8376
		public static readonly Texture2D CurveResetTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/CurveReset", true);

		// Token: 0x040020B9 RID: 8377
		public static readonly Texture2D QuickZoomHor1Tex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomHor1", true);

		// Token: 0x040020BA RID: 8378
		public static readonly Texture2D QuickZoomHor100Tex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomHor100", true);

		// Token: 0x040020BB RID: 8379
		public static readonly Texture2D QuickZoomHor20kTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomHor20k", true);

		// Token: 0x040020BC RID: 8380
		public static readonly Texture2D QuickZoomVer1Tex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomVer1", true);

		// Token: 0x040020BD RID: 8381
		public static readonly Texture2D QuickZoomVer100Tex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomVer100", true);

		// Token: 0x040020BE RID: 8382
		public static readonly Texture2D QuickZoomVer20kTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomVer20k", true);

		// Token: 0x040020BF RID: 8383
		public static readonly Texture2D IconBlog = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Blog", true);

		// Token: 0x040020C0 RID: 8384
		public static readonly Texture2D IconForums = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Forums", true);

		// Token: 0x040020C1 RID: 8385
		public static readonly Texture2D IconReddit = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Reddit", true);

		// Token: 0x040020C2 RID: 8386
		public static readonly Texture2D IconWiki = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Wiki", true);

		// Token: 0x040020C3 RID: 8387
		public static readonly Texture2D IconTwitter = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Twitter", true);

		// Token: 0x040020C4 RID: 8388
		public static readonly Texture2D IconX = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/X", true);

		// Token: 0x040020C5 RID: 8389
		public static readonly Texture2D IconBook = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Book", true);

		// Token: 0x040020C6 RID: 8390
		public static readonly Texture2D IconSoundtrack = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Soundtrack", true);

		// Token: 0x040020C7 RID: 8391
		public static readonly Texture2D ShowLearningHelper = ContentFinder<Texture2D>.Get("UI/Buttons/ShowLearningHelper", true);

		// Token: 0x040020C8 RID: 8392
		public static readonly Texture2D ShowZones = ContentFinder<Texture2D>.Get("UI/Buttons/ShowZones", true);

		// Token: 0x040020C9 RID: 8393
		public static readonly Texture2D ShowFertilityOverlay = ContentFinder<Texture2D>.Get("UI/Buttons/ShowFertilityOverlay", true);

		// Token: 0x040020CA RID: 8394
		public static readonly Texture2D ShowPollutionOverlay = ContentFinder<Texture2D>.Get("UI/Buttons/ShowPollutionOverlay", true);

		// Token: 0x040020CB RID: 8395
		public static readonly Texture2D ShowTerrainAffordanceOverlay = ContentFinder<Texture2D>.Get("UI/Buttons/ShowTerrainAffordanceOverlay", true);

		// Token: 0x040020CC RID: 8396
		public static readonly Texture2D ShowBeauty = ContentFinder<Texture2D>.Get("UI/Buttons/ShowBeauty", true);

		// Token: 0x040020CD RID: 8397
		public static readonly Texture2D ShowRoomStats = ContentFinder<Texture2D>.Get("UI/Buttons/ShowRoomStats", true);

		// Token: 0x040020CE RID: 8398
		public static readonly Texture2D ShowColonistBar = ContentFinder<Texture2D>.Get("UI/Buttons/ShowColonistBar", true);

		// Token: 0x040020CF RID: 8399
		public static readonly Texture2D ShowRoofOverlay = ContentFinder<Texture2D>.Get("UI/Buttons/ShowRoofOverlay", true);

		// Token: 0x040020D0 RID: 8400
		public static readonly Texture2D AutoHomeArea = ContentFinder<Texture2D>.Get("UI/Buttons/AutoHomeArea", true);

		// Token: 0x040020D1 RID: 8401
		public static readonly Texture2D AutoRebuild = ContentFinder<Texture2D>.Get("UI/Buttons/AutoRebuild", true);

		// Token: 0x040020D2 RID: 8402
		public static readonly Texture2D CategorizedResourceReadout = ContentFinder<Texture2D>.Get("UI/Buttons/ResourceReadoutCategorized", true);

		// Token: 0x040020D3 RID: 8403
		public static readonly Texture2D LockNorthUp = ContentFinder<Texture2D>.Get("UI/Buttons/LockNorthUp", true);

		// Token: 0x040020D4 RID: 8404
		public static readonly Texture2D UsePlanetDayNightSystem = ContentFinder<Texture2D>.Get("UI/Buttons/UsePlanetDayNightSystem", true);

		// Token: 0x040020D5 RID: 8405
		public static readonly Texture2D ShowExpandingIcons = ContentFinder<Texture2D>.Get("UI/Buttons/ShowExpandingIcons", true);

		// Token: 0x040020D6 RID: 8406
		public static readonly Texture2D ShowWorldFeatures = ContentFinder<Texture2D>.Get("UI/Buttons/ShowWorldFeatures", true);

		// Token: 0x040020D7 RID: 8407
		public static readonly Texture2D ShowTemperatureOverlay = ContentFinder<Texture2D>.Get("UI/Buttons/ToggleHeatOverlay", true);

		// Token: 0x040020D8 RID: 8408
		public static readonly Texture2D ShowVacuumOverlay = ContentFinder<Texture2D>.Get("UI/Buttons/ToggleVacuumOverlay", true);

		// Token: 0x040020D9 RID: 8409
		public static readonly Texture2D ShowLandmarkIcons = ContentFinder<Texture2D>.Get("UI/Buttons/DisplayLandmarks", true);

		// Token: 0x040020DA RID: 8410
		public static readonly Texture2D ShowOtherFactionBases = ContentFinder<Texture2D>.Get("UI/Buttons/DisplayOtherFactionBases", true);

		// Token: 0x040020DB RID: 8411
		public static readonly Texture2D ShowImportantLocations = ContentFinder<Texture2D>.Get("UI/Buttons/DisplayImportantLocations", true);

		// Token: 0x040020DC RID: 8412
		public static readonly Texture2D[] SpeedButtonTextures = new Texture2D[]
		{
			ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Pause", true),
			ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Normal", true),
			ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Fast", true),
			ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Superfast", true),
			ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Superfast", true)
		};
	}
}
