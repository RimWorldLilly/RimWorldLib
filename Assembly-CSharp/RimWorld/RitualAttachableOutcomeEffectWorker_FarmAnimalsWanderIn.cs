using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001658 RID: 5720
	public class RitualAttachableOutcomeEffectWorker_FarmAnimalsWanderIn : RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x06008FE8 RID: 36840 RVA: 0x0034E39C File Offset: 0x0034C59C
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			IncidentParms parms = new IncidentParms
			{
				target = jobRitual.Map,
				totalBodySize = (outcome.BestPositiveOutcome(jobRitual) ? 3f : 2f),
				customLetterText = "RitualAttachedOutcome_FarmAnimalsWanderIn_Desc".Translate(jobRitual.RitualLabel)
			};
			if (IncidentDefOf.FarmAnimalsWanderIn.Worker.TryExecute(parms))
			{
				extraOutcomeDesc = this.def.letterInfoText;
			}
		}

		// Token: 0x040057E6 RID: 22502
		public const float PositiveOutcomeBodysize = 2f;

		// Token: 0x040057E7 RID: 22503
		public const float BestOutcomeBodysize = 3f;
	}
}
