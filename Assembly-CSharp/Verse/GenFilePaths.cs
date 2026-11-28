using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006EA RID: 1770
	public static class GenFilePaths
	{
		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x06003820 RID: 14368 RVA: 0x00167EF8 File Offset: 0x001660F8
		public static string SaveDataFolderPath
		{
			get
			{
				if (GenFilePaths.saveDataPath == null)
				{
					string text;
					if (GenCommandLine.TryGetCommandLineArg("savedatafolder", out text))
					{
						text.TrimEnd(new char[]
						{
							'\\',
							'/'
						});
						if (text == "")
						{
							text = (Path.DirectorySeparatorChar.ToString() ?? "");
						}
						GenFilePaths.saveDataPath = text;
						Log.Message("Save data folder overridden to " + GenFilePaths.saveDataPath);
					}
					else
					{
						DirectoryInfo directoryInfo = new DirectoryInfo(UnityData.dataPath);
						if (UnityData.isEditor)
						{
							GenFilePaths.saveDataPath = Path.Combine(directoryInfo.Parent.ToString(), "SaveData");
						}
						else if (UnityData.platform == RuntimePlatform.OSXPlayer || UnityData.platform == RuntimePlatform.OSXEditor)
						{
							string path = Path.Combine(Directory.GetParent(UnityData.persistentDataPath).ToString(), "RimWorld");
							if (!Directory.Exists(path))
							{
								Directory.CreateDirectory(path);
							}
							GenFilePaths.saveDataPath = path;
						}
						else
						{
							GenFilePaths.saveDataPath = Application.persistentDataPath;
						}
					}
					DirectoryInfo directoryInfo2 = new DirectoryInfo(GenFilePaths.saveDataPath);
					if (!directoryInfo2.Exists)
					{
						directoryInfo2.Create();
					}
				}
				return GenFilePaths.saveDataPath;
			}
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x06003821 RID: 14369 RVA: 0x00168008 File Offset: 0x00166208
		public static string ScenarioPreviewImagePath
		{
			get
			{
				if (!UnityData.isEditor)
				{
					return Path.Combine(GenFilePaths.ExecutableDir.FullName, "ScenarioPreview.jpg");
				}
				return Path.Combine(Path.Combine(Path.Combine(GenFilePaths.ExecutableDir.FullName, "PlatformSpecific"), "All"), "ScenarioPreview.jpg");
			}
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x06003822 RID: 14370 RVA: 0x00168059 File Offset: 0x00166259
		private static DirectoryInfo ExecutableDir
		{
			get
			{
				return new DirectoryInfo(UnityData.dataPath).Parent;
			}
		}

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x06003823 RID: 14371 RVA: 0x0016806A File Offset: 0x0016626A
		public static string ModsFolderPath
		{
			get
			{
				if (GenFilePaths.modsFolderPath == null)
				{
					GenFilePaths.modsFolderPath = GenFilePaths.GetOrCreateModsFolder("Mods");
				}
				return GenFilePaths.modsFolderPath;
			}
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06003824 RID: 14372 RVA: 0x00168087 File Offset: 0x00166287
		public static string OfficialModsFolderPath
		{
			get
			{
				if (GenFilePaths.officialModsFolderPath == null)
				{
					GenFilePaths.officialModsFolderPath = GenFilePaths.GetOrCreateModsFolder("Data");
				}
				return GenFilePaths.officialModsFolderPath;
			}
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06003825 RID: 14373 RVA: 0x001680A4 File Offset: 0x001662A4
		public static string ConfigFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Config");
			}
		}

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06003826 RID: 14374 RVA: 0x001680B0 File Offset: 0x001662B0
		public static string SavedGamesFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Saves");
			}
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06003827 RID: 14375 RVA: 0x001680BC File Offset: 0x001662BC
		private static string ScenariosFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Scenarios");
			}
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06003828 RID: 14376 RVA: 0x001680C8 File Offset: 0x001662C8
		private static string ModListsFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("ModLists");
			}
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x06003829 RID: 14377 RVA: 0x001680D4 File Offset: 0x001662D4
		private static string IdeosFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Ideos");
			}
		}

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x0600382A RID: 14378 RVA: 0x001680E0 File Offset: 0x001662E0
		private static string XenotypesFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Xenotypes");
			}
		}

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x0600382B RID: 14379 RVA: 0x001680EC File Offset: 0x001662EC
		private static string XenogermsFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Xenogerms");
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x0600382C RID: 14380 RVA: 0x001680F8 File Offset: 0x001662F8
		private static string CameraConfigsFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("CameraConfigs");
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x0600382D RID: 14381 RVA: 0x00168104 File Offset: 0x00166304
		private static string ExternalHistoryFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("External");
			}
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x0600382E RID: 14382 RVA: 0x00168110 File Offset: 0x00166310
		public static string ScreenshotFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Screenshots");
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x0600382F RID: 14383 RVA: 0x0016811C File Offset: 0x0016631C
		public static string DevOutputFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("DevOutput");
			}
		}

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06003830 RID: 14384 RVA: 0x00168128 File Offset: 0x00166328
		public static string ModsConfigFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "ModsConfig.xml");
			}
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x06003831 RID: 14385 RVA: 0x00168139 File Offset: 0x00166339
		public static string ConceptKnowledgeFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "Knowledge.xml");
			}
		}

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x06003832 RID: 14386 RVA: 0x0016814A File Offset: 0x0016634A
		public static string PrefsFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "Prefs.xml");
			}
		}

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x06003833 RID: 14387 RVA: 0x0016815B File Offset: 0x0016635B
		public static string KeyPrefsFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "KeyPrefs.xml");
			}
		}

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x06003834 RID: 14388 RVA: 0x0016816C File Offset: 0x0016636C
		public static string LastPlayedVersionFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "LastPlayedVersion.txt");
			}
		}

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x06003835 RID: 14389 RVA: 0x0016817D File Offset: 0x0016637D
		public static string DevModePermanentlyDisabledFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "DevModeDisabled");
			}
		}

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x06003836 RID: 14390 RVA: 0x0016818E File Offset: 0x0016638E
		public static string BackstoryOutputFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.DevOutputFolderPath, "Fresh_Backstories.xml");
			}
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x06003837 RID: 14391 RVA: 0x0016819F File Offset: 0x0016639F
		public static string TempFolderPath
		{
			get
			{
				return Application.temporaryCachePath;
			}
		}

		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x06003838 RID: 14392 RVA: 0x001681A8 File Offset: 0x001663A8
		public static IEnumerable<FileInfo> AllSavedGameFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.SavedGamesFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rws"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x06003839 RID: 14393 RVA: 0x0016821C File Offset: 0x0016641C
		public static IEnumerable<FileInfo> AllCustomScenarioFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.ScenariosFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rsc"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x0600383A RID: 14394 RVA: 0x00168290 File Offset: 0x00166490
		public static IEnumerable<FileInfo> AllModListFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.ModListsFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rml"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x0600383B RID: 14395 RVA: 0x00168304 File Offset: 0x00166504
		public static IEnumerable<FileInfo> AllCustomIdeoFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.IdeosFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rid"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x0600383C RID: 14396 RVA: 0x00168378 File Offset: 0x00166578
		public static IEnumerable<FileInfo> AllCustomXenotypeFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.XenotypesFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".xtp"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x0600383D RID: 14397 RVA: 0x001683EC File Offset: 0x001665EC
		public static IEnumerable<FileInfo> AllCustomXenogermFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.XenogermsFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".xgm"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x0600383E RID: 14398 RVA: 0x00168460 File Offset: 0x00166660
		public static IEnumerable<FileInfo> AllCameraConfigFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.CameraConfigsFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".ccg"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x0600383F RID: 14399 RVA: 0x001684D4 File Offset: 0x001666D4
		public static IEnumerable<FileInfo> AllExternalHistoryFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.ExternalHistoryFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rwh"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x06003840 RID: 14400 RVA: 0x00168548 File Offset: 0x00166748
		private static string FolderUnderSaveData(string folderName)
		{
			string text = Path.Combine(GenFilePaths.SaveDataFolderPath, folderName);
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
			return text;
		}

		// Token: 0x06003841 RID: 14401 RVA: 0x00168575 File Offset: 0x00166775
		public static string FilePathForSavedGame(string gameName)
		{
			return Path.Combine(GenFilePaths.SavedGamesFolderPath, gameName + ".rws");
		}

		// Token: 0x06003842 RID: 14402 RVA: 0x0016858C File Offset: 0x0016678C
		public static string AbsPathForScenario(string scenarioName)
		{
			return Path.Combine(GenFilePaths.ScenariosFolderPath, scenarioName + ".rsc");
		}

		// Token: 0x06003843 RID: 14403 RVA: 0x001685A3 File Offset: 0x001667A3
		public static string AbsPathForIdeo(string ideoName)
		{
			return Path.Combine(GenFilePaths.IdeosFolderPath, ideoName + ".rid");
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x001685BA File Offset: 0x001667BA
		public static string AbsFilePathForXenotype(string xenotypeName)
		{
			return Path.Combine(GenFilePaths.XenotypesFolderPath, xenotypeName + ".xtp");
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x001685D1 File Offset: 0x001667D1
		public static string AbsFilePathForModList(string modListName)
		{
			return Path.Combine(GenFilePaths.ModListsFolderPath, modListName + ".rml");
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x001685E8 File Offset: 0x001667E8
		public static string AbsFilePathForXenogerm(string xenogermName)
		{
			return Path.Combine(GenFilePaths.XenogermsFolderPath, xenogermName + ".xgm");
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x001685FF File Offset: 0x001667FF
		public static string AbsFilePathForCameraConfig(string configName)
		{
			return Path.Combine(GenFilePaths.CameraConfigsFolderPath, configName + ".ccg");
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x00168618 File Offset: 0x00166818
		public static string ContentPath<T>()
		{
			if (typeof(T) == typeof(AudioClip))
			{
				return "Sounds/";
			}
			if (typeof(T) == typeof(Texture2D))
			{
				return "Textures/";
			}
			if (typeof(T) == typeof(string))
			{
				return "Strings/";
			}
			if (typeof(T) == typeof(AssetBundle))
			{
				return "AssetBundles/";
			}
			if (typeof(T) == typeof(Shader))
			{
				return "Materials/";
			}
			throw new ArgumentException();
		}

		// Token: 0x06003849 RID: 14409 RVA: 0x001686D0 File Offset: 0x001668D0
		private static string GetOrCreateModsFolder(string folderName)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(UnityData.dataPath);
			DirectoryInfo directoryInfo2;
			if (UnityData.isEditor)
			{
				directoryInfo2 = directoryInfo;
			}
			else
			{
				directoryInfo2 = directoryInfo.Parent;
			}
			string text = Path.Combine(directoryInfo2.ToString(), folderName);
			DirectoryInfo directoryInfo3 = new DirectoryInfo(text);
			if (!directoryInfo3.Exists)
			{
				directoryInfo3.Create();
			}
			return text;
		}

		// Token: 0x0600384A RID: 14410 RVA: 0x00168720 File Offset: 0x00166920
		public static string SafeURIForUnityWWWFromPath(string rawPath)
		{
			string text = rawPath;
			for (int i = 0; i < GenFilePaths.FilePathRaw.Length; i++)
			{
				text = text.Replace(GenFilePaths.FilePathRaw[i], GenFilePaths.FilePathSafe[i]);
			}
			return "file:///" + text;
		}

		// Token: 0x040024AC RID: 9388
		private static string saveDataPath = null;

		// Token: 0x040024AD RID: 9389
		private static string modsFolderPath = null;

		// Token: 0x040024AE RID: 9390
		private static string officialModsFolderPath = null;

		// Token: 0x040024AF RID: 9391
		public const string SoundsFolder = "Sounds/";

		// Token: 0x040024B0 RID: 9392
		public const string SoundsFolderName = "Sounds";

		// Token: 0x040024B1 RID: 9393
		public const string TexturesFolder = "Textures/";

		// Token: 0x040024B2 RID: 9394
		public const string TexturesFolderName = "Textures";

		// Token: 0x040024B3 RID: 9395
		public const string StringsFolder = "Strings/";

		// Token: 0x040024B4 RID: 9396
		public const string DefsFolder = "Defs/";

		// Token: 0x040024B5 RID: 9397
		public const string PatchesFolder = "Patches/";

		// Token: 0x040024B6 RID: 9398
		public const string AssetBundlesFolderName = "AssetBundles";

		// Token: 0x040024B7 RID: 9399
		public const string AssetBundlesFolder = "AssetBundles/";

		// Token: 0x040024B8 RID: 9400
		public const string MaterialsFolder = "Materials/";

		// Token: 0x040024B9 RID: 9401
		public const string AssetsFolderName = "Assets";

		// Token: 0x040024BA RID: 9402
		public const string ResourcesFolderName = "Resources";

		// Token: 0x040024BB RID: 9403
		public const string ModsFolderName = "Mods";

		// Token: 0x040024BC RID: 9404
		public const string AssembliesFolder = "Assemblies/";

		// Token: 0x040024BD RID: 9405
		public const string OfficialModsFolderName = "Data";

		// Token: 0x040024BE RID: 9406
		public const string CoreFolderName = "Core";

		// Token: 0x040024BF RID: 9407
		public const string BackstoriesPath = "Backstories";

		// Token: 0x040024C0 RID: 9408
		public const string SavedGameExtension = ".rws";

		// Token: 0x040024C1 RID: 9409
		public const string ScenarioExtension = ".rsc";

		// Token: 0x040024C2 RID: 9410
		public const string ModListExtension = ".rml";

		// Token: 0x040024C3 RID: 9411
		public const string IdeoExtension = ".rid";

		// Token: 0x040024C4 RID: 9412
		public const string XenotypeExtension = ".xtp";

		// Token: 0x040024C5 RID: 9413
		public const string XenogermExtension = ".xgm";

		// Token: 0x040024C6 RID: 9414
		public const string CameraConfigExtension = ".ccg";

		// Token: 0x040024C7 RID: 9415
		public const string ExternalHistoryFileExtension = ".rwh";

		// Token: 0x040024C8 RID: 9416
		private const string SaveDataFolderCommand = "savedatafolder";

		// Token: 0x040024C9 RID: 9417
		private static readonly string[] FilePathRaw = new string[]
		{
			"Ž",
			"ž",
			"Ÿ",
			"¡",
			"¢",
			"£",
			"¤",
			"¥",
			"¦",
			"§",
			"¨",
			"©",
			"ª",
			"À",
			"Á",
			"Â",
			"Ã",
			"Ä",
			"Å",
			"Æ",
			"Ç",
			"È",
			"É",
			"Ê",
			"Ë",
			"Ì",
			"Í",
			"Î",
			"Ï",
			"Ð",
			"Ñ",
			"Ò",
			"Ó",
			"Ô",
			"Õ",
			"Ö",
			"Ù",
			"Ú",
			"Û",
			"Ü",
			"Ý",
			"Þ",
			"ß",
			"à",
			"á",
			"â",
			"ã",
			"ä",
			"å",
			"æ",
			"ç",
			"è",
			"é",
			"ê",
			"ë",
			"ì",
			"í",
			"î",
			"ï",
			"ð",
			"ñ",
			"ò",
			"ó",
			"ô",
			"õ",
			"ö",
			"ù",
			"ú",
			"û",
			"ü",
			"ý",
			"þ",
			"ÿ"
		};

		// Token: 0x040024CA RID: 9418
		private static readonly string[] FilePathSafe = new string[]
		{
			"%8E",
			"%9E",
			"%9F",
			"%A1",
			"%A2",
			"%A3",
			"%A4",
			"%A5",
			"%A6",
			"%A7",
			"%A8",
			"%A9",
			"%AA",
			"%C0",
			"%C1",
			"%C2",
			"%C3",
			"%C4",
			"%C5",
			"%C6",
			"%C7",
			"%C8",
			"%C9",
			"%CA",
			"%CB",
			"%CC",
			"%CD",
			"%CE",
			"%CF",
			"%D0",
			"%D1",
			"%D2",
			"%D3",
			"%D4",
			"%D5",
			"%D6",
			"%D9",
			"%DA",
			"%DB",
			"%DC",
			"%DD",
			"%DE",
			"%DF",
			"%E0",
			"%E1",
			"%E2",
			"%E3",
			"%E4",
			"%E5",
			"%E6",
			"%E7",
			"%E8",
			"%E9",
			"%EA",
			"%EB",
			"%EC",
			"%ED",
			"%EE",
			"%EF",
			"%F0",
			"%F1",
			"%F2",
			"%F3",
			"%F4",
			"%F5",
			"%F6",
			"%F9",
			"%FA",
			"%FB",
			"%FC",
			"%FD",
			"%FE",
			"%FF"
		};
	}
}
