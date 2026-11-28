using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200142D RID: 5165
	public class CompAbilityEffect_RaidEnemy : CompAbilityEffect_WithDest
	{
		// Token: 0x170015CD RID: 5581
		// (get) Token: 0x06007F70 RID: 32624 RVA: 0x002D8387 File Offset: 0x002D6587
		public new CompProperties_RaidEnemy Props
		{
			get
			{
				return (CompProperties_RaidEnemy)this.props;
			}
		}

		// Token: 0x06007F71 RID: 32625 RVA: 0x002D8394 File Offset: 0x002D6594
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = this.parent.pawn.Map;
			incidentParms.points = this.Props.points;
			incidentParms.faction = Find.FactionManager.FirstFactionOfDef(this.Props.factionDef);
			incidentParms.raidStrategy = this.Props.raidStrategyDef;
			incidentParms.raidArrivalMode = this.Props.pawnArrivalModeDef;
			incidentParms.raidStrategy = this.Props.raidStrategyDef;
			incidentParms.spawnCenter = target.Cell;
			IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
		}
	}
}
