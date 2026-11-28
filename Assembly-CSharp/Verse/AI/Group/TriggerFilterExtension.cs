using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000965 RID: 2405
	public static class TriggerFilterExtension
	{
		// Token: 0x0600496F RID: 18799 RVA: 0x001CA5A0 File Offset: 0x001C87A0
		public static Trigger WithFilter(this Trigger t, TriggerFilter f)
		{
			if (t.filters == null)
			{
				t.filters = new List<TriggerFilter>();
			}
			t.filters.Add(f);
			return t;
		}
	}
}
