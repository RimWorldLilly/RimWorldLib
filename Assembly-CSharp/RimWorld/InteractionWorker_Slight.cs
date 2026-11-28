using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001525 RID: 5413
	public class InteractionWorker_Slight : InteractionWorker
	{
		// Token: 0x06008475 RID: 33909 RVA: 0x002FBE22 File Offset: 0x002FA022
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (initiator.IsSlave && !recipient.IsSlave)
			{
				return 0f;
			}
			return 0.02f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
		}

		// Token: 0x040051D0 RID: 20944
		private const float BaseSelectionWeight = 0.02f;
	}
}
