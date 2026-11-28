using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011F2 RID: 4594
	public class StorytellerComp_Triggered : StorytellerComp
	{
		// Token: 0x1700135D RID: 4957
		// (get) Token: 0x0600722E RID: 29230 RVA: 0x0027EF39 File Offset: 0x0027D139
		private StorytellerCompProperties_Triggered Props
		{
			get
			{
				return (StorytellerCompProperties_Triggered)this.props;
			}
		}

		// Token: 0x0600722F RID: 29231 RVA: 0x0027EF48 File Offset: 0x0027D148
		public override void Notify_PawnEvent(Pawn p, AdaptationEvent ev, DamageInfo? dinfo = null)
		{
			if (!p.RaceProps.Humanlike || !p.IsColonist)
			{
				return;
			}
			if (ev == AdaptationEvent.Died || ev == AdaptationEvent.Kidnapped || ev == AdaptationEvent.LostBecauseMapClosed || ev == AdaptationEvent.Downed)
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction)
				{
					if (pawn.RaceProps.Humanlike && !pawn.IsPrisoner)
					{
						if (pawn != p && !pawn.Downed)
						{
							return;
						}
						if (ModsConfig.BiotechActive && pawn.Deathresting && !SanguophageUtility.ShouldBeDeathrestingOrInComaInsteadOfDead(pawn))
						{
							return;
						}
					}
				}
				if (ModsConfig.AnomalyActive && (DeathRefusalUtility.HasPlayerControlledDeathRefusal(p) || DeathRefusalUtility.PlayerHasCorpseWithDeathRefusal()))
				{
					return;
				}
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				if (anyPlayerHomeMap != null)
				{
					IncidentParms parms = StorytellerUtility.DefaultParmsNow(this.Props.incident.category, anyPlayerHomeMap);
					if (this.Props.incident.Worker.CanFireNow(parms))
					{
						QueuedIncident qi = new QueuedIncident(new FiringIncident(this.Props.incident, this, parms), Find.TickManager.TicksGame + this.Props.delayTicks, 0);
						Find.Storyteller.incidentQueue.Add(qi);
					}
				}
			}
		}

		// Token: 0x06007230 RID: 29232 RVA: 0x0027F090 File Offset: 0x0027D290
		public override string ToString()
		{
			string str = base.ToString();
			string str2 = " ";
			IncidentDef incident = this.Props.incident;
			return str + str2 + ((incident != null) ? incident.ToString() : null);
		}
	}
}
