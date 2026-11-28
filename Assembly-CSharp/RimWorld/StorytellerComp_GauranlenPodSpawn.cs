using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020011D5 RID: 4565
	public class StorytellerComp_GauranlenPodSpawn : StorytellerComp
	{
		// Token: 0x1700134B RID: 4939
		// (get) Token: 0x060071E4 RID: 29156 RVA: 0x0027E66D File Offset: 0x0027C86D
		protected StorytellerCompProperties_GauranlenPodSpawn Props
		{
			get
			{
				return (StorytellerCompProperties_GauranlenPodSpawn)this.props;
			}
		}

		// Token: 0x060071E5 RID: 29157 RVA: 0x0027E67A File Offset: 0x0027C87A
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_GauranlenPodSpawn.<MakeIntervalIncidents>d__2 <MakeIntervalIncidents>d__ = new StorytellerComp_GauranlenPodSpawn.<MakeIntervalIncidents>d__2(-2);
			<MakeIntervalIncidents>d__.<>4__this = this;
			<MakeIntervalIncidents>d__.<>3__target = target;
			return <MakeIntervalIncidents>d__;
		}
	}
}
