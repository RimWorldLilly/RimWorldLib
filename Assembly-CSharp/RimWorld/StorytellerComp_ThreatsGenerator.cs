using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020011F0 RID: 4592
	public class StorytellerComp_ThreatsGenerator : StorytellerComp
	{
		// Token: 0x1700135C RID: 4956
		// (get) Token: 0x0600722A RID: 29226 RVA: 0x0027EEED File Offset: 0x0027D0ED
		protected StorytellerCompProperties_ThreatsGenerator Props
		{
			get
			{
				return (StorytellerCompProperties_ThreatsGenerator)this.props;
			}
		}

		// Token: 0x0600722B RID: 29227 RVA: 0x0027EEFA File Offset: 0x0027D0FA
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_ThreatsGenerator.<MakeIntervalIncidents>d__2 <MakeIntervalIncidents>d__ = new StorytellerComp_ThreatsGenerator.<MakeIntervalIncidents>d__2(-2);
			<MakeIntervalIncidents>d__.<>4__this = this;
			<MakeIntervalIncidents>d__.<>3__target = target;
			return <MakeIntervalIncidents>d__;
		}
	}
}
