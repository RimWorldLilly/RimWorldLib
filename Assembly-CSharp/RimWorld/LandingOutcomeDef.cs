using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F83 RID: 3971
	public class LandingOutcomeDef : Def
	{
		// Token: 0x17001162 RID: 4450
		// (get) Token: 0x0600659E RID: 26014 RVA: 0x00242F44 File Offset: 0x00241144
		public LandingOutcomeWorker Worker
		{
			get
			{
				LandingOutcomeWorker result;
				if ((result = this.worker) == null)
				{
					result = (this.worker = (LandingOutcomeWorker)Activator.CreateInstance(this.workerClass, new object[]
					{
						this
					}));
				}
				return result;
			}
		}

		// Token: 0x04003C54 RID: 15444
		public float weight;

		// Token: 0x04003C55 RID: 15445
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04003C56 RID: 15446
		[MustTranslate]
		public string letterText;

		// Token: 0x04003C57 RID: 15447
		public LetterDef letterDef;

		// Token: 0x04003C58 RID: 15448
		public Type workerClass;

		// Token: 0x04003C59 RID: 15449
		private LandingOutcomeWorker worker;
	}
}
