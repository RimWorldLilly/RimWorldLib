using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B3E RID: 2878
	public class JobGiver_FleeImmediateThreat : ThinkNode_JobGiver
	{
		// Token: 0x06005470 RID: 21616 RVA: 0x001F614C File Offset: 0x001F434C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = FleeUtility.FleeLargeFireJob(pawn, 1, this.distToFireToFlee, Mathf.CeilToInt(this.fleeDistRange.RandomInRange));
			if (job != null)
			{
				return job;
			}
			Thing danger;
			if (!GenAI.EnemyIsNear(pawn, (float)this.distToDangerToFlee, out danger, false, false))
			{
				return null;
			}
			return FleeUtility.FleeJob(pawn, danger, Mathf.CeilToInt(this.fleeDistRange.RandomInRange));
		}

		// Token: 0x06005471 RID: 21617 RVA: 0x001F61A8 File Offset: 0x001F43A8
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_FleeImmediateThreat jobGiver_FleeImmediateThreat = (JobGiver_FleeImmediateThreat)base.DeepCopy(resolve);
			jobGiver_FleeImmediateThreat.fleeDistRange = this.fleeDistRange;
			jobGiver_FleeImmediateThreat.distToFireToFlee = this.distToFireToFlee;
			jobGiver_FleeImmediateThreat.distToDangerToFlee = this.distToDangerToFlee;
			return jobGiver_FleeImmediateThreat;
		}

		// Token: 0x0400327D RID: 12925
		private FloatRange fleeDistRange = new FloatRange(50f, 75f);

		// Token: 0x0400327E RID: 12926
		private int distToFireToFlee = 4;

		// Token: 0x0400327F RID: 12927
		private int distToDangerToFlee = 40;
	}
}
