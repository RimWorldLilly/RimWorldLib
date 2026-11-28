using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.ProfilerUI
{
	// Token: 0x0200078A RID: 1930
	internal static class ProfilerClipboard
	{
		// Token: 0x06003D03 RID: 15619 RVA: 0x00181891 File Offset: 0x0017FA91
		public static void Put(StringBuilder builder)
		{
			GUIUtility.systemCopyBuffer = ProfilerClipboard.AlignColumns(builder.ToString());
		}

		// Token: 0x06003D04 RID: 15620 RVA: 0x001818A4 File Offset: 0x0017FAA4
		public static string AlignColumns(string text)
		{
			string[] array = text.Replace("\r\n", "\n").Split('\n', StringSplitOptions.None);
			List<int> list = new List<int>();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				ReadOnlySpan<char> span = array2[i].AsSpan();
				int num = 0;
				int j = span.IndexOf('\t');
				while (j >= 0)
				{
					if (num >= list.Count)
					{
						list.Add(0);
					}
					list[num] = Math.Max(list[num], j);
					span = span.Slice(j + 1);
					j = span.IndexOf('\t');
					num++;
				}
			}
			if (list.Count == 0)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder(text.Length + text.Length / 2);
			bool flag = true;
			foreach (string text2 in array)
			{
				if (!flag)
				{
					stringBuilder.Append('\n');
				}
				flag = false;
				ReadOnlySpan<char> readOnlySpan = text2.AsSpan();
				int num2 = 0;
				int k = readOnlySpan.IndexOf('\t');
				while (k >= 0)
				{
					if (num2 > 0)
					{
						stringBuilder.Append("  ");
					}
					stringBuilder.Append(' ', list[num2] - k);
					stringBuilder.Append(readOnlySpan.Slice(0, k));
					readOnlySpan = readOnlySpan.Slice(k + 1);
					k = readOnlySpan.IndexOf('\t');
					num2++;
				}
				if (num2 > 0)
				{
					stringBuilder.Append("  ");
				}
				stringBuilder.Append(readOnlySpan);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003D05 RID: 15621 RVA: 0x00181A2D File Offset: 0x0017FC2D
		public static bool AppendRow(StringBuilder builder, IList<ProfilerRowData> rows, int index)
		{
			if (index < 0 || index >= rows.Count)
			{
				return false;
			}
			rows[index].AppendCopyLine(builder);
			return true;
		}

		// Token: 0x06003D06 RID: 15622 RVA: 0x00181A4C File Offset: 0x0017FC4C
		public static void AppendSubtree(StringBuilder builder, IList<ProfilerRowData> rows, int index)
		{
			int depth = rows[index].Depth;
			int num = index;
			while (num < rows.Count && (num == index || rows[num].Depth > depth))
			{
				rows[num].AppendCopyLine(builder, rows[num].Depth - depth);
				builder.AppendLine();
				num++;
			}
		}

		// Token: 0x06003D07 RID: 15623 RVA: 0x00181AAC File Offset: 0x0017FCAC
		public static void AppendRows(StringBuilder builder, IList<ProfilerRowData> rows)
		{
			for (int i = 0; i < rows.Count; i++)
			{
				rows[i].AppendCopyLine(builder);
				builder.AppendLine();
			}
		}

		// Token: 0x040027AC RID: 10156
		public const string CallTreeHeader = "%\ttotal\tself\tcalls\talloc\tscope";

		// Token: 0x040027AD RID: 10157
		public const string PieHeader = "%\ttotal\tself\tcalls\talloc\tzone";
	}
}
