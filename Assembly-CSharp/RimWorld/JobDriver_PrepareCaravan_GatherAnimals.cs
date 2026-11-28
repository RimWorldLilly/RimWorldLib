using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009AC RID: 2476
	public class JobDriver_PrepareCaravan_GatherAnimals : JobDriver_RopeToDestination
	{
		// Token: 0x06004B0E RID: 19214 RVA: 0x001D26A8 File Offset: 0x001D08A8
		protected override bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest)
		{
			PawnDuty duty = this.pawn.mindState.duty;
			IntVec3 intVec = (duty != null) ? duty.focus.Cell : IntVec3.Invalid;
			if (!intVec.IsValid)
			{
				return false;
			}
			if (!this.pawn.Position.InHorDistOf(intVec, 2f))
			{
				return false;
			}
			District district = intVec.GetDistrict(this.pawn.Map, RegionType.Set_Passable);
			return district == this.pawn.GetDistrict(RegionType.Set_Passable) && district == ropee.GetDistrict(RegionType.Set_Passable);
		}

		// Token: 0x06004B0F RID: 19215 RVA: 0x001D2738 File Offset: 0x001D0938
		protected override void ProcessArrivedRopee(Pawn ropee)
		{
			PawnDuty duty = ropee.mindState.duty;
			IntVec3 spot = (duty != null) ? duty.focus.Cell : IntVec3.Invalid;
			if (spot.IsValid)
			{
				ropee.roping.RopeToSpot(spot);
			}
		}

		// Token: 0x06004B10 RID: 19216 RVA: 0x001D277B File Offset: 0x001D097B
		protected override bool ShouldOpportunisticallyRopeAnimal(Pawn animal)
		{
			return JobGiver_PrepareCaravan_GatherPawns.DoesAnimalNeedGathering(this.pawn, animal);
		}
	}
}
