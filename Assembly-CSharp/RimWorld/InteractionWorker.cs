using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F7E RID: 3966
	public class InteractionWorker
	{
		// Token: 0x0600658C RID: 25996 RVA: 0x00242CE5 File Offset: 0x00240EE5
		public virtual float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0f;
		}

		// Token: 0x0600658D RID: 25997 RVA: 0x00242CEC File Offset: 0x00240EEC
		public virtual void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
		}

		// Token: 0x04003C3B RID: 15419
		public InteractionDef interaction;
	}
}
