using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AE7 RID: 2791
	public class JobGiver_AIWaitWithOverseer : ThinkNode_JobGiver
	{
		// Token: 0x06005367 RID: 21351 RVA: 0x001F0F20 File Offset: 0x001EF120
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn overseer = pawn.GetOverseer();
			if (overseer == null || overseer.Awake())
			{
				return null;
			}
			IntVec3 c = this.CanUseCell(pawn.Position, pawn) ? pawn.Position : this.GetWaitDest(overseer.Position, pawn);
			if (c.IsValid)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Wait_WithSleeping, c, overseer);
				job.expiryInterval = 120;
				job.expireRequiresEnemiesNearby = true;
				return job;
			}
			return null;
		}

		// Token: 0x06005368 RID: 21352 RVA: 0x001F0F98 File Offset: 0x001EF198
		private IntVec3 GetWaitDest(IntVec3 root, Pawn mech)
		{
			Map map = mech.Map;
			IntVec3 result;
			if (CellFinder.TryFindRandomReachableNearbyCell(root, map, 1.9f, TraverseParms.For(mech, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), (IntVec3 c) => this.CanUseCell(c, mech), null, out result, 999999))
			{
				return result;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06005369 RID: 21353 RVA: 0x001F1000 File Offset: 0x001EF200
		private bool CanUseCell(IntVec3 c, Pawn mech)
		{
			Map map = mech.Map;
			return c.Standable(map) && mech.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn) && mech.CanReserve(c, 1, -1, null, false) && c.GetDoor(map) == null;
		}

		// Token: 0x0400321F RID: 12831
		private const float RandomCellNearRadius = 1.9f;
	}
}
