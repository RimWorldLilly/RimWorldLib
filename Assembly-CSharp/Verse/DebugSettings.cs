using System;

namespace Verse
{
	// Token: 0x020006B8 RID: 1720
	public static class DebugSettings
	{
		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x060036AE RID: 13998 RVA: 0x0015EE3C File Offset: 0x0015D03C
		public static bool ShowDevGizmos
		{
			get
			{
				return Prefs.DevMode && DebugSettings.godMode;
			}
		}

		// Token: 0x04002348 RID: 9032
		public const bool DebugBuild = false;

		// Token: 0x04002349 RID: 9033
		public static bool enableDamage = true;

		// Token: 0x0400234A RID: 9034
		public static bool enablePlayerDamage = true;

		// Token: 0x0400234B RID: 9035
		public static bool enableRandomMentalStates = true;

		// Token: 0x0400234C RID: 9036
		public static bool enableStoryteller = true;

		// Token: 0x0400234D RID: 9037
		public static bool enableRandomDiseases = true;

		// Token: 0x0400234E RID: 9038
		public static bool enableTranslationWindowInEnglish = false;

		// Token: 0x0400234F RID: 9039
		public static bool godMode = false;

		// Token: 0x04002350 RID: 9040
		public static bool devPalette = false;

		// Token: 0x04002351 RID: 9041
		public static bool pauseOnError = false;

		// Token: 0x04002352 RID: 9042
		public static bool noAnimals = false;

		// Token: 0x04002353 RID: 9043
		public static bool unlimitedPower = false;

		// Token: 0x04002354 RID: 9044
		public static bool pathThroughWalls = false;

		// Token: 0x04002355 RID: 9045
		public static bool instantRecruit = false;

		// Token: 0x04002356 RID: 9046
		public static bool alwaysSocialFight = false;

		// Token: 0x04002357 RID: 9047
		public static bool alwaysDoLovin = false;

		// Token: 0x04002358 RID: 9048
		public static bool detectRegionListersBugs = false;

		// Token: 0x04002359 RID: 9049
		public static bool instantVisitorsGift = false;

		// Token: 0x0400235A RID: 9050
		public static bool lowFPS = false;

		// Token: 0x0400235B RID: 9051
		public static bool allowUndraftedMechOrders = false;

		// Token: 0x0400235C RID: 9052
		public static bool editableGlowerColors = false;

		// Token: 0x0400235D RID: 9053
		public static bool showHiddenPawns = false;

		// Token: 0x0400235E RID: 9054
		public static bool showHiddenInfo = false;

		// Token: 0x0400235F RID: 9055
		public static bool anomalyDarkeningFX = true;

		// Token: 0x04002360 RID: 9056
		public static bool fastResearch = false;

		// Token: 0x04002361 RID: 9057
		public static bool fastLearning = false;

		// Token: 0x04002362 RID: 9058
		public static bool fastEcology = false;

		// Token: 0x04002363 RID: 9059
		public static bool fastEcologyRegrowRateOnly = false;

		// Token: 0x04002364 RID: 9060
		public static bool fastCrafting = false;

		// Token: 0x04002365 RID: 9061
		public static bool fastCaravans = false;

		// Token: 0x04002366 RID: 9062
		public static bool fastMapUnpollution = false;

		// Token: 0x04002367 RID: 9063
		public static bool activateAllBuildingDemands = false;

		// Token: 0x04002368 RID: 9064
		public static bool activateAllIdeoRoles = false;

		// Token: 0x04002369 RID: 9065
		public static bool showLocomotionUrgency = false;

		// Token: 0x0400236A RID: 9066
		public static bool playRitualAmbience = true;

		// Token: 0x0400236B RID: 9067
		public static bool simulateUsingSteamDeck = false;

		// Token: 0x0400236C RID: 9068
		public static bool logRaidInfo = false;

		// Token: 0x0400236D RID: 9069
		public static bool logTranslationLookupErrors = false;

		// Token: 0x0400236E RID: 9070
		public static bool logPsychicRitualTransitions = false;

		// Token: 0x0400236F RID: 9071
		public static bool fastMonolithRespawn = false;

		// Token: 0x04002370 RID: 9072
		public static bool searchIgnoresRestrictions = false;

		// Token: 0x04002371 RID: 9073
		public static bool alwaysRareCatches = false;

		// Token: 0x04002372 RID: 9074
		public static bool alwaysNegativeCatches = false;

		// Token: 0x04002373 RID: 9075
		public static bool logMismatchedLayoutFactions = false;

		// Token: 0x04002374 RID: 9076
		public static bool loopGravshipCutscene = false;

		// Token: 0x04002375 RID: 9077
		public static bool skipGravshipTileSelection = false;

		// Token: 0x04002376 RID: 9078
		public static bool ignoreGravshipRange = false;
	}
}
