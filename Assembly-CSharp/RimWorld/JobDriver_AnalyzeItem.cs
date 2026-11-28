using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A13 RID: 2579
	public class JobDriver_AnalyzeItem : JobDriver_StudyItem
	{
		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x06004DE6 RID: 19942 RVA: 0x001DC0A2 File Offset: 0x001DA2A2
		public CompAnalyzable AnalyzableComp
		{
			get
			{
				return base.ThingToStudy.TryGetComp<CompAnalyzable>();
			}
		}

		// Token: 0x06004DE7 RID: 19943 RVA: 0x001DC0AF File Offset: 0x001DA2AF
		protected override IEnumerable<Toil> GetStudyToils()
		{
			JobDriver_AnalyzeItem.<GetStudyToils>d__2 <GetStudyToils>d__ = new JobDriver_AnalyzeItem.<GetStudyToils>d__2(-2);
			<GetStudyToils>d__.<>4__this = this;
			return <GetStudyToils>d__;
		}
	}
}
