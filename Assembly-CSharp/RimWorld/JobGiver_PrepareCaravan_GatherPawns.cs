using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000AF0 RID: 2800
	public class JobGiver_PrepareCaravan_GatherPawns : JobGiver_PrepareCaravan_RopePawns
	{
		// Token: 0x17000F00 RID: 3840
		// (get) Token: 0x06005387 RID: 21383 RVA: 0x001F19F4 File Offset: 0x001EFBF4
		protected override JobDef RopeJobDef
		{
			get
			{
				return JobDefOf.PrepareCaravan_GatherAnimals;
			}
		}

		// Token: 0x06005388 RID: 21384 RVA: 0x001F19FB File Offset: 0x001EFBFB
		protected override bool AnimalNeedsGathering(Pawn pawn, Pawn animal)
		{
			return JobGiver_PrepareCaravan_GatherPawns.DoesAnimalNeedGathering(pawn, animal);
		}

		// Token: 0x06005389 RID: 21385 RVA: 0x001F1A04 File Offset: 0x001EFC04
		public static bool DoesAnimalNeedGathering(Pawn pawn, Pawn animal)
		{
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			return AnimalPenUtility.NeedsToBeManagedByRope(animal) && !animal.roping.IsRopedByPawn && (!animal.roping.IsRopedToSpot || !(animal.roping.RopedToSpot == cell)) && pawn.GetLord() == animal.GetLord() && GatherAnimalsAndSlavesForCaravanUtility.CanRoperTakeAnimalToDest(pawn, animal, cell) && pawn.CanReserve(animal, 1, -1, null, false);
		}
	}
}
