using System;

namespace Verse.AI
{
	// Token: 0x02000895 RID: 2197
	public class MentalStateWorker_CorpseObsession : MentalStateWorker
	{
		// Token: 0x06004435 RID: 17461 RVA: 0x001ABB12 File Offset: 0x001A9D12
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(pawn) != null;
		}
	}
}
