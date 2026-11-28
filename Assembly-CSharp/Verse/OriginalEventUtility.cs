using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000650 RID: 1616
	public static class OriginalEventUtility
	{
		// Token: 0x060032B0 RID: 12976 RVA: 0x00143460 File Offset: 0x00141660
		public static void RecordOriginalEvent(Event e)
		{
			OriginalEventUtility.originalType = new EventType?(e.type);
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x060032B1 RID: 12977 RVA: 0x00143474 File Offset: 0x00141674
		public static EventType EventType
		{
			get
			{
				EventType? eventType = OriginalEventUtility.originalType;
				if (eventType == null)
				{
					return Event.current.rawType;
				}
				return eventType.GetValueOrDefault();
			}
		}

		// Token: 0x060032B2 RID: 12978 RVA: 0x001434A2 File Offset: 0x001416A2
		public static void Reset()
		{
			OriginalEventUtility.originalType = null;
		}

		// Token: 0x04002065 RID: 8293
		private static EventType? originalType;
	}
}
