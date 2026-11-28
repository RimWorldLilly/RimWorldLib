using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F6C RID: 3948
	public class HistoryAutoRecorderGroupDef : Def
	{
		// Token: 0x0600655B RID: 25947 RVA: 0x00241B6B File Offset: 0x0023FD6B
		public static HistoryAutoRecorderGroupDef Named(string defName)
		{
			return DefDatabase<HistoryAutoRecorderGroupDef>.GetNamed(defName, true);
		}

		// Token: 0x04003BB3 RID: 15283
		public bool useFixedScale;

		// Token: 0x04003BB4 RID: 15284
		public Vector2 fixedScale;

		// Token: 0x04003BB5 RID: 15285
		public bool integersOnly;

		// Token: 0x04003BB6 RID: 15286
		public bool onlyPositiveValues = true;

		// Token: 0x04003BB7 RID: 15287
		public bool devModeOnly;

		// Token: 0x04003BB8 RID: 15288
		public List<HistoryAutoRecorderDef> historyAutoRecorderDefs = new List<HistoryAutoRecorderDef>();
	}
}
