using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F2D RID: 3885
	public class BabyPlayDef : Def
	{
		// Token: 0x17001122 RID: 4386
		// (get) Token: 0x06006471 RID: 25713 RVA: 0x0023E235 File Offset: 0x0023C435
		public BabyPlayGiver Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (BabyPlayGiver)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x040039A8 RID: 14760
		public Type workerClass;

		// Token: 0x040039A9 RID: 14761
		public JobDef jobDef;

		// Token: 0x040039AA RID: 14762
		private BabyPlayGiver workerInt;
	}
}
