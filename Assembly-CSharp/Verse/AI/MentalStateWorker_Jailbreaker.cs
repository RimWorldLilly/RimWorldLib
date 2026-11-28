using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000896 RID: 2198
	public class MentalStateWorker_Jailbreaker : MentalStateWorker
	{
		// Token: 0x06004437 RID: 17463 RVA: 0x001ABB30 File Offset: 0x001A9D30
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && JailbreakerMentalStateUtility.FindPrisoner(pawn) != null;
		}
	}
}
