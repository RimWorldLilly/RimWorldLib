using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;
using Waylith;
using Waylith.Graphics;

namespace RimWorld
{
	// Token: 0x02001CC3 RID: 7363
	[StaticConstructorOnStartup]
	public class Dialog_Options : Window
	{
		// Token: 0x1700226A RID: 8810
		// (get) Token: 0x0600BD09 RID: 48393 RVA: 0x0043A609 File Offset: 0x00438809
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(650f, 600f);
			}
		}

		// Token: 0x0600BD0A RID: 48394 RVA: 0x0043A61A File Offset: 0x0043881A
		public Dialog_Options(OptionCategoryDef initialCategory) : this()
		{
			this.selectedCategory = initialCategory;
			this.selectedMod = null;
		}

		// Token: 0x0600BD0B RID: 48395 RVA: 0x0043A630 File Offset: 0x00438830
		public Dialog_Options(Mod initialMod) : this()
		{
			this.selectedCategory = null;
			this.selectedMod = initialMod;
		}

		// Token: 0x0600BD0C RID: 48396 RVA: 0x0043A648 File Offset: 0x00438848
		public Dialog_Options() : base(null)
		{
			this.doCloseX = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600BD0D RID: 48397 RVA: 0x0043A6A0 File Offset: 0x004388A0
		public override void PostOpen()
		{
			base.PostOpen();
			this.quickSearchWidget.Reset();
			this.cachedModsWithSettings = from mod in LoadedModManager.ModHandles
			where !mod.SettingsCategory().NullOrEmpty()
			orderby mod.SettingsCategory()
			select mod;
			foreach (OptionCategoryDef optionCategoryDef in DefDatabase<OptionCategoryDef>.AllDefs)
			{
				if (!optionCategoryDef.modContentPack.IsOfficialMod)
				{
					Log.Error("Unofficial OptionCategoryDef: " + optionCategoryDef.label + " found, ignoring");
				}
			}
			this.hasModSettings = LoadedModManager.ModHandles.Any((Mod mod) => !mod.SettingsCategory().NullOrEmpty());
		}

		// Token: 0x0600BD0E RID: 48398 RVA: 0x0043A7A0 File Offset: 0x004389A0
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			IEnumerable<OptionCategoryDef> allDefs = DefDatabase<OptionCategoryDef>.AllDefs;
			Text.Anchor = TextAnchor.MiddleLeft;
			int num = 0;
			foreach (OptionCategoryDef optionCategoryDef in allDefs)
			{
				if ((Prefs.DevMode || !optionCategoryDef.isDev) && optionCategoryDef.modContentPack.IsOfficialMod && (optionCategoryDef != OptionCategoryDefOf.Mods || this.hasModSettings))
				{
					Rect rect = new Rect(0f, (float)num * 50f, 160f, 48f);
					this.DoCategoryRow(rect.ContractedBy(4f), optionCategoryDef);
					num++;
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			float num2 = 60f;
			Rect inRect2 = new Rect(177f, 0f, inRect.width - 160f - 17f, inRect.height - num2);
			this.DoOptions(this.selectedCategory, inRect2);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			if (Widgets.ButtonText(new Rect(inRect.width / 2f - Dialog_Options.OkayButtonSize.x / 2f, inRect.yMax - Dialog_Options.OkayButtonSize.y, Dialog_Options.OkayButtonSize.x, Dialog_Options.OkayButtonSize.y), "OK".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600BD0F RID: 48399 RVA: 0x0043A924 File Offset: 0x00438B24
		private void DoCategoryRow(Rect r, OptionCategoryDef optionCategory)
		{
			Widgets.DrawOptionBackground(r, optionCategory == this.selectedCategory);
			if (Widgets.ButtonInvisible(r, true))
			{
				this.selectedCategory = optionCategory;
				this.selectedMod = null;
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			float num = r.x + 10f;
			Rect position = new Rect(num, r.y + (r.height - 20f) / 2f, 20f, 20f);
			Texture2D image = ContentFinder<Texture2D>.Get(optionCategory.texPath, true);
			GUI.DrawTexture(position, image);
			num += 30f;
			Widgets.Label(new Rect(num, r.y, r.width - num, r.height), optionCategory.label.CapitalizeFirst());
		}

		// Token: 0x0600BD10 RID: 48400 RVA: 0x0043A9E4 File Offset: 0x00438BE4
		private void DoOptions(OptionCategoryDef category, Rect inRect)
		{
			bool flag = this.optionsViewRectHeight > inRect.height;
			Rect outRect = new Rect(inRect);
			Rect viewRect = new Rect(outRect.x, outRect.y, outRect.width - (flag ? 26f : 0f), this.optionsViewRectHeight);
			Widgets.BeginScrollView(outRect, ref this.optionsScrollPosition, viewRect, true);
			Listing_Standard listing_Standard = new Listing_Standard();
			Rect rect = new Rect(viewRect.x, viewRect.y, viewRect.width, 999999f);
			listing_Standard.Begin(rect);
			listing_Standard.verticalSpacing = 5f;
			listing_Standard.Gap(12f);
			if (category == OptionCategoryDefOf.General)
			{
				this.DoGeneralOptions(listing_Standard);
			}
			else if (category == OptionCategoryDefOf.Graphics)
			{
				this.DoVideoOptions(listing_Standard);
			}
			else if (category == OptionCategoryDefOf.Audio)
			{
				this.DoAudioOptions(listing_Standard);
			}
			else if (category == OptionCategoryDefOf.Gameplay)
			{
				this.DoGameplayOptions(listing_Standard);
			}
			else if (category == OptionCategoryDefOf.Interface)
			{
				this.DoUIOptions(listing_Standard);
			}
			else if (category == OptionCategoryDefOf.Controls)
			{
				this.DoControlsOptions(listing_Standard);
			}
			else if (category == OptionCategoryDefOf.Dev)
			{
				this.DoDevOptions(listing_Standard);
			}
			else if (category == OptionCategoryDefOf.Mods)
			{
				this.DoModOptions(listing_Standard);
			}
			if (this.selectedMod != null)
			{
				float num = 15f;
				Rect inRect2 = new Rect(0f, num, viewRect.width, inRect.height - num);
				Mod mod = this.selectedMod;
				if (mod != null)
				{
					mod.DoSettingsWindowContents(inRect2);
				}
			}
			this.optionsViewRectHeight = listing_Standard.CurHeight;
			listing_Standard.End();
			Widgets.EndScrollView();
		}

		// Token: 0x0600BD11 RID: 48401 RVA: 0x0043AB6C File Offset: 0x00438D6C
		private void DoGeneralOptions(Listing_Standard listing)
		{
			if (listing.ButtonTextLabeledPct("ChooseLanguage".Translate(), LanguageDatabase.activeLanguage.DisplayName, 0.6f, TextAnchor.MiddleLeft, null, null, Dialog_Options.LanguageIconTex))
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					Messages.Message("ChangeLanguageFromMainMenu".Translate(), MessageTypeDefOf.RejectInput, false);
					SoundDefOf.Click.PlayOneShotOnCamera(null);
				}
				else
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (LoadedLanguage localLang2 in LanguageDatabase.AllLoadedLanguages)
					{
						LoadedLanguage localLang = localLang2;
						list.Add(new FloatMenuOption(localLang.DisplayName, delegate()
						{
							LanguageDatabase.SelectLanguage(localLang);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
			}
			float autosaveIntervalDays = Prefs.AutosaveIntervalDays;
			string text = "Days".Translate();
			string text2 = "day".Translate();
			if (listing.ButtonTextLabeledPct("AutosaveInterval".Translate(), autosaveIntervalDays.ToString() + " " + ((autosaveIntervalDays == 1f) ? text2 : text), 0.6f, TextAnchor.MiddleLeft, null, "AutosaveIntervalTooltip".Translate(), null))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				if (Prefs.DevMode)
				{
					list2.Add(new FloatMenuOption("0.05 " + text + "(debug)", delegate()
					{
						Prefs.AutosaveIntervalDays = 0.05f;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					list2.Add(new FloatMenuOption("0.075 " + text + "(debug)", delegate()
					{
						Prefs.AutosaveIntervalDays = 0.075f;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					list2.Add(new FloatMenuOption("0.1 " + text + "(debug)", delegate()
					{
						Prefs.AutosaveIntervalDays = 0.1f;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					list2.Add(new FloatMenuOption("0.125 " + text + "(debug)", delegate()
					{
						Prefs.AutosaveIntervalDays = 0.125f;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					list2.Add(new FloatMenuOption("0.25 " + text + "(debug)", delegate()
					{
						Prefs.AutosaveIntervalDays = 0.25f;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				list2.Add(new FloatMenuOption("0.5 " + text, delegate()
				{
					Prefs.AutosaveIntervalDays = 0.5f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				list2.Add(new FloatMenuOption(1.ToString() + " " + text2, delegate()
				{
					Prefs.AutosaveIntervalDays = 1f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				list2.Add(new FloatMenuOption(3.ToString() + " " + text, delegate()
				{
					Prefs.AutosaveIntervalDays = 3f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				list2.Add(new FloatMenuOption(7.ToString() + " " + text, delegate()
				{
					Prefs.AutosaveIntervalDays = 7f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				list2.Add(new FloatMenuOption(14.ToString() + " " + text, delegate()
				{
					Prefs.AutosaveIntervalDays = 14f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			if (Current.ProgramState == ProgramState.Playing && Current.Game.Info.permadeathMode && Prefs.AutosaveIntervalDays > 1f)
			{
				GUI.color = Color.red;
				listing.Label("MaxPermadeathAutosaveIntervalInfo".Translate(1f), -1f, null);
				GUI.color = Color.white;
			}
			int autosavesCount = Prefs.AutosavesCount;
			Prefs.AutosavesCount = Mathf.RoundToInt(listing.SliderLabeled("AutosavesCount".Translate(autosavesCount), (float)autosavesCount, 1f, 25f, 0.6f, null));
			bool runInBackground = Prefs.RunInBackground;
			listing.CheckboxLabeled("RunInBackground".Translate(), ref runInBackground, null, 30f, 0.6f);
			Prefs.RunInBackground = runInBackground;
			if (!DevModePermanentlyDisabledUtility.Disabled || Prefs.DevMode)
			{
				bool devMode = Prefs.DevMode;
				listing.CheckboxLabeled("DevelopmentMode".Translate(), ref devMode, null, 30f, 0.6f);
				Prefs.DevMode = devMode;
			}
			if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
			{
				listing.verticalSpacing = 0f;
				if (listing.ButtonTextLabeledPct("SaveGameDataFolder".Translate(), "OpenFolder".Translate(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
				{
					Application.OpenURL(GenFilePaths.SavedGamesFolderPath);
					SoundDefOf.Click.PlayOneShotOnCamera(null);
				}
				listing.SubLabel(GenFilePaths.SavedGamesFolderPath, 0.6f);
				listing.Gap(12f);
				if (listing.ButtonTextLabeledPct("LogFileFolder".Translate(), "OpenFolder".Translate(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
				{
					Application.OpenURL(Path.GetDirectoryName(Application.consoleLogPath));
					SoundDefOf.Click.PlayOneShotOnCamera(null);
				}
				listing.SubLabel(Path.GetDirectoryName(Application.consoleLogPath), 0.6f);
				listing.Gap(12f);
				listing.verticalSpacing = 5f;
			}
			else
			{
				if (listing.ButtonTextLabeledPct("SaveGameDataLocation".Translate(), "ShowFolder".Translate(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
				{
					Find.WindowStack.Add(new Dialog_MessageBox(Path.GetFullPath(GenFilePaths.SaveDataFolderPath), null, null, null, null, null, false, null, null, WindowLayer.Dialog));
				}
				if (listing.ButtonTextLabeledPct("LogFileLocation".Translate(), "ShowFolder".Translate(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
				{
					Find.WindowStack.Add(new Dialog_MessageBox(Application.consoleLogPath, null, null, null, null, null, false, null, null, WindowLayer.Dialog));
				}
			}
			if (listing.ButtonTextLabeledPct("RestoreToDefaultSettingsLabel".Translate(), "RestoreToDefaultSettings".Translate(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				WindowStack windowStack = Find.WindowStack;
				TaggedString text3 = "ResetAndRestartConfirmationDialog".Translate();
				Action buttonAAction = new Action(this.RestoreToDefaultSettings);
				windowStack.Add(new Dialog_MessageBox(text3, "Yes".Translate(), buttonAAction, "No".Translate(), null, null, false, null, null, WindowLayer.Dialog));
			}
		}

		// Token: 0x0600BD12 RID: 48402 RVA: 0x0043B304 File Offset: 0x00439504
		private static string MonitorLabel(string id)
		{
			MonitorInfo[] monitors = EngineWindow.Monitors;
			for (int i = 0; i < monitors.Length; i++)
			{
				if (monitors[i].Id == id)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler.AppendFormatted<int>(i + 1);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted(monitors[i].Name);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
			}
			return "Monitor_Automatic".Translate();
		}

		// Token: 0x0600BD13 RID: 48403 RVA: 0x0043B382 File Offset: 0x00439582
		private static void SetPreferredMonitor(string id)
		{
			Prefs.PreferredMonitorId = id;
			EngineWindow.SetPreferredMonitor(id.NullOrEmpty() ? null : id);
		}

		// Token: 0x1700226B RID: 8811
		// (get) Token: 0x0600BD14 RID: 48404 RVA: 0x0043B39B File Offset: 0x0043959B
		private static List<GpuDescription> SelectableGpus
		{
			get
			{
				return (from g in GraphicsDevice.EnumeratedGpus
				where g.Usable
				select g).ToList<GpuDescription>();
			}
		}

		// Token: 0x0600BD15 RID: 48405 RVA: 0x0043B3CC File Offset: 0x004395CC
		private static string GpuLabel(GpuDescription gpu, List<GpuDescription> gpus)
		{
			if (gpu == null)
			{
				return "Gpu_Automatic".Translate();
			}
			string text;
			if (gpus.Count((GpuDescription g) => g.Name == gpu.Name) <= 1)
			{
				text = gpu.Name;
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted(gpu.Name);
				defaultInterpolatedStringHandler.AppendLiteral(" #");
				defaultInterpolatedStringHandler.AppendFormatted<int>((from g in gpus
				where g.Name == gpu.Name
				select g).ToList<GpuDescription>().IndexOf(gpu) + 1);
				text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			string text2 = text;
			if (!gpu.IsActive)
			{
				return text2;
			}
			return "Gpu_InUse".Translate(text2);
		}

		// Token: 0x0600BD16 RID: 48406 RVA: 0x0043B4A0 File Offset: 0x004396A0
		private static GpuDescription PreferredGpu(List<GpuDescription> gpus)
		{
			if (Prefs.PreferredGpuUuid.NullOrEmpty() && Prefs.PreferredGpuName.NullOrEmpty())
			{
				return null;
			}
			GpuDescription result;
			if ((result = gpus.FirstOrDefault((GpuDescription g) => g.Uuid.EqualsIgnoreCase(Prefs.PreferredGpuUuid))) == null)
			{
				result = gpus.FirstOrDefault((GpuDescription g) => g.Name.EqualsIgnoreCase(Prefs.PreferredGpuName));
			}
			return result;
		}

		// Token: 0x0600BD17 RID: 48407 RVA: 0x0043B518 File Offset: 0x00439718
		private static void SetPreferredGpu(GpuDescription gpu, List<GpuDescription> gpus)
		{
			Prefs.PreferredGpuName = (((gpu != null) ? gpu.Name : null) ?? "");
			Prefs.PreferredGpuUuid = (((gpu != null) ? gpu.Uuid : null) ?? "");
			GpuDescription gpuDescription = gpu;
			if (gpu == null)
			{
				gpuDescription = gpus.FirstOrDefault((GpuDescription g) => g.IsAutomaticChoice);
			}
			GpuDescription gpuDescription2 = gpuDescription;
			if (gpuDescription2 != null && !gpuDescription2.IsActive)
			{
				Find.WindowStack.Add(new Dialog_MessageBox("GpuChangedRestart".Translate(), "Yes".Translate(), delegate()
				{
					Prefs.Save();
					GenCommandLine.Restart();
				}, "No".Translate(), null, null, false, null, null, WindowLayer.Dialog));
			}
		}

		// Token: 0x0600BD18 RID: 48408 RVA: 0x0043B5EC File Offset: 0x004397EC
		private static string DisplayModeLabel(FullScreenMode mode)
		{
			TaggedString taggedString;
			if (mode != FullScreenMode.ExclusiveFullScreen)
			{
				if (mode != FullScreenMode.Windowed)
				{
					taggedString = "BorderlessFullscreen".Translate();
				}
				else
				{
					taggedString = "DisplayMode_Windowed".Translate();
				}
			}
			else
			{
				taggedString = "DisplayMode_ExclusiveFullscreen".Translate();
			}
			return taggedString;
		}

		// Token: 0x0600BD19 RID: 48409 RVA: 0x0043B630 File Offset: 0x00439830
		private void DoVideoOptions(Listing_Standard listing)
		{
			if (listing.ButtonTextLabeledPct("Resolution".Translate(), Dialog_Options.ResToString(Screen.width, Screen.height), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				using (IEnumerator<UnityEngine.Resolution> enumerator = (from x in UnityGUIBugsFixer.ScreenResolutionsWithoutDuplicates
				where x.width >= 1024 && x.height >= 768
				orderby x.width, x.height
				select x).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UnityEngine.Resolution res = enumerator.Current;
						list.Add(new FloatMenuOption(Dialog_Options.ResToString(res.width, res.height), delegate()
						{
							if (!ResolutionUtility.UIScaleSafeWithResolution(Prefs.UIScalePreference, res.width, res.height))
							{
								Messages.Message("MessageScreenResTooSmallForUIScale".Translate(), MessageTypeDefOf.RejectInput, false);
								return;
							}
							ResolutionUtility.SafeSetResolution(res);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				if (!list.Any<FloatMenuOption>())
				{
					list.Add(new FloatMenuOption("NoneBrackets".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			if (listing.ButtonTextLabeledPct("DisplayMode".Translate(), Dialog_Options.DisplayModeLabel(Screen.fullScreenMode), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				FullScreenMode[] array = new FullScreenMode[3];
				array[0] = FullScreenMode.Windowed;
				array[1] = FullScreenMode.FullScreenWindow;
				FullScreenMode[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					FullScreenMode mode = array2[i];
					list2.Add(new FloatMenuOption(Dialog_Options.DisplayModeLabel(mode), delegate()
					{
						ResolutionUtility.SafeSetFullScreenMode(mode);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			if (EngineWindow.Monitors.Length > 1 && listing.ButtonTextLabeledPct("Monitor".Translate(), Dialog_Options.MonitorLabel(Prefs.PreferredMonitorId), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list3 = new List<FloatMenuOption>();
				list3.Add(new FloatMenuOption(Dialog_Options.MonitorLabel(""), delegate()
				{
					Dialog_Options.SetPreferredMonitor("");
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				List<FloatMenuOption> list4 = list3;
				MonitorInfo[] monitors = EngineWindow.Monitors;
				for (int i = 0; i < monitors.Length; i++)
				{
					MonitorInfo monitorInfo = monitors[i];
					string id = monitorInfo.Id;
					list4.Add(new FloatMenuOption(Dialog_Options.MonitorLabel(id), delegate()
					{
						Dialog_Options.SetPreferredMonitor(id);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list4));
			}
			List<GpuDescription> gpus = Dialog_Options.SelectableGpus;
			if (gpus.Count > 1 && listing.ButtonTextLabeledPct("Gpu".Translate(), Dialog_Options.GpuLabel(Dialog_Options.PreferredGpu(gpus), gpus), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list5 = new List<FloatMenuOption>
				{
					new FloatMenuOption(Dialog_Options.GpuLabel(null, gpus), delegate()
					{
						Dialog_Options.SetPreferredGpu(null, gpus);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0)
				};
				using (List<GpuDescription>.Enumerator enumerator2 = gpus.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						GpuDescription gpu = enumerator2.Current;
						list5.Add(new FloatMenuOption(Dialog_Options.GpuLabel(gpu, gpus), delegate()
						{
							Dialog_Options.SetPreferredGpu(gpu, gpus);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list5));
			}
			Rect rect = listing.GetRect(30f, 0.6f);
			rect.width = Math.Min(rect.width + 24f, listing.ColumnWidth);
			TooltipHandler.TipRegion(rect, "TextureCompressionAlwaysOn_Tooltip".Translate());
			bool flag = true;
			Widgets.CheckboxLabeled(rect, "TextureCompression".Translate(), ref flag, true, null, null, false, false);
			listing.Gap(listing.verticalSpacing);
			bool plantWindSway = Prefs.PlantWindSway;
			listing.CheckboxLabeled("PlantWindSway".Translate(), ref plantWindSway, null, 30f, 0.6f);
			Prefs.PlantWindSway = plantWindSway;
			float screenShakeIntensity = Prefs.ScreenShakeIntensity;
			Prefs.ScreenShakeIntensity = (float)Math.Round((double)listing.SliderLabeled(string.Format("{0}: {1}", "ScreenShakeIntensity".Translate(), screenShakeIntensity.ToStringPercent("N0")), screenShakeIntensity, 0f, 2f, 0.6f, null), 1);
			bool smoothCameraJumps = Prefs.SmoothCameraJumps;
			listing.CheckboxLabeled("SmoothCameraJumps".Translate(), ref smoothCameraJumps, "SmoothCameraJumpsDesc".Translate(), 30f, 0.6f);
			Prefs.SmoothCameraJumps = smoothCameraJumps;
			bool gravshipCutscenes = Prefs.GravshipCutscenes;
			listing.CheckboxLabeled("GravshipCutscenes".Translate(), ref gravshipCutscenes, "GravshipCutscenesDesc".Translate(), 30f, 0.6f);
			Prefs.GravshipCutscenes = gravshipCutscenes;
		}

		// Token: 0x0600BD1A RID: 48410 RVA: 0x0043BBF0 File Offset: 0x00439DF0
		private void DoAudioOptions(Listing_Standard listing)
		{
			Prefs.VolumeMaster = listing.SliderLabeled(string.Format("{0}: {1}", "MasterVolume".Translate(), Prefs.VolumeMaster.ToStringPercent()), Prefs.VolumeMaster, 0f, 1f, 0.6f, "MasterVolumeTooltip".Translate());
			Prefs.VolumeGame = listing.SliderLabeled(string.Format("{0}: {1}", "GameVolume".Translate(), Prefs.VolumeGame.ToStringPercent()), Prefs.VolumeGame, 0f, 1f, 0.6f, "GameVolumeTooltip".Translate());
			Prefs.VolumeMusic = listing.SliderLabeled(string.Format("{0}: {1}", "MusicVolume".Translate(), Prefs.VolumeMusic.ToStringPercent()), Prefs.VolumeMusic, 0f, 1f, 0.6f, "MusicVolumeTooltip".Translate());
			Prefs.VolumeAmbient = listing.SliderLabeled(string.Format("{0}: {1}", "AmbientVolume".Translate(), Prefs.VolumeAmbient.ToStringPercent()), Prefs.VolumeAmbient, 0f, 1f, 0.6f, "AmbientVolumeTooltip".Translate());
			Prefs.VolumeUI = listing.SliderLabeled(string.Format("{0}: {1}", "UIVolume".Translate(), Prefs.VolumeUI.ToStringPercent()), Prefs.VolumeUI, 0f, 1f, 0.6f, "UIVolumeTooltip".Translate());
		}

		// Token: 0x0600BD1B RID: 48411 RVA: 0x0043BD94 File Offset: 0x00439F94
		private void DoGameplayOptions(Listing_Standard listing)
		{
			if (Current.ProgramState == ProgramState.Playing && listing.ButtonTextLabeledPct("ChangeStoryteller".Translate(), "Modify".Translate(), 0.6f, TextAnchor.UpperLeft, "OptionsButton-ChooseStoryteller", null, null) && TutorSystem.AllowAction("ChooseStoryteller"))
			{
				Find.WindowStack.Add(new Page_SelectStorytellerInGame());
			}
			int maxNumberOfPlayerSettlements = Prefs.MaxNumberOfPlayerSettlements;
			int num = Mathf.RoundToInt(listing.SliderLabeled("MaxNumberOfPlayerSettlements".Translate(maxNumberOfPlayerSettlements), (float)maxNumberOfPlayerSettlements, 1f, 5f, 0.6f, null));
			Prefs.MaxNumberOfPlayerSettlements = num;
			if (maxNumberOfPlayerSettlements != num && num > 1)
			{
				TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.MaxNumberOfPlayerSettlements, Array.Empty<string>());
			}
			bool pauseOnLoad = Prefs.PauseOnLoad;
			listing.CheckboxLabeled("PauseOnLoad".Translate(), ref pauseOnLoad, null, 30f, 0.6f);
			Prefs.PauseOnLoad = pauseOnLoad;
			AutomaticPauseMode automaticPauseMode = Prefs.AutomaticPauseMode;
			if (listing.ButtonTextLabeledPct("AutomaticPauseModeSetting".Translate(), Prefs.AutomaticPauseMode.ToStringHuman(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (object obj in Enum.GetValues(typeof(AutomaticPauseMode)))
				{
					AutomaticPauseMode localPmode2 = (AutomaticPauseMode)obj;
					AutomaticPauseMode localPmode = localPmode2;
					list.Add(new FloatMenuOption(localPmode.ToStringHuman(), delegate()
					{
						Prefs.AutomaticPauseMode = localPmode;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			Prefs.AutomaticPauseMode = automaticPauseMode;
			bool adaptiveTrainingEnabled = Prefs.AdaptiveTrainingEnabled;
			listing.CheckboxLabeled("LearningHelper".Translate(), ref adaptiveTrainingEnabled, "LearningHelperTooltip".Translate(), 30f, 0.6f);
			Prefs.AdaptiveTrainingEnabled = adaptiveTrainingEnabled;
			if (listing.ButtonTextLabeledPct("ResetAdaptiveTutor".Translate(), "Reset".Translate(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmResetLearningHelper".Translate(), delegate()
				{
					Messages.Message("AdaptiveTutorIsReset".Translate(), MessageTypeDefOf.TaskCompletion, false);
					PlayerKnowledgeDatabase.ResetPersistent();
				}, true, null, WindowLayer.Dialog));
			}
			listing.verticalSpacing = 0f;
			if (Prefs.PreferredNames.Count < 6)
			{
				if (listing.ButtonTextLabeledPct("NamesYouWantToSee".Translate(), "AddName".Translate() + "...", 0.6f, TextAnchor.MiddleLeft, null, null, null))
				{
					Find.WindowStack.Add(new Dialog_AddPreferredName());
				}
			}
			else
			{
				listing.Label("NamesYouWantToSee".Translate(), -1f, null);
			}
			listing.SubLabel("NamesYouWantToSeeSubText".Translate(), 0.6f);
			listing.verticalSpacing = 5f;
			Prefs.PreferredNames.RemoveAll((string n) => n.NullOrEmpty());
			float num2 = 20f;
			listing.Indent(num2);
			for (int i = 0; i < Prefs.PreferredNames.Count; i++)
			{
				string name = Prefs.PreferredNames[i];
				PawnBio pawnBio = (from b in SolidBioDatabase.allBios
				where b.name.ToString() == name
				select b).FirstOrDefault<PawnBio>();
				if (pawnBio == null)
				{
					name += " [N]";
				}
				else
				{
					PawnBioType bioType = pawnBio.BioType;
					if (bioType != PawnBioType.BackstoryInGame)
					{
						if (bioType == PawnBioType.PirateKing)
						{
							name += " [PK]";
						}
					}
					else
					{
						name += " [B]";
					}
				}
				Rect rect = listing.GetRect(24f, 1f);
				rect.width -= num2;
				if (i % 2 == 0)
				{
					Widgets.DrawLightHighlight(rect);
				}
				Widgets.Label(new Rect(rect.x + 4f, rect.y, rect.width - 4f, rect.height), name);
				if (Widgets.ButtonImage(new Rect(rect.xMax - 24f, rect.y, 24f, 24f), TexButton.Delete, Color.white, GenUI.SubtleMouseoverColor, true, null))
				{
					Prefs.PreferredNames.RemoveAt(i);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
			}
			listing.Outdent(num2);
			listing.Gap(12f);
		}

		// Token: 0x0600BD1C RID: 48412 RVA: 0x0043C274 File Offset: 0x0043A474
		private void DoUIOptions(Listing_Standard listing)
		{
			if (listing.ButtonTextLabeledPct("UIScale".Translate(), Prefs.UIScalePreference.ToString() + "x", 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				for (int i = 0; i < Dialog_Options.UIScales.Length; i++)
				{
					float scale = Dialog_Options.UIScales[i];
					list.Add(new FloatMenuOption(Dialog_Options.UIScales[i].ToString() + "x", delegate()
					{
						if (scale > 1f && !ResolutionUtility.UIScaleSafeWithResolution(scale, Screen.width, Screen.height))
						{
							Messages.Message("MessageScreenResTooSmallForUIScale".Translate(), MessageTypeDefOf.RejectInput, false);
							return;
						}
						ResolutionUtility.SafeSetUIScale(scale);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			bool respectSystemScaling = Prefs.RespectSystemScaling;
			listing.CheckboxLabeled("RespectSystemScaling".Translate(), ref respectSystemScaling, "RespectSystemScaling_Tooltip".Translate(), 30f, 0.6f);
			Prefs.RespectSystemScaling = respectSystemScaling;
			if (listing.ButtonTextLabeledPct("TemperatureMode".Translate(), Prefs.TemperatureMode.ToStringHuman(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				foreach (object obj in Enum.GetValues(typeof(TemperatureDisplayMode)))
				{
					TemperatureDisplayMode localTmode2 = (TemperatureDisplayMode)obj;
					TemperatureDisplayMode localTmode = localTmode2;
					list2.Add(new FloatMenuOption(localTmode.ToStringHuman(), delegate()
					{
						Prefs.TemperatureMode = localTmode;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			if (listing.ButtonTextLabeledPct("ShowWeaponsUnderPortrait".Translate(), Prefs.ShowWeaponsUnderPortraitMode.ToStringHuman(), 0.6f, TextAnchor.MiddleLeft, null, "ShowWeaponsUnderPortraitTooltip".Translate(), null))
			{
				List<FloatMenuOption> list3 = new List<FloatMenuOption>();
				using (IEnumerator enumerator = Enum.GetValues(typeof(ShowWeaponsUnderPortraitMode)).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ShowWeaponsUnderPortraitMode mode = (ShowWeaponsUnderPortraitMode)enumerator.Current;
						list3.Add(new FloatMenuOption(mode.ToStringHuman(), delegate()
						{
							Prefs.ShowWeaponsUnderPortraitMode = mode;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list3));
			}
			if (listing.ButtonTextLabeledPct("ShowAnimalNames".Translate(), Prefs.AnimalNameMode.ToStringHuman(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list4 = new List<FloatMenuOption>();
				foreach (object obj2 in Enum.GetValues(typeof(AnimalNameDisplayMode)))
				{
					AnimalNameDisplayMode localMode5 = (AnimalNameDisplayMode)obj2;
					AnimalNameDisplayMode localMode = localMode5;
					list4.Add(new FloatMenuOption(localMode.ToStringHuman(), delegate()
					{
						Prefs.AnimalNameMode = localMode;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list4));
			}
			if (ModsConfig.BiotechActive && listing.ButtonTextLabeledPct("ShowMechNames".Translate(), Prefs.MechNameMode.ToStringHuman(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list5 = new List<FloatMenuOption>();
				foreach (object obj3 in Enum.GetValues(typeof(MechNameDisplayMode)))
				{
					MechNameDisplayMode localMode2 = (MechNameDisplayMode)obj3;
					MechNameDisplayMode localMode = localMode2;
					list5.Add(new FloatMenuOption(localMode.ToStringHuman(), delegate()
					{
						Prefs.MechNameMode = localMode;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list5));
			}
			if (listing.ButtonTextLabeledPct("DotHighlightDisplayMode".Translate(), Prefs.DotHighlightDisplayMode.ToStringHuman(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list6 = new List<FloatMenuOption>();
				foreach (object obj4 in Enum.GetValues(typeof(DotHighlightDisplayMode)))
				{
					DotHighlightDisplayMode localMode3 = (DotHighlightDisplayMode)obj4;
					DotHighlightDisplayMode localMode = localMode3;
					list6.Add(new FloatMenuOption(localMode.ToStringHuman(), delegate()
					{
						Prefs.DotHighlightDisplayMode = localMode;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list6));
			}
			bool flag = Prefs.DotHighlightDisplayMode > DotHighlightDisplayMode.None;
			if (!flag)
			{
				GUI.color = Color.gray;
			}
			if (listing.ButtonTextLabeledPct("HighlightStyleMode".Translate(), Prefs.HighlightStyleMode.ToStringHuman(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list7 = new List<FloatMenuOption>();
				foreach (object obj5 in Enum.GetValues(typeof(HighlightStyleMode)))
				{
					HighlightStyleMode localMode4 = (HighlightStyleMode)obj5;
					HighlightStyleMode localMode = localMode4;
					list7.Add(new FloatMenuOption(localMode.ToStringHuman(), delegate()
					{
						Prefs.HighlightStyleMode = localMode;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				if (flag)
				{
					Find.WindowStack.Add(new FloatMenu(list7));
				}
			}
			if (!flag)
			{
				GUI.color = Color.white;
			}
			TaggedString taggedString = Prefs.RandomBackgroundImage ? "Random".Translate() : Prefs.BackgroundImageExpansion.LabelCap;
			if ((from e in ModLister.AllExpansions
			where !e.isCore && e.Status != ExpansionStatus.NotInstalled
			select e).Any<ExpansionDef>() && listing.ButtonTextLabeledPct("SetBackgroundImage".Translate(), taggedString, 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list8 = new List<FloatMenuOption>();
				foreach (ExpansionDef expansionDef in ModLister.AllExpansions)
				{
					if (expansionDef.Status != ExpansionStatus.NotInstalled)
					{
						ExpansionDef localExpansion = expansionDef;
						list8.Add(new FloatMenuOption(expansionDef.label, delegate()
						{
							Prefs.BackgroundImageExpansion = localExpansion;
							Prefs.RandomBackgroundImage = false;
						}, expansionDef.Icon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
					}
				}
				list8.Add(new FloatMenuOption("Random".Translate(), delegate()
				{
					Prefs.RandomBackgroundImage = true;
					((UI_BackgroundMain)UIMenuBackgroundManager.background).overrideBGImage = (from exp in ModLister.AllExpansions
					where exp.Status == ExpansionStatus.Active
					select exp).RandomElement<ExpansionDef>().BackgroundImage;
				}, Dialog_Options.RandomBackgroundTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
				Find.WindowStack.Add(new FloatMenu(list8));
			}
			bool showRealtimeClock = Prefs.ShowRealtimeClock;
			listing.CheckboxLabeled("ShowRealtimeClock".Translate(), ref showRealtimeClock, null, 30f, 0.6f);
			Prefs.ShowRealtimeClock = showRealtimeClock;
			bool twelveHourClockMode = Prefs.TwelveHourClockMode;
			listing.CheckboxLabeled("TwelveHourClockMode".Translate(), ref twelveHourClockMode, null, 30f, 0.6f);
			Prefs.TwelveHourClockMode = twelveHourClockMode;
			bool hatsOnlyOnMap = Prefs.HatsOnlyOnMap;
			listing.CheckboxLabeled("HatsShownOnlyOnMap".Translate(), ref hatsOnlyOnMap, null, 30f, 0.6f);
			if (hatsOnlyOnMap != Prefs.HatsOnlyOnMap)
			{
				PortraitsCache.Clear();
			}
			Prefs.HatsOnlyOnMap = hatsOnlyOnMap;
			if (!SteamDeck.IsSteamDeck)
			{
				bool disableTinyText = Prefs.DisableTinyText;
				listing.CheckboxLabeled("DisableTinyText".Translate(), ref disableTinyText, null, 30f, 0.6f);
				if (Prefs.DisableTinyText != disableTinyText)
				{
					Prefs.DisableTinyText = disableTinyText;
					Widgets.ClearLabelCache();
					GenUI.ClearLabelWidthCache();
					if (Current.ProgramState == ProgramState.Playing)
					{
						Find.ColonistBar.drawer.ClearLabelCache();
					}
				}
			}
			bool flag2 = !Prefs.CustomCursorEnabled;
			listing.CheckboxLabeled("CustomCursor".Translate(), ref flag2, null, 30f, 0.6f);
			Prefs.CustomCursorEnabled = !flag2;
			bool visibleMood = Prefs.VisibleMood;
			listing.CheckboxLabeled("VisibleMood".Translate(), ref visibleMood, "VisibleMoodDesc".Translate(), 30f, 0.6f);
			Prefs.VisibleMood = visibleMood;
		}

		// Token: 0x0600BD1D RID: 48413 RVA: 0x0043CBB0 File Offset: 0x0043ADB0
		private void DoControlsOptions(Listing_Standard listing)
		{
			if (!SteamDeck.IsSteamDeckInNonKeyboardMode && listing.ButtonTextLabeledPct("KeyboardConfig".Translate(), "ModifyConfig".Translate(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				Find.WindowStack.Add(new Dialog_KeyBindings());
			}
			if (SteamDeck.IsSteamDeck && listing.ButtonTextLabeledPct("ControllerConfig".Translate(), "ModifyConfig".Translate(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				SteamDeck.ShowConfigPage();
			}
			if (SteamDeck.IsSteamDeck)
			{
				bool steamDeckKeyboardMode = Prefs.SteamDeckKeyboardMode;
				listing.CheckboxLabeled("SteamDeckKeyboardMode".Translate(), ref steamDeckKeyboardMode, null, 30f, 0.6f);
				Prefs.SteamDeckKeyboardMode = steamDeckKeyboardMode;
			}
			float mapDragSensitivity = Prefs.MapDragSensitivity;
			Prefs.MapDragSensitivity = (float)Math.Round((double)listing.SliderLabeled("MapDragSensitivity".Translate() + ": " + mapDragSensitivity.ToStringPercent("F0"), mapDragSensitivity, 0.8f, 2.5f, 0.6f, null), 2);
			bool edgeScreenScroll = Prefs.EdgeScreenScroll;
			listing.CheckboxLabeled("EdgeScreenScroll".Translate(), ref edgeScreenScroll, null, 30f, 0.6f);
			Prefs.EdgeScreenScroll = edgeScreenScroll;
			bool zoomToMouse = Prefs.ZoomToMouse;
			listing.CheckboxLabeled("ZoomToMouse".Translate(), ref zoomToMouse, null, 30f, 0.6f);
			Prefs.ZoomToMouse = zoomToMouse;
			bool zoomSwitchWorldLayer = Prefs.ZoomSwitchWorldLayer;
			listing.CheckboxLabeled("ZoomSwitchLayer".Translate(), ref zoomSwitchWorldLayer, "ZoomSwitchLayer_Tooltip".Translate(), 30f, 0.6f);
			Prefs.ZoomSwitchWorldLayer = zoomSwitchWorldLayer;
			bool rememberDrawStlyes = Prefs.RememberDrawStlyes;
			listing.CheckboxLabeled("RememberDrawStyle".Translate(), ref rememberDrawStlyes, "RememberDrawStyle_Tooltip".Translate(), 30f, 0.6f);
			Prefs.RememberDrawStlyes = rememberDrawStlyes;
		}

		// Token: 0x0600BD1E RID: 48414 RVA: 0x0043CDA0 File Offset: 0x0043AFA0
		private void DoDevOptions(Listing_Standard listing)
		{
			bool testMapSizes = Prefs.TestMapSizes;
			listing.CheckboxLabeled("EnableTestMapSizes".Translate(), ref testMapSizes, null, 30f, 0.6f);
			Prefs.TestMapSizes = testMapSizes;
			bool logVerbose = Prefs.LogVerbose;
			listing.CheckboxLabeled("LogVerbose".Translate(), ref logVerbose, null, 30f, 0.6f);
			Prefs.LogVerbose = logVerbose;
			bool resetModsConfigOnCrash = Prefs.ResetModsConfigOnCrash;
			listing.CheckboxLabeled("ResetModsConfigOnCrash".Translate(), ref resetModsConfigOnCrash, null, 30f, 0.6f);
			Prefs.ResetModsConfigOnCrash = resetModsConfigOnCrash;
			bool disableQuickStartCryptoSickness = Prefs.DisableQuickStartCryptoSickness;
			listing.CheckboxLabeled("DisableQuickStartCryptoSickness".Translate(), ref disableQuickStartCryptoSickness, null, 30f, 0.6f);
			Prefs.DisableQuickStartCryptoSickness = disableQuickStartCryptoSickness;
			bool startDevPaletteOn = Prefs.StartDevPaletteOn;
			listing.CheckboxLabeled("StartDevPaletteOn".Translate(), ref startDevPaletteOn, null, 30f, 0.6f);
			Prefs.StartDevPaletteOn = startDevPaletteOn;
			bool openLogOnWarnings = Prefs.OpenLogOnWarnings;
			listing.CheckboxLabeled("OpenLogOnWarnings".Translate(), ref openLogOnWarnings, null, 30f, 0.6f);
			Prefs.OpenLogOnWarnings = openLogOnWarnings;
			bool closeLogWindowOnEscape = Prefs.CloseLogWindowOnEscape;
			listing.CheckboxLabeled("CloseLogWindowOnEscape".Translate(), ref closeLogWindowOnEscape, null, 30f, 0.6f);
			Prefs.CloseLogWindowOnEscape = closeLogWindowOnEscape;
			if (!DevModePermanentlyDisabledUtility.Disabled && listing.ButtonTextLabeledPct("PermanentlyDisableDevMode".Translate(), "Disable".Translate(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmPermanentlyDisableDevMode".Translate(), new Action(DevModePermanentlyDisabledUtility.Disable), true, null, WindowLayer.Dialog));
			}
		}

		// Token: 0x0600BD1F RID: 48415 RVA: 0x0043CF54 File Offset: 0x0043B154
		private void DoModOptions(Listing_Standard listing)
		{
			Rect rect = listing.GetRect(30f, 1f);
			this.quickSearchWidget.OnGUI(rect, null, null);
			this.modFilter = this.quickSearchWidget.filter.Text;
			listing.Gap(12f);
			Rect rect2 = default(Rect);
			int num = 0;
			foreach (Mod mod in this.cachedModsWithSettings)
			{
				if (mod.SettingsCategory().ToLower().Contains(this.modFilter.ToLower()) || mod.Content.Name.ToLower().Contains(this.modFilter.ToLower()))
				{
					if (num % 2 == 0)
					{
						rect2 = listing.GetRect(40f, 1f);
					}
					Rect rect3 = ((num % 2 == 0) ? rect2.LeftHalf() : rect2.RightHalf()).ContractedBy(2f);
					Text.Anchor = TextAnchor.MiddleLeft;
					Widgets.DrawOptionBackground(rect3, false);
					if (Widgets.ButtonInvisible(rect3, true))
					{
						Find.WindowStack.Add(new Dialog_ModSettings(mod));
					}
					if (Mouse.IsOver(rect3) && Text.CalcSize(mod.SettingsCategory()).x > rect3.width - rect3.height)
					{
						TooltipHandler.TipRegion(rect3, mod.SettingsCategory());
					}
					if (!mod.Content.ModMetaData.Icon.NullOrBad())
					{
						GUI.DrawTexture(new Rect(rect3.x, rect3.y, rect3.height, rect3.height).ContractedBy(2f), mod.Content.ModMetaData.Icon);
					}
					rect3.xMin += rect3.height;
					Widgets.Label(rect3, mod.SettingsCategory().Truncate(rect3.width, this.modOptionTruncationCache));
					Text.Anchor = TextAnchor.UpperLeft;
					num++;
				}
			}
		}

		// Token: 0x0600BD20 RID: 48416 RVA: 0x0043D170 File Offset: 0x0043B370
		public override void PreClose()
		{
			base.PreClose();
			Prefs.Save();
			if (this.selectedMod != null)
			{
				this.selectedMod.WriteSettings();
			}
		}

		// Token: 0x0600BD21 RID: 48417 RVA: 0x0043D190 File Offset: 0x0043B390
		public void RestoreToDefaultSettings()
		{
			foreach (FileInfo fileInfo in new DirectoryInfo(GenFilePaths.ConfigFolderPath).GetFiles("*.xml"))
			{
				try
				{
					fileInfo.Delete();
				}
				catch (SystemException)
				{
				}
			}
			Find.WindowStack.Add(new Dialog_MessageBox("ResetAndRestart".Translate(), null, new Action(GenCommandLine.Restart), null, null, null, false, null, null, WindowLayer.Dialog));
		}

		// Token: 0x0600BD22 RID: 48418 RVA: 0x0043D20C File Offset: 0x0043B40C
		public static string ResToString(int width, int height)
		{
			string text = width.ToString() + "x" + height.ToString();
			if (width == 1280 && height == 720)
			{
				text += " (720p)";
			}
			if (width == 1920 && height == 1080)
			{
				text += " (1080p)";
			}
			return text;
		}

		// Token: 0x04006C0B RID: 27659
		public OptionCategoryDef selectedCategory = OptionCategoryDefOf.General;

		// Token: 0x04006C0C RID: 27660
		public Mod selectedMod;

		// Token: 0x04006C0D RID: 27661
		private bool hasModSettings;

		// Token: 0x04006C0E RID: 27662
		private Vector2 categoryScrollPosition;

		// Token: 0x04006C0F RID: 27663
		private Vector2 optionsScrollPosition;

		// Token: 0x04006C10 RID: 27664
		private float optionsViewRectHeight;

		// Token: 0x04006C11 RID: 27665
		private QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x04006C12 RID: 27666
		private string modFilter = "";

		// Token: 0x04006C13 RID: 27667
		private IEnumerable<Mod> cachedModsWithSettings;

		// Token: 0x04006C14 RID: 27668
		private const float CategoryListWidth = 160f;

		// Token: 0x04006C15 RID: 27669
		private const float CategoryRowHeight = 48f;

		// Token: 0x04006C16 RID: 27670
		private const float CategoryRowSpacing = 2f;

		// Token: 0x04006C17 RID: 27671
		private const float CategoryIconSize = 20f;

		// Token: 0x04006C18 RID: 27672
		private const float BottomRowHeight = 50f;

		// Token: 0x04006C19 RID: 27673
		private const float ListingVerticalSpacing = 5f;

		// Token: 0x04006C1A RID: 27674
		private const float SearchBarHeight = 30f;

		// Token: 0x04006C1B RID: 27675
		private const float ModIconSize = 32f;

		// Token: 0x04006C1C RID: 27676
		private static readonly Vector2 OkayButtonSize = new Vector2(160f, 40f);

		// Token: 0x04006C1D RID: 27677
		public static readonly float[] UIScales = new float[]
		{
			0.75f,
			1f,
			1.25f,
			1.5f,
			1.75f,
			2f,
			2.5f,
			3f,
			3.5f,
			4f
		};

		// Token: 0x04006C1E RID: 27678
		public static readonly Texture2D RandomBackgroundTex = ContentFinder<Texture2D>.Get("UI/Icons/RandomBackground", true);

		// Token: 0x04006C1F RID: 27679
		public static readonly Texture2D LanguageIconTex = ContentFinder<Texture2D>.Get("UI/Icons/Language", true);

		// Token: 0x04006C20 RID: 27680
		private Dictionary<string, string> modOptionTruncationCache = new Dictionary<string, string>();
	}
}
