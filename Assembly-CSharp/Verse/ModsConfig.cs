using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000347 RID: 839
	public static class ModsConfig
	{
		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001A05 RID: 6661 RVA: 0x0009B55C File Offset: 0x0009975C
		public static IEnumerable<ModMetaData> ActiveModsInLoadOrder
		{
			get
			{
				ModLister.EnsureInit();
				if (ModsConfig.activeModsInLoadOrderCachedDirty)
				{
					ModsConfig.activeModsInLoadOrderCached.Clear();
					for (int i = 0; i < ModsConfig.data.activeMods.Count; i++)
					{
						ModsConfig.activeModsInLoadOrderCached.Add(ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[i], false));
					}
					ModsConfig.activeModsInLoadOrderCachedDirty = false;
				}
				return ModsConfig.activeModsInLoadOrderCached;
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001A06 RID: 6662 RVA: 0x0009B5C4 File Offset: 0x000997C4
		public static ExpansionDef LastInstalledExpansion
		{
			get
			{
				for (int i = ModsConfig.data.knownExpansions.Count - 1; i >= 0; i--)
				{
					ExpansionDef expansionWithIdentifier = ModLister.GetExpansionWithIdentifier(ModsConfig.data.knownExpansions[i]);
					if (expansionWithIdentifier != null && !expansionWithIdentifier.isCore && expansionWithIdentifier.Status != ExpansionStatus.NotInstalled)
					{
						return expansionWithIdentifier;
					}
				}
				return null;
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001A07 RID: 6663 RVA: 0x0009B61A File Offset: 0x0009981A
		public static bool RoyaltyActive
		{
			get
			{
				return ModsConfig.royaltyActive;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001A08 RID: 6664 RVA: 0x0009B621 File Offset: 0x00099821
		public static bool IdeologyActive
		{
			get
			{
				return ModsConfig.ideologyActive;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001A09 RID: 6665 RVA: 0x0009B628 File Offset: 0x00099828
		public static bool BiotechActive
		{
			get
			{
				return ModsConfig.biotechActive;
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001A0A RID: 6666 RVA: 0x0009B62F File Offset: 0x0009982F
		public static bool AnomalyActive
		{
			get
			{
				return ModsConfig.anomalyActive;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001A0B RID: 6667 RVA: 0x0009B636 File Offset: 0x00099836
		public static bool OdysseyActive
		{
			get
			{
				return ModsConfig.odysseyActive;
			}
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x0009B640 File Offset: 0x00099840
		static ModsConfig()
		{
			bool flag = false;
			bool flag2 = false;
			ModsConfig.data = DirectXmlLoader.ItemFromXmlFile<ModsConfig.ModsConfigData>(GenFilePaths.ModsConfigFilePath, true);
			if (ModsConfig.data.version != null)
			{
				bool flag3 = false;
				int num2;
				if (ModsConfig.data.version.Contains("."))
				{
					int num = VersionControl.MinorFromVersionString(ModsConfig.data.version);
					if (VersionControl.MajorFromVersionString(ModsConfig.data.version) != VersionControl.CurrentMajor || num != VersionControl.CurrentMinor)
					{
						flag3 = true;
					}
				}
				else if (ModsConfig.data.version.Length > 0 && ModsConfig.data.version.All(new Func<char, bool>(char.IsNumber)) && int.TryParse(ModsConfig.data.version, out num2) && num2 <= 2009)
				{
					flag3 = true;
				}
				if (flag3)
				{
					Log.Message(string.Concat(new string[]
					{
						"Mods config data is from version ",
						ModsConfig.data.version,
						" while we are running ",
						VersionControl.CurrentVersionStringWithRev,
						". Resetting."
					}));
					ModsConfig.data = new ModsConfig.ModsConfigData();
					flag = true;
				}
			}
			for (int i = 0; i < ModsConfig.data.activeMods.Count; i++)
			{
				string packageId = ModsConfig.data.activeMods[i];
				if (ModLister.GetModWithIdentifier(packageId, false) == null)
				{
					ModMetaData modMetaData = ModLister.AllInstalledMods.FirstOrDefault((ModMetaData m) => m.FolderName == packageId);
					if (modMetaData != null)
					{
						ModsConfig.data.activeMods[i] = modMetaData.PackageId;
						flag2 = true;
					}
					string text;
					if (ModsConfig.TryGetPackageIdWithoutExtraSteamPostfix(packageId, out text) && ModLister.GetModWithIdentifier(text, false) != null)
					{
						ModsConfig.data.activeMods[i] = text;
					}
				}
			}
			HashSet<string> hashSet = new HashSet<string>();
			foreach (ModMetaData modMetaData2 in ModLister.AllInstalledMods)
			{
				if (modMetaData2.Active)
				{
					if (hashSet.Contains(modMetaData2.PackageIdNonUnique))
					{
						modMetaData2.Active = false;
						Log.Warning("There was more than one enabled instance of mod with PackageID: " + modMetaData2.PackageIdNonUnique + ". Disabling the duplicates.");
						continue;
					}
					hashSet.Add(modMetaData2.PackageIdNonUnique);
				}
				if (!modMetaData2.IsCoreMod && modMetaData2.Official && ModsConfig.IsExpansionNew(modMetaData2.PackageId))
				{
					ModsConfig.SetActive(modMetaData2.PackageId, true);
					ModsConfig.AddKnownExpansion(modMetaData2.PackageId);
					int num3 = ModsConfig.data.activeMods.IndexOf(modMetaData2.PackageId);
					if (!modMetaData2.ForceLoadAfter.NullOrEmpty<string>())
					{
						foreach (string identifier in modMetaData2.ForceLoadAfter)
						{
							ModMetaData activeModWithIdentifier = ModLister.GetActiveModWithIdentifier(identifier, false);
							if (activeModWithIdentifier != null)
							{
								int num4 = ModsConfig.data.activeMods.IndexOf(activeModWithIdentifier.PackageId);
								if (num4 != -1 && num4 > num3)
								{
									string text2;
									ModsConfig.TryReorder(num3, num4, out text2);
									Gen.Swap<int>(ref num4, ref num3);
								}
							}
						}
					}
					if (!modMetaData2.ForceLoadBefore.NullOrEmpty<string>())
					{
						foreach (string identifier2 in modMetaData2.ForceLoadBefore)
						{
							ModMetaData activeModWithIdentifier2 = ModLister.GetActiveModWithIdentifier(identifier2, false);
							if (activeModWithIdentifier2 != null)
							{
								int num5 = ModsConfig.data.activeMods.IndexOf(activeModWithIdentifier2.PackageId);
								if (num5 != -1 && num5 < num3)
								{
									string text2;
									ModsConfig.TryReorder(num3, num5, out text2);
									Gen.Swap<int>(ref num5, ref num3);
								}
							}
						}
					}
					Prefs.Notify_NewExpansion();
					flag2 = true;
				}
			}
			if (ModsConfig.newKnownExpansions.Any<string>())
			{
				ModsConfig.newKnownExpansions.SortBy((string x) => ModsConfig.ExpansionsInReleaseOrder.IndexOf(x));
				ModsConfig.data.knownExpansions.AddRange(ModsConfig.newKnownExpansions);
				ModsConfig.newKnownExpansions.Clear();
			}
			if (!File.Exists(GenFilePaths.ModsConfigFilePath) || flag)
			{
				ModsConfig.Reset();
			}
			else if (flag2)
			{
				ModsConfig.Save();
			}
			List<string> ids;
			if (ModsConfig.TryGetCommandLineModList(out ids))
			{
				ModsConfig.ApplyCommandLineModList(ids);
			}
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x0009BB2C File Offset: 0x00099D2C
		private static bool TryGetCommandLineModList(out List<string> ids)
		{
			ids = null;
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			int i = 0;
			while (i < commandLineArgs.Length)
			{
				string text;
				if (commandLineArgs[i].StartsWith("-mod=", StringComparison.OrdinalIgnoreCase))
				{
					text = commandLineArgs[i].Substring(5);
				}
				else
				{
					if (!string.Equals(commandLineArgs[i], "-mod", StringComparison.OrdinalIgnoreCase) || i + 1 >= commandLineArgs.Length)
					{
						i++;
						continue;
					}
					text = commandLineArgs[i + 1];
				}
				ids = (from s in text.Split(new char[]
				{
					';',
					','
				}, StringSplitOptions.RemoveEmptyEntries)
				select s.Trim() into s
				where s.Length > 0
				select s).ToList<string>();
				return true;
			}
			return false;
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x0009BBFC File Offset: 0x00099DFC
		private static void ApplyCommandLineModList(List<string> ids)
		{
			ModsConfig.data.activeMods.Clear();
			ModsConfig.data.activeMods.Add("ludeon.rimworld");
			foreach (ModMetaData modMetaData in ModLister.AllInstalledMods)
			{
				if (modMetaData.Official && !modMetaData.IsCoreMod && modMetaData.VersionCompatible)
				{
					ModsConfig.data.activeMods.Add(modMetaData.PackageId);
				}
			}
			using (List<string>.Enumerator enumerator2 = ids.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					string id = enumerator2.Current;
					ModMetaData modMetaData2 = ModLister.GetModWithIdentifier(id.ToLowerInvariant().Trim(), true) ?? ModLister.AllInstalledMods.FirstOrDefault((ModMetaData m) => m.FolderName == id);
					if (modMetaData2 == null)
					{
						Log.Warning("-mod: no installed mod matches '" + id + "'");
					}
					else if (modMetaData2.Source != ContentSource.ModsFolder)
					{
						Log.Warning("-mod: '" + id + "' is not in the local Mods folder, ignoring");
					}
					else if (!ModsConfig.data.activeMods.Contains(modMetaData2.PackageId))
					{
						ModsConfig.data.activeMods.Add(modMetaData2.PackageId);
					}
				}
			}
			ModsConfig.commandLineModList = true;
			ModsConfig.activeModsInLoadOrderCachedDirty = true;
			ModsConfig.TrySortMods();
			Log.Message("Running with command line mod list: " + ModsConfig.data.activeMods.ToCommaList(false, false));
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x0009BDB0 File Offset: 0x00099FB0
		public static bool TryGetPackageIdWithoutExtraSteamPostfix(string packageId, out string nonSteamPackageId)
		{
			if (packageId.EndsWith(ModMetaData.SteamModPostfix))
			{
				nonSteamPackageId = packageId.Substring(0, packageId.Length - ModMetaData.SteamModPostfix.Length);
				return true;
			}
			nonSteamPackageId = null;
			return false;
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x0009BDE0 File Offset: 0x00099FE0
		public static void DeactivateNotInstalledMods(Action<string> logCallback = null)
		{
			for (int i = ModsConfig.data.activeMods.Count - 1; i >= 0; i--)
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[i], false);
				string identifier;
				if (modWithIdentifier == null && ModsConfig.TryGetPackageIdWithoutExtraSteamPostfix(ModsConfig.data.activeMods[i], out identifier))
				{
					modWithIdentifier = ModLister.GetModWithIdentifier(identifier, false);
				}
				if (modWithIdentifier == null)
				{
					if (logCallback != null)
					{
						logCallback("Deactivating " + ModsConfig.data.activeMods[i]);
					}
					ModsConfig.data.activeMods.RemoveAt(i);
				}
			}
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x0009BE80 File Offset: 0x0009A080
		public static void Reset()
		{
			ModsConfig.data.activeMods.Clear();
			ModsConfig.data.activeMods.Add("ludeon.rimworld");
			foreach (ModMetaData modMetaData in ModLister.AllInstalledMods)
			{
				if (modMetaData.Official && !modMetaData.IsCoreMod && modMetaData.VersionCompatible)
				{
					ModsConfig.data.activeMods.Add(modMetaData.PackageId);
				}
			}
			ModsConfig.activeModsInLoadOrderCachedDirty = true;
			ModsConfig.TrySortMods();
			ModsConfig.Save();
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x0009BF2C File Offset: 0x0009A12C
		public static bool TryReorder(int modIndex, int newIndex, out string errorMessage)
		{
			errorMessage = null;
			if (modIndex == newIndex)
			{
				return false;
			}
			if (ModsConfig.ReorderConflict(modIndex, newIndex, out errorMessage))
			{
				return false;
			}
			ModsConfig.data.activeMods.Insert(newIndex, ModsConfig.data.activeMods[modIndex]);
			ModsConfig.data.activeMods.RemoveAt((modIndex < newIndex) ? modIndex : (modIndex + 1));
			ModsConfig.activeModsInLoadOrderCachedDirty = true;
			return true;
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x0009BF90 File Offset: 0x0009A190
		private static bool ReorderConflict(int modIndex, int newIndex, out string errorMessage)
		{
			errorMessage = null;
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[modIndex], false);
			if (modWithIdentifier.IsCoreMod)
			{
				foreach (string text in ModsConfig.ExpansionsInReleaseOrder)
				{
					int num = ModsConfig.data.activeMods.IndexOf(text);
					if (num != -1 && num < newIndex)
					{
						errorMessage = "ModReorderConflict_MustLoadBefore".Translate(modWithIdentifier.Name, ModLister.GetModWithIdentifier(text, false).Name);
						return true;
					}
				}
			}
			if (modWithIdentifier.Source == ContentSource.OfficialModsFolder && ModsConfig.data.activeMods.IndexOf(ModsConfig.Core) >= newIndex)
			{
				errorMessage = "ModReorderConflict_MustLoadAfter".Translate(modWithIdentifier.Name, ModLister.GetModWithIdentifier(ModsConfig.Core, false).Name);
				return true;
			}
			if (!modWithIdentifier.ForceLoadBefore.NullOrEmpty<string>())
			{
				foreach (string identifier in modWithIdentifier.ForceLoadBefore)
				{
					ModMetaData modWithIdentifier2 = ModLister.GetModWithIdentifier(identifier, false);
					if (modWithIdentifier2 != null)
					{
						for (int i = newIndex - 1; i >= 0; i--)
						{
							if (modWithIdentifier2.SamePackageId(ModsConfig.data.activeMods[i], false))
							{
								errorMessage = "ModReorderConflict_MustLoadBefore".Translate(modWithIdentifier.Name, ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[i], false).Name);
								return true;
							}
						}
					}
				}
			}
			if (!modWithIdentifier.ForceLoadAfter.NullOrEmpty<string>())
			{
				foreach (string identifier2 in modWithIdentifier.ForceLoadAfter)
				{
					ModMetaData modWithIdentifier3 = ModLister.GetModWithIdentifier(identifier2, false);
					if (modWithIdentifier3 != null)
					{
						for (int j = newIndex; j < ModsConfig.data.activeMods.Count; j++)
						{
							if (modWithIdentifier3.SamePackageId(ModsConfig.data.activeMods[j], false))
							{
								errorMessage = "ModReorderConflict_MustLoadAfter".Translate(modWithIdentifier.Name, ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[j], false).Name);
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x0009C248 File Offset: 0x0009A448
		public static void Reorder(List<int> newIndices)
		{
			List<string> list = new List<string>();
			foreach (int index in newIndices)
			{
				list.Add(ModsConfig.data.activeMods[index]);
			}
			ModsConfig.data.activeMods = list;
			ModsConfig.activeModsInLoadOrderCachedDirty = true;
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x0009C2BC File Offset: 0x0009A4BC
		public static bool IsActive(ModMetaData mod)
		{
			return ModsConfig.IsActive(mod.PackageId);
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x0009C2C9 File Offset: 0x0009A4C9
		[Obsolete("Callers should use ModLister.AllModsActiveNoSuffix instead which automatically trims _steam suffixes.")]
		public static bool AreAllActive(string mods)
		{
			if (mods != null && mods.Contains(','))
			{
				return ModsConfig.AreAllActive(mods.ToLower().Split(',', StringSplitOptions.None));
			}
			return ModsConfig.IsActive(mods);
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x0009C2F4 File Offset: 0x0009A4F4
		[Obsolete("Callers should use ModLister.AllModsActiveNoSuffix instead which automatically trims _steam suffixes.")]
		public static bool AreAllActive(IEnumerable<string> mods)
		{
			using (IEnumerator<string> enumerator = mods.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!ModsConfig.IsActive(enumerator.Current.Trim()))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x0009C348 File Offset: 0x0009A548
		[Obsolete("Callers should use ModLister.AnyModActiveNoSuffix instead which automatically trims _steam suffixes.")]
		public static bool IsAnyActiveOrEmpty(IEnumerable<string> mods, bool trimNames = false)
		{
			if (!mods.EnumerableNullOrEmpty<string>())
			{
				foreach (string text in mods)
				{
					if (ModsConfig.IsActive(trimNames ? text.Trim() : text))
					{
						return true;
					}
				}
				return false;
			}
			return true;
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x0009C3B0 File Offset: 0x0009A5B0
		public static bool IsActive(string id)
		{
			return ModsConfig.activeModsHashSet.Contains(id.ToLower());
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x0009C3C2 File Offset: 0x0009A5C2
		public static void SetActive(ModMetaData mod, bool active)
		{
			ModsConfig.SetActive(mod.PackageId, active);
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x0009C3D0 File Offset: 0x0009A5D0
		public static void SetActive(string modIdentifier, bool active)
		{
			string item = modIdentifier.ToLower();
			if (active)
			{
				if (!ModsConfig.data.activeMods.Contains(item))
				{
					ModsConfig.data.activeMods.Add(item);
					ModsConfig.EnsureModAdheresToForcedLoadOrder(modIdentifier);
				}
			}
			else if (ModsConfig.data.activeMods.Contains(item))
			{
				ModsConfig.data.activeMods.Remove(item);
			}
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x0009C43C File Offset: 0x0009A63C
		public static void EnsureModAdheresToForcedLoadOrder(string modIdentifier)
		{
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(modIdentifier, false);
			if (modWithIdentifier == null)
			{
				return;
			}
			string item = modIdentifier.ToLower();
			if (!ModsConfig.data.activeMods.Contains(item))
			{
				return;
			}
			int? num = null;
			int num2 = ModsConfig.data.activeMods.IndexOf(item);
			if (!modWithIdentifier.ForceLoadAfter.NullOrEmpty<string>())
			{
				foreach (string text in modWithIdentifier.ForceLoadAfter)
				{
					int num3 = ModsConfig.data.activeMods.IndexOf(text.ToLower());
					if (num3 != -1)
					{
						num = new int?(Mathf.Max(num.GetValueOrDefault(int.MinValue), num3 + 1));
					}
				}
			}
			if (!modWithIdentifier.ForceLoadBefore.NullOrEmpty<string>())
			{
				foreach (string text2 in modWithIdentifier.ForceLoadBefore)
				{
					int num4 = ModsConfig.data.activeMods.IndexOf(text2.ToLower());
					if (num4 != -1)
					{
						num = new int?(Mathf.Min(num.GetValueOrDefault(int.MaxValue), num4));
					}
				}
			}
			if (num != null && num.Value != num2)
			{
				ModsConfig.data.activeMods.Remove(item);
				ModsConfig.data.activeMods.Insert(num.Value, item);
			}
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x0009C5CC File Offset: 0x0009A7CC
		public static void SetActiveToList(List<string> mods)
		{
			ModsConfig.data.activeMods = (from mod in mods
			where ModLister.GetModWithIdentifier(mod, false) != null
			select mod).ToList<string>();
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x0009C607 File Offset: 0x0009A807
		public static bool IsExpansionNew(string id)
		{
			return !ModsConfig.data.knownExpansions.Contains(id.ToLower());
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x0009C621 File Offset: 0x0009A821
		public static void AddKnownExpansion(string id)
		{
			if (!ModsConfig.IsExpansionNew(id))
			{
				Log.Error("Tried to add already known expansion: " + id);
				return;
			}
			ModsConfig.newKnownExpansions.Add(id.ToLower());
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x0009C64C File Offset: 0x0009A84C
		public static void Save()
		{
			if (ModsConfig.commandLineModList)
			{
				Log.Message("ModsConfig.xml not saved, the mod list came from the command line");
				return;
			}
			ModsConfig.data.version = VersionControl.CurrentVersionStringWithRev;
			DirectXmlSaver.SaveDataObject(ModsConfig.data, GenFilePaths.ModsConfigFilePath);
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x0009C680 File Offset: 0x0009A880
		public static void SaveFromList(List<string> mods)
		{
			if (ModsConfig.commandLineModList)
			{
				Log.Message("ModsConfig.xml not saved, the mod list came from the command line");
				return;
			}
			DirectXmlSaver.SaveDataObject(new ModsConfig.ModsConfigData
			{
				version = VersionControl.CurrentVersionStringWithRev,
				activeMods = mods,
				knownExpansions = ModsConfig.data.knownExpansions
			}, GenFilePaths.ModsConfigFilePath);
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x0009C6D0 File Offset: 0x0009A8D0
		public static void RestartFromChangedMods()
		{
			Find.WindowStack.Add(new Dialog_MessageBox("ModsChanged".Translate(), null, delegate()
			{
				GenCommandLine.Restart();
			}, null, null, null, false, null, null, WindowLayer.Dialog));
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x0009C720 File Offset: 0x0009A920
		public static Dictionary<string, string> GetModWarnings()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			List<ModMetaData> mods = ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>();
			for (int i = 0; i < mods.Count; i++)
			{
				int index = i;
				ModMetaData modMetaData = mods[index];
				StringBuilder stringBuilder = new StringBuilder("");
				ModMetaData activeModWithIdentifier = ModLister.GetActiveModWithIdentifier(modMetaData.PackageId, false);
				if (activeModWithIdentifier != null && modMetaData != activeModWithIdentifier)
				{
					stringBuilder.AppendLine("ModWithSameIdAlreadyActive".Translate(activeModWithIdentifier.Name));
				}
				List<string> list = ModsConfig.FindConflicts(modMetaData.IncompatibleWith, null);
				if (list.Any<string>())
				{
					stringBuilder.AppendLine("ModIncompatibleWithTip".Translate(list.ToCommaList(true, false)));
				}
				List<string> list2 = ModsConfig.FindConflicts(modMetaData.LoadBefore, (ModMetaData beforeMod) => mods.IndexOf(beforeMod) < index);
				if (list2.Any<string>())
				{
					stringBuilder.AppendLine("ModMustLoadBefore".Translate(list2.ToCommaList(true, false)));
				}
				List<string> list3 = ModsConfig.FindConflicts(modMetaData.ForceLoadBefore, (ModMetaData beforeMod) => mods.IndexOf(beforeMod) < index);
				if (list3.Any<string>())
				{
					stringBuilder.AppendLine("ModMustLoadBefore".Translate(list3.ToCommaList(true, false)));
				}
				List<string> list4 = ModsConfig.FindConflicts(modMetaData.LoadAfter, (ModMetaData afterMod) => mods.IndexOf(afterMod) > index);
				if (list4.Any<string>())
				{
					stringBuilder.AppendLine("ModMustLoadAfter".Translate(list4.ToCommaList(true, false)));
				}
				List<string> list5 = ModsConfig.FindConflicts(modMetaData.ForceLoadAfter, (ModMetaData afterMod) => mods.IndexOf(afterMod) > index);
				if (list5.Any<string>())
				{
					stringBuilder.AppendLine("ModMustLoadAfter".Translate(list5.ToCommaList(true, false)));
				}
				if (modMetaData.Dependencies.Any<ModDependency>())
				{
					List<string> list6 = modMetaData.UnsatisfiedDependencies();
					if (list6.Any<string>())
					{
						stringBuilder.AppendLine("ModUnsatisfiedDependency".Translate(list6.ToCommaList(true, false)));
					}
				}
				dictionary.Add(modMetaData.PackageId, stringBuilder.ToString().TrimEndNewlines());
			}
			return dictionary;
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x0009C998 File Offset: 0x0009AB98
		public static bool ModHasAnyOrderingIssues(ModMetaData mod)
		{
			List<ModMetaData> mods = ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>();
			int index = mods.IndexOf(mod);
			return index != -1 && (ModsConfig.FindConflicts(mod.LoadBefore, (ModMetaData beforeMod) => mods.IndexOf(beforeMod) < index).Count > 0 || ModsConfig.FindConflicts(mod.LoadAfter, (ModMetaData afterMod) => mods.IndexOf(afterMod) > index).Count > 0);
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x0009CA1C File Offset: 0x0009AC1C
		private static List<string> FindConflicts(List<string> modsToCheck, Func<ModMetaData, bool> predicate)
		{
			List<string> list = new List<string>();
			foreach (string identifier in modsToCheck)
			{
				ModMetaData activeModWithIdentifier = ModLister.GetActiveModWithIdentifier(identifier, true);
				if (activeModWithIdentifier != null && (predicate == null || predicate(activeModWithIdentifier)))
				{
					list.Add(activeModWithIdentifier.Name);
				}
			}
			return list;
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x0009CA8C File Offset: 0x0009AC8C
		public static void TrySortMods()
		{
			List<ModMetaData> list = ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>();
			DirectedAcyclicGraph directedAcyclicGraph = new DirectedAcyclicGraph(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				ModMetaData modMetaData = list[i];
				using (IEnumerator<string> enumerator = modMetaData.LoadBefore.Concat(modMetaData.ForceLoadBefore).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string before = enumerator.Current;
						ModMetaData modMetaData2 = list.FirstOrDefault((ModMetaData m) => m.SamePackageId(before, true));
						if (modMetaData2 != null)
						{
							directedAcyclicGraph.AddEdge(list.IndexOf(modMetaData2), i);
						}
					}
				}
				using (IEnumerator<string> enumerator = modMetaData.LoadAfter.Concat(modMetaData.ForceLoadAfter).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string after = enumerator.Current;
						ModMetaData modMetaData3 = list.FirstOrDefault((ModMetaData m) => m.SamePackageId(after, true));
						if (modMetaData3 != null)
						{
							directedAcyclicGraph.AddEdge(i, list.IndexOf(modMetaData3));
						}
					}
				}
			}
			int num = directedAcyclicGraph.FindCycle();
			if (num == -1)
			{
				ModsConfig.Reorder(directedAcyclicGraph.TopologicalSort());
				return;
			}
			if (Find.WindowStack != null)
			{
				Find.WindowStack.Add(new Dialog_MessageBox("ModCyclicDependency".Translate(list[num].Name), null, null, null, null, null, false, null, null, WindowLayer.Dialog));
				return;
			}
			Log.Error("ModCyclicDependency".Translate(list[num].Name));
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x0009CC44 File Offset: 0x0009AE44
		private static void RecacheActiveMods()
		{
			ModsConfig.activeModsHashSet.Clear();
			foreach (string item in ModsConfig.data.activeMods)
			{
				ModsConfig.activeModsHashSet.Add(item);
			}
			ModsConfig.royaltyActive = ModsConfig.IsActive("ludeon.rimworld.royalty");
			ModsConfig.ideologyActive = ModsConfig.IsActive("ludeon.rimworld.ideology");
			ModsConfig.biotechActive = ModsConfig.IsActive("ludeon.rimworld.biotech");
			ModsConfig.anomalyActive = ModsConfig.IsActive("ludeon.rimworld.anomaly");
			ModsConfig.odysseyActive = ModsConfig.IsActive("ludeon.rimworld.odyssey");
			ModsConfig.activeModsInLoadOrderCachedDirty = true;
		}

		// Token: 0x0400154A RID: 5450
		private static ModsConfig.ModsConfigData data;

		// Token: 0x0400154B RID: 5451
		private static bool royaltyActive;

		// Token: 0x0400154C RID: 5452
		private static bool ideologyActive;

		// Token: 0x0400154D RID: 5453
		private static bool biotechActive;

		// Token: 0x0400154E RID: 5454
		private static bool anomalyActive;

		// Token: 0x0400154F RID: 5455
		private static bool odysseyActive;

		// Token: 0x04001550 RID: 5456
		private static HashSet<string> activeModsHashSet = new HashSet<string>();

		// Token: 0x04001551 RID: 5457
		private static List<ModMetaData> activeModsInLoadOrderCached = new List<ModMetaData>();

		// Token: 0x04001552 RID: 5458
		private static bool activeModsInLoadOrderCachedDirty;

		// Token: 0x04001553 RID: 5459
		private static List<string> newKnownExpansions = new List<string>();

		// Token: 0x04001554 RID: 5460
		private static readonly string Core = "ludeon.rimworld";

		// Token: 0x04001555 RID: 5461
		private static readonly List<string> ExpansionsInReleaseOrder = new List<string>
		{
			"ludeon.rimworld.royalty",
			"ludeon.rimworld.ideology",
			"ludeon.rimworld.biotech",
			"ludeon.rimworld.anomaly",
			"ludeon.rimworld.odyssey"
		};

		// Token: 0x04001556 RID: 5462
		private static bool commandLineModList;

		// Token: 0x0200267C RID: 9852
		private class ModsConfigData
		{
			// Token: 0x04009DC0 RID: 40384
			[LoadAlias("buildNumber")]
			public string version;

			// Token: 0x04009DC1 RID: 40385
			public List<string> activeMods = new List<string>();

			// Token: 0x04009DC2 RID: 40386
			public List<string> knownExpansions = new List<string>();
		}
	}
}
