using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020005AC RID: 1452
	public static class DelayedErrorWindowRequest
	{
		// Token: 0x06002C08 RID: 11272 RVA: 0x0010ADB4 File Offset: 0x00108FB4
		public static void DelayedErrorWindowRequestOnGUI()
		{
			try
			{
				for (int i = 0; i < DelayedErrorWindowRequest.requests.Count; i++)
				{
					Find.WindowStack.Add(new Dialog_MessageBox(DelayedErrorWindowRequest.requests[i].text, "OK".Translate(), null, null, null, DelayedErrorWindowRequest.requests[i].title, false, null, null, WindowLayer.Dialog));
				}
			}
			finally
			{
				DelayedErrorWindowRequest.requests.Clear();
			}
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x0010AE40 File Offset: 0x00109040
		public static void Add(string text, string title = null)
		{
			DelayedErrorWindowRequest.Request item = default(DelayedErrorWindowRequest.Request);
			item.text = text;
			item.title = title;
			DelayedErrorWindowRequest.requests.Add(item);
		}

		// Token: 0x04001DD7 RID: 7639
		private static List<DelayedErrorWindowRequest.Request> requests = new List<DelayedErrorWindowRequest.Request>();

		// Token: 0x020027A4 RID: 10148
		private struct Request
		{
			// Token: 0x0400A21C RID: 41500
			public string text;

			// Token: 0x0400A21D RID: 41501
			public string title;
		}
	}
}
