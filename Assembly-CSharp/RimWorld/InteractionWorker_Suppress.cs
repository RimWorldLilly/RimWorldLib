using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001528 RID: 5416
	public class InteractionWorker_Suppress : InteractionWorker
	{
		// Token: 0x0600847B RID: 33915 RVA: 0x002FBF46 File Offset: 0x002FA146
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
			SlaveRebellionUtility.IncrementInteractionSuppression(initiator, recipient);
		}
	}
}
