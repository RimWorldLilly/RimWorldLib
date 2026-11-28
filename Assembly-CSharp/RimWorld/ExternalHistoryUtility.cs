using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200102D RID: 4141
	public static class ExternalHistoryUtility
	{
		// Token: 0x06006856 RID: 26710 RVA: 0x0024DFB8 File Offset: 0x0024C1B8
		static ExternalHistoryUtility()
		{
			try
			{
				ExternalHistoryUtility.cachedFiles = GenFilePaths.AllExternalHistoryFiles.ToList<FileInfo>();
			}
			catch (Exception ex)
			{
				Log.Error("Could not get external history files: " + ex.Message);
			}
		}

		// Token: 0x170011ED RID: 4589
		// (get) Token: 0x06006857 RID: 26711 RVA: 0x0024E010 File Offset: 0x0024C210
		public static IEnumerable<FileInfo> Files
		{
			get
			{
				return new ExternalHistoryUtility.<get_Files>d__5(-2);
			}
		}

		// Token: 0x06006858 RID: 26712 RVA: 0x0024E01C File Offset: 0x0024C21C
		public static ExternalHistory Load(string path)
		{
			ExternalHistory result = null;
			try
			{
				result = new ExternalHistory();
				Scribe.loader.InitLoading(path);
				try
				{
					Scribe_Deep.Look<ExternalHistory>(ref result, "externalHistory", Array.Empty<object>());
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
			}
			catch (Exception ex)
			{
				Log.Error("Could not load external history (" + path + "): " + ex.Message);
				return null;
			}
			return result;
		}

		// Token: 0x06006859 RID: 26713 RVA: 0x0024E0A4 File Offset: 0x0024C2A4
		public static string GetRandomGameplayID()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < ExternalHistoryUtility.gameplayIDLength; i++)
			{
				int index = Rand.Range(0, ExternalHistoryUtility.gameplayIDAvailableChars.Length);
				stringBuilder.Append(ExternalHistoryUtility.gameplayIDAvailableChars[index]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600685A RID: 26714 RVA: 0x0024E0F0 File Offset: 0x0024C2F0
		public static bool IsValidGameplayID(string ID)
		{
			if (ID.NullOrEmpty() || ID.Length != ExternalHistoryUtility.gameplayIDLength)
			{
				return false;
			}
			for (int i = 0; i < ID.Length; i++)
			{
				bool flag = false;
				for (int j = 0; j < ExternalHistoryUtility.gameplayIDAvailableChars.Length; j++)
				{
					if (ID[i] == ExternalHistoryUtility.gameplayIDAvailableChars[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600685B RID: 26715 RVA: 0x0024E15C File Offset: 0x0024C35C
		public static string GetCurrentUploadDate()
		{
			return DateTime.UtcNow.ToString("yyMMdd");
		}

		// Token: 0x0600685C RID: 26716 RVA: 0x0024E17C File Offset: 0x0024C37C
		public static int GetCurrentUploadTime()
		{
			return (int)(DateTime.UtcNow.TimeOfDay.TotalSeconds / 2.0);
		}

		// Token: 0x04004271 RID: 17009
		private static List<FileInfo> cachedFiles;

		// Token: 0x04004272 RID: 17010
		private static int gameplayIDLength = 20;

		// Token: 0x04004273 RID: 17011
		private static string gameplayIDAvailableChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
	}
}
