using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020011C6 RID: 4550
	public class StorytellerComp_CategoryIndividualMTBByBiome : StorytellerComp
	{
		// Token: 0x17001343 RID: 4931
		// (get) Token: 0x060071C1 RID: 29121 RVA: 0x0027E2A0 File Offset: 0x0027C4A0
		protected StorytellerCompProperties_CategoryIndividualMTBByBiome Props
		{
			get
			{
				return (StorytellerCompProperties_CategoryIndividualMTBByBiome)this.props;
			}
		}

		// Token: 0x060071C2 RID: 29122 RVA: 0x0027E2AD File Offset: 0x0027C4AD
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_CategoryIndividualMTBByBiome.<MakeIntervalIncidents>d__2 <MakeIntervalIncidents>d__ = new StorytellerComp_CategoryIndividualMTBByBiome.<MakeIntervalIncidents>d__2(-2);
			<MakeIntervalIncidents>d__.<>4__this = this;
			<MakeIntervalIncidents>d__.<>3__target = target;
			return <MakeIntervalIncidents>d__;
		}

		// Token: 0x060071C3 RID: 29123 RVA: 0x0027E2C4 File Offset: 0x0027C4C4
		public override string ToString()
		{
			string str = base.ToString();
			string str2 = " ";
			IncidentCategoryDef category = this.Props.category;
			return str + str2 + ((category != null) ? category.ToString() : null);
		}
	}
}
