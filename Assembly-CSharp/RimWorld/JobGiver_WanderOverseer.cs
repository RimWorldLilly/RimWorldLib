using System;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BAF RID: 2991
	public class JobGiver_WanderOverseer : JobGiver_Wander
	{
		// Token: 0x06005638 RID: 22072 RVA: 0x00200C21 File Offset: 0x001FEE21
		public JobGiver_WanderOverseer()
		{
			this.wanderRadius = 3f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06005639 RID: 22073 RVA: 0x00200C46 File Offset: 0x001FEE46
		private GlobalTargetInfo Target(Pawn pawn)
		{
			return pawn.GetOverseer();
		}

		// Token: 0x0600563A RID: 22074 RVA: 0x00200C54 File Offset: 0x001FEE54
		protected override Job TryGiveJob(Pawn pawn)
		{
			GlobalTargetInfo globalTargetInfo = this.Target(pawn);
			if (globalTargetInfo.Map != pawn.Map)
			{
				return null;
			}
			Job job = base.TryGiveJob(pawn);
			job.reportStringOverride = "Escorting".Translate(globalTargetInfo.Thing.Named("TARGET"));
			return job;
		}

		// Token: 0x0600563B RID: 22075 RVA: 0x00200CA8 File Offset: 0x001FEEA8
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return this.Target(pawn).Cell;
		}

		// Token: 0x0600563C RID: 22076 RVA: 0x00200CC4 File Offset: 0x001FEEC4
		protected override void DecorateGotoJob(Job job)
		{
			job.expiryInterval = 120;
			job.expireRequiresEnemiesNearby = true;
		}
	}
}
