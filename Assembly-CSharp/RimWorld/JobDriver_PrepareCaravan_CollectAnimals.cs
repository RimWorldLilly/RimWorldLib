using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020009AD RID: 2477
	public class JobDriver_PrepareCaravan_CollectAnimals : JobDriver_RopeToDestination
	{
		// Token: 0x06004B12 RID: 19218 RVA: 0x001D2791 File Offset: 0x001D0991
		protected override bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest)
		{
			return false;
		}

		// Token: 0x06004B13 RID: 19219 RVA: 0x001D2794 File Offset: 0x001D0994
		protected override void ProcessArrivedRopee(Pawn ropee)
		{
		}

		// Token: 0x06004B14 RID: 19220 RVA: 0x001D2796 File Offset: 0x001D0996
		protected override bool ShouldOpportunisticallyRopeAnimal(Pawn animal)
		{
			return JobGiver_PrepareCaravan_CollectPawns.DoesAnimalNeedGathering(this.pawn, animal);
		}

		// Token: 0x06004B15 RID: 19221 RVA: 0x001D27A4 File Offset: 0x001D09A4
		protected override Thing FindDistantAnimalToRope()
		{
			Lord lord = this.pawn.GetLord();
			if (lord == null)
			{
				return null;
			}
			return GenClosest.ClosestThing_Global(this.pawn.Position, lord.ownedPawns, 99999f, (Thing t) => this.ShouldOpportunisticallyRopeAnimal(t as Pawn), null, false);
		}
	}
}
