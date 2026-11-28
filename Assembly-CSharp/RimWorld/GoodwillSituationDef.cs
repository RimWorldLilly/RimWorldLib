using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F67 RID: 3943
	public class GoodwillSituationDef : Def
	{
		// Token: 0x17001152 RID: 4434
		// (get) Token: 0x06006552 RID: 25938 RVA: 0x00241AAD File Offset: 0x0023FCAD
		public GoodwillSituationWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (GoodwillSituationWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x04003BA5 RID: 15269
		public Type workerClass = typeof(GoodwillSituationWorker);

		// Token: 0x04003BA6 RID: 15270
		public int baseMaxGoodwill = 100;

		// Token: 0x04003BA7 RID: 15271
		public MemeDef meme;

		// Token: 0x04003BA8 RID: 15272
		public MemeDef otherMeme;

		// Token: 0x04003BA9 RID: 15273
		public int naturalGoodwillOffset;

		// Token: 0x04003BAA RID: 15274
		public bool versusAll;

		// Token: 0x04003BAB RID: 15275
		[Unsaved(false)]
		private GoodwillSituationWorker workerInt;
	}
}
