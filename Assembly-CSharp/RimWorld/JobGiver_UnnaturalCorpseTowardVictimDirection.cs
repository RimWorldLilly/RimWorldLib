using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B2C RID: 2860
	public class JobGiver_UnnaturalCorpseTowardVictimDirection : ThinkNode_JobGiver
	{
		// Token: 0x0600543C RID: 21564 RVA: 0x001F5460 File Offset: 0x001F3660
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
			Vector3 normalized = (haunted.PositionHeld - pawn.Position).ToVector3().normalized;
			IntVec3 ideal = pawn.Position + (normalized * 10f).ToIntVec3();
			IntVec3 c;
			if (this.TryGetNearbyCell(pawn, ideal, out c))
			{
				return JobMaker.MakeJob(JobDefOf.Goto, c);
			}
			return null;
		}

		// Token: 0x0600543D RID: 21565 RVA: 0x001F54FC File Offset: 0x001F36FC
		private bool TryGetNearbyCell(Pawn pawn, IntVec3 ideal, out IntVec3 cell)
		{
			return RCellFinder.TryFindRandomCellNearWith(ideal, (IntVec3 x) => x.Standable(pawn.Map) && pawn.CanReach(pawn.Position, x, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn), pawn.Map, out cell, 5, int.MaxValue);
		}

		// Token: 0x04003267 RID: 12903
		private const float LookAheadDist = 10f;
	}
}
