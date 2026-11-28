using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x0200007E RID: 126
	public static class GenDictionary
	{
		// Token: 0x0600053F RID: 1343 RVA: 0x0001E900 File Offset: 0x0001CB00
		public static string ToStringFullContents<K, V>(this Dictionary<K, V> dict)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<K, V> keyValuePair in dict)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				K key = keyValuePair.Key;
				string str = key.ToString();
				string str2 = ": ";
				V value = keyValuePair.Value;
				stringBuilder2.AppendLine(str + str2 + value.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0001E990 File Offset: 0x0001CB90
		public static bool NullOrEmpty<K, V>(this Dictionary<K, V> dict)
		{
			return dict == null || dict.Count == 0;
		}
	}
}
