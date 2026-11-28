using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B64 RID: 2916
	public class JobGiver_SeekAllowedArea : ThinkNode_JobGiver
	{
		// Token: 0x060054FC RID: 21756 RVA: 0x001F9E84 File Offset: 0x001F8084
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.Position.IsForbidden(pawn))
			{
				return null;
			}
			if (this.HasJobWithSpawnedAllowedTarget(pawn))
			{
				return null;
			}
			Region region = pawn.GetRegion(RegionType.Set_Passable);
			if (region == null)
			{
				return null;
			}
			TraverseParms traverseParms = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParms, false);
			Region reg = null;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				if (r.IsDoorway)
				{
					return false;
				}
				if (!r.IsForbiddenEntirely(pawn))
				{
					reg = r;
					return true;
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, 9999, RegionType.Set_Passable);
			if (reg == null)
			{
				return null;
			}
			IntVec3 c;
			if (!reg.TryFindRandomCellInRegionUnforbidden(pawn, null, out c))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Goto, c);
		}

		// Token: 0x060054FD RID: 21757 RVA: 0x001F9F58 File Offset: 0x001F8158
		private bool HasJobWithSpawnedAllowedTarget(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			return curJob != null && (this.IsSpawnedAllowedTarget(curJob.targetA, pawn) || this.IsSpawnedAllowedTarget(curJob.targetB, pawn) || this.IsSpawnedAllowedTarget(curJob.targetC, pawn));
		}

		// Token: 0x060054FE RID: 21758 RVA: 0x001F9FA0 File Offset: 0x001F81A0
		private bool IsSpawnedAllowedTarget(LocalTargetInfo target, Pawn pawn)
		{
			if (!target.IsValid)
			{
				return false;
			}
			if (target.HasThing)
			{
				return target.Thing.Spawned && !target.Thing.Position.IsForbidden(pawn);
			}
			return target.Cell.InBounds(pawn.Map) && !target.Cell.IsForbidden(pawn);
		}
	}
}
