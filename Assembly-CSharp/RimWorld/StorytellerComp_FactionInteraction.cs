using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020011D3 RID: 4563
	public class StorytellerComp_FactionInteraction : StorytellerComp
	{
		// Token: 0x1700134A RID: 4938
		// (get) Token: 0x060071DF RID: 29151 RVA: 0x0027E5FB File Offset: 0x0027C7FB
		private StorytellerCompProperties_FactionInteraction Props
		{
			get
			{
				return (StorytellerCompProperties_FactionInteraction)this.props;
			}
		}

		// Token: 0x060071E0 RID: 29152 RVA: 0x0027E608 File Offset: 0x0027C808
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_FactionInteraction.<MakeIntervalIncidents>d__2 <MakeIntervalIncidents>d__ = new StorytellerComp_FactionInteraction.<MakeIntervalIncidents>d__2(-2);
			<MakeIntervalIncidents>d__.<>4__this = this;
			<MakeIntervalIncidents>d__.<>3__target = target;
			return <MakeIntervalIncidents>d__;
		}

		// Token: 0x060071E1 RID: 29153 RVA: 0x0027E61F File Offset: 0x0027C81F
		public override string ToString()
		{
			return base.ToString() + " (" + this.Props.incident.defName + ")";
		}
	}
}
