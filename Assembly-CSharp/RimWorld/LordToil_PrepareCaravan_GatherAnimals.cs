using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CBC RID: 3260
	public class LordToil_PrepareCaravan_GatherAnimals : LordToil_PrepareCaravan_RopeAnimals
	{
		// Token: 0x06005C8F RID: 23695 RVA: 0x0021FEC4 File Offset: 0x0021E0C4
		public LordToil_PrepareCaravan_GatherAnimals(IntVec3 destinationPoint) : base(destinationPoint, null)
		{
		}

		// Token: 0x06005C90 RID: 23696 RVA: 0x0021FEE1 File Offset: 0x0021E0E1
		protected override PawnDuty MakeRopeDuty()
		{
			return new PawnDuty(DutyDefOf.PrepareCaravan_GatherAnimals, this.destinationPoint, -1f);
		}

		// Token: 0x06005C91 RID: 23697 RVA: 0x0021FF00 File Offset: 0x0021E100
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				GatherAnimalsAndSlavesForCaravanUtility.CheckArrived(this.lord, this.lord.ownedPawns, this.destinationPoint, "AllAnimalsGathered", new Predicate<Pawn>(AnimalPenUtility.NeedsToBeManagedByRope), (Pawn x) => x.roping.RopedToSpot == this.destinationPoint);
			}
		}
	}
}
