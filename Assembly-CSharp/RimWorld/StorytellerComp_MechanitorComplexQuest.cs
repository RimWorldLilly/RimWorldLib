using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020011D9 RID: 4569
	public class StorytellerComp_MechanitorComplexQuest : StorytellerComp
	{
		// Token: 0x1700134F RID: 4943
		// (get) Token: 0x060071EF RID: 29167 RVA: 0x0027E74C File Offset: 0x0027C94C
		private StorytellerCompProperties_MechanitorComplexQuest Props
		{
			get
			{
				return (StorytellerCompProperties_MechanitorComplexQuest)this.props;
			}
		}

		// Token: 0x17001350 RID: 4944
		// (get) Token: 0x060071F0 RID: 29168 RVA: 0x0027E759 File Offset: 0x0027C959
		private bool ExistingMechanitorOrMechlink
		{
			get
			{
				return MechanitorUtility.AnyMechanitorInPlayerFaction() || MechanitorUtility.AnyMechlinkInMap();
			}
		}

		// Token: 0x060071F1 RID: 29169 RVA: 0x0027E769 File Offset: 0x0027C969
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_MechanitorComplexQuest.<MakeIntervalIncidents>d__4 <MakeIntervalIncidents>d__ = new StorytellerComp_MechanitorComplexQuest.<MakeIntervalIncidents>d__4(-2);
			<MakeIntervalIncidents>d__.<>4__this = this;
			<MakeIntervalIncidents>d__.<>3__target = target;
			return <MakeIntervalIncidents>d__;
		}
	}
}
