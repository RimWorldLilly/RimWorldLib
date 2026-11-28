using System;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B26 RID: 2854
	public class JobGiver_TakeCombatEnhancingDrug : ThinkNode_JobGiver
	{
		// Token: 0x06005429 RID: 21545 RVA: 0x001F4F2F File Offset: 0x001F312F
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_TakeCombatEnhancingDrug jobGiver_TakeCombatEnhancingDrug = (JobGiver_TakeCombatEnhancingDrug)base.DeepCopy(resolve);
			jobGiver_TakeCombatEnhancingDrug.onlyIfInDanger = this.onlyIfInDanger;
			return jobGiver_TakeCombatEnhancingDrug;
		}

		// Token: 0x0600542A RID: 21546 RVA: 0x001F4F4C File Offset: 0x001F314C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.RaceProps.Humanlike)
			{
				return null;
			}
			if (pawn.IsTeetotaler())
			{
				return null;
			}
			if (Find.TickManager.TicksGame - pawn.mindState.lastTakeCombatEnhancingDrugTick < 20000)
			{
				return null;
			}
			Thing thing = pawn.inventory.FindCombatEnhancingDrug();
			if (thing == null)
			{
				return null;
			}
			if (this.onlyIfInDanger)
			{
				Lord lord = pawn.GetLord();
				if (lord == null)
				{
					if (!this.HarmedRecently(pawn))
					{
						return null;
					}
				}
				else
				{
					int num = 0;
					int num2 = Mathf.Clamp(lord.ownedPawns.Count / 2, 1, 4);
					for (int i = 0; i < lord.ownedPawns.Count; i++)
					{
						if (this.HarmedRecently(lord.ownedPawns[i]))
						{
							num++;
							if (num >= num2)
							{
								break;
							}
						}
					}
					if (num < num2)
					{
						return null;
					}
				}
			}
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, thing);
			job.count = 1;
			return job;
		}

		// Token: 0x0600542B RID: 21547 RVA: 0x001F502A File Offset: 0x001F322A
		private bool HarmedRecently(Pawn pawn)
		{
			return Find.TickManager.TicksGame - pawn.mindState.lastHarmTick < 2500;
		}

		// Token: 0x0600542C RID: 21548 RVA: 0x001F5049 File Offset: 0x001F3249
		[Obsolete("Will be removed in a future update, use pawn.inventory.FindCombatEnhancingDrug()")]
		private Thing FindCombatEnhancingDrug(Pawn pawn)
		{
			return pawn.inventory.FindCombatEnhancingDrug();
		}

		// Token: 0x04003263 RID: 12899
		private bool onlyIfInDanger;

		// Token: 0x04003264 RID: 12900
		private const int TakeEveryTicks = 20000;
	}
}
