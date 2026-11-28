using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B2D RID: 2861
	public class JobGiver_UnnaturalCorpseUnstuck : JobGiver_UnnaturalCorpseSkip
	{
		// Token: 0x0600543F RID: 21567 RVA: 0x001F5542 File Offset: 0x001F3742
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Downed)
			{
				return null;
			}
			if (GenTicks.TicksGame - pawn.TickSpawned <= 30)
			{
				return null;
			}
			if (pawn.pather.MovedRecently(30))
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x06005440 RID: 21568 RVA: 0x001F5578 File Offset: 0x001F3778
		protected override bool TryGetSkipCell(Pawn pawn, Pawn victim, out IntVec3 cell)
		{
			if (base.TryGetCellAlongPath(pawn, victim, 10, out cell))
			{
				return true;
			}
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(pawn.Position, 2f, 10f))
			{
				if (this.ValidateCell(pawn, intVec, victim.PositionHeld, pawn.Map))
				{
					cell = intVec;
					return true;
				}
			}
			if (!pawn.pather.MovedRecently(300))
			{
				cell = victim.PositionHeld;
				return true;
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06005441 RID: 21569 RVA: 0x001F562C File Offset: 0x001F382C
		private bool ValidateCell(Pawn pawn, IntVec3 start, IntVec3 goal, Map map)
		{
			if (!start.Standable(map))
			{
				return false;
			}
			if (start.GetFirstPawn(map) != null)
			{
				return false;
			}
			float num = pawn.Position.DistanceTo(goal);
			return start.DistanceTo(goal) + 1f < num && pawn.CanReach(start, goal, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn);
		}

		// Token: 0x04003268 RID: 12904
		private const int StuckTicks = 30;

		// Token: 0x04003269 RID: 12905
		private const int StuckTeleportToTicks = 300;

		// Token: 0x0400326A RID: 12906
		private const int MaxDist = 10;

		// Token: 0x0400326B RID: 12907
		private const int MinDist = 2;
	}
}
