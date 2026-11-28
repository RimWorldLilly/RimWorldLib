using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B29 RID: 2857
	public class JobGiver_UnnaturalCorpseIdle : ThinkNode_JobGiver
	{
		// Token: 0x06005432 RID: 21554 RVA: 0x001F5178 File Offset: 0x001F3378
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Downed)
			{
				return null;
			}
			UnnaturalCorpseTracker unnaturalCorpseTracker;
			if (!Find.Anomaly.TryGetUnnaturalCorpseTrackerForAwoken(pawn, out unnaturalCorpseTracker))
			{
				return null;
			}
			Pawn haunted = unnaturalCorpseTracker.Haunted;
			if (haunted.DestroyedOrNull())
			{
				return null;
			}
			if (haunted.Dead)
			{
				return JobMaker.MakeJob(JobDefOf.Wait, 300, false);
			}
			return null;
		}
	}
}
