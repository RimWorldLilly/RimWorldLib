using System;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001FDF RID: 8159
	public static class VersionUpdateDialogMaker
	{
		// Token: 0x0600CEFC RID: 52988 RVA: 0x004C3FF4 File Offset: 0x004C21F4
		public static void CreateVersionUpdateDialogIfNecessary()
		{
			if (!VersionUpdateDialogMaker.dialogDone && LastPlayedVersion.Version != null && (VersionControl.CurrentMajor != LastPlayedVersion.Version.Major || VersionControl.CurrentMinor != LastPlayedVersion.Version.Minor))
			{
				VersionUpdateDialogMaker.CreateNewVersionDialog();
			}
		}

		// Token: 0x0600CEFD RID: 52989 RVA: 0x004C4034 File Offset: 0x004C2234
		private static void CreateNewVersionDialog()
		{
			string value = LastPlayedVersion.Version.Major.ToString() + "." + LastPlayedVersion.Version.Minor.ToString();
			string value2 = VersionControl.CurrentMajor.ToString() + "." + VersionControl.CurrentMinor.ToString();
			string text = "GameUpdatedToNewVersionInitial".Translate(value, value2);
			text += "\n\n";
			if (BackCompatibility.IsSaveCompatibleWith(LastPlayedVersion.Version.ToString()))
			{
				text += "GameUpdatedToNewVersionSavesCompatible".Translate();
			}
			else
			{
				text += "GameUpdatedToNewVersionSavesIncompatible".Translate();
			}
			text += "\n\n";
			if (SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				text += "GameUpdatedToNewVersionSteamController".Translate();
			}
			else
			{
				text += "GameUpdatedToNewVersionSteam".Translate();
			}
			Find.WindowStack.Add(new Dialog_MessageBox(text, null, null, null, null, null, false, null, null, WindowLayer.Dialog));
			VersionUpdateDialogMaker.dialogDone = true;
		}

		// Token: 0x040083BD RID: 33725
		private static bool dialogDone;
	}
}
