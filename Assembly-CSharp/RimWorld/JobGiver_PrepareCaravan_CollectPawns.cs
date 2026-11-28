using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000AF1 RID: 2801
	public class JobGiver_PrepareCaravan_CollectPawns : JobGiver_PrepareCaravan_RopePawns
	{
		// Token: 0x17000F01 RID: 3841
		// (get) Token: 0x0600538B RID: 21387 RVA: 0x001F1A8E File Offset: 0x001EFC8E
		protected override JobDef RopeJobDef
		{
			get
			{
				return JobDefOf.PrepareCaravan_CollectAnimals;
			}
		}

		// Token: 0x0600538C RID: 21388 RVA: 0x001F1A95 File Offset: 0x001EFC95
		protected override bool AnimalNeedsGathering(Pawn pawn, Pawn animal)
		{
			return JobGiver_PrepareCaravan_CollectPawns.DoesAnimalNeedGathering(pawn, animal);
		}

		// Token: 0x0600538D RID: 21389 RVA: 0x001F1AA0 File Offset: 0x001EFCA0
		public static bool DoesAnimalNeedGathering(Pawn pawn, Pawn animal)
		{
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			return AnimalPenUtility.NeedsToBeManagedByRope(animal) && !GatherAnimalsAndSlavesForCaravanUtility.IsRopedByCaravanPawn(animal) && pawn.GetLord() == animal.GetLord() && GatherAnimalsAndSlavesForCaravanUtility.CanRoperTakeAnimalToDest(pawn, animal, cell) && pawn.CanReserve(animal, 1, -1, null, false);
		}
	}
}
