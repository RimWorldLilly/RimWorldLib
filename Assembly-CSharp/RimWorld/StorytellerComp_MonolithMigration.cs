using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020011DB RID: 4571
	public class StorytellerComp_MonolithMigration : StorytellerComp
	{
		// Token: 0x17001351 RID: 4945
		// (get) Token: 0x060071F4 RID: 29172 RVA: 0x0027E7A0 File Offset: 0x0027C9A0
		protected StorytellerCompProperties_MonolithMigration Props
		{
			get
			{
				return (StorytellerCompProperties_MonolithMigration)this.props;
			}
		}

		// Token: 0x060071F5 RID: 29173 RVA: 0x0027E7AD File Offset: 0x0027C9AD
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_MonolithMigration.<MakeIntervalIncidents>d__2 <MakeIntervalIncidents>d__ = new StorytellerComp_MonolithMigration.<MakeIntervalIncidents>d__2(-2);
			<MakeIntervalIncidents>d__.<>4__this = this;
			<MakeIntervalIncidents>d__.<>3__target = target;
			return <MakeIntervalIncidents>d__;
		}
	}
}
