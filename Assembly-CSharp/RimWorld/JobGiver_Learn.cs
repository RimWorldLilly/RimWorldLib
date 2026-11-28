using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B8D RID: 2957
	public class JobGiver_Learn : ThinkNode_JobGiver
	{
		// Token: 0x060055CA RID: 21962 RVA: 0x001FF11C File Offset: 0x001FD31C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return null;
			}
			if (pawn.needs.learning.CurLevelPercentage >= 0.95f)
			{
				return null;
			}
			List<LearningDesireDef> activeLearningDesires = pawn.learning.ActiveLearningDesires;
			if (activeLearningDesires.NullOrEmpty<LearningDesireDef>())
			{
				return null;
			}
			JobGiver_Learn.tmpRandomLearningDesires.Clear();
			JobGiver_Learn.tmpRandomLearningDesires.AddRange(activeLearningDesires.InRandomOrder(null));
			for (int i = 0; i < JobGiver_Learn.tmpRandomLearningDesires.Count; i++)
			{
				if (JobGiver_Learn.tmpRandomLearningDesires[i].Worker.CanDo(pawn))
				{
					Job job = JobGiver_Learn.tmpRandomLearningDesires[i].Worker.TryGiveJob(pawn);
					if (job != null)
					{
						JobGiver_Learn.tmpRandomLearningDesires.Clear();
						job.isLearningDesire = true;
						return job;
					}
				}
			}
			JobGiver_Learn.tmpRandomLearningDesires.Clear();
			return null;
		}

		// Token: 0x040032F3 RID: 13043
		private const float MaxLearning = 0.95f;

		// Token: 0x040032F4 RID: 13044
		private static List<LearningDesireDef> tmpRandomLearningDesires = new List<LearningDesireDef>();
	}
}
