using System;
using System.IO;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x02000518 RID: 1304
	public static class GameDataSaveLoader
	{
		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x060026A2 RID: 9890 RVA: 0x000EFA3A File Offset: 0x000EDC3A
		public static bool CurrentGameStateIsValuable
		{
			get
			{
				return Find.TickManager.TicksGame > GameDataSaveLoader.lastSaveTick + 60;
			}
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x060026A3 RID: 9891 RVA: 0x000EFA50 File Offset: 0x000EDC50
		public static bool SavingIsTemporarilyDisabled
		{
			get
			{
				return Find.TilePicker.Active || Find.WindowStack.WindowsPreventSave || WorldComponent_GravshipController.CutsceneInProgress;
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x060026A4 RID: 9892 RVA: 0x000EFA71 File Offset: 0x000EDC71
		public static bool IsSavingOrLoadingExternalIdeo
		{
			get
			{
				return GameDataSaveLoader.isSavingOrLoadingExternalIdeo;
			}
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x000EFA78 File Offset: 0x000EDC78
		public static void SaveScenario(Scenario scen, string absFilePath)
		{
			try
			{
				scen.fileName = Path.GetFileNameWithoutExtension(absFilePath);
				SafeSaver.Save(absFilePath, "savedscenario", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Scribe_Deep.Look<Scenario>(ref scen, "scenario", Array.Empty<object>());
				}, false);
			}
			catch (Exception ex)
			{
				Log.Error("Exception while saving world: " + ex.ToString());
			}
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x000EFAE8 File Offset: 0x000EDCE8
		public static bool TryLoadScenario(string absPath, ScenarioCategory category, out Scenario scen)
		{
			scen = null;
			bool result;
			using (Log.LockMessages())
			{
				try
				{
					Scribe.loader.InitLoading(absPath);
					try
					{
						ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.Scenario, true);
						Scribe_Deep.Look<Scenario>(ref scen, "scenario", Array.Empty<object>());
						Scribe.loader.FinalizeLoading();
					}
					catch
					{
						Scribe.ForceStop();
						throw;
					}
					scen.fileName = Path.GetFileNameWithoutExtension(new FileInfo(absPath).Name);
					scen.Category = category;
				}
				catch (Exception value)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Exception loading scenario: ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					scen = null;
					Scribe.ForceStop();
				}
				if (scen == null)
				{
					result = false;
				}
				else
				{
					for (int i = scen.parts.Count - 1; i >= 0; i--)
					{
						ScenPart scenPart = scen.parts[i];
						if (((scenPart != null) ? scenPart.def : null) == null || scen.parts[i].HasNullDefs())
						{
							scen.parts.RemoveAt(i);
							scen.valid = false;
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x000EFC2C File Offset: 0x000EDE2C
		public static void SaveGame(string fileName)
		{
			try
			{
				SafeSaver.Save(GenFilePaths.FilePathForSavedGame(fileName), "savegame", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Game game = Current.Game;
					Scribe_Deep.Look<Game>(ref game, "game", Array.Empty<object>());
				}, Find.GameInfo.permadeathMode);
				GameDataSaveLoader.lastSaveTick = Find.TickManager.TicksGame;
			}
			catch (Exception ex)
			{
				string str = "Exception while saving game: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x000EFCB4 File Offset: 0x000EDEB4
		public static void CheckVersionAndLoadGame(string saveFileName)
		{
			PreLoadUtility.CheckVersionAndLoad(GenFilePaths.FilePathForSavedGame(saveFileName), ScribeMetaHeaderUtility.ScribeHeaderMode.Map, delegate
			{
				GameDataSaveLoader.LoadGame(saveFileName);
			}, false);
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x000EFCEC File Offset: 0x000EDEEC
		public static void LoadGame(string saveFileName)
		{
			GameDataSaveLoader.<>c__DisplayClass30_0 CS$<>8__locals1 = new GameDataSaveLoader.<>c__DisplayClass30_0();
			CS$<>8__locals1.saveFileName = saveFileName;
			LongEventHandler.QueueLongEvent(new Action(CS$<>8__locals1.<LoadGame>g__PreLoadAct|0), "Play", "LoadingLongEvent", true, null, true, false);
			Game game = Current.Game;
			if (game == null)
			{
				return;
			}
			game.Dispose();
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x000EFD27 File Offset: 0x000EDF27
		public static void LoadGame(FileInfo saveFile)
		{
			GameDataSaveLoader.LoadGame(Path.GetFileNameWithoutExtension(saveFile.Name));
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x000EFD3C File Offset: 0x000EDF3C
		public static void SaveIdeo(Ideo ideo, string absFilePath)
		{
			try
			{
				GameDataSaveLoader.isSavingOrLoadingExternalIdeo = true;
				ideo.fileName = Path.GetFileNameWithoutExtension(absFilePath);
				SafeSaver.Save(absFilePath, "savedideo", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Scribe_Deep.Look<Ideo>(ref ideo, "ideo", Array.Empty<object>());
				}, false);
			}
			catch (Exception ex)
			{
				Log.Error("Exception while saving ideo: " + ex.ToString());
			}
			finally
			{
				GameDataSaveLoader.isSavingOrLoadingExternalIdeo = false;
			}
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x000EFDC4 File Offset: 0x000EDFC4
		public static bool TryLoadIdeo(string absPath, out Ideo ideo)
		{
			ideo = null;
			try
			{
				GameDataSaveLoader.isSavingOrLoadingExternalIdeo = true;
				Scribe.loader.InitLoading(absPath);
				try
				{
					ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.Ideo, true);
					Scribe_Deep.Look<Ideo>(ref ideo, "ideo", Array.Empty<object>());
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
				ideo.fileName = Path.GetFileNameWithoutExtension(new FileInfo(absPath).Name);
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading ideo: " + ex.ToString());
				ideo = null;
				Scribe.ForceStop();
			}
			finally
			{
				GameDataSaveLoader.isSavingOrLoadingExternalIdeo = false;
			}
			return ideo != null;
		}

		// Token: 0x060026AD RID: 9901 RVA: 0x000EFE80 File Offset: 0x000EE080
		public static void SaveXenotype(CustomXenotype xenotype, string absFilePath)
		{
			try
			{
				xenotype.fileName = Path.GetFileNameWithoutExtension(absFilePath);
				SafeSaver.Save(absFilePath, "savedXenotype", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Scribe_Deep.Look<CustomXenotype>(ref xenotype, "xenotype", Array.Empty<object>());
				}, false);
			}
			catch (Exception ex)
			{
				Log.Error("Exception while saving xenotype: " + ex.ToString());
			}
		}

		// Token: 0x060026AE RID: 9902 RVA: 0x000EFEF0 File Offset: 0x000EE0F0
		public static bool TryLoadXenotype(string absPath, out CustomXenotype xenotype)
		{
			xenotype = null;
			try
			{
				Scribe.loader.InitLoading(absPath);
				try
				{
					ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.Xenotype, true);
					Scribe_Deep.Look<CustomXenotype>(ref xenotype, "xenotype", Array.Empty<object>());
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
				xenotype.fileName = Path.GetFileNameWithoutExtension(new FileInfo(absPath).Name);
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading xenotype: " + ex.ToString());
				xenotype = null;
				Scribe.ForceStop();
			}
			return xenotype != null;
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x000EFF90 File Offset: 0x000EE190
		public static void SaveModList(ModList modList, string absFilePath)
		{
			try
			{
				modList.fileName = Path.GetFileNameWithoutExtension(absFilePath);
				SafeSaver.Save(absFilePath, "savedModList", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Scribe_Deep.Look<ModList>(ref modList, "modList", Array.Empty<object>());
				}, false);
			}
			catch (Exception ex)
			{
				string str = "Exception while saving mod list: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x000F0004 File Offset: 0x000EE204
		public static bool TryLoadModList(string absPath, out ModList modList)
		{
			modList = null;
			try
			{
				Scribe.loader.InitLoading(absPath);
				try
				{
					ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.ModList, true);
					Scribe_Deep.Look<ModList>(ref modList, "modList", Array.Empty<object>());
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
				modList.fileName = Path.GetFileNameWithoutExtension(new FileInfo(absPath).Name);
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading mod list: " + ex.ToString());
				modList = null;
				Scribe.ForceStop();
			}
			return modList != null;
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x000F00A4 File Offset: 0x000EE2A4
		public static void SaveCameraConfig(CameraMapConfig config, string absFilePath)
		{
			try
			{
				config.fileName = Path.GetFileNameWithoutExtension(absFilePath);
				SafeSaver.Save(absFilePath, "cameraConfig", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Scribe_Deep.Look<CameraMapConfig>(ref config, "camConfig", Array.Empty<object>());
				}, false);
			}
			catch (Exception ex)
			{
				Log.Error("Exception while saving camera config: " + ex.ToString());
			}
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x000F0114 File Offset: 0x000EE314
		public static bool TryLoadCameraConfig(string absPath, out CameraMapConfig config)
		{
			config = null;
			try
			{
				Scribe.loader.InitLoading(absPath);
				try
				{
					ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.CameraConfig, true);
					Scribe_Deep.Look<CameraMapConfig>(ref config, "camConfig", Array.Empty<object>());
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
				config.fileName = Path.GetFileNameWithoutExtension(new FileInfo(absPath).Name);
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading camera config: " + ex.ToString());
				config = null;
				Scribe.ForceStop();
			}
			return config != null;
		}

		// Token: 0x04001BD7 RID: 7127
		private static int lastSaveTick = -9999;

		// Token: 0x04001BD8 RID: 7128
		private static bool isSavingOrLoadingExternalIdeo;

		// Token: 0x04001BD9 RID: 7129
		public const string SavedScenarioParentNodeName = "savedscenario";

		// Token: 0x04001BDA RID: 7130
		public const string SavedWorldParentNodeName = "savedworld";

		// Token: 0x04001BDB RID: 7131
		public const string SavedGameParentNodeName = "savegame";

		// Token: 0x04001BDC RID: 7132
		public const string SavedIdeoParentNodeName = "savedideo";

		// Token: 0x04001BDD RID: 7133
		public const string SavedXenotypeParentNodeName = "savedXenotype";

		// Token: 0x04001BDE RID: 7134
		public const string SavedXenogermParentNode = "savedXenogerm";

		// Token: 0x04001BDF RID: 7135
		public const string SavedModListParentNodeName = "savedModList";

		// Token: 0x04001BE0 RID: 7136
		public const string SavedCameraConfigParentNodeName = "cameraConfig";

		// Token: 0x04001BE1 RID: 7137
		public const string GameNodeName = "game";

		// Token: 0x04001BE2 RID: 7138
		public const string WorldNodeName = "world";

		// Token: 0x04001BE3 RID: 7139
		public const string ScenarioNodeName = "scenario";

		// Token: 0x04001BE4 RID: 7140
		public const string IdeoNodeName = "ideo";

		// Token: 0x04001BE5 RID: 7141
		public const string XenotypeNodeName = "xenotype";

		// Token: 0x04001BE6 RID: 7142
		public const string XenogermNodeName = "xenogerm";

		// Token: 0x04001BE7 RID: 7143
		public const string ModListNodeName = "modList";

		// Token: 0x04001BE8 RID: 7144
		public const string CameraConfigNodeName = "camConfig";

		// Token: 0x04001BE9 RID: 7145
		public const string AutosavePrefix = "Autosave";

		// Token: 0x04001BEA RID: 7146
		public const string AutostartSaveName = "autostart";
	}
}
