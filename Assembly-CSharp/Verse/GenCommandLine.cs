using System;
using System.Threading;
using Waylith;

namespace Verse
{
	// Token: 0x020006E4 RID: 1764
	public static class GenCommandLine
	{
		// Token: 0x060037DE RID: 14302 RVA: 0x00165B54 File Offset: 0x00163D54
		public static bool CommandLineArgPassed(string key)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (string.Compare(commandLineArgs[i], key, true) == 0 || string.Compare(commandLineArgs[i], "-" + key, true) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060037DF RID: 14303 RVA: 0x00165B9C File Offset: 0x00163D9C
		public static bool TryGetCommandLineArg(string key, out string value)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i].Contains('='))
				{
					string[] array = commandLineArgs[i].Split('=', StringSplitOptions.None);
					if (array.Length == 2 && (string.Compare(array[0], key, true) == 0 || string.Compare(array[0], "-" + key, true) == 0))
					{
						value = array[1];
						return true;
					}
				}
			}
			value = null;
			return false;
		}

		// Token: 0x060037E0 RID: 14304 RVA: 0x00165C08 File Offset: 0x00163E08
		public static void Restart()
		{
			try
			{
				RestartManager.RequestRestart();
				Root.Shutdown();
				LongEventHandler.QueueLongEvent(delegate()
				{
					Thread.Sleep(10000);
				}, "Restarting", true, null, true, false, null);
			}
			catch (Exception ex)
			{
				string str = "Error restarting: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				Find.WindowStack.Add(new Dialog_MessageBox("FailedToRestart".Translate(), null, null, null, null, null, false, null, null, WindowLayer.Dialog));
			}
		}
	}
}
