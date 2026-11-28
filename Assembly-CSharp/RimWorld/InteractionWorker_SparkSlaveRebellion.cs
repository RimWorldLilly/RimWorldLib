using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001527 RID: 5415
	public class InteractionWorker_SparkSlaveRebellion : InteractionWorker
	{
		// Token: 0x06008479 RID: 33913 RVA: 0x002FBEE0 File Offset: 0x002FA0E0
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			if (!SlaveRebellionUtility.CanParticipateInSlaveRebellion(recipient))
			{
				letterText = null;
				letterLabel = null;
				letterDef = null;
				lookTargets = null;
				return;
			}
			SlaveRebellionUtility.StartSlaveRebellion(recipient, out letterText, out letterLabel, out letterDef, out lookTargets, false);
			lookTargets = new LookTargets(new TargetInfo[]
			{
				initiator,
				recipient
			});
		}
	}
}
