using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200142F RID: 5167
	public class CompAbilityEffect_RandomAlliedMilitaryAid : CompAbilityEffect_WithDest
	{
		// Token: 0x170015CE RID: 5582
		// (get) Token: 0x06007F74 RID: 32628 RVA: 0x002D8452 File Offset: 0x002D6652
		public new CompProperties_RandomAlliedMilitaryAid Props
		{
			get
			{
				return (CompProperties_RandomAlliedMilitaryAid)this.props;
			}
		}

		// Token: 0x06007F75 RID: 32629 RVA: 0x002D8460 File Offset: 0x002D6660
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = this.parent.pawn.Map;
			incidentParms.faction = this.GetRandomAlliedFaction();
			incidentParms.raidArrivalModeForQuickMilitaryAid = true;
			incidentParms.points = this.Props.points;
			incidentParms.spawnCenter = target.Cell;
			if (!IncidentDefOf.RaidFriendly.Worker.TryExecute(incidentParms))
			{
				Log.Error("Failed to request military aid");
			}
		}

		// Token: 0x06007F76 RID: 32630 RVA: 0x002D84DE File Offset: 0x002D66DE
		private Faction GetRandomAlliedFaction()
		{
			return Find.FactionManager.RandomAlliedFaction(true, false, true, TechLevel.Undefined);
		}

		// Token: 0x06007F77 RID: 32631 RVA: 0x002D84EE File Offset: 0x002D66EE
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.GetRandomAlliedFaction() != null && base.CanApplyOn(target, dest);
		}
	}
}
