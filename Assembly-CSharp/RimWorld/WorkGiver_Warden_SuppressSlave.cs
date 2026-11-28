using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BEC RID: 3052
	public class WorkGiver_Warden_SuppressSlave : WorkGiver_Warden
	{
		// Token: 0x0600576C RID: 22380 RVA: 0x00206824 File Offset: 0x00204A24
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckIdeology("Slave suppression"))
			{
				return null;
			}
			Pawn pawn2 = t as Pawn;
			if (!base.ShouldTakeCareOfSlave(pawn, pawn2, forced))
			{
				return null;
			}
			if (pawn2.guest.slaveInteractionMode != SlaveInteractionModeDefOf.Suppress || pawn2.Downed || !pawn2.Awake() || !pawn.CanReserve(t, 1, -1, null, forced))
			{
				return null;
			}
			Need_Suppression need_Suppression;
			pawn2.needs.TryGetNeed<Need_Suppression>(out need_Suppression);
			if (need_Suppression == null || !need_Suppression.CanBeSuppressedNow || !pawn2.guest.ScheduledForSlaveSuppression)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.SlaveSuppress, t);
		}
	}
}
