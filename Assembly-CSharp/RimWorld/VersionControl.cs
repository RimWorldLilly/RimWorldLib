using System;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02000996 RID: 2454
	public static class VersionControl
	{
		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x060049DE RID: 18910 RVA: 0x001CBA8F File Offset: 0x001C9C8F
		public static Version CurrentVersion
		{
			get
			{
				return VersionControl.version;
			}
		}

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x060049DF RID: 18911 RVA: 0x001CBA96 File Offset: 0x001C9C96
		public static string CurrentVersionString
		{
			get
			{
				return VersionControl.versionString;
			}
		}

		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x060049E0 RID: 18912 RVA: 0x001CBA9D File Offset: 0x001C9C9D
		public static string CurrentVersionStringWithoutBuild
		{
			get
			{
				return VersionControl.versionStringWithoutBuild;
			}
		}

		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x060049E1 RID: 18913 RVA: 0x001CBAA4 File Offset: 0x001C9CA4
		public static string CurrentVersionStringWithRev
		{
			get
			{
				return VersionControl.versionStringWithRev;
			}
		}

		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x060049E2 RID: 18914 RVA: 0x001CBAAB File Offset: 0x001C9CAB
		public static int CurrentMajor
		{
			get
			{
				return VersionControl.version.Major;
			}
		}

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x060049E3 RID: 18915 RVA: 0x001CBAB7 File Offset: 0x001C9CB7
		public static int CurrentMinor
		{
			get
			{
				return VersionControl.version.Minor;
			}
		}

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x060049E4 RID: 18916 RVA: 0x001CBAC3 File Offset: 0x001C9CC3
		public static int CurrentBuild
		{
			get
			{
				return VersionControl.version.Build;
			}
		}

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x060049E5 RID: 18917 RVA: 0x001CBACF File Offset: 0x001C9CCF
		public static int CurrentRevision
		{
			get
			{
				return VersionControl.version.Revision;
			}
		}

		// Token: 0x17000D81 RID: 3457
		// (get) Token: 0x060049E6 RID: 18918 RVA: 0x001CBADB File Offset: 0x001C9CDB
		public static DateTime CurrentBuildDate
		{
			get
			{
				return VersionControl.buildDate;
			}
		}

		// Token: 0x060049E7 RID: 18919 RVA: 0x001CBAE4 File Offset: 0x001C9CE4
		static VersionControl()
		{
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			VersionControl.buildDate = new DateTime(2000, 1, 1).AddDays((double)version.Build);
			int build = version.Build - 4805;
			int revision = version.Revision * 2 / 60;
			VersionControl.version = new Version(version.Major, version.Minor, build, revision);
			VersionControl.versionStringWithRev = string.Concat(new string[]
			{
				VersionControl.version.Major.ToString(),
				".",
				VersionControl.version.Minor.ToString(),
				".",
				VersionControl.version.Build.ToString(),
				" rev",
				VersionControl.version.Revision.ToString()
			});
			VersionControl.versionString = string.Concat(new string[]
			{
				VersionControl.version.Major.ToString(),
				".",
				VersionControl.version.Minor.ToString(),
				".",
				VersionControl.version.Build.ToString()
			});
			VersionControl.versionStringWithoutBuild = VersionControl.version.Major.ToString() + "." + VersionControl.version.Minor.ToString();
		}

		// Token: 0x060049E8 RID: 18920 RVA: 0x001CBC6C File Offset: 0x001C9E6C
		public static void DrawInfoInCorner()
		{
			Text.Font = GameFont.Small;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			string text = "VersionIndicator".Translate(VersionControl.versionString);
			string versionExtraInfo = VersionControl.GetVersionExtraInfo();
			if (!versionExtraInfo.NullOrEmpty())
			{
				text = text + " (" + versionExtraInfo + ")";
			}
			text += "\n" + "CompiledOn".Translate(VersionControl.buildDate.ToString("MMM d yyyy"));
			if (SteamManager.Initialized)
			{
				text += "\n" + "LoggedIntoSteamAs".Translate(SteamUtility.SteamPersonaName);
			}
			Rect rect = new Rect(10f, 10f, 330f, Text.CalcHeight(text, 330f));
			Widgets.Label(rect, text);
			GUI.color = Color.white;
			LatestVersionGetter component = Current.Root.gameObject.GetComponent<LatestVersionGetter>();
			Rect rect2 = new Rect(10f, rect.yMax - 5f, 330f, 999f);
			component.DrawAt(rect2);
		}

		// Token: 0x060049E9 RID: 18921 RVA: 0x001CBDA8 File Offset: 0x001C9FA8
		private static string GetVersionExtraInfo()
		{
			string text = "";
			if (UnityData.Is32BitBuild)
			{
				text += "32-bit";
			}
			else if (UnityData.Is64BitBuild)
			{
				text += "64-bit";
			}
			return text;
		}

		// Token: 0x060049EA RID: 18922 RVA: 0x001CBDE4 File Offset: 0x001C9FE4
		public static void LogVersionNumber()
		{
			Log.Message("RimWorld " + VersionControl.versionStringWithRev);
		}

		// Token: 0x060049EB RID: 18923 RVA: 0x001CBDFA File Offset: 0x001C9FFA
		public static bool IsCompatible(Version v)
		{
			return v.Major == VersionControl.CurrentMajor && v.Minor == VersionControl.CurrentMinor;
		}

		// Token: 0x060049EC RID: 18924 RVA: 0x001CBE18 File Offset: 0x001CA018
		public static bool TryParseVersionString(string str, out Version version)
		{
			version = null;
			if (str == null)
			{
				return false;
			}
			string[] array = str.Split('.', StringSplitOptions.None);
			if (array.Length < 2)
			{
				return false;
			}
			for (int i = 0; i < 2; i++)
			{
				int num;
				if (!int.TryParse(array[i], out num))
				{
					return false;
				}
				if (num < 0)
				{
					return false;
				}
			}
			version = new Version(int.Parse(array[0]), int.Parse(array[1]));
			return true;
		}

		// Token: 0x060049ED RID: 18925 RVA: 0x001CBE78 File Offset: 0x001CA078
		public static int BuildFromVersionString(string str)
		{
			str = VersionControl.VersionStringWithoutRev(str);
			int result = 0;
			string[] array = str.Split('.', StringSplitOptions.None);
			if (array.Length < 3 || !int.TryParse(array[2], out result))
			{
				Log.Warning("Could not get build from version string " + str);
			}
			return result;
		}

		// Token: 0x060049EE RID: 18926 RVA: 0x001CBEBC File Offset: 0x001CA0BC
		public static int MinorFromVersionString(string str)
		{
			str = VersionControl.VersionStringWithoutRev(str);
			int result = 0;
			string[] array = str.Split('.', StringSplitOptions.None);
			if (array.Length < 2 || !int.TryParse(array[1], out result))
			{
				Log.Warning("Could not get minor version from version string " + str);
			}
			return result;
		}

		// Token: 0x060049EF RID: 18927 RVA: 0x001CBF00 File Offset: 0x001CA100
		public static int MajorFromVersionString(string str)
		{
			str = VersionControl.VersionStringWithoutRev(str);
			int result = 0;
			if (!int.TryParse(str.Split('.', StringSplitOptions.None)[0], out result))
			{
				Log.Warning("Could not get major version from version string " + str);
			}
			return result;
		}

		// Token: 0x060049F0 RID: 18928 RVA: 0x001CBF3C File Offset: 0x001CA13C
		public static string VersionStringWithoutRev(string str)
		{
			return str.Split(' ', StringSplitOptions.None)[0];
		}

		// Token: 0x060049F1 RID: 18929 RVA: 0x001CBF4C File Offset: 0x001CA14C
		public static Version VersionFromString(string str)
		{
			if (str.NullOrEmpty())
			{
				throw new ArgumentException("str");
			}
			string[] array = str.Split('.', StringSplitOptions.None);
			if (array.Length > 3)
			{
				throw new ArgumentException("str");
			}
			int major = 0;
			int minor = 0;
			int build = 0;
			for (int i = 0; i < array.Length; i++)
			{
				int num;
				if (!int.TryParse(array[i], out num))
				{
					throw new ArgumentException("str");
				}
				if (num < 0)
				{
					throw new ArgumentException("str");
				}
				switch (i)
				{
				case 0:
					major = num;
					break;
				case 1:
					minor = num;
					break;
				case 2:
					build = num;
					break;
				}
			}
			return new Version(major, minor, build);
		}

		// Token: 0x060049F2 RID: 18930 RVA: 0x001CBFF4 File Offset: 0x001CA1F4
		public static bool IsWellFormattedVersionString(string str)
		{
			string[] array = str.Split('.', StringSplitOptions.None);
			if (array.Length != 2)
			{
				return false;
			}
			for (int i = 0; i < 2; i++)
			{
				int num;
				if (!int.TryParse(array[i], out num))
				{
					return false;
				}
				if (num < 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04002F5A RID: 12122
		private static Version version;

		// Token: 0x04002F5B RID: 12123
		private static string versionStringWithoutBuild;

		// Token: 0x04002F5C RID: 12124
		private static string versionString;

		// Token: 0x04002F5D RID: 12125
		private static string versionStringWithRev;

		// Token: 0x04002F5E RID: 12126
		private static DateTime buildDate;
	}
}
