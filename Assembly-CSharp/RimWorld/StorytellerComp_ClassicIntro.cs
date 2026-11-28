using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011CA RID: 4554
	public class StorytellerComp_ClassicIntro : StorytellerComp
	{
		// Token: 0x17001345 RID: 4933
		// (get) Token: 0x060071CB RID: 29131 RVA: 0x0027E3A7 File Offset: 0x0027C5A7
		protected int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		// Token: 0x060071CC RID: 29132 RVA: 0x0027E3B9 File Offset: 0x0027C5B9
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_ClassicIntro.<MakeIntervalIncidents>d__2 <MakeIntervalIncidents>d__ = new StorytellerComp_ClassicIntro.<MakeIntervalIncidents>d__2(-2);
			<MakeIntervalIncidents>d__.<>4__this = this;
			<MakeIntervalIncidents>d__.<>3__target = target;
			return <MakeIntervalIncidents>d__;
		}
	}
}
