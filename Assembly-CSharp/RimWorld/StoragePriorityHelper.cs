using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A8B RID: 6795
	public static class StoragePriorityHelper
	{
		// Token: 0x0600AB81 RID: 43905 RVA: 0x003C94D4 File Offset: 0x003C76D4
		public static string Label(this StoragePriority p)
		{
			switch (p)
			{
			case StoragePriority.Unstored:
				return "StoragePriorityUnstored".Translate();
			case StoragePriority.Low:
				return "StoragePriorityLow".Translate();
			case StoragePriority.Normal:
				return "StoragePriorityNormal".Translate();
			case StoragePriority.Preferred:
				return "StoragePriorityPreferred".Translate();
			case StoragePriority.Important:
				return "StoragePriorityImportant".Translate();
			case StoragePriority.Critical:
				return "StoragePriorityCritical".Translate();
			default:
				return "Unknown";
			}
		}
	}
}
