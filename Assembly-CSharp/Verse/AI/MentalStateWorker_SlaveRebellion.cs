using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200089C RID: 2204
	public class MentalStateWorker_SlaveRebellion : MentalStateWorker
	{
		// Token: 0x06004443 RID: 17475 RVA: 0x001ABC09 File Offset: 0x001A9E09
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && SlaveRebellionUtility.FindSlaveForRebellion(pawn) != null;
		}
	}
}
