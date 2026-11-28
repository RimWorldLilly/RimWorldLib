using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001659 RID: 5721
	public class RitualAttachableOutcomeEffectWorker_PsyfocusRecharge : RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x06008FEA RID: 36842 RVA: 0x0034E424 File Offset: 0x0034C624
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = this.def.letterInfoText;
			foreach (Pawn pawn in totalPresence.Keys)
			{
				Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
				if (psychicEntropy != null)
				{
					psychicEntropy.RechargePsyfocus();
				}
			}
		}
	}
}
