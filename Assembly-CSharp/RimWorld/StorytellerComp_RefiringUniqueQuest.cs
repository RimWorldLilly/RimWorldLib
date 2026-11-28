using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E7 RID: 4583
	public class StorytellerComp_RefiringUniqueQuest : StorytellerComp
	{
		// Token: 0x17001356 RID: 4950
		// (get) Token: 0x06007213 RID: 29203 RVA: 0x0027EC78 File Offset: 0x0027CE78
		private int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		// Token: 0x17001357 RID: 4951
		// (get) Token: 0x06007214 RID: 29204 RVA: 0x0027EC8A File Offset: 0x0027CE8A
		private StorytellerCompProperties_RefiringUniqueQuest Props
		{
			get
			{
				return (StorytellerCompProperties_RefiringUniqueQuest)this.props;
			}
		}

		// Token: 0x06007215 RID: 29205 RVA: 0x0027EC97 File Offset: 0x0027CE97
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_RefiringUniqueQuest.<MakeIntervalIncidents>d__5 <MakeIntervalIncidents>d__ = new StorytellerComp_RefiringUniqueQuest.<MakeIntervalIncidents>d__5(-2);
			<MakeIntervalIncidents>d__.<>4__this = this;
			<MakeIntervalIncidents>d__.<>3__target = target;
			return <MakeIntervalIncidents>d__;
		}

		// Token: 0x06007216 RID: 29206 RVA: 0x0027ECAE File Offset: 0x0027CEAE
		public override void Initialize()
		{
			base.Initialize();
			if ((float)GenTicks.TicksGame >= this.Props.minDaysPassed * 60000f)
			{
				this.generateSkipped = true;
			}
		}

		// Token: 0x06007217 RID: 29207 RVA: 0x0027ECD6 File Offset: 0x0027CED6
		public override string ToString()
		{
			string str = base.ToString();
			string str2 = " ";
			IncidentDef incident = this.Props.incident;
			return str + str2 + ((incident != null) ? incident.ToString() : null);
		}

		// Token: 0x04004826 RID: 18470
		private bool generateSkipped;
	}
}
