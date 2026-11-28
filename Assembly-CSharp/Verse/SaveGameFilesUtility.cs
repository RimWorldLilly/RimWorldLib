using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Verse
{
	// Token: 0x0200051C RID: 1308
	public static class SaveGameFilesUtility
	{
		// Token: 0x060026C9 RID: 9929 RVA: 0x000F0944 File Offset: 0x000EEB44
		public static bool IsAutoSave(string fileName)
		{
			return fileName.Length >= 8 && fileName.Substring(0, 8) == "Autosave";
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x000F0964 File Offset: 0x000EEB64
		public static bool SavedGameNamedExists(string fileName)
		{
			using (IEnumerator<string> enumerator = (from f in GenFilePaths.AllSavedGameFiles
			select Path.GetFileNameWithoutExtension(f.Name)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == fileName)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x000F09DC File Offset: 0x000EEBDC
		public static string UnusedDefaultFileName(string factionLabel)
		{
			int num = 1;
			string text;
			do
			{
				text = factionLabel + num.ToString();
				num++;
			}
			while (SaveGameFilesUtility.SavedGameNamedExists(text));
			return text;
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x000F0A0C File Offset: 0x000EEC0C
		public static FileInfo GetAutostartSaveFile()
		{
			if (!Prefs.DevMode)
			{
				return null;
			}
			return GenFilePaths.AllSavedGameFiles.FirstOrDefault((FileInfo x) => Path.GetFileNameWithoutExtension(x.Name).ToLower() == "autostart".ToLower());
		}
	}
}
