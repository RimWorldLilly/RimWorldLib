using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using RimWorld.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000227 RID: 551
	public class LoadedLanguage
	{
		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000FEE RID: 4078 RVA: 0x00059E3A File Offset: 0x0005803A
		public string DisplayName
		{
			get
			{
				return GenText.SplitCamelCase(this.folderName);
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000FEF RID: 4079 RVA: 0x00059E47 File Offset: 0x00058047
		public string FriendlyNameNative
		{
			get
			{
				if (this.info == null || this.info.friendlyNameNative.NullOrEmpty())
				{
					return this.folderName;
				}
				return this.info.friendlyNameNative;
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000FF0 RID: 4080 RVA: 0x00059E75 File Offset: 0x00058075
		public string FriendlyNameEnglish
		{
			get
			{
				if (this.info == null || this.info.friendlyNameEnglish.NullOrEmpty())
				{
					return this.folderName;
				}
				return this.info.friendlyNameEnglish;
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000FF1 RID: 4081 RVA: 0x00059EA3 File Offset: 0x000580A3
		public IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> AllDirectories
		{
			get
			{
				LoadedLanguage.<get_AllDirectories>d__31 <get_AllDirectories>d__ = new LoadedLanguage.<get_AllDirectories>d__31(-2);
				<get_AllDirectories>d__.<>4__this = this;
				return <get_AllDirectories>d__;
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x00059EB3 File Offset: 0x000580B3
		public LanguageWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (LanguageWorker)Activator.CreateInstance(this.info.languageWorkerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000FF3 RID: 4083 RVA: 0x00059EDE File Offset: 0x000580DE
		public LanguageWordInfo WordInfo
		{
			get
			{
				return this.wordInfo;
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000FF4 RID: 4084 RVA: 0x00059EE6 File Offset: 0x000580E6
		public string LegacyFolderName
		{
			get
			{
				return this.legacyFolderName;
			}
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x00059EF0 File Offset: 0x000580F0
		public LoadedLanguage(string folderName)
		{
			this.folderName = folderName;
			this.legacyFolderName = (folderName.Contains("(") ? folderName.Substring(0, folderName.IndexOf("(") - 1) : folderName).Trim();
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x00059F88 File Offset: 0x00058188
		public void LoadMetadata()
		{
			if (this.info != null && this.infoIsRealMetadata)
			{
				return;
			}
			this.infoIsRealMetadata = true;
			foreach (ModContentPack modContentPack in LoadedModManager.RunningMods)
			{
				foreach (string path in modContentPack.foldersToLoadDescendingOrder)
				{
					string text = Path.Combine(path, "Languages");
					if (new DirectoryInfo(text).Exists)
					{
						foreach (VirtualDirectory virtualDirectory in AbstractFilesystem.GetDirectories(text, "*", SearchOption.TopDirectoryOnly, false))
						{
							if (virtualDirectory.Name == this.folderName || virtualDirectory.Name == this.legacyFolderName)
							{
								this.info = DirectXmlLoader.ItemFromXmlFile<LanguageInfo>(virtualDirectory, "LanguageInfo.xml", false);
								if (this.info.friendlyNameNative.NullOrEmpty() && virtualDirectory.FileExists("FriendlyName.txt"))
								{
									this.info.friendlyNameNative = virtualDirectory.ReadAllText("FriendlyName.txt");
								}
								if (this.info.friendlyNameNative.NullOrEmpty())
								{
									this.info.friendlyNameNative = this.folderName;
								}
								if (this.info.friendlyNameEnglish.NullOrEmpty())
								{
									this.info.friendlyNameEnglish = this.folderName;
								}
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0005A170 File Offset: 0x00058370
		public void InitMetadata(VirtualDirectory directory)
		{
			this.infoIsRealMetadata = false;
			this.info = new LanguageInfo();
			string text = Regex.Replace(directory.Name, "(\\B[A-Z]+?(?=[A-Z][^A-Z])|\\B[A-Z]+?(?=[^A-Z]))", " $1");
			string friendlyNameEnglish = text;
			string friendlyNameNative = text;
			int num = text.FirstIndexOf((char c) => c == '(');
			int num2 = text.LastIndexOf(")");
			if (num >= 0 && num2 >= 0 && num2 > num)
			{
				friendlyNameEnglish = text.Substring(0, num - 1);
				friendlyNameNative = text.Substring(num + 1, num2 - num - 1);
			}
			this.info.friendlyNameEnglish = friendlyNameEnglish;
			this.info.friendlyNameNative = friendlyNameNative;
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0005A220 File Offset: 0x00058420
		public void LoadData()
		{
			if (this.dataIsLoaded)
			{
				return;
			}
			this.dataIsLoaded = true;
			DeepProfiler.Start("Loading language data: " + this.folderName);
			try
			{
				this.tmpAlreadyLoadedFiles.Clear();
				foreach (Tuple<VirtualDirectory, ModContentPack, string> tuple in this.AllDirectories)
				{
					Tuple<VirtualDirectory, ModContentPack, string> localDirectory = tuple;
					if (!this.tmpAlreadyLoadedFiles.ContainsKey(localDirectory.Item2))
					{
						this.tmpAlreadyLoadedFiles[localDirectory.Item2] = new HashSet<string>();
					}
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						if (this.icon == BaseContent.BadTex)
						{
							VirtualFile file = localDirectory.Item1.GetFile("LangIcon.png");
							if (file.Exists)
							{
								this.icon = ModContentLoader<Texture2D>.LoadItem(file).contentItem;
							}
						}
					});
					VirtualDirectory directory = localDirectory.Item1.GetDirectory("CodeLinked");
					if (directory.Exists)
					{
						List<string> list = this.loadErrors;
						string str = "Translations aren't called CodeLinked any more. Please rename to Keyed: ";
						VirtualDirectory virtualDirectory = directory;
						list.Add(str + ((virtualDirectory != null) ? virtualDirectory.ToString() : null));
					}
					else
					{
						directory = localDirectory.Item1.GetDirectory("Keyed");
					}
					if (directory.Exists)
					{
						List<VirtualFile> list2 = new List<VirtualFile>();
						foreach (VirtualFile virtualFile in directory.GetFiles("*.xml", SearchOption.AllDirectories))
						{
							if (this.TryRegisterFileIfNew(localDirectory, virtualFile.FullPath))
							{
								list2.Add(virtualFile);
							}
						}
						List<string> list3 = (from x in list2.AsParallel<VirtualFile>()
						select x.ReadAllText()).ToList<string>();
						for (int i = 0; i < list3.Count; i++)
						{
							this.LoadFromFile_Keyed(list2[i], list3[i]);
						}
					}
					VirtualDirectory directory2 = localDirectory.Item1.GetDirectory("DefLinked");
					if (directory2.Exists)
					{
						List<string> list4 = this.loadErrors;
						string str2 = "Translations aren't called DefLinked any more. Please rename to DefInjected: ";
						VirtualDirectory virtualDirectory2 = directory2;
						list4.Add(str2 + ((virtualDirectory2 != null) ? virtualDirectory2.ToString() : null));
					}
					else
					{
						directory2 = localDirectory.Item1.GetDirectory("DefInjected");
					}
					if (directory2.Exists)
					{
						foreach (VirtualDirectory virtualDirectory3 in directory2.GetDirectories("*", SearchOption.TopDirectoryOnly))
						{
							string name = virtualDirectory3.Name;
							Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(name, null);
							if (typeInAnyAssembly == null && name.Length > 3)
							{
								typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(name.Substring(0, name.Length - 1), null);
							}
							if (typeInAnyAssembly == null)
							{
								List<string> list5 = this.loadErrors;
								string[] array = new string[5];
								array[0] = "Error loading language from ";
								int num = 1;
								Tuple<VirtualDirectory, ModContentPack, string> tuple2 = tuple;
								array[num] = ((tuple2 != null) ? tuple2.ToString() : null);
								array[2] = ": dir ";
								array[3] = virtualDirectory3.Name;
								array[4] = " doesn't correspond to any def type. Skipping...";
								list5.Add(string.Concat(array));
							}
							else
							{
								List<VirtualFile> list6 = new List<VirtualFile>();
								foreach (VirtualFile virtualFile2 in virtualDirectory3.GetFiles("*.xml", SearchOption.AllDirectories))
								{
									if (this.TryRegisterFileIfNew(localDirectory, virtualFile2.FullPath))
									{
										list6.Add(virtualFile2);
									}
								}
								List<string> list7 = (from x in list6.AsParallel<VirtualFile>()
								select x.ReadAllText()).ToList<string>();
								for (int j = 0; j < list7.Count; j++)
								{
									this.LoadFromFile_DefInject(list6[j], typeInAnyAssembly, list7[j]);
								}
							}
						}
					}
					this.EnsureAllDefTypesHaveDefInjectionPackage();
					VirtualDirectory directory3 = localDirectory.Item1.GetDirectory("Strings");
					if (directory3.Exists)
					{
						foreach (VirtualDirectory virtualDirectory4 in directory3.GetDirectories("*", SearchOption.TopDirectoryOnly))
						{
							foreach (VirtualFile virtualFile3 in virtualDirectory4.GetFiles("*.txt", SearchOption.AllDirectories))
							{
								if (this.TryRegisterFileIfNew(localDirectory, virtualFile3.FullPath))
								{
									this.LoadFromFile_Strings(virtualFile3, directory3);
								}
							}
						}
					}
					this.wordInfo.LoadFrom(localDirectory, this);
				}
			}
			catch (Exception ex)
			{
				string str3 = "Exception loading language data. Rethrowing. Exception: ";
				Exception ex2 = ex;
				Log.Error(str3 + ((ex2 != null) ? ex2.ToString() : null));
				throw;
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x0005A7A8 File Offset: 0x000589A8
		public bool TryRegisterFileIfNew(Tuple<VirtualDirectory, ModContentPack, string> dir, string filePath)
		{
			if (!filePath.StartsWith(dir.Item3))
			{
				Log.Error("Failed to get a relative path for a file: " + filePath + ", located in " + dir.Item3);
				return false;
			}
			string item = filePath.Substring(dir.Item3.Length);
			if (!this.tmpAlreadyLoadedFiles.ContainsKey(dir.Item2))
			{
				this.tmpAlreadyLoadedFiles[dir.Item2] = new HashSet<string>();
			}
			else if (this.tmpAlreadyLoadedFiles[dir.Item2].Contains(item))
			{
				return false;
			}
			this.tmpAlreadyLoadedFiles[dir.Item2].Add(item);
			return true;
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0005A854 File Offset: 0x00058A54
		private void LoadFromFile_Strings(VirtualFile file, VirtualDirectory stringsTopDir)
		{
			string text;
			try
			{
				text = file.ReadAllText();
			}
			catch (Exception ex)
			{
				List<string> list = this.loadErrors;
				string str = "Exception loading from strings file ";
				string str2 = (file != null) ? file.ToString() : null;
				string str3 = ": ";
				Exception ex2 = ex;
				list.Add(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				return;
			}
			string text2 = file.FullPath;
			if (stringsTopDir != null)
			{
				text2 = text2.Substring(stringsTopDir.FullPath.Length + 1);
			}
			text2 = text2.Substring(0, text2.Length - Path.GetExtension(text2).Length);
			text2 = text2.Replace('\\', '/');
			List<string> list2 = new List<string>();
			foreach (string item in GenText.LinesFromString(text))
			{
				list2.Add(item);
			}
			List<string> list3;
			if (this.stringFiles.TryGetValue(text2, out list3))
			{
				using (List<string>.Enumerator enumerator2 = list2.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						string item2 = enumerator2.Current;
						list3.Add(item2);
					}
					return;
				}
			}
			this.stringFiles.Add(text2, list2);
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0005A9A0 File Offset: 0x00058BA0
		private void LoadFromFile_Keyed(VirtualFile file, string preloadedFileContents)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			try
			{
				foreach (DirectXmlLoaderSimple.XmlKeyValuePair xmlKeyValuePair in DirectXmlLoaderSimple.ValuesFromXmlFile(preloadedFileContents))
				{
					if (dictionary.ContainsKey(xmlKeyValuePair.key))
					{
						this.loadErrors.Add("Duplicate keyed translation key: " + xmlKeyValuePair.key + " in language " + this.folderName);
					}
					else
					{
						dictionary.Add(xmlKeyValuePair.key, xmlKeyValuePair.value);
						dictionary2.Add(xmlKeyValuePair.key, xmlKeyValuePair.lineNumber);
					}
				}
			}
			catch (Exception ex)
			{
				List<string> list = this.loadErrors;
				string str = "Exception loading from translation file ";
				string str2 = (file != null) ? file.ToString() : null;
				string str3 = ": ";
				Exception ex2 = ex;
				list.Add(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				dictionary.Clear();
				dictionary2.Clear();
				this.anyKeyedReplacementsXmlParseError = true;
				this.lastKeyedReplacementsXmlParseErrorInFile = file.Name;
			}
			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				string text = keyValuePair.Value;
				LoadedLanguage.KeyedReplacement keyedReplacement = new LoadedLanguage.KeyedReplacement();
				if (text == "TODO")
				{
					keyedReplacement.isPlaceholder = true;
					text = "";
				}
				keyedReplacement.key = keyValuePair.Key;
				keyedReplacement.value = text;
				keyedReplacement.fileSource = file.Name;
				keyedReplacement.fileSourceLine = dictionary2[keyValuePair.Key];
				keyedReplacement.fileSourceFullPath = file.FullPath;
				this.keyedReplacements.SetOrAdd(keyValuePair.Key, keyedReplacement);
			}
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0005AB78 File Offset: 0x00058D78
		public void LoadFromFile_DefInject(VirtualFile file, Type defType, string preloadedFileContents)
		{
			DefInjectionPackage defInjectionPackage = (from di in this.defInjections
			where di.defType == defType
			select di).FirstOrDefault<DefInjectionPackage>();
			if (defInjectionPackage == null)
			{
				defInjectionPackage = new DefInjectionPackage(defType);
				this.defInjections.Add(defInjectionPackage);
			}
			bool flag;
			defInjectionPackage.AddDataFromFile(file, out flag, preloadedFileContents);
			if (flag)
			{
				this.anyDefInjectionsXmlParseError = true;
				this.lastDefInjectionsXmlParseErrorInFile = file.Name;
			}
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x0005ABEC File Offset: 0x00058DEC
		private void EnsureAllDefTypesHaveDefInjectionPackage()
		{
			using (IEnumerator<Type> enumerator = GenDefDatabase.AllDefTypesWithDatabases().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Type defType = enumerator.Current;
					if (!this.defInjections.Any((DefInjectionPackage x) => x.defType == defType))
					{
						this.defInjections.Add(new DefInjectionPackage(defType));
					}
				}
			}
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x0005AC6C File Offset: 0x00058E6C
		public bool HaveTextForKey(string key, bool allowPlaceholders = false)
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			LoadedLanguage.KeyedReplacement keyedReplacement;
			return key != null && this.keyedReplacements.TryGetValue(key, out keyedReplacement) && (allowPlaceholders || !keyedReplacement.isPlaceholder);
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x0005ACAC File Offset: 0x00058EAC
		public bool TryGetTextFromKey(string key, out TaggedString translated)
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			if (key == null)
			{
				translated = key;
				return false;
			}
			LoadedLanguage.KeyedReplacement keyedReplacement;
			if (!this.keyedReplacements.TryGetValue(key, out keyedReplacement) || keyedReplacement.isPlaceholder)
			{
				translated = key;
				return false;
			}
			translated = keyedReplacement.value;
			return true;
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0005AD10 File Offset: 0x00058F10
		public bool TryGetStringsFromFile(string fileName, out List<string> stringsList)
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			if (!this.stringFiles.TryGetValue(fileName, out stringsList))
			{
				stringsList = null;
				return false;
			}
			return true;
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0005AD38 File Offset: 0x00058F38
		public string GetKeySourceFileAndLine(string key)
		{
			LoadedLanguage.KeyedReplacement keyedReplacement;
			if (!this.keyedReplacements.TryGetValue(key, out keyedReplacement))
			{
				return "unknown";
			}
			return keyedReplacement.fileSource + ":" + keyedReplacement.fileSourceLine.ToString();
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0005AD76 File Offset: 0x00058F76
		public Gender ResolveGender(string str, string fallback = null, Gender defaultGender = Gender.Male)
		{
			return this.wordInfo.ResolveGender(str, fallback, defaultGender);
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0005AD88 File Offset: 0x00058F88
		public void InjectIntoData_BeforeImpliedDefs()
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			foreach (DefInjectionPackage defInjectionPackage in this.defInjections)
			{
				try
				{
					defInjectionPackage.InjectIntoDefs(false);
				}
				catch (Exception ex)
				{
					string str = "Critical error while injecting translations into defs: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0005AE18 File Offset: 0x00059018
		public void InjectIntoData_AfterImpliedDefs()
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			int num = this.loadErrors.Count;
			foreach (DefInjectionPackage defInjectionPackage in this.defInjections)
			{
				try
				{
					defInjectionPackage.InjectIntoDefs(true);
					num += defInjectionPackage.loadErrors.Count;
				}
				catch (Exception ex)
				{
					string str = "Critical error while injecting translations into defs: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			if (num != 0)
			{
				this.anyError = true;
				Log.Warning(string.Concat(new string[]
				{
					"Translation data for language ",
					LanguageDatabase.activeLanguage.FriendlyNameEnglish,
					" has ",
					num.ToString(),
					" errors. Generate translation report for more info."
				}));
			}
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0005AF0C File Offset: 0x0005910C
		public override string ToString()
		{
			return this.info.friendlyNameEnglish;
		}

		// Token: 0x04000EE8 RID: 3816
		public string folderName;

		// Token: 0x04000EE9 RID: 3817
		public LanguageInfo info;

		// Token: 0x04000EEA RID: 3818
		private LanguageWorker workerInt;

		// Token: 0x04000EEB RID: 3819
		private LanguageWordInfo wordInfo = new LanguageWordInfo();

		// Token: 0x04000EEC RID: 3820
		private bool dataIsLoaded;

		// Token: 0x04000EED RID: 3821
		public List<string> loadErrors = new List<string>();

		// Token: 0x04000EEE RID: 3822
		public bool anyKeyedReplacementsXmlParseError;

		// Token: 0x04000EEF RID: 3823
		public string lastKeyedReplacementsXmlParseErrorInFile;

		// Token: 0x04000EF0 RID: 3824
		public bool anyDefInjectionsXmlParseError;

		// Token: 0x04000EF1 RID: 3825
		public string lastDefInjectionsXmlParseErrorInFile;

		// Token: 0x04000EF2 RID: 3826
		public bool anyError;

		// Token: 0x04000EF3 RID: 3827
		private string legacyFolderName;

		// Token: 0x04000EF4 RID: 3828
		private Dictionary<ModContentPack, HashSet<string>> tmpAlreadyLoadedFiles = new Dictionary<ModContentPack, HashSet<string>>();

		// Token: 0x04000EF5 RID: 3829
		public Texture2D icon = BaseContent.BadTex;

		// Token: 0x04000EF6 RID: 3830
		public Dictionary<string, LoadedLanguage.KeyedReplacement> keyedReplacements = new Dictionary<string, LoadedLanguage.KeyedReplacement>();

		// Token: 0x04000EF7 RID: 3831
		public List<DefInjectionPackage> defInjections = new List<DefInjectionPackage>();

		// Token: 0x04000EF8 RID: 3832
		public Dictionary<string, List<string>> stringFiles = new Dictionary<string, List<string>>();

		// Token: 0x04000EF9 RID: 3833
		public const string OldKeyedTranslationsFolderName = "CodeLinked";

		// Token: 0x04000EFA RID: 3834
		public const string KeyedTranslationsFolderName = "Keyed";

		// Token: 0x04000EFB RID: 3835
		public const string OldDefInjectionsFolderName = "DefLinked";

		// Token: 0x04000EFC RID: 3836
		public const string DefInjectionsFolderName = "DefInjected";

		// Token: 0x04000EFD RID: 3837
		public const string LanguagesFolderName = "Languages";

		// Token: 0x04000EFE RID: 3838
		public const string PlaceholderText = "TODO";

		// Token: 0x04000EFF RID: 3839
		private bool infoIsRealMetadata;

		// Token: 0x020025A1 RID: 9633
		public class KeyedReplacement
		{
			// Token: 0x04009A5A RID: 39514
			public string key;

			// Token: 0x04009A5B RID: 39515
			public string value;

			// Token: 0x04009A5C RID: 39516
			public string fileSource;

			// Token: 0x04009A5D RID: 39517
			public int fileSourceLine;

			// Token: 0x04009A5E RID: 39518
			public string fileSourceFullPath;

			// Token: 0x04009A5F RID: 39519
			public bool isPlaceholder;
		}
	}
}
