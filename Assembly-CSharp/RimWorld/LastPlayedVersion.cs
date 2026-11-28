using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FDE RID: 8158
	public static class LastPlayedVersion
	{
		// Token: 0x1700254F RID: 9551
		// (get) Token: 0x0600CEFA RID: 52986 RVA: 0x004C3F11 File Offset: 0x004C2111
		public static Version Version
		{
			get
			{
				LastPlayedVersion.InitializeIfNeeded();
				return LastPlayedVersion.lastPlayedVersionInt;
			}
		}

		// Token: 0x0600CEFB RID: 52987 RVA: 0x004C3F20 File Offset: 0x004C2120
		public static void InitializeIfNeeded()
		{
			if (LastPlayedVersion.initialized)
			{
				return;
			}
			try
			{
				string text = null;
				if (File.Exists(GenFilePaths.LastPlayedVersionFilePath))
				{
					try
					{
						text = File.ReadAllText(GenFilePaths.LastPlayedVersionFilePath);
					}
					catch (Exception ex)
					{
						Log.Error("Exception getting last played version data. Path: " + GenFilePaths.LastPlayedVersionFilePath + ". Exception: " + ex.ToString());
					}
				}
				if (text != null)
				{
					try
					{
						LastPlayedVersion.lastPlayedVersionInt = VersionControl.VersionFromString(text);
					}
					catch (Exception ex2)
					{
						Log.Error("Exception parsing last version from string '" + text + "': " + ex2.ToString());
					}
				}
				if (LastPlayedVersion.lastPlayedVersionInt != VersionControl.CurrentVersion)
				{
					File.WriteAllText(GenFilePaths.LastPlayedVersionFilePath, VersionControl.CurrentVersionString);
				}
			}
			finally
			{
				LastPlayedVersion.initialized = true;
			}
		}

		// Token: 0x040083BB RID: 33723
		private static bool initialized;

		// Token: 0x040083BC RID: 33724
		private static Version lastPlayedVersionInt;
	}
}
