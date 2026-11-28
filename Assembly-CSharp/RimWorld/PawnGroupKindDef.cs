using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB8 RID: 4024
	public class PawnGroupKindDef : Def
	{
		// Token: 0x17001186 RID: 4486
		// (get) Token: 0x06006640 RID: 26176 RVA: 0x002455CC File Offset: 0x002437CC
		public PawnGroupKindWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnGroupKindWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x04003E9C RID: 16028
		public Type workerClass = typeof(PawnGroupKindWorker);

		// Token: 0x04003E9D RID: 16029
		[Unsaved(false)]
		private PawnGroupKindWorker workerInt;
	}
}
