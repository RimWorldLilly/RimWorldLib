using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009AA RID: 2474
	public class JobDriver_RopeRoamerToUnenclosedPen : JobDriver_RopeToPen
	{
		// Token: 0x06004B03 RID: 19203 RVA: 0x001D2537 File Offset: 0x001D0737
		protected override bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest)
		{
			return roperWaitingAtDest;
		}

		// Token: 0x06004B04 RID: 19204 RVA: 0x001D253A File Offset: 0x001D073A
		protected override void ProcessArrivedRopee(Pawn ropee)
		{
			MentalState_Roaming mentalState_Roaming = JobDriver_RopeRoamerToUnenclosedPen.RoamingMentalState(ropee);
			if (mentalState_Roaming == null)
			{
				return;
			}
			mentalState_Roaming.RecoverFromState();
		}

		// Token: 0x06004B05 RID: 19205 RVA: 0x001D254C File Offset: 0x001D074C
		protected override bool ShouldOpportunisticallyRopeAnimal(Pawn animal, CompAnimalPenMarker targetPenMarker)
		{
			return targetPenMarker == base.DestinationMarker && JobDriver_RopeRoamerToUnenclosedPen.RoamingMentalState(animal) != null;
		}

		// Token: 0x06004B06 RID: 19206 RVA: 0x001D2562 File Offset: 0x001D0762
		private static MentalState_Roaming RoamingMentalState(Pawn ropee)
		{
			return ropee.MentalState as MentalState_Roaming;
		}
	}
}
