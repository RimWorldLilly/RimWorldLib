using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CFD RID: 7421
	public static class PageUtility
	{
		// Token: 0x0600BF83 RID: 49027 RVA: 0x004581B4 File Offset: 0x004563B4
		public static Page StitchedPages(IEnumerable<Page> pages)
		{
			List<Page> list = pages.ToList<Page>();
			if (list.Count == 0)
			{
				return null;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (i > 0)
				{
					list[i].prev = list[i - 1];
				}
				if (i < list.Count - 1)
				{
					list[i].next = list[i + 1];
				}
			}
			return list[0];
		}

		// Token: 0x0600BF84 RID: 49028 RVA: 0x00458223 File Offset: 0x00456423
		public static void InitGameStart()
		{
			LongEventHandler.QueueLongEvent(new Action(PageUtility.<InitGameStart>g__PreLoadAct|1_0), "Play", "GeneratingMap", true, null, true, false);
		}

		// Token: 0x0600BF85 RID: 49029 RVA: 0x00458244 File Offset: 0x00456444
		[CompilerGenerated]
		internal static void <InitGameStart>g__PreLoadAct|1_0()
		{
			Find.GameInitData.PrepForMapGen();
			Find.Scenario.PreMapGenerate();
		}
	}
}
