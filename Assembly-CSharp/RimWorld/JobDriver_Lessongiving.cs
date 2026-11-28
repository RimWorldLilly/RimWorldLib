using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A07 RID: 2567
	public class JobDriver_Lessongiving : JobDriver
	{
		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x06004D98 RID: 19864 RVA: 0x001DB01F File Offset: 0x001D921F
		private Thing Desk
		{
			get
			{
				return base.TargetThingA;
			}
		}

		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x06004D99 RID: 19865 RVA: 0x001DB027 File Offset: 0x001D9227
		private Pawn Student
		{
			get
			{
				return (Pawn)base.TargetThingB;
			}
		}

		// Token: 0x06004D9A RID: 19866 RVA: 0x001DB034 File Offset: 0x001D9234
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return ModsConfig.BiotechActive && this.pawn.ReserveSittableOrSpot(SchoolUtility.DeskSpotTeacher(this.Desk), this.job, errorOnFailed);
		}

		// Token: 0x06004D9B RID: 19867 RVA: 0x001DB05F File Offset: 0x001D925F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Lessongiving.<MakeNewToils>d__9 <MakeNewToils>d__ = new JobDriver_Lessongiving.<MakeNewToils>d__9(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004D9C RID: 19868 RVA: 0x001DB070 File Offset: 0x001D9270
		private SkillDef ChooseSkill(Pawn student)
		{
			return (from s in this.pawn.skills.skills
			where !s.TotallyDisabled && s.def.lessonInteraction != null && !student.skills.GetSkill(s.def).TotallyDisabled
			orderby s descending
			select s).Take(4).RandomElement<SkillRecord>().def;
		}

		// Token: 0x06004D9D RID: 19869 RVA: 0x001DB0E0 File Offset: 0x001D92E0
		public override string GetReport()
		{
			string str = (this.taughtSkill != null) ? (" (" + this.taughtSkill.label + ")") : "";
			return this.ReportStringProcessed(this.job.def.reportString) + str;
		}

		// Token: 0x06004D9E RID: 19870 RVA: 0x001DB133 File Offset: 0x001D9333
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.isReadyToTeach, "isReadyToTeach", false, false);
			Scribe_Defs.Look<SkillDef>(ref this.taughtSkill, "taughtSkill");
		}

		// Token: 0x0400306E RID: 12398
		private const int InteractionTicks = 900;

		// Token: 0x0400306F RID: 12399
		private const int TopTeachableSkillCount = 4;

		// Token: 0x04003070 RID: 12400
		public bool isReadyToTeach;

		// Token: 0x04003071 RID: 12401
		public SkillDef taughtSkill;
	}
}
