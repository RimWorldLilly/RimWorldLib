using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x0200052B RID: 1323
	public class ScribeMetaHeaderUtility
	{
		// Token: 0x06002712 RID: 10002 RVA: 0x000F32F8 File Offset: 0x000F14F8
		public static void WriteMetaHeader()
		{
			if (Scribe.EnterNode("meta"))
			{
				try
				{
					string currentVersionStringWithRev = VersionControl.CurrentVersionStringWithRev;
					Scribe_Values.Look<string>(ref currentVersionStringWithRev, "gameVersion", null, false);
					List<string> list = (from mod in LoadedModManager.RunningMods
					select mod.PackageId).ToList<string>();
					Scribe_Collections.Look<string>(ref list, "modIds", LookMode.Undefined, Array.Empty<object>());
					List<uint> list2 = (from mod in LoadedModManager.RunningMods
					select mod.SteamAppId).ToList<uint>();
					Scribe_Collections.Look<uint>(ref list2, "modSteamIds", LookMode.Undefined, Array.Empty<object>());
					List<string> list3 = (from mod in LoadedModManager.RunningMods
					select mod.Name).ToList<string>();
					Scribe_Collections.Look<string>(ref list3, "modNames", LookMode.Undefined, Array.Empty<object>());
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x000F3404 File Offset: 0x000F1604
		public static void LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode mode, bool logVersionConflictWarning)
		{
			ScribeMetaHeaderUtility.loadedGameVersion = "Unknown";
			ScribeMetaHeaderUtility.loadedGameVersionMajor = 0;
			ScribeMetaHeaderUtility.loadedGameVersionMinor = 0;
			ScribeMetaHeaderUtility.loadedGameVersionBuild = 0;
			ScribeMetaHeaderUtility.loadedModIdsList = null;
			ScribeMetaHeaderUtility.loadedModNamesList = null;
			ScribeMetaHeaderUtility.modListChanged = false;
			ScribeMetaHeaderUtility.lastMode = mode;
			if (Scribe.mode != LoadSaveMode.Inactive && Scribe.EnterNode("meta"))
			{
				try
				{
					Scribe_Values.Look<string>(ref ScribeMetaHeaderUtility.loadedGameVersion, "gameVersion", null, false);
					Scribe_Collections.Look<string>(ref ScribeMetaHeaderUtility.loadedModIdsList, "modIds", LookMode.Undefined, Array.Empty<object>());
					Scribe_Collections.Look<string>(ref ScribeMetaHeaderUtility.loadedModNamesList, "modNames", LookMode.Undefined, Array.Empty<object>());
					if (Scribe.mode == LoadSaveMode.LoadingVars && !ScribeMetaHeaderUtility.loadedGameVersion.NullOrEmpty())
					{
						try
						{
							ScribeMetaHeaderUtility.loadedGameVersionMajor = VersionControl.MajorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion);
							ScribeMetaHeaderUtility.loadedGameVersionMinor = VersionControl.MinorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion);
							ScribeMetaHeaderUtility.loadedGameVersionBuild = VersionControl.BuildFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion);
						}
						catch (Exception ex)
						{
							string str = "Error parsing loaded version. ";
							Exception ex2 = ex;
							Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
						}
					}
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			ScribeMetaHeaderUtility.modListChanged = !ScribeMetaHeaderUtility.LoadedModsMatchesActiveModsNoInfo();
			if (logVersionConflictWarning && (mode == ScribeMetaHeaderUtility.ScribeHeaderMode.Map || !UnityData.isEditor) && !ScribeMetaHeaderUtility.VersionsMatch())
			{
				Log.Warning(string.Concat(new string[]
				{
					"Loaded file (",
					mode.ToString(),
					") is from version ",
					ScribeMetaHeaderUtility.loadedGameVersion,
					", we are running version ",
					VersionControl.CurrentVersionStringWithRev,
					"."
				}));
			}
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x000F3594 File Offset: 0x000F1794
		private static bool VersionsMatch()
		{
			return ScribeMetaHeaderUtility.loadedGameVersionBuild == VersionControl.BuildFromVersionString(VersionControl.CurrentVersionStringWithRev);
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x000F35A8 File Offset: 0x000F17A8
		public static bool TryCreateDialogsForVersionMismatchWarnings(Action confirmedAction)
		{
			string text = null;
			string title = null;
			if (!BackCompatibility.IsSaveCompatibleWith(ScribeMetaHeaderUtility.loadedGameVersion) && !ScribeMetaHeaderUtility.VersionsMatch())
			{
				title = "VersionMismatch".Translate();
				string value = ScribeMetaHeaderUtility.loadedGameVersion.NullOrEmpty() ? ("(" + "UnknownLower".TranslateSimple() + ")") : ScribeMetaHeaderUtility.loadedGameVersion;
				if (ScribeMetaHeaderUtility.lastMode == ScribeMetaHeaderUtility.ScribeHeaderMode.Map)
				{
					text = "SaveGameIncompatibleWarningText".Translate(value, VersionControl.CurrentVersionString);
				}
				else if (ScribeMetaHeaderUtility.lastMode == ScribeMetaHeaderUtility.ScribeHeaderMode.World)
				{
					text = "WorldFileVersionMismatch".Translate(value, VersionControl.CurrentVersionString);
				}
				else
				{
					text = "FileIncompatibleWarning".Translate(value, VersionControl.CurrentVersionString);
				}
			}
			string text2;
			string text3;
			if (!ScribeMetaHeaderUtility.LoadedModsMatchesActiveMods(out text2, out text3))
			{
				Find.WindowStack.Add(new Dialog_ModMismatch(confirmedAction, ScribeMetaHeaderUtility.loadedModIdsList, ScribeMetaHeaderUtility.loadedModNamesList));
				return true;
			}
			if (text != null)
			{
				Dialog_MessageBox dialog_MessageBox = Dialog_MessageBox.CreateConfirmation(text, confirmedAction, false, title, WindowLayer.Dialog);
				dialog_MessageBox.buttonAText = "LoadAnyway".Translate();
				Find.WindowStack.Add(dialog_MessageBox);
				return true;
			}
			return false;
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x000F36E8 File Offset: 0x000F18E8
		public static bool LoadedModsMatchesActiveMods(out string loadedModsSummary, out string runningModsSummary)
		{
			loadedModsSummary = null;
			runningModsSummary = null;
			List<string> list = (from mod in LoadedModManager.RunningMods
			select mod.PackageId).ToList<string>();
			List<string> b = (from mod in LoadedModManager.RunningMods
			select mod.FolderName).ToList<string>();
			if (ScribeMetaHeaderUtility.ModListsMatch(ScribeMetaHeaderUtility.loadedModIdsList, list) || ScribeMetaHeaderUtility.ModListsMatch(ScribeMetaHeaderUtility.loadedModIdsList, b))
			{
				return true;
			}
			if (ScribeMetaHeaderUtility.loadedModNamesList == null)
			{
				loadedModsSummary = "None".Translate();
			}
			else
			{
				loadedModsSummary = ScribeMetaHeaderUtility.loadedModNamesList.ToCommaList(false, false);
			}
			runningModsSummary = (from id in list
			select ModLister.GetModWithIdentifier(id, false).Name).ToCommaList(false, false);
			return false;
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x000F37CC File Offset: 0x000F19CC
		public static bool LoadedModsMatchesActiveModsNoInfo()
		{
			List<string> b = (from mod in LoadedModManager.RunningMods
			select mod.PackageId).ToList<string>();
			if (ScribeMetaHeaderUtility.ModListsMatch(ScribeMetaHeaderUtility.loadedModIdsList, b))
			{
				return true;
			}
			List<string> b2 = (from mod in LoadedModManager.RunningMods
			select mod.FolderName).ToList<string>();
			return ScribeMetaHeaderUtility.ModListsMatch(ScribeMetaHeaderUtility.loadedModIdsList, b2);
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x000F3854 File Offset: 0x000F1A54
		private static bool ModListsMatch(List<string> a, List<string> b)
		{
			if (a == null || b == null)
			{
				return false;
			}
			if (a.Count != b.Count)
			{
				return false;
			}
			for (int i = 0; i < a.Count; i++)
			{
				if (a[i].ToLower() != b[i].ToLower())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x000F38AC File Offset: 0x000F1AAC
		public static string GameVersionOf(FileInfo file)
		{
			if (!file.Exists)
			{
				throw new ArgumentException();
			}
			try
			{
				using (StreamReader streamReader = new StreamReader(file.FullName))
				{
					using (XmlTextReader xmlTextReader = new XmlTextReader(streamReader))
					{
						if (ScribeMetaHeaderUtility.ReadToMetaElement(xmlTextReader) && xmlTextReader.ReadToDescendant("gameVersion"))
						{
							return VersionControl.VersionStringWithoutRev(xmlTextReader.ReadString());
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Exception getting game version of " + file.Name + ": " + ex.ToString());
			}
			return null;
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x000F3968 File Offset: 0x000F1B68
		public static bool ReadToMetaElement(XmlTextReader textReader)
		{
			return ScribeMetaHeaderUtility.ReadToNextElement(textReader) && ScribeMetaHeaderUtility.ReadToNextElement(textReader) && !(textReader.Name != "meta");
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x000F3993 File Offset: 0x000F1B93
		private static bool ReadToNextElement(XmlTextReader textReader)
		{
			while (textReader.Read())
			{
				if (textReader.NodeType == XmlNodeType.Element)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001C2B RID: 7211
		private static ScribeMetaHeaderUtility.ScribeHeaderMode lastMode;

		// Token: 0x04001C2C RID: 7212
		public static string loadedGameVersion;

		// Token: 0x04001C2D RID: 7213
		public static int loadedGameVersionMajor;

		// Token: 0x04001C2E RID: 7214
		public static int loadedGameVersionMinor;

		// Token: 0x04001C2F RID: 7215
		public static int loadedGameVersionBuild;

		// Token: 0x04001C30 RID: 7216
		public static List<string> loadedModIdsList;

		// Token: 0x04001C31 RID: 7217
		public static List<string> loadedModNamesList;

		// Token: 0x04001C32 RID: 7218
		public static List<int> loadedModSteamIdsList;

		// Token: 0x04001C33 RID: 7219
		public static bool modListChanged;

		// Token: 0x04001C34 RID: 7220
		public const string MetaNodeName = "meta";

		// Token: 0x04001C35 RID: 7221
		public const string GameVersionNodeName = "gameVersion";

		// Token: 0x04001C36 RID: 7222
		public const string ModIdsNodeName = "modIds";

		// Token: 0x04001C37 RID: 7223
		public const string ModNamesNodeName = "modNames";

		// Token: 0x04001C38 RID: 7224
		public const string ModSteamIdsNodeName = "modSteamIds";

		// Token: 0x02002774 RID: 10100
		public enum ScribeHeaderMode
		{
			// Token: 0x0400A141 RID: 41281
			None,
			// Token: 0x0400A142 RID: 41282
			Map,
			// Token: 0x0400A143 RID: 41283
			World,
			// Token: 0x0400A144 RID: 41284
			Scenario,
			// Token: 0x0400A145 RID: 41285
			Ideo,
			// Token: 0x0400A146 RID: 41286
			Xenotype,
			// Token: 0x0400A147 RID: 41287
			Xenogerm,
			// Token: 0x0400A148 RID: 41288
			ModList,
			// Token: 0x0400A149 RID: 41289
			CameraConfig
		}
	}
}
