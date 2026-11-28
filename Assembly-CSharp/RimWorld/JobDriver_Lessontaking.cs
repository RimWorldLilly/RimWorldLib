using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A08 RID: 2568
	public class JobDriver_Lessontaking : JobDriver
	{
		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x06004DA2 RID: 19874 RVA: 0x001DB3A2 File Offset: 0x001D95A2
		private Thing Desk
		{
			get
			{
				return base.TargetThingA;
			}
		}

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x06004DA3 RID: 19875 RVA: 0x001DB3AA File Offset: 0x001D95AA
		private Pawn Teacher
		{
			get
			{
				return (Pawn)base.TargetThingB;
			}
		}

		// Token: 0x06004DA4 RID: 19876 RVA: 0x001DB3B7 File Offset: 0x001D95B7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.ReserveSittableOrSpot(SchoolUtility.DeskSpotStudent(this.Desk), this.job, errorOnFailed);
		}

		// Token: 0x06004DA5 RID: 19877 RVA: 0x001DB3D6 File Offset: 0x001D95D6
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Lessontaking.<MakeNewToils>d__8 <MakeNewToils>d__ = new JobDriver_Lessontaking.<MakeNewToils>d__8(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004DA6 RID: 19878 RVA: 0x001DB3E8 File Offset: 0x001D95E8
		public override string GetReport()
		{
			string text = this.ReportStringProcessed(this.job.def.reportString);
			if (this.Teacher != null)
			{
				text += " " + "ReportTakingLessonFrom".Translate(this.Teacher.Named("TEACHER"));
				Pawn_JobTracker jobs = this.Teacher.jobs;
				JobDriver_Lessongiving jobDriver_Lessongiving = ((jobs != null) ? jobs.curDriver : null) as JobDriver_Lessongiving;
				if (jobDriver_Lessongiving != null && jobDriver_Lessongiving.taughtSkill != null)
				{
					text = text + " (" + jobDriver_Lessongiving.taughtSkill.label + ")";
				}
			}
			return text;
		}

		// Token: 0x06004DA7 RID: 19879 RVA: 0x001DB489 File Offset: 0x001D9689
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.isReadyToLearn, "isReadyToLearn", false, false);
			Scribe_Values.Look<int>(ref this.waitingForTeacherTicks, "waitForTeacherTicks", 0, false);
		}

		// Token: 0x04003072 RID: 12402
		public const int TicksWaitForTeacher = 5000;

		// Token: 0x04003073 RID: 12403
		public bool isReadyToLearn;

		// Token: 0x04003074 RID: 12404
		public int waitingForTeacherTicks;
	}
}
