using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CD4 RID: 3284
	public class LordToil_EnterShuttleOrLeave : LordToil
	{
		// Token: 0x1700103B RID: 4155
		// (get) Token: 0x06005CF9 RID: 23801 RVA: 0x00221CED File Offset: 0x0021FEED
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005CFA RID: 23802 RVA: 0x00221CF0 File Offset: 0x0021FEF0
		public LordToil_EnterShuttleOrLeave(Thing shuttle, LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false, bool interruptCurrentJob = false)
		{
			this.shuttle = shuttle;
			this.locomotion = locomotion;
			this.canDig = canDig;
			this.interruptCurrentJob = interruptCurrentJob;
		}

		// Token: 0x06005CFB RID: 23803 RVA: 0x00221D15 File Offset: 0x0021FF15
		private DutyDef GetExpectedDutyDef(Pawn pawn)
		{
			if (!this.shuttle.Spawned || !pawn.CanReach(this.shuttle, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return DutyDefOf.ExitMapBestAndDefendSelf;
			}
			return DutyDefOf.EnterTransporterAndDefendSelf;
		}

		// Token: 0x06005CFC RID: 23804 RVA: 0x00221D48 File Offset: 0x0021FF48
		private void EnsureCorrectDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				DutyDef expectedDutyDef = this.GetExpectedDutyDef(pawn);
				if (pawn.mindState != null && (pawn.mindState.duty == null || pawn.mindState.duty.def != expectedDutyDef))
				{
					pawn.mindState.duty = new PawnDuty(expectedDutyDef, this.shuttle, -1f);
					pawn.mindState.duty.locomotion = this.locomotion;
					pawn.mindState.duty.canDig = this.canDig;
					if (this.interruptCurrentJob && pawn.jobs != null && pawn.jobs.curJob != null)
					{
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
		}

		// Token: 0x06005CFD RID: 23805 RVA: 0x00221E34 File Offset: 0x00220034
		public override void UpdateAllDuties()
		{
			this.EnsureCorrectDuties();
		}

		// Token: 0x06005CFE RID: 23806 RVA: 0x00221E3C File Offset: 0x0022003C
		public override void LordToilTick()
		{
			this.EnsureCorrectDuties();
		}

		// Token: 0x040034EF RID: 13551
		public Thing shuttle;

		// Token: 0x040034F0 RID: 13552
		public LocomotionUrgency locomotion;

		// Token: 0x040034F1 RID: 13553
		public bool canDig;

		// Token: 0x040034F2 RID: 13554
		public bool interruptCurrentJob;
	}
}
