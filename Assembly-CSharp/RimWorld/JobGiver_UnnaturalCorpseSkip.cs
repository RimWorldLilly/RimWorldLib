using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B2A RID: 2858
	public class JobGiver_UnnaturalCorpseSkip : ThinkNode_JobGiver
	{
		// Token: 0x06005434 RID: 21556 RVA: 0x001F51D4 File Offset: 0x001F33D4
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Downed)
			{
				return null;
			}
			UnnaturalCorpseTracker unnaturalCorpseTracker;
			if (!Find.Anomaly.TryGetUnnaturalCorpseTrackerForAwoken(pawn, out unnaturalCorpseTracker))
			{
				return null;
			}
			Pawn haunted = unnaturalCorpseTracker.Haunted;
			if (haunted.DestroyedOrNull())
			{
				return null;
			}
			if (!SkipUtility.CanEntitySkipNow(pawn, AbilityDefOf.UnnaturalCorpseSkip))
			{
				return null;
			}
			if (pawn.CanReachImmediate(haunted, PathEndMode.Touch))
			{
				return null;
			}
			IntVec3 cell;
			if (this.TryGetSkipCell(pawn, haunted, out cell))
			{
				return SkipUtility.GetEntitySkipJob(pawn, cell, AbilityDefOf.UnnaturalCorpseSkip);
			}
			return null;
		}

		// Token: 0x06005435 RID: 21557 RVA: 0x001F5248 File Offset: 0x001F3448
		protected virtual bool TryGetSkipCell(Pawn pawn, Pawn victim, out IntVec3 cell)
		{
			int maxCells = Mathf.CeilToInt(pawn.GetStatValue(StatDefOf.MoveSpeed, true, -1) * 40f);
			return this.TryGetNearbySkipCell(pawn, pawn.PositionHeld, victim, maxCells, out cell);
		}

		// Token: 0x06005436 RID: 21558 RVA: 0x001F5280 File Offset: 0x001F3480
		protected bool TryGetNearbySkipCell(Pawn pawn, IntVec3 startPos, Pawn victim, int maxCells, out IntVec3 cell)
		{
			return RCellFinder.TryFindRandomCellNearWith(startPos, (IntVec3 x) => this.ValidateCell(pawn, x, victim.SpawnedParentOrMe, maxCells, pawn.Map), pawn.Map, out cell, 5, int.MaxValue);
		}

		// Token: 0x06005437 RID: 21559 RVA: 0x001F52D8 File Offset: 0x001F34D8
		protected virtual bool ValidateCell(Pawn pawn, IntVec3 start, Thing goal, int maxCells, Map map)
		{
			return start.Standable(map) && start.GetFence(map) == null && start.GetDoor(map) == null && !start.Fogged(map) && start.GetFirstPawn(map) == null && pawn.Position.InHorDistOf(goal.PositionHeld, (float)maxCells) && !pawn.Position.InHorDistOf(goal.PositionHeld, 5f) && pawn.CanReach(start, goal, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn);
		}

		// Token: 0x06005438 RID: 21560 RVA: 0x001F5368 File Offset: 0x001F3568
		protected bool TryGetCellAlongPath(Pawn pawn, Pawn victim, int maxDist, out IntVec3 cell)
		{
			if (pawn.CanReach(pawn.Position, victim.PositionHeld, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && pawn.pather.Moving && pawn.pather.curPath != null && pawn.pather.curPath.NodesLeftCount > 1)
			{
				for (int i = 1; i < pawn.pather.curPath.NodesLeftCount - 1; i++)
				{
					IntVec3 intVec = pawn.pather.curPath.Peek(i);
					if (this.ValidateCell(pawn, intVec, victim.SpawnedParentOrMe, maxDist, pawn.Map))
					{
						cell = intVec;
						return true;
					}
				}
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x04003265 RID: 12901
		private const float MoveSpeedCellsFactor = 40f;

		// Token: 0x04003266 RID: 12902
		private const float MinDistFromTarget = 5f;
	}
}
