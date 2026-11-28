using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020011EB RID: 4587
	public class StorytellerComp_SingleMTB : StorytellerComp
	{
		// Token: 0x17001359 RID: 4953
		// (get) Token: 0x0600721F RID: 29215 RVA: 0x0027EE07 File Offset: 0x0027D007
		private StorytellerCompProperties_SingleMTB Props
		{
			get
			{
				return (StorytellerCompProperties_SingleMTB)this.props;
			}
		}

		// Token: 0x06007220 RID: 29216 RVA: 0x0027EE14 File Offset: 0x0027D014
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_SingleMTB.<MakeIntervalIncidents>d__2 <MakeIntervalIncidents>d__ = new StorytellerComp_SingleMTB.<MakeIntervalIncidents>d__2(-2);
			<MakeIntervalIncidents>d__.<>4__this = this;
			<MakeIntervalIncidents>d__.<>3__target = target;
			return <MakeIntervalIncidents>d__;
		}

		// Token: 0x06007221 RID: 29217 RVA: 0x0027EE2B File Offset: 0x0027D02B
		public override string ToString()
		{
			string str = base.ToString();
			string str2 = " ";
			IncidentDef incident = this.Props.incident;
			return str + str2 + ((incident != null) ? incident.ToString() : null);
		}
	}
}
