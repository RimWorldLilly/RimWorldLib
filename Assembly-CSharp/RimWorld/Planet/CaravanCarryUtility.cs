using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002382 RID: 9090
	public static class CaravanCarryUtility
	{
		// Token: 0x0600E209 RID: 57865 RVA: 0x0053D770 File Offset: 0x0053B970
		public static bool CarriedByCaravan(this Pawn p)
		{
			Caravan caravan = p.GetCaravan();
			return caravan != null && caravan.carryTracker.IsCarried(p);
		}

		// Token: 0x0600E20A RID: 57866 RVA: 0x0053D795 File Offset: 0x0053B995
		public static bool WouldBenefitFromBeingCarried(Pawn p)
		{
			return CaravanBedUtility.WouldBenefitFromRestingInBed(p);
		}
	}
}
