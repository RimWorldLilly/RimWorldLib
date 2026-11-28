using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000711 RID: 1809
	public static class PermadeathModeUtility
	{
		// Token: 0x06003927 RID: 14631 RVA: 0x0016ECCD File Offset: 0x0016CECD
		public static string GeneratePermadeathSaveName()
		{
			return PermadeathModeUtility.NewPermadeathSaveNameWithAppendedNumberIfNecessary(GenFile.SanitizedFileName(NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, null, false, null, null, null)), null);
		}

		// Token: 0x06003928 RID: 14632 RVA: 0x0016ECF3 File Offset: 0x0016CEF3
		public static string GeneratePermadeathSaveNameBasedOnPlayerInput(string factionName, string acceptedNameEvenIfTaken = null)
		{
			return PermadeathModeUtility.NewPermadeathSaveNameWithAppendedNumberIfNecessary(GenFile.SanitizedFileName(factionName), acceptedNameEvenIfTaken);
		}

		// Token: 0x06003929 RID: 14633 RVA: 0x0016ED04 File Offset: 0x0016CF04
		public static void CheckUpdatePermadeathModeUniqueNameOnGameLoad(string filename)
		{
			if (Current.Game.Info.permadeathMode && Current.Game.Info.permadeathModeUniqueName != filename)
			{
				Log.Warning("Savefile's name has changed and doesn't match permadeath mode's unique name. Fixing...");
				Current.Game.Info.permadeathModeUniqueName = filename;
			}
		}

		// Token: 0x0600392A RID: 14634 RVA: 0x0016ED54 File Offset: 0x0016CF54
		private static string NewPermadeathSaveNameWithAppendedNumberIfNecessary(string name, string acceptedNameEvenIfTaken = null)
		{
			int num = 0;
			string text;
			do
			{
				num++;
				text = name;
				if (num != 1)
				{
					text += num.ToString();
				}
				text = PermadeathModeUtility.AppendedPermadeathModeSuffix(text);
			}
			while (SaveGameFilesUtility.SavedGameNamedExists(text) && text != acceptedNameEvenIfTaken);
			return text;
		}

		// Token: 0x0600392B RID: 14635 RVA: 0x0016ED94 File Offset: 0x0016CF94
		private static string AppendedPermadeathModeSuffix(string str)
		{
			return str + " " + "PermadeathModeSaveSuffix".Translate();
		}
	}
}
