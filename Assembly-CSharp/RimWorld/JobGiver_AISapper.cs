using System;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000AE0 RID: 2784
	public class JobGiver_AISapper : ThinkNode_JobGiver
	{
		// Token: 0x0600534C RID: 21324 RVA: 0x001F012B File Offset: 0x001EE32B
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AISapper jobGiver_AISapper = (JobGiver_AISapper)base.DeepCopy(resolve);
			jobGiver_AISapper.canMineMineables = this.canMineMineables;
			jobGiver_AISapper.canMineNonMineables = this.canMineNonMineables;
			return jobGiver_AISapper;
		}

		// Token: 0x0600534D RID: 21325 RVA: 0x001F0154 File Offset: 0x001EE354
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 intVec = pawn.mindState.duty.focus.Cell;
			if (intVec.IsValid && (float)intVec.DistanceToSquared(pawn.Position) < 100f && intVec.GetRoom(pawn.Map) == pawn.GetRoom(RegionType.Set_All) && intVec.WithinRegions(pawn.Position, pawn.Map, 9, TraverseMode.NoPassClosedDoors, RegionType.Set_Passable))
			{
				pawn.GetLord().Notify_ReachedDutyLocation(pawn);
				return null;
			}
			if (!intVec.IsValid)
			{
				IAttackTarget attackTarget;
				if (!(from x in pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn)
				where !x.ThreatDisabled(pawn) && x.Thing.Faction == Faction.OfPlayer && pawn.Position.DistanceToSquared(x.Thing.Position) <= 2500 && pawn.CanReach(x.Thing, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.PassAllDestroyableThings)
				select x).TryRandomElement(out attackTarget))
				{
					return null;
				}
				intVec = attackTarget.Thing.Position;
			}
			if (!pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.PassAllDestroyableThings))
			{
				return null;
			}
			using (PawnPath pawnPath = pawn.Map.pathFinder.FindPathNow(pawn.Position, intVec, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassAllDestroyableThings, false, false, false, true), null, PathEndMode.OnCell, null))
			{
				IntVec3 cellBeforeBlocker;
				Thing thing = pawnPath.FirstBlockingBuilding(out cellBeforeBlocker, pawn);
				if (thing != null)
				{
					Job job = DigUtility.PassBlockerJob(pawn, thing, cellBeforeBlocker, this.canMineMineables, this.canMineNonMineables);
					if (job != null)
					{
						return job;
					}
				}
			}
			return JobMaker.MakeJob(JobDefOf.Goto, intVec, 500, true);
		}

		// Token: 0x0400320F RID: 12815
		private bool canMineMineables = true;

		// Token: 0x04003210 RID: 12816
		private bool canMineNonMineables = true;

		// Token: 0x04003211 RID: 12817
		private const float ReachDestDist = 10f;

		// Token: 0x04003212 RID: 12818
		private const int CheckOverrideInterval = 500;
	}
}
