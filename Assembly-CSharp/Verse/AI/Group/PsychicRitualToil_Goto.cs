using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000926 RID: 2342
	public class PsychicRitualToil_Goto : PsychicRitualToil
	{
		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x0600479E RID: 18334 RVA: 0x001C10AE File Offset: 0x001BF2AE
		public bool AnyPawnsArrived
		{
			get
			{
				return this.finalGoto.AnyPawnsDone;
			}
		}

		// Token: 0x17000D37 RID: 3383
		// (get) Token: 0x0600479F RID: 18335 RVA: 0x001C10BB File Offset: 0x001BF2BB
		public bool FinalGatherPhase
		{
			get
			{
				return !this.allowWanderWhileWaiting || this.initialGoto.AllPawnsDone;
			}
		}

		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x060047A0 RID: 18336 RVA: 0x001C10D2 File Offset: 0x001BF2D2
		public virtual IEnumerable<Pawn> BlockingPawns
		{
			get
			{
				if (this.FinalGatherPhase)
				{
					return this.finalGoto.BlockingPawns;
				}
				return this.initialGoto.BlockingPawns;
			}
		}

		// Token: 0x060047A1 RID: 18337 RVA: 0x001C10F3 File Offset: 0x001BF2F3
		protected PsychicRitualToil_Goto()
		{
		}

		// Token: 0x060047A2 RID: 18338 RVA: 0x001C10FC File Offset: 0x001BF2FC
		public PsychicRitualToil_Goto(IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> rolePositions)
		{
			this.rolePositions = new Dictionary<PsychicRitualRoleDef, List<IntVec3>>(rolePositions.Count);
			foreach (KeyValuePair<PsychicRitualRoleDef, List<IntVec3>> keyValuePair in rolePositions)
			{
				PsychicRitualRoleDef psychicRitualRoleDef;
				List<IntVec3> list;
				keyValuePair.Deconstruct(out psychicRitualRoleDef, out list);
				PsychicRitualRoleDef key = psychicRitualRoleDef;
				List<IntVec3> collection = list;
				this.rolePositions[key] = new List<IntVec3>(collection);
			}
		}

		// Token: 0x060047A3 RID: 18339 RVA: 0x001C1178 File Offset: 0x001BF378
		public virtual List<Pawn> ControlledPawns(PsychicRitual psychicRitual)
		{
			PsychicRitualToil_Goto.tmpControlledPawns.Clear();
			foreach (PsychicRitualRoleDef psychicRitualRoleDef in this.rolePositions.Keys)
			{
				if (psychicRitualRoleDef.psychicRitualWaitsForArrival)
				{
					PsychicRitualToil_Goto.tmpControlledPawns.AddRange(psychicRitual.assignments.AssignedPawns(psychicRitualRoleDef));
				}
			}
			return PsychicRitualToil_Goto.tmpControlledPawns;
		}

		// Token: 0x060047A4 RID: 18340 RVA: 0x001C11F8 File Offset: 0x001BF3F8
		public virtual bool WaitingOnPawn(Pawn pawn)
		{
			if (this.FinalGatherPhase)
			{
				return this.finalGoto.WaitingOnPawn(pawn);
			}
			return this.initialGoto.WaitingOnPawn(pawn);
		}

		// Token: 0x060047A5 RID: 18341 RVA: 0x001C121C File Offset: 0x001BF41C
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			List<Pawn> pawns = this.ControlledPawns(psychicRitual);
			this.initialGoto = new JobSyncTracker(pawns, this, "initial", "goto", JobCondition.Succeeded);
			this.finalGoto = new JobSyncTracker(pawns, this, "final", "goto", JobCondition.Succeeded);
			this.finalGatherPhaseStarted = !this.allowWanderWhileWaiting;
		}

		// Token: 0x060047A6 RID: 18342 RVA: 0x001C1270 File Offset: 0x001BF470
		public override void UpdateAllDuties(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			bool finalGatherPhase = this.FinalGatherPhase;
			foreach (KeyValuePair<PsychicRitualRoleDef, List<IntVec3>> keyValuePair in this.rolePositions)
			{
				PsychicRitualRoleDef psychicRitualRoleDef;
				List<IntVec3> list;
				keyValuePair.Deconstruct(out psychicRitualRoleDef, out list);
				PsychicRitualRoleDef role = psychicRitualRoleDef;
				List<IntVec3> list2 = list;
				int num = 0;
				foreach (Pawn pawn in psychicRitual.assignments.AssignedPawns(role))
				{
					IntVec3 c = list2[num++];
					DutyDef dutyDef;
					if (finalGatherPhase || this.initialGoto.WaitingOnPawn(pawn))
					{
						dutyDef = (pawn.IsPrisonerOfColony ? DutyDefOf.Idle : DutyDefOf.Goto);
					}
					else
					{
						dutyDef = DutyDefOf.WanderClose;
					}
					string text = finalGatherPhase ? "final" : "initial";
					Pawn pawn2 = pawn;
					DutyDef def = dutyDef;
					LocalTargetInfo? focus = new LocalTargetInfo?(c);
					string tag = text;
					this.SetPawnDuty(pawn2, psychicRitual, parent, def, focus, null, null, tag, 0f, null);
				}
			}
		}

		// Token: 0x060047A7 RID: 18343 RVA: 0x001C13B8 File Offset: 0x001BF5B8
		public override bool Tick(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			foreach (Pawn pawn in this.ControlledPawns(psychicRitual))
			{
				Pawn_MindState mindState = pawn.mindState;
				if (((mindState != null) ? mindState.duty : null) == null)
				{
					int num;
					if (this.lastUpdateDutyTick != GenTicks.TicksGame - 1)
					{
						num = 0;
					}
					else
					{
						int num2 = this.updatesInRow;
						this.updatesInRow = num2 + 1;
						num = num2;
					}
					this.updatesInRow = num;
					this.lastUpdateDutyTick = GenTicks.TicksGame;
					if (this.updatesInRow == 10)
					{
						Debug.LogWarning("UpdateAllDuties called 10 times in 10 ticks, ensure controlled pawns are being assigned valid duties.");
					}
					this.UpdateAllDuties(psychicRitual, parent);
					break;
				}
			}
			foreach (Pawn pawn2 in this.ControlledPawns(psychicRitual))
			{
				if (pawn2.IsPrisonerOfColony)
				{
					IntVec3 position = pawn2.Position;
					Pawn_MindState mindState2 = pawn2.mindState;
					IntVec3? intVec;
					if (mindState2 == null)
					{
						intVec = null;
					}
					else
					{
						PawnDuty duty = mindState2.duty;
						intVec = ((duty != null) ? new IntVec3?(duty.focus.Cell) : null);
					}
					if (position == intVec)
					{
						this.ForceFinished(pawn2);
					}
				}
				if (pawn2.Downed && pawn2.Spawned)
				{
					IntVec3 position = pawn2.Position;
					Pawn_MindState mindState3 = pawn2.mindState;
					IntVec3? intVec2;
					if (mindState3 == null)
					{
						intVec2 = null;
					}
					else
					{
						PawnDuty duty2 = mindState3.duty;
						intVec2 = ((duty2 != null) ? new IntVec3?(duty2.focus.Cell) : null);
					}
					if (!(position != intVec2))
					{
						this.ForceFinished(pawn2);
					}
				}
			}
			if (this.FinalGatherPhase && !this.finalGatherPhaseStarted)
			{
				this.finalGatherPhaseStarted = true;
				this.UpdateAllDuties(psychicRitual, parent);
				foreach (Pawn pawn3 in this.ControlledPawns(psychicRitual))
				{
					pawn3.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
			return this.finalGoto.AllPawnsDone;
		}

		// Token: 0x060047A8 RID: 18344 RVA: 0x001C1600 File Offset: 0x001BF800
		private void ForceFinished(Pawn pawn)
		{
			if (this.FinalGatherPhase)
			{
				this.finalGoto.ForcePawnFinished(pawn);
				return;
			}
			this.initialGoto.ForcePawnFinished(pawn);
		}

		// Token: 0x060047A9 RID: 18345 RVA: 0x001C1624 File Offset: 0x001BF824
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.finalGatherPhaseStarted, "finalGatherPhaseStarted", false, false);
			Scribe_Deep.Look<JobSyncTracker>(ref this.initialGoto, "initialGoto", Array.Empty<object>());
			Scribe_Deep.Look<JobSyncTracker>(ref this.finalGoto, "finalGoto", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.allowWanderWhileWaiting, "allowWanderWhileWaiting", false, false);
			Scribe_Collections.Look<PsychicRitualRoleDef, List<IntVec3>>(ref this.rolePositions, "rolePositions", LookMode.Def, LookMode.Value);
		}

		// Token: 0x060047AA RID: 18346 RVA: 0x001C1698 File Offset: 0x001BF898
		public override void Notify_PawnJobDone(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, Job job, JobCondition condition)
		{
			this.initialGoto.Notify_PawnJobDone(pawn, job, condition);
			this.finalGoto.Notify_PawnJobDone(pawn, job, condition);
			LocalTargetInfo value = (LocalTargetInfo)psychicRitual.assignments.Target;
			if (this.FinalGatherPhase)
			{
				if (!this.finalGoto.WaitingOnPawn(pawn))
				{
					this.SetPawnDuty(pawn, psychicRitual, parent, DutyDefOf.WaitForRitualParticipants, new LocalTargetInfo?(value), new LocalTargetInfo?(pawn.PositionHeld), null, null, 0f, null);
					return;
				}
			}
			else if (!this.initialGoto.WaitingOnPawn(pawn))
			{
				this.SetPawnDuty(pawn, psychicRitual, parent, DutyDefOf.WaitForRitualParticipants, new LocalTargetInfo?(value), new LocalTargetInfo?(pawn.PositionHeld), null, null, 0f, null);
			}
		}

		// Token: 0x060047AB RID: 18347 RVA: 0x001C1776 File Offset: 0x001BF976
		public override void Notify_PawnLost(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, PawnLostCondition cond)
		{
			this.initialGoto.Notify_PawnLost(pawn);
			this.finalGoto.Notify_PawnLost(pawn);
		}

		// Token: 0x060047AC RID: 18348 RVA: 0x001C1790 File Offset: 0x001BF990
		public override ThinkResult Notify_DutyResult(PsychicRitual psychicRitual, PsychicRitualGraph parent, ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			if (result.Job != null)
			{
				return result;
			}
			PsychicRitualRoleDef psychicRitualRoleDef = psychicRitual.assignments.RoleForPawn(pawn, true);
			if (psychicRitualRoleDef == null)
			{
				return result;
			}
			if (psychicRitualRoleDef.ConditionAllowed(PsychicRitualRoleDef.Condition.NoPath))
			{
				return result;
			}
			if (psychicRitualRoleDef.CanReach(pawn, psychicRitual.assignments.Target))
			{
				return result;
			}
			TaggedString reason = PsychicRitualRoleDef.ConditionToReason(pawn, PsychicRitualRoleDef.Condition.NoPath);
			psychicRitual.LeaveOrCancelPsychicRitual(psychicRitualRoleDef, pawn, reason);
			return result;
		}

		// Token: 0x060047AD RID: 18349 RVA: 0x001C17FC File Offset: 0x001BF9FC
		public override string GetJobReport(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			bool finalGatherPhase = this.FinalGatherPhase;
			if (!finalGatherPhase && this.initialGoto.WaitingOnPawn(pawn))
			{
				return "PsychicRitualToil_Goto_JobReport_Moving".Translate();
			}
			if (finalGatherPhase && this.finalGoto.WaitingOnPawn(pawn))
			{
				return "PsychicRitualToil_Goto_JobReport_Moving".Translate();
			}
			return "PsychicRitualToil_Goto_JobReport_Waiting".Translate();
		}

		// Token: 0x04002E5A RID: 11866
		protected const string FinalSyncTag = "final";

		// Token: 0x04002E5B RID: 11867
		protected const string InitialSyncTag = "initial";

		// Token: 0x04002E5C RID: 11868
		protected const string GotoTag = "goto";

		// Token: 0x04002E5D RID: 11869
		public bool allowWanderWhileWaiting;

		// Token: 0x04002E5E RID: 11870
		protected Dictionary<PsychicRitualRoleDef, List<IntVec3>> rolePositions;

		// Token: 0x04002E5F RID: 11871
		protected JobSyncTracker initialGoto;

		// Token: 0x04002E60 RID: 11872
		protected JobSyncTracker finalGoto;

		// Token: 0x04002E61 RID: 11873
		private bool finalGatherPhaseStarted;

		// Token: 0x04002E62 RID: 11874
		private int lastUpdateDutyTick;

		// Token: 0x04002E63 RID: 11875
		private int updatesInRow;

		// Token: 0x04002E64 RID: 11876
		private static List<Pawn> tmpControlledPawns = new List<Pawn>(8);
	}
}
