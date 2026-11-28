using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F7A RID: 3962
	public class InspirationDef : Def
	{
		// Token: 0x1700115B RID: 4443
		// (get) Token: 0x06006581 RID: 25985 RVA: 0x0024276E File Offset: 0x0024096E
		public InspirationWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (InspirationWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x04003C0B RID: 15371
		public Type inspirationClass = typeof(Inspiration);

		// Token: 0x04003C0C RID: 15372
		public Type workerClass = typeof(InspirationWorker);

		// Token: 0x04003C0D RID: 15373
		public float baseCommonality = 1f;

		// Token: 0x04003C0E RID: 15374
		public float baseDurationDays = 1f;

		// Token: 0x04003C0F RID: 15375
		public bool allowedOnAnimals;

		// Token: 0x04003C10 RID: 15376
		public bool allowedOnNonColonists;

		// Token: 0x04003C11 RID: 15377
		public bool allowedOnDownedPawns = true;

		// Token: 0x04003C12 RID: 15378
		public List<StatDef> requiredNonDisabledStats;

		// Token: 0x04003C13 RID: 15379
		public List<SkillRequirement> requiredSkills;

		// Token: 0x04003C14 RID: 15380
		public List<SkillRequirement> requiredAnySkill;

		// Token: 0x04003C15 RID: 15381
		public List<WorkTypeDef> requiredNonDisabledWorkTypes;

		// Token: 0x04003C16 RID: 15382
		public List<WorkTypeDef> requiredAnyNonDisabledWorkType;

		// Token: 0x04003C17 RID: 15383
		public WorkTags requiredNonDisabledWorkTags;

		// Token: 0x04003C18 RID: 15384
		public List<PawnCapacityDef> requiredCapacities;

		// Token: 0x04003C19 RID: 15385
		public List<SkillDef> associatedSkills;

		// Token: 0x04003C1A RID: 15386
		public float minAge;

		// Token: 0x04003C1B RID: 15387
		public List<StatModifier> statOffsets;

		// Token: 0x04003C1C RID: 15388
		public List<StatModifier> statFactors;

		// Token: 0x04003C1D RID: 15389
		[MustTranslate]
		public string beginLetter;

		// Token: 0x04003C1E RID: 15390
		[MustTranslate]
		public string beginLetterLabel;

		// Token: 0x04003C1F RID: 15391
		public LetterDef beginLetterDef;

		// Token: 0x04003C20 RID: 15392
		[MustTranslate]
		public string endMessage;

		// Token: 0x04003C21 RID: 15393
		[MustTranslate]
		public string baseInspectLine;

		// Token: 0x04003C22 RID: 15394
		[Unsaved(false)]
		private InspirationWorker workerInt;
	}
}
