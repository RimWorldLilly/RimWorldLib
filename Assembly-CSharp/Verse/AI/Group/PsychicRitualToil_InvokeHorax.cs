using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse.AI.Group
{
	// Token: 0x02000928 RID: 2344
	public class PsychicRitualToil_InvokeHorax : PsychicRitualToil
	{
		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x060047B5 RID: 18357 RVA: 0x001C1A65 File Offset: 0x001BFC65
		public int TicksSinceStarted
		{
			get
			{
				return Find.TickManager.TicksGame - this.startingTick;
			}
		}

		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x060047B6 RID: 18358 RVA: 0x001C1A78 File Offset: 0x001BFC78
		public int TicksLeft
		{
			get
			{
				return Mathf.RoundToInt(this.hoursUntilOutcome * 2500f) - this.TicksSinceStarted;
			}
		}

		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x060047B7 RID: 18359 RVA: 0x001C1A92 File Offset: 0x001BFC92
		public bool ShouldDoCompleteSoonEffect
		{
			get
			{
				return this.TicksLeft <= EffecterDefOf.PsychicRitual_CompleteSoon.maintainTicks;
			}
		}

		// Token: 0x060047B8 RID: 18360 RVA: 0x001C1AAC File Offset: 0x001BFCAC
		public PsychicRitualToil_InvokeHorax(PsychicRitualRoleDef invokerRole, IEnumerable<IntVec3> invokerPositions, PsychicRitualRoleDef targetRole, IEnumerable<IntVec3> targetPositions, PsychicRitualRoleDef chanterRole, IEnumerable<IntVec3> chanterPositions, PsychicRitualRoleDef defenderRole, IEnumerable<IntVec3> defenderPositions, IngredientCount requiredOffering)
		{
			this.invokerRole = invokerRole;
			this.targetRole = targetRole;
			this.chanterRole = chanterRole;
			this.defenderRole = defenderRole;
			this.requiredOffering = requiredOffering;
			this.invokerPositions = new List<IntVec3>(invokerPositions ?? Enumerable.Empty<IntVec3>());
			this.targetPositions = new List<IntVec3>(targetPositions ?? Enumerable.Empty<IntVec3>());
			this.chanterPositions = new List<IntVec3>(chanterPositions ?? Enumerable.Empty<IntVec3>());
			this.defenderPositions = new List<IntVec3>(defenderPositions ?? Enumerable.Empty<IntVec3>());
		}

		// Token: 0x060047B9 RID: 18361 RVA: 0x001C1B47 File Offset: 0x001BFD47
		protected PsychicRitualToil_InvokeHorax()
		{
		}

		// Token: 0x060047BA RID: 18362 RVA: 0x001C1B5C File Offset: 0x001BFD5C
		public virtual void HoldRequiredOfferings(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			if (this.requiredOffering == null)
			{
				return;
			}
			foreach (Pawn pawn in psychicRitual.assignments.AssignedPawns(this.invokerRole))
			{
				ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
				for (int i = innerContainer.Count - 1; i >= 0; i--)
				{
					Thing thing = innerContainer[i];
					if (this.requiredOffering.filter.Allows(thing))
					{
						innerContainer.TryTransferToContainer(thing, pawn.carryTracker.innerContainer, Mathf.CeilToInt(this.requiredOffering.GetBaseCount()), true);
					}
				}
			}
		}

		// Token: 0x060047BB RID: 18363 RVA: 0x001C1C18 File Offset: 0x001BFE18
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			this.startingTick = Find.TickManager.TicksGame;
			this.HoldRequiredOfferings(psychicRitual, parent);
			Lord lord = psychicRitual.lord;
			TaggedString taggedString;
			MessageTypeDef def;
			if (((lord != null) ? lord.faction : null) == Faction.OfPlayer)
			{
				taggedString = "PsychicRitualDef_InvocationCircle_PsychicRitualBegunMessage".Translate(psychicRitual.assignments.FirstAssignedPawn(this.invokerRole), psychicRitual.def.label);
				def = MessageTypeDefOf.NeutralEvent;
			}
			else
			{
				taggedString = "MessageAIPsychicRitualBegan".Translate(psychicRitual.lord.faction.Named("FACTION")).CapitalizeFirst() + ": " + psychicRitual.def.LabelCap;
				def = MessageTypeDefOf.ThreatSmall;
			}
			Messages.Message(taggedString.CapitalizeFirst(), psychicRitual.assignments.Target, def, true);
		}

		// Token: 0x060047BC RID: 18364 RVA: 0x001C1CFC File Offset: 0x001BFEFC
		public override void UpdateAllDuties(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			LocalTargetInfo value = (LocalTargetInfo)psychicRitual.assignments.Target;
			int num = 0;
			bool playerRitual = psychicRitual.lord.faction == Faction.OfPlayer;
			if (this.invokerRole != null)
			{
				foreach (Pawn pawn in psychicRitual.assignments.AssignedPawns(this.invokerRole))
				{
					IntVec3 bestStandableRolePosition = psychicRitual.def.GetBestStandableRolePosition(playerRitual, this.invokerPositions[num++], value.Cell, psychicRitual.Map, 8f);
					this.SetPawnDuty(pawn, psychicRitual, parent, DutyDefOf.Invoke, new LocalTargetInfo?(value), new LocalTargetInfo?(bestStandableRolePosition), null, null, 0f, null);
				}
			}
			if (this.targetRole != null)
			{
				num = 0;
				foreach (Pawn pawn2 in psychicRitual.assignments.AssignedPawns(this.targetRole))
				{
					IntVec3 bestStandableRolePosition2 = psychicRitual.def.GetBestStandableRolePosition(playerRitual, this.targetPositions[num++], value.Cell, psychicRitual.Map, 8f);
					Pawn pawn3 = pawn2;
					DutyDef idle = DutyDefOf.Idle;
					LocalTargetInfo? focus = new LocalTargetInfo?(bestStandableRolePosition2);
					Rot4? overrideFacing = new Rot4?(Rot4.South);
					this.SetPawnDuty(pawn3, psychicRitual, parent, idle, focus, null, null, null, 0f, overrideFacing);
				}
			}
			if (this.chanterRole != null)
			{
				num = 0;
				foreach (Pawn pawn4 in psychicRitual.assignments.AssignedPawns(this.chanterRole))
				{
					IntVec3 bestStandableRolePosition3 = psychicRitual.def.GetBestStandableRolePosition(playerRitual, this.chanterPositions[num++], value.Cell, psychicRitual.Map, 8f);
					this.SetPawnDuty(pawn4, psychicRitual, parent, DutyDefOf.PsychicRitualDance, new LocalTargetInfo?(value), new LocalTargetInfo?(bestStandableRolePosition3), null, null, 0f, null);
				}
			}
			if (this.defenderRole != null)
			{
				num = 0;
				foreach (Pawn pawn5 in psychicRitual.assignments.AssignedPawns(this.defenderRole))
				{
					IntVec3 bestStandableRolePosition4 = psychicRitual.def.GetBestStandableRolePosition(playerRitual, this.defenderPositions[num++], value.Cell, psychicRitual.Map, 8f);
					this.SetPawnDuty(pawn5, psychicRitual, parent, DutyDefOf.DefendInvoker, new LocalTargetInfo?(bestStandableRolePosition4), null, null, null, 3f, null);
				}
			}
		}

		// Token: 0x060047BD RID: 18365 RVA: 0x001C2024 File Offset: 0x001C0224
		protected virtual void UpdateProgressBar(PsychicRitual psychicRitual)
		{
			TargetInfo target = psychicRitual.assignments.Target;
			if (!target.IsValid)
			{
				return;
			}
			if (this.progressBar == null)
			{
				Thing thing = psychicRitual.assignments.Target.Thing;
				if (thing != null && !thing.Destroyed)
				{
					this.progressBar = EffecterDefOf.ProgressBarAlwaysVisible.SpawnAttached(thing, thing.MapHeld, 1f);
				}
				else
				{
					this.progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn(target.Cell, target.Map, 1f);
				}
			}
			Effecter effecter = this.progressBar;
			if (effecter != null)
			{
				effecter.EffectTick(target, TargetInfo.Invalid);
			}
			Effecter effecter2 = this.progressBar;
			SubEffecter_ProgressBar subEffecter_ProgressBar = ((effecter2 != null) ? effecter2.children[0] : null) as SubEffecter_ProgressBar;
			MoteProgressBar moteProgressBar = (subEffecter_ProgressBar != null) ? subEffecter_ProgressBar.mote : null;
			if (moteProgressBar != null)
			{
				moteProgressBar.progress = Mathf.Clamp01((float)this.TicksSinceStarted / (this.hoursUntilOutcome * 2500f));
				moteProgressBar.alwaysShow = true;
			}
		}

		// Token: 0x060047BE RID: 18366 RVA: 0x001C211C File Offset: 0x001C031C
		public override bool Tick(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			if (this.requiredOffering != null)
			{
				float num = this.requiredOffering.GetBaseCount();
				foreach (Pawn pawn in psychicRitual.assignments.AssignedPawns(this.invokerRole))
				{
					Pawn_CarryTracker carryTracker = pawn.carryTracker;
					Thing thing = (carryTracker != null) ? carryTracker.CarriedThing : null;
					if (thing != null && this.requiredOffering.filter.Allows(thing))
					{
						num -= (float)thing.stackCount;
					}
				}
				if (num > 0f)
				{
					psychicRitual.CancelPsychicRitual("PsychicRitualToil_InvokeHorax_MissingRequiredOffering".Translate(psychicRitual.assignments.FirstAssignedPawn(this.invokerRole).Named("PAWN"), this.requiredOffering.filter.Summary));
					return true;
				}
			}
			if (this.targetRole != null)
			{
				Pawn pawn2 = psychicRitual.assignments.FirstAssignedPawn(this.targetRole);
				if (pawn2.mindState.duty != null && pawn2.PositionHeld != pawn2.mindState.duty.focus.Cell)
				{
					if (psychicRitual.lord.ownedPawns.Contains(pawn2))
					{
						psychicRitual.lord.Notify_PawnLost(pawn2, PawnLostCondition.LordRejected, null);
					}
					psychicRitual.CancelPsychicRitual("PsychicRitualToil_InvokeHorax_PawnIsOutOfPosition".Translate(pawn2.Named("PAWN")));
					return true;
				}
			}
			this.UpdateProgressBar(psychicRitual);
			this.TickSound(psychicRitual);
			this.TickVfx(psychicRitual);
			return (float)this.TicksSinceStarted > this.hoursUntilOutcome * 2500f;
		}

		// Token: 0x060047BF RID: 18367 RVA: 0x001C22C0 File Offset: 0x001C04C0
		private void TickVfx(PsychicRitual psychicRitual)
		{
			if (this.effecterOngoing == null)
			{
				this.effecterOngoing = EffecterDefOf.PsychicRitual_Sustained.Spawn(psychicRitual.assignments.Target.Cell, psychicRitual.assignments.Target.Cell, psychicRitual.Map, 1f);
				foreach (Pawn pawn in psychicRitual.assignments.AllAssignedPawns)
				{
					if (psychicRitual.assignments.RoleForPawn(pawn, true).showInvocationVfx)
					{
						this.motes.Add(MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_HateChantShadow, new Vector3(0f, 0f, 0f), 1f, -1f));
						this.motes.Add(MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_PsychicRitualInvocation, new Vector3(0f, 0f, 0f), 1f, -1f));
					}
				}
				float num = 4f;
				int num2 = 6;
				ThingDef[] list = new ThingDef[]
				{
					ThingDefOf.Mote_RitualCandleA,
					ThingDefOf.Mote_RitualCandleB,
					ThingDefOf.Mote_RitualCandleC
				};
				for (int i = 0; i < num2; i++)
				{
					float f = (float)i / (float)num2 * 3.1415927f * 2f;
					IntVec3 cell = psychicRitual.assignments.Target.Cell;
					cell.x += (int)(num * Mathf.Cos(f));
					cell.z += (int)(num * Mathf.Sin(f));
					this.motes.Add(MoteMaker.MakeStaticMote(cell, psychicRitual.Map, Rand.Element<ThingDef>(list), 1f));
				}
			}
			this.effecterOngoing.EffectTick(psychicRitual.assignments.Target, psychicRitual.assignments.Target);
			foreach (Mote mote in this.motes)
			{
				if (mote != null)
				{
					mote.Maintain();
				}
			}
			TargetInfo targetInfo = new TargetInfo(psychicRitual.assignments.Target.Cell, psychicRitual.Map, false);
			if (this.completeSoonEffecter == null && this.ShouldDoCompleteSoonEffect)
			{
				this.completeSoonEffecter = EffecterDefOf.PsychicRitual_CompleteSoon.Spawn(targetInfo, targetInfo, 1f);
			}
			Effecter effecter = this.completeSoonEffecter;
			if (effecter != null)
			{
				effecter.EffectTick(targetInfo, targetInfo);
			}
			if (Rand.MTBEventOccurs(8f, 60f, 1f))
			{
				Pawn pawn2 = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
				if (pawn2 != null && pawn2.Spawned)
				{
					SocialInteractionUtility.ImitateInteractionWithNoPawn(pawn2, InteractionDefOf.CreepyWords);
				}
			}
		}

		// Token: 0x060047C0 RID: 18368 RVA: 0x001C259C File Offset: 0x001C079C
		private void TickSound(PsychicRitual psychicRitual)
		{
			if (this.TicksLeft <= 300)
			{
				this.TickCompleteSfx(psychicRitual);
				return;
			}
			if (this.sustainerOngoingHuman == null)
			{
				this.sustainerOngoingHuman = SoundDefOf.PsychicRitual_Ongoing_Human.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(psychicRitual.assignments.Target.Cell, psychicRitual.Map, false), MaintenanceType.PerTick));
			}
			if (Mathf.Clamp01((float)this.TicksSinceStarted / (this.hoursUntilOutcome * 2500f)) >= 0.6f && this.sustainerOngoing == null)
			{
				this.sustainerOngoing = SoundDefOf.PsychicRitual_Ongoing.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(psychicRitual.assignments.Target.Cell, psychicRitual.Map, false), MaintenanceType.PerTick));
			}
			Sustainer sustainer = this.sustainerOngoingHuman;
			if (sustainer != null)
			{
				sustainer.Maintain();
			}
			Sustainer sustainer2 = this.sustainerOngoing;
			if (sustainer2 == null)
			{
				return;
			}
			sustainer2.Maintain();
		}

		// Token: 0x060047C1 RID: 18369 RVA: 0x001C267C File Offset: 0x001C087C
		private void TickCompleteSfx(PsychicRitual psychicRitual)
		{
			if (this.completeFadeoutTriggered)
			{
				if (this.sustainerOngoingHuman != null)
				{
					this.sustainerOngoingHuman.End();
					this.sustainerOngoingHuman = null;
				}
				if (this.sustainerOngoing != null)
				{
					this.sustainerOngoing.End();
					this.sustainerOngoing = null;
					return;
				}
			}
			else
			{
				if (this.sustainerOngoingHuman == null)
				{
					this.sustainerOngoingHuman = SoundDefOf.PsychicRitual_Ongoing_Human.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(psychicRitual.assignments.Target.Cell, psychicRitual.Map, false), MaintenanceType.PerTick));
				}
				if (this.sustainerOngoing == null)
				{
					this.sustainerOngoing = SoundDefOf.PsychicRitual_Ongoing.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(psychicRitual.assignments.Target.Cell, psychicRitual.Map, false), MaintenanceType.PerTick));
				}
				SoundDefOf.PsychicRitual_Complete.PlayOneShot(SoundInfo.InMap(new TargetInfo(psychicRitual.assignments.Target.Cell, psychicRitual.Map, false), MaintenanceType.None));
				this.completeFadeoutTriggered = true;
			}
		}

		// Token: 0x060047C2 RID: 18370 RVA: 0x001C277C File Offset: 0x001C097C
		public virtual void ConsumeRequiredOffering(PsychicRitual psychicRitual)
		{
			if (this.requiredOffering == null)
			{
				return;
			}
			float num = this.requiredOffering.GetBaseCount();
			foreach (Pawn pawn in psychicRitual.assignments.AssignedPawns(this.invokerRole))
			{
				Pawn_CarryTracker carryTracker = pawn.carryTracker;
				Thing thing = (carryTracker != null) ? carryTracker.CarriedThing : null;
				if (thing != null && this.requiredOffering.filter.Allows(thing))
				{
					int num2 = Mathf.Min(Mathf.CeilToInt(num), thing.stackCount);
					if (num2 < thing.stackCount)
					{
						thing.stackCount -= num2;
					}
					else
					{
						thing.Destroy(DestroyMode.Vanish);
					}
					num -= (float)num2;
					if (num <= 0f)
					{
						return;
					}
				}
			}
			if (num > 0f)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Invocation should consume ");
				defaultInterpolatedStringHandler.AppendFormatted(this.requiredOffering.Summary);
				defaultInterpolatedStringHandler.AppendLiteral(", but ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(num);
				defaultInterpolatedStringHandler.AppendLiteral(" is unaccounted for.");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x060047C3 RID: 18371 RVA: 0x001C28A8 File Offset: 0x001C0AA8
		public override void End(PsychicRitual psychicRitual, PsychicRitualGraph parent, bool success)
		{
			base.End(psychicRitual, parent, success);
			Effecter effecter = this.progressBar;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBar = null;
			PsychicRitualDef_InvocationCircle psychicRitualDef_InvocationCircle = psychicRitual.def as PsychicRitualDef_InvocationCircle;
			if (psychicRitualDef_InvocationCircle != null && psychicRitualDef_InvocationCircle.TargetRole != null)
			{
				Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(psychicRitualDef_InvocationCircle.TargetRole);
				if (pawn != null && pawn.IsPrisonerOfColony)
				{
					pawn.guest.WaitInsteadOfEscapingFor(1250);
				}
			}
			if (!success)
			{
				this.PlayInterruptedSfx(psychicRitual);
				return;
			}
			psychicRitual.succeeded = true;
			TargetInfo targetInfo = new TargetInfo(psychicRitual.assignments.Target.Cell, psychicRitual.Map, false);
			EffecterDefOf.PsychicRitual_Complete.SpawnMaintained(targetInfo, targetInfo, 1f);
			float power;
			psychicRitual.def.CalculateMaxPower(psychicRitual.assignments, null, out power);
			psychicRitual.maxPower = new float?(1f);
			psychicRitual.power = power;
			this.ConsumeRequiredOffering(psychicRitual);
			PsychicRitualRoleAssignments assignments = psychicRitual.assignments;
			Pawn pawn2 = (assignments != null) ? assignments.FirstAssignedPawn(this.invokerRole) : null;
			if (pawn2 != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.PerformedPsychicRitual, new object[]
				{
					pawn2
				});
			}
		}

		// Token: 0x060047C4 RID: 18372 RVA: 0x001C29C8 File Offset: 0x001C0BC8
		public override string GetJobReport(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualRoleDef psychicRitualRoleDef = psychicRitual.assignments.RoleForPawn(pawn, true);
			if (psychicRitualRoleDef == null)
			{
				return base.GetJobReport(psychicRitual, parent, pawn);
			}
			string str = " (" + "DurationLeft".Translate(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false)).Resolve() + ")";
			if (pawn.Downed)
			{
				return null;
			}
			if (psychicRitualRoleDef == this.invokerRole)
			{
				return "PsychicRitualToil_InvokeHorax_JobReport_Invoker".Translate().Resolve() + str;
			}
			if (psychicRitualRoleDef == this.targetRole)
			{
				return "PsychicRitualToil_InvokeHorax_JobReport_Target".Translate().Resolve() + str;
			}
			if (psychicRitualRoleDef == this.chanterRole)
			{
				return "PsychicRitualToil_InvokeHorax_JobReport_Chanter".Translate().Resolve() + str;
			}
			if (psychicRitualRoleDef == this.defenderRole)
			{
				return "PsychicRitualToil_InvokeHorax_JobReport_Defender".Translate().Resolve() + str;
			}
			return base.GetJobReport(psychicRitual, parent, pawn);
		}

		// Token: 0x060047C5 RID: 18373 RVA: 0x001C2AC4 File Offset: 0x001C0CC4
		public override string GetReport(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			return "DurationLeft".Translate(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false)).Resolve() ?? "";
		}

		// Token: 0x060047C6 RID: 18374 RVA: 0x001C2B04 File Offset: 0x001C0D04
		public override ThinkResult Notify_DutyResult(PsychicRitual psychicRitual, PsychicRitualGraph parent, ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			if (result.Job == null)
			{
				PsychicRitualRoleDef psychicRitualRoleDef = psychicRitual.assignments.RoleForPawn(pawn, true);
				if (psychicRitualRoleDef != null && (psychicRitualRoleDef == this.invokerRole || psychicRitualRoleDef == this.chanterRole) && pawn.Position != pawn.mindState.duty.focusSecond.Cell)
				{
					psychicRitual.LeaveOrCancelPsychicRitual(psychicRitualRoleDef, pawn, "PsychicRitualToil_InvokeHorax_PawnIsOutOfPosition".Translate(pawn.Named("PAWN")));
				}
			}
			return base.Notify_DutyResult(psychicRitual, parent, result, pawn, issueParams);
		}

		// Token: 0x060047C7 RID: 18375 RVA: 0x001C2B8F File Offset: 0x001C0D8F
		private Gizmo DebugFinishGizmo()
		{
			return new Command_Action
			{
				defaultLabel = "Debug: Finish psychic ritual",
				defaultDesc = "Finishes the current ritual and causes its outcome to occur immediately.",
				action = delegate()
				{
					this.hoursUntilOutcome = 0f;
				}
			};
		}

		// Token: 0x060047C8 RID: 18376 RVA: 0x001C2BBE File Offset: 0x001C0DBE
		public override IEnumerable<Gizmo> GetBuildingGizmos(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
			PsychicRitualToil_InvokeHorax.<GetBuildingGizmos>d__42 <GetBuildingGizmos>d__ = new PsychicRitualToil_InvokeHorax.<GetBuildingGizmos>d__42(-2);
			<GetBuildingGizmos>d__.<>4__this = this;
			<GetBuildingGizmos>d__.<>3__psychicRitual = psychicRitual;
			<GetBuildingGizmos>d__.<>3__parent = parent;
			<GetBuildingGizmos>d__.<>3__building = building;
			return <GetBuildingGizmos>d__;
		}

		// Token: 0x060047C9 RID: 18377 RVA: 0x001C2BE3 File Offset: 0x001C0DE3
		public override IEnumerable<Gizmo> GetPawnGizmos(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil_InvokeHorax.<GetPawnGizmos>d__43 <GetPawnGizmos>d__ = new PsychicRitualToil_InvokeHorax.<GetPawnGizmos>d__43(-2);
			<GetPawnGizmos>d__.<>4__this = this;
			<GetPawnGizmos>d__.<>3__psychicRitual = psychicRitual;
			<GetPawnGizmos>d__.<>3__parent = parent;
			<GetPawnGizmos>d__.<>3__pawn = pawn;
			return <GetPawnGizmos>d__;
		}

		// Token: 0x060047CA RID: 18378 RVA: 0x001C2C08 File Offset: 0x001C0E08
		private void PlayInterruptedSfx(PsychicRitual psychicRitual)
		{
			SoundDefOf.PsychicRitual_Interrupted.PlayOneShot(SoundInfo.InMap(new TargetInfo(psychicRitual.assignments.Target.Cell, psychicRitual.Map, false), MaintenanceType.None));
			SoundDefOf.PsychicRitual_Interrupted_Human.PlayOneShot(SoundInfo.InMap(new TargetInfo(psychicRitual.assignments.Target.Cell, psychicRitual.Map, false), MaintenanceType.None));
			if (this.sustainerOngoingHuman != null)
			{
				this.sustainerOngoingHuman.info.volumeFactor = 0f;
				this.sustainerOngoingHuman.End();
				this.sustainerOngoingHuman = null;
			}
			if (this.sustainerOngoing != null)
			{
				this.sustainerOngoing.info.volumeFactor = 0f;
				this.sustainerOngoing.End();
				this.sustainerOngoing = null;
			}
		}

		// Token: 0x060047CB RID: 18379 RVA: 0x001C2CD4 File Offset: 0x001C0ED4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.hoursUntilHoraxEffect, "hoursUntilHoraxEffect", 0f, false);
			Scribe_Values.Look<float>(ref this.hoursUntilOutcome, "hoursUntilOutcome", 0f, false);
			Scribe_Collections.Look<IntVec3>(ref this.invokerPositions, "invokerPositions", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<IntVec3>(ref this.targetPositions, "targetPositions", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<IntVec3>(ref this.chanterPositions, "chanterPositions", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<IntVec3>(ref this.defenderPositions, "defenderPositions", LookMode.Value, Array.Empty<object>());
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.targetRole, "targetRole");
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.chanterRole, "chanterRole");
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.defenderRole, "defenderRole");
			Scribe_Values.Look<int>(ref this.startingTick, "startingTick", 0, false);
			Scribe_Deep.Look<IngredientCount>(ref this.requiredOffering, "requiredOffering", Array.Empty<object>());
		}

		// Token: 0x04002E67 RID: 11879
		public float hoursUntilHoraxEffect;

		// Token: 0x04002E68 RID: 11880
		public float hoursUntilOutcome;

		// Token: 0x04002E69 RID: 11881
		protected List<IntVec3> invokerPositions;

		// Token: 0x04002E6A RID: 11882
		protected List<IntVec3> targetPositions;

		// Token: 0x04002E6B RID: 11883
		protected List<IntVec3> chanterPositions;

		// Token: 0x04002E6C RID: 11884
		protected List<IntVec3> defenderPositions;

		// Token: 0x04002E6D RID: 11885
		public PsychicRitualRoleDef invokerRole;

		// Token: 0x04002E6E RID: 11886
		public PsychicRitualRoleDef targetRole;

		// Token: 0x04002E6F RID: 11887
		public PsychicRitualRoleDef chanterRole;

		// Token: 0x04002E70 RID: 11888
		public PsychicRitualRoleDef defenderRole;

		// Token: 0x04002E71 RID: 11889
		public IngredientCount requiredOffering;

		// Token: 0x04002E72 RID: 11890
		private const float CreepyWordsMTBSeconds = 8f;

		// Token: 0x04002E73 RID: 11891
		private int startingTick;

		// Token: 0x04002E74 RID: 11892
		private Effecter progressBar;

		// Token: 0x04002E75 RID: 11893
		private Sustainer sustainerOngoingHuman;

		// Token: 0x04002E76 RID: 11894
		private Sustainer sustainerOngoing;

		// Token: 0x04002E77 RID: 11895
		private Effecter effecterOngoing;

		// Token: 0x04002E78 RID: 11896
		private Effecter completeSoonEffecter;

		// Token: 0x04002E79 RID: 11897
		private bool completeFadeoutTriggered;

		// Token: 0x04002E7A RID: 11898
		private readonly List<Mote> motes = new List<Mote>();
	}
}
