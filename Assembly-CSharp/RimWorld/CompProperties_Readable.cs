using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA9 RID: 3753
	public abstract class CompProperties_Readable : CompProperties
	{
		// Token: 0x060062D2 RID: 25298 RVA: 0x00239151 File Offset: 0x00237351
		public CompProperties_Readable()
		{
			this.compClass = typeof(CompReadable);
		}

		// Token: 0x04003687 RID: 13959
		public MentalBreakIntensity mentalBreakIntensity = MentalBreakIntensity.Major;

		// Token: 0x04003688 RID: 13960
		public DevelopmentalStage developmentalStageFilter = DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x04003689 RID: 13961
		public List<ReadingOutcomeProperties> doers;
	}
}
