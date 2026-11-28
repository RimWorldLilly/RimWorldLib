using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AB1 RID: 2737
	public class JobGiver_HiveDefense : JobGiver_AIFightEnemies
	{
		// Token: 0x0600527C RID: 21116 RVA: 0x001EBC48 File Offset: 0x001E9E48
		protected override IntVec3 GetFlagPosition(Pawn pawn)
		{
			Hive hive = pawn.mindState.duty.focus.Thing as Hive;
			if (hive != null && hive.Spawned)
			{
				return hive.Position;
			}
			return pawn.Position;
		}

		// Token: 0x0600527D RID: 21117 RVA: 0x001EBC88 File Offset: 0x001E9E88
		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}

		// Token: 0x0600527E RID: 21118 RVA: 0x001EBC9A File Offset: 0x001E9E9A
		protected override Job MeleeAttackJob(Pawn pawn, Thing enemyTarget)
		{
			Job job = base.MeleeAttackJob(pawn, enemyTarget);
			job.attackDoorIfTargetLost = true;
			return job;
		}
	}
}
