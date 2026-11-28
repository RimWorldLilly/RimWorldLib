using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000925 RID: 2341
	public class PsychicRitualToil_GatherOfferings : PsychicRitualToil
	{
		// Token: 0x06004794 RID: 18324 RVA: 0x001C0D13 File Offset: 0x001BEF13
		protected PsychicRitualToil_GatherOfferings()
		{
		}

		// Token: 0x06004795 RID: 18325 RVA: 0x001C0D1B File Offset: 0x001BEF1B
		public PsychicRitualToil_GatherOfferings(PsychicRitualRoleDef offeringGatherer, IngredientCount requiredOffering)
		{
			this.gathererRole = offeringGatherer;
			this.requiredOffering = requiredOffering;
		}

		// Token: 0x06004796 RID: 18326 RVA: 0x001C0D34 File Offset: 0x001BEF34
		public override void UpdateAllDuties(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			DutyDef def;
			if (this.offeringsGathered || this.PawnsHaveOfferings(psychicRitual))
			{
				this.offeringsGathered = true;
				def = DutyDefOf.Idle;
			}
			else
			{
				def = DutyDefOf.GatherOfferingsForPsychicRitual;
			}
			foreach (Pawn pawn in psychicRitual.assignments.AssignedPawns(this.gathererRole))
			{
				this.SetPawnDuty(pawn, psychicRitual, parent, def, null, null, null, null, 0f, null);
			}
		}

		// Token: 0x06004797 RID: 18327 RVA: 0x001C0DE0 File Offset: 0x001BEFE0
		public override bool Tick(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			return this.offeringsGathered;
		}

		// Token: 0x06004798 RID: 18328 RVA: 0x001C0DE8 File Offset: 0x001BEFE8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.gathererRole, "gathererRole");
			Scribe_Values.Look<bool>(ref this.offeringsGathered, "offeringsGathered", false, false);
			Scribe_Deep.Look<IngredientCount>(ref this.requiredOffering, "requiredOffering", Array.Empty<object>());
		}

		// Token: 0x06004799 RID: 18329 RVA: 0x001C0E28 File Offset: 0x001BF028
		public static float PawnsOfferingCount(IEnumerable<Pawn> pawns, IngredientCount offering)
		{
			float num = 0f;
			foreach (Pawn pawn in pawns)
			{
				foreach (Thing thing in ((IEnumerable<Thing>)pawn.inventory.GetDirectlyHeldThings()))
				{
					if (offering.filter.Allows(thing))
					{
						num += (float)thing.stackCount;
						if (num >= offering.GetBaseCount())
						{
							return offering.GetBaseCount();
						}
					}
				}
			}
			return num;
		}

		// Token: 0x0600479A RID: 18330 RVA: 0x001C0ED8 File Offset: 0x001BF0D8
		private bool PawnsHaveOfferings(PsychicRitual psychicRitual)
		{
			float baseCount = this.requiredOffering.GetBaseCount();
			return PsychicRitualToil_GatherOfferings.PawnsOfferingCount(psychicRitual.assignments.AssignedPawns(this.gathererRole), this.requiredOffering) >= baseCount;
		}

		// Token: 0x0600479B RID: 18331 RVA: 0x001C0F14 File Offset: 0x001BF114
		public override void Notify_PawnJobDone(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, Job job, JobCondition condition)
		{
			base.Notify_PawnJobDone(psychicRitual, parent, pawn, job, condition);
			if (psychicRitual.assignments.RoleForPawn(pawn, true) != this.gathererRole)
			{
				return;
			}
			if (this.offeringsGathered || this.PawnsHaveOfferings(psychicRitual))
			{
				this.offeringsGathered = true;
				this.SetPawnDuty(pawn, psychicRitual, parent, DutyDefOf.Idle, null, null, null, null, 0f, null);
			}
		}

		// Token: 0x0600479C RID: 18332 RVA: 0x001C0F94 File Offset: 0x001BF194
		public override ThinkResult Notify_DutyResult(PsychicRitual psychicRitual, PsychicRitualGraph parent, ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			result = base.Notify_DutyResult(psychicRitual, parent, result, pawn, issueParams);
			if (result.Job != null)
			{
				return result;
			}
			if (psychicRitual.assignments.RoleForPawn(pawn, true) != this.gathererRole)
			{
				return result;
			}
			if (this.offeringsGathered || this.PawnsHaveOfferings(psychicRitual))
			{
				this.offeringsGathered = true;
				this.SetPawnDuty(pawn, psychicRitual, parent, DutyDefOf.Idle, null, null, null, null, 0f, null);
				return new ThinkResult(JobMaker.MakeJob(JobDefOf.Wait, 1, false), null, null, false);
			}
			TaggedString reason = "PsychicRitualToil_GatherOfferings_OfferingUnavailable".Translate(pawn.Named("PAWN"), this.requiredOffering.filter.Summary);
			psychicRitual.LeaveOrCancelPsychicRitual(this.gathererRole, pawn, reason);
			return result;
		}

		// Token: 0x0600479D RID: 18333 RVA: 0x001C107E File Offset: 0x001BF27E
		public override string GetJobReport(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			if (psychicRitual.assignments.RoleForPawn(pawn, true) == this.gathererRole)
			{
				return "PsychicRitualToil_GatherOfferings_JobReport".Translate();
			}
			return base.GetJobReport(psychicRitual, parent, pawn);
		}

		// Token: 0x04002E57 RID: 11863
		protected PsychicRitualRoleDef gathererRole;

		// Token: 0x04002E58 RID: 11864
		protected bool offeringsGathered;

		// Token: 0x04002E59 RID: 11865
		protected IngredientCount requiredOffering;
	}
}
