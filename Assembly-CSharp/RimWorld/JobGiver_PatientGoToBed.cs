using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B58 RID: 2904
	public class JobGiver_PatientGoToBed : ThinkNode_JobGiver
	{
		// Token: 0x17000F0A RID: 3850
		// (get) Token: 0x060054D7 RID: 21719 RVA: 0x001F91B4 File Offset: 0x001F73B4
		public override string CrawlingReportStringOverride
		{
			get
			{
				return base.CrawlingReportStringOverride ?? "ReportStringCrawlingToBed".Translate();
			}
		}

		// Token: 0x060054D8 RID: 21720 RVA: 0x001F91D0 File Offset: 0x001F73D0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (this.urgentOnly && !HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn))
			{
				return null;
			}
			if (!HealthAIUtility.ShouldSeekMedicalRest(pawn))
			{
				return null;
			}
			if (this.respectTimetable && RestUtility.TimetablePreventsLayDown(pawn) && !HealthAIUtility.ShouldHaveSurgeryDoneNow(pawn) && !HealthAIUtility.ShouldBeTendedNowByPlayer(pawn))
			{
				return null;
			}
			if (RestUtility.DisturbancePreventsLyingDown(pawn))
			{
				return null;
			}
			if (pawn.Downed && !pawn.health.CanCrawl)
			{
				return null;
			}
			if (pawn.GetPosture().InBed() && pawn.Downed)
			{
				return null;
			}
			Thing thing = RestUtility.FindBedFor(pawn, pawn, false, false, null);
			if (thing == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.LayDown, thing);
			job.checkOverrideOnExpire = true;
			return job;
		}

		// Token: 0x060054D9 RID: 21721 RVA: 0x001F9282 File Offset: 0x001F7482
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_PatientGoToBed jobGiver_PatientGoToBed = (JobGiver_PatientGoToBed)base.DeepCopy(resolve);
			jobGiver_PatientGoToBed.respectTimetable = this.respectTimetable;
			return jobGiver_PatientGoToBed;
		}

		// Token: 0x040032AE RID: 12974
		public bool respectTimetable = true;

		// Token: 0x040032AF RID: 12975
		public bool urgentOnly;
	}
}
