using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B9B RID: 2971
	public class JobGiver_EscapingHoldingPlatform : JobGiver_AIFightEnemies
	{
		// Token: 0x06005600 RID: 22016 RVA: 0x001FFD69 File Offset: 0x001FDF69
		public JobGiver_EscapingHoldingPlatform()
		{
			this.chaseTarget = true;
		}

		// Token: 0x06005601 RID: 22017 RVA: 0x001FFD78 File Offset: 0x001FDF78
		protected override Job MeleeAttackJob(Pawn pawn, Thing enemyTarget)
		{
			Job job = base.MeleeAttackJob(pawn, enemyTarget);
			job.attackDoorIfTargetLost = true;
			job.canBashDoors = true;
			return job;
		}

		// Token: 0x06005602 RID: 22018 RVA: 0x001FFD90 File Offset: 0x001FDF90
		protected override bool ShouldLoseTarget(Pawn pawn)
		{
			Thing enemyTarget = pawn.mindState.enemyTarget;
			return enemyTarget.Destroyed || ((IAttackTarget)enemyTarget).ThreatDisabled(pawn);
		}
	}
}
