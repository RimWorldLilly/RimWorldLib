using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C14 RID: 3092
	public class WorkGiver_Hack : WorkGiver_Scanner
	{
		// Token: 0x17000F6F RID: 3951
		// (get) Token: 0x06005841 RID: 22593 RVA: 0x0020A864 File Offset: 0x00208A64
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Hackable);
			}
		}

		// Token: 0x06005842 RID: 22594 RVA: 0x0020A870 File Offset: 0x00208A70
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompHackable compHackable = t.TryGetComp<CompHackable>();
			if (compHackable == null)
			{
				return false;
			}
			if (!compHackable.Autohack && !forced)
			{
				return false;
			}
			AcceptanceReport acceptanceReport = compHackable.CanHackNow(pawn);
			if (!acceptanceReport.Accepted && !acceptanceReport.Reason.NullOrEmpty())
			{
				JobFailReason.Is(acceptanceReport.Reason, null);
			}
			return (forced || pawn.CanReserve(t, 1, -1, null, false)) && (!forced || !ModsConfig.IdeologyActive || t.def != ThingDefOf.AncientEnemyTerminal) && acceptanceReport.Accepted;
		}

		// Token: 0x06005843 RID: 22595 RVA: 0x0020A8F9 File Offset: 0x00208AF9
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.TryGetComp<CompHackable>() == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Hack, t);
			job.playerForced = forced;
			return job;
		}
	}
}
