using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000948 RID: 2376
	public class LordToil_ExitMap : LordToil
	{
		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x0600490D RID: 18701 RVA: 0x001C916E File Offset: 0x001C736E
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x0600490E RID: 18702 RVA: 0x001C9171 File Offset: 0x001C7371
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x0600490F RID: 18703 RVA: 0x001C9174 File Offset: 0x001C7374
		public virtual DutyDef ExitDuty
		{
			get
			{
				return DutyDefOf.ExitMapBest;
			}
		}

		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x06004910 RID: 18704 RVA: 0x001C917B File Offset: 0x001C737B
		protected LordToilData_ExitMap Data
		{
			get
			{
				return (LordToilData_ExitMap)this.data;
			}
		}

		// Token: 0x06004911 RID: 18705 RVA: 0x001C9188 File Offset: 0x001C7388
		public LordToil_ExitMap(LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false, bool interruptCurrentJob = false)
		{
			this.data = new LordToilData_ExitMap();
			this.Data.locomotion = locomotion;
			this.Data.canDig = canDig;
			this.Data.interruptCurrentJob = interruptCurrentJob;
		}

		// Token: 0x06004912 RID: 18706 RVA: 0x001C91C0 File Offset: 0x001C73C0
		public override void UpdateAllDuties()
		{
			LordToilData_ExitMap data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(this.ExitDuty);
				pawnDuty.locomotion = data.locomotion;
				pawnDuty.canDig = data.canDig;
				Pawn pawn = this.lord.ownedPawns[i];
				pawn.mindState.duty = pawnDuty;
				if (this.Data.interruptCurrentJob && pawn.jobs.curJob != null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}
	}
}
