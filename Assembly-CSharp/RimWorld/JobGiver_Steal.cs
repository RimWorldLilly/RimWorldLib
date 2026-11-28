using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B23 RID: 2851
	public class JobGiver_Steal : ThinkNode_JobGiver
	{
		// Token: 0x0600541C RID: 21532 RVA: 0x001F4888 File Offset: 0x001F2A88
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (!RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn, true))
			{
				return null;
			}
			Thing thing;
			if (StealAIUtility.TryFindBestItemToSteal(pawn.Position, pawn.Map, 12f, out thing, pawn, null) && !GenAI.InDangerousCombat(pawn))
			{
				Job job = JobMaker.MakeJob(JobDefOf.Steal);
				job.targetA = thing;
				job.targetB = c;
				job.count = Mathf.Min(thing.stackCount, (int)(pawn.GetStatValue(StatDefOf.CarryingCapacity, true, -1) / thing.def.VolumePerUnit));
				return job;
			}
			return null;
		}

		// Token: 0x0400325A RID: 12890
		public const float ItemsSearchRadiusInitial = 7f;

		// Token: 0x0400325B RID: 12891
		private const float ItemsSearchRadiusOngoing = 12f;
	}
}
