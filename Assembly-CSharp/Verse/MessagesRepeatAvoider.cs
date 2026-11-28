using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000630 RID: 1584
	public static class MessagesRepeatAvoider
	{
		// Token: 0x060031BD RID: 12733 RVA: 0x0013D99D File Offset: 0x0013BB9D
		public static void Reset()
		{
			MessagesRepeatAvoider.lastShowTimes.Clear();
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x0013D9AC File Offset: 0x0013BBAC
		public static bool MessageShowAllowed(string tag, float minSecondsSinceLastShow)
		{
			float num;
			if (!MessagesRepeatAvoider.lastShowTimes.TryGetValue(tag, out num))
			{
				num = -99999f;
			}
			bool flag = RealTime.LastRealTime > num + minSecondsSinceLastShow;
			if (flag)
			{
				MessagesRepeatAvoider.lastShowTimes[tag] = RealTime.LastRealTime;
			}
			return flag;
		}

		// Token: 0x04001FD8 RID: 8152
		private static Dictionary<string, float> lastShowTimes = new Dictionary<string, float>();
	}
}
