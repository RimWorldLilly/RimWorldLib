using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200141A RID: 5146
	public class CompAbilityEffect_Infestation : CompAbilityEffect_WithDest
	{
		// Token: 0x170015C4 RID: 5572
		// (get) Token: 0x06007F3B RID: 32571 RVA: 0x002D75AF File Offset: 0x002D57AF
		public new CompProperties_Infestation Props
		{
			get
			{
				return (CompProperties_Infestation)this.props;
			}
		}

		// Token: 0x06007F3C RID: 32572 RVA: 0x002D75BC File Offset: 0x002D57BC
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = this.parent.pawn.Map;
			incidentParms.points = this.Props.points;
			incidentParms.infestationLocOverride = new IntVec3?(target.Cell);
			incidentParms.forced = true;
			IncidentDefOf.Infestation.Worker.TryExecute(incidentParms);
		}
	}
}
