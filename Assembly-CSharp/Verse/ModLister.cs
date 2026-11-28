using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Steamworks;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000340 RID: 832
	public static class ModLister
	{
		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001980 RID: 6528 RVA: 0x0009964E File Offset: 0x0009784E
		public static IEnumerable<ModMetaData> AllInstalledMods
		{
			get
			{
				return ModLister.mods;
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06001981 RID: 6529 RVA: 0x00099658 File Offset: 0x00097858
		public static IEnumerable<DirectoryInfo> AllActiveModDirs
		{
			get
			{
				return from mod in ModLister.mods
				where mod.Active
				select mod.RootDir;
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001982 RID: 6530 RVA: 0x000996B4 File Offset: 0x000978B4
		public static List<ExpansionDef> AllExpansions
		{
			get
			{
				if (ModLister.AllExpansionsCached.NullOrEmpty<ExpansionDef>())
				{
					ModLister.AllExpansionsCached = DefDatabase<ExpansionDef>.AllDefsListForReading.Where(delegate(ExpansionDef e)
					{
						ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(e.linkedMod, false);
						return modWithIdentifier == null || modWithIdentifier.Official;
					}).ToList<ExpansionDef>();
				}
				return ModLister.AllExpansionsCached;
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001983 RID: 6531 RVA: 0x00099705 File Offset: 0x00097905
		public static bool RoyaltyInstalled
		{
			get
			{
				return ModLister.royaltyInstalled;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001984 RID: 6532 RVA: 0x0009970C File Offset: 0x0009790C
		public static bool IdeologyInstalled
		{
			get
			{
				return ModLister.ideologyInstalled;
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001985 RID: 6533 RVA: 0x00099713 File Offset: 0x00097913
		public static bool BiotechInstalled
		{
			get
			{
				return ModLister.biotechInstalled;
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001986 RID: 6534 RVA: 0x0009971A File Offset: 0x0009791A
		public static bool AnomalyInstalled
		{
			get
			{
				return ModLister.anomalyInstalled;
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001987 RID: 6535 RVA: 0x00099721 File Offset: 0x00097921
		public static bool OdysseyInstalled
		{
			get
			{
				return ModLister.odysseyInstalled;
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001988 RID: 6536 RVA: 0x00099728 File Offset: 0x00097928
		public static bool ShouldLogIssues
		{
			get
			{
				return !ModLister.modListBuilt && !ModLister.nestedRebuildInProgress;
			}
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x0009973B File Offset: 0x0009793B
		static ModLister()
		{
			ModLister.RebuildModList();
			ModLister.modListBuilt = true;
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x00099770 File Offset: 0x00097970
		public static void EnsureInit()
		{
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x00099774 File Offset: 0x00097974
		public static void RebuildModList()
		{
			ModLister.nestedRebuildInProgress = ModLister.rebuildingModList;
			ModLister.rebuildingModList = true;
			string s = "Rebuilding mods list";
			ModLister.mods.Clear();
			ModLister.modsByPackageId.Clear();
			ModLister.modsByPackageIdIgnorePostfix.Clear();
			WorkshopItems.EnsureInit();
			DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.LogErrors);
			s += "\nAdding official mods from content folder:";
			foreach (string localAbsPath in from d in new DirectoryInfo(GenFilePaths.OfficialModsFolderPath).GetDirectories()
			select d.FullName)
			{
				ModMetaData modMetaData = new ModMetaData(localAbsPath, true);
				if (ModLister.TryAddMod(modMetaData))
				{
					s = s + "\n  Adding " + modMetaData.ToStringLong();
				}
			}
			s += "\nAdding mods from mods folder:";
			foreach (string localAbsPath2 in from d in new DirectoryInfo(GenFilePaths.ModsFolderPath).GetDirectories()
			select d.FullName)
			{
				ModMetaData modMetaData2 = new ModMetaData(localAbsPath2, false);
				if (ModLister.TryAddMod(modMetaData2))
				{
					s = s + "\n  Adding " + modMetaData2.ToStringLong();
				}
			}
			s += "\nAdding mods from Steam:";
			foreach (WorkshopItem workshopItem in from it in WorkshopItems.AllSubscribedItems
			where it is WorkshopItem_Mod
			select it)
			{
				ModMetaData modMetaData3 = new ModMetaData(workshopItem);
				if (ModLister.TryAddMod(modMetaData3))
				{
					s = s + "\n  Adding " + modMetaData3.ToStringLong();
				}
			}
			s += "\nDeactivating not-installed mods:";
			ModsConfig.DeactivateNotInstalledMods(delegate(string log)
			{
				s = s + "\n   " + log;
			});
			if (ModLister.mods.Count((ModMetaData m) => m.Active) == 0)
			{
				s += "\nThere are no active mods. Activating Core mod.";
				ModLister.mods.First((ModMetaData m) => m.IsCoreMod).Active = true;
			}
			ModLister.RecacheExpansionsInstalled();
			if (Prefs.LogVerbose)
			{
				Log.Message(s);
			}
			ModLister.rebuildingModList = false;
			ModLister.nestedRebuildInProgress = false;
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x00099A6C File Offset: 0x00097C6C
		public static int InstalledModsListHash(bool activeOnly)
		{
			int num = 17;
			int num2 = 0;
			foreach (ModMetaData modMetaData in ModsConfig.ActiveModsInLoadOrder)
			{
				if (!activeOnly || ModsConfig.IsActive(modMetaData.PackageId))
				{
					num = num * 31 + modMetaData.GetHashCode();
					num = num * 31 + num2 * 2654241;
					num2++;
				}
			}
			return num;
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x00099AE4 File Offset: 0x00097CE4
		public static ModMetaData GetModWithIdentifier(string identifier, bool ignorePostfix = false)
		{
			if (ignorePostfix)
			{
				if (!ModLister.modsByPackageIdIgnorePostfix.ContainsKey(identifier))
				{
					return null;
				}
				return ModLister.modsByPackageIdIgnorePostfix[identifier].ElementAtOrDefault(0);
			}
			else
			{
				if (!ModLister.modsByPackageId.ContainsKey(identifier))
				{
					return null;
				}
				return ModLister.modsByPackageId[identifier].ElementAtOrDefault(0);
			}
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x00099B38 File Offset: 0x00097D38
		public static ModMetaData GetActiveModWithIdentifier(string identifier, bool ignorePostfix = false)
		{
			List<ModMetaData> list;
			if (!(ignorePostfix ? ModLister.modsByPackageIdIgnorePostfix : ModLister.modsByPackageId).TryGetValue(identifier.ToLowerInvariant().Trim(), out list))
			{
				return null;
			}
			foreach (ModMetaData modMetaData in list)
			{
				if (modMetaData.Active)
				{
					return modMetaData;
				}
			}
			return null;
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x00099BB4 File Offset: 0x00097DB4
		public static ExpansionDef GetExpansionWithIdentifier(string packageId)
		{
			for (int i = 0; i < ModLister.AllExpansions.Count; i++)
			{
				if (ModLister.AllExpansions[i].linkedMod == packageId)
				{
					return ModLister.AllExpansions[i];
				}
			}
			return null;
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x00099BFC File Offset: 0x00097DFC
		public static bool HasActiveModWithName(string name)
		{
			foreach (ModMetaData modMetaData in ModLister.mods)
			{
				if (modMetaData.Active && modMetaData.Name == name)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x00099C64 File Offset: 0x00097E64
		public static bool AnyFromListActive(List<string> mods)
		{
			using (List<string>.Enumerator enumerator = mods.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (ModLister.GetActiveModWithIdentifier(enumerator.Current, false) != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x00099CBC File Offset: 0x00097EBC
		public static bool AnyModActiveNoSuffix(List<string> modIds)
		{
			using (List<string>.Enumerator enumerator = modIds.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (ModLister.GetActiveModWithIdentifier(enumerator.Current.Trim(), true) != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x00099D18 File Offset: 0x00097F18
		public static bool AnyModActiveNoSuffix(IEnumerable<string> modIds)
		{
			using (IEnumerator<string> enumerator = modIds.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (ModLister.GetActiveModWithIdentifier(enumerator.Current.Trim(), true) != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x00099D6C File Offset: 0x00097F6C
		public static bool AllModsActiveNoSuffix(List<string> modIds)
		{
			using (List<string>.Enumerator enumerator = modIds.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (ModLister.GetActiveModWithIdentifier(enumerator.Current.Trim(), true) == null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x00099DC8 File Offset: 0x00097FC8
		public static bool AllModsActiveNoSuffix(IEnumerable<string> modIds)
		{
			using (IEnumerator<string> enumerator = modIds.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (ModLister.GetActiveModWithIdentifier(enumerator.Current.Trim(), true) == null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x00099E1C File Offset: 0x0009801C
		private static void RecacheExpansionsInstalled()
		{
			ModLister.royaltyInstalled = ModLister.modsByPackageId.ContainsKey("ludeon.rimworld.royalty");
			ModLister.ideologyInstalled = ModLister.modsByPackageId.ContainsKey("ludeon.rimworld.ideology");
			ModLister.biotechInstalled = ModLister.modsByPackageId.ContainsKey("ludeon.rimworld.biotech");
			ModLister.anomalyInstalled = ModLister.modsByPackageId.ContainsKey("ludeon.rimworld.anomaly");
			ModLister.odysseyInstalled = ModLister.modsByPackageId.ContainsKey("ludeon.rimworld.odyssey");
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x00099E90 File Offset: 0x00098090
		private static bool TryAddMod(ModMetaData mod)
		{
			if (mod.Official && !mod.IsCoreMod && SteamManager.Initialized && mod.SteamAppId != 0U)
			{
				bool flag = true;
				try
				{
					flag = SteamApps.BIsDlcInstalled(new AppId_t(mod.SteamAppId));
				}
				catch (Exception ex)
				{
					string str = "Could not determine if a DLC is installed: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
				if (!flag)
				{
					return false;
				}
			}
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(mod.PackageId, false);
			if (modWithIdentifier == null)
			{
				ModLister.mods.Add(mod);
				if (ModLister.modsByPackageId.ContainsKey(mod.PackageId))
				{
					ModLister.modsByPackageId[mod.PackageId].Add(mod);
				}
				else
				{
					ModLister.modsByPackageId.Add(mod.PackageId, new List<ModMetaData>
					{
						mod
					});
				}
				if (ModLister.modsByPackageIdIgnorePostfix.ContainsKey(mod.packageIdLowerCase))
				{
					ModLister.modsByPackageIdIgnorePostfix[mod.packageIdLowerCase].Add(mod);
				}
				else
				{
					ModLister.modsByPackageIdIgnorePostfix.Add(mod.packageIdLowerCase, new List<ModMetaData>
					{
						mod
					});
				}
				return true;
			}
			if (mod.RootDir.FullName != modWithIdentifier.RootDir.FullName)
			{
				if (mod.OnSteamWorkshop != modWithIdentifier.OnSteamWorkshop)
				{
					ModMetaData modMetaData = mod.OnSteamWorkshop ? mod : modWithIdentifier;
					if (!modMetaData.appendPackageIdSteamPostfix)
					{
						modMetaData.appendPackageIdSteamPostfix = true;
						return ModLister.TryAddMod(mod);
					}
				}
				Log.Error(string.Concat(new string[]
				{
					"Tried loading mod with the same packageId multiple times: ",
					mod.PackageIdPlayerFacing,
					". Ignoring the duplicates.\n",
					mod.RootDir.FullName,
					"\n",
					modWithIdentifier.RootDir.FullName
				}));
				return false;
			}
			return false;
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x0009A054 File Offset: 0x00098254
		private static bool CheckDLC(bool dlc, string featureName, string dlcNameIndef, string installedPropertyName)
		{
			if (!dlc)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(98, 3);
				defaultInterpolatedStringHandler.AppendFormatted(featureName);
				defaultInterpolatedStringHandler.AppendLiteral(" is ");
				defaultInterpolatedStringHandler.AppendFormatted(dlcNameIndef);
				defaultInterpolatedStringHandler.AppendLiteral("-specific game system. If you want to use this code please check ModLister.");
				defaultInterpolatedStringHandler.AppendFormatted(installedPropertyName);
				defaultInterpolatedStringHandler.AppendLiteral(" before calling it.");
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), featureName.GetHashCode());
			}
			return dlc;
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x0009A0BD File Offset: 0x000982BD
		public static bool CheckRoyalty(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled, featureNameSingular, "a Royalty", "RoyaltyInstalled");
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x0009A0D4 File Offset: 0x000982D4
		public static bool CheckIdeology(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.IdeologyInstalled, featureNameSingular, "an Ideology", "IdeologyInstalled");
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x0009A0EB File Offset: 0x000982EB
		public static bool CheckBiotech(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.BiotechInstalled, featureNameSingular, "a Biotech", "BiotechInstalled");
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x0009A102 File Offset: 0x00098302
		public static bool CheckAnomaly(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.AnomalyInstalled, featureNameSingular, "an Anomaly", "AnomalyInstalled");
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x0009A119 File Offset: 0x00098319
		public static bool CheckOdyssey(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.OdysseyInstalled, featureNameSingular, "an Odyssey", "OdysseyInstalled");
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x0009A130 File Offset: 0x00098330
		public static bool CheckIdeologyOrBiotech(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.IdeologyInstalled || ModLister.BiotechInstalled, featureNameSingular, "a Ideology or Biotech", "IdeologyInstalled or BiotechInstalled");
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x0009A151 File Offset: 0x00098351
		public static bool CheckRoyaltyAndIdeology(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled && ModLister.IdeologyInstalled, featureNameSingular, "a Royalty and Ideology", "RoyaltyInstalled and IdeologyInstalled");
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x0009A172 File Offset: 0x00098372
		public static bool CheckRoyaltyOrIdeology(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled || ModLister.IdeologyInstalled, featureNameSingular, "a Royalty or Ideology", "RoyaltyInstalled or IdeologyInstalled");
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x0009A193 File Offset: 0x00098393
		public static bool CheckRoyaltyOrBiotech(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled || ModLister.BiotechInstalled, featureNameSingular, "a Royalty or Biotech", "RoyaltyInstalled or BiotechInstalled");
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x0009A1B4 File Offset: 0x000983B4
		public static bool CheckRoyaltyOrAnomaly(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled || ModLister.AnomalyInstalled, featureNameSingular, "a Royalty or Anomaly", "RoyaltyInstalled or AnomalyInstalled");
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x0009A1D5 File Offset: 0x000983D5
		public static bool CheckBiotechOrAnomaly(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.BiotechInstalled || ModLister.AnomalyInstalled, featureNameSingular, "a Biotech or Anomaly", "BiotechInstalled or AnomalyInstalled");
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x0009A1F6 File Offset: 0x000983F6
		public static bool CheckRoyaltyOrOdyssey(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled || ModLister.OdysseyInstalled, featureNameSingular, "a Royalty or Odyssey", "RoyaltyInstalled or OdysseyInstalled");
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x0009A217 File Offset: 0x00098417
		public static bool CheckRoyaltyOrIdeologyOrBiotech(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled || ModLister.BiotechInstalled || ModLister.IdeologyInstalled, featureNameSingular, "a Royalty or Ideology or Biotech", "RoyaltyInstalled or IdeologyInstalled or BiotechInstalled");
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x0009A23F File Offset: 0x0009843F
		public static bool CheckBiotechOrAnomalyOrOdyssey(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.BiotechInstalled || ModLister.AnomalyInstalled || ModLister.OdysseyInstalled, featureNameSingular, "a Biotech or Anomaly or Odyssey", "BiotechInstalled or AnomalyInstalled or OdysseyInstalled");
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x0009A267 File Offset: 0x00098467
		public static bool CheckAnyExpansion(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled || ModLister.IdeologyInstalled || ModLister.BiotechInstalled || ModLister.AnomalyInstalled || ModLister.OdysseyInstalled, featureNameSingular, "a Royalty or Ideology or Biotech or Anomaly or Odyssey", "RoyaltyInstalled or IdeologyInstalled or BiotechInstalled or AnomalyInstalled or OdysseyInstalled");
		}

		// Token: 0x04001515 RID: 5397
		private static List<ModMetaData> mods = new List<ModMetaData>();

		// Token: 0x04001516 RID: 5398
		private static Dictionary<string, List<ModMetaData>> modsByPackageId = new Dictionary<string, List<ModMetaData>>(StringComparer.CurrentCultureIgnoreCase);

		// Token: 0x04001517 RID: 5399
		private static Dictionary<string, List<ModMetaData>> modsByPackageIdIgnorePostfix = new Dictionary<string, List<ModMetaData>>(StringComparer.CurrentCultureIgnoreCase);

		// Token: 0x04001518 RID: 5400
		private static bool modListBuilt;

		// Token: 0x04001519 RID: 5401
		private static bool rebuildingModList;

		// Token: 0x0400151A RID: 5402
		private static bool nestedRebuildInProgress;

		// Token: 0x0400151B RID: 5403
		private static List<ExpansionDef> AllExpansionsCached;

		// Token: 0x0400151C RID: 5404
		private static bool royaltyInstalled;

		// Token: 0x0400151D RID: 5405
		private static bool ideologyInstalled;

		// Token: 0x0400151E RID: 5406
		private static bool biotechInstalled;

		// Token: 0x0400151F RID: 5407
		private static bool anomalyInstalled;

		// Token: 0x04001520 RID: 5408
		private static bool odysseyInstalled;
	}
}
