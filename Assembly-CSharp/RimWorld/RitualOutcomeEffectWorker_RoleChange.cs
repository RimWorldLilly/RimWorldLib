using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020016ED RID: 5869
	public class RitualOutcomeEffectWorker_RoleChange : RitualOutcomeEffectWorker
	{
		// Token: 0x17001934 RID: 6452
		// (get) Token: 0x060092C6 RID: 37574 RVA: 0x0035A1AE File Offset: 0x003583AE
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060092C7 RID: 37575 RVA: 0x0035A1B1 File Offset: 0x003583B1
		public RitualOutcomeEffectWorker_RoleChange()
		{
		}

		// Token: 0x060092C8 RID: 37576 RVA: 0x0035A1B9 File Offset: 0x003583B9
		public RitualOutcomeEffectWorker_RoleChange(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x060092C9 RID: 37577 RVA: 0x0035A1C4 File Offset: 0x003583C4
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			RitualOutcomeComp ritualOutcomeComp = this.def.comps.First<RitualOutcomeComp>();
			float num = ritualOutcomeComp.QualityOffset(jobRitual, base.DataForComp(ritualOutcomeComp));
			bool flag = num > 0.5f;
			RitualOutcomePossibility ritualOutcomePossibility = flag ? this.def.outcomeChances[1] : this.def.outcomeChances[0];
			string text = ritualOutcomePossibility.description.Formatted(jobRitual.Ritual.Label, jobRitual.Ritual.ideo.Named("IDEO")).CapitalizeFirst();
			text += "\n\n" + "RitualOutcomeQualitySpecific".Translate(jobRitual.Ritual.Label, num.ToStringPercent()).CapitalizeFirst() + ":\n";
			text = text + "\n  - " + ritualOutcomeComp.GetDesc(jobRitual, base.DataForComp(ritualOutcomeComp)).CapitalizeFirst();
			Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(ritualOutcomePossibility.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), text, ritualOutcomePossibility.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, jobRitual.selectedTarget, null, null, null, null, 0, true);
			if (flag)
			{
				Pawn pawn = jobRitual.assignments.FirstAssignedPawn("role_changer");
				Precept_Role roleChangeSelection = jobRitual.assignments.RoleChangeSelection;
				Precept_Role role = pawn.Ideo.GetRole(pawn);
				if (roleChangeSelection != null)
				{
					if (role != null)
					{
						role.Unassign(pawn, false);
					}
					roleChangeSelection.Assign(pawn, true);
					return;
				}
				if (role != null)
				{
					role.Unassign(pawn, true);
				}
			}
		}

		// Token: 0x060092CA RID: 37578 RVA: 0x0035A390 File Offset: 0x00358590
		public override RitualOutcomePossibility GetForcedOutcome(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			RitualOutcomeComp ritualOutcomeComp = this.def.comps.First<RitualOutcomeComp>();
			if (ritualOutcomeComp.GetQualityFactor(ritual, ritualTarget, obligation, assignments, base.DataForComp(ritualOutcomeComp)).quality <= 0.5f)
			{
				return this.def.outcomeChances[0];
			}
			return this.def.outcomeChances[1];
		}

		// Token: 0x060092CB RID: 37579 RVA: 0x0035A3EF File Offset: 0x003585EF
		public override IEnumerable<string> BlockingIssues(Precept_Ritual ritual, TargetInfo target, RitualRoleAssignments assignments)
		{
			RitualOutcomeEffectWorker_RoleChange.<BlockingIssues>d__6 <BlockingIssues>d__ = new RitualOutcomeEffectWorker_RoleChange.<BlockingIssues>d__6(-2);
			<BlockingIssues>d__.<>4__this = this;
			<BlockingIssues>d__.<>3__ritual = ritual;
			<BlockingIssues>d__.<>3__target = target;
			<BlockingIssues>d__.<>3__assignments = assignments;
			return <BlockingIssues>d__;
		}
	}
}
