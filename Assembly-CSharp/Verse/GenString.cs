using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000081 RID: 129
	public static class GenString
	{
		// Token: 0x0600058B RID: 1419 RVA: 0x0002026C File Offset: 0x0001E46C
		static GenString()
		{
			for (int i = 0; i < 10000; i++)
			{
				GenString.numberStrings[i] = (i - 5000).ToString();
			}
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x000202AE File Offset: 0x0001E4AE
		public static string ToStringCached(this int num)
		{
			if (num < -4999)
			{
				return num.ToString();
			}
			if (num > 4999)
			{
				return num.ToString();
			}
			return GenString.numberStrings[num + 5000];
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x000202DD File Offset: 0x0001E4DD
		public static IEnumerable<string> SplitBy(this string str, int chunkLength)
		{
			GenString.<SplitBy>d__3 <SplitBy>d__ = new GenString.<SplitBy>d__3(-2);
			<SplitBy>d__.<>3__str = str;
			<SplitBy>d__.<>3__chunkLength = chunkLength;
			return <SplitBy>d__;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x000202F4 File Offset: 0x0001E4F4
		public static StringBuilder AppendLineIfNotEmpty(this StringBuilder sb)
		{
			if (sb.Length > 0)
			{
				sb.AppendLine();
			}
			return sb;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00020307 File Offset: 0x0001E507
		public static int GetHashCodeSafe(this string val)
		{
			if (val == null)
			{
				return 0;
			}
			return val.GetHashCode();
		}

		// Token: 0x040001D5 RID: 469
		private static string[] numberStrings = new string[10000];
	}
}
