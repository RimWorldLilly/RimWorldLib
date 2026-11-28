using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011D1 RID: 4561
	public class StorytellerComp_DissolutionTriggered : StorytellerComp
	{
		// Token: 0x17001349 RID: 4937
		// (get) Token: 0x060071DB RID: 29147 RVA: 0x0027E510 File Offset: 0x0027C710
		private StorytellerCompProperties_DissolutionTriggered Props
		{
			get
			{
				return (StorytellerCompProperties_DissolutionTriggered)this.props;
			}
		}

		// Token: 0x060071DC RID: 29148 RVA: 0x0027E520 File Offset: 0x0027C720
		public override void Notify_DissolutionEvent(Thing thing)
		{
			if (thing.def == this.Props.thing)
			{
				IncidentParms incidentParms = new IncidentParms
				{
					target = thing.MapHeld,
					spawnCenter = thing.Position
				};
				incidentParms.points = StorytellerUtility.DefaultThreatPointsNow(incidentParms.target);
				if (Rand.Chance(base.IncidentChanceFinal(this.Props.incident, incidentParms.target)) && this.Props.incident.Worker.CanFireNow(incidentParms))
				{
					Find.Storyteller.incidentQueue.Add(this.Props.incident, Find.TickManager.TicksGame + this.Props.delayTicks, incidentParms, 0);
				}
			}
		}
	}
}
