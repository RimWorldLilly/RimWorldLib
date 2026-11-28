using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BE1 RID: 3041
	public class WorkGiver_Warden_DoExecution : WorkGiver_Warden
	{
		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x0600574E RID: 22350 RVA: 0x00205E94 File Offset: 0x00204094
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x0600574F RID: 22351 RVA: 0x00205E97 File Offset: 0x00204097
		public static void ResetStaticData()
		{
			WorkGiver_Warden_DoExecution.IncapableOfViolenceLowerTrans = "IncapableOfViolenceLower".Translate();
		}

		// Token: 0x06005750 RID: 22352 RVA: 0x00205EB0 File Offset: 0x002040B0
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, forced))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if (pawn2.guest.IsInteractionDisabled(PrisonerInteractionModeDefOf.Execution) || !pawn.CanReserve(t, 1, -1, null, forced))
			{
				return null;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				JobFailReason.Is(WorkGiver_Warden_DoExecution.IncapableOfViolenceLowerTrans, null);
				return null;
			}
			if (!base.IsExecutionIdeoAllowed(pawn, pawn2))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.PrisonerExecution, t);
		}

		// Token: 0x04003330 RID: 13104
		private static string IncapableOfViolenceLowerTrans;
	}
}
