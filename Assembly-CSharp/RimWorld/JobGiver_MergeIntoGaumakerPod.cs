using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BD6 RID: 3030
	public class JobGiver_MergeIntoGaumakerPod : ThinkNode_JobGiver
	{
		// Token: 0x0600571D RID: 22301 RVA: 0x0020506C File Offset: 0x0020326C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return null;
			}
			if (pawn.connections == null || pawn.connections.ConnectedThings.NullOrEmpty<Thing>())
			{
				return null;
			}
			foreach (Thing thing in pawn.connections.ConnectedThings)
			{
				CompTreeConnection compTreeConnection = thing.TryGetComp<CompTreeConnection>();
				if (compTreeConnection != null && compTreeConnection.ShouldEnterGaumakerPod(pawn) && pawn.CanReach(compTreeConnection.gaumakerPod, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return JobMaker.MakeJob(JobDefOf.MergeIntoGaumakerPod, thing, compTreeConnection.gaumakerPod);
				}
			}
			return null;
		}
	}
}
