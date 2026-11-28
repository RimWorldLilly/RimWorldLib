using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200151A RID: 5402
	public class InteractionWorker_Insult : InteractionWorker
	{
		// Token: 0x0600844F RID: 33871 RVA: 0x002F99E8 File Offset: 0x002F7BE8
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (initiator.IsSlave && !recipient.IsSlave)
			{
				return 0f;
			}
			return 0.007f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
		}

		// Token: 0x040051B5 RID: 20917
		private const float BaseSelectionWeight = 0.007f;
	}
}
