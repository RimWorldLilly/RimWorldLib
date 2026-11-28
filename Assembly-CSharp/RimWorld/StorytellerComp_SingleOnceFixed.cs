using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011ED RID: 4589
	public class StorytellerComp_SingleOnceFixed : StorytellerComp
	{
		// Token: 0x1700135A RID: 4954
		// (get) Token: 0x06007224 RID: 29220 RVA: 0x0027EE7F File Offset: 0x0027D07F
		protected int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		// Token: 0x1700135B RID: 4955
		// (get) Token: 0x06007225 RID: 29221 RVA: 0x0027EE91 File Offset: 0x0027D091
		private StorytellerCompProperties_SingleOnceFixed Props
		{
			get
			{
				return (StorytellerCompProperties_SingleOnceFixed)this.props;
			}
		}

		// Token: 0x06007226 RID: 29222 RVA: 0x0027EE9E File Offset: 0x0027D09E
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_SingleOnceFixed.<MakeIntervalIncidents>d__4 <MakeIntervalIncidents>d__ = new StorytellerComp_SingleOnceFixed.<MakeIntervalIncidents>d__4(-2);
			<MakeIntervalIncidents>d__.<>4__this = this;
			<MakeIntervalIncidents>d__.<>3__target = target;
			return <MakeIntervalIncidents>d__;
		}
	}
}
