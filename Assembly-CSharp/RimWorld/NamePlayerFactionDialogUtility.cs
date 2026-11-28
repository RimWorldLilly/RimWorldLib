using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001CBC RID: 7356
	public static class NamePlayerFactionDialogUtility
	{
		// Token: 0x0600BCEB RID: 48363 RVA: 0x00439F62 File Offset: 0x00438162
		public static bool IsValidName(string s)
		{
			return s.Length != 0 && s.Length <= 64 && GenText.IsValidFilename(s) && !GrammarResolver.ContainsSpecialChars(s);
		}

		// Token: 0x0600BCEC RID: 48364 RVA: 0x00439F90 File Offset: 0x00438190
		public static void Named(string s)
		{
			Faction.OfPlayer.Name = s;
			if (Find.GameInfo.permadeathMode)
			{
				string oldSavefileName = Find.GameInfo.permadeathModeUniqueName;
				string newSavefileName = PermadeathModeUtility.GeneratePermadeathSaveNameBasedOnPlayerInput(s, oldSavefileName);
				if (oldSavefileName != newSavefileName)
				{
					Func<FileInfo, bool> <>9__1;
					LongEventHandler.QueueLongEvent(delegate()
					{
						Find.GameInfo.permadeathModeUniqueName = newSavefileName;
						Find.Autosaver.DoAutosave();
						IEnumerable<FileInfo> allSavedGameFiles = GenFilePaths.AllSavedGameFiles;
						Func<FileInfo, bool> predicate;
						if ((predicate = <>9__1) == null)
						{
							predicate = (<>9__1 = ((FileInfo x) => Path.GetFileNameWithoutExtension(x.Name) == oldSavefileName));
						}
						FileInfo fileInfo = allSavedGameFiles.FirstOrDefault(predicate);
						if (fileInfo != null)
						{
							fileInfo.Delete();
						}
					}, "Autosaving", false, null, true, false, null);
				}
			}
		}

		// Token: 0x04006C00 RID: 27648
		public const int CharacterLimit = 64;
	}
}
