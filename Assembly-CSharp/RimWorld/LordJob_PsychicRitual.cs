using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C93 RID: 3219
	public class LordJob_PsychicRitual : LordJob
	{
		// Token: 0x06005B1A RID: 23322 RVA: 0x002184B4 File Offset: 0x002166B4
		public override AcceptanceReport AllowsDrafting(Pawn pawn)
		{
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return base.AllowsDrafting(pawn);
			}
			return lordToil_PsychicRitual.RitualData.psychicRitual.def.AllowsDrafting(pawn);
		}

		// Token: 0x06005B1B RID: 23323 RVA: 0x002184F3 File Offset: 0x002166F3
		public LordJob_PsychicRitual(PsychicRitualDef def, PsychicRitualRoleAssignments assignments, float points = -1f)
		{
			this.def = def;
			this.assignments = assignments;
			this.points = points;
		}

		// Token: 0x06005B1C RID: 23324 RVA: 0x00218510 File Offset: 0x00216710
		protected LordJob_PsychicRitual()
		{
		}

		// Token: 0x06005B1D RID: 23325 RVA: 0x00218518 File Offset: 0x00216718
		public override StateGraph CreateGraph()
		{
			if (!ModLister.CheckAnomaly("Psychic ritual"))
			{
				return null;
			}
			StateGraph stateGraph = new StateGraph();
			stateGraph.AddToil(new LordToil_PsychicRitual(this.def, this.assignments));
			return stateGraph;
		}

		// Token: 0x06005B1E RID: 23326 RVA: 0x00218544 File Offset: 0x00216744
		public override string GetReport(Pawn pawn)
		{
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return base.GetJobReport(pawn);
			}
			string pawnReport = lordToil_PsychicRitual.RitualData.psychicRitualToil.GetPawnReport(lordToil_PsychicRitual.RitualData.psychicRitual, null, pawn);
			if (pawnReport == null)
			{
				return null;
			}
			return pawnReport.CapitalizeFirst();
		}

		// Token: 0x06005B1F RID: 23327 RVA: 0x00218598 File Offset: 0x00216798
		public override string GetJobReport(Pawn pawn)
		{
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return base.GetJobReport(pawn);
			}
			string jobReport = lordToil_PsychicRitual.RitualData.psychicRitualToil.GetJobReport(lordToil_PsychicRitual.RitualData.psychicRitual, null, pawn);
			if (jobReport == null)
			{
				return null;
			}
			return jobReport.CapitalizeFirst();
		}

		// Token: 0x06005B20 RID: 23328 RVA: 0x002185EC File Offset: 0x002167EC
		public override AcceptanceReport AllowsFloatMenu(Pawn pawn)
		{
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return base.AllowsFloatMenu(pawn);
			}
			return lordToil_PsychicRitual.RitualData.psychicRitualToil.AllowsFloatMenu(lordToil_PsychicRitual.RitualData.psychicRitual, null, pawn);
		}

		// Token: 0x06005B21 RID: 23329 RVA: 0x00218634 File Offset: 0x00216834
		public override bool BlocksSocialInteraction(Pawn pawn)
		{
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return base.BlocksSocialInteraction(pawn);
			}
			return lordToil_PsychicRitual.RitualData.psychicRitualToil.BlocksSocialInteraction(lordToil_PsychicRitual.RitualData.psychicRitual, null, pawn);
		}

		// Token: 0x06005B22 RID: 23330 RVA: 0x0021867C File Offset: 0x0021687C
		public override bool DutyActiveWhenDown(Pawn pawn)
		{
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return base.BlocksSocialInteraction(pawn);
			}
			return lordToil_PsychicRitual.RitualData.psychicRitualToil.DutyActiveWhenDown(lordToil_PsychicRitual.RitualData.psychicRitual, null, pawn);
		}

		// Token: 0x06005B23 RID: 23331 RVA: 0x002186C4 File Offset: 0x002168C4
		public override AcceptanceReport AbilityAllowed(Ability ability)
		{
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return base.AbilityAllowed(ability);
			}
			return lordToil_PsychicRitual.RitualData.psychicRitualToil.AbilityAllowed(lordToil_PsychicRitual.RitualData.psychicRitual, null, ability);
		}

		// Token: 0x06005B24 RID: 23332 RVA: 0x0021870C File Offset: 0x0021690C
		public override bool ShouldRemovePawn(Pawn pawn, PawnLostCondition reason)
		{
			if (reason == PawnLostCondition.LordRejected)
			{
				return true;
			}
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return base.ShouldRemovePawn(pawn, reason);
			}
			return lordToil_PsychicRitual.RitualData.psychicRitualToil.ShouldRemovePawn(lordToil_PsychicRitual.RitualData.psychicRitual, null, pawn, reason);
		}

		// Token: 0x06005B25 RID: 23333 RVA: 0x0021875C File Offset: 0x0021695C
		public override ThinkResult Notify_DutyConstantResult(ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return base.Notify_DutyConstantResult(result, pawn, issueParams);
			}
			return lordToil_PsychicRitual.RitualData.psychicRitualToil.Notify_DutyConstantResult(lordToil_PsychicRitual.RitualData.psychicRitual, null, result, pawn, issueParams);
		}

		// Token: 0x06005B26 RID: 23334 RVA: 0x002187A8 File Offset: 0x002169A8
		public override ThinkResult Notify_DutyResult(ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return base.Notify_DutyResult(result, pawn, issueParams);
			}
			return lordToil_PsychicRitual.RitualData.psychicRitualToil.Notify_DutyResult(lordToil_PsychicRitual.RitualData.psychicRitual, null, result, pawn, issueParams);
		}

		// Token: 0x06005B27 RID: 23335 RVA: 0x002187F4 File Offset: 0x002169F4
		public override void Notify_MapRemoved()
		{
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return;
			}
			lordToil_PsychicRitual.RitualData.psychicRitual.CancelPsychicRitual(null);
		}

		// Token: 0x06005B28 RID: 23336 RVA: 0x0021882C File Offset: 0x00216A2C
		public override void ExposeData()
		{
			Scribe_Defs.Look<PsychicRitualDef>(ref this.def, "def");
			Scribe_Deep.Look<PsychicRitualRoleAssignments>(ref this.assignments, "assignments", Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.points, "points", 0f, false);
		}

		// Token: 0x06005B29 RID: 23337 RVA: 0x00218869 File Offset: 0x00216A69
		public override bool EndPawnJobOnCleanup(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			return ((curJob != null) ? curJob.lord : null) == this.lord;
		}

		// Token: 0x06005B2A RID: 23338 RVA: 0x00218885 File Offset: 0x00216A85
		public override bool PrisonerSecure(Pawn pawn)
		{
			return true;
		}

		// Token: 0x04003437 RID: 13367
		public PsychicRitualDef def;

		// Token: 0x04003438 RID: 13368
		public PsychicRitualRoleAssignments assignments;

		// Token: 0x04003439 RID: 13369
		public float points;
	}
}
