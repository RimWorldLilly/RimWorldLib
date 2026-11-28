using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011DE RID: 4574
	public class StorytellerCompProperties_OnOffCycle : StorytellerCompProperties
	{
		// Token: 0x17001353 RID: 4947
		// (get) Token: 0x060071FB RID: 29179 RVA: 0x0027E826 File Offset: 0x0027CA26
		public IncidentCategoryDef IncidentCategory
		{
			get
			{
				if (this.incident != null)
				{
					return this.incident.category;
				}
				return this.category;
			}
		}

		// Token: 0x060071FC RID: 29180 RVA: 0x0027E842 File Offset: 0x0027CA42
		public StorytellerCompProperties_OnOffCycle()
		{
			this.compClass = typeof(StorytellerComp_OnOffCycle);
		}

		// Token: 0x060071FD RID: 29181 RVA: 0x0027E865 File Offset: 0x0027CA65
		public override IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
		{
			StorytellerCompProperties_OnOffCycle.<ConfigErrors>d__15 <ConfigErrors>d__ = new StorytellerCompProperties_OnOffCycle.<ConfigErrors>d__15(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04004811 RID: 18449
		public float onDays;

		// Token: 0x04004812 RID: 18450
		public float offDays;

		// Token: 0x04004813 RID: 18451
		public float onDaysNoTreeConnectors;

		// Token: 0x04004814 RID: 18452
		public float offDaysNoTreeConnectors;

		// Token: 0x04004815 RID: 18453
		public float minSpacingDays;

		// Token: 0x04004816 RID: 18454
		public FloatRange numIncidentsRange = FloatRange.Zero;

		// Token: 0x04004817 RID: 18455
		public SimpleCurve acceptFractionByDaysPassedCurve;

		// Token: 0x04004818 RID: 18456
		public SimpleCurve acceptPercentFactorPerThreatPointsCurve;

		// Token: 0x04004819 RID: 18457
		public SimpleCurve acceptPercentFactorPerProgressScoreCurve;

		// Token: 0x0400481A RID: 18458
		public IncidentDef incident;

		// Token: 0x0400481B RID: 18459
		private IncidentCategoryDef category;

		// Token: 0x0400481C RID: 18460
		public float forceRaidEnemyBeforeDaysPassed;
	}
}
