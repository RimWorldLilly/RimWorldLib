using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000918 RID: 2328
	public class PsychicRitual : IExposable, ILoadReferenceable
	{
		// Token: 0x17000D28 RID: 3368
		// (get) Token: 0x060046FA RID: 18170 RVA: 0x001BDE73 File Offset: 0x001BC073
		public Map Map
		{
			get
			{
				return this.lord.Map;
			}
		}

		// Token: 0x17000D29 RID: 3369
		// (get) Token: 0x060046FB RID: 18171 RVA: 0x001BDE80 File Offset: 0x001BC080
		public float PowerPercent
		{
			get
			{
				if (this.maxPower == null)
				{
					return Mathf.Clamp01(this.power);
				}
				return Mathf.Clamp01(this.power / this.maxPower.Value);
			}
		}

		// Token: 0x060046FC RID: 18172 RVA: 0x001BDEB2 File Offset: 0x001BC0B2
		public PsychicRitual()
		{
			this.loadID = Find.UniqueIDsManager.GetNextPsychicRitualID();
		}

		// Token: 0x060046FD RID: 18173 RVA: 0x001BDED4 File Offset: 0x001BC0D4
		public virtual void Start()
		{
			this.power = 0f;
			foreach (KeyValuePair<PsychicRitualRoleDef, List<Pawn>> keyValuePair in this.assignments.RoleAssignments)
			{
				PsychicRitualRoleDef psychicRitualRoleDef;
				List<Pawn> list;
				keyValuePair.Deconstruct(out psychicRitualRoleDef, out list);
				PsychicRitualRoleDef psychicRitualRoleDef2 = psychicRitualRoleDef;
				foreach (Pawn pawn in list)
				{
					if (!pawn.Awake() && !psychicRitualRoleDef2.ConditionAllowed(PsychicRitualRoleDef.Condition.Sleeping))
					{
						if (!pawn.health.capacities.CanBeAwake)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Attempting to wake ");
							defaultInterpolatedStringHandler.AppendFormatted(pawn.ToStringSafe<Pawn>());
							defaultInterpolatedStringHandler.AppendLiteral(" for the ");
							defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(this.def.LabelCap);
							defaultInterpolatedStringHandler.AppendLiteral(" psychic ritual, but they cannot be awake.");
							Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						RestUtility.WakeUp(pawn, false);
					}
					if (pawn != null)
					{
						Pawn_JobTracker jobs = pawn.jobs;
						if (jobs != null)
						{
							jobs.EndCurrentJob(JobCondition.InterruptForced, false, true);
						}
					}
					if (pawn != null)
					{
						Pawn_JobTracker jobs2 = pawn.jobs;
						if (jobs2 != null)
						{
							jobs2.ClearQueuedJobs(true);
						}
					}
					if (pawn.Drafted && !psychicRitualRoleDef2.ConditionAllowed(PsychicRitualRoleDef.Condition.Drafted))
					{
						pawn.drafter.Drafted = false;
					}
				}
			}
		}

		// Token: 0x060046FE RID: 18174 RVA: 0x001BE06C File Offset: 0x001BC26C
		public virtual PsychicRitual.LeftOrCanceled LeaveOrCancelPsychicRitual(PsychicRitualRoleDef role, Pawn pawn, TaggedString reason)
		{
			if (this.def.nonRequiredPawnsMayLeave && this.assignments.RoleAssignedCount(role) > role.MinCount)
			{
				this.LeavePsychicRitual(pawn, reason);
				return PsychicRitual.LeftOrCanceled.Left;
			}
			this.CancelPsychicRitual(reason);
			return PsychicRitual.LeftOrCanceled.Canceled;
		}

		// Token: 0x060046FF RID: 18175 RVA: 0x001BE0A4 File Offset: 0x001BC2A4
		public virtual void CancelPsychicRitual(TaggedString reason)
		{
			Lord lord = this.lord;
			if (((lord != null) ? lord.faction : null) != null && this.lord.faction == Faction.OfPlayer)
			{
				Find.PsychicRitualManager.ClearCooldown(this.def);
				Messages.Message(this.def.PsychicRitualCanceledMessage(reason).CapitalizeFirst().EndWithPeriod(), this.assignments.Target, MessageTypeDefOf.NeutralEvent, true);
			}
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null || lordToil_PsychicRitual.RitualData.removeLordOnCancel)
			{
				this.lord.lordManager.RemoveLord(this.lord);
			}
			this.canceled = true;
		}

		// Token: 0x06004700 RID: 18176 RVA: 0x001BE164 File Offset: 0x001BC364
		public virtual void LeavePsychicRitual(Pawn pawn, TaggedString reason)
		{
			this.TryMessagePawnLost(pawn, reason);
			if (this.lord.ownedPawns.Contains(pawn))
			{
				this.lord.Notify_PawnLost(pawn, PawnLostCondition.LordRejected, null);
			}
		}

		// Token: 0x06004701 RID: 18177 RVA: 0x001BE1A3 File Offset: 0x001BC3A3
		public virtual void ReleaseAllPawnsAndBuildings()
		{
			this.lord.RemoveAllPawns(true);
			this.lord.RemoveAllBuildings();
		}

		// Token: 0x06004702 RID: 18178 RVA: 0x001BE1BC File Offset: 0x001BC3BC
		public void TryMessagePawnLost(Pawn pawn, TaggedString reason)
		{
			Lord lord = this.lord;
			if (((lord != null) ? lord.faction : null) == Faction.OfPlayer && PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				Messages.Message(this.def.LeftPsychicRitualMessage(pawn, reason).CapitalizeFirst().EndWithPeriod(), pawn, MessageTypeDefOf.NeutralEvent, false);
			}
		}

		// Token: 0x06004703 RID: 18179 RVA: 0x001BE220 File Offset: 0x001BC420
		public virtual void Notify_PawnLost(Pawn pawn, PawnLostCondition cond)
		{
			Pawn_JobTracker jobs = pawn.jobs;
			Lord lord;
			if (jobs == null)
			{
				lord = null;
			}
			else
			{
				Job curJob = jobs.curJob;
				lord = ((curJob != null) ? curJob.lord : null);
			}
			if (lord == this.lord)
			{
				Pawn_JobTracker jobs2 = pawn.jobs;
				if (jobs2 != null)
				{
					jobs2.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
			if (cond == PawnLostCondition.LordRejected)
			{
				return;
			}
			if (this.succeeded)
			{
				return;
			}
			TaggedString reason = PsychicRitualRoleDef.PawnLostConditionToPsychicRitualReason(this, pawn, cond);
			PsychicRitualRoleDef psychicRitualRoleDef = this.assignments.RoleForPawn(pawn, true);
			if (psychicRitualRoleDef != null && this.assignments.RoleAssignedCount(psychicRitualRoleDef) <= psychicRitualRoleDef.MinCount)
			{
				this.CancelPsychicRitual(reason);
				return;
			}
			if (!this.def.nonRequiredPawnsMayLeave)
			{
				this.CancelPsychicRitual(reason);
				return;
			}
			if (this.lord.ownedPawns.Contains(pawn))
			{
				this.TryMessagePawnLost(pawn, reason);
			}
		}

		// Token: 0x06004704 RID: 18180 RVA: 0x001BE2E0 File Offset: 0x001BC4E0
		public virtual void ExposeData()
		{
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
			Scribe_Defs.Look<PsychicRitualDef>(ref this.def, "def");
			Scribe_Values.Look<float>(ref this.power, "power", 0f, false);
			Scribe_Values.Look<float?>(ref this.maxPower, "maxPower", null, false);
			Scribe_Values.Look<bool>(ref this.canceled, "canceled", false, false);
			Scribe_Values.Look<bool>(ref this.succeeded, "succeeded", false, false);
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				LordJob_PsychicRitual lordJob_PsychicRitual = this.lord.LordJob as LordJob_PsychicRitual;
				if (lordJob_PsychicRitual != null)
				{
					this.assignments = lordJob_PsychicRitual.assignments;
					return;
				}
				Log.Error("Error during loading psychic ritual: LordJob is not a LordJob_PsychicRitual.");
			}
		}

		// Token: 0x06004705 RID: 18181 RVA: 0x001BE395 File Offset: 0x001BC595
		public string GetUniqueLoadID()
		{
			return "PsychicRitual_" + this.loadID.ToString();
		}

		// Token: 0x04002E27 RID: 11815
		public Lord lord;

		// Token: 0x04002E28 RID: 11816
		public PsychicRitualDef def;

		// Token: 0x04002E29 RID: 11817
		public PsychicRitualRoleAssignments assignments;

		// Token: 0x04002E2A RID: 11818
		public bool canceled;

		// Token: 0x04002E2B RID: 11819
		public bool succeeded;

		// Token: 0x04002E2C RID: 11820
		private int loadID = -1;

		// Token: 0x04002E2D RID: 11821
		public float power;

		// Token: 0x04002E2E RID: 11822
		public float? maxPower;

		// Token: 0x02002BC2 RID: 11202
		public enum LeftOrCanceled
		{
			// Token: 0x0400B2A4 RID: 45732
			Invalid,
			// Token: 0x0400B2A5 RID: 45733
			Left,
			// Token: 0x0400B2A6 RID: 45734
			Canceled
		}
	}
}
