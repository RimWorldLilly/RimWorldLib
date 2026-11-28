using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020011DD RID: 4573
	public class StorytellerComp_NoxiousHaze : StorytellerComp
	{
		// Token: 0x17001352 RID: 4946
		// (get) Token: 0x060071F8 RID: 29176 RVA: 0x0027E7FA File Offset: 0x0027C9FA
		protected StorytellerCompProperties_NoxiousHaze Props
		{
			get
			{
				return (StorytellerCompProperties_NoxiousHaze)this.props;
			}
		}

		// Token: 0x060071F9 RID: 29177 RVA: 0x0027E807 File Offset: 0x0027CA07
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_NoxiousHaze.<MakeIntervalIncidents>d__2 <MakeIntervalIncidents>d__ = new StorytellerComp_NoxiousHaze.<MakeIntervalIncidents>d__2(-2);
			<MakeIntervalIncidents>d__.<>4__this = this;
			<MakeIntervalIncidents>d__.<>3__target = target;
			return <MakeIntervalIncidents>d__;
		}
	}
}
