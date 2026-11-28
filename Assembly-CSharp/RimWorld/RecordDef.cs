using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD4 RID: 4052
	public class RecordDef : Def
	{
		// Token: 0x1700119E RID: 4510
		// (get) Token: 0x060066C3 RID: 26307 RVA: 0x00247691 File Offset: 0x00245891
		public RecordWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RecordWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x04003FDC RID: 16348
		public RecordType type;

		// Token: 0x04003FDD RID: 16349
		public Type workerClass = typeof(RecordWorker);

		// Token: 0x04003FDE RID: 16350
		public List<JobDef> measuredTimeJobs;

		// Token: 0x04003FDF RID: 16351
		public int displayOrder = 99999;

		// Token: 0x04003FE0 RID: 16352
		[Unsaved(false)]
		private RecordWorker workerInt;
	}
}
