using System;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BAE RID: 2990
	public class JobGiver_WanderControlGroupTarget : JobGiver_Wander
	{
		// Token: 0x06005633 RID: 22067 RVA: 0x00200B79 File Offset: 0x001FED79
		public JobGiver_WanderControlGroupTarget()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06005634 RID: 22068 RVA: 0x00200B9E File Offset: 0x001FED9E
		private GlobalTargetInfo Target(Pawn pawn)
		{
			return pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Overseer, null).mechanitor.GetControlGroup(pawn).Target;
		}

		// Token: 0x06005635 RID: 22069 RVA: 0x00200BC4 File Offset: 0x001FEDC4
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (this.Target(pawn).Map != pawn.Map)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x06005636 RID: 22070 RVA: 0x00200BF4 File Offset: 0x001FEDF4
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return this.Target(pawn).Cell;
		}

		// Token: 0x06005637 RID: 22071 RVA: 0x00200C10 File Offset: 0x001FEE10
		protected override void DecorateGotoJob(Job job)
		{
			job.expiryInterval = 120;
			job.expireRequiresEnemiesNearby = true;
		}
	}
}
