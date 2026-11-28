using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using KTrie;
using RimWorld;
using UnityEngine;
using UnityStubs;

namespace Verse
{
	// Token: 0x0200033F RID: 831
	public class ModContentPack
	{
		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x0600195B RID: 6491 RVA: 0x00098748 File Offset: 0x00096948
		public string RootDir
		{
			get
			{
				return this.rootDirInt.FullName;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x0600195C RID: 6492 RVA: 0x00098755 File Offset: 0x00096955
		public string PackageId
		{
			get
			{
				return this.packageIdInt;
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x0600195D RID: 6493 RVA: 0x0009875D File Offset: 0x0009695D
		public string PackageIdPlayerFacing
		{
			get
			{
				return this.packageIdPlayerFacingInt;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x0600195E RID: 6494 RVA: 0x00098765 File Offset: 0x00096965
		public uint SteamAppId
		{
			get
			{
				ModMetaData modMetaData = this.ModMetaData;
				if (modMetaData == null)
				{
					return 0U;
				}
				return modMetaData.SteamAppId;
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x0600195F RID: 6495 RVA: 0x00098778 File Offset: 0x00096978
		public ModMetaData ModMetaData
		{
			get
			{
				return ModLister.GetModWithIdentifier(this.PackageId, false);
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06001960 RID: 6496 RVA: 0x00098786 File Offset: 0x00096986
		public string FolderName
		{
			get
			{
				return this.rootDirInt.Name;
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001961 RID: 6497 RVA: 0x00098793 File Offset: 0x00096993
		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06001962 RID: 6498 RVA: 0x0009879B File Offset: 0x0009699B
		public int OverwritePriority
		{
			get
			{
				return (!this.IsCoreMod) ? 1 : 0;
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001963 RID: 6499 RVA: 0x000987A6 File Offset: 0x000969A6
		public bool IsCoreMod
		{
			get
			{
				return this.PackageId == "ludeon.rimworld";
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001964 RID: 6500 RVA: 0x000987B8 File Offset: 0x000969B8
		public bool IsOfficialMod
		{
			get
			{
				return this.official;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001965 RID: 6501 RVA: 0x000987C0 File Offset: 0x000969C0
		public IEnumerable<Def> AllDefs
		{
			get
			{
				return this.defs;
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001966 RID: 6502 RVA: 0x000987C8 File Offset: 0x000969C8
		public IEnumerable<PatchOperation> Patches
		{
			get
			{
				if (this.patches == null)
				{
					this.LoadPatches();
				}
				return this.patches;
			}
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x000987E0 File Offset: 0x000969E0
		public List<string> AllAssetNamesInBundle(int index)
		{
			if (this.allAssetNamesInBundleCached == null)
			{
				this.allAssetNamesInBundleCached = new List<List<string>>();
				foreach (AssetBundle assetBundle in this.assetBundles.loadedAssetBundles)
				{
					this.allAssetNamesInBundleCached.Add(new List<string>(assetBundle.GetAllAssetNames()));
				}
			}
			return this.allAssetNamesInBundleCached[index];
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x00098868 File Offset: 0x00096A68
		public StringTrieSet AllAssetNamesInBundleTrie(int index)
		{
			if (this.allAssetNamesInBundleCachedTrie == null)
			{
				this.allAssetNamesInBundleCachedTrie = new List<StringTrieSet>();
				foreach (AssetBundle assetBundle in this.assetBundles.loadedAssetBundles)
				{
					StringTrieSet stringTrieSet = new StringTrieSet();
					foreach (string item in assetBundle.GetAllAssetNames())
					{
						stringTrieSet.Add(item);
					}
					this.allAssetNamesInBundleCachedTrie.Add(stringTrieSet);
				}
			}
			return this.allAssetNamesInBundleCachedTrie[index];
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x0009890C File Offset: 0x00096B0C
		public ModContentPack(DirectoryInfo directory, string packageId, string packageIdPlayerFacing, int loadOrder, string name, bool official)
		{
			this.rootDirInt = directory;
			this.loadOrder = loadOrder;
			this.nameInt = name;
			this.official = official;
			this.packageIdInt = packageId.ToLower();
			this.packageIdPlayerFacingInt = packageIdPlayerFacing;
			this.audioClips = new ModContentHolder<AudioClip>(this);
			this.textures = new ModContentHolder<Texture2D>(this);
			this.strings = new ModContentHolder<string>(this);
			this.assetBundles = new ModAssetBundlesHandler(this);
			this.assemblies = new ModAssemblyHandler(this);
			this.InitLoadFolders();
			foreach (string folder in this.foldersToLoadDescendingOrder)
			{
				ContentPacks.MountLoadFolder(folder);
			}
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x000989E4 File Offset: 0x00096BE4
		public void ClearDestroy()
		{
			this.audioClips.ClearDestroy();
			this.textures.ClearDestroy();
			this.assetBundles.ClearDestroy();
			this.allAssetNamesInBundleCached = null;
			this.allAssetNamesInBundleCachedTrie = null;
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x00098A18 File Offset: 0x00096C18
		public ModContentHolder<T> GetContentHolder<T>() where T : class
		{
			if (typeof(T) == typeof(Texture2D))
			{
				return (ModContentHolder<T>)this.textures;
			}
			if (typeof(T) == typeof(AudioClip))
			{
				return (ModContentHolder<T>)this.audioClips;
			}
			if (typeof(T) == typeof(string))
			{
				return (ModContentHolder<T>)this.strings;
			}
			string str = "Mod lacks manager for asset type ";
			ModContentHolder<string> modContentHolder = this.strings;
			Log.Error(str + ((modContentHolder != null) ? modContentHolder.ToString() : null));
			return null;
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x00098ABC File Offset: 0x00096CBC
		private void ReloadContentInt(bool hotReload = false)
		{
			if (hotReload)
			{
				return;
			}
			DeepProfiler.Start("Reload audio clips");
			try
			{
				this.audioClips.ReloadAll(hotReload);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Reload textures");
			try
			{
				this.textures.ReloadAll(hotReload);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Reload strings");
			try
			{
				this.strings.ReloadAll(hotReload);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Reload asset bundles");
			try
			{
				this.assetBundles.ReloadAll(hotReload);
				this.allAssetNamesInBundleCached = null;
				this.allAssetNamesInBundleCachedTrie = null;
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x00098B88 File Offset: 0x00096D88
		public void ReloadContent(bool hotReload = false)
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.ReloadContentInt(hotReload);
			});
			if (!hotReload)
			{
				this.assemblies.ReloadAll();
			}
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x00098BC0 File Offset: 0x00096DC0
		public IEnumerable<LoadableXmlAsset> LoadDefs(bool hotReload = false)
		{
			ModContentPack.<LoadDefs>d__57 <LoadDefs>d__ = new ModContentPack.<LoadDefs>d__57(-2);
			<LoadDefs>d__.<>4__this = this;
			<LoadDefs>d__.<>3__hotReload = hotReload;
			return <LoadDefs>d__;
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x00098BD8 File Offset: 0x00096DD8
		private void InitLoadFolders()
		{
			this.foldersToLoadDescendingOrder = new List<string>();
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.PackageId, false);
			if (((modWithIdentifier != null) ? modWithIdentifier.loadFolders : null) != null && modWithIdentifier.loadFolders.DefinedVersions().Count > 0)
			{
				List<LoadFolder> list = modWithIdentifier.LoadFoldersForVersion(VersionControl.CurrentVersionString);
				if (list != null && list.Count > 0)
				{
					this.<InitLoadFolders>g__AddFolders|58_0(list);
					return;
				}
				string text = (from x in modWithIdentifier.loadFolders.DefinedVersions().Where(delegate(string x)
				{
					if (x == "default" || x.NullOrEmpty() || !x.Contains("."))
					{
						return false;
					}
					bool result;
					try
					{
						result = (VersionControl.VersionFromString(x) <= VersionControl.CurrentVersion);
					}
					catch
					{
						result = false;
					}
					return result;
				})
				orderby x descending
				select x).FirstOrDefault<string>();
				if (text != null)
				{
					List<LoadFolder> list2 = modWithIdentifier.LoadFoldersForVersion(text);
					if (list2 != null)
					{
						this.<InitLoadFolders>g__AddFolders|58_0(list2);
						return;
					}
				}
				List<LoadFolder> list3 = modWithIdentifier.LoadFoldersForVersion("default");
				if (list3 != null)
				{
					this.<InitLoadFolders>g__AddFolders|58_0(list3);
					return;
				}
			}
			if (this.foldersToLoadDescendingOrder.Count == 0)
			{
				string text2 = Path.Combine(this.RootDir, VersionControl.CurrentVersionStringWithoutBuild);
				if (Directory.Exists(text2))
				{
					this.foldersToLoadDescendingOrder.Add(text2);
				}
				else
				{
					Version version = new Version(0, 0);
					List<Version> list4 = new List<Version>();
					DirectoryInfo[] directories = this.rootDirInt.GetDirectories();
					for (int i = 0; i < directories.Length; i++)
					{
						Version item;
						if (VersionControl.TryParseVersionString(directories[i].Name, out item))
						{
							list4.Add(item);
						}
					}
					list4.Sort();
					foreach (Version version2 in list4)
					{
						if ((version2 > version || version > VersionControl.CurrentVersion) && (version2 <= VersionControl.CurrentVersion || version.Major == 0))
						{
							version = version2;
						}
					}
					if (version.Major > 0)
					{
						this.foldersToLoadDescendingOrder.Add(Path.Combine(this.RootDir, version.ToString()));
					}
				}
				string text3 = Path.Combine(this.RootDir, ModContentPack.CommonFolderName);
				if (Directory.Exists(text3))
				{
					this.foldersToLoadDescendingOrder.Add(text3);
				}
				this.foldersToLoadDescendingOrder.Add(this.RootDir);
			}
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x00098E30 File Offset: 0x00097030
		public static List<string> AllLoadFolders(ModMetaData mod)
		{
			return ModContentPack.AllLoadFolders(mod.RootDir.FullName, mod.loadFolders);
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x00098E48 File Offset: 0x00097048
		public static List<string> AllLoadFolders(string rootDir, ModLoadFolders loadFolders)
		{
			ModContentPack.<>c__DisplayClass61_0 CS$<>8__locals1;
			CS$<>8__locals1.folders = new List<string>();
			List<string> list = (loadFolders != null) ? loadFolders.DefinedVersions() : null;
			if (list != null && list.Count > 0)
			{
				List<ValueTuple<Version, string>> list2 = new List<ValueTuple<Version, string>>();
				foreach (string text in list)
				{
					Version item;
					if (VersionControl.TryParseVersionString(text, out item))
					{
						list2.Add(new ValueTuple<Version, string>(item, text));
					}
				}
				foreach (ValueTuple<Version, string> valueTuple in ModContentPack.VersionsInRange<string>(list2))
				{
					string item2 = valueTuple.Item2;
					foreach (LoadFolder loadFolder in loadFolders.FoldersForVersion(item2))
					{
						ModContentPack.<AllLoadFolders>g__Add|61_0(Path.Combine(rootDir, loadFolder.folderName), ref CS$<>8__locals1);
					}
				}
				List<LoadFolder> list3 = loadFolders.FoldersForVersion("default");
				if (list3 != null)
				{
					foreach (LoadFolder loadFolder2 in list3)
					{
						ModContentPack.<AllLoadFolders>g__Add|61_0(Path.Combine(rootDir, loadFolder2.folderName), ref CS$<>8__locals1);
					}
				}
				return CS$<>8__locals1.folders;
			}
			List<ValueTuple<Version, string>> list4 = new List<ValueTuple<Version, string>>();
			foreach (DirectoryInfo directoryInfo in new DirectoryInfo(rootDir).GetDirectories())
			{
				Version item3;
				if (VersionControl.TryParseVersionString(directoryInfo.Name, out item3))
				{
					list4.Add(new ValueTuple<Version, string>(item3, directoryInfo.FullName));
				}
			}
			foreach (ValueTuple<Version, string> valueTuple2 in ModContentPack.VersionsInRange<string>(list4))
			{
				ModContentPack.<AllLoadFolders>g__Add|61_0(valueTuple2.Item2, ref CS$<>8__locals1);
			}
			string text2 = Path.Combine(rootDir, ModContentPack.CommonFolderName);
			if (Directory.Exists(text2))
			{
				ModContentPack.<AllLoadFolders>g__Add|61_0(text2, ref CS$<>8__locals1);
			}
			ModContentPack.<AllLoadFolders>g__Add|61_0(rootDir, ref CS$<>8__locals1);
			return CS$<>8__locals1.folders;
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x00099098 File Offset: 0x00097298
		[return: TupleElementNames(new string[]
		{
			"version",
			"item"
		})]
		private static IEnumerable<ValueTuple<Version, T>> VersionsInRange<T>([TupleElementNames(new string[]
		{
			"version",
			"item"
		})] List<ValueTuple<Version, T>> candidates)
		{
			ModContentPack.<VersionsInRange>d__62<T> <VersionsInRange>d__ = new ModContentPack.<VersionsInRange>d__62<T>(-2);
			<VersionsInRange>d__.<>3__candidates = candidates;
			return <VersionsInRange>d__;
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x000990A8 File Offset: 0x000972A8
		private void LoadPatches()
		{
			DeepProfiler.Start("Loading all patches");
			this.patches = new List<PatchOperation>();
			this.loadedAnyPatches = false;
			List<LoadableXmlAsset> list = DirectXmlLoader.XmlAssetsInModFolder(this, "Patches/", null).ToList<LoadableXmlAsset>();
			for (int i = 0; i < list.Count; i++)
			{
				XmlElement documentElement = list[i].xmlDoc.DocumentElement;
				if (documentElement.Name != "Patch")
				{
					Log.Error(string.Format("Unexpected document element in patch XML; got {0}, expected 'Patch'", documentElement.Name));
				}
				else
				{
					foreach (object obj in documentElement.ChildNodes)
					{
						XmlNode xmlNode = (XmlNode)obj;
						if (xmlNode.NodeType == XmlNodeType.Element)
						{
							if (xmlNode.Name != "Operation")
							{
								Log.Error(string.Format("Unexpected element in patch XML; got {0}, expected 'Operation'", xmlNode.Name));
							}
							else
							{
								PatchOperation patchOperation = DirectXmlToObject.ObjectFromXml<PatchOperation>(xmlNode, false);
								patchOperation.sourceFile = list[i].FullFilePath;
								this.patches.Add(patchOperation);
								this.loadedAnyPatches = true;
							}
						}
					}
				}
			}
			DeepProfiler.End();
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x000991EC File Offset: 0x000973EC
		public static Dictionary<string, FileInfo> GetAllFilesForMod(ModContentPack mod, string contentPath, Func<string, bool> validateExtension = null, List<string> foldersToLoadDebug = null)
		{
			List<string> list = foldersToLoadDebug ?? mod.foldersToLoadDescendingOrder;
			Dictionary<string, FileInfo> dictionary = new Dictionary<string, FileInfo>();
			for (int i = 0; i < list.Count; i++)
			{
				string text = list[i];
				string text2 = Path.Combine(text, contentPath);
				if (ContentPacks.DirectoryExists(text2))
				{
					List<string> list2 = ContentPacks.EnumerateFiles(text2, true).ToList<string>();
					list2.Sort(StringComparer.Ordinal);
					for (int j = 0; j < list2.Count; j++)
					{
						FileInfo fileInfo = new FileInfo(list2[j]);
						if (validateExtension == null || validateExtension(fileInfo.Extension))
						{
							string key = fileInfo.FullName.Substring(text.Length + 1);
							if (!dictionary.ContainsKey(key))
							{
								dictionary.Add(key, fileInfo);
							}
						}
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x000992BC File Offset: 0x000974BC
		public static List<Tuple<string, FileInfo>> GetAllFilesForModPreserveOrder(ModContentPack mod, string contentPath, Func<string, bool> validateExtension = null, List<string> foldersToLoadDebug = null)
		{
			List<string> list = foldersToLoadDebug ?? mod.foldersToLoadDescendingOrder;
			List<Tuple<string, FileInfo>> list2 = new List<Tuple<string, FileInfo>>();
			for (int i = list.Count - 1; i >= 0; i--)
			{
				string text = list[i];
				DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(text, contentPath));
				if (directoryInfo.Exists)
				{
					FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
					Array.Sort<FileInfo>(files, (FileInfo a, FileInfo b) => a.Name.CompareTo(b.Name));
					foreach (FileInfo fileInfo in files)
					{
						if (validateExtension == null || validateExtension(fileInfo.Extension))
						{
							string item = fileInfo.FullName.Substring(text.Length + 1);
							list2.Add(new Tuple<string, FileInfo>(item, fileInfo));
						}
					}
				}
			}
			HashSet<string> hashSet = new HashSet<string>();
			for (int k = list2.Count - 1; k >= 0; k--)
			{
				Tuple<string, FileInfo> tuple = list2[k];
				if (!hashSet.Contains(tuple.Item1))
				{
					hashSet.Add(tuple.Item1);
				}
				else
				{
					list2.RemoveAt(k);
				}
			}
			return list2;
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x000993EE File Offset: 0x000975EE
		public bool AnyContentLoaded()
		{
			return this.AnyNonTranslationContentLoaded() || this.AnyTranslationsLoaded();
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x00099408 File Offset: 0x00097608
		public bool AnyNonTranslationContentLoaded()
		{
			return (this.textures.contentList != null && this.textures.contentList.Count != 0) || (this.audioClips.contentList != null && this.audioClips.contentList.Count != 0) || (this.strings.contentList != null && this.strings.contentList.Count != 0) || !this.assemblies.loadedAssemblies.NullOrEmpty<Assembly>() || !this.assetBundles.loadedAssetBundles.NullOrEmpty<AssetBundle>() || this.loadedAnyPatches || this.AllDefs.Any<Def>();
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x000994BC File Offset: 0x000976BC
		public bool AnyTranslationsLoaded()
		{
			foreach (string path in this.foldersToLoadDescendingOrder)
			{
				string path2 = Path.Combine(path, "Languages");
				if (Directory.Exists(path2) && Directory.EnumerateFiles(path2, "*", SearchOption.AllDirectories).Any<string>())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x00099534 File Offset: 0x00097734
		public void ClearPatchesCache()
		{
			this.patches = null;
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x0009953D File Offset: 0x0009773D
		public void ClearDefs()
		{
			this.defs.Clear();
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x0009954A File Offset: 0x0009774A
		public void AddDef(Def def, string source = "Unknown")
		{
			def.modContentPack = this;
			def.fileName = source;
			this.defs.Add(def);
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x00099566 File Offset: 0x00097766
		public override string ToString()
		{
			return this.PackageIdPlayerFacing;
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x000995D0 File Offset: 0x000977D0
		[CompilerGenerated]
		private void <InitLoadFolders>g__AddFolders|58_0(List<LoadFolder> folders)
		{
			for (int i = folders.Count - 1; i >= 0; i--)
			{
				if (folders[i].ShouldLoad)
				{
					this.foldersToLoadDescendingOrder.Add(Path.Combine(this.RootDir, folders[i].folderName));
				}
			}
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x00099620 File Offset: 0x00097820
		[CompilerGenerated]
		internal static void <AllLoadFolders>g__Add|61_0(string folder, ref ModContentPack.<>c__DisplayClass61_0 A_1)
		{
			folder = Path.TrimEndingDirectorySeparator(Path.GetFullPath(folder));
			if (!A_1.folders.Contains(folder, StringComparer.OrdinalIgnoreCase))
			{
				A_1.folders.Add(folder);
			}
		}

		// Token: 0x040014FA RID: 5370
		private DirectoryInfo rootDirInt;

		// Token: 0x040014FB RID: 5371
		public int loadOrder;

		// Token: 0x040014FC RID: 5372
		private string nameInt;

		// Token: 0x040014FD RID: 5373
		private string packageIdInt;

		// Token: 0x040014FE RID: 5374
		private string packageIdPlayerFacingInt;

		// Token: 0x040014FF RID: 5375
		private bool official;

		// Token: 0x04001500 RID: 5376
		private ModContentHolder<AudioClip> audioClips;

		// Token: 0x04001501 RID: 5377
		private ModContentHolder<Texture2D> textures;

		// Token: 0x04001502 RID: 5378
		private ModContentHolder<string> strings;

		// Token: 0x04001503 RID: 5379
		public ModAssetBundlesHandler assetBundles;

		// Token: 0x04001504 RID: 5380
		public ModAssemblyHandler assemblies;

		// Token: 0x04001505 RID: 5381
		private List<PatchOperation> patches;

		// Token: 0x04001506 RID: 5382
		private List<Def> defs = new List<Def>();

		// Token: 0x04001507 RID: 5383
		private List<List<string>> allAssetNamesInBundleCached;

		// Token: 0x04001508 RID: 5384
		private List<StringTrieSet> allAssetNamesInBundleCachedTrie;

		// Token: 0x04001509 RID: 5385
		public List<string> foldersToLoadDescendingOrder;

		// Token: 0x0400150A RID: 5386
		private bool loadedAnyPatches;

		// Token: 0x0400150B RID: 5387
		public const string LudeonPackageIdAuthor = "ludeon";

		// Token: 0x0400150C RID: 5388
		public const string CoreModPackageId = "ludeon.rimworld";

		// Token: 0x0400150D RID: 5389
		public const string RoyaltyModPackageId = "ludeon.rimworld.royalty";

		// Token: 0x0400150E RID: 5390
		public const string IdeologyModPackageId = "ludeon.rimworld.ideology";

		// Token: 0x0400150F RID: 5391
		public const string BiotechModPackageId = "ludeon.rimworld.biotech";

		// Token: 0x04001510 RID: 5392
		public const string AnomalyModPackageId = "ludeon.rimworld.anomaly";

		// Token: 0x04001511 RID: 5393
		public const string OdysseyModPackageId = "ludeon.rimworld.odyssey";

		// Token: 0x04001512 RID: 5394
		public static readonly string[] ProductPackageIDs = new string[]
		{
			"ludeon.rimworld",
			"ludeon.rimworld.royalty",
			"ludeon.rimworld.ideology",
			"ludeon.rimworld.biotech",
			"ludeon.rimworld.anomaly",
			"ludeon.rimworld.odyssey"
		};

		// Token: 0x04001513 RID: 5395
		public static readonly string CommonFolderName = "Common";

		// Token: 0x04001514 RID: 5396
		public static readonly Version FirstAssetPackVersion = new Version(1, 7);
	}
}
