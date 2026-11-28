using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200165B RID: 5723
	public class RitualAttachableOutcomeEffectWorker_InsectJellyTunnels : RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x06008FEF RID: 36847 RVA: 0x0034E584 File Offset: 0x0034C784
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			IncidentParms parms = new IncidentParms
			{
				target = jobRitual.Map,
				points = (float)(outcome.BestPositiveOutcome(jobRitual) ? 320 : 210)
			};
			if (IncidentDefOf.Infestation_Jelly.Worker.TryExecute(parms))
			{
				extraOutcomeDesc = this.def.letterInfoText;
			}
		}

		// Token: 0x040057E8 RID: 22504
		public const int PositiveOutcomePoints = 210;

		// Token: 0x040057E9 RID: 22505
		public const int BestOutcomePoints = 320;
	}
}
