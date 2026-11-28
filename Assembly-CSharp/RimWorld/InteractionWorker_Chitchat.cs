using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001514 RID: 5396
	public class InteractionWorker_Chitchat : InteractionWorker
	{
		// Token: 0x0600843E RID: 33854 RVA: 0x002F9179 File Offset: 0x002F7379
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (initiator.Inhumanized())
			{
				return 0f;
			}
			return 1f;
		}
	}
}
