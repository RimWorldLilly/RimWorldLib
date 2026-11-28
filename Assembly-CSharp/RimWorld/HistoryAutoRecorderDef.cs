using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F6A RID: 3946
	public class HistoryAutoRecorderDef : Def
	{
		// Token: 0x17001153 RID: 4435
		// (get) Token: 0x06006556 RID: 25942 RVA: 0x00241B0F File Offset: 0x0023FD0F
		public HistoryAutoRecorderWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (HistoryAutoRecorderWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x06006557 RID: 25943 RVA: 0x00241B35 File Offset: 0x0023FD35
		public static HistoryAutoRecorderDef Named(string defName)
		{
			return DefDatabase<HistoryAutoRecorderDef>.GetNamed(defName, true);
		}

		// Token: 0x04003BAD RID: 15277
		public Type workerClass;

		// Token: 0x04003BAE RID: 15278
		public int recordTicksFrequency = 60000;

		// Token: 0x04003BAF RID: 15279
		public Color graphColor = Color.green;

		// Token: 0x04003BB0 RID: 15280
		[MustTranslate]
		public string graphLabelY;

		// Token: 0x04003BB1 RID: 15281
		public string valueFormat;

		// Token: 0x04003BB2 RID: 15282
		[Unsaved(false)]
		private HistoryAutoRecorderWorker workerInt;
	}
}
