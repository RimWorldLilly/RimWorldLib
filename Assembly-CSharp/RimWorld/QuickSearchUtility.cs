using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020020AB RID: 8363
	public static class QuickSearchUtility
	{
		// Token: 0x170025D7 RID: 9687
		// (get) Token: 0x0600D2CB RID: 53963 RVA: 0x004D884C File Offset: 0x004D6A4C
		private static string[] SearchingTexts
		{
			get
			{
				if (QuickSearchUtility.searchingTexts == null)
				{
					string str = "Searching".Translate();
					QuickSearchUtility.searchingTexts = new string[]
					{
						str + ".",
						str + "..",
						str + "..."
					};
				}
				return QuickSearchUtility.searchingTexts;
			}
		}

		// Token: 0x170025D8 RID: 9688
		// (get) Token: 0x0600D2CC RID: 53964 RVA: 0x004D88AA File Offset: 0x004D6AAA
		public static string CurrentSearchText
		{
			get
			{
				return QuickSearchUtility.SearchingTexts[Time.frameCount / 20 % QuickSearchUtility.searchingTexts.Length];
			}
		}

		// Token: 0x0600D2CD RID: 53965 RVA: 0x004D88C2 File Offset: 0x004D6AC2
		public static void ResetStaticData()
		{
			QuickSearchUtility.searchingTexts = null;
		}

		// Token: 0x040085A9 RID: 34217
		private static string[] searchingTexts;
	}
}
