using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200092A RID: 2346
	public class PsychicRitualToil_Multiplex : PsychicRitualToil
	{
		// Token: 0x060047D3 RID: 18387 RVA: 0x001C2E52 File Offset: 0x001C1052
		protected PsychicRitualToil_Multiplex()
		{
		}

		// Token: 0x060047D4 RID: 18388 RVA: 0x001C2E70 File Offset: 0x001C1070
		public PsychicRitualToil_Multiplex(Dictionary<PsychicRitualRoleDef, PsychicRitualToil> roleToils, PsychicRitualToil fallback = null)
		{
			this.roleToils = new Dictionary<PsychicRitualRoleDef, PsychicRitualToil>(roleToils);
			this.fallback = fallback;
			this.childToils = new List<PsychicRitualToil>(roleToils.Values);
			this.childToils.Add(fallback);
			this.childToils.RemoveDuplicates(null);
		}

		// Token: 0x060047D5 RID: 18389 RVA: 0x001C2ED5 File Offset: 0x001C10D5
		public PsychicRitualToil ToilForPawn(PsychicRitual psychicRitual, Pawn pawn)
		{
			return this.roleToils.GetWithFallback(psychicRitual.assignments.RoleForPawn(pawn, true), this.fallback);
		}

		// Token: 0x060047D6 RID: 18390 RVA: 0x001C2EF8 File Offset: 0x001C10F8
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			foreach (PsychicRitualToil psychicRitualToil in this.childToils)
			{
				psychicRitualToil.Start(psychicRitual, parent);
			}
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x001C2F4C File Offset: 0x001C114C
		public override void End(PsychicRitual psychicRitual, PsychicRitualGraph parent, bool success)
		{
			foreach (PsychicRitualToil psychicRitualToil in this.childToils)
			{
				psychicRitualToil.End(psychicRitual, parent, success);
			}
		}

		// Token: 0x060047D8 RID: 18392 RVA: 0x001C2FA0 File Offset: 0x001C11A0
		public override bool Tick(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			bool flag = true;
			foreach (PsychicRitualToil psychicRitualToil in this.childToils)
			{
				flag = (psychicRitualToil.Tick(psychicRitual, parent) && flag);
			}
			return flag;
		}

		// Token: 0x060047D9 RID: 18393 RVA: 0x001C2FF8 File Offset: 0x001C11F8
		public override AcceptanceReport AllowsFloatMenu(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				return psychicRitualToil.AllowsFloatMenu(psychicRitual, parent, pawn);
			}
			return base.AllowsFloatMenu(psychicRitual, parent, pawn);
		}

		// Token: 0x060047DA RID: 18394 RVA: 0x001C3024 File Offset: 0x001C1224
		public override bool BlocksSocialInteraction(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				return psychicRitualToil.BlocksSocialInteraction(psychicRitual, parent, pawn);
			}
			return base.BlocksSocialInteraction(psychicRitual, parent, pawn);
		}

		// Token: 0x060047DB RID: 18395 RVA: 0x001C3050 File Offset: 0x001C1250
		public override void SetPawnDuty(Pawn pawn, PsychicRitual psychicRitual, PsychicRitualGraph parent, DutyDef def, LocalTargetInfo? focus = null, LocalTargetInfo? focusSecond = null, LocalTargetInfo? focusThird = null, string tag = null, float wanderRadius = 0f, Rot4? overrideFacing = null)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				psychicRitualToil.SetPawnDuty(pawn, psychicRitual, parent, def, focus, focusSecond, focusThird, tag, wanderRadius, null);
				return;
			}
			base.SetPawnDuty(pawn, psychicRitual, parent, def, focus, focusSecond, focusThird, tag, wanderRadius, null);
		}

		// Token: 0x060047DC RID: 18396 RVA: 0x001C30A8 File Offset: 0x001C12A8
		public override void UpdateAllDuties(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			foreach (PsychicRitualToil psychicRitualToil in this.childToils)
			{
				psychicRitualToil.UpdateAllDuties(psychicRitual, parent);
			}
		}

		// Token: 0x060047DD RID: 18397 RVA: 0x001C30FC File Offset: 0x001C12FC
		public override bool DutyActiveWhenDown(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				return psychicRitualToil.DutyActiveWhenDown(psychicRitual, parent, pawn);
			}
			return base.DutyActiveWhenDown(psychicRitual, parent, pawn);
		}

		// Token: 0x060047DE RID: 18398 RVA: 0x001C3128 File Offset: 0x001C1328
		public override bool ClearJobOnStart(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				return psychicRitualToil.ClearJobOnStart(psychicRitual, parent, pawn);
			}
			return base.ClearJobOnStart(psychicRitual, parent, pawn);
		}

		// Token: 0x060047DF RID: 18399 RVA: 0x001C3154 File Offset: 0x001C1354
		public override string GetJobReport(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				return psychicRitualToil.GetJobReport(psychicRitual, parent, pawn);
			}
			return base.GetJobReport(psychicRitual, parent, pawn);
		}

		// Token: 0x060047E0 RID: 18400 RVA: 0x001C3180 File Offset: 0x001C1380
		public override IEnumerable<Gizmo> GetPawnGizmos(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				return psychicRitualToil.GetPawnGizmos(psychicRitual, parent, pawn);
			}
			return base.GetPawnGizmos(psychicRitual, parent, pawn);
		}

		// Token: 0x060047E1 RID: 18401 RVA: 0x001C31AC File Offset: 0x001C13AC
		public override IEnumerable<Gizmo> GetBuildingGizmos(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
			return this.childToils.SelectMany((PsychicRitualToil toil) => toil.GetBuildingGizmos(psychicRitual, parent, building));
		}

		// Token: 0x060047E2 RID: 18402 RVA: 0x001C31EC File Offset: 0x001C13EC
		public override string GetPawnReport(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				return psychicRitualToil.GetPawnReport(psychicRitual, parent, pawn);
			}
			return base.GetPawnReport(psychicRitual, parent, pawn);
		}

		// Token: 0x060047E3 RID: 18403 RVA: 0x001C3218 File Offset: 0x001C1418
		public override string GetReport(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			return null;
		}

		// Token: 0x060047E4 RID: 18404 RVA: 0x001C321C File Offset: 0x001C141C
		public override bool ShouldRemovePawn(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, PawnLostCondition condition)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				return psychicRitualToil.ShouldRemovePawn(psychicRitual, parent, pawn, condition);
			}
			return base.ShouldRemovePawn(psychicRitual, parent, pawn, condition);
		}

		// Token: 0x060047E5 RID: 18405 RVA: 0x001C324C File Offset: 0x001C144C
		public override AcceptanceReport AbilityAllowed(PsychicRitual psychicRitual, PsychicRitualGraph parent, Ability ability)
		{
			foreach (PsychicRitualToil psychicRitualToil in this.childToils)
			{
				AcceptanceReport result = psychicRitualToil.AbilityAllowed(psychicRitual, parent, ability);
				if (!result.Accepted)
				{
					return result;
				}
			}
			return base.AbilityAllowed(psychicRitual, parent, ability);
		}

		// Token: 0x060047E6 RID: 18406 RVA: 0x001C32B8 File Offset: 0x001C14B8
		public override void Notify_BuildingDespawnedOnMap(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
			foreach (PsychicRitualToil psychicRitualToil in this.childToils)
			{
				psychicRitualToil.Notify_BuildingDespawnedOnMap(psychicRitual, parent, building);
			}
		}

		// Token: 0x060047E7 RID: 18407 RVA: 0x001C330C File Offset: 0x001C150C
		public override void Notify_BuildingLost(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
			foreach (PsychicRitualToil psychicRitualToil in this.childToils)
			{
				psychicRitualToil.Notify_BuildingLost(psychicRitual, parent, building);
			}
		}

		// Token: 0x060047E8 RID: 18408 RVA: 0x001C3360 File Offset: 0x001C1560
		public override void Notify_BuildingSpawnedOnMap(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
			foreach (PsychicRitualToil psychicRitualToil in this.childToils)
			{
				psychicRitualToil.Notify_BuildingSpawnedOnMap(psychicRitual, parent, building);
			}
		}

		// Token: 0x060047E9 RID: 18409 RVA: 0x001C33B4 File Offset: 0x001C15B4
		public override void Notify_CorpseLost(PsychicRitual psychicRitual, PsychicRitualGraph parent, Corpse corpse)
		{
			foreach (PsychicRitualToil psychicRitualToil in this.childToils)
			{
				psychicRitualToil.Notify_CorpseLost(psychicRitual, parent, corpse);
			}
		}

		// Token: 0x060047EA RID: 18410 RVA: 0x001C3408 File Offset: 0x001C1608
		public override ThinkResult Notify_DutyConstantResult(PsychicRitual psychicRitual, PsychicRitualGraph parent, ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				return psychicRitualToil.Notify_DutyConstantResult(psychicRitual, parent, result, pawn, issueParams);
			}
			return base.Notify_DutyConstantResult(psychicRitual, parent, result, pawn, issueParams);
		}

		// Token: 0x060047EB RID: 18411 RVA: 0x001C3440 File Offset: 0x001C1640
		public override ThinkResult Notify_DutyResult(PsychicRitual psychicRitual, PsychicRitualGraph parent, ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				return psychicRitualToil.Notify_DutyResult(psychicRitual, parent, result, pawn, issueParams);
			}
			return base.Notify_DutyResult(psychicRitual, parent, result, pawn, issueParams);
		}

		// Token: 0x060047EC RID: 18412 RVA: 0x001C3478 File Offset: 0x001C1678
		public override void Notify_PawnJobDone(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, Job job, JobCondition condition)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				psychicRitualToil.Notify_PawnJobDone(psychicRitual, parent, pawn, job, condition);
				return;
			}
			base.Notify_PawnJobDone(psychicRitual, parent, pawn, job, condition);
		}

		// Token: 0x060047ED RID: 18413 RVA: 0x001C34AC File Offset: 0x001C16AC
		public override void Notify_ReachedDutyLocation(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				psychicRitualToil.Notify_ReachedDutyLocation(psychicRitual, parent, pawn);
				return;
			}
			base.Notify_ReachedDutyLocation(psychicRitual, parent, pawn);
		}

		// Token: 0x060047EE RID: 18414 RVA: 0x001C34D8 File Offset: 0x001C16D8
		public override void Notify_PawnLost(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, PawnLostCondition cond)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				psychicRitualToil.Notify_PawnLost(psychicRitual, parent, pawn, cond);
				return;
			}
			base.Notify_PawnLost(psychicRitual, parent, pawn, cond);
		}

		// Token: 0x060047EF RID: 18415 RVA: 0x001C3508 File Offset: 0x001C1708
		public override void Notify_ConstructionFailed(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				psychicRitualToil.Notify_ConstructionFailed(psychicRitual, parent, pawn, frame, newBlueprint);
				return;
			}
			base.Notify_ConstructionFailed(psychicRitual, parent, pawn, frame, newBlueprint);
		}

		// Token: 0x060047F0 RID: 18416 RVA: 0x001C353C File Offset: 0x001C173C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<PsychicRitualRoleDef, PsychicRitualToil>(ref this.roleToils, "roleToils", LookMode.Def, LookMode.Reference, ref this.tmpRolesLoading, ref this.tmpToilsLoading, true, false, false);
			Scribe_Collections.Look<PsychicRitualToil>(ref this.childToils, "childToils", LookMode.Deep, Array.Empty<object>());
			Scribe_References.Look<PsychicRitualToil>(ref this.fallback, "fallback", false);
		}

		// Token: 0x04002E7C RID: 11900
		private Dictionary<PsychicRitualRoleDef, PsychicRitualToil> roleToils;

		// Token: 0x04002E7D RID: 11901
		private List<PsychicRitualToil> childToils;

		// Token: 0x04002E7E RID: 11902
		private PsychicRitualToil fallback;

		// Token: 0x04002E7F RID: 11903
		private List<PsychicRitualRoleDef> tmpRolesLoading = new List<PsychicRitualRoleDef>();

		// Token: 0x04002E80 RID: 11904
		private List<PsychicRitualToil> tmpToilsLoading = new List<PsychicRitualToil>();
	}
}
