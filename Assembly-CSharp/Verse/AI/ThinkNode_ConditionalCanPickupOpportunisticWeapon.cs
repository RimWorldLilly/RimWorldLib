using System;

namespace Verse.AI
{
	// Token: 0x020008C9 RID: 2249
	public class ThinkNode_ConditionalCanPickupOpportunisticWeapon : ThinkNode_Conditional
	{
		// Token: 0x060045B7 RID: 17847 RVA: 0x001B747F File Offset: 0x001B567F
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.pickupOpportunisticWeapon;
		}
	}
}
