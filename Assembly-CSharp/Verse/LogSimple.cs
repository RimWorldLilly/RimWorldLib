using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006BE RID: 1726
	public static class LogSimple
	{
		// Token: 0x060036C2 RID: 14018 RVA: 0x0015FA84 File Offset: 0x0015DC84
		public static void Message(string text)
		{
			for (int i = 0; i < LogSimple.tabDepth; i++)
			{
				text = "  " + text;
			}
			LogSimple.messages.Add(text);
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x0015FAB9 File Offset: 0x0015DCB9
		public static void BeginTabMessage(string text)
		{
			LogSimple.Message(text);
			LogSimple.tabDepth++;
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x0015FACD File Offset: 0x0015DCCD
		public static void EndTab()
		{
			LogSimple.tabDepth--;
		}

		// Token: 0x060036C5 RID: 14021 RVA: 0x0015FADC File Offset: 0x0015DCDC
		public static void FlushToFileAndOpen()
		{
			LogSimple.<>c__DisplayClass5_0 CS$<>8__locals1 = new LogSimple.<>c__DisplayClass5_0();
			if (LogSimple.messages.Count == 0)
			{
				return;
			}
			string value = LogSimple.CompiledLog();
			LogSimple.<>c__DisplayClass5_0 CS$<>8__locals2 = CS$<>8__locals1;
			ReadOnlySpan<char> str = GenFilePaths.SaveDataFolderPath;
			char directorySeparatorChar = Path.DirectorySeparatorChar;
			CS$<>8__locals2.path = str + new ReadOnlySpan<char>(ref directorySeparatorChar) + "LogSimple.txt";
			using (StreamWriter streamWriter = new StreamWriter(CS$<>8__locals1.path, false))
			{
				streamWriter.Write(value);
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				Application.OpenURL(CS$<>8__locals1.path);
			});
			LogSimple.messages.Clear();
		}

		// Token: 0x060036C6 RID: 14022 RVA: 0x0015FB7C File Offset: 0x0015DD7C
		public static void FlushToStandardLog()
		{
			if (LogSimple.messages.Count == 0)
			{
				return;
			}
			Log.Message(LogSimple.CompiledLog());
			LogSimple.messages.Clear();
		}

		// Token: 0x060036C7 RID: 14023 RVA: 0x0015FBA0 File Offset: 0x0015DDA0
		private static string CompiledLog()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in LogSimple.messages)
			{
				stringBuilder.AppendLine(value);
			}
			return stringBuilder.ToString().TrimEnd();
		}

		// Token: 0x04002404 RID: 9220
		private static List<string> messages = new List<string>();

		// Token: 0x04002405 RID: 9221
		private static int tabDepth = 0;
	}
}
