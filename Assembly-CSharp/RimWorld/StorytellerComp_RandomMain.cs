using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E2 RID: 4578
	public class StorytellerComp_RandomMain : StorytellerComp
	{
		// Token: 0x17001355 RID: 4949
		// (get) Token: 0x06007208 RID: 29192 RVA: 0x0027EA93 File Offset: 0x0027CC93
		protected StorytellerCompProperties_RandomMain Props
		{
			get
			{
				return (StorytellerCompProperties_RandomMain)this.props;
			}
		}

		// Token: 0x06007209 RID: 29193 RVA: 0x0027EAA0 File Offset: 0x0027CCA0
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_RandomMain.<MakeIntervalIncidents>d__2 <MakeIntervalIncidents>d__ = new StorytellerComp_RandomMain.<MakeIntervalIncidents>d__2(-2);
			<MakeIntervalIncidents>d__.<>4__this = this;
			<MakeIntervalIncidents>d__.<>3__target = target;
			return <MakeIntervalIncidents>d__;
		}

		// Token: 0x0600720A RID: 29194 RVA: 0x0027EAB8 File Offset: 0x0027CCB8
		private IncidentCategoryDef ChooseRandomCategory(IIncidentTarget target, List<IncidentCategoryDef> skipCategories)
		{
			if (!skipCategories.Contains(IncidentCategoryDefOf.ThreatBig) && (float)(Find.TickManager.TicksGame - target.StoryState.LastThreatBigTick) > 60000f * this.Props.maxThreatBigIntervalDays)
			{
				return IncidentCategoryDefOf.ThreatBig;
			}
			return (from cw in this.Props.categoryWeights
			where !skipCategories.Contains(cw.category)
			select cw).RandomElementByWeight((IncidentCategoryEntry cw) => cw.weight).category;
		}

		// Token: 0x0600720B RID: 29195 RVA: 0x0027EB5C File Offset: 0x0027CD5C
		public override IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(incCat, target);
			if (incidentParms.points >= 0f)
			{
				incidentParms.points *= this.Props.randomPointsFactorRange.RandomInRange;
			}
			return incidentParms;
		}
	}
}
