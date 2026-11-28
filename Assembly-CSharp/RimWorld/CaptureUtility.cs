using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200099B RID: 2459
	public static class CaptureUtility
	{
		// Token: 0x06004A4D RID: 19021 RVA: 0x001CE234 File Offset: 0x001CC434
		public static void OrderArrest(Pawn arrestor, Pawn victim)
		{
			Thing t;
			if (!CaptureUtility.TryGetBed(arrestor, victim, out t))
			{
				return;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Arrest, victim, t);
			job.count = 1;
			job.playerForced = true;
			arrestor.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
		}

		// Token: 0x06004A4E RID: 19022 RVA: 0x001CE29C File Offset: 0x001CC49C
		public static bool CanArrest(Pawn arrestor, Pawn victim, out string reason)
		{
			Thing thing;
			if (!CaptureUtility.TryGetBed(arrestor, victim, out thing))
			{
				reason = "NoPrisonerBedShort".Translate();
				return false;
			}
			reason = null;
			return true;
		}

		// Token: 0x06004A4F RID: 19023 RVA: 0x001CE2CB File Offset: 0x001CC4CB
		public static bool TryGetBed(Pawn arrestor, Pawn victim, out Thing bed)
		{
			bed = RestUtility.FindBedFor(victim, arrestor, false, false, new GuestStatus?(GuestStatus.Prisoner));
			if (bed == null)
			{
				bed = RestUtility.FindBedFor(victim, arrestor, false, true, new GuestStatus?(GuestStatus.Prisoner));
			}
			return bed != null;
		}
	}
}
