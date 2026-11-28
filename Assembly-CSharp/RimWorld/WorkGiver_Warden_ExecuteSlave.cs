using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BE4 RID: 3044
	public class WorkGiver_Warden_ExecuteSlave : WorkGiver_Warden
	{
		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x06005756 RID: 22358 RVA: 0x00206111 File Offset: 0x00204311
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06005757 RID: 22359 RVA: 0x00206114 File Offset: 0x00204314
		public static void ResetStaticData()
		{
			WorkGiver_Warden_ExecuteSlave.IncapableOfViolenceLowerTrans = "IncapableOfViolenceLower".Translate();
		}

		// Token: 0x06005758 RID: 22360 RVA: 0x0020612C File Offset: 0x0020432C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfSlave(pawn, t, forced))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if (pawn2.guest.slaveInteractionMode != SlaveInteractionModeDefOf.Execute || !pawn.CanReserve(t, 1, -1, null, forced))
			{
				return null;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				JobFailReason.Is(WorkGiver_Warden_ExecuteSlave.IncapableOfViolenceLowerTrans, null);
				return null;
			}
			if (!base.IsExecutionIdeoAllowed(pawn, pawn2))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.SlaveExecution, t);
		}

		// Token: 0x04003331 RID: 13105
		private static string IncapableOfViolenceLowerTrans;
	}
}
