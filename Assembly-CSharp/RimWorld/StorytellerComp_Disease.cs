using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020011CF RID: 4559
	public class StorytellerComp_Disease : StorytellerComp
	{
		// Token: 0x17001348 RID: 4936
		// (get) Token: 0x060071D6 RID: 29142 RVA: 0x0027E498 File Offset: 0x0027C698
		protected StorytellerCompProperties_Disease Props
		{
			get
			{
				return (StorytellerCompProperties_Disease)this.props;
			}
		}

		// Token: 0x060071D7 RID: 29143 RVA: 0x0027E4A5 File Offset: 0x0027C6A5
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_Disease.<MakeIntervalIncidents>d__3 <MakeIntervalIncidents>d__ = new StorytellerComp_Disease.<MakeIntervalIncidents>d__3(-2);
			<MakeIntervalIncidents>d__.<>4__this = this;
			<MakeIntervalIncidents>d__.<>3__target = target;
			return <MakeIntervalIncidents>d__;
		}

		// Token: 0x060071D8 RID: 29144 RVA: 0x0027E4BC File Offset: 0x0027C6BC
		public override string ToString()
		{
			string str = base.ToString();
			string str2 = " ";
			IncidentCategoryDef category = this.Props.category;
			return str + str2 + ((category != null) ? category.ToString() : null);
		}

		// Token: 0x040047F8 RID: 18424
		private float CaravanDiseaseMTBFactor = 4f;
	}
}
