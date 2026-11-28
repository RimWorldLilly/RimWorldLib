using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FCE RID: 4046
	public class RaidAgeRestrictionDef : Def
	{
		// Token: 0x1700119C RID: 4508
		// (get) Token: 0x060066AB RID: 26283 RVA: 0x0024700E File Offset: 0x0024520E
		public RaidAgeRestrictionWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RaidAgeRestrictionWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x04003FC1 RID: 16321
		public Type workerClass = typeof(RaidAgeRestrictionWorker);

		// Token: 0x04003FC2 RID: 16322
		public DevelopmentalStage developmentStage;

		// Token: 0x04003FC3 RID: 16323
		public FloatRange? ageRange;

		// Token: 0x04003FC4 RID: 16324
		public float threatPointsFactor = 1f;

		// Token: 0x04003FC5 RID: 16325
		public int earliestDay;

		// Token: 0x04003FC6 RID: 16326
		public float chance;

		// Token: 0x04003FC7 RID: 16327
		[MustTranslate]
		public string arrivalTextExtra;

		// Token: 0x04003FC8 RID: 16328
		private RaidAgeRestrictionWorker workerInt;
	}
}
