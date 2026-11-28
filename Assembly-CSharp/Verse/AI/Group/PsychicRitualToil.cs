using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200091F RID: 2335
	public abstract class PsychicRitualToil : IExposable, ILoadReferenceable
	{
		// Token: 0x06004758 RID: 18264 RVA: 0x001BFA44 File Offset: 0x001BDC44
		public PsychicRitualToil()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PsychicRitualToil_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(Find.UniqueIDsManager.GetNextPsychicRitualToilID());
			this.uniqueId = defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06004759 RID: 18265 RVA: 0x001BFA8B File Offset: 0x001BDC8B
		public string GetUniqueLoadID()
		{
			return this.uniqueId;
		}

		// Token: 0x0600475A RID: 18266 RVA: 0x001BFA93 File Offset: 0x001BDC93
		public virtual void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
		}

		// Token: 0x0600475B RID: 18267 RVA: 0x001BFA95 File Offset: 0x001BDC95
		public virtual bool Tick(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			return true;
		}

		// Token: 0x0600475C RID: 18268 RVA: 0x001BFA98 File Offset: 0x001BDC98
		public virtual void UpdateAllDuties(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
		}

		// Token: 0x0600475D RID: 18269 RVA: 0x001BFA9A File Offset: 0x001BDC9A
		public virtual void End(PsychicRitual psychicRitual, PsychicRitualGraph parent, bool success)
		{
		}

		// Token: 0x0600475E RID: 18270 RVA: 0x001BFA9C File Offset: 0x001BDC9C
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.uniqueId, "uniqueId", null, false);
		}

		// Token: 0x0600475F RID: 18271 RVA: 0x001BFAB0 File Offset: 0x001BDCB0
		public virtual string GetJobReport(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			return null;
		}

		// Token: 0x06004760 RID: 18272 RVA: 0x001BFAB3 File Offset: 0x001BDCB3
		public virtual string GetPawnReport(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			return psychicRitual.def.GetPawnReport(psychicRitual, pawn);
		}

		// Token: 0x06004761 RID: 18273 RVA: 0x001BFAC2 File Offset: 0x001BDCC2
		public virtual string GetReport(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			return null;
		}

		// Token: 0x06004762 RID: 18274 RVA: 0x001BFAC5 File Offset: 0x001BDCC5
		public virtual AcceptanceReport AllowsFloatMenu(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			return psychicRitual.def.AllowsFloatMenu(pawn);
		}

		// Token: 0x06004763 RID: 18275 RVA: 0x001BFAD3 File Offset: 0x001BDCD3
		public virtual bool BlocksSocialInteraction(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			return psychicRitual.def.BlocksSocialInteraction(pawn);
		}

		// Token: 0x06004764 RID: 18276 RVA: 0x001BFAE1 File Offset: 0x001BDCE1
		public virtual bool ClearJobOnStart(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			return true;
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x001BFAE4 File Offset: 0x001BDCE4
		public virtual AcceptanceReport AbilityAllowed(PsychicRitual psychicRitual, PsychicRitualGraph parent, Ability ability)
		{
			return psychicRitual.def.AbilityAllowed(ability);
		}

		// Token: 0x06004766 RID: 18278 RVA: 0x001BFAF4 File Offset: 0x001BDCF4
		public virtual IEnumerable<Gizmo> GetPawnGizmos(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualRoleDef psychicRitualRoleDef = psychicRitual.assignments.RoleForPawn(pawn, true);
			if (psychicRitualRoleDef == null)
			{
				return Enumerable.Empty<Gizmo>();
			}
			return psychicRitualRoleDef.GetPawnGizmos(psychicRitual, pawn);
		}

		// Token: 0x06004767 RID: 18279 RVA: 0x001BFB20 File Offset: 0x001BDD20
		public virtual IEnumerable<Gizmo> GetBuildingGizmos(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x06004768 RID: 18280 RVA: 0x001BFB28 File Offset: 0x001BDD28
		public virtual void SetPawnDuty(Pawn pawn, PsychicRitual psychicRitual, PsychicRitualGraph parent, DutyDef def, LocalTargetInfo? focus = null, LocalTargetInfo? focusSecond = null, LocalTargetInfo? focusThird = null, string tag = null, float wanderRadius = 0f, Rot4? overrideFacing = null)
		{
			if (((pawn != null) ? pawn.mindState : null) == null)
			{
				return;
			}
			pawn.mindState.duty = new PawnDuty(def, focus ?? LocalTargetInfo.Invalid, focusSecond ?? LocalTargetInfo.Invalid, focusThird ?? LocalTargetInfo.Invalid, -1f)
			{
				tag = tag,
				socialModeMaxOverride = new RandomSocialMode?(RandomSocialMode.Off),
				source = this,
				locomotion = LocomotionUrgency.Sprint,
				wanderRadius = new float?(wanderRadius),
				overrideFacing = (overrideFacing ?? Rot4.Invalid)
			};
		}

		// Token: 0x06004769 RID: 18281 RVA: 0x001BFBF8 File Offset: 0x001BDDF8
		public virtual bool ShouldRemovePawn(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, PawnLostCondition condition)
		{
			PsychicRitualRoleDef psychicRitualRoleDef = psychicRitual.assignments.RoleForPawn(pawn, true);
			return psychicRitualRoleDef == null || !psychicRitualRoleDef.ConditionAllowed(condition);
		}

		// Token: 0x0600476A RID: 18282 RVA: 0x001BFC25 File Offset: 0x001BDE25
		public virtual void Notify_PawnLost(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, PawnLostCondition cond)
		{
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x001BFC27 File Offset: 0x001BDE27
		public virtual void Notify_PawnJobDone(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, Job job, JobCondition condition)
		{
		}

		// Token: 0x0600476C RID: 18284 RVA: 0x001BFC29 File Offset: 0x001BDE29
		public virtual void Notify_BuildingLost(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
		}

		// Token: 0x0600476D RID: 18285 RVA: 0x001BFC2B File Offset: 0x001BDE2B
		public virtual void Notify_CorpseLost(PsychicRitual psychicRitual, PsychicRitualGraph parent, Corpse corpse)
		{
		}

		// Token: 0x0600476E RID: 18286 RVA: 0x001BFC2D File Offset: 0x001BDE2D
		public virtual void Notify_ReachedDutyLocation(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
		}

		// Token: 0x0600476F RID: 18287 RVA: 0x001BFC2F File Offset: 0x001BDE2F
		public virtual void Notify_ConstructionFailed(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
		}

		// Token: 0x06004770 RID: 18288 RVA: 0x001BFC31 File Offset: 0x001BDE31
		public virtual void Notify_BuildingSpawnedOnMap(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
		}

		// Token: 0x06004771 RID: 18289 RVA: 0x001BFC33 File Offset: 0x001BDE33
		public virtual void Notify_BuildingDespawnedOnMap(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
		}

		// Token: 0x06004772 RID: 18290 RVA: 0x001BFC35 File Offset: 0x001BDE35
		public virtual ThinkResult Notify_DutyConstantResult(PsychicRitual psychicRitual, PsychicRitualGraph parent, ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			return result;
		}

		// Token: 0x06004773 RID: 18291 RVA: 0x001BFC38 File Offset: 0x001BDE38
		public virtual ThinkResult Notify_DutyResult(PsychicRitual psychicRitual, PsychicRitualGraph parent, ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			return result;
		}

		// Token: 0x06004774 RID: 18292 RVA: 0x001BFC3B File Offset: 0x001BDE3B
		public virtual bool DutyActiveWhenDown(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			return false;
		}

		// Token: 0x04002E47 RID: 11847
		private string uniqueId;
	}
}
