using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019A0 RID: 6560
	public class CompProperties_StudyUnlocks : CompProperties
	{
		// Token: 0x0600A4C1 RID: 42177 RVA: 0x003ADF87 File Offset: 0x003AC187
		public CompProperties_StudyUnlocks()
		{
			this.compClass = typeof(CompStudyUnlocks);
		}

		// Token: 0x0600A4C2 RID: 42178 RVA: 0x003ADFAA File Offset: 0x003AC1AA
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_StudyUnlocks.<ConfigErrors>d__4 <ConfigErrors>d__ = new CompProperties_StudyUnlocks.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04006167 RID: 24935
		public List<StudyNote> studyNotes = new List<StudyNote>();

		// Token: 0x04006168 RID: 24936
		public float? defaultStudyAmount;

		// Token: 0x04006169 RID: 24937
		public KnowledgeCategoryDef defaultCategoryOverride;
	}
}
