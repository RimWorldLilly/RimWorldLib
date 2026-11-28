using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B42 RID: 2882
	public class JobGiver_GetAgeReversal : ThinkNode_JobGiver
	{
		// Token: 0x06005479 RID: 21625 RVA: 0x001F6370 File Offset: 0x001F4570
		public CompBiosculpterPod GetBiosculpterPod(Pawn pawn)
		{
			List<CompBiosculpterPod> list = CompBiosculpterPod.BiotunedPods(pawn);
			if (list != null && CompBiosculpterPod.CanAgeReverse(pawn) && pawn.ageTracker.AgeReversalDemandedDeadlineTicks <= 0L)
			{
				foreach (CompBiosculpterPod compBiosculpterPod in list)
				{
					if (compBiosculpterPod.parent.Spawned && compBiosculpterPod.AutoAgeReversal && compBiosculpterPod.CanAcceptOnceCycleChosen(pawn) && compBiosculpterPod.PawnCanUseNow(pawn, compBiosculpterPod.GetCycle(compBiosculpterPod.AgeReversalCycleKey)))
					{
						return compBiosculpterPod;
					}
				}
			}
			return null;
		}

		// Token: 0x0600547A RID: 21626 RVA: 0x001F6414 File Offset: 0x001F4614
		public override float GetPriority(Pawn pawn)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return 0f;
			}
			if (this.GetBiosculpterPod(pawn) != null)
			{
				return 7.4f;
			}
			return 0f;
		}

		// Token: 0x0600547B RID: 21627 RVA: 0x001F6438 File Offset: 0x001F4638
		protected override Job TryGiveJob(Pawn pawn)
		{
			CompBiosculpterPod biosculpterPod = this.GetBiosculpterPod(pawn);
			if (biosculpterPod == null || !pawn.CanReserve(biosculpterPod.parent, 1, -1, null, false))
			{
				return null;
			}
			Job job = biosculpterPod.EnterBiosculpterJob();
			biosculpterPod.ConfigureJobForCycle(job, biosculpterPod.GetCycle(biosculpterPod.AgeReversalCycleKey), null);
			return job;
		}
	}
}
