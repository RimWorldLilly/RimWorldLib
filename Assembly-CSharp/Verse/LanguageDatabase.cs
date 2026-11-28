using System;
using System.Collections.Generic;
using System.IO;
using RimWorld.IO;
using Steamworks;
using UnityEngine;
using UnityStubs.Gui;
using Verse.Steam;

namespace Verse
{
	// Token: 0x0200020F RID: 527
	public static class LanguageDatabase
	{
		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000F4B RID: 3915 RVA: 0x000552D9 File Offset: 0x000534D9
		public static IEnumerable<LoadedLanguage> AllLoadedLanguages
		{
			get
			{
				return LanguageDatabase.languages;
			}
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x000552E0 File Offset: 0x000534E0
		public static void SelectLanguage(LoadedLanguage lang)
		{
			Prefs.LangFolderName = lang.folderName;
			LongEventHandler.QueueLongEvent(delegate()
			{
				PlayDataLoader.ClearAllPlayData();
				PlayDataLoader.LoadAllPlayData(false);
			}, "LoadingLongEvent", true, null, true, false, null);
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x0005531B File Offset: 0x0005351B
		public static void Clear()
		{
			LanguageDatabase.languages.Clear();
			LanguageDatabase.activeLanguage = null;
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x00055330 File Offset: 0x00053530
		public static void InitAllMetadata()
		{
			foreach (ModContentPack modContentPack in LoadedModManager.RunningMods)
			{
				HashSet<string> hashSet = new HashSet<string>();
				foreach (string path in modContentPack.foldersToLoadDescendingOrder)
				{
					string text = Path.Combine(path, "Languages");
					if (new DirectoryInfo(text).Exists)
					{
						foreach (VirtualDirectory virtualDirectory in AbstractFilesystem.GetDirectories(text, "*", SearchOption.TopDirectoryOnly, false))
						{
							if (!virtualDirectory.FullPath.StartsWith(text))
							{
								Log.Error("Failed to get a relative path for a file: " + virtualDirectory.FullPath + ", located in " + text);
							}
							else
							{
								string item = virtualDirectory.FullPath.Substring(text.Length);
								if (!hashSet.Contains(item))
								{
									LanguageDatabase.InitLanguageMetadataFrom(virtualDirectory);
									hashSet.Add(item);
								}
							}
						}
					}
				}
			}
			LanguageDatabase.languages.SortBy((LoadedLanguage l) => l.folderName);
			LanguageDatabase.defaultLanguage = LanguageDatabase.languages.FirstOrDefault((LoadedLanguage la) => la.folderName == LanguageDatabase.DefaultLangFolderName);
			LanguageDatabase.activeLanguage = LanguageDatabase.languages.FirstOrDefault((LoadedLanguage la) => la.folderName == Prefs.LangFolderName);
			if (LanguageDatabase.activeLanguage == null)
			{
				Prefs.LangFolderName = LanguageDatabase.DefaultLangFolderName;
				LanguageDatabase.activeLanguage = LanguageDatabase.languages.FirstOrDefault((LoadedLanguage la) => la.folderName == Prefs.LangFolderName);
			}
			if (LanguageDatabase.activeLanguage == null || LanguageDatabase.defaultLanguage == null)
			{
				Log.Error("No default language found!");
				LanguageDatabase.defaultLanguage = LanguageDatabase.languages[0];
				LanguageDatabase.activeLanguage = LanguageDatabase.languages[0];
			}
			LanguageDatabase.activeLanguage.LoadMetadata();
			IGuiBackend guiBackend = GuiBackend.Current;
			if (guiBackend == null)
			{
				return;
			}
			guiBackend.SetTextLanguage(LanguageDatabase.activeLanguage.folderName);
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x000555C0 File Offset: 0x000537C0
		private static LoadedLanguage InitLanguageMetadataFrom(VirtualDirectory langDir)
		{
			LoadedLanguage loadedLanguage = LanguageDatabase.languages.FirstOrDefault((LoadedLanguage lib) => lib.folderName == langDir.Name || lib.LegacyFolderName == langDir.Name);
			if (loadedLanguage == null)
			{
				loadedLanguage = new LoadedLanguage(langDir.Name);
				LanguageDatabase.languages.Add(loadedLanguage);
			}
			if (loadedLanguage != null)
			{
				loadedLanguage.InitMetadata(langDir);
			}
			return loadedLanguage;
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x00055620 File Offset: 0x00053820
		public static string SystemLanguageFolderName()
		{
			if (SteamManager.Initialized)
			{
				string text = SteamApps.GetCurrentGameLanguage().CapitalizeFirst();
				if (LanguageDatabase.SupportedAutoSelectLanguages.Contains(text))
				{
					return text;
				}
			}
			string text2 = Application.systemLanguage.ToString();
			if (LanguageDatabase.SupportedAutoSelectLanguages.Contains(text2))
			{
				return text2;
			}
			return LanguageDatabase.DefaultLangFolderName;
		}

		// Token: 0x04000EB6 RID: 3766
		private static List<LoadedLanguage> languages = new List<LoadedLanguage>();

		// Token: 0x04000EB7 RID: 3767
		public static LoadedLanguage activeLanguage;

		// Token: 0x04000EB8 RID: 3768
		public static LoadedLanguage defaultLanguage;

		// Token: 0x04000EB9 RID: 3769
		public static readonly string DefaultLangFolderName = "English";

		// Token: 0x04000EBA RID: 3770
		private static readonly List<string> SupportedAutoSelectLanguages = new List<string>
		{
			"Arabic",
			"ChineseSimplified",
			"ChineseTraditional",
			"Czech",
			"Danish",
			"Dutch",
			"English",
			"Estonian",
			"Finnish",
			"French",
			"German",
			"Hungarian",
			"Italian",
			"Japanese",
			"Korean",
			"Norwegian",
			"Polish",
			"Portuguese",
			"PortugueseBrazilian",
			"Romanian",
			"Russian",
			"Slovak",
			"Spanish",
			"SpanishLatin",
			"Swedish",
			"Turkish",
			"Ukrainian"
		};
	}
}
