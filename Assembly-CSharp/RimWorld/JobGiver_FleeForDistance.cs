using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B39 RID: 2873
	public class JobGiver_FleeForDistance : ThinkNode_JobGiver
	{
		// Token: 0x06005463 RID: 21603 RVA: 0x001F5D50 File Offset: 0x001F3F50
		protected override Job TryGiveJob(Pawn pawn)
		{
			Thing danger;
			if (GenAI.EnemyIsNear(pawn, this.enemyDistToFleeRange.min, out danger, true, true))
			{
				return null;
			}
			if (GenAI.EnemyIsNear(pawn, this.enemyDistToFleeRange.max, out danger, true, true))
			{
				return FleeUtility.FleeJob(pawn, danger, Mathf.CeilToInt(this.fleeDistRange.RandomInRange));
			}
			return null;
		}

		// Token: 0x06005464 RID: 21604 RVA: 0x001F5DA6 File Offset: 0x001F3FA6
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_FleeForDistance jobGiver_FleeForDistance = (JobGiver_FleeForDistance)base.DeepCopy(resolve);
			jobGiver_FleeForDistance.enemyDistToFleeRange = this.enemyDistToFleeRange;
			jobGiver_FleeForDistance.fleeDistRange = this.fleeDistRange;
			return jobGiver_FleeForDistance;
		}

		// Token: 0x04003271 RID: 12913
		protected FloatRange enemyDistToFleeRange = new FloatRange(2.9f, 7.9f);

		// Token: 0x04003272 RID: 12914
		protected FloatRange fleeDistRange = new FloatRange(13.5f, 20f);
	}
}
