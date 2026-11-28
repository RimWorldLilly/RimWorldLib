using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020011F4 RID: 4596
	public class StorytellerComp_WorkSite : StorytellerComp
	{
		// Token: 0x1700135E RID: 4958
		// (get) Token: 0x06007233 RID: 29235 RVA: 0x0027F0E4 File Offset: 0x0027D2E4
		public StorytellerCompProperties_WorkSite Props
		{
			get
			{
				return (StorytellerCompProperties_WorkSite)this.props;
			}
		}

		// Token: 0x06007234 RID: 29236 RVA: 0x0027F0F1 File Offset: 0x0027D2F1
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_WorkSite.<MakeIntervalIncidents>d__2 <MakeIntervalIncidents>d__ = new StorytellerComp_WorkSite.<MakeIntervalIncidents>d__2(-2);
			<MakeIntervalIncidents>d__.<>4__this = this;
			<MakeIntervalIncidents>d__.<>3__target = target;
			return <MakeIntervalIncidents>d__;
		}

		// Token: 0x06007235 RID: 29237 RVA: 0x0027F108 File Offset: 0x0027D308
		public override string ToString()
		{
			string str = base.ToString();
			string str2 = " ";
			IncidentDef incident = this.Props.incident;
			return str + str2 + ((incident != null) ? incident.ToString() : null);
		}
	}
}
