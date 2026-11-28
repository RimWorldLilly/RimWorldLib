using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x0200202A RID: 8234
	public sealed class PlaySettings : IExposable
	{
		// Token: 0x0600D082 RID: 53378 RVA: 0x004CA90C File Offset: 0x004C8B0C
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.showLearningHelper, "showLearningHelper", false, false);
			Scribe_Values.Look<bool>(ref this.showZones, "showZones", false, false);
			Scribe_Values.Look<bool>(ref this.showBeauty, "showBeauty", false, false);
			Scribe_Values.Look<bool>(ref this.showRoomStats, "showRoomStats", false, false);
			Scribe_Values.Look<bool>(ref this.showColonistBar, "showColonistBar", false, false);
			Scribe_Values.Look<bool>(ref this.showRoofOverlay, "showRoofOverlay", false, false);
			Scribe_Values.Look<bool>(ref this.showFertilityOverlay, "showFertilityOverlay", false, false);
			Scribe_Values.Look<bool>(ref this.showTerrainAffordanceOverlay, "showTerrainAffordanceOverlay", false, false);
			Scribe_Values.Look<bool>(ref this.showPollutionOverlay, "showPollutionOverlay", false, false);
			Scribe_Values.Look<bool>(ref this.showVacuumOverlay, "showVacuumOverlay", false, false);
			Scribe_Values.Look<bool>(ref this.autoHomeArea, "autoHomeArea", false, false);
			Scribe_Values.Look<bool>(ref this.autoRebuild, "autoRebuild", false, false);
			Scribe_Values.Look<bool>(ref this.lockNorthUp, "lockNorthUp", false, false);
			Scribe_Values.Look<bool>(ref this.usePlanetDayNightSystem, "usePlanetDayNightSystem", false, false);
			Scribe_Values.Look<bool>(ref this.showImportantExpandingIcons, "showImportantExpandingIcons", true, false);
			Scribe_Values.Look<bool>(ref this.showBasesExpandingIcons, "showBasesExpandingIcons", true, false);
			Scribe_Values.Look<bool>(ref this.showExpandingLandmarks, "showExpandingLandmarks", false, false);
			Scribe_Values.Look<bool>(ref this.showWorldFeatures, "showWorldFeatures", false, false);
			Scribe_Values.Look<bool>(ref this.showTemperatureOverlay, "showTemperatureOverlay", false, false);
			Scribe_Values.Look<bool>(ref this.useWorkPriorities, "useWorkPriorities", false, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForColonist, "defaultCareForColonist", MedicalCareCategory.Best, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForTamedAnimal, "defaultCareForTamedAnimal", MedicalCareCategory.HerbalOrWorse, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForPrisoner, "defaultCareForPrisoner", MedicalCareCategory.HerbalOrWorse, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForSlave, "defaultCareForSlave", MedicalCareCategory.HerbalOrWorse, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForNeutralFaction, "defaultCareForNeutralFaction", MedicalCareCategory.HerbalOrWorse, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForWildlife, "defaultCareForWildlife", MedicalCareCategory.HerbalOrWorse, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForHostileFaction, "defaultCareForHumanlikeEnemies", MedicalCareCategory.HerbalOrWorse, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForFriendlyFaction, "defaultCareForFriendlyFaction", MedicalCareCategory.HerbalOrWorse, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForNoFaction, "defaultCareForNoFaction", MedicalCareCategory.HerbalOrWorse, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForEntities, "defaultCareForEntities", MedicalCareCategory.NoMeds, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForGhouls, "defaultCareForGhouls", MedicalCareCategory.NoMeds, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600D083 RID: 53379 RVA: 0x004CAB4D File Offset: 0x004C8D4D
		public void DoPlaySettingsGlobalControls(WidgetRow row, bool worldView)
		{
			bool flag = this.showColonistBar;
			if (worldView)
			{
				this.DoWorldViewControls(row);
			}
			else
			{
				this.DoMapControls(row);
			}
			if (flag != this.showColonistBar)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x0600D084 RID: 53380 RVA: 0x004CAB7C File Offset: 0x004C8D7C
		private void DoWorldViewControls(WidgetRow row)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				row.ToggleableIcon(ref this.showColonistBar, TexButton.ShowColonistBar, SteamDeck.IsSteamDeckInNonKeyboardMode ? "ShowColonistBarToggleButtonController".Translate() : "ShowColonistBarToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			}
			bool flag = this.lockNorthUp;
			row.ToggleableIcon(ref this.lockNorthUp, TexButton.LockNorthUp, "LockNorthUpToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			if (flag != this.lockNorthUp && this.lockNorthUp)
			{
				Find.WorldCameraDriver.RotateSoNorthIsUp(true);
			}
			row.ToggleableIcon(ref this.showImportantExpandingIcons, TexButton.ShowImportantLocations, "ShowImportantExpandingIconsToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			row.ToggleableIcon(ref this.showBasesExpandingIcons, TexButton.ShowOtherFactionBases, "ShowBasesExpandingIconsToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			if (ModsConfig.OdysseyActive)
			{
				row.ToggleableIcon(ref this.showExpandingLandmarks, TexButton.ShowLandmarkIcons, "ShowExpandingLandmarksToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			}
			row.ToggleableIcon(ref this.usePlanetDayNightSystem, TexButton.UsePlanetDayNightSystem, "UsePlanetDayNightSystemToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			row.ToggleableIcon(ref this.showWorldFeatures, TexButton.ShowWorldFeatures, "ShowWorldFeaturesToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			string value = KeyPrefs.KeyPrefsData.GetBoundKeyCode(KeyBindingDefOf.OpenMapSearch, KeyPrefs.BindingSlot.A).ToStringReadable();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("HotKeyTip".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(value);
			defaultInterpolatedStringHandler.AppendLiteral("\n\n");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("SearchTheWorldDesc".Translate());
			string tooltip = defaultInterpolatedStringHandler.ToStringAndClear();
			if (row.ButtonIcon(TexButton.SearchButton, tooltip, null, null, null, true, -1f) || (KeyBindingDefOf.OpenMapSearch.JustPressed && Event.current.type == EventType.keyDown))
			{
				Find.WindowStack.Add(new Dialog_WorldSearch());
			}
		}

		// Token: 0x0600D085 RID: 53381 RVA: 0x004CAD98 File Offset: 0x004C8F98
		private void DoMapControls(WidgetRow row)
		{
			row.ToggleableIcon(ref this.showLearningHelper, TexButton.ShowLearningHelper, "ShowLearningHelperWhenEmptyToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			row.ToggleableIcon(ref this.showZones, TexButton.ShowZones, "ZoneVisibilityToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			string value = KeyPrefs.KeyPrefsData.GetBoundKeyCode(KeyBindingDefOf.ToggleBeautyDisplay, KeyPrefs.BindingSlot.A).ToStringReadable();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("HotKeyTip".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(value);
			defaultInterpolatedStringHandler.AppendLiteral("\n\n");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("ShowBeautyToggleButton".Translate());
			string tooltip = defaultInterpolatedStringHandler.ToStringAndClear();
			row.ToggleableIcon(ref this.showBeauty, TexButton.ShowBeauty, tooltip, SoundDefOf.Mouseover_ButtonToggle, null);
			PlaySettings.CheckKeyBindingToggle(KeyBindingDefOf.ToggleBeautyDisplay, ref this.showBeauty);
			string value2 = KeyPrefs.KeyPrefsData.GetBoundKeyCode(KeyBindingDefOf.ToggleRoomStatsDisplay, KeyPrefs.BindingSlot.A).ToStringReadable();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(4, 3);
			defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("HotKeyTip".Translate());
			defaultInterpolatedStringHandler2.AppendLiteral(": ");
			defaultInterpolatedStringHandler2.AppendFormatted(value2);
			defaultInterpolatedStringHandler2.AppendLiteral("\n\n");
			defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("ShowRoomStatsToggleButton".Translate());
			string tooltip2 = defaultInterpolatedStringHandler2.ToStringAndClear();
			row.ToggleableIcon(ref this.showRoomStats, TexButton.ShowRoomStats, tooltip2, SoundDefOf.Mouseover_ButtonToggle, null);
			PlaySettings.CheckKeyBindingToggle(KeyBindingDefOf.ToggleRoomStatsDisplay, ref this.showRoomStats);
			row.ToggleableIcon(ref this.showColonistBar, TexButton.ShowColonistBar, SteamDeck.IsSteamDeckInNonKeyboardMode ? "ShowColonistBarToggleButtonController".Translate() : "ShowColonistBarToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			row.ToggleableIcon(ref this.showRoofOverlay, TexButton.ShowRoofOverlay, "ShowRoofOverlayToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			row.ToggleableIcon(ref this.showFertilityOverlay, TexButton.ShowFertilityOverlay, "ShowFertilityOverlayToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			row.ToggleableIcon(ref this.showTerrainAffordanceOverlay, TexButton.ShowTerrainAffordanceOverlay, "ShowTerrainAffordanceOverlayToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			row.ToggleableIcon(ref this.autoHomeArea, TexButton.AutoHomeArea, "AutoHomeAreaToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			row.ToggleableIcon(ref this.autoRebuild, TexButton.AutoRebuild, "AutoRebuildButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			row.ToggleableIcon(ref this.showTemperatureOverlay, TexButton.ShowTemperatureOverlay, "ShowTemperatureOverlayToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			bool resourceReadoutCategorized = Prefs.ResourceReadoutCategorized;
			bool flag = resourceReadoutCategorized;
			row.ToggleableIcon(ref resourceReadoutCategorized, TexButton.CategorizedResourceReadout, "CategorizedResourceReadoutToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			if (resourceReadoutCategorized != flag)
			{
				Prefs.ResourceReadoutCategorized = resourceReadoutCategorized;
			}
			if (ModsConfig.BiotechActive)
			{
				row.ToggleableIcon(ref this.showPollutionOverlay, TexButton.ShowPollutionOverlay, "ShowPollutionOverlayToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			}
			if (ModsConfig.OdysseyActive && Find.CurrentMap != null && Find.CurrentMap.Biome.inVacuum)
			{
				row.ToggleableIcon(ref this.showVacuumOverlay, TexButton.ShowVacuumOverlay, "ShowVacuumOverlayToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			}
			else
			{
				this.showVacuumOverlay = false;
			}
			string value3 = KeyPrefs.KeyPrefsData.GetBoundKeyCode(KeyBindingDefOf.OpenMapSearch, KeyPrefs.BindingSlot.A).ToStringReadable();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(4, 3);
			defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("HotKeyTip".Translate());
			defaultInterpolatedStringHandler3.AppendLiteral(": ");
			defaultInterpolatedStringHandler3.AppendFormatted(value3);
			defaultInterpolatedStringHandler3.AppendLiteral("\n\n");
			defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("SearchTheMapDesc".Translate());
			string tooltip3 = defaultInterpolatedStringHandler3.ToStringAndClear();
			if (row.ButtonIcon(TexButton.SearchButton, tooltip3, null, null, null, true, -1f) || (KeyBindingDefOf.OpenMapSearch.JustPressed && Event.current.type == EventType.keyDown))
			{
				Event.current.Use();
				Find.WindowStack.Add(new Dialog_MapSearch(Find.CurrentMap));
			}
			if (ModsConfig.AnomalyActive && Find.Anomaly.AnomalyStudyEnabled)
			{
				UIHighlighter.HighlightOpportunity(row.ButtonIconRect(-1f), "EntityCodex");
				if (row.ButtonIcon(TexButton.CodexButton, "EntityCodexGizmoTip".Translate(), null, null, null, true, -1f))
				{
					Find.WindowStack.Add(new Dialog_EntityCodex(null));
				}
			}
		}

		// Token: 0x0600D086 RID: 53382 RVA: 0x004CB233 File Offset: 0x004C9433
		private static void CheckKeyBindingToggle(KeyBindingDef keyBinding, ref bool value)
		{
			if (!keyBinding.KeyDownEvent)
			{
				return;
			}
			value = !value;
			if (value)
			{
				SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				return;
			}
			SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
		}

		// Token: 0x040084AA RID: 33962
		public bool showLearningHelper = true;

		// Token: 0x040084AB RID: 33963
		public bool showZones = true;

		// Token: 0x040084AC RID: 33964
		public bool showBeauty;

		// Token: 0x040084AD RID: 33965
		public bool showRoomStats;

		// Token: 0x040084AE RID: 33966
		public bool showColonistBar = true;

		// Token: 0x040084AF RID: 33967
		public bool showRoofOverlay;

		// Token: 0x040084B0 RID: 33968
		public bool showFertilityOverlay;

		// Token: 0x040084B1 RID: 33969
		public bool showTerrainAffordanceOverlay;

		// Token: 0x040084B2 RID: 33970
		public bool showPollutionOverlay;

		// Token: 0x040084B3 RID: 33971
		public bool showVacuumOverlay;

		// Token: 0x040084B4 RID: 33972
		public bool autoHomeArea = true;

		// Token: 0x040084B5 RID: 33973
		public bool autoRebuild;

		// Token: 0x040084B6 RID: 33974
		public bool showTemperatureOverlay;

		// Token: 0x040084B7 RID: 33975
		public bool lockNorthUp = true;

		// Token: 0x040084B8 RID: 33976
		public bool usePlanetDayNightSystem = true;

		// Token: 0x040084B9 RID: 33977
		public bool showImportantExpandingIcons = true;

		// Token: 0x040084BA RID: 33978
		public bool showBasesExpandingIcons = true;

		// Token: 0x040084BB RID: 33979
		public bool showExpandingLandmarks;

		// Token: 0x040084BC RID: 33980
		public bool showWorldFeatures = true;

		// Token: 0x040084BD RID: 33981
		public bool useWorkPriorities;

		// Token: 0x040084BE RID: 33982
		public MedicalCareCategory defaultCareForColonist = MedicalCareCategory.Best;

		// Token: 0x040084BF RID: 33983
		public MedicalCareCategory defaultCareForPrisoner = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x040084C0 RID: 33984
		public MedicalCareCategory defaultCareForSlave = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x040084C1 RID: 33985
		public MedicalCareCategory defaultCareForTamedAnimal = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x040084C2 RID: 33986
		public MedicalCareCategory defaultCareForFriendlyFaction = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x040084C3 RID: 33987
		public MedicalCareCategory defaultCareForNeutralFaction = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x040084C4 RID: 33988
		public MedicalCareCategory defaultCareForHostileFaction = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x040084C5 RID: 33989
		public MedicalCareCategory defaultCareForNoFaction = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x040084C6 RID: 33990
		public MedicalCareCategory defaultCareForWildlife = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x040084C7 RID: 33991
		public MedicalCareCategory defaultCareForEntities = MedicalCareCategory.NoMeds;

		// Token: 0x040084C8 RID: 33992
		public MedicalCareCategory defaultCareForGhouls = MedicalCareCategory.NoMeds;
	}
}
