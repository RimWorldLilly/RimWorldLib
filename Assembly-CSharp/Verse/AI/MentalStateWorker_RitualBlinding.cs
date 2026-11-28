using System;

namespace Verse.AI
{
	// Token: 0x0200089B RID: 2203
	public class MentalStateWorker_RitualBlinding : MentalStateWorker
	{
		// Token: 0x06004441 RID: 17473 RVA: 0x001ABBEB File Offset: 0x001A9DEB
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && RitualBlindingMentalStateUtility.FindPawnToBlind(pawn) != null;
		}
	}
}
