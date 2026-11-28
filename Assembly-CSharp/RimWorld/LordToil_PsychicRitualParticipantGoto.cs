using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CEE RID: 3310
	public class LordToil_PsychicRitualParticipantGoto : LordToil
	{
		// Token: 0x17001056 RID: 4182
		// (get) Token: 0x06005D6E RID: 23918 RVA: 0x00223BC1 File Offset: 0x00221DC1
		protected LordToilData_PsychicRitualParticipantGoto Data
		{
			get
			{
				return (LordToilData_PsychicRitualParticipantGoto)this.data;
			}
		}

		// Token: 0x06005D6F RID: 23919 RVA: 0x00223BCE File Offset: 0x00221DCE
		public LordToil_PsychicRitualParticipantGoto(int timeoutTicks = -1)
		{
			this.data = new LordToilData_PsychicRitualParticipantGoto();
			this.Data.timeoutTicks = timeoutTicks;
		}

		// Token: 0x17001057 RID: 4183
		// (get) Token: 0x06005D70 RID: 23920 RVA: 0x00223BED File Offset: 0x00221DED
		public bool AnyPawnsArrived
		{
			get
			{
				return this.Data.finalGoto.AnyPawnsDone;
			}
		}

		// Token: 0x17001058 RID: 4184
		// (get) Token: 0x06005D71 RID: 23921 RVA: 0x00223BFF File Offset: 0x00221DFF
		public bool AllPawnsArrived
		{
			get
			{
				return this.Data.finalGoto.AllPawnsDone;
			}
		}

		// Token: 0x17001059 RID: 4185
		// (get) Token: 0x06005D72 RID: 23922 RVA: 0x00223C11 File Offset: 0x00221E11
		public bool TimedOut
		{
			get
			{
				return this.Data.timeoutTicks != -1 && this.Data.ticksElapsed >= this.Data.timeoutTicks;
			}
		}

		// Token: 0x06005D73 RID: 23923 RVA: 0x00223C3E File Offset: 0x00221E3E
		public LordToil_PsychicRitualParticipantGoto(IEnumerable<PsychicRitualParticipant> participants)
		{
			this.data = new LordToilData_PsychicRitualParticipantGoto();
			this.SetParticipants(participants);
		}

		// Token: 0x06005D74 RID: 23924 RVA: 0x00223C58 File Offset: 0x00221E58
		public override void UpdateAllDuties()
		{
			if (this.Data.participants.Count == 0)
			{
				return;
			}
			foreach (PsychicRitualParticipant psychicRitualParticipant in this.Participants)
			{
				Pawn pawn;
				IntVec3 intVec;
				psychicRitualParticipant.Deconstruct(out pawn, out intVec);
				Pawn pawn2 = pawn;
				IntVec3 c = intVec;
				DutyDef goto_NoZeroLengthPaths = DutyDefOf.Goto_NoZeroLengthPaths;
				pawn2.mindState.duty = new PawnDuty(goto_NoZeroLengthPaths, c, -1f)
				{
					tag = "final"
				};
			}
		}

		// Token: 0x06005D75 RID: 23925 RVA: 0x00223CEC File Offset: 0x00221EEC
		public override void LordToilTick()
		{
			this.Data.ticksElapsed++;
		}

		// Token: 0x06005D76 RID: 23926 RVA: 0x00223D04 File Offset: 0x00221F04
		public override void Notify_PawnJobDone(Pawn pawn, JobCondition condition)
		{
			LordToilData_PsychicRitualParticipantGoto data = this.Data;
			if (data != null)
			{
				JobSyncTracker finalGoto = data.finalGoto;
				if (finalGoto != null)
				{
					finalGoto.Notify_PawnJobDone(pawn, pawn.CurJob, condition);
				}
			}
			LordToilData_PsychicRitualParticipantGoto data2 = this.Data;
			if (((data2 != null) ? data2.finalGoto : null) != null && !this.Data.finalGoto.WaitingOnPawn(pawn))
			{
				Pawn_MindState mindState = pawn.mindState;
				if (((mindState != null) ? mindState.duty : null) != null)
				{
					pawn.mindState.duty.def = DutyDefOf.Idle;
				}
			}
		}

		// Token: 0x06005D77 RID: 23927 RVA: 0x00223D85 File Offset: 0x00221F85
		public override void Notify_PawnLost(Pawn victim, PawnLostCondition cond)
		{
			this.Data.finalGoto.Notify_PawnLost(victim);
		}

		// Token: 0x06005D78 RID: 23928 RVA: 0x00223D98 File Offset: 0x00221F98
		public void SetParticipants(IEnumerable<PsychicRitualParticipant> participants)
		{
			PsychicRitualParticipant[] array = (participants as PsychicRitualParticipant[]) ?? participants.ToArray<PsychicRitualParticipant>();
			this.Data.participants = new List<PsychicRitualParticipant>(array);
			this.Data.finalGoto = new JobSyncTracker(from participant in array
			select participant.pawn, null, "final", null, JobCondition.Succeeded);
		}

		// Token: 0x1700105A RID: 4186
		// (get) Token: 0x06005D79 RID: 23929 RVA: 0x00223E04 File Offset: 0x00222004
		public virtual IEnumerable<PsychicRitualParticipant> Participants
		{
			get
			{
				return this.Data.participants;
			}
		}

		// Token: 0x04003510 RID: 13584
		private const string FinalSyncTag = "final";
	}
}
