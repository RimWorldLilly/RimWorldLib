using System;
using System.IO;
using System.Threading;

namespace Verse
{
	// Token: 0x0200051A RID: 1306
	public static class SafeSaver
	{
		// Token: 0x060026B5 RID: 9909 RVA: 0x000F0244 File Offset: 0x000EE444
		private static string GetFileFullPath(string path)
		{
			return Path.GetFullPath(path);
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x000F024C File Offset: 0x000EE44C
		private static string GetNewFileFullPath(string path)
		{
			return Path.GetFullPath(path + SafeSaver.NewFileSuffix);
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x000F025E File Offset: 0x000EE45E
		private static string GetOldFileFullPath(string path)
		{
			return Path.GetFullPath(path + SafeSaver.OldFileSuffix);
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x000F0270 File Offset: 0x000EE470
		public static void Save(string path, string documentElementName, Action saveAction, bool leaveOldFile = false)
		{
			try
			{
				SafeSaver.CleanSafeSaverFiles(path);
				if (!File.Exists(SafeSaver.GetFileFullPath(path)))
				{
					SafeSaver.DoSave(SafeSaver.GetFileFullPath(path), documentElementName, saveAction);
				}
				else
				{
					SafeSaver.DoSave(SafeSaver.GetNewFileFullPath(path), documentElementName, saveAction);
					try
					{
						SafeSaver.SafeMove(SafeSaver.GetFileFullPath(path), SafeSaver.GetOldFileFullPath(path));
					}
					catch (Exception ex)
					{
						string[] array = new string[6];
						array[0] = "Could not move file from \"";
						array[1] = SafeSaver.GetFileFullPath(path);
						array[2] = "\" to \"";
						array[3] = SafeSaver.GetOldFileFullPath(path);
						array[4] = "\": ";
						int num = 5;
						Exception ex2 = ex;
						array[num] = ((ex2 != null) ? ex2.ToString() : null);
						Log.Warning(string.Concat(array));
						throw;
					}
					try
					{
						SafeSaver.SafeMove(SafeSaver.GetNewFileFullPath(path), SafeSaver.GetFileFullPath(path));
					}
					catch (Exception ex3)
					{
						string[] array2 = new string[6];
						array2[0] = "Could not move file from \"";
						array2[1] = SafeSaver.GetNewFileFullPath(path);
						array2[2] = "\" to \"";
						array2[3] = SafeSaver.GetFileFullPath(path);
						array2[4] = "\": ";
						int num2 = 5;
						Exception ex4 = ex3;
						array2[num2] = ((ex4 != null) ? ex4.ToString() : null);
						Log.Warning(string.Concat(array2));
						SafeSaver.RemoveFileIfExists(SafeSaver.GetFileFullPath(path), false);
						SafeSaver.RemoveFileIfExists(SafeSaver.GetNewFileFullPath(path), false);
						try
						{
							SafeSaver.SafeMove(SafeSaver.GetOldFileFullPath(path), SafeSaver.GetFileFullPath(path));
						}
						catch (Exception ex5)
						{
							string[] array3 = new string[6];
							array3[0] = "Could not move file from \"";
							array3[1] = SafeSaver.GetOldFileFullPath(path);
							array3[2] = "\" back to \"";
							array3[3] = SafeSaver.GetFileFullPath(path);
							array3[4] = "\": ";
							int num3 = 5;
							Exception ex6 = ex5;
							array3[num3] = ((ex6 != null) ? ex6.ToString() : null);
							Log.Warning(string.Concat(array3));
						}
						throw;
					}
					if (!leaveOldFile)
					{
						SafeSaver.RemoveFileIfExists(SafeSaver.GetOldFileFullPath(path), true);
					}
				}
			}
			catch (Exception ex7)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(SafeSaver.GetFileFullPath(path), ex7.ToString()));
				throw;
			}
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x000F0490 File Offset: 0x000EE690
		private static void CleanSafeSaverFiles(string path)
		{
			SafeSaver.RemoveFileIfExists(SafeSaver.GetOldFileFullPath(path), true);
			SafeSaver.RemoveFileIfExists(SafeSaver.GetNewFileFullPath(path), true);
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x000F04AC File Offset: 0x000EE6AC
		private static void DoSave(string fullPath, string documentElementName, Action saveAction)
		{
			try
			{
				Scribe.saver.InitSaving(fullPath, documentElementName);
				saveAction();
				Scribe.saver.FinalizeSaving();
			}
			catch (Exception ex)
			{
				string str = "An exception was thrown during saving to \"";
				string str2 = "\": ";
				Exception ex2 = ex;
				Log.Warning(str + fullPath + str2 + ((ex2 != null) ? ex2.ToString() : null));
				Scribe.saver.ForceStop();
				SafeSaver.RemoveFileIfExists(fullPath, false);
				throw;
			}
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x000F0520 File Offset: 0x000EE720
		private static void RemoveFileIfExists(string path, bool rethrow)
		{
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch (Exception ex)
			{
				string str = "Could not remove file \"";
				string str2 = "\": ";
				Exception ex2 = ex;
				Log.Warning(str + path + str2 + ((ex2 != null) ? ex2.ToString() : null));
				if (rethrow)
				{
					throw;
				}
			}
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x000F0578 File Offset: 0x000EE778
		private static void SafeMove(string from, string to)
		{
			Exception ex = null;
			for (int i = 0; i < 50; i++)
			{
				try
				{
					File.Move(from, to);
					return;
				}
				catch (Exception ex2)
				{
					if (ex == null)
					{
						ex = ex2;
					}
				}
				Thread.Sleep(1);
			}
			throw ex;
		}

		// Token: 0x04001BEB RID: 7147
		private static readonly string NewFileSuffix = ".new";

		// Token: 0x04001BEC RID: 7148
		private static readonly string OldFileSuffix = ".old";
	}
}
