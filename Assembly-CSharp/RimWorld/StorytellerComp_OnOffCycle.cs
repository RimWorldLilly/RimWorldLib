using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011DF RID: 4575
	public class StorytellerComp_OnOffCycle : StorytellerComp
	{
		// Token: 0x17001354 RID: 4948
		// (get) Token: 0x060071FE RID: 29182 RVA: 0x0027E875 File Offset: 0x0027CA75
		protected StorytellerCompProperties_OnOffCycle Props
		{
			get
			{
				return (StorytellerCompProperties_OnOffCycle)this.props;
			}
		}

		// Token: 0x060071FF RID: 29183 RVA: 0x0027E882 File Offset: 0x0027CA82
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_OnOffCycle.<MakeIntervalIncidents>d__2 <MakeIntervalIncidents>d__ = new StorytellerComp_OnOffCycle.<MakeIntervalIncidents>d__2(-2);
			<MakeIntervalIncidents>d__.<>4__this = this;
			<MakeIntervalIncidents>d__.<>3__target = target;
			return <MakeIntervalIncidents>d__;
		}

		// Token: 0x06007200 RID: 29184 RVA: 0x0027E89C File Offset: 0x0027CA9C
		private FiringIncident GenerateIncident(IIncidentTarget target)
		{
			if (this.Props.IncidentCategory == null)
			{
				return null;
			}
			IncidentParms parms = this.GenerateParms(this.Props.IncidentCategory, target);
			IncidentDef def2;
			if ((float)GenDate.DaysPassedSinceSettle < this.Props.forceRaidEnemyBeforeDaysPassed)
			{
				if (!IncidentDefOf.RaidEnemy.Worker.CanFireNow(parms))
				{
					return null;
				}
				def2 = IncidentDefOf.RaidEnemy;
			}
			else if (this.Props.incident != null)
			{
				if (!this.Props.incident.Worker.CanFireNow(parms))
				{
					return null;
				}
				def2 = this.Props.incident;
			}
			else if (!base.UsableIncidentsInCategory(this.Props.IncidentCategory, parms).TryRandomElementByWeight((IncidentDef def) => this.IncidentChanceFinal(def, target), out def2))
			{
				return null;
			}
			return new FiringIncident(def2, this, parms);
		}

		// Token: 0x06007201 RID: 29185 RVA: 0x0027E97C File Offset: 0x0027CB7C
		public override string ToString()
		{
			if (this.Props.incident == null && this.Props.IncidentCategory == null)
			{
				return "";
			}
			return base.ToString() + " (" + ((this.Props.incident != null) ? this.Props.incident.defName : this.Props.IncidentCategory.defName) + ")";
		}
	}
}
